using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Firecracker_Engine
{

    //MASSIVE COMMENT!
    /* HOWTO!
     * Follow this guide to create new objects in the engine.
     * Overload this object with your own.
     * i.e. "public class AIObject : CBaseObject { }  
     * Then you MUST include the following line at the top of your class 
     * 
     * #pragma warning disable 108
     *  public const string ClassName = "MoveableObject";
     * #pragma warning restore 108
     * 
     * Replace the class name with your class name
     * Feel free to overload any of the base functionality.
     * If you need to know how it works, reference another object that already exists.
     * 
     * Then go into Firecracker.CreateObject and add another definition for your object.
     * Easy as that...
     * Should work from there.
     */


    /// <summary>
    /// This is the base object class that all game objects are built on.
    /// </summary>
    public class CBaseObject
    {
        // NOTE:Include this line in every child class. This will generate a 
        // CS0108 warning so add the #pragma warning disable/restore 108 around it.
        public const string ClassName = "CBaseObject";

        // The properties that every object in the world contains.
        protected Vector3 m_vWorldPosition;
        protected Vector3 m_vWorldRotation;
        protected Vector3 m_vWorldScale;

        protected string m_sObjectName;
        public string ObjectName { get { return m_sObjectName; } }

        private string m_sObjectType;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="WorldPosition">The objects position relative to 0.0.0</param>
        /// <param name="WorldRotation">The objects rotation</param>
        /// <param name="WorldScale">The objects scale in each axis</param>
        public CBaseObject(Vector3 WorldPosition, Vector3 WorldRotation, Vector3 WorldScale)
        {
            m_vWorldPosition = WorldPosition;
            m_vWorldRotation = WorldRotation;
            m_vWorldScale    = WorldScale;

            // Note: Each subclass to this must have it's own m_sObjectType with that class' type.
            m_sObjectType = "CBaseObject";
        }

        /// <summary>
        /// The default constructor
        /// </summary>
        public CBaseObject()
        : this (new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1))
        {
            
        }

        /// <summary>
        /// This is called once each time the object is rendered. 
        /// This is a generic render function. Overload this if you
        /// need any special rendering done.
        /// </summary>
        public virtual void Render()
        {
            
        }

        public virtual void Render(SpriteBatch spriteBatch)
        {

        }

        /// <summary>
        /// This is called once on each game tick.
        /// This must be overloaded in order for your game objects to do anything.
        /// </summary>
        /// <param name="fTime"></param>
        public virtual void Tick(GameTime gameTime)
        {

        }

        /// <summary>
        /// Checks to see if this object is of a certain type.
        /// Note: This should be done with an RTTI Hierarchy but I believe c# doesn't support that
        /// To overload this properly, call the base IsA then do a check of the local type.
        /// </summary>
        /// <param name="ObjectType">The type of the object</param>
        /// <returns>True if it matches.</returns>
        public virtual bool IsA(string ObjectType)
        {
            if (m_sObjectType.CompareTo(ObjectType) == 0)
                return true;

            return false;

        }

        //Override these.

        /// <summary>
        /// Called once when the gameplay begins
        /// </summary>
        public virtual void OnBeginGameplay() { }

        /// <summary>
        /// Called once when gameplay ends.
        /// </summary>
        public virtual void OnEndGameplay() { }

        /// <summary>
        /// Called when the game is paused, like when the in game pause menu is displayed.
        /// </summary>
        public virtual void OnPauseGameplay() { }

        /// <summary>
        /// This is called when the object is about to be deleted from the game.
        /// Use this to clean up any resources this object uses.
        /// </summary>
        public virtual void OnToBeDeleted() { }


        /// <summary>
        /// This function takes the ObjectDefinition and pulls out all the known 
        /// variables and sets them.
        /// </summary>
        /// <param name="objDef">The object definition</param>
        public virtual void LoadPropertiesList(ObjectDefinition objDef) 
        {

            m_sObjectName = objDef.ObjectName;
            m_sObjectType = objDef.ObjectClassType;

            foreach (KeyValuePair<string, string> propertyItem in objDef.ClassProperties)
            {
                if (propertyItem.Key.CompareTo("WorldPosition") == 0)
                {
                    m_vWorldPosition = Helpers.ParseVector3(propertyItem.Value);
                }
                if (propertyItem.Key.CompareTo("WorldScale") == 0)
                {
                    m_vWorldScale = Helpers.ParseVector3(propertyItem.Value);
                }
                if (propertyItem.Key.CompareTo("WorldRotation") == 0)
                {
                    m_vWorldRotation = Helpers.ParseVector3(propertyItem.Value);
                }
            }
        }

        /// <summary>
        /// Loads the game resources associated with this object.
        /// </summary>
        public virtual void LoadResources()
        {

        }

    }
    
    
}
