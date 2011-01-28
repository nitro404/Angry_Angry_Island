using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Firecracker_Engine
{
    class CMoveableObject : CBaseObject
    {
#pragma warning disable 108
        public const string ClassName = "MoveableObject";
#pragma warning restore 108

        Vector3 m_vVelocity;
        Vector3 m_vAcceleration;
        Vector3 m_vRotationalVelocity;
        Vector3 m_vRotationAcceleration;

        public CMoveableObject()
            : base()
        {
            m_vVelocity = new Vector3();
            m_vAcceleration = new Vector3();
            m_vRotationalVelocity = new Vector3();
            m_vRotationAcceleration = new Vector3();
        }

        public override void Tick(float fTime)
        {
            base.Tick(fTime);
            // Move the object.
            m_vWorldPosition += m_vVelocity;

            // Update the velocity.
            m_vVelocity += m_vAcceleration;

            // Rotate the object
            m_vWorldRotation = m_vRotationalVelocity;

            // Update the roational velocity.
            m_vRotationalVelocity += m_vRotationAcceleration;
        }


        public override void OnBeginGameplay()
        {
            base.OnBeginGameplay();
        }

        public override void LoadPropertiesList(ObjectDefinition objDef)
        {
            base.LoadPropertiesList(objDef);

            // TODO: Load this class' values here.
            if (objDef.ClassProperties.ContainsKey("Velocity"))
            {
                m_vVelocity = Helpers.ParseVector3( objDef.ClassProperties["Velocity"] );
            }
            else if (objDef.ClassProperties.ContainsKey("Acceleration"))
            {
                m_vAcceleration = Helpers.ParseVector3( objDef.ClassProperties["Acceleration"]);
            }
            else if (objDef.ClassProperties.ContainsKey("Rotation"))
            {
                m_vAcceleration = Helpers.ParseVector3(objDef.ClassProperties["Rotation"]);
            }
            else if (objDef.ClassProperties.ContainsKey("RotationAccel"))
            {
                m_vAcceleration = Helpers.ParseVector3(objDef.ClassProperties["RotationAccel"]);
            }

        }

    }
}
