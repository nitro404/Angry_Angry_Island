using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Firecracker_Engine {

	public class Edge {

		private Vertex m_a;
		private Vertex m_b;

		public Edge(Vertex a, Vertex b) {
			this.a = a;
			this.b = b;
		}
		
		public Vertex a {
			get { return m_a; }
			set { if(value != null) { m_a = value; } }
		}

		public Vertex b {
			get { return m_b; }
			set { if (value != null) { m_b = value; } }
		}

		public float deltaX {
			get { return m_b.x - m_a.x; }
		}

		public float deltaY {
			get { return m_b.y - m_a.y; }
		}

		public float length {
			get { return (float) Math.Sqrt(Math.Pow(m_b.x - m_a.x, 2) + Math.Pow(m_b.y - m_a.y, 2)); }
		}

		public bool containsVertex(Vertex v) {
			return v != null && (m_a.Equals(v) || m_b.Equals(v));
		}

		public static Edge parseFrom(String input) {
			if(input == null) { return null; }

			String data = input.Trim();

			if(data.Length == 0) { return null; }

			string[] vertexData = data.Split(';');

			if(vertexData.Length != 2) { return null; }

			Vertex a = Vertex.parseFrom(vertexData[0]);
			Vertex b = Vertex.parseFrom(vertexData[1]);

			if(a == null || b == null) { return null; }

			return new Edge(a, b);
		}

		public void writeTo(StreamWriter output) {
			if(output == null) { return; }

			a.writeTo(output);
			output.Write("; ");
			b.writeTo(output);
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		public override bool Equals(object o) {
			Edge e = o as Edge;
			return e != null && m_a.Equals(e.m_a) && m_b.Equals(e.m_b);
		}

		public override string ToString() {
			return a + " " + b;
		}

	}

}
