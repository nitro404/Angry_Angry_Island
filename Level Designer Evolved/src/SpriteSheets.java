// ======================================= //
// Level Designer Evolved                  //
//                                         //
// Author: Kevin Scroggins                 //
// E-Mail: nitro404@hotmail.com            //
// ======================================= //

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.util.Vector;

public class SpriteSheets {
	
	/** The collection of SpriteSheets */
	private Vector<SpriteSheet> spriteSheets;
	
	/**
	 * Constructs an empty SpriteSheet collection.
	 */
	public SpriteSheets() {
		this.spriteSheets = new Vector<SpriteSheet>(5);
	}
	
	/**
	 * Returns the number of SpriteSheets in the SpriteSheet collection.
	 * 
	 * @return the number of SpriteSheets in the SpriteSheet collection.
	 */
	public int size() {
		return this.spriteSheets.size();
	}
	
	/**
	 * Returns the SpriteSheet located at the specified index.
	 * 
	 * @param index the location to get the SpriteSheet from.
	 * @return the SpriteSheet located at the specified index.
	 */
	public SpriteSheet elementAt(int index) {
		return this.getSpriteSheet(index);
	}
	
	/**
	 * Returns the SpriteSheet located at the specified index.
	 * 
	 * @param index the location to get the SpriteSheet from.
	 * @return the SpriteSheet located at the specified index.
	 */
	public SpriteSheet getSpriteSheet(int index) {
		if(index < 0 || index >= this.spriteSheets.size()) { return null; }
		
		return this.spriteSheets.elementAt(index);
	}

	/**
	 * Returns the SpriteSheet with a matching name.
	 * 
	 * @param name the name to match.
	 * @return the SpriteSheet with a matching name.
	 */
	public SpriteSheet getSpriteSheet(String name) {
		if(name == null || name.trim().length() == 0) { return null; }
		String temp = name.trim();
		
		// loop through the collection of SpriteSheets and return the SpriteSheet with a matching name
		for(int i=0;i<this.spriteSheets.size();i++) {
			if(this.spriteSheets.elementAt(i).getName() != null &&
			   this.spriteSheets.elementAt(i).getName().equalsIgnoreCase(temp)) {
				return this.spriteSheets.elementAt(i);
			}
		}
		return null;
	}
	
	/**
	 * Returns the SpriteSheet's index with a matching name.
	 * 
	 * @param name the name to match.
	 * @return the SpriteSheet with a matching name.
	 */
	public int getSpriteSheetIndex(String name) {
		if(name == null || name.trim().length() == 0) { return -1; }
		String temp = name.trim();
		
		// loop through the collection of SpriteSheets and return the SpriteSheets's index that has a matching name
		for(int i=0;i<this.spriteSheets.size();i++) {
			if(this.spriteSheets.elementAt(i).getName() != null &&
			   this.spriteSheets.elementAt(i).getName().equalsIgnoreCase(temp)) {
				return i;
			}
		}
		return -1;
	}
	
	/**
	 * Returns the Sprite contained within the collection of SpritrSheets that has a matching name.
	 * 
	 * @param name the name to match.
	 * @return the Sprite contained within the collection of SpritrSheets that has a matching name.
	 */
	public Sprite getSprite(String name) {
		if(name == null || name.trim().length() == 0) { return null; }
		String temp = name.trim();
		
		// loop through the collection of SpriteSheets and the Sprites contained in each SpriteSheet
		// until a Sprite with a matching name is found, then return it
		for(int i=0;i<this.spriteSheets.size();i++) {
			for(int j=0;j<this.spriteSheets.elementAt(i).size();j++) {
				if(this.spriteSheets.elementAt(i).elementAt(j).getName() != null &&
				   temp.equalsIgnoreCase(this.spriteSheets.elementAt(i).elementAt(j).getName())) {
					return this.spriteSheets.elementAt(i).elementAt(j);
				}
			}
		}
		return null;
	}
	
	/**
	 * Adds the specified SpriteSheet to the current SpriteSheet collection.
	 * 
	 * @param spriteSheet the SpriteSheet to add.
	 * @return true if the SpriteSheet was valid and successfully added.
	 */
	public boolean add(SpriteSheet spriteSheet) {
		if(spriteSheet == null) { return false; }
		
		if(!this.spriteSheets.contains(spriteSheet)) {
			this.spriteSheets.add(spriteSheet);
			return true;
		}
		return false;
	}
	
	/**
	 * Parses a collection of SpriteSheets from a SpriteSheet data file.
	 * 
	 * @param fileName the sprite sheet data file name.
	 * @param spriteDirectory the directory containing the SpriteSheet image files.
	 * @return the parsed collection of SpriteSheets.
	 */
	public static SpriteSheets parseFrom(String fileName, String spriteDirectory) {
		if(fileName == null || spriteDirectory == null) {
			System.out.println("ERROR: Cannot parse sprite sheet collection from null parameter(s).");
			return null;
		}
		
		// validate the sprite sheet data file
		File file = new File(fileName);
		File dir = new File(spriteDirectory);
		if(!file.exists() || !file.isFile() || !dir.exists() || !dir.isDirectory()) {
			System.out.println("ERROR: Invalid or missing sprite sheet file or sprite sheet directory.");
			return null;
		}
		
		// open the SpriteSheet data file
		BufferedReader in;
		SpriteSheet spriteSheet;
		SpriteSheets spriteSheets = new SpriteSheets();
		try {
			in = new BufferedReader(new FileReader(file));
			
			// parse all of the SpriteSheets contained within the SpriteSheet data file and add the to the SpriteSheet collection
			do {
				spriteSheet = SpriteSheet.parseFrom(in, spriteDirectory);
				if(spriteSheet != null) {
					spriteSheets.add(spriteSheet);
				}
			} while(spriteSheet != null);
			
			in.close();
		}
		catch(IOException e) {
			System.out.println("ERROR: Error reading sprite sheet file.");
			return null;
		}
		
		return spriteSheets;
	}
	
}