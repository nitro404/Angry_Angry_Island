using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Firecracker_Engine
{
    /// <summary>
    /// The objectID class is meant to be a handy way to reference objects without the need for a 
    /// direct reference.
    /// </summary>
    public class CObjectID
    {
        int m_ID;
        static int m_CurrentMaxID = 0;

        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public CObjectID()
        {
            m_ID = 0;
        }

        public void SetNull() { m_ID = 0; }

        public void AssignNewID()
        {
            // Only assign a new ID if there is not one already.
            // We may decide to change that behaviour later.
            if (m_ID != 0) return;

            m_CurrentMaxID++;
            m_ID = m_CurrentMaxID;
        }

        public static bool operator ==(CObjectID val1, CObjectID val2)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(val1, val2) )
                return true;

            // If one is null, but not both, return false.
            if (((object)val1 == null) || ((object)val2 == null))
            {
                return false;
            }

            // Return true if the fields match:
            return (val1.m_ID == val2.m_ID);
        }
        public static bool operator !=(CObjectID val1, CObjectID val2)
        {
            return !(val1.m_ID==val2.m_ID);
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

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

        CObjectID m_ID;

        private string m_sObjectType;

        public CObjectID ID
        {
            get { return m_ID; }
            set 
            {
                //TODO: Cyril: only set the ID if it is not already taken.
                m_ID = value;
            }
        }

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

        /// <summary>
        /// This is called once on each game tick.
        /// This must be overloaded in order for your game objects to do anything.
        /// </summary>
        /// <param name="fTime"></param>
        public virtual void Tick(float fTime)
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
            foreach (KeyValuePair<string, string> propertyItem in objDef.ClassProperties)
            {
                if (propertyItem.Key.CompareTo("WorldPosition") == 0)
                {
                    m_vWorldPosition = Helpers.ParseVector3(propertyItem.Value);
                }
                else if (propertyItem.Key.CompareTo("WorldScale") == 0)
                {
                    m_vWorldScale = Helpers.ParseVector3(propertyItem.Value);
                }
                else if (propertyItem.Key.CompareTo("WorldRotation") == 0)
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
    
    /// <summary>
    /// Object proxy.
    /// Holds a reference to a game object so that you don't have to search for it.
    /// Also keeps track of wether the object has been destroyed.
    /// </summary>
    class CObjectProxy
    {
        CObjectID m_ObjectID;
        CBaseObject m_oBaseObject;

        public CObjectProxy()
        {
            m_ObjectID = new CObjectID();
            m_ObjectID.SetNull();
            m_oBaseObject = null;
        }

        public bool IsValid()
        {
            if ((object)m_oBaseObject == null) return false; // make sure the object still exists.
            else if (m_oBaseObject.ID != m_ObjectID) return false; // make sure the stored ID is still the same.
            else return true;
        }

        public CBaseObject GetObject()
        {
            if ((object)m_oBaseObject == null) return null;
            else return m_oBaseObject;
        }

        public void SetObjectReference(ref CBaseObject BaseObject)
        {
            if (BaseObject == null)
                return;
            m_oBaseObject = BaseObject;
            m_ObjectID.ID = BaseObject.ID.ID;
        }
    }
}
