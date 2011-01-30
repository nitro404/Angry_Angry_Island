// ======================================= //
// Level Designer Evolved                  //
//                                         //
// Author: Kevin Scroggins                 //
// E-Mail: nitro404@hotmail.com            //
// ======================================= //

import java.awt.Graphics;
import java.io.*;

/**
 * 
 * 
 * @author Kevin Scroggins
 */
public class Entity {

	/**
	 * The location of the Entity within the Level.
	 */
	public Vertex location;
	
	/**
	 * The Sprite assigned to the current entity. 
	 */
	public Sprite sprite;
	
	/**
	 * The index of the Sprite within the originating SpriteSheet (for quick reference).
	 */
	public int spriteSheetIndex;
	
	public float layerDepth;
	
	/**
	 * Constructs an Entity and initialises it with the specified location and Sprite. 
	 * 
	 * @param location the Vertex to initialise the Entity with based on its location within the Level.
	 * @param sprite the Sprite to be assigned to the Entity which represents it visually.
	 */
	public Entity(Vertex location, Sprite sprite) {
		this.location = location;
		this.sprite = sprite;
		this.spriteSheetIndex = -1;
	}
	
	/**
	 * Returns the location of the Entity within the Level, represented as a Vertex.
	 * 
	 * @return the location of the Entity within the Level, represented as a Vertex.
	 */
	public Vertex getPosition() {
		return this.location;
	}
	
	/**
	 * Returns the index of the Sprite within its originating SpriteSheet.
	 * 
	 * @return the index of the Sprite within its originating SpriteSheet.
	 */
	public int getSpriteIndex() {
		return (this.sprite == null) ? -1 : this.sprite.getIndex();
	}
	
	/**
	 * Returns true if the Entity is tiled (aligned to the grid).
	 * 
	 * @return true if the Entity is tiled (aligned to the grid).
	 */
	public boolean isTiled() {
		return (this.sprite == null) ? false : this.sprite.isTiled();
	}
	
	/**
	 * Returns the Sprite which has been assigned to the Entity.
	 * 
	 * @return the Sprite which has been assigned to the Entity.
	 */
	public Sprite getSprite() {
		return this.sprite;
	}
	
	/**
	 * Returns the type of Sprite which has been assigned to the Entity.
	 * 
	 * @return the type of Sprite which has been assigned to the Entity.
	 */
	public int getType() {
		return (this.sprite == null) ? Sprite.TYPE_UNKNOWN : this.sprite.getType();
	}
	
	/**
	 * Returns the width of Sprite which has been assigned to the Entity.
	 * 
	 * @return the width of Sprite which has been assigned to the Entity.
	 */
	public int getWidth() {
		return (this.sprite == null) ? -1 : this.sprite.getWidth();
	}
	
	/**
	 * Returns the height of Sprite which has been assigned to the Entity.
	 * 
	 * @return the height of Sprite which has been assigned to the Entity.
	 */
	public int getHeight() {
		return (this.sprite == null) ? -1 : this.sprite.getHeight();
	}
	
	/**
	 * Renders the Entity's Sprite onto the screen at it's corresponding location.
	 * 
	 * @param g the Graphics object to render the Entity's Sprite onto.
	 */
	public void paintOn(Graphics g, int gridSize) {
		if(gridSize <= 0) { return; }
		
		if(sprite.isTiled()) {
			sprite.paintOn(g, location.x, location.y);
		}
		else {
			sprite.paintOn(g, location.x * gridSize, location.y * gridSize);
		}
	}
	
