// ======================================= //
// Level Designer Evolved                  //
//                                         //
// Author: Kevin Scroggins                 //
// E-Mail: nitro404@hotmail.com            //
// ======================================= //

import java.awt.Dimension;
import java.awt.Point;
import java.awt.Rectangle;
import java.io.BufferedReader;
import java.io.File;
import java.io.IOException;
import java.util.Vector;

/**
 * The SpriteSheet class represents a collection of Sprites (images) which are part of a larger SpriteSheet (image).
 * The SpritrSheet class has support for parsing sprite sheets from a specific set of attributes
 * as well as reading a list of sprite sheet data from a file, then parsing and creating a corresponding sprite sheet.
 *
 * There are 3 types of sprite sheets:
 * 1) Arbitrary Offsets - A collection of offsets (x, y) and sizes (width, height) which each represent a sub-image (Sprite) within a larger image (SpriteSheet).
 *                        Can be instantiated using a constructor or parsed from a file using an array of Rectangles. Name and type can also be specified
 *                        for each individual Sprite if using the file parser in the order they are to be parsed.
 *
 * 2) Single Grid - Only requires a single offset (x, y) and a single size (width, height) as well as a specification indicating the number of rows and columns, and
 *                  the direction to parse (vertical or horizontal). Name and type can also be specified for each Sprite in the order they are to be parsed.
 *                  The constructor will automatically loop through, incrementing as appropriate and creating each Sprite from the SpriteSheet.
 *
 * 3) Multiple Grids - This specification currently only works from the data file parser and does not have a constructor available.
 *                     Requires the same specifications as a single grid for each grid it is to parse within a single image - these specifications include
 *                     offset of the current grid (x, y), sizes of each item in the current grid (width, height), number of rows and columns in the current grid
 *                     and the direction to parse the current grid (vertical or horizontal).
 * 
 * @author Kevin Scroggins
 */
public class SpriteSheet {
	
	// local variables
	private Vector<Sprite> sprites;
	private String name;
	
	/** Invalid SpriteSheet type constant. */
	final public static int TYPE_INVALID = -1;
	/** Arbitrary Offset SpriteSheet type constant. */
	final public static int TYPE_ARBITRARY_OFFSETS = 0;
	/** Single Grid SpriteSheet type constant. */
	final public static int TYPE_SINGLE_GRID = 1;
	/** Multiple Grid SpriteSheet type constant. */
	final public static int TYPE_MULTIPLE_GRIDS = 2;
	
	final public static int TYPE_SINGLE = 3;
	
	/**
	 * Constructs an empty SpriteSheet.
	 */
	public SpriteSheet() {
		this.sprites = new Vector<Sprite>();
		this.name = null;
	}
	
	/**
	 * Constructs and initialises a SpriteSheet using a specified SpriteSheet image and a collection of offsets.
	 * 
	 * @param sprite the SpriteSheet image to parse into a collection of Sprites.
	 * @param offsets the offsets specifying how to parse the SpriteSheet image.
	 */
	public SpriteSheet(Sprite sprite,
					   Vector<Rectangle> offsets) {
		this.name = null;
		sprites = new Vector<Sprite>((offsets.size() <= 1) ? 10 : offsets.size());
		
		if(sprite == null || sprite.getImage() == null) {
			System.out.println("ERROR: SpriteSheet cannot be parsed from null image.");
			System.exit(1);
		}
		
		// loop through the collection of offsets and parse up the sprite sheet image
		// according to each offset and store each sprite that is parsed
		Rectangle o = null;
		for(int i=0;i<offsets.size();i++) {
			o = offsets.elementAt(i);
			if(o != null) {
				sprites.add(new Sprite(sprite.getImage().getSubimage(o.x, o.y, o.width, o.height), i));
			}
		}
	}

