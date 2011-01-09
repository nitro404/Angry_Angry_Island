// ======================================= //
// Level Designer Evolved                  //
//                                         //
// Author: Kevin Scroggins                 //
// E-Mail: nitro404@hotmail.com            //
// ======================================= //

import java.awt.Color;
import java.awt.Dimension;
import java.awt.Point;
import java.io.File;
import java.util.StringTokenizer;

/**
 * The Utilities class is a static, non-instantiable class which contains helper functions utilised by the Level Designer.
 * 
 * @author Kevin Scroggins
 */
public class Utilities {

	/**
	 * Parses a Dimension object from a String in the form:
	 * "#,#" OR "#x#" where
	 * # is an arbitrary integer.
	 * 
	 * @param data the String to be parsed into a Dimension object.
	 * @return the Dimension object parsed from the String.
	 */
	public static Dimension parseDimension(String data) {
		// verify that the String is valid
		if(data != null && data.trim().length() > 0) {
			// create a StringTokenizer to parse the String
			StringTokenizer st = new StringTokenizer(data.trim(), ",x ", false);
			// parse integers from the tokens contained within the String (if there are 2) and return a new Dimension object that has been instantiated with these values
			if(st.countTokens() == 2) {
				try {
					return new Dimension(Integer.parseInt(st.nextToken()), Integer.parseInt(st.nextToken()));
				}
				catch(NumberFormatException e) { }
			}
		}
		return null;
	}
	
	/**
	 * Parses a Point object from a String in the form:
	 * "#,#" where
	 * # is an arbitrary integer.
	 * 
	 * @param data the String to be parsed into a Point object.
	 * @return the Point object parsed from the String.
	 */
	public static Point parsePoint(String data) {
		// verify that the String is valid
		if(data != null && data.trim().length() > 0) {
			// create a StringTokenizer to parse the String
			StringTokenizer st = new StringTokenizer(data.trim(), ", ", false);
			// parse integers from the tokens contained within the String (if there are 2)
			// and return a new Point object that has been instantiated with these values
			if(st.countTokens() == 2) {
				try {
					return new Point(Integer.parseInt(st.nextToken()), Integer.parseInt(st.nextToken()));
				}
				catch(NumberFormatException e) { }
			}
		}
		return null;
	}
	
	/**
	 * Parses a Color object from a String in the form:
	 * "R, G, B" where
	 * R, G and B are integers between 0 and 255 representing the amount of red, green and blue (respectively) in the colour.
	 * 
	 * @param data the String to be parsed into a Color object.
	 * @return the Color object parsed from the specified String.
	 */
	public static Color parseColour(String data) {
		// verify that the String is valid
		if(data != null && data.trim().length() > 0) {
			// create a StringTokenizer to parse the String
			StringTokenizer st = new StringTokenizer(data.trim(), ", ", false);
			// parse integers from the tokens contained within the String (if there are 3)
			// and return a new Color object that has been instantiated with these values
			if(st.countTokens() == 3) {
				try {
					return new Color(Integer.parseInt(st.nextToken()),
							         Integer.parseInt(st.nextToken()),
							         Integer.parseInt(st.nextToken()));
				}
				catch(NumberFormatException e) { }
			}
		}
		return null;
	}
	
	/**
	 * Verifies that a file exists and is actually a file from a specified file name.
	 * 
	 * @param fileName the file name to verify.
	 * @return true if the file exists and is a valid file based on the specified file name.
	 */
	public static boolean isValidFile(String fileName) {
		if(fileName == null || fileName.trim().length() == 0) { return false; }
		File file = new File(fileName.trim());
		if(!file.exists() || !file.isFile()) { return false; }
		return true;
	}
	
	/**
	 * Verifies that a directory exists and is actually a directory from a specified path.
	 * 
	 * @param fileName the path to verify.
	 * @return true if the directory exists and is a valid directory based on the specified path.
	 */
	public static boolean isValidDirectory(String fileName) {
		if(fileName == null || fileName.trim().length() == 0) { return false; }
		File file = new File(fileName.trim());
		if(!file.exists() || !file.isDirectory()) { return false; }
		return true;
	}
	
	/**
	 * Returns the first valid file name or the default file name if none of the file names could be validated.
	 * 
	 * @param initialFileName the first file name to validate.
	 * @param defaultFileName the second (and default) file name to validate.
	 * @param alternateFileName the alternate file name to validate.
	 * @return the valid file name (or default file name) based on the validation process.
	 */
	public static String getValidFile(String initialFileName, String defaultFileName, String alternateFileName) {
		String fileName = Utilities.isValidFile(initialFileName) ? initialFileName : null;
		if(fileName == null) { fileName = Utilities.isValidFile(defaultFileName) ? defaultFileName : null; }
		if(fileName == null) { fileName = Utilities.isValidFile(alternateFileName) ? alternateFileName : null; }
		if(fileName == null) { fileName = defaultFileName; }
		return fileName;
	}
	
	/**
	 * Returns the first valid directory or the default directory if none of the directories could be validated.
	 * 
	 * @param initialFileName the first directory to validate.
	 * @param defaultFileName the second (and default) directory to validate.
	 * @param alternateFileName the alternate directory to validate.
	 * @return the valid directory (or default directory) based on the validation process.
	 */
	public static String getValidDirectory(String initialFileName, String defaultFileName, String alternateFileName) {
		String fileName = Utilities.isValidDirectory(initialFileName) ? initialFileName : null;
		if(fileName == null) { fileName = Utilities.isValidDirectory(defaultFileName) ? defaultFileName : null; }
		if(fileName == null) { fileName = Utilities.isValidDirectory(alternateFileName) ? alternateFileName : null; }
		if(fileName == null) { fileName = defaultFileName; }
		return fileName;
	}
	
	/**
	 * Returns the extension from the specified file name (if any), otherwise null.
	 * 
	 * @param fileName the file name to have the extension parsed from.
	 * @return the extension from the specified file name (if any), otherwise null.
	 */
	public static String getExtension(String fileName) {
		if(fileName.lastIndexOf('.') != -1) {
			return fileName.substring(fileName.lastIndexOf('.') + 1, fileName.length());
		}
		else {
			return null;
		}
	}
	
	/**
	 * Returns true if the specified file name has the specified extension.
	 * 
	 * @param fileName the file name to match the specified file extension to.
	 * @param fileExtension the file extension to match.
	 * @return true if the specified file name has the specified extension.
	 */
	public static boolean hasExtension(String fileName, String fileExtension) {
		String actualFileExtension = getExtension(fileName);
		return actualFileExtension != null && actualFileExtension.equalsIgnoreCase(fileExtension);
	}
	
	/**
	 * Returns the file name without its extension (if it had one), otherwise the original String.
	 * 
	 * @param fileName the file name to remove the extension from.
	 * @return the file name without its extension.
	 */
	public static String removeExtension(String fileName) {
		if(fileName.lastIndexOf('.') != -1) {
			return fileName.substring(0, fileName.lastIndexOf('.'));
		}
		else {
			return fileName;
		}
	}
	
}
