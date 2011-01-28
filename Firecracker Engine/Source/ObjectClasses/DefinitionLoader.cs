using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firecracker_Engine
{
    public static class DefinitionManager
    {
        private static List<ObjectDefinition> m_lObjectDefinitionList = new List<ObjectDefinition>();

        static DefinitionManager()
        {

        }

        public static bool LoadDefinitions(string sDefinitionFolder)
        {
            // Load all the definitions.

            // First, make sure the file is open.
            Filesystem.OpenFolder(sDefinitionFolder, "*.DEF");

            // Next start reading the file one line at a time.
            string currentLine = "";
            Filesystem.ReadLine(ref currentLine);

            while (Filesystem.IsFileOpen)
            {
                // Skip any line with a #. It's a comment.
                if (currentLine.StartsWith("#"))
                    continue;

                if (currentLine.Substring(0, Math.Min(currentLine.Length, 14)).CompareTo("BeginObjectDef") == 0)
                {
                    // This is an object definition. Start loading it.
                    ObjectDefinition newObjectDefinition = Filesystem.LoadObjectDefinition();
                    if (newObjectDefinition.ObjectClassType.Length != 0)
                    {
                        m_lObjectDefinitionList.Add(newObjectDefinition);
                        newObjectDefinition = new ObjectDefinition();
                    }
                }
                //else if ( ... Add other loadable types here for example, a cutscene def or a set of physics edges... )


                Filesystem.ReadLine(ref currentLine);
            }

            // The file will close itself when it hits the EOF so we don't need to worry about cleanup.
            return true;
        }

        public static ObjectDefinition QueryDefinitionByName(string sDefinitionName)
        {
            for (int i = 0; i < m_lObjectDefinitionList.Count; i++)
            {
                if (m_lObjectDefinitionList[i].ObjectName.Equals(sDefinitionName))
                {
                    return m_lObjectDefinitionList[i];
                }
            }
            return new ObjectDefinition();
        }
    }
}