	/**
	 * Constructs and initialises a SpriteSheet using a specified SpriteSheet image and a collection of offsets.
	 * 
	 * @param sprite the SpriteSheet image to parse into a collection of Sprites.
	 * @param offsets the offsets specifying how to parse the SpriteSheet image.
	 */
	public SpriteSheet(Sprite sprite,
					   Rectangle[] offsets) {
		this.name = null;
		sprites = new Vector<Sprite>((offsets.length <= 1) ? 10 : offsets.length);
		
		if(sprite == null || sprite.getImage() == null) {
			System.out.println("ERROR: SpriteSheet cannot be parsed from null image.");
			System.exit(1);
		}
		
		// loop through the collection of offsets and parse up the sprite sheet image
		// according to each offset and store each sprite that is parsed
		Rectangle o = null;
		for(int i=0;i<offsets.length;i++) {
			o = offsets[i];
			if(o != null) {
				sprites.add(new Sprite(sprite.getImage().getSubimage(o.x, o.y, o.width, o.height), i));
			}
		}
	}
	
	/**
	 * Constructs and initialises a SpriteSheet using a grid specification.
	 * 
	 * @param sprite the SpriteSheet image to parse into a collection of Sprites.
	 * @param xOffset the x offset of the grid.
	 * @param yOffset the y offset of the grid.
	 * @param width the width of each cell in the grid.
	 * @param height the height of each cell in the grid.
	 * @param xIncrement the amount to increment by to get to the next column.
	 * @param yIncrement the amount to increment by to get to the next row.
	 * @param horizontal the parsing direction.
	 * @param numberOfRows the number of rows to parse.
	 * @param numberOfColumns the number of columns to parse.
	 */
	public SpriteSheet(Sprite sprite,
					   int xOffset,
					   int yOffset,
					   int width,
					   int height,
					   int xIncrement,
					   int yIncrement,
					   boolean horizontal,
					   int numberOfRows,
					   int numberOfColumns) {
		this.name = null;
		int size = numberOfRows * numberOfColumns;
		sprites = new Vector<Sprite>((size <= 1) ? 10 : size);
		
		if(sprite == null || sprite.getImage() == null) {
			System.out.println("ERROR: SpriteSheet cannot be parsed from null image.");
			System.exit(1);
		}
		
		// start parsing at the initial offset
		// loop through all of the rows and columns incrementing the x position and y position as appropriate
		// parse each sprite according to the current offset and size and store it
		int xPos = xOffset;
		int yPos = yOffset;
		int k = 0;
		for(int i=0;i<numberOfRows;i++) {
			for(int j=0;j<numberOfColumns;j++) {
				sprites.add(new Sprite(sprite.getImage().getSubimage(xPos, yPos, width, height), k++));
				if(horizontal) { xPos += xIncrement; }
				else { yPos += yIncrement; }
			}
			if(horizontal) { yPos += yIncrement; xPos = xOffset; }
			else { xPos += xIncrement; yPos = yOffset; }
		}
	}
	
	/**
	 * Returns the number of Sprites in the current SpriteSheet.
	 * 
	 * @return the number of Sprites in the current SpriteSheet.
	 */
	int size() {
		return this.sprites.size();
	}
	
	/**
	 * Returns the Sprite located at the specified index.
	 * 
	 * @param index the location to obtain a Sprite from.
	 * @return the Sprite located at the specified index.
	 */
	Sprite elementAt(int index) {
		return this.getSprite(index);
	}
	
	/**
	 * Returns the Sprite located at the specified index.
	 * 
	 * @param index the location to obtain a Sprite from.
	 * @return the Sprite located at the specified index.
	 */
	Sprite getSprite(int index) {
		if(index < 0 || index >= sprites.size()) { return null; }
		return sprites.elementAt(index);
	}
	
	/**
	 * Returns the Sprite with a matching name located within the SpriteSheet.
	 * 
	 * @param name the name to match.
	 * @return the Sprite with a matching name located within the SpriteSheet.
	 */
	Sprite getSprite(String name) {
		if(name == null || name.trim().length() == 0) { return null; }
		String temp = name.trim();
		
		// loop through the collection of sprites until a Sprite with a matching name is found, then return it
		for(int i=0;i<sprites.size();i++) {
			if(this.sprites.elementAt(i).getName() != null &&
			   this.sprites.elementAt(i).getName().equalsIgnoreCase(temp)) {
				return this.sprites.elementAt(i);
			}
		}
		return null;
	}

