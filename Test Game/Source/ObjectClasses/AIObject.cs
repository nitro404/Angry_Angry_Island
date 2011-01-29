using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firecracker_Engine;

namespace Test_Game
{
    public enum AIWanderType
    {
        AI_Random,
        AI_RandomTowardsNearest,
        AI_Path,
        AI_None,
    }

    public class AIObject : CBaseObject
    {
#pragma warning disable 108
        public const string ClassName = "AIObject";
#pragma warning restore 108

        // Note Loaded
        protected float m_fAge;

        // Loaded.
        protected float m_fMaxAge;
        protected AIWanderType m_eWanderType;


        //-------------------
        public AIObject()
            : base()
        {
        }

        public override bool IsA(string ObjectType)
        {
            if (ObjectType.Equals(ObjectType))
            {
                return true;
            }
            return base.IsA(ObjectType);
        }

        public override void Tick(float fTime)
        {

            base.Tick(fTime);
        }
        public override void LoadPropertiesList(ObjectDefinition objDef)
        {
            base.LoadPropertiesList(objDef);

            if (objDef.ClassProperties.ContainsKey("MaxAge"))
            {
                m_fMaxAge = float.Parse(objDef.ClassProperties["MaxAge"]);
            }
            if (objDef.ClassProperties.ContainsKey("AI_Type"))
            {
                m_eWanderType = (AIWanderType)Helpers.StringToEnum<AIWanderType>(objDef.ClassProperties["AI_Type"]);
            }

        }

        public override void LoadResources()
        {
            base.LoadResources();
        }
    }
}
