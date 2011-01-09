// ======================================= //
// Level Designer Evolved                  //
//                                         //
// Author: Kevin Scroggins                 //
// E-Mail: nitro404@hotmail.com            //
// ======================================= //

import java.util.Vector;
import java.awt.*;
import java.io.*;

/**
 * The Level class represents an area in which the player can explore and interact with.
 * 
 * @author Kevin Scroggins
 */
public class Level {
	
	/** The size of the Grid in the current Level. */
	public Vertex gridSize;
	/** The dimensions of the current Level. */
	public Dimension dimensions;
	/** The collection dynamic collision boundaries associated with the Level. */
	private Graph collisionData;
	/** The collection of static Objects located in the Level. */
	private Vector<Entity> entities;
	/** The collection of tiles (grid-aligned Sprites) located in the Level. */
	private Vector<Entity> tiles;
	
	/** A String representing the type of Level that the Level Designer currently supports. */
	final public static String LEVEL_TYPE = "2D Level";
	/** The version of the Level that the Level Designer currently supports. */
	final public static double LEVEL_VERSION = 2.0;
	/** The size of the Grid the Level Designer supports by default. */
	final public static int GRID_SIZE = 64;
	
	/**
	 * Constructs an empty Level object.
	 */
	public Level() {
		// initialise the collections associated with the Level
		this.collisionData = new Graph();
		this.entities = new Vector<Entity>();
		this.tiles = new Vector<Entity>();
	}

	/**
	 * Adds a specified Vertex to the Level's collision boundary collection.
	 * 
	 * @param v the Vertex to be added.
	 */
	public void addVertex(Vertex v) {
		this.collisionData.addVertex(v);
	}
	
	/**
	 * Adds a specified Edge to the Level's collision boundary collection.
	 * 
	 * @param e the Edge to be added.
	 */
	public void addEdge(Edge e) {
		this.collisionData.addEdge(e);
	}
	
	/**
	 * Returns the number of Vertices contained within the Level's collision boundary collection.
	 * 
	 * @return the number of Vertices contained within the Level's collision boundary collection.
	 */
	public int numberOfVertices() {
		return this.collisionData.vertices.size();
	}
	
	/**
	 * Returns the number of Edges contained within the Level's collision boundary collection.
	 * 
	 * @return the number of Edges contained within the Level's collision boundary collection.
	 */
	public int numberOfEdges() {
		return this.collisionData.edges.size();
	}
	
	/**
	 * Checks if the specified Vertex is contained within the Level's collision boundary collection.
	 * 
	 * @param v the Vertex to match.
	 * @return true if the specified Vertex exists.
	 */
	public boolean containsVertex(Vertex v) {
		return this.collisionData.containsVertex(v);
	}
	
	/**
	 * Checks if the specified Edge is contained within the Level's collision boundary collection.
	 * 
	 * @param e the Edge to match.
	 * @return true if the specified Edge exists.
	 */
	public boolean containsEdge(Edge e) {
		return this.collisionData.containsEdge(e);
	}

	/**
	 * Returns the Vertex located at the specified index if the index is not out of bounds and there is at least one Vertex in the Level's boundary collision collection.
	 * 
	 * @param index the index of the Vertex to return.
	 * @return the Vertex located at the specified index if the index is not out of bounds and there is at least one Vertex in the Level's boundary collision collection.
	 */
	public Vertex getVertex(int index) {
		if(index < 0 || index >= this.collisionData.vertices.size()) { return null; }
		
		// return the Vertex located at the specified index
		return this.collisionData.vertices.elementAt(index);
	}

	/**
	 * Returns the Edge located at the specified index if the index is not out of bounds and there is at least one Edge in the Level's boundary collision collection.
	 * 
	 * @param index the index of the Edge to return.
	 * @return the Edge located at the specified index if the index is not out of bounds and there is at least one Edge in the Level's boundary collision collection.
	 */
	public Edge getEdge(int index) {
		if(index < 0 || index >= this.collisionData.edges.size()) { return null; }
		
		// return the Edge located at the specified index
		return this.collisionData.edges.elementAt(index);
	}
	
