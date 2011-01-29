using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firecracker_Engine {

	public enum BoolChange { None, Toggle, Enable, Disable }

	public class CommandInterpreter {

		public CommandInterpreter() { }

		// execute a command
		public void execute(string command) {
			if(command == null) { return; }
			string cmd = command.Trim();

			if(cmd.Length == 0) { return; }

			// parse the command and execute the corresponding function
			if(matchCommand(cmd, "quit") || matchCommand(cmd, "exit")) { Firecracker.engineInstance.Exit(); } // close the game
			else if(matchCommand(cmd, "clear") || matchCommand(cmd, "cls")) { Firecracker.console.clear(); } // clear the console
			else if(matchCommand(cmd, "echo")) { Firecracker.console.writeLine(getStringValue(cmd)); } // write text to the console
			else if(matchCommand(cmd, "menu")) { Firecracker.screenManager.set(ScreenType.Menu, getScreenVisibilityChange(cmd)); } // change the menu's visibility
			else if(matchCommand(cmd, "console")) { Firecracker.screenManager.set(ScreenType.Console, getScreenVisibilityChange(cmd)); } // change the console's visibility
			else if(matchCommand(cmd, "map") || matchCommand(cmd, "level")) { // load a specified level
				string levelName = getStringValue(cmd);
				bool levelLoaded = Firecracker.engineInstance.loadLevel(levelName);
				if(levelLoaded) {
					Firecracker.console.writeLine("Loaded map: " + levelName);
				}
				else {
					Firecracker.console.writeLine("Unable to load map: " + levelName);
				}
			}
			else if(matchCommand(cmd, "bind")) { // bind a key to a command
				string bind = getStringValue(cmd);
				if(bind.Length == 0) {
					Firecracker.console.writeLine("Unable to bind key");
					return;
				}
				int spaceIndex = bind.IndexOf(" ");
				if(spaceIndex < 0) {
					Firecracker.console.writeLine("Unable to bind key");
					return;
				}

				string keyString = bind.Substring(0, spaceIndex);
				string keyCommand = bind.Substring(spaceIndex + 1, bind.Length - spaceIndex - 1);

				if(Firecracker.controlSystem.createKeyBind(keyString, keyCommand)) {
					Firecracker.console.writeLine("Successfully bound key \"" + keyString + "\" to command \"" + keyCommand + "\"");
				}
				else {
					Firecracker.console.writeLine("Unable to bind key \"" + keyString + "\" to command \"" + keyCommand + "\"");
				}
			}
			else if(matchCommand(cmd, "unbind")) { // unbind a command from a key
				string keyString = getStringValue(cmd);
				if(keyString.Length == 0) {
					Firecracker.console.writeLine("Unable to unbind key \"" + keyString + "\"");
					return;
				}

				if(Firecracker.controlSystem.removeKeyBind(keyString)) {
					Firecracker.console.writeLine("Successfully unbound key \"" + keyString + "\"");
				}
				else {
					Firecracker.console.writeLine("Unable to unbind key \"" + keyString + "\"");
				}
			}
			else if(matchCommand(cmd, "unbindall")) { // unbind all commands from all keys
				Firecracker.controlSystem.removeAllKeyBinds();
				Firecracker.console.writeLine("Successfully unbound all keys");
			}
			else { Firecracker.console.writeLine("Unknown command: " + cmd); }
		}

		// check to see if some input matches a specified command
		private static bool matchCommand(string input, string command) {
			if(input == null || command == null) { return false; }

			string temp = input.Trim();
			int spaceIndex = temp.IndexOf(" ");

			string inputCmd;
			if(spaceIndex < 0) { inputCmd = temp; }
			else { inputCmd = temp.Substring(0, spaceIndex); }

			if(inputCmd.Length != command.Length) { return false; }
			return inputCmd.Equals(command, StringComparison.OrdinalIgnoreCase);
		}

		// parse a string value which trails after a command (delimited by whitespace)
		private static string getStringValue(string data) {
			if(data == null) { return ""; }
			int spaceIndex = data.IndexOf(" ");
			if(spaceIndex < 0) { return ""; }
			return data.Substring(spaceIndex + 1, data.Length - spaceIndex - 1);
		}

		// parse a integer value which trails after a command (delimited by whitespace)
		private static int getIntValue(string data) {
			return int.Parse(getStringValue(data));
		}

		// parse a float value which trails after a command (delimited by whitespace)
		private static float getFloatValue(string data) {
			return float.Parse(getStringValue(data));
		}

		// parse a boolean float which trails after a command (delimited by whitespace)
		private static bool getBoolValue(string data) {
			return bool.Parse(getStringValue(data));
		}

		// check to see if a string value trailing after a command is a valid screen visibility change value
		public static ScreenVisibilityChange getScreenVisibilityChange(string data) {
			string temp = getStringValue(data).Trim().ToLower();

			if(temp.Equals("toggle", StringComparison.OrdinalIgnoreCase)) {
				return ScreenVisibilityChange.Toggle;
			}

			if(temp.Equals("1", StringComparison.OrdinalIgnoreCase) || 
			   temp.Equals("on", StringComparison.OrdinalIgnoreCase) ||
			   temp.Equals("enable", StringComparison.OrdinalIgnoreCase) ||
			   temp.Equals("true", StringComparison.OrdinalIgnoreCase) ||
			   temp.Equals("show", StringComparison.OrdinalIgnoreCase) ||
               temp.Equals("open", StringComparison.OrdinalIgnoreCase)){
				return ScreenVisibilityChange.Show;
			}

			if(temp.Equals("0", StringComparison.OrdinalIgnoreCase) || 
			   temp.Equals("off", StringComparison.OrdinalIgnoreCase) ||
			   temp.Equals("disable", StringComparison.OrdinalIgnoreCase) ||
			   temp.Equals("false", StringComparison.OrdinalIgnoreCase) ||
			   temp.Equals("hide", StringComparison.OrdinalIgnoreCase) ||
               temp.Equals("close", StringComparison.OrdinalIgnoreCase)) {
				return ScreenVisibilityChange.Hide;
			}

			return ScreenVisibilityChange.None;
		}

		// check to see if a string value trailing after a command is a valid boolean change value
		public static BoolChange getBoolChange(string data) {
			string temp = getStringValue(data).Trim().ToLower();

			if(temp.Equals("toggle", StringComparison.OrdinalIgnoreCase)) {
				return BoolChange.Toggle;
			}

			if(temp.Equals("1", StringComparison.OrdinalIgnoreCase) || 
			   temp.Equals("on", StringComparison.OrdinalIgnoreCase) ||
			   temp.Equals("enable", StringComparison.OrdinalIgnoreCase) ||
			   temp.Equals("true", StringComparison.OrdinalIgnoreCase)) {
				return BoolChange.Enable;
			}

			if(temp.Equals("0", StringComparison.OrdinalIgnoreCase) || 
			   temp.Equals("off", StringComparison.OrdinalIgnoreCase) ||
			   temp.Equals("disable", StringComparison.OrdinalIgnoreCase) ||
			   temp.Equals("false", StringComparison.OrdinalIgnoreCase)) {
				return BoolChange.Disable;
			}

			return BoolChange.None;
		}

	}

}
