// ======================================= //
// Level Designer Evolved                  //
//                                         //
// Author: Kevin Scroggins                 //
// E-Mail: nitro404@hotmail.com            //
// ======================================= //

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.util.Vector;

/**
 * The VariableSystem class represents a collection of Variable objects.
 * 
 * @author Kevin Scroggins
 */
public class VariableSystem {
	
	/** The collection of Variable objects. */
	private Vector<Variable> variables;
	
	/** Constructs an empty VariableSystem object with a default size of 10. */
	public VariableSystem() {
		this.variables = new Vector<Variable>(10);
	}
	
	/**
	 * Creates and adds a Variable object to the collection of Variables.
	 * 
	 * @param id the id of the Variable to be created.
	 * @param value the value of the Variable to be created.
	 * @return true if the Variable is valid and not already contained within the collection of Variables.
	 */
	public boolean add(String id, String value) {
		return this.add(new Variable(id, value));
	}
	
	/**
	 * Adds a Variable object to the collection of Variables.
	 * 
	 * @param v the Variable to be added to the collection of Variables.
	 * @return true if the Variable is valid and not already contained within the collection of Variables.
	 */
	public boolean add(Variable v) {
		// verify that the Variable is valid and not already contained in the Variables collection, then add it
		if(v != null && v.getID().length() > 0 && !this.contains(v)) {
			this.variables.add(v);
			return true;
		}
		return false;
	}
	
	/**
	 * Adds (merges) another VariableSystem into the current collection.
	 * 
	 * @param v the collection of Variables to add (merge) into the current VariableSystem. 
	 */
	public void add(VariableSystem v) {
		if(v == null) { return; }
		
		// loop through all of the variables in the specified Variables collection and add them to the current collection
		for(int i=0;i<v.variables.size();i++) {
			this.add(v.variables.elementAt(i));
		}
	}
	
	/**
	 * Adds (merges) a Vector of Variable objects into the current collection.
	 * 
	 * @param v a Vector of Variable objects to add (merge) into the current Variables collection.
	 */
	public void add(Vector<Variable> v) {
		if(v == null) { return; }
		
		// loop through all of the variables in the specified Vector of Variable objects and add them to the current collection
		for(int i=0;i<v.size();i++) {
			this.add(v.elementAt(i));
		}
	}
	
	/**
	 * Removes a Variable located at a specified index.
	 * 
	 * @param index the index from which to remove a Variable.
	 * @return true if the Variable was successfully removed from the Variables collection.
	 */
	public boolean remove(int index) {
		if(index < 0 || index >= this.variables.size()) { return false; }
		
		this.variables.remove(index);
		
		return true;
	}
	
	/**
	 * Removes a Variable based on its id.
	 * 
	 * @param id the id of the variable to remove.
	 * @return true if the Variable was located and removed from the collection of Variables.
	 */
	public boolean remove(String id) {
		if(id == null || id.trim().length() < 1) { return false; }
		String temp = id.trim();
		
		// loop through and check to see if any variables contain a matching id, and remove the corresponding Variable if one is found
		for(int i=0;i<this.variables.size();i++) {
			if(this.variables.elementAt(i).getID().equalsIgnoreCase(temp)) {
				this.variables.remove(i);
				return true;
			}
		}
		
		return false;
	}
	
	/**
	 * Removes a Variable from the current collection of Variables.
	 * 
	 * @param v the Variable to be removed.
	 * @return true if the Variable was located and removed from the collection of Variables.
	 */
	public boolean remove(Variable v) {
		if(v == null) { return false; }
		
		// loop through and search for a Variable matching the corresponding parameter and remove it if it is found
		for(int i=0;i<this.variables.size();i++) {
			if(this.variables.elementAt(i).equals(v)) {
				this.variables.remove(i);
				return true;
			}
		}
		
		return false;
	}
	
	/**
	 * Clears the current collection of Variables.
	 */
	public void clear() {
		this.variables.clear();
	}
	
	/**
	 * Returns the number of Variable objects stored within the collection of Variables.
	 * 
	 * @return the number of Variable objects stored within the collection of Variables.
	 */
	public int size() {
		return this.variables.size();
	}
	
