using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace Firecracker_Engine {

	public class Vertex {

		public int x;
		public int y;

		public Vertex(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public Vertex(Vector2 v) {
			this.x = (int) v.X;
			this.y = (int) v.Y;
		}

		public void setLocation(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public Vector2 toVector() {
			return new Vector2(x, y);
		}

		public static Vertex parseFrom(String input) {
			if(input == null) { return null; }

			String data = input.Trim();

			if(data.Length == 0) { return null; }

			String[] vertexData = data.Split(',');

			if(vertexData.Length != 2) { return null; }

			int x, y;
			try {
				x = Int32.Parse(vertexData[0].Trim());
				y = Int32.Parse(vertexData[1].Trim());
				//x = Int32.Parse(data.Substring(0, data.IndexOf(',')).Trim());
				//y = Int32.Parse(data.Substring(data.LastIndexOf(',', data.Length - 1) + 1, data.Length).Trim());
			}
			catch(Exception) { return null; }

			return new Vertex(x, y);
		}

		public void writeTo(StreamWriter output) {
			if(output == null) { return; }

			output.Write(x + ", " + y);
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		public override bool Equals(object o) {
			Vertex v = o as Vertex;
			return v != null && x == v.x && y == v.y;
		}

		public override string ToString() {
			return "(" + x + ", " + y + ")";
		}

	}

}
