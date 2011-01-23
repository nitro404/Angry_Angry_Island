using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firecracker_Engine
{
    class LevelLoader
    {
        public LevelLoader()
        {
            
        }

        public bool LoadLevel()
        {
            
            // Start loading the level.
            
            // First, make sure the file is open.
            if (!Filesystem.IsFileOpen)
                return false;

            // Next start reading the file one line at a time.
            string currentLine = "";
            Filesystem.ReadLine(ref currentLine);

            while (Filesystem.IsFileOpen)
            {
                if (currentLine.Substring(0,14).CompareTo("BeginObjectDef") == 0)
                {
                    // This is an object definition. Start loading it.
                    LoadObjectDefinition();
                }


                Filesystem.ReadLine(ref currentLine);
            }

            return true;
        }
        
        enum CurrentState
        {
            State_ObjectProperties,
            State_Plugins,
            State_None
        }
        
        private bool LoadObjectDefinition()
        {
            CurrentState eCurrentState = CurrentState.State_ObjectProperties;
            ObjectDefinition NewObjectDef = new ObjectDefinition(false);
            PluginDef NewPluginDef = new PluginDef(false);
            string pluginName = "";
            string sCurrentLine = "";

            while (Filesystem.ReadLine(ref sCurrentLine))
            {
                switch (eCurrentState)
                {
                    case CurrentState.State_ObjectProperties:
                        {
                            if (sCurrentLine.Substring(0, 10).CompareTo("ObjectName") == 0)
                            {
                                NewObjectDef.ObjectClassName = sCurrentLine.Substring(10).Trim(" \t\"\'".ToCharArray());
                            }
                            else if (sCurrentLine.Substring(0, 9).CompareTo("PluginDef") == 0)
                            {
                                pluginName = sCurrentLine.Substring(9).Trim(" \t\"\'".ToCharArray());
                                eCurrentState = CurrentState.State_Plugins;
                            }
                        }
                        break;
                    case CurrentState.State_Plugins:
                        {
                            // Load the plugin information
                            if (sCurrentLine.Substring(0, 12).CompareTo("EndPluginDef") == 0)
                            {
                                NewObjectDef.PluginDefinitionList.Add(pluginName, NewPluginDef);
                                NewPluginDef = new PluginDef();
                                pluginName = "";
                                eCurrentState = CurrentState.State_ObjectProperties;
                            }
                            else
                            {
                                NewPluginDef.PluginParameters.Add(
                                        sCurrentLine.Substring(0, sCurrentLine.IndexOf(' ')),
                                        sCurrentLine.Substring(sCurrentLine.IndexOf('{'), sCurrentLine.LastIndexOf('}'))
                                        );
                            }
                        }
                        break;
                }
                if (sCurrentLine.Substring(0,12).CompareTo("EndObjectDef") == 0)
                {
                    // this is the end of this object definition.
                    // Now load this object;
                }
            }
            return true;
        }
    }

    struct ObjectDefinition
    {
        public ObjectDefinition(bool JustToMakeCSharpHappy)
        {
            this.ObjectClassName = "";
            this.PluginDefinitionList = new Dictionary<string, PluginDef>();
        }
        public string ObjectClassName;

        // list all the saved base object properties here.
        
        // This is the list of plugins that this object will have.
        public Dictionary<string, PluginDef> PluginDefinitionList;
    }

    struct PluginDef
    {
        public PluginDef(bool JustToMakeCSharpHappy)
        {
            this.PluginParameters = new Dictionary<string, string>();
        }
        public Dictionary<string, string> PluginParameters;
    }
}
