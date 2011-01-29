using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firecracker_Engine;

namespace Test_Game
{
    /// <summary>
    /// This class extends the functionality of the Firecracker_Engine.LevelLoaderObjectTypeList function
    /// This must be included in each game project.
    /// </summary>
    public static class ObjectDefinitions
    {
        /// <summary>
        /// On creation initialize the delegate.
        /// </summary>
        static ObjectDefinitions()
        {
        }

        public static void Initialize()
        {
            Firecracker.CreateObjectDelegate = new CreateObjectDelegate(CreateObject);
        }

        /// <summary>
        /// Attempt to create the object given then definition.
        /// </summary>
        /// <param name="objDef">The object definition as loaded.</param>
        /// <returns>A downcasted CBaseObject, or null if the object is not a game defined type.</returns>
        public static CBaseObject CreateObject(ObjectDefinition objDef, ObjectDefinition objOverwriteDefinition)
        {
            CBaseObject returnObject = null;
            switch (objDef.ObjectClassType)
            {
                case TestGameObject.ClassName:
                    {
                        TestGameObject newObject = new TestGameObject();
                        newObject.LoadPropertiesList(objDef);
                        newObject.LoadPropertiesList(objOverwriteDefinition);
                        returnObject = newObject;
                    }
                    break;
                case Player.ClassName:
                    {
                        Player newObject = new Player();
                        newObject.LoadPropertiesList(objDef);
                        newObject.LoadPropertiesList(objOverwriteDefinition);
                        returnObject = newObject;
                    }
                    break;
                case AIObject.ClassName:
                    {
                        AIObject newObject = new AIObject();
                        newObject.LoadPropertiesList(objDef);
                        newObject.LoadPropertiesList(objOverwriteDefinition);
                        returnObject = newObject;
                    }
                    break;
            }
            return returnObject;
        }
    }
}
