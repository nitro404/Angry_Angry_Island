using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firecracker_Engine {

	class Key {

		private List<string> m_keyStrings;

		// constructor for a key with 1 string alias
		public Key(string keyString) {
			m_keyStrings = new List<string>();
			addKeyString(keyString);
		}

		// constructor for a key with 2 string aliases
		public Key(string keyString, string altKeyString) {
			m_keyStrings = new List<string>();
			addKeyString(keyString);
			addKeyString(altKeyString);
		}

		// constructor for a key with 3 string aliases
		public Key(string keyString, string altKeyString, string altKeyString2) {
			m_keyStrings = new List<string>();
			addKeyString(keyString);
			addKeyString(altKeyString);
			addKeyString(altKeyString2);
		}

		// constructor for a key with 4 string aliases
		public Key(string keyString, string altKeyString, string altKeyString2, string altKeyString3) {
			m_keyStrings = new List<string>();
			addKeyString(keyString);
			addKeyString(altKeyString);
			addKeyString(altKeyString2);
			addKeyString(altKeyString3);
		}

		// constructor for a key with 5 string aliases
		public Key(string keyString, string altKeyString, string altKeyString2, string altKeyString3, string altKeyString4) {
			m_keyStrings = new List<string>();
			addKeyString(keyString);
			addKeyString(altKeyString);
			addKeyString(altKeyString2);
			addKeyString(altKeyString3);
			addKeyString(altKeyString4);
		}

		// constructor for a key with 6 string aliases
		public Key(string keyString, string altKeyString, string altKeyString2, string altKeyString3, string altKeyString4, string altKeyString5) {
			m_keyStrings = new List<string>();
			addKeyString(keyString);
			addKeyString(altKeyString);
			addKeyString(altKeyString2);
			addKeyString(altKeyString3);
			addKeyString(altKeyString4);
			addKeyString(altKeyString5);
		}

		// return the number of aliases for the current key
		public int size() { return m_keyStrings.Count(); }

		// get an arbitrary alias for the current key
		public string getKeyString(int index) {
			if(index < 0 || index >= m_keyStrings.Count()) { return null; }
			return m_keyStrings[index];
		}

		// add an alias for the current key
		public bool addKeyString(string keyString) {
			if(keyString == null) { return false; }
			string formattedKeyString = keyString.Trim();
			if(!m_keyStrings.Contains(formattedKeyString)) {
				m_keyStrings.Add(formattedKeyString);
				return true;
			}
			return false;
		}

	}

}