	/**
	 * Removes the specified Vertex from the Level's boundary collision collection.
	 * 
	 * @param v the Vertex to remove.
	 * @return true if the specified Vertex was successfully removed.
	 */
	public boolean removeVertex(Vertex v) {
		// remove the specified Vertex
		boolean removed = collisionData.vertices.remove(v);
		
		// collect all Edges connected to the specified vertex
		Vector<Edge> edgesToRemove = new Vector<Edge>();
		for(int i=0;i<collisionData.size();i++) {
			if(this.collisionData.edges.elementAt(i).a.equals(v) || this.collisionData.edges.elementAt(i).b.equals(v)) {
				edgesToRemove.add(this.collisionData.edges.elementAt(i));
			}
		}
		
		// remove all collected Edges
		for(int i=0;i<edgesToRemove.size();i++) {
			this.collisionData.edges.remove(edgesToRemove.elementAt(i));
		}
		return removed;
	}
	
	/**
	 * Returns the specified Edge from the Level's boundary collision collection.
	 * 
	 * @param e the Edge to be removed.
	 * @return true if the specified Edge was successfully removed.
	 */
	public boolean removeEdge(Edge e) {
		return collisionData.edges.remove(e);
	}
	
	/**
	 * Adds an Entity to the Level's collection of Entities if it does not already exist and is valid.
	 * 
	 * @param e the Entity to be added.
	 */
	public void addEntity(Entity e) {
		// verify that the Entity is valid
		if(e == null ||
		   e.getSprite() == null ||
		   e.isTiled()) { return; }
		
		// add the Entity if it is not a duplicate
		if(!this.entities.contains(e)) {
			this.entities.add(e);
		}
	}
	
	/**
	 * Adds an Tile to the Level's collection of Tiles if it does not already exist and is valid.
	 * 
	 * @param e the Tile to be added.
	 */
	public void addTile(Entity e) {
		// verify that the Tile is valid
		if(e == null ||
		   e.getSprite() == null ||
		   !e.isTiled()) { return; }
		
		// add the Tile if it is not a duplicate
		if(!this.tiles.contains(e)) {
			this.tiles.add(e);
		}
	}
	
	/**
	 * Returns the number of Entities contained within the Level.
	 * 
	 * @return the number of Entities contained within the Level.
	 */
	public int numberOfEntities() {
		return this.entities.size();
	}
	
	/**
	 * Returns the number of Tiles contained within the Level.
	 * 
	 * @return the number of Tiles contained within the Level.
	 */
	public int numberOfTiles() {
		return this.tiles.size();
	}
	
	/**
	 * Checks if the Entity is contained within the Level's collection of Entities.
	 * 
	 * @param e the Entity to be matched.
	 * @return true if the Entity exists.
	 */
	public boolean containsEntity(Entity e) {
		if(e == null) { return false; }
		
		return this.entities.contains(e);
	}
	
	/**
	 * Checks if the Tile is contained within the Level's collection of Tiles.
	 * 
	 * @param e the Tile to be matched.
	 * @return true if the Tile exists.
	 */
	public boolean containsTile(Entity e) {
		if(e == null) { return false; }
		
		return this.tiles.contains(e);
	}
	
	/**
	 * Removes the specified Entity from the Level's collection of Entities.
	 * 
	 * @param e the Entity to be removed.
	 * @return true of the Entity was successfully removed.
	 */
	public boolean removeEntity(Entity e) {
		if(e == null) { return false; }
		
		return this.entities.remove(e);
	}
	
	/**
	 * Removes the specified Tile from the Level's collection of Tiles.
	 * 
	 * @param e the Tile to be removed.
	 * @return true of the Tile was successfully removed.
	 */
	public boolean removeTile(Entity e) {
		if(e == null) { return false; }
		
		return this.tiles.remove(e);
	}
	
