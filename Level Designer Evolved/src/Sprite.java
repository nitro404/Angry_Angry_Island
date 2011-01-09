// ======================================= //
// Level Designer Evolved                  //
//                                         //
// Author: Kevin Scroggins                 //
// E-Mail: nitro404@hotmail.com            //
// ======================================= //

import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import javax.imageio.ImageIO;

/**
 * The Sprite class represents a 2D image which can be drawn onto the screen.
 * 
 * @author Kevin Scroggins
 */
public class Sprite {
	
	// local variables
	private File imageFile;
	private BufferedImage image;
	private String name;
	private String parentName;
	private int index;
	private int type;
	
	/** Unknown Sprite type constant. */
	public static int TYPE_UNKNOWN = -1;
	/** SpriteSheet image Sprite type constant. */
	public static int TYPE_SHEET = 0;
	/** Tiled Sprite type constant. */
	public static int TYPE_TILE = 1;
	/** Object Sprite type constant. */
	public static int TYPE_OBJECT = 2;
	
	
	/**
	 * Constructs a new Sprite from a specified file and directory.
	 * 
	 * @param fileName the sprite image file name.
	 * @param spriteDirectory the directory to read the file from.
	 */
	public Sprite(String fileName, String spriteDirectory) {
		this(fileName, spriteDirectory, -1);
	}
	
	/**
	 * Constructs a Sprite from a specified file and directory and assigns a specified index to it.
	 * 
	 * @param fileName the sprite image file name.
	 * @param spriteDirectory the directory to read the file from.
	 * @param index the index of the sprite within its parent SpriteSheet.
	 */
	public Sprite(String fileName, String spriteDirectory, int index) {
		this.name = null;
		this.parentName = null;
		this.index = index;
		this.type = TYPE_UNKNOWN;
		
		// generate the file path
		String imagePath = spriteDirectory;
		if(imagePath.charAt(imagePath.length() - 1) != '\\' && imagePath.charAt(imagePath.length() - 1) != '/') {
			imagePath += '\\';
		}
		imagePath += fileName;
		
		// create a file pointing to the image
		this.imageFile = new File(imagePath);
		
		// validate the file
		if(!this.imageFile.exists() || !this.imageFile.isFile()) {
			System.out.println("ERROR: Sprite: \"" + this.imageFile.getPath() + "\" does not exist or is an invalid image.");
			System.exit(1);
		}
		
		// read the image and store it
		try {
			this.image = ImageIO.read(this.imageFile);
		}
		catch(IOException e) {
			System.out.println("ERROR: Unable to read sprite image: \"" + this.imageFile.getPath() + "\".");
			System.exit(1);
		}
	}
	
	/**
	 * Constructs a Sprite from a BufferedImage.
	 * 
	 * @param image the image to assign to the Sprite.
	 */
	public Sprite(BufferedImage image) {
		this(image, -1);
	}
	
	/**
	 * Constructs a Sprite from a BufferedImage and assigns a specific index to it.
	 * 
	 * @param image the image to assign to the Sprite.
	 * @param index the index of the Sprite within its parent SpriteSheet.
	 */
	public Sprite(BufferedImage image, int index) {
		this.name = null;
		this.parentName = null;
		this.index = index;
		this.type = TYPE_UNKNOWN;
		
		if(image == null) {
			System.out.println("ERROR: Sprite cannot be created from null image.");
			System.exit(1);
		}
		
		this.image = image;
	}
	
	/**
	 * Constructs a Sprite from a sub-image of an image and assigns a specific index to it.
	 * 
	 * @param image the image to assign to the Sprite.
	 * @param index the index of the Sprite within its parent SpriteSheet.
	 * @param x the x offset of the sub-image.
	 * @param y the y offset of the sub-image.
	 * @param width the width of the sub-image.
	 * @param height the height of the sub-image.
	 */
	public Sprite(BufferedImage image, int index, int x, int y, int width, int height) {
		this.name = null;
		this.parentName = null;
		this.index = index;
		this.type = TYPE_UNKNOWN;
		
		if(image == null) {
			System.out.println("ERROR: Sprite cannot be created from null image.");
			System.exit(1);
		}
		
		this.image = image.getSubimage(x, y, width, height);
	}
	
	/**
	 * Returns the width of the Sprite.
	 * 
	 * @return the width of the Sprite.
	 */
	public int getWidth() {
		return this.image.getWidth();
	}
	