	/**
	 * Returns a sub-collection of Sprites from the specified range within the current SpriteSheet.
	 * 
	 * @param startIndex the index to start collecting Sprites at.
	 * @param endIndex the index to collect Sprites to.
	 * @return the sub-collection of Sprites from the specified range within the current SpriteSheet.
	 */
	Vector<Sprite> getSprites(int startIndex, int endIndex) {
		if(startIndex < 0 || endIndex >= sprites.size()) { return null; }
		
		// create a collection of Sprites and add the Sprites within the specified range to the collection, then return it
		Vector<Sprite> spriteGroup = new Vector<Sprite>(endIndex - startIndex + 1);
		for(int i=startIndex;i<=endIndex;i++) {
			spriteGroup.add(sprites.elementAt(i));
		}
		return spriteGroup;
	}
	
	/**
	 * Returns the name of the current SpriteSheet.
	 * 
	 * @return the name of the current SpriteSheet.
	 */
	public String getName() {
		return this.name;
	}
	
	/**
	 * Changes the name of the current SpriteSheet.
	 * 
	 * @param name the name to assign to the current SpriteSheet.
	 */
	public void setName(String name) {
		this.name = name;
	}
	
	/**
	 * Parse the SpriteSheet type from a String and return the corresponding integer representation.
	 * 
	 * @param data the String representation of the SpriteSheet type.
	 * @return the integer representation of the SpriteSheet type.
	 */
	public static int parseType(String data) {
		if(data == null || data.trim().length() == 0) { return TYPE_INVALID; }
		String temp = data.trim();
		if(temp.equalsIgnoreCase("Arbitrary Offsets")) {
			return TYPE_ARBITRARY_OFFSETS;
		}
		else if(temp.equalsIgnoreCase("Single Grid")) {
			return TYPE_SINGLE_GRID;
		}
		else if(temp.equalsIgnoreCase("Multiple Grids")) {
			return TYPE_MULTIPLE_GRIDS;
		}
		return TYPE_INVALID;
		
	}
	
