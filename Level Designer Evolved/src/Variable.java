// ======================================= //
// Level Designer Evolved                  //
//                                         //
// Author: Kevin Scroggins                 //
// E-Mail: nitro404@hotmail.com            //
// ======================================= //

import java.io.IOException;
import java.io.PrintWriter;

/**
 * The Variable class represents a binding between an id and a value.
 * 
 * @author Kevin Scroggins
 */
public class Variable {

	/**
	 * The id associated with the current Variable.
	 */
	private String id;
	
	/**
	 * The value assigned to the current Variable.
	 */
	private String value;
	
	/**
	 * The character used to separate the id from the value.
	 */
	final private static char separatorChar = '=';
	
	/**
	 * Constructs a Variable object instantiated with the specified id and value. 
	 * 
	 * @param id the id to associate with the current Variable.
	 * @param value the value to assign to the current Variable.
	 */
	public Variable(String id, String value) {
		this.id = (id == null) ? "" : id;
		this.value = (value == null) ? "" : value;
	}
	
	/**
	 * Returns id associated with the current Variable.
	 * 
	 * @return id associated with the current Variable.
	 */
	public String getID() {
		return this.id;
	}
	
	/**
	 * Returns the value assigned to the current Variable.
	 * 
	 * @return the value assigned to the current Variable.
	 */
	public String getValue() {
		return this.value;
	}
	
	/**
	 * Sets the id of the current Variable to the new, specified id.
	 * 
	 * @param id the new id to associate with the current Variable. 
	 */
	public void setID(String id) {
		this.id = (id == null) ? "" : id;
	}
	
	/**
	 * Sets the value of the current Variable to the new, specified value.
	 * 
	 * @param value the new value to assign to the current Variable.
	 */
	public void setValue(String value) {
		this.value = (value == null) ? "" : value;
	}
	
	/**
	 * Creates a Variable from a specified String and returns it.
	 * 
	 * Parses the Variable from a String in the form:
	 * "ID: Value" where
	 * ID is the id to be associated with the Variable,
	 * Value is the data to be assigned to the value of the Variable and
	 * : is the separator character.
	 * 
	 * @param data the data to be parsed into a Variable.
	 * @return the Variable parsed from the data String.
	 */
	public static Variable parseFrom(String data) {
		if(data == null || data.trim().length() < 1) { return null; }
		String temp = data.trim();
		
		// find the index of the separator character, if it exists
		int separatorIndex = temp.indexOf(separatorChar);
		if(separatorIndex < 1) { return null; }
		
		// return a new Variable instantiated with the id and data parsed from the string 
		String newID = temp.substring(0, separatorIndex).trim();
		String newValue = temp.substring(separatorIndex + 1, temp.length()).trim();
		
		return new Variable(newID, newValue);
	}
	
	/**
	 * Writes a Variable to the specified PrintWriter.
	 * 
	 * Outputs the Variable to the form:
	 * "ID: Value" where
	 * ID is the id associated with the current Variable,
	 * Value is the data assigned to the current Variable and
	 * : is the separator character.
	 * 
	 * @param out the output stream to write the Variable to.
	 * @throws IOException if there was an error writing to the output stream.
	 */
	public void writeTo(PrintWriter out) throws IOException {
		if(out == null) { return; }
		
		out.println(this.id + separatorChar + this.value);
	}
	
	/* (non-Javadoc)
	 * @see java.lang.Object#equals(java.lang.Object)
	 */
	public boolean equals(Object o) {
		if(o == null || !(o instanceof Variable)) {
			return false;
		}
		
		Variable v = (Variable) o;
		
		// return true if the id of each Variable matches
		return this.id.equalsIgnoreCase(v.id);
	}
	
	/* (non-Javadoc)
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		// return a String representation of the Variable with the id and value separated by the separator character (default is colon)
		return this.id + separatorChar + " " + this.value;
	}
	
	
}
