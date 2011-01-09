// ======================================= //
// Level Designer Evolved                  //
//                                         //
// Author: Kevin Scroggins                 //
// E-Mail: nitro404@hotmail.com            //
// ======================================= //

public class LevelDesigner {
	
	/**
	 * The main function for the Level Designer. Creates and displays the EditorWindow.
	 * 
	 * @param args the arguments to be passed to the program - only accepts an alternate settings file specified in the form "-filename.ext"
	 */
	public static void main(String[] args) {
		// read in an optional alternate settings file in the form "-filename.ext"
		String settingsFileName = null;
		if(args.length == 1) {
			if(args[0].charAt(0) == '-') {
				settingsFileName = args[0].substring(1, args[0].length());
			}
		}
		
		// initialise and display the main editor window
		EditorWindow window = new EditorWindow(settingsFileName);
		window.setVisible(true);
	}
	
}
