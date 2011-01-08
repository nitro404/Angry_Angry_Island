using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Firecracker_Engine{

    class TheClipboard
    {
        /// <summary>
        /// This function handles the data stored on the clipboard and allows us to implement it in our program.
        /// </summary>
        /// <returns>Either the data from the clipboard (if some is available) or an error message.</returns>
        public string getClipboardText()
        {
            //theData interprets the data imported from the clipboard on the current system
            IDataObject theData = Clipboard.GetDataObject();

            //If the data can be interpreted as text, pass the text result into a string.
            //Otherwise, give us the error message.
            if (theData.GetDataPresent(DataFormats.Text))
            {
                return (string)theData.GetData(DataFormats.Text);
            }
            else
            {
                return "Error - either no data, or no text data is stored in clipboard";
            }
        }

        /// <summary>
        /// Adds text data to the clipboard.
        /// </summary>
        /// <param name="copiedData">The data copied to the clipboard</param>
        /// <returns>True if data copied successfully. If no data found, returns false.</returns>
        public bool setClipboardText(string copiedData)
        {
            //If the string is not empty, copy the data to the clipboard, and return true.
            //If not, return false.
            if (copiedData != "")
            {
                Clipboard.SetDataObject(copiedData);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
