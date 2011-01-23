using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Firecracker_Engine
{
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


    public class CBaseObject
    {
        // The properties that every object in the world contains.
        Vector3 m_vWorldPosition;
        Vector3 m_vWorldRotation;
        Vector3 m_vWorldScale;

        CObjectID m_ID;

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
                else if (m_oBaseObject.m_ID != m_ObjectID) return false; // make sure the stored ID is still the same.
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
}
