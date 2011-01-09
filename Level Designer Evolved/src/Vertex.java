// ======================================= //
// Level Designer Evolved                  //
//                                         //
// Author: Kevin Scroggins                 //
// E-Mail: nitro404@hotmail.com            //
// ======================================= //

import java.io.*;
import java.awt.*;

/**
 * The Vertex class represents a location with an integer x and y coordinate.
 * 
 * @author Kevin Scroggins
 */
public class Vertex {
	
	/** The x coordinate of the Vertex. */
	public int x;
	
	/** The y coordinate of the Vertex. */
	public int y;
	
	/**
	 * Constructs a Vertex based on the specified coordinates.
	 * 
	 * @param x the initial x coordinate of the Vertex.
	 * @param y the initial y coordinate of the Vertex.
	 */
	public Vertex(int x, int y) {
		this.x = x;
		this.y = y;
	}
	
	/**
	 * Constructs a Vertex using the x and y values of the specified Point.
	 * 
	 * @param p a point to initialise the Vertex with.
	 */
	public Vertex(Point p) {
		this.x = p.x;
		this.y = p.y;
	}
	
	/**
	 * Returns the x coordinate of the Vertex.
	 * 
	 * @return the x coordinate of the Vertex.
	 */
	public int getX() {
		return this.x;
	}
	
	/**
	 * Returns the y coordinate of the Vertex.
	 * 
	 * @return the y coordinate of the Vertex.
	 */
	public int getY() {
		return this.y;
	}
	
	/**
	 * Sets the location of the Vertex to the specified coordinates.
	 * 
	 * @param x the new x coordinate of the Vertex.
	 * @param y the new y coordinate of the Vertex.
	 */
	public void setLocation(int x, int y) {
		this.x = x;
		this.y = y;
	}
	
	/**
	 * Creates a Point object from the coordinates stored in the current Vertex and returns it.
	 * 
	 * @return a Point representation of the Vertex.
	 */
	public Point toPoint() {
		return new Point(x, y);
	}
	
	/**
	 * Creates a Vertex object from a specified String and returns it.
	 * 
	 * Parses the Vertex from a String of the form:
	 * "x, y" where
	 * x and y are the corresponding coordinates of the Vertex.
	 * 
	 * @param input the String to parse the Vertex from.
	 * @return the Vertex parsed from the String.
	 */
	public static Vertex parseFrom(String input) {
		if(input == null || input.trim().length() == 0) {
			return null;
		}
		
		String data = input.trim();
		
		// extract the values of each coordinate and store them
		int x = Integer.valueOf(data.substring(0, data.indexOf(',')).trim());
		int y = Integer.valueOf(data.substring(data.lastIndexOf(',', data.length() - 1) + 1, data.length()).trim());
		
		return new Vertex(x, y);
	}
	
	/**
	 * Writes the Vertex to the specified PrintWriter.
	 * 
	 * Outputs the Vertex to the form:
	 * "x, y" where
	 * x and y are the corresponding coordinates of the current Vertex.
	 * 
	 * @param out the PrintWriter to write the Vertex to.
	 * @throws IOException if there was an error writing to the output stream.
	 */
	public void writeTo(PrintWriter out) throws IOException {
		out.print(x + ", " + y);
	}
	
	/**
	 * Renders the Vertex onto the specified Graphics object.
	 * 
	 * @param g the Graphics object to render the Vertex onto.
	 */
	public void paintOn(Graphics g) {
		int radius = 2;
		g.fillOval(x - radius, 
				   y - radius,
				   radius * 2, radius * 2);
	}
	
	/* (non-Javadoc)
	 * @see java.lang.Object#equals(java.lang.Object)
	 */
	public boolean equals(Object o) {
		if(o == null || !(o instanceof Vertex)) {
			return false;
		}
		
		Vertex p = (Vertex) o;
		
		// check to see that the x and y coordinates of each Vertex match
		return this.x == p.x && this.y == p.y;
	}
	
	/* (non-Javadoc)
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		// return a String representation of the Vertex in the form (x, y)
		return "(" + this.x + ", " + this.y + ")";
	}
}
