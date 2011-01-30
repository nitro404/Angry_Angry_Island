using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firecracker_Engine
{

    public static class LevelLoader
    {

        static LevelLoader()
        {

        }

        public static bool LoadLevel(string levelName)
        {

            // Start loading the level.

            // First, make sure the file is open.
            Filesystem.OpenFile(levelName, Filesystem.AccessType.AccessType_ReadOnly);

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
                    ObjectDefinition newObjectDefinition = Filesystem.LoadObjectDescription();
                    if (newObjectDefinition.ObjectClassType.Length != 0)
                    {
                        Firecracker.engineInstance.CreateObjectByDefinition(newObjectDefinition);
                    }
                }
                //else if ( ... Add other loadable types here for example, a cutscene def or a set of physics edges... )


                Filesystem.ReadLine(ref currentLine);
            }
            

            // The file will close itself when it hits the EOF so we don't need to worry about cleanup.
            return true;
        }

    }
    /// <summary>
    /// This is the ObjectDefinition.
    /// It is used to store all the loaded information from the level file
    /// before it is transformed into an actual object in the engine.
    /// </summary>
    public struct ObjectDefinition
    {
        public string ObjectClassType;
        public string ObjectName;

        // list all the saved object properties here.
        public Dictionary<string, string> ClassProperties;
        
        // This is the list of plugins that this object will have.
        //public Dictionary<string, PluginDef> PluginDefinitionList;



        public ObjectDefinition(bool JustToMakeCSharpHappy)
        {
            this.ObjectClassType = "";
            this.ObjectName = "";
            //this.PluginDefinitionList = new Dictionary<string, PluginDef>();
            ClassProperties = new Dictionary<string, string>();
        }

    }

    /// <summary>
    /// Stores the loaded information for a plugin.
    /// NOTE: Plugin support is temporarily suspended until further notice.
    /// </summary>
    public struct PluginDef
    {
        public PluginDef(bool JustToMakeCSharpHappy)
        {
            this.PluginParameters = new Dictionary<string, string>();
        }
        public Dictionary<string, string> PluginParameters;
    }
}
