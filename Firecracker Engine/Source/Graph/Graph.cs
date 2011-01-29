using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Firecracker_Engine {

	public class Graph {

		public List<Vertex> m_vertices;
		public List<Edge> m_edges;

		public Graph() {
			m_vertices = new List<Vertex>();
			m_edges = new List<Edge>();
		}

		public bool addVertex(Vertex v) {
			if(v != null && !containsVertex(v)) {
				m_vertices.Add(v);
				return true;
			}
			return false;
		}

		public bool addEdge(Edge e) {
			if(e != null && !containsEdge(e)) {
				m_edges.Add(e);

				int indexA = indexOfVertex(e.a);

				if(indexA >= 0) {
					e.a = m_vertices[indexA];
				}
				else {
					m_vertices.Add(e.a);
				}

				int indexB = indexOfVertex(e.b);

				if(indexB >= 0) {
					e.b = m_vertices[indexB];
				}
				else {
					m_vertices.Add(e.b);
				}

				return true;
			}
			return false;
		}

		public int size() {
			return m_edges.Count();
		}

		public int numberOfVertices() {
			return m_vertices.Count();
		}

		public int numberOfEdges() {
			return m_edges.Count();
		}

		public Vertex vertexAt(int index) {
			if(index < 0 || index >= m_vertices.Count()) { return null; }
			return m_vertices[index];
		}

		public Edge edgeAt(int index) {
			if(index < 0 || index >= m_edges.Count()) { return null; }
			return m_edges[index];
		}

		public bool containsVertex(Vertex v) {
			if(v == null) { return false; }

			for(int i=0;i<m_vertices.Count();i++) {
				if(m_vertices[i].Equals(v)) {
					return true;
				}
			}
			return false;
		}

		public bool containsEdge(Edge e) {
			if(e == null) { return false; }

			for(int i=0;i<m_edges.Count();i++) {
				if(m_edges[i].Equals(e)) {
					return true;
				}
			}
			return false;
		}

		public int indexOfVertex(Vertex v) {
			if(v == null) { return -1; }

			for(int i=0;i<m_vertices.Count();i++) {
				if(m_vertices[i].Equals(v)) {
					return i;
				}
			}
			return -1;
		}

		public int indexOfEdge(Edge e) {
			if(e == null) { return -1; }

			for(int i=0;i<m_edges.Count();i++) {
				if(m_edges[i].Equals(e)) {
					return i;
				}
			}
			return -1;
		}

		public bool removeVertex(Vertex v) {
			int vertexIndex = indexOfVertex(v);
			if(vertexIndex >= 0) {
				m_vertices.RemoveAt(vertexIndex);
				for(int i=0;i<m_edges.Count();i++) {
					if(m_edges[i].a.Equals(v) || m_edges[i].b.Equals(v)) {
						m_edges.RemoveAt(i);
						i--;
					}
				}
				return true;
			}
			return false;
		}

		public bool removeEdge(Edge e) {
			int edgeIndex = indexOfEdge(e);
			if(edgeIndex >= 0) {
				m_edges.RemoveAt(edgeIndex);
				return true;
			}
			return false;
		}

		public static Graph parseFrom(StreamReader input, int numberOfEdges) {
			if(input == null || numberOfEdges <= 0) { return null; }

			Graph g = new Graph();

			String data;
			for(int i=0;i<numberOfEdges;i++) {
				data = input.ReadLine();
				if(input == null) { return null; }
				data = data.Trim();

				g.addEdge(Edge.parseFrom(data));
			}

			return g;
		}

		public static Graph parseFrom(StreamReader input) {
			if(input == null) { return null; }

			Graph g = new Graph();

			String data;
			while((data = input.ReadLine()) != null) {
				data = data.Trim();
				if(data.Length == 0) { break; }

				g.addEdge(Edge.parseFrom(data));
			}

			return g;
		}

		public void writeTo(StreamWriter output) {
			if(output == null) { return; }

			for(int i=0;i<m_edges.Count();i++) {
				output.Write('\t');
				m_edges[i].writeTo(output);
				output.WriteLine();
			}
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		public override bool Equals(object o) {
			Graph g = o as Graph;
			return g != null && m_edges.Equals(g.m_edges);
		}

		public override string ToString() {
			String s = "[";

			for(int i=0;i<m_edges.Count();i++) {
				s += m_edges[i];
				if(i < m_edges.Count() - 1) {
					s += ", ";
				}
			}

			s += "]";

			return s;
		}

	}

}