	/**
	 * Checks to see if a Variable with an id matching the specified parameter exists. 
	 * 
	 * @param id the id to be matched.
	 * @return true if a Variable with an id matching the specified parameter is found.
	 */
	public boolean contains(String id) {
		if(id == null || id.trim().length() < 1) { return false; }
		String temp = id.trim();
		
		// loop through and check to see if any variables contain a matching id
		for(int i=0;i<this.variables.size();i++) {
			if(this.variables.elementAt(i).getID().equalsIgnoreCase(temp)) {
				return true;
			}
		}
		
		return false;
	}
	
	/**
	 * Checks to see if a matching Variable exists within the collection of Variables.
	 * 
	 * @param v the Variable to be matched.
	 * @return true if a matching Variable is found within the collection of Variables.
	 */
	public boolean contains(Variable v) {
		if(v == null) { return false; }
		
		// loop through and search for a Variable matching the corresponding parameter
		for(int i=0;i<this.variables.size();i++) {
			if(this.variables.elementAt(i).equals(v)) {
				return true;
			}
		}
		
		return false;
	}
	
	/**
	 * Returns the index of the Variable matching the specified id if it exists, otherwise returns -1.
	 * 
	 * @param id the id to be matched.
	 * @return the index of the Variable matching the specified id if it exists, otherwise returns -1.
	 */
	public int indexOf(String id) {
		if(id == null || id.trim().length() < 1) { return -1; }
		String temp = id.trim();
		
		// loop through and check to see if any variables contain a matching id
		for(int i=0;i<this.variables.size();i++) {
			if(this.variables.elementAt(i).getID().equalsIgnoreCase(temp)) {
				return i;
			}
		}
		
		return -1;
	}
	
	/**
	 * Returns the index of the Variable matching the specified Variable if it exists, otherwise returns -1.
	 * 
	 * @param v the Variable to be matched.
	 * @return the index of the Variable matching the specified Variable if it exists, otherwise returns -1.
	 */
	public int indexOf(Variable v) {
		if(v == null) { return -1; }
		
		// loop through and search for a Variable matching the corresponding parameter
		for(int i=0;i<this.variables.size();i++) {
			if(this.variables.elementAt(i).equals(v)) {
				return i;
			}
		}
		
		return -1;
	}
	
	/**
	 * Returns the Variable located at the specified index, otherwise returns null if the index is out of range or there are no elements.
	 * 
	 * @param index the index of the Variable to be returned.
	 * @return the Variable located at the specified index, otherwise returns null if the index is out of range or there are no elements.
	 */
	public Variable elementAt(int index) {
		if(index < 0 || index >= this.variables.size()) { return null; }
		
		// return the Variable at the specified index if the index is within the boundaries of the collection of Variables
		return this.variables.elementAt(index);
	}
	
	/**
	 * Returns the value of a Variable matching the corresponding id if it exists in the collection of Variables, otherwise returns null.
	 * 
	 * @param id the id to be matched.
	 * @return the value of a Variable matching the corresponding id if it exists in the collection of Variables, otherwise returns null.
	 */
	public String getValue(String id) {
		if(id == null || id.trim().length() < 1) { return null; }
		String temp = id.trim();
		
		// loop through and check to see if any variables contain a matching id, if one exists then return the value of the corresponding Variable
		for(int i=0;i<this.variables.size();i++) {
			if(this.variables.elementAt(i).getID().equalsIgnoreCase(temp)) {
				return this.variables.elementAt(i).getValue();
			}
		}
		
		return null;
	}
	
	/**
	 * Parses a collection of Variables from the specified file name.
	 * 
	 * @param fileName the name of the file to be parsed into a collection of variables.
	 * @return true if a collection of Variables was successfully parsed from the specified file name.
	 */
	public boolean parseFrom(String fileName) {
		if(fileName == null) { return false; }
		return parseFrom(new File(fileName), false);
	}
	