	/**
	 * Parses a SpriteSheet from an input stream and returns it.
	 * 
	 * @param in the input stream to parse a SpriteSheet from.
	 * @param spriteDirectory the directory containing the SpriteSheet image files.
	 * @return the SpriteSheet parsed from the input stream.
	 */
	public static SpriteSheet parseFrom(BufferedReader in, String spriteDirectory) {
		if(in == null || spriteDirectory == null) {
			System.out.println("ERROR: Cannot parse sprite sheet from null parameter(s).");
			return null;
		}
		
		String input;
		String data;
		Variable v;
		VariableSystem properties = new VariableSystem();
		String spriteSheetName, spriteSheetFileName;
		File spriteSheetFile;
		Sprite spriteSheetImage;
		int spriteSheetType;
		SpriteSheet spriteSheet;
		try {
			while((input = in.readLine()) != null) {
				data = input.trim();
				
				// if the line is blank, discard it
				if(data.length() == 0) {
					continue;
				}
				// otherwise if it contains data
				else {
					// attempt to parse a variable from the data and store it if successful
					v = Variable.parseFrom(input);
					
					if(v != null) {
						properties.add(v);
					}
					
					// determine if the variable indicates that the attributes for a set of Sprites or collection of grids is about to begin
					if(v != null && (v.getID().equalsIgnoreCase("Attributes") ||
					   v.getID().equalsIgnoreCase("Grids") ||
					   v.getID().equalsIgnoreCase("Number of Sprites"))) {
						// get the SpriteSheet name
						spriteSheetName = properties.getValue("SpriteSheet Name").replace(",", "");
						if(spriteSheetName == null) {
							System.out.println("ERROR: Sprite sheet must have a name.");
							return null;
						}
						
						// parse the SpriteSheet type
						try { spriteSheetType = SpriteSheet.parseType(properties.getValue("SpriteSheet Type")); }
						catch(NumberFormatException e) {
							System.out.println("ERROR: Invalid sprite sheet type.");
							return null;
						}
						if(spriteSheetType == TYPE_INVALID) {
							System.out.println("ERROR: Invalid sprite sheet type.");
							return null;
						}
						
						// get the SpriteSheet image file name, then load the SpriteSheet image
						spriteSheetFileName = properties.getValue("File Name");
						if(spriteSheetFileName == null) {
							System.out.println("ERROR: Invalid or missing sprite sheet image file name.");
							return null;
						} 
						spriteSheetFile = new File(spriteDirectory +
												   ((spriteSheetFileName.charAt(spriteSheetFileName.length() - 1) != '\\' &&
												     spriteSheetFileName.charAt(spriteSheetFileName.length() - 1) != '/') ? "\\" : "") +
												   spriteSheetFileName);
						if(!spriteSheetFile.exists() || !spriteSheetFile.isFile()) {
							System.out.println("ERROR: Invalid or missing sprite sheet image file name.");
							return null;
						}
						spriteSheetImage = new Sprite(spriteSheetFileName, spriteDirectory);
						spriteSheetImage.setType(Sprite.TYPE_SHEET);
						
						// Arbitrary Offset SpriteSheet Type ======================================
						if(spriteSheetType == TYPE_ARBITRARY_OFFSETS) {
							VariableSystem spriteAttributes; 
							int numberOfSprites;
							
							// verify that the number of Sprite offsets is valid
							try { numberOfSprites = Integer.parseInt(v.getValue()); }
							catch(NumberFormatException e) {
								System.out.println("ERROR: Invalid number of sprites.");
								return null;
							}
							if(numberOfSprites <= 0) {
								System.out.println("ERROR: Must parse at least 1 sprite from a sprite sheet.");
								return null;
							}
							
							int spriteIndex;
							String spriteName, spriteType;
							Point spriteOffset;
							Dimension spriteSize;
							spriteAttributes = new VariableSystem();
							Rectangle offsets[] = new Rectangle[numberOfSprites];
							String spriteNames[] = new String[numberOfSprites];
							int spriteTypes[] = new int[numberOfSprites];
							for(int i=0;i<numberOfSprites;i++) {
								spriteNames[i] = null;
								spriteTypes[i] = Sprite.TYPE_UNKNOWN;
								offsets[i] = new Rectangle(0, 0, 0, 0);
							}
							// loop through and collect the attributes for each Sprite
							for(int i=0;i<numberOfSprites;i++) {
								// get the 5 Sprite attributes (index, name, type, offset, size)
								spriteAttributes.clear();
								for(int j=0;j<5;j++) {
									spriteAttributes.add(Variable.parseFrom(in.readLine()));
								}
								
								// store the Sprite index and validate it
								try { spriteIndex = Integer.parseInt(spriteAttributes.getValue("Sprite")); }
								catch(NumberFormatException e) {
									System.out.println("ERROR: Invalid sprite attribute index.");
									return null;
								}
								if(spriteIndex < 0 || spriteIndex >= numberOfSprites) {
									System.out.println("ERROR: Sprite attribute index out of range.");
									return null;
								} 
								
								// get the Sprite name
								spriteName = spriteAttributes.getValue("Name").replace(",", "");
								if(spriteName == null) {
									System.out.println("ERROR: Sprite must have a name.");
									return null;
								}
								
								// get the Sprite type, then parse the type to an integer
								spriteType = spriteAttributes.getValue("Type");
								if(spriteType == null) {
									System.out.println("ERROR: Sprite must have a type.");
									return null;
								}
								
								// get the offset for the Sprite
								spriteOffset = Utilities.parsePoint(spriteAttributes.getValue("Pixel Offset"));
								if(spriteOffset == null) {
									System.out.println("ERROR: Invalid or missing sprite offset.");
									return null;
								}
								
								// get the size for the Sprite
								spriteSize = Utilities.parseDimension(spriteAttributes.getValue("Size"));
								if(spriteSize == null) {
									System.out.println("ERROR: Invalid or missing sprite tile size.");
									return null;
								}
								
								// assign the values to temporary storage
								spriteNames[spriteIndex] = spriteName;
								spriteTypes[spriteIndex] = Sprite.parseType(spriteType);
								offsets[spriteIndex] = new Rectangle(spriteOffset.x, spriteOffset.y, spriteSize.width, spriteSize.height);
							}
							
							// once the attributes and offsets for all of the Sprites have been collected, create the SpriteSheet
							spriteSheet = new SpriteSheet(spriteSheetImage, offsets);
							spriteSheet.setName(spriteSheetName);
							
							// loop through the temporarily stored Sprite attributes and assign them to each corresponding Sprite
							for(int i=0;i<numberOfSprites;i++) {
								spriteSheet.getSprite(i).setName(spriteNames[i]);
								spriteSheet.getSprite(i).setIndex(i);
								spriteSheet.getSprite(i).setParentName(spriteSheetName);
								spriteSheet.getSprite(i).setType(spriteTypes[i]);
							}
						}
						// Single Grid SpriteSheet Type ===========================================
						else if(spriteSheetType == TYPE_SINGLE_GRID) {
							VariableSystem spriteAttributes;
							Point offset;
							Dimension size, increment;
							boolean horizontal;
							int numberOfRows, numberOfColumns, numberOfAttributes;
							
							// get the number of Sprite attributes to read
							try { numberOfAttributes = Integer.parseInt(v.getValue()); }
							catch(NumberFormatException e) {
								System.out.println("ERROR: Invalid number of sprite attributes.");
								return null;
							}
							
							// get the grid offset
							offset = Utilities.parsePoint(properties.getValue("Pixel Offset"));
							if(offset == null) {
								System.out.println("ERROR: Invalid or missing sprite sheet offset.");
								return null;
							}
							
							// get the cell size
							size = Utilities.parseDimension(properties.getValue("Size"));
							if(size == null) {
								System.out.println("ERROR: Invalid or missing sprite sheet tile size.");
								return null;
							}
							
							// get the increment values
							increment = Utilities.parseDimension(properties.getValue("Increment"));
							if(increment == null) {
								System.out.println("ERROR: Invalid or missing sprite sheet tile increment.");
								return null;
							}
							
							// get the parsing direction
							String temp = properties.getValue("Horizontal");
							if(temp == null) {
								System.out.println("ERROR: Missing horizontal / vertical sprite sheet specification.");
								return null;
							}
							horizontal = temp.equalsIgnoreCase("true");
							
							// get the number of rows in the grid
							try { numberOfRows = Integer.parseInt(properties.getValue("Number of Rows")); }
							catch(NumberFormatException e) {
								System.out.println("ERROR: Missing number of rows in sprite sheet.");
								return null;
							}
							
							// get the number of columns in the grid
							try { numberOfColumns = Integer.parseInt(properties.getValue("Number of Columns")); }
							catch(NumberFormatException e) {
								System.out.println("ERROR: Missing number of columns in sprite sheet.");
								return null;
							}
							
							// create the SpriteSheet based on the corresponding specifications
							spriteSheet = new SpriteSheet(spriteSheetImage,
														  offset.x,
														  offset.y,
														  size.width,
														  size.height,
														  increment.width,
														  increment.height,
														  horizontal,
														  numberOfRows,
														  numberOfColumns);
							spriteSheet.setName(spriteSheetName);
							
							int spriteIndex;
							String spriteName, spriteType;
							spriteAttributes = new VariableSystem();
							// loop through the SpriteSheet specifications
							for(int i=0;i<numberOfAttributes;i++) {
								// obtain the 3 specifications for the current Sprite (index, name, type)
								for(int j=0;j<3;j++) {
									spriteAttributes.add(Variable.parseFrom(in.readLine()));
								}
								
								// get the Sprite index and validate it
								try { spriteIndex = Integer.parseInt(spriteAttributes.getValue("Sprite")); }
								catch(NumberFormatException e) {
									System.out.println("ERROR: Invalid sprite attribute index.");
									return null;
								}
								if(spriteIndex < 0 || spriteIndex >= spriteSheet.size()) {
									System.out.println("ERROR: Sprite attribute index out of range.");
									return null;
								}
								
								// get the Sprite name
								spriteName = spriteAttributes.getValue("Name").replace(",", "");;
								
								// get the Sprite type and parse it to an integer
								spriteType = spriteAttributes.getValue("Type");
								if(spriteName == null || spriteType == null) {
									System.out.println("ERROR: Sprite name or type missing.");
									return null;
								}
								
								// assign the attributes to the corresponding Sprite
								spriteSheet.sprites.elementAt(spriteIndex).setName(spriteName);
								spriteSheet.sprites.elementAt(spriteIndex).setIndex(spriteIndex);
								spriteSheet.sprites.elementAt(spriteIndex).setParentName(spriteSheetName);
								spriteSheet.sprites.elementAt(spriteIndex).setType(Sprite.parseType(spriteType));
								
								// clear the attributes of the current Sprite
								spriteAttributes.clear();
							}
						}
						// Multiple Grids SpriteSheet Type ========================================
						else if(spriteSheetType == TYPE_MULTIPLE_GRIDS) {
							int numberOfGrids;
							int spriteIndexOffset = 0;
							
							// verify that the number of grids to parse is specified and validate it
							try { numberOfGrids = Integer.parseInt(v.getValue()); }
							catch(NumberFormatException e) {
								System.out.println("ERROR: Invalid number of grids.");
								return null;
							}
							if(numberOfGrids < 1) {
								System.out.println("ERROR: Must parse at least 1 grid.");
								return null;
							}
							
							// create an empty SpriteSheet
							spriteSheet = new SpriteSheet();
							spriteSheet.setName(spriteSheetName);
							
							// temporary storage
							VariableSystem gridAttributes = new VariableSystem();
							int numberOfAttributes[] = new int[numberOfGrids];
							Point offset[] = new Point[numberOfGrids];
							Dimension size[] = new Dimension[numberOfGrids];
							Dimension increment[] = new Dimension[numberOfGrids];
							boolean horizontal[] = new boolean[numberOfGrids];
							int numberOfRows[] = new int[numberOfGrids];
							int numberOfColumns[] = new int[numberOfGrids];
							for(int i=0;i<numberOfGrids;i++) {
								numberOfAttributes[i] = -1;
								offset[i] = null;
								size[i] = null;
								increment[i] = null;
								horizontal[i] = true;
								numberOfRows[i] = -1;
								numberOfColumns[i] = -1;
							}
							
							// loop through all of the grids
							for(int i=0;i<numberOfGrids;i++) {
								// obtain the 8 attributes for each grid (index, offset, size, increment, parse direction, number of rows, number of columns, number of Sprite attributes)
								for(int j=0;j<8;j++) {
									gridAttributes.add(Variable.parseFrom(in.readLine()));
								}
								
								// get the current grid index and validate it
								int gridIndex;
								try { gridIndex = Integer.parseInt(gridAttributes.getValue("Grid")); }
								catch(NumberFormatException e) {
									System.out.println("ERROR: Invalid number of sprite attributes.");
									return null;
								}
								if(gridIndex < 0 || gridIndex >= numberOfGrids) {
									System.out.println("ERROR: Invalid number of sprite attributes.");
									return null;
								}
								
								// get the number of Sprite attributes in the current grid
								try { numberOfAttributes[gridIndex] = Integer.parseInt(gridAttributes.getValue("Attributes")); }
								catch(NumberFormatException e) {
									System.out.println("ERROR: Invalid number of sprite attributes.");
									return null;
								}
								
								// get the offset for the current grid
								offset[gridIndex] = Utilities.parsePoint(gridAttributes.getValue("Pixel Offset"));
								if(offset[gridIndex] == null) {
									System.out.println("ERROR: Invalid or missing sprite sheet offset.");
									return null;
								}
								
								// get the size of the cells in the grid
								size[gridIndex] = Utilities.parseDimension(gridAttributes.getValue("Size"));
								if(size[gridIndex] == null) {
									System.out.println("ERROR: Invalid or missing sprite sheet tile size.");
									return null;
								}
								
								// get the increment values for the current grid
								increment[gridIndex] = Utilities.parseDimension(gridAttributes.getValue("Increment"));
								if(increment[gridIndex] == null) {
									System.out.println("ERROR: Invalid or missing sprite sheet tile increment.");
									return null;
								}
								
								// get the parsing direction for the current grid
								String temp = gridAttributes.getValue("Horizontal");
								if(temp == null) {
									System.out.println("ERROR: Missing horizontal / vertical sprite sheet specification.");
									return null;
								}
								horizontal[gridIndex] = temp.equalsIgnoreCase("true");
								
								// get the number of rows in the current grid
								try { numberOfRows[gridIndex] = Integer.parseInt(gridAttributes.getValue("Number of Rows")); }
								catch(NumberFormatException e) {
									System.out.println("ERROR: Missing number of rows in sprite sheet.");
									return null;
								}
								
								// get the number of columns in the current grid
								try { numberOfColumns[gridIndex] = Integer.parseInt(gridAttributes.getValue("Number of Columns")); }
								catch(NumberFormatException e) {
									System.out.println("ERROR: Missing number of columns in sprite sheet.");
									return null;
								}
								
								// start parsing at the initial offset
								// loop through all of the rows and columns incrementing the x position and y position as appropriate
								// parse each Sprite according to the current offset and size and store it in the main Sprite collection
								int xPos = offset[gridIndex].x;
								int yPos = offset[gridIndex].y;
								for(int j=0;j<numberOfRows[gridIndex];j++) {
									for(int k=0;k<numberOfColumns[gridIndex];k++) {
										spriteSheet.sprites.add(new Sprite(spriteSheetImage.getImage().getSubimage(xPos, yPos, size[gridIndex].width, size[gridIndex].height)));
										if(horizontal[gridIndex]) { xPos += increment[gridIndex].width; }
										else { yPos += increment[gridIndex].height; }
									}
									if(horizontal[gridIndex]) { yPos += increment[gridIndex].height; xPos = offset[gridIndex].x; }
									else { xPos += increment[gridIndex].width; yPos = offset[gridIndex].y; }
								}
								
								int spriteIndex;
								String spriteName, spriteType;
								VariableSystem spriteAttributes = new VariableSystem();
								// loop through the Sprite attribute specifications for the current grid
								for(int j=0;j<numberOfAttributes[gridIndex];j++) {
									// read in the 3 Sprite attributes (index, name, type) and store them
									for(int k=0;k<3;k++) {
										spriteAttributes.add(Variable.parseFrom(in.readLine()));
									}
									
									// get the Sprite index and validate it
									try { spriteIndex = spriteIndexOffset + Integer.parseInt(spriteAttributes.getValue("Sprite")); }
									catch(NumberFormatException e) {
										System.out.println("ERROR: Invalid sprite attribute index.");
										return null;
									}
									if(spriteIndex < spriteIndexOffset || spriteIndex >= spriteSheet.size()) {
										System.out.println("ERROR: Sprite attribute index out of range.");
										return null;
									}
									
									// get the Sprite name
									spriteName = spriteAttributes.getValue("Name").replace(",", "");;
									
									// get the Sprite type and parse it to an integer
									spriteType = spriteAttributes.getValue("Type");
									if(spriteName == null || spriteType == null) {
										System.out.println("ERROR: Sprite name or type missing.");
										return null;
									}
									
									// assign the current set of attributes to the corresponding Sprite
									spriteSheet.sprites.elementAt(spriteIndex).setName(spriteName);
									spriteSheet.sprites.elementAt(spriteIndex).setIndex(spriteIndex);
									spriteSheet.sprites.elementAt(spriteIndex).setParentName(spriteSheetName);
									spriteSheet.sprites.elementAt(spriteIndex).setType(Sprite.parseType(spriteType));
									
									// clear the current collection of Sprite attributes
									spriteAttributes.clear();
								}
								
								// clear the current collection of grid attributes and update the Sprite index offset (current position in the main SpriteSheet collection)
								gridAttributes.clear();
								spriteIndexOffset = spriteSheet.size();
							}
						}
						// Invalid SpriteSheet Type ===============================================
						else {
							System.out.println("ERROR: Invalid sprite sheet type specified.");
							return null;
						}
						
						return spriteSheet;
					}
				}
			}
		}
		catch(IOException e ) {
			System.out.println("ERROR: Error reading sprite sheet file.");
			return null;
		}
		catch(Exception e) {
			e.printStackTrace();
			System.out.println("ERROR: Unknown exception encountered parsing sprite sheet file.");
			return null;
		}
		return null;
	}
	
}
