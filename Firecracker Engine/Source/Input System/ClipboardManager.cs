using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Firecracker_Engine {

	public class ClipboardManager {

		/// <summary>
		/// This function handles the data stored on the clipboard and allows us to implement it in our program.
		/// </summary>
		/// <returns>Either the data from the clipboard (if some is available) or an empty string.</returns>
		public static string getClipboardText() {
            try
            {
                //theData interprets the data imported from the clipboard on the current system
                IDataObject theData = Clipboard.GetDataObject();


                //If the data can be interpreted as text, pass the text result into a string.
                //Otherwise, give us the error message.
                if (theData.GetDataPresent(DataFormats.Text))
                {
                    return (string)theData.GetData(DataFormats.Text);
                }
                return "";
            }
            catch (Exception)//Added Try/Catch to handle exceptions caused by no clipboard data being available
            {
                return "";
            }
		}

		/// <summary>
		/// Adds text data to the clipboard.
		/// </summary>
		/// <param name="copiedData">The data copied to the clipboard</param>
		/// <returns>True if data copied successfully. If no data found, returns false.</returns>
		public static bool setClipboardText(string copiedData) {
			//If the string is not empty, copy the data to the clipboard, and return true.
			//If not, return false.
			if(copiedData != null && copiedData.Length != 0) {
				Clipboard.SetDataObject(copiedData);
				return true;
			}
			return false;
		}

	}
}