	/**
	 * Parses a collection of Variables from the specified file name and adds them to the current collection of Variables (if appropriate).
	 * 
	 * @param fileName the name of the file to be parsed into a collection of Variables.
	 * @param append specifies if the collection of Variables parsed should be added to the current collection of Variables.
	 * @return true if a collection of Variables was successfully parsed and added to the current collection of Variables from the specified file name.
	 */
	public boolean parseFrom(String fileName, boolean append) {
		if(fileName == null) { return false; }
		return parseFrom(new File(fileName), append);
	}
	
	/**
	 * Parses a collection of Variables from the specified file.
	 * 
	 * @param file the file to be parsed into a collection of Variables.
	 * @return true if a collection of Variables was successfully parsed from the specified file.
	 */
	public boolean parseFrom(File file) {
		return parseFrom(file, false);
	}
	
	/**
	 * Parses a collection of Variables from the specified file and adds them to the current collection of Variables (if appropriate).
	 * 
	 * @param file the file to be parsed into a collection of Variables.
	 * @param append specifies if the collection of Variables parsed should be added to the current collection of Variables.
	 * @return true if a collection of Variables was successfully parsed from the specified file.
	 */
	public boolean parseFrom(File file, boolean append) {
		if(file == null || !file.exists() || !file.isFile()) { return false; }
		
		BufferedReader in;
		String input;
		try {
			// open the file
			in = new BufferedReader(new FileReader(file));
			
			// initialise the collection of Variables if not appending
			if(!append) {
				this.variables = new Vector<Variable>(10);
			}
			
			// loop until the end of the file
			while((input = in.readLine()) != null) {
				// parse a Variable from the current line
				Variable v = Variable.parseFrom(input);
				
				// if the Variable was successfully parsed
				if(v != null) {
					// remove the new Variable if it already exists (replace it)
					if(this.contains(v)) {
						this.remove(v);
					}
					
					// add the Variable to the collection
					this.add(v);
				}
			}
			
			in.close();
		}
		catch(IOException e) {
			return false;
		}
		
		return true;
	}
	
	/**
	 * Outputs a collection of Variables to a specified file.
	 * 
	 * @param fileName the name of the file to write the collection of Variables to.
	 * @return true if writing to the file was successful.
	 */
	public boolean writeTo(String fileName) {
		return this.writeTo(new File(fileName));
	}
	
	/**
	 * Outputs a collection of Variables to a specified file.
	 * 
	 * @param file the file to write the collection of Variables to.
	 * @return true if writing to the file was successful.
	 */
	public boolean writeTo(File file) {
		if(file == null || !file.exists() || !file.isFile()) { return false; }
		
		PrintWriter out;
		try {
			// open the file for writing, write to it and then close the file
			out = new PrintWriter(new FileWriter(file));
			this.writeTo(out);
			out.close();
		}
		catch(IOException e) {
			return false;
		}
		return true;
	}
	
	/**
	 * Writes a collection of Variables to a specified PrintWriter.
	 * 
	 * @param out the PrintWriter to write the collection of Variables to.
	 * @throws IOException if there was an error writing to the output stream.
	 */
	public void writeTo(PrintWriter out) throws IOException {
		for(int i=0;i<this.variables.size();i++) {
			this.variables.elementAt(i).writeTo(out);
		}
	}
	
	/* (non-Javadoc)
	 * @see java.lang.Object#equals(java.lang.Object)
	 */
	public boolean equals(Object o) {
		if(o == null || !(o instanceof VariableSystem)) { return false; }
		
		VariableSystem v = (VariableSystem) o;
		
		// check the size of each collection of Variables
		if(this.variables.size() != v.variables.size()) { return false; }
		
		// verify that each Variable in the current collection is also in the other collection of Variables
		for(int i=0;i<this.variables.size();i++) {
			if(!v.contains(this.variables.elementAt(i))) {
				return false;
			}
		}
		return true;
	}
	
	/* (non-Javadoc)
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		String s = new String();
		
		// return a String representation of the collection of Variables using comma to separate the data
		for(int i=0;i<this.variables.size();i++) {
			s += this.variables.elementAt(i);
			if(i<this.variables.size()-1) {
				s += ", ";
			}
		}
		
		return s;
	}
	
}
