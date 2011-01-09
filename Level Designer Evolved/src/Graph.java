// ======================================= //
// Level Designer Evolved                  //
//                                         //
// Author: Kevin Scroggins                 //
// E-Mail: nitro404@hotmail.com            //
// ======================================= //

import java.util.Vector;
import java.awt.Color;
import java.awt.Graphics;
import java.io.*;

/**
 * The Graph class represents a collection of Edges and Vertices which form a Graph.
 * 
 * @author Kevin Scroggins
 */
public class Graph {
	
	/** The collection of Edges making up the Graph. */
	public Vector<Edge> edges;
	
	/** A collection of all the unique Vertices in the collection of Edges. */
	public Vector<Vertex> vertices;
	
	/**
	 * Constructs an empty Graph object.
	 */
	public Graph() {
		// initialise the Vertex and Edge collections
		this.edges = new Vector<Edge>();
		this.vertices = new Vector<Vertex>();
	}
	
	/**
	 * Adds the specified Vertex to the collection of vertices if it is not already contained in the Graph.
	 * 
	 * @param v the Vertex to be added to the Graph.
	 */
	public void addVertex(Vertex v) {
		// if the Vertex is not null and not already contained in the Graph, then add it to the Graph
		if(v != null && !this.vertices.contains(v)) {
			this.vertices.add(v);
		}
	}
	
	/**
	 * Adds the specified Edge to the collection of Edges if it is not already contained in the Graph.
	 * 
	 * Also adds both Vertices to the Graph if they are not already contained in the collection of Vertices.
	 * 
	 * If either of the Edge's Vertices are already in the Graph, the Edge is set to reference the Vertex so there are no duplicate Vertices.
	 * 
	 * @param e the Edge to be added to the Graph.
	 */
	public void addEdge(Edge e) {
		// if the Edge is not null and not contained in the Graph, then add it to the Graph
		if(e != null && !this.edges.contains(e) && e.a != null && e.b != null) {
			this.edges.add(e);
		
			// check to see if the Vertex is already contained in the list of vertices
			int indexA = this.vertices.indexOf(e.a);
			
			// if the Vertex already exists, set the Edge to reference this Vertex
			if(indexA >= 0) {
				e.a = this.vertices.elementAt(indexA);
			}
			// otherwise add the Vertex to the collection of Vertices
			else {
				this.vertices.add(e.a);
			}
			
			// check to see if the Vertex is already contained in the list of vertices
			int indexB = this.vertices.indexOf(e.b);
			
			// if the Vertex already exists, set the Edge to reference this Vertex
			if(indexB >= 0) {
				e.b = this.vertices.elementAt(indexB);
			}
			// otherwise add the Vertex to the collection of Vertices
			else {
				this.vertices.add(e.b);
			}
		}
	}

	/**
	 * Returns the number of Edges in the Graph.
	 * 
	 * @return the number of Edges in the Graph.
	 */
	public int size() {
		return this.edges.size();
	}
	
	/**
	 * Check to see if the Graph contains the specified Vertex. 
	 * 
	 * @param v the Vertex to check.
	 * @return true if the specified Vertex is contained within the Graph.
	 */
	public boolean containsVertex(Vertex v) {
		if(v == null) {
			return false;
		}
		
		// loop through the collection of Vertices to see if any of them are equal to the specified Vertex
		for(int i=0;i<this.vertices.size();i++) {
			if(this.vertices.elementAt(i).equals(v)) {
				return true;
			}
		}
		return false;
	}
	
	/**
	 * Check to see if the Graph contains the specified Edge.
	 * 
	 * @param e the Edge to check.
	 * @return true if the specified Edge is contained within the Graph.
	 */
	public boolean containsEdge(Edge e) {
		if(e == null) {
			return false;
		}
		
		// loop through the collection of Edges to see if any of them are equal to the specified Edge
		for(int i=0;i<this.edges.size();i++) {
			if(this.edges.elementAt(i).equals(e)) {
				return true;
			}
		}
		return false;
	}
	
	/**
	 * Writes the Graph to the specified PrintWriter.
	 * 
	 * Outputs the Graph to the format:
	 * "	x1, y1; x2, y2" where
	 * x1 and y1 are the x and y coordinates of the first Vertex of the Edge and
	 * x2 and y2 are the x and y coordinates of the second Vertex of the Edge
	 * for each Edge contained within the Graph.
	 * 
	 * @param out the PrintWriter to write the Graph to.
	 * @throws IOException if there was an error writing to the output stream.
	 */
	public void writeTo(PrintWriter out) throws IOException {
		for(int i=0;i<this.edges.size();i++) {
			out.print("\t");
			this.edges.elementAt(i).writeTo(out);
			out.println();
		}
	}
	
	/**
	 * Renders the Graph onto the specified Graphics object.
	 * 
	 * @param g the Graphics component to render the Graph onto.
	 * @param edgeColour the colour to render each Edge of the Graph.
	 * @param vertexColour the colour to render each Vertex of the Graph. 
	 */
	public void paintOn(Graphics g, Color edgeColour, Color vertexColour) {
		// render the edges
		g.setColor(edgeColour);
		for(int i=0;i<this.edges.size();i++) {
			this.edges.elementAt(i).paintOn(g);
		}
		
		// render the vertices
		g.setColor(vertexColour);
		for(int i=0;i<this.vertices.size();i++) {
			this.vertices.elementAt(i).paintOn(g);
		}
	}
	
	/* (non-Javadoc)
	 * @see java.lang.Object#equals(java.lang.Object)
	 */
	public boolean equals(Object o) {
		if(o == null || !(o instanceof Graph)) {
			return false;
		}
		
		Graph g = (Graph) o;
		
		// check to see that the Edges of each Graph match
		return this.edges.equals(g.edges);
	}
	
	/* (non-Javadoc)
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		// return a String representation of the Graph in the form:
		// [(x1, y1) (x2, y2), (x3, y3) (x4, y4), ... (xn, yn)]
		String s = "[";
		for(int i=0;i<this.edges.size();i++) {
			s += this.edges.elementAt(i);
			if(i < this.edges.size() - 1) {
				s += ", ";
			}
		}
		s += "]";
		return s;
	}
}