	/**
	 * Removes the Entity at the specified index from the Level's collection of Entities at the specified index.
	 * 
	 * @param index the index to remove an Entity from.
	 * @return true if an Entity was successfully removed at the specified index.
	 */
	public boolean removeEntity(int index) {
		if(index < 0 || index >= this.entities.size()) { return false; }
		
		return this.entities.remove(index) != null;
	}
	
	/**
	 * Removes the Tile at the specified index from the Level's collection of Tiles.
	 * 
	 * @param index the index to remove an Tile from.
	 * @return true if an Tile was successfully removed at the specified index.
	 */
	public boolean removeTile(int index) {
		if(index < 0 || index >= this.tiles.size()) { return false; }
		
		return this.tiles.remove(index) != null;
	}
	
	/**
	 * Returns the Entity at the specified index if the index is within range and the Level contains at least one Entity, otherwise returns null.
	 * 
	 * @param index the index of the Entity to return;
	 * @return the Entity at the specified index if the index is within range and the Level contains at least one Entity, otherwise returns null.
	 */
	public Entity getEntity(int index) {
		if(index < 0 || index >= this.entities.size()) { return null; }
		
		return this.entities.elementAt(index);
	}
	
	/**
	 * Returns the Tile at the specified index if the index is within range and the Level contains at least one Tile, otherwise returns null.
	 * 
	 * @param index the index of the Tile to return;
	 * @return the Tile at the specified index if the index is within range and the Level contains at least one Tile, otherwise returns null.
	 */
	public Entity getTile(int index) {
		if(index < 0 || index >= this.tiles.size()) { return null; }
		
		return this.tiles.elementAt(index);
	}
	
	/**
	 * Locates the current Entity within the Level (if it exists) and brings it to the front of the screen.
	 * 
	 * @param e the Entity to be swapped.
	 */
	public void bringSpriteToFront(Entity e) {
		if(e == null) { return; }
		
		if(this.entities.remove(e)) {
			this.entities.add(e);
		}
		else if(this.tiles.remove(e)) {
			this.tiles.add(e);
		}
	}
	
	/**
	 * Locates the current Entity within the Level (if it exists) and sends it to the back of the screen.
	 * 
	 * @param e the Entity to be swapped.
	 */
	public void sendSpriteToBack(Entity e) {
		if(e == null) { return; }
		
		if(this.entities.remove(e)) {
			this.entities.insertElementAt(e, 0);
		}
		else if(this.tiles.remove(e)) {
			this.tiles.insertElementAt(e, 0);
		}
	}
	
	/**
	 * Removes a specified Entity from the Level.
	 * 
	 * @param e the entity to be removed.
	 */
	public void deleteSprite(Entity e) {
		this.removeEntity(e);
		this.removeTile(e);
	}
	
	/**
	 * Parses a Level from the specified file and instantiates all of the Entities using their corresponding Sprites found in the collection of SpriteSheets.
	 * 
	 * @param fileName the Level file to load.
	 * @param spriteSheets the SpriteSheet collection to instantiate the Entity Sprites with.
	 * @return the Level parsed from the specified input parameters.
	 */
	public static Level parseFrom(String fileName, SpriteSheets spriteSheets) {
		if(fileName == null || fileName.trim().length() == 0) {
			return null;
		}
		try {
			// open the file and read the Level from the input stream
			BufferedReader in = new BufferedReader(new FileReader(fileName.trim()));
			Level level = readFrom(in, spriteSheets);
			if(in != null) {
				in.close();
			}
			return level;
		}
		catch(FileNotFoundException e) {
			System.out.println("ERROR: Unable to open file: \"" + fileName + "\"");
			e.printStackTrace();
		}
		catch(IOException e) {
			System.out.println("ERROR: " + e.getMessage());
			e.printStackTrace();
		}
		catch(Exception e) {
			System.out.println("ERROR: Corrupted map file.");
			e.printStackTrace();
		}
		return null;
	}
	
