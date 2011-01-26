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
                if (currentLine.Substring(0,Math.Min(currentLine.Length,14)).CompareTo("BeginObjectDef") == 0)
                {
                    // This is an object definition. Start loading it.
                    LoadObjectDefinition();
                }

                Filesystem.ReadLine(ref currentLine);
            }

            // The file will close itself when it hits the EOF so we don't need to worry about cleanup.
            return true;
        }
        
        enum CurrentState
        {
            State_ObjectProperties,
            State_Plugins,
            State_None
        }

        private static bool LoadObjectDefinition()
        {
            CurrentState eCurrentState = CurrentState.State_ObjectProperties;
            ObjectDefinition NewObjectDef = new ObjectDefinition(false);
            PluginDef NewPluginDef = new PluginDef(false);
            string pluginName = "";
            string sCurrentLine = "";

            while (Filesystem.ReadLine(ref sCurrentLine))
            {
                if (sCurrentLine.Trim(" \t\"\'".ToCharArray()).Length == 0)
                    continue;

                switch (eCurrentState)
                {
                    case CurrentState.State_ObjectProperties:
                        {
                            if (sCurrentLine.Substring(0, Math.Min(sCurrentLine.Length, 10)).CompareTo("ObjectName") == 0)
                            {
                                NewObjectDef.ObjectName = sCurrentLine.Substring(10).Trim(" \t\"\'".ToCharArray());
                            }
                            //else if (sCurrentLine.Substring(0, Math.Min(sCurrentLine.Length,10)).CompareTo("ObjectType") == 0)
                            //{
                            //    NewObjectDef.ObjectClassType = sCurrentLine.Substring(10).Trim(" \t\"\'".ToCharArray());
                            //}
                            else if (sCurrentLine.Substring(0, Math.Min(sCurrentLine.Length,11)).CompareTo("ObjectClass") == 0)
                            {
                                NewObjectDef.ObjectClassType = sCurrentLine.Substring(11).Trim(" \t\"\'".ToCharArray());
                            }
                            else if (sCurrentLine.Substring(0, Math.Min(sCurrentLine.Length, 9)).CompareTo("PluginDef") == 0)
                            {
                                pluginName = sCurrentLine.Substring(9).Trim(" \t\"\'".ToCharArray());
                                eCurrentState = CurrentState.State_Plugins;
                            }
                            else if (sCurrentLine.CompareTo("EndObjectDef") == 0)
                            {
                                // this is the end of this object definition.
                                // Now load this object;

                                GlobalFirecrackerRef.Instance.AddObjectToList(LevelLoaderObjectTypeList.CreateObject(NewObjectDef));
                                return true;
                            }
                            else
                            {
                                // If it is none of the above then it is a class property.
                                NewObjectDef.ClassProperties.Add(
                                    sCurrentLine.Substring(0, sCurrentLine.IndexOfAny(" \t".ToCharArray())),
                                    sCurrentLine.Substring(sCurrentLine.IndexOfAny(" \t".ToCharArray())).Trim(" \t\"\'".ToCharArray())
                                    );
                            }
                        }
                        break;
                    case CurrentState.State_Plugins:
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
                        break;
                }
            }
            return true;
        }
    }

    public static class LevelLoaderObjectTypeList
    {
        public static CBaseObject CreateObject(ObjectDefinition objDef)
        {
            // This is a list of all the object types in the engine. 
            // To complete this list with the GameDefined types overload
            // This class and this method in the game.
            CBaseObject returnObject = null;
            switch(objDef.ObjectClassType)
            {
                case CBaseObject.ClassName:
                    {
                        CBaseObject newObject = new CBaseObject();
                        newObject.LoadPropertiesList(objDef);
                        returnObject = newObject;
                    }
                    break;
                case CMoveableObject.ClassName:
                    {
                        CMoveableObject newObject = new CMoveableObject();
                        newObject.LoadPropertiesList(objDef);
                        returnObject = newObject;
                    }
                    break;
                default:
                    return null;
            }

            return returnObject;
        }
    }
    
    public struct ObjectDefinition
    {
        public string ObjectClassType;
        public string ObjectName;

        // list all the saved object properties here.
        public Dictionary<string, string> ClassProperties;
        
        // This is the list of plugins that this object will have.
        public Dictionary<string, PluginDef> PluginDefinitionList;



        public ObjectDefinition(bool JustToMakeCSharpHappy)
        {
            this.ObjectClassType = "";
            this.ObjectName = "";
            this.PluginDefinitionList = new Dictionary<string, PluginDef>();
            ClassProperties = new Dictionary<string, string>();
        }

    }

    public struct PluginDef
    {
        public PluginDef(bool JustToMakeCSharpHappy)
        {
            this.PluginParameters = new Dictionary<string, string>();
        }
        public Dictionary<string, string> PluginParameters;
    }
}