	/**
	 * Returns the height of the Sprite.
	 * 
	 * @return the height of the Sprite.
	 */
	public int getHeight() {
		return this.image.getHeight();
	}
	
	/**
	 * Returns the size of the Sprite.
	 * 
	 * @return the size of the Sprite.
	 */
	public Dimension getDimensions() {
		return new Dimension(this.image.getWidth(), this.image.getHeight());
	}
	
	/**
	 * Checks to see if the Sprite is tiled or not.
	 * 
	 * @return true if the Sprite is tiled.
	 */
	public boolean isTiled() {
		return this.type == TYPE_TILE;
	}
	
	/**
	 * Returns the name of the Sprite.
	 * 
	 * @return the name of the Sprite.
	 */
	public String getName() {
		return this.name;
	}
	
	/**
	 * Returns the name of the SpriteSheet the Sprite belongs to.
	 * 
	 * @return the name of the SpriteSheet the Sprite belongs to.
	 */
	public String getParentName() {
		return this.parentName;
	}
	
	/**
	 * Returns the index of the Sprite within the SpriteSheet it belongs to.
	 * 
	 * @return the index of the Sprite within the SpriteSheet it belongs to.
	 */
	public int getIndex() {
		return this.index;
	}
	
	/**
	 * Returns the Sprite type.
	 * 
	 * @return the Sprite type.
	 */
	public int getType() {
		return this.type;
	}
	
	/**
	 * Changes the name assigned to the current Sprite.
	 * 
	 * @param name the name to assign to the Sprite.
	 */
	public void setName(String name) {
		this.name = name;
	}
	
	/**
	 * Changes the parent name assigned to the current Sprite.
	 * 
	 * @param parentName the parent name to assign to the Sprite.
	 */
	public void setParentName(String parentName) {
		this.parentName = parentName;
	}
	
	/**
	 * Changes the indexed location of the Sprite within it's parent SpriteSheet.
	 * 
	 * @param index the location of the Sprite within its parent SpriteSheet.
	 */
	public void setIndex(int index) {
		this.index = index;
	}
	
	/**
	 * Changes the sprite type.
	 * 
	 * @param type the type to change the Sprite to.
	 */
	public void setType(int type) {
		if(type == TYPE_SHEET ||
		   type == TYPE_TILE ||
		   type == TYPE_OBJECT) {
			this.type = type;
		}
		else {
			this.type = TYPE_UNKNOWN;
		}
	}
	
	/**
	 * Get the image stored inside of the current Sprite.
	 * 
	 * @return the image representing the current Sprite. 
	 */
	public BufferedImage getImage() {
		return this.image;
	}
	
	/**
	 * Converts a String type into an integer representing a constant value of the same Sprite type.
	 * 
	 * @param data the String representation of the Sprite type.
	 * @return the integer representation of the Sprite Type.
	 */
	public static int parseType(String data) {
		if(data == null || data.trim().length() == 0) { return TYPE_UNKNOWN; }
		String typeString = data.trim();
		
		if(typeString.equalsIgnoreCase("SpriteSheet")) {
			return TYPE_SHEET;
		}
		else if(typeString.equalsIgnoreCase("Tile")) {
			return TYPE_TILE;
		}
		else if(typeString.equalsIgnoreCase("Object")) {
			return TYPE_OBJECT;
		}
		else {
			return TYPE_UNKNOWN;
		}
	}
	
	/**
	 * Renders the Sprite onto the specified Graphics component.
	 * 
	 * @param g the Graphics component to render to.
	 * @param x the x coordinate to draw the Sprite at.
	 * @param y the y coordinate to draw the Sprite at.
	 */
	public void paintOn(Graphics g, int x, int y) {
		((Graphics2D) g).drawImage(image, null, x, y);
	}
	
	/**
	 * Renders the Sprite onto the specified Graphics component, centred around the specified position.
	 * 
	 * @param g the Graphics component to render to.
	 * @param x the x coordinate to draw the Sprite at.
	 * @param y the y coordinate to draw the Sprite at.
	 */
	public void paintOnCentered(Graphics g, int x, int y) {
		((Graphics2D) g).drawImage(image, null, (int) (x - (image.getWidth() / 2.0)), (int) (y - (image.getHeight() / 2.0)));
	}
	
}
