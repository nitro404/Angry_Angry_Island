using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace Firecracker_Engine {

	public enum LevelType { Cartesian, Isometric }

	public class Level : CBaseObject {

#pragma warning disable 108
        public const string ClassName = "Level";
#pragma warning restore 108

		private LevelType m_type;
		private int m_gridSize;
		private Point m_dimensions;
		//private Graph m_collisionData;
        List<int[]> m_lCollisionData;

		public static float LEVEL_VERSION = 2.0f;
		public static LevelType DEFAULT_LEVEL_TYPE = LevelType.Cartesian;
		public static int DEFAULT_GRID_SIZE = 64;
		public static String CARTESIAN_TYPE = "2D Cartesian Level";
		public static String ISOMETRIC_TYPE = "2D Isometric Level";

		public Level() : this(LevelType.Cartesian, DEFAULT_GRID_SIZE, Point.Zero) { }

		public Level(LevelType type, int gridSize, Point dimensions) {
			m_type = type;
			m_gridSize = (gridSize < 1) ? DEFAULT_GRID_SIZE : gridSize;
			m_dimensions = dimensions;
			if(dimensions.X < 0) { dimensions.X = 0; }
			if(dimensions.Y < 0) { dimensions.Y = 0; }
			//m_collisionData = new Graph();
            m_lCollisionData = new List<int[]>();
		}

		public LevelType type {
			get { return m_type; }
			set { m_type = value; }
		}

		public int gridSize {
			get { return m_gridSize; }
			set { if(gridSize >= 1) { m_gridSize = value; } }
		}

		public Point dimensions {
			get { return m_dimensions; }
			set { if(value.X >= 0 && value.Y >= 0) { m_dimensions = value; } }
		}

		/*public Graph collisionData {
			get { return m_collisionData; }
			set { if(value != null) { m_collisionData = value; } }
		}*/

		public Vector2 getScreenPosition(Vector2 gamePosition) {
			return getScreenPosition(gamePosition, m_type, m_gridSize);
		}

		//public Vector2 getScreenPosition(Vertex gamePosition) {
		//	return getScreenPosition(gamePosition.toVector(), m_type, m_gridSize);
		//}

		public static Vector2 getScreenPosition(Vector2 gamePosition, LevelType type, int gridSize) {
			if (type == LevelType.Cartesian) {
				return gamePosition;
			}
			else if (type == LevelType.Isometric) {
				Vector2 screenPosition = Vector2.Zero;
				float isoCos = (float) Math.Cos(Math.PI / 4.0f);
				float isoSin = (float) Math.Sin(Math.PI / 4.0f);

				float x = gamePosition.X * ((gridSize / 2.0f) / 45.0f);
				float y = gamePosition.Y * (gridSize / 45.0f);

				screenPosition.X = (isoCos * x) + (isoSin * y);
				screenPosition.Y = ((-isoSin) * x) + (isoCos * y);

				return screenPosition;
			}
			return Vector2.Zero;
		}

		public Vector2 getGamePosition(Vector2 screenPosition) {
			return getGamePosition(screenPosition, m_type, gridSize);
		}

		//public Vector2 getGamePosition(Vertex screenPosition) {
		//	return getGamePosition(screenPosition.toVector(), m_type, gridSize);
		//}

		public static Vector2 getGamePosition(Vector2 screenPosition, LevelType type, int gridSize) {
			if (type == LevelType.Cartesian) {
				return screenPosition;
			}
			else if (type == LevelType.Isometric) {
				Vector2 gamePosition = Vector2.Zero;
				float isoCos = (float) Math.Cos(Math.PI / 4.0f);
				float isoSin = (float) Math.Sin(Math.PI / 4.0f);

				gamePosition.X = (isoCos * screenPosition.X) + ((-isoCos) * screenPosition.Y);
				gamePosition.X *= 45.0f / (gridSize / 2.0f);

				gamePosition.Y = (isoSin * screenPosition.Y) + (isoCos * screenPosition.Y);
				gamePosition.Y *= 45.0f / gridSize;

				return gamePosition;
			}
			return Vector2.Zero;
		}

        public override void LoadPropertiesList(ObjectDefinition objDef)
        {
            base.LoadPropertiesList(objDef);

            // Load all the info here
            if (objDef.ClassProperties.ContainsKey("Collision_Edges"))
            {
                m_lCollisionData = Helpers.ParseIntX4Array(objDef.ClassProperties["Collision_Edges"]);
            }
            if (objDef.ClassProperties.ContainsKey("Grid_Size"))
            {
                m_gridSize = int.Parse(objDef.ClassProperties["Grid_Size"]);
            }
            if (objDef.ClassProperties.ContainsKey("Dimensions"))
            {
                dimensions = Helpers.ParsePoint(objDef.ClassProperties["Dimensions"]);
            }
            if (objDef.ClassProperties.ContainsKey("Tiles"))
            {
                
            }
            if (objDef.ClassProperties.ContainsKey("GridLayout"))
            {
                m_type = (LevelType)Helpers.StringToEnum<LevelType>(objDef.ClassProperties["GridLayout"]);
            }
        }

		public static Level readFrom(String fileName) {
			if(fileName == null || fileName.Length == 0) { return null; }

			Level level;
			LevelType type = DEFAULT_LEVEL_TYPE;
			int gridSize = DEFAULT_GRID_SIZE;
			Point dimensions = Point.Zero;
			int numberOfCollisionEdges = 0;

			StreamReader input = null;
			try {
				input = File.OpenText(fileName);
			}
			catch(Exception) {
				return null;
			}

			String data;

			// read initial header
			while((data = input.ReadLine()) != null) {
				data = data.Trim();
				if(data.Length == 0) { continue; }

				// read and verify level type
				String[] versionHeader = data.Split(':');
				if(versionHeader.Length != 2) { return null; }

				String typeString = versionHeader[0] = versionHeader[0].Trim();
				if(typeString.Equals(CARTESIAN_TYPE, StringComparison.OrdinalIgnoreCase)) {
					type = LevelType.Cartesian;
				}
				else if(typeString.Equals(ISOMETRIC_TYPE, StringComparison.OrdinalIgnoreCase)) {
					type = LevelType.Isometric;
				}
				else {
					return null;
				}

				// read and verify level version
				String versionString = versionHeader[1].Trim();
				String[] versionData = versionString.Split(' ');
				if(versionData.Length != 2) { return null; }
				if(!versionData[0].Equals("Version", StringComparison.OrdinalIgnoreCase)) { return null; }
				try {
					float version = float.Parse(versionData[1]);

					if(version != LEVEL_VERSION) { return null; }
				}
				catch(Exception) { return null; }

				break;
			}

			// read grid size
			while((data = input.ReadLine()) != null) {
				data = data.Trim();
				if (data.Length == 0) { continue; }

				// read and store grid size
				String[] gridHeader = data.Split(':');
				if(gridHeader.Length != 2) { return null; }
				if(!gridHeader[0].Trim().Equals("Grid Size", StringComparison.OrdinalIgnoreCase)) { return null; }
				try {
					gridSize = Int32.Parse(gridHeader[1].Trim());
				}
				catch(Exception) { return null; }

				break;
			}

			// read map dimensions
			while((data = input.ReadLine()) != null) {
				data = data.Trim();
				if (data.Length == 0) { continue; }

				// read and store grid size
				String[] dimensionHeader = data.Split(':');
				if(dimensionHeader.Length != 2) { return null; }
				if(!dimensionHeader[0].Trim().Equals("Dimensions", StringComparison.OrdinalIgnoreCase)) { return null; }
				String[] dimensionData = dimensionHeader[1].Split(',');
				if(dimensionData.Length != 2) { return null; }
				try {
					dimensions.X = Int32.Parse(dimensionData[0].Trim());
					dimensions.Y = Int32.Parse(dimensionData[1].Trim());
				}
				catch(Exception) { return null; }

				break;
			}

			// read collision data header
			while((data = input.ReadLine()) != null) {
				data = data.Trim();
				if (data.Length == 0) { continue; }

				// verify header and parse number of collision edges
				String[] collisionHeader = data.Split(':');
				if(collisionHeader.Length != 2) { return null; }
				if(!collisionHeader[0].Trim().Equals("Collision Edges", StringComparison.OrdinalIgnoreCase)) { return null; }
				try {
					numberOfCollisionEdges = Int32.Parse(collisionHeader[1].Trim());
				}
				catch(Exception) { return null; }

				break;
			}

			level = new Level(type, gridSize, dimensions);

			//level.collisionData = Graph.parseFrom(input, numberOfCollisionEdges);

			// TODO: Read objects

			input.Close();

			return level;
		}

	}

}
