using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Firecracker_Engine {

	class ControlSystem {

		// list of commands
		private string[] m_commands;

		// list of valid input keys
		private static Keys[] m_keys = new Keys[] {
			Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12,
			Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0,
			Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z,
			Keys.OemMinus, Keys.OemPlus, Keys.OemOpenBrackets, Keys.OemCloseBrackets, Keys.OemBackslash, Keys.OemSemicolon, Keys.OemQuotes, Keys.OemComma, Keys.OemPeriod, Keys.OemQuestion,
			Keys.Tab, Keys.Space, Keys.Up, Keys.Down, Keys.Left, Keys.Right,
			Keys.Enter, Keys.Home, Keys.End, Keys.PageUp, Keys.PageDown, Keys.Delete, Keys.Back
		};

		// list of aliases for each input key
		private static Key[] m_keyStrings = new Key[] {
			new Key("F1"), new Key("F2"), new Key("F3"), new Key("F4"), new Key("F5"), new Key("F6"), new Key("F7"), new Key("F8"), new Key("F9"), new Key("F10"), new Key("F11"), new Key("F12"),
			new Key("1", "One"), new Key("2", "Two"), new Key("3", "Three"), new Key("4", "Four"), new Key("5", "Five"), new Key("6", "Six"), new Key("7", "Seven"), new Key("8", "Eight"), new Key("9", "Nine"), new Key("0", "Zero"),
			new Key("A"), new Key("B"), new Key("C"), new Key("D"), new Key("E"), new Key("F"), new Key("G"), new Key("H"), new Key("I"), new Key("J"), new Key("K"), new Key("L"), new Key("M"), new Key("N"), new Key("O"), new Key("P"), new Key("Q"), new Key("R"), new Key("S"), new Key("T"), new Key("U"), new Key("V"), new Key("W"), new Key("X"), new Key("Y"), new Key("Z"),
			new Key("-", "_", "Minus", "Underscore"), new Key("+", "=", "Equals", "Plus"), new Key("[", "{", "OpenBracket", "OpenBrace"), new Key("]", "}", "CloseBracket", "CloseBrace"), new Key("\\", "|", "Backslash", "Bar"), new Key(":", ";", "Colon", "SemiColon"), new Key("\'", "\"", "Apostrophe", "Quote"), new Key(",", "<", "Comma", "LessThan", "OpenAngleBracket"), new Key(".", ">", "Period", "GreaterThan", "CloseAngleBracket"), new Key("/", "?", "Slash", "ForwardSlash", "Question", "QuestionMark"),
			new Key("Tab"), new Key("Space"), new Key("Up", "UpArrow"), new Key("Down", "DownArrow"), new Key("Left", "LeftArrow"), new Key("Right", "RightArrow"),
			new Key("Enter", "Return"), new Key("Home"), new Key("End"), new Key("PageUp", "PgUp"), new Key("PageDown", "PgDn"), new Key("Delete", "Del"), new Key("Backspace")
		};

		// "global" variables
		private GameSettings m_settings;
		private CommandInterpreter m_interpreter;

		public ControlSystem() {
			m_commands = new string[m_keys.Length];
		}

		// initialize the control system
		public void initialize(GameSettings settings, CommandInterpreter interpreter) {
			m_settings = settings;
			m_interpreter = interpreter;

			// get the list of controls from the settings file manager
			List<Variable> controlVariables = settings.getControls();

			// create the key bindings based on these controls
			for(int i=0;i<controlVariables.Count();i++) {
				string key = controlVariables[i].id;
				string cmd = controlVariables[i].value;

				createKeyBind(key, cmd);
			}
		}

		// bind a key to a command
		public bool createKeyBind(string key, string cmd) {
			if(key == null || cmd == null) { return false; }

			// get the index of the key based on its alias
			int keyIndex = getKeyIndex(key);

			// if the key is valid, store the command
			if(keyIndex >= 0) {
				m_commands[keyIndex] = cmd;
				m_settings.createKeyBind(m_keyStrings[keyIndex], cmd);
				return true;
			}

			return false;
		}

		// unbind a command from a key
		public bool removeKeyBind(string key) {
			if(key == null) { return false; }

			// get the index of the key based on its alias
			int keyIndex = getKeyIndex(key);

			// if the key is valid, set the command to null
			if(keyIndex >= 0) {
				m_commands[keyIndex] = null;
				m_settings.removeKeyBind(m_keyStrings[keyIndex]);
				return true;
			}

			return false;
		}

		// unbind all commands from all keys
		public void removeAllKeyBinds() {
			for(int i=0;i<m_commands.Length;i++) {
				m_commands[i] = null;
			}

			m_settings.removeAllKeyBinds();
		}

		// get the index of a key based on its alias
		public static int getKeyIndex(string data) {
			if(data == null) { return -1; }
			string keyString = data.Trim();

			// loop through all of the keys and all of the aliases for each key, until a match is found
			for(int i=0;i<m_keyStrings.Length;i++) {
				for(int j=0;j<m_keyStrings[i].size();j++) {
					if(keyString.Equals(m_keyStrings[i].getKeyString(j), StringComparison.OrdinalIgnoreCase)) {
						return i;
					}
				}
			}
			return -1;
		}

		public void handleInput(GameTime gameTime) {
			// get the keys currently pressed
			KeyboardState keyboard = Keyboard.GetState();
			Keys[] pressedKeys = keyboard.GetPressedKeys();

			// loop through all of the input keys, and if the current pressed key has a command associated with it, execute the command
			for(int i=0;i<pressedKeys.Length;i++) {
				for(int j=0;j<m_keys.Length;j++) {
					if(pressedKeys[i] == m_keys[j] && m_commands[j] != null) {
						m_interpreter.execute(m_commands[j]);
						break;
					}
				}
			}
		}

	}

}
