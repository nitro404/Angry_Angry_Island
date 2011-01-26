using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Firecracker_Engine
{
    /// <summary>
    /// Filesystem class
    /// This subsystem is used to manage file IO.
    /// NOTE: Currently it only supports a single file being accessed at a time.
    /// Author:  CyrilM - 22 Jan 2011
    /// </summary>
    public static class Filesystem
    {
        private static StreamReader m_FileReader;
        private static StreamWriter m_FileWriter;
        private static bool m_bIsFileOpen = false;
        private static AccessType m_eAccessType = AccessType.AccessType_None;

        public static bool IsFileOpen
        {
            get { return m_bIsFileOpen; }
        }

        static Filesystem ()
        {
        }

        /// <summary>
        /// File access types.
        /// </summary>
        public enum AccessType
        {
            AccessType_ReadOnly,
            AccessType_WriteOnly,
            AccessType_None
        };

        /// <summary>
        /// Opens a single file for access.
        /// </summary>
        /// <param name="sFilename">Name of the file you want to open. The name is relative to the "Content" directory</param>
        /// <param name="eAccessType">The access type. Deffault is AccessType_ReadOnly</param>
        /// <returns>True on success</returns>
        public static bool OpenFile(string sFilename, AccessType eAccessType )
        {
            if (m_bIsFileOpen)
                CloseFile();
            string targetFile = string.Concat(Directory.GetCurrentDirectory(), "\\", sFilename);
            if (!File.Exists(targetFile))
            {
                // oops. this file doesn't exist!
                return false;
            }

            switch (eAccessType)
            {
                case AccessType.AccessType_ReadOnly:
                    {
                        m_FileReader = new StreamReader(sFilename);
                        m_bIsFileOpen = true;
                        m_eAccessType = eAccessType;
                    }
                    break;
                case AccessType.AccessType_WriteOnly:
                    {
                        m_FileWriter = new StreamWriter(sFilename);
                        m_bIsFileOpen = true;
                        m_eAccessType = eAccessType;
                    }
                    break;
                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// Closes an open file.
        /// </summary>
        public static void CloseFile()
        {
            if (m_bIsFileOpen)
            {
                if (m_eAccessType == AccessType.AccessType_ReadOnly)
                {
                    m_FileReader.Close();
                }
                else if (m_eAccessType == AccessType.AccessType_WriteOnly)
                {
                    m_FileWriter.Close();
                }

                m_bIsFileOpen = false;
                m_eAccessType = AccessType.AccessType_None;
            }
        }

        /// <summary>
        /// Attempts to read a line from the file.
        /// If an EOF is encountered then the file is closed.
        /// </summary>
        /// <param name="sLineData">A reference to a String that will contain the contents of the next line read.</param>
        /// <returns>False if the line could not be read. or and EOF was encountered.</returns>
        public static bool ReadLine(ref String sLineData)
        {
            if (!m_bIsFileOpen)
                return false;

            if (m_eAccessType == AccessType.AccessType_ReadOnly)
            {
                if (m_FileReader.EndOfStream)
                {
                    CloseFile();
                    return false;
                }
                sLineData = m_FileReader.ReadLine();
            }
            else if (m_eAccessType == AccessType.AccessType_WriteOnly)
            {
                // cannot read from a write stream.
                return false;
            }

            return true;
        }

        /// <summary>
        /// Attempts to write a string to the open file.
        /// </summary>
        /// <param name="sLineData">The data to write to the file.</param>
        /// <returns>False if the data could not be saved.</returns>
        public static bool SaveLine(String sLineData)
        {
            if (!m_bIsFileOpen)
                return false;

            if (m_eAccessType == AccessType.AccessType_ReadOnly)
            {
                return false;
            }
            else if (m_eAccessType == AccessType.AccessType_WriteOnly)
            {
                m_FileWriter.Write(sLineData);
            }

            return true;
        }

    }
}