	/**
	 * Parses a Level from the specified input stream and instantiates all of the Entities using their corresponding Sprites found in the collection of SpriteSheets.
	 * 
	 * @param in the input stream to parse the Level from.
	 * @param spriteSheets the SpriteSheet collection to instantiate the Entity Sprites with.
	 * @return the Level parsed from the specified input parameters.
	 * @throws IOException if there was an error reading the Level from the specified input stream.
	 */
	public static Level readFrom(BufferedReader in, SpriteSheets spriteSheets) throws IOException {
		if(in == null) {
			return null;
		}
		
		String input;
		Level level = new Level();
		
		// input the Level header and verify the type and version of the map
		input = in.readLine();
		String levelType = input.substring(0, input.indexOf(':', 0)).trim();
		if(!LEVEL_TYPE.equalsIgnoreCase(levelType)) {
			System.out.println("ERROR: Incompatible level type (" + levelType + "). Current editor only supports levels of type " + LEVEL_TYPE + ".");
			return null;
		}
		double levelVersion = Double.valueOf(input.substring(input.lastIndexOf(' ', input.length() - 1), input.length()).trim());
		if(LEVEL_VERSION != levelVersion) {
			System.out.println("ERROR: Incompatible map version (" + levelVersion + "). Current editor only supports version " + LEVEL_VERSION + ".");
			return null;
		}
		
		// read in the grid size and verify it
		input = in.readLine();
		String gridSizeHeader = input.substring(0, input.indexOf(':', 0)).trim();
		if(!gridSizeHeader.equalsIgnoreCase("Grid Size")) {
			System.out.println("ERROR: Corrupted level file. Expected header \"Grid Size\", found \"" + gridSizeHeader + "\".");
			return null;
		}
		int fileGridSize = Integer.valueOf(input.substring(input.indexOf(':', 0) + 1, input.length()).trim());
		if(fileGridSize != GRID_SIZE) {
			System.out.println("ERROR: Incompatible grid size: " + fileGridSize + ". The editor only supports a grid size of " + GRID_SIZE + ".");
			return null;
		}
		
		// read in the map dimensions
		input = in.readLine();
		String dimensionsHeader = input.substring(0, input.indexOf(':', 0)).trim();
		if(!dimensionsHeader.equalsIgnoreCase("Dimensions")) {
			System.out.println("ERROR: Corrupted level file. Expected header \"Dimensions\", found \"" + dimensionsHeader + "\".");
			return null;
		}
		int mapWidth = Integer.valueOf(input.substring(input.indexOf(':', 0) + 1, input.indexOf(',', 0)).trim());
		int mapHeight = Integer.valueOf(input.substring(input.indexOf(',', 0) + 1, input.length()).trim());
		level.gridSize = new Vertex(mapWidth, mapHeight);
		level.dimensions = new Dimension((mapWidth * GRID_SIZE) + 1, (mapHeight * GRID_SIZE) + 1);
		
		// read in the collision data
		input = in.readLine();
		String edgesHeader = input.substring(0, input.indexOf(':', 0)).trim();
		if(!edgesHeader.equalsIgnoreCase("Edges")) {
			System.out.println("ERROR: Corrupted level file. Expected header \"Edges\", found \"" + edgesHeader + "\".");
			return null;
		}
		int numberOfEdges = Integer.valueOf(input.substring(input.lastIndexOf(':', input.length() - 1) + 1, input.length()).trim());
		for(int j=0;j<numberOfEdges;j++) {
			input = in.readLine().trim();
			level.addEdge(Edge.parseFrom(input));
		}
		
		// read in the entities
		input = in.readLine();
		String entitiesHeader = input.substring(0, input.indexOf(':', 0)).trim();
		if(!entitiesHeader.equalsIgnoreCase("Entities")) {
			System.out.println("ERROR: Corrupted level file. Expected header \"Entites\", found \"" + entitiesHeader + "\".");
			return null;
		}
		int numberOfEntities = Integer.valueOf(input.substring(input.lastIndexOf(':', input.length() - 1) + 1, input.length()).trim());
		for(int i=0;i<numberOfEntities;i++) {
			input = in.readLine().trim();
			level.addEntity(Entity.parseFrom(input, spriteSheets));
		}
		
		// read in the tiles
		input = in.readLine();
		String tilesHeader = input.substring(0, input.indexOf(':', 0)).trim();
		if(!tilesHeader.equalsIgnoreCase("Tiles")) {
			System.out.println("ERROR: Corrupted level file. Expected header \"Tiles\", found \"" + tilesHeader + "\".");
			return null;
		}
		int numberOfTiles = Integer.valueOf(input.substring(input.lastIndexOf(':', input.length() - 1) + 1, input.length()).trim());
		for(int i=0;i<numberOfTiles;i++) {
			input = in.readLine().trim();
			level.addTile(Entity.parseFrom(input, spriteSheets));
		}
		
		return level;
	}
	
