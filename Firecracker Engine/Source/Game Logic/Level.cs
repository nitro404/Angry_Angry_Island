using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine {

	public enum LevelType { Cartesian, Isometric }

	public class Level {

		private LevelType m_type;
		private int m_gridSize;
		private Point m_dimensions;
		private List<GameObject> m_objects;
		private Graph m_collisionData;

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
			m_objects = new List<GameObject>();
			m_collisionData = new Graph();
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

		public Graph collisionData {
			get { return m_collisionData; }
			set { if(value != null) { m_collisionData = value; } }
		}

		public int numberOfObjects() {
			return m_objects.Count();
		}

		public bool addObject(GameObject o) {
			if(o == null || containsObject(o)) { return false; }
			m_objects.Add(o);
			return true;
		}

		public GameObject objectAt(int index) {
			if(index < 0 || index >= m_objects.Count()) { return null; }
			return m_objects[index];
		}

		public bool containsObject(GameObject o) {
			if(o == null) { return false; }

			for(int i=0;i<m_objects.Count();i++) {
				if(m_objects[i].Equals(o)) {
					return true;
				}
			}
			return false;
		}

		public int indexOfObject(GameObject o) {
			if(o == null) { return -1; }

			for(int i=0;i<m_objects.Count();i++) {
				if(m_objects[i].Equals(o)) {
					return i;
				}
			}
			return -1;
		}

		public bool removeObject(int index) {
			if(index < 0 || index >= m_objects.Count()) { return false; }

			m_objects.RemoveAt(index);
			return true;
		}

		public bool removeObject(GameObject o) {
			if(o == null) { return false; }

			return m_objects.Remove(o);
		}

		public Vector2 getScreenPosition(Vector2 gamePosition) {
			return getScreenPosition(gamePosition, m_type, m_gridSize);
		}

		public Vector2 getScreenPosition(Vertex gamePosition) {
			return getScreenPosition(gamePosition.toVector(), m_type, m_gridSize);
		}

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

		public Vector2 getGamePosition(Vertex screenPosition) {
			return getGamePosition(screenPosition.toVector(), m_type, gridSize);
		}

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

		public static Level readFrom(String fileName) {
			if(fileName == null || fileName.Length == 0) { return null; }

			Level level;
			LevelType type = DEFAULT_LEVEL_TYPE;
			int gridSize = DEFAULT_GRID_SIZE;
			Point dimensions = Point.Zero;
			int numberOfCollisionEdges = 0;
			int numberOfObjects = 0;

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

			level.collisionData = Graph.parseFrom(input, numberOfCollisionEdges);

			// read objects header and number of objects
			while((data = input.ReadLine()) != null) {
				String objectsHeader = data.Trim();
				if(objectsHeader.Length == 0) { continue; }

				// separate header data
				String[] headerData = objectsHeader.Split(':');
				if(headerData.Length != 2) {
					return null;
				}

				// verify the objects header
				if(!headerData[0].Trim().Equals("Objects", StringComparison.OrdinalIgnoreCase)) { return null; }

				// parse the number of objects
				try {
					numberOfObjects = Int32.Parse(headerData[1].Trim());
				}
				catch(Exception) { return null; }

				break;
			}

			// load the objects
			int currentObject = 0;
			while(currentObject < numberOfObjects) {
				data = input.ReadLine();
				String objectHeader = data.Trim();
				if(objectHeader.Length == 0) { continue; }

				// parse object type
				String objectType;
				if(objectHeader[objectHeader.Length - 1] == ':') {
					objectType = objectHeader.Substring(0, objectHeader.Length - 1);
				}
				else { return null; }

				// parse the object based on its type
				GameObject newObject = null;
				if(objectType.Equals("Static Object", StringComparison.OrdinalIgnoreCase)) {
					newObject = StaticObject.parseFrom(input, Firecracker.spriteSheets);
				}
				else if(objectType.Equals("Game Tile", StringComparison.OrdinalIgnoreCase)) {
					newObject = GameTile.parseFrom(input, Firecracker.spriteSheets);
				}
                else if (objectType.Equals("NPCObject", StringComparison.OrdinalIgnoreCase))
                {
                    newObject = NPCObject.parseFrom(input, Firecracker.spriteSheets);
                }
                else if (objectType.Equals("Player", StringComparison.OrdinalIgnoreCase))
                {
                    newObject = new Player();
                }

				// verify that the object was successfully parsed
				if(newObject != null) {
					currentObject++;
					level.addObject(newObject);
				}
				else {
					return null;
				}
			}

			input.Close();

			return level;
		}

		public void update(GameTime gameTime) {
			for(int i=0;i<m_objects.Count();i++) {
				if(m_objects[i].toBeDeleted) {
					m_objects.RemoveAt(i);
					i--;
				}
			}

			for(int i=0;i<m_objects.Count();i++) {
				m_objects[i].update(gameTime);
			}
		}

		public void draw(SpriteBatch spriteBatch) {
			for(int i=0;i<m_objects.Count();i++) {
				m_objects[i].draw(spriteBatch);
			}
		}

	}

}
