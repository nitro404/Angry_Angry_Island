// ======================================= //
// Level Designer Evolved                  //
//                                         //
// Author: Kevin Scroggins                 //
// E-Mail: nitro404@hotmail.com            //
// ======================================= //

import java.awt.Graphics;
import java.io.*;
import java.util.StringTokenizer;

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
	public void paintOn(Graphics g) {
		// if the Sprite is tiled, align it to the grid and then render it
		if(sprite.isTiled()) {
			sprite.paintOn(g, location.x * Level.GRID_SIZE, location.y * Level.GRID_SIZE);
		}
		// otherwise, simply render it at it's corresponding location
		else {
			sprite.paintOn(g, location.x, location.y);
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
	public static Entity parseFrom(String input, SpriteSheets spriteSheets) {
		if(input == null || input.trim().length() == 0) {
			return null;
		}
		
		String data = input.trim();
		
		// create a StringTokenizer to parse through the data String based on the delimiter as a comma
		// if less than the x and y coordinates or more than the x and y coordinates and the Sprite name / parent SpriteSheet name have been specified, then do not parse the Entity
		StringTokenizer st = new StringTokenizer(data, ",", false);
		if(st.countTokens() < 2 || st.countTokens() > 4) { return null; }
		
		// parse the Vertex (location) of the Entity from the String
		Vertex v = new Vertex(Integer.valueOf(st.nextToken().trim()), Integer.valueOf(st.nextToken().trim()));
		
		// parse the Sprite name and the parent SpriteSheet name from the String
		Entity newEntity = null;
		SpriteSheet spriteSheet;
		Sprite sprite;
		if(st.countTokens() == 2) {
			String spriteSheetName = st.nextToken().trim();
			String spriteName = st.nextToken().trim();
			
			// get the SpriteSheet based on its name
			spriteSheet = spriteSheets.getSpriteSheet(spriteSheetName);
			if(spriteSheet == null) {
				System.out.println("ERROR: Unable to load entity sprite \"" + spriteName + "\" from sprite sheet \"" + spriteSheetName + "\".");
				return null;
			}
			
			// get the Sprite from the corresponding SpriteSheet based on its name
			sprite = spriteSheet.getSprite(spriteName);
			if(sprite == null) {
				System.out.println("ERROR: Unable to load entity sprite \"" + spriteName + "\" from sprite sheet \"" + spriteSheetName + "\".");
				return null;
			}
			
			// create the entity and initialise its index within the parent SpriteSheet
			newEntity = new Entity(v, sprite);
			newEntity.spriteSheetIndex = spriteSheets.getSpriteSheetIndex(spriteSheetName);
		}
		else {
			newEntity = new Entity(v, null);
		}
		return newEntity;
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
		this.location.writeTo(out);
		if(sprite != null) {
			out.print(", " + sprite.getParentName() + ", " + sprite.getName());
		}
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
