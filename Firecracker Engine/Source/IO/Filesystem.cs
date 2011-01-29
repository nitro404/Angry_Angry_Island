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
        private static List<string> m_lFilesToOpen = new List<string>();
        private static bool m_bIsDirectoryOpen = false;
        private static string m_sCurrentFolder = "";

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
                        return true;
                    }
                case AccessType.AccessType_WriteOnly:
                    {
                        m_FileWriter = new StreamWriter(sFilename);
                        m_bIsFileOpen = true;
                        m_eAccessType = eAccessType;
                        return true;
                    }
                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// Transparently opens every file in a folder one by one so that you can read from each file.
        /// When an EOF is encountered the file is closed and the next one is opened.
        /// </summary>
        /// <param name="sFolderName">The name of the folder to open</param>
        /// <param name="sFileTypes">The extention of the files to open. Only files with this extention will be accessed. Use "*.txt" for example</param>
        /// <returns>True if the operation was successful</returns>
        public static bool OpenFolder(string sFolderName, string sFileTypes)
        {
            m_lFilesToOpen = new List<string>();
            m_sCurrentFolder = string.Concat(Directory.GetCurrentDirectory(), "\\", sFolderName);

            DirectoryInfo DirInfo = new DirectoryInfo(m_sCurrentFolder);
            FileInfo[] fInfo = DirInfo.GetFiles(sFileTypes);

            for (int i = 0; i < fInfo.Length; i++ )
            {
                m_lFilesToOpen.Add(fInfo[i].Name);
            }

            if (m_lFilesToOpen.Count != 0)
            {
                if (!OpenFile( string.Concat("Content\\Objects\\", m_lFilesToOpen[0]), AccessType.AccessType_ReadOnly))
                {
                    CloseFolder();
                    return false;
                }
                m_lFilesToOpen.RemoveAt(0);
            }
            else
            {
                return false;
            }

            m_bIsDirectoryOpen = true;
            return true;
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

        public static void CloseFolder()
        {
            CloseFile();
            m_bIsDirectoryOpen = false;
            m_sCurrentFolder = "";
            m_lFilesToOpen = new List<string>();
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
                    if (m_bIsDirectoryOpen )
                    {
                        if (m_lFilesToOpen.Count == 0)
                        {
                            CloseFolder();
                            return false;
                        }
                        else
                        {

                            if (!OpenFile(m_lFilesToOpen[0], AccessType.AccessType_ReadOnly))
                                return false;
                            else
                            {
                                m_lFilesToOpen.RemoveAt(0);
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
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

        enum CurrentState
        {
            State_ObjectProperties,
            State_Plugins,
            State_None
        }

        public static ObjectDefinition LoadObjectDefinition()
        {
            CurrentState eCurrentState = CurrentState.State_ObjectProperties;
            ObjectDefinition NewObjectDef = new ObjectDefinition(false);
            PluginDef NewPluginDef = new PluginDef(false);
            string sCurrentLine = "";
            char[] trimString = " \t\"\'".ToCharArray();

            while (Filesystem.ReadLine(ref sCurrentLine))
            {
                if (sCurrentLine.Trim(" \t\"\'".ToCharArray()).Length == 0)
                    continue;
                // Skip any line with a #. It's a comment.
                if (sCurrentLine.StartsWith("#"))
                    continue;

                switch (eCurrentState)
                {
                    case CurrentState.State_ObjectProperties:
                        {
                            if (sCurrentLine.Substring(0, Math.Min(sCurrentLine.Length, 10)).CompareTo("ObjectName") == 0)
                            {
                                NewObjectDef.ObjectName = sCurrentLine.Substring(10).Trim(trimString);
                            }
                            else if (sCurrentLine.Substring(0, Math.Min(sCurrentLine.Length,11)).CompareTo("ObjectClass") == 0)
                            {
                                NewObjectDef.ObjectClassType = sCurrentLine.Substring(11).Trim(trimString);
                            }
                            //else if (sCurrentLine.Substring(0, Math.Min(sCurrentLine.Length, 9)).CompareTo("PluginDef") == 0)
                            //{
                            //    pluginName = sCurrentLine.Substring(9).Trim(trimString);
                            //    eCurrentState = CurrentState.State_Plugins;
                            //}
                            else if (sCurrentLine.CompareTo("EndObjectDef") == 0)
                            {
                                // this is the end of this object definition.
                                return NewObjectDef;
                            }
                            else
                            {
                                string param = sCurrentLine.Substring(0, sCurrentLine.IndexOfAny(trimString));
                                string value = sCurrentLine.Substring(sCurrentLine.IndexOfAny(trimString)).Trim(trimString);
                                int leftCount = Helpers.CountStringOccurrences(value, "{");
                                int rightCount = Helpers.CountStringOccurrences(value, "}");
                                if (leftCount > rightCount)
                                {
                                    while (Filesystem.ReadLine(ref sCurrentLine))
                                    {
                                        value = string.Concat(value, sCurrentLine.Trim(trimString));
                                        leftCount = Helpers.CountStringOccurrences(value, "{");
                                        rightCount = Helpers.CountStringOccurrences(value, "}");
                                        if (leftCount == rightCount)
                                            break;
                                    }
                                }
                                // If it is none of the above then it is a class property.
                                NewObjectDef.ClassProperties.Add(
                                    param,
                                    value
                                    );
                            }
                        }
                        break;
                    /*case CurrentState.State_Plugins:
                        {
                            // Load the plugin information
                            if (sCurrentLine.CompareTo("EndPluginDef") == 0)
                            {
                                NewObjectDef.PluginDefinitionList.Add(pluginName, NewPluginDef);
                                NewPluginDef = new PluginDef(false);
                                pluginName = "";
                                eCurrentState = CurrentState.State_ObjectProperties;
                            }
                            else
                            {
                                NewPluginDef.PluginParameters.Add(
                                        sCurrentLine.Substring(0, sCurrentLine.IndexOf(' ')),
                                        sCurrentLine.Substring(sCurrentLine.IndexOf('{')+1, sCurrentLine.LastIndexOf('}') - (sCurrentLine.IndexOf('{')+1) )
                                        );
                            }
                        }
                        break;*/
                }
            }
            return NewObjectDef;
        }

        public static ObjectDefinition LoadObjectDescription()
        {
            CurrentState eCurrentState = CurrentState.State_ObjectProperties;
            ObjectDefinition NewObjectDef = new ObjectDefinition(false);
            PluginDef NewPluginDef = new PluginDef(false);
            string sCurrentLine = "";
            char[] trimString = " \t\"\'".ToCharArray();

            while (Filesystem.ReadLine(ref sCurrentLine))
            {
                if (sCurrentLine.Trim(" \t\"\'".ToCharArray()).Length == 0)
                    continue;
                // Skip any line with a #. It's a comment.
                if (sCurrentLine.StartsWith("#"))
                    continue;

                switch (eCurrentState)
                {
                    case CurrentState.State_ObjectProperties:
                        {
                            if (sCurrentLine.Substring(0, Math.Min(sCurrentLine.Length, 10)).CompareTo("ObjectName") == 0)
                            {
                                NewObjectDef.ObjectName = sCurrentLine.Substring(10).Trim(trimString);
                            }
                            else if (sCurrentLine.Substring(0, Math.Min(sCurrentLine.Length, 9)).CompareTo("ObjectDef") == 0)
                            {
                                NewObjectDef.ObjectClassType = sCurrentLine.Substring(9).Trim(trimString);
                            }
                            else if (sCurrentLine.CompareTo("EndObjectDef") == 0)
                            {
                                // this is the end of this object definition.
                                return NewObjectDef;
                            }
                            else
                            {
                                string param = sCurrentLine.Substring(0, sCurrentLine.IndexOfAny(trimString));
                                string value = sCurrentLine.Substring(sCurrentLine.IndexOfAny(trimString)).Trim(trimString);
                                int leftCount = Helpers.CountStringOccurrences(value, "{");
                                int rightCount = Helpers.CountStringOccurrences(value, "}");
                                if (leftCount > rightCount)
                                {
                                    while (Filesystem.ReadLine(ref sCurrentLine))
                                    {
                                        value = string.Concat(value, sCurrentLine);
                                        leftCount = Helpers.CountStringOccurrences(value, "{");
                                        rightCount = Helpers.CountStringOccurrences(value, "}");
                                        if (leftCount == rightCount)
                                            break;
                                    }
                                }
                                // If it is none of the above then it is a class property.
                                NewObjectDef.ClassProperties.Add(
                                    param,
                                    value
                                    );
                            }
                        }
                        break;
                }
            }
            return NewObjectDef;
        }
    }
}