	/**
	 * Creates an Entity from the specified String and SpriteSheets collection and returns it.
	 * 
	 * Parses the Entity from a String in the form: "xPos, yPos, SpriteSheetName, SpriteName" where
	 *  xPos is the x coordinate of the Entity's location Vertex,
	 *  yPos is the y coordinate of the Entity's location Vertex,
	 *  SpriteSheetName is the name of the Sprite's originating SpriteSheet and
	 *  SpriteName is the name associated with the corresponding Sprite.
	 * 
	 * @param input the data string to be parsed into an Entity.
	 * @param spriteSheets the collection of SpriteSheets to initialise the Entity's corresponding Sprite with.
	 * @return the Entity parsed from the specified input parameters.
	 */
	public static Entity parseFrom(BufferedReader in, SpriteSheets spriteSheets) {
		if(in == null || spriteSheets == null) { return null; }

		Entity entity;
		VariableSystem properties = new VariableSystem();

		// store all of the animation properties
		String data;
		Variable property;
		do {
			try { data = in.readLine(); }
			catch(IOException e) { return null; }
			if(data == null) { return null; }

			data = data.trim();
			if(data.length() == 0) { continue; }

			property = Variable.parseFrom(data);
			if(property == null) { return null; }

			properties.add(property);
		} while(properties.size() < 4);
		
		// get the object's position
		String positionData = properties.getValue("Position");
		if(positionData == null) { return null; }
		
		// parse the sprite's position
		String[] positionValues = positionData.split(",");
		if(positionValues.length != 2) { return null; }

		Vertex newPosition = new Vertex();
		try {
			newPosition.x = Integer.parseInt(positionValues[0]);
			newPosition.y = Integer.parseInt(positionValues[1]);
		}
		catch(Exception e) { return null; }

        // get the layer depth of this sprite
        String layerDepthData = properties.getValue("LayerDepth");
        if(layerDepthData == null) { return null; }
        
        float layerDepth;
        try {
            layerDepth = Float.parseFloat(layerDepthData);
        }
        catch(Exception e) { return null; }

		// get the sprite's name
        String spriteName = properties.getValue("Sprite Name");
		if(spriteName == null) { return null; }

		// get the name of the spritesheet in which the sprite is found
		String spriteSheetName = properties.getValue("SpriteSheet Name");
		if(spriteSheetName == null) { return null; }
		
		// get the object's sprite
		SpriteSheet spriteSheet = spriteSheets.getSpriteSheet(spriteSheetName);
		if(spriteSheet == null) { return null; }
		Sprite sprite = spriteSheet.getSprite(spriteName);
		if(sprite == null) { return null; }

		// create the object
		entity = new Entity(newPosition, sprite);
		entity.layerDepth = layerDepth;

		return entity;
	}
	
	/**
	 * Writes the Entity's corresponding information to the specified PrintWriter.
	 * 
	 * Outputs to the form: "xPos, yPos, SpriteSheetName, SpriteName" where
	 *  xPos is the x coordinate of the Entity's location Vertex,
	 *  yPos is the y coordinate of the Entity's location Vertex,
	 *  SpriteSheetName is the name of the Sprite's originating SpriteSheet and
	 *  SpriteName is the name associated with the corresponding Sprite.
	 * 
	 * @param out the PrintWriter to write the Entity's corresponding information onto.
	 * @throws IOException if there was an error writing to the output stream.
	 */
	public void writeTo(PrintWriter out) throws IOException {
		out.println("\tStatic Object" + Variable.separatorChar);
		out.println("\t\tPosition" + Variable.separatorChar + " " + location.x + ", " + location.y);
		out.println("\t\tLayerDepth" + Variable.separatorChar + " " + layerDepth);
		out.println("\t\tSprite Name" + Variable.separatorChar + " " + sprite.getName());
		out.println("\t\tSpriteSheet Name" + Variable.separatorChar + " " + sprite.getParentName());
	}
	
	/* (non-Javadoc)
	 * @see java.lang.Object#equals(java.lang.Object)
	 */
	public boolean equals(Object o) {
		if(o == null || !(o instanceof Entity)) {
			return false;
		}
		
		Entity e = (Entity) o;
		
		if((this.sprite == null && e.sprite != null) ||
		   (this.sprite != null && e.sprite == null) ||
		   (this.sprite != null && e.sprite != null && !this.sprite.equals(e.sprite))) {
			return false;
		}
		
		return this.location.equals(e.location);
	}
	
	/* (non-Javadoc)
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return this.location.toString();
	}
	
}
