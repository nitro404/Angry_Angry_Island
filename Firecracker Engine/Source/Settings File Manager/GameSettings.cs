using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Firecracker_Engine {

	class GameSettings {

		private static string m_defaultFileName = "settings.ini";
		private static string m_defaultSpriteSheetFileName = "spritesheets.ini";

		public int m_screenWidth;
		public int m_screenHeight;
		public bool m_fullScreen;
		private string m_spriteSheetFileName;

		private VariableSystem m_variables;

		public GameSettings() {
			// initialize game settings to default values
			screenWidth = 1024;
			screenHeight = 768;
			fullScreen = false;
			m_spriteSheetFileName = m_defaultSpriteSheetFileName;
		}

		public static string defaultFileName {
			get { return m_defaultFileName; }
		}

		public static string defaultSpriteSheetFileName {
			get { return m_defaultSpriteSheetFileName; }
		}

		public int screenWidth {
			get { return m_screenWidth; }
			set { if(value >= 640 && value <= 4096) { m_screenWidth = value; } }
		}

		public int screenHeight {
			get { return m_screenHeight; }
			set { if(value >= 480 && value <= 3072) { m_screenHeight = value; } }
		}

		public bool fullScreen {
			get { return m_fullScreen; }
			set { m_fullScreen = value; }
		}

		public string spriteSheetFileName {
			get { return m_spriteSheetFileName; }
			set { if(value != null) { m_spriteSheetFileName = value; } }
		}

		// load game settings from a specified file name
		public bool loadFrom(string fileName) {
			// use a variable system to parse the settings file
			VariableSystem newVariables = VariableSystem.readFrom(fileName);
			if(newVariables == null) { return false; }

			m_variables = newVariables;

			// create local variables instantiated with data parsed from the variable system
			try { screenWidth = int.Parse(m_variables.getValue("Screen Width", "Settings")); } catch(Exception) { }
			try { screenHeight = int.Parse(m_variables.getValue("Screen Height", "Settings")); } catch(Exception) { }
			try { fullScreen = bool.Parse(m_variables.getValue("Fullscreen", "Settings")); } catch(Exception) { }
			spriteSheetFileName = m_variables.getValue("SpriteSheet File", "Paths");

			return true;
		}

		public bool saveTo(string fileName) {
			// update the variable system with the new game settings values
			m_variables.setValue("Screen Width", m_screenWidth.ToString(), "Settings");
			m_variables.setValue("Screen Height", m_screenHeight.ToString(), "Settings");
			m_variables.setValue("Fullscreen", m_fullScreen.ToString().ToLower(), "Settings");
			m_variables.setValue("SpriteSheet File", m_spriteSheetFileName, "Paths");

			// group the variables by categories
			m_variables.sort();

			// update the settings file with the changes
			return m_variables.writeTo(fileName);
		}

	}

}