	/**
	 * Outputs the current Level to the specified file.
	 * 
	 * @param fileName the file to output the Level to.
	 */
	public void writeTo(String fileName) {
		if(fileName == null || fileName.trim().length() == 0) {
			return;
		}
		try {
			// open the output stream and write the Level to it
			PrintWriter out = new PrintWriter(new FileWriter(fileName));
			this.writeTo(out);
			if(out != null) {
				out.close();
			}
		}
		catch(FileNotFoundException e) {
			System.out.println("ERROR: Unable to open file: \"" + fileName + "\"");
			e.printStackTrace();
		}
		catch(IOException e) {
			System.out.println("ERROR: " + e.getMessage());
			e.printStackTrace();
		}
		catch(Exception e) {
			System.out.println("ERROR: Corrupted map file.");
			e.printStackTrace();
		}
	}
	
	/**
	 * Outputs the Level to the specified output stream.
	 * 
	 * @param out the output stream to Write the Level to.
	 * @throws IOException if there was an error writing to the output file.
	 */
	public void writeTo(PrintWriter out) throws IOException {
		// write the level header and version
		out.println(LEVEL_TYPE + ": Version " + LEVEL_VERSION);
		
		// write grid size
		out.println("Grid Size: " + GRID_SIZE);
		
		// write the dimensions
		out.print("Dimensions: ");
		gridSize.writeTo(out);
		out.println();
		
		// write the edges
		out.println("Edges: " + this.collisionData.size());
		this.collisionData.writeTo(out);
		
		// write the entities
		out.println("Entities: " + this.entities.size());
		for(int i=0;i<this.entities.size();i++) {
			out.print("\t");
			this.entities.elementAt(i).writeTo(out);
			out.println();
		}
		
		// write the tiles
		out.println("Tiles: " + this.tiles.size());
		for(int i=0;i<this.tiles.size();i++) {
			out.print("\t");
			this.tiles.elementAt(i).writeTo(out);
			out.println();
		}
	}

	/**
	 * Renders the Level onto the specified Graphics component.
	 * Currently only renders the collision boundaries and not the Entities contained within the Level.
	 * 
	 * @param g the Graphics component to render onto.
	 * @param lineColour the colour to render the Edges of the Graph.
	 * @param vertexColour the colour to render the Vertices of each Edge.
	 */
	public void paintOn(Graphics g, Color lineColour, Color vertexColour) {
		collisionData.paintOn(g, lineColour, vertexColour);
	}
	
	/* (non-Javadoc)
	 * @see java.lang.Object#equals(java.lang.Object)
	 */
	public boolean equals(Object o) {
		if(o == null || !(o instanceof Level)) {
			return false;
		}
		
		Level level = (Level) o;
		
		// compare the entity collections
		if(this.entities.size() != level.entities.size()) {
			return false;
		}
		for(int i=0;i<this.entities.size();i++) {
			if(!level.entities.contains(this.entities.elementAt(i))) {
				return false;
			}
		}
		
		// compare the tile collections
		if(this.tiles.size() != level.tiles.size()) {
			return false;
		}
		for(int i=0;i<this.tiles.size();i++) {
			if(!level.tiles.contains(this.tiles.elementAt(i))) {
				return false;
			}
		}
		
		// compare the collision data
		return this.collisionData.equals(level.collisionData);
	}
	
	/* (non-Javadoc)
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return "Level";
	}
	
}
