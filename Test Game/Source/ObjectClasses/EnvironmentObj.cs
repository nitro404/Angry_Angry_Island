using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firecracker_Engine;

namespace Test_Game
{
    class CEnvironmentObj :CBaseObject
    {

#pragma warning disable 108
        public const string ClassName = "Environment";
#pragma warning restore 108

        protected int m_Sold;
        protected float m_fMaxAge;
        protected int m_danger;
        protected String file;

        public CEnvironmentObj()
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
            //doesnt load anymore
            if (objDef.ClassProperties.ContainsKey("MaxAge"))
            {
                m_fMaxAge = float.Parse(objDef.ClassProperties["MaxAge"]);
            }
            if (objDef.ClassProperties.ContainsKey("Sold"))
            {
                m_Sold = int.Parse(objDef.ClassProperties["Sold"]);
            }
            if (objDef.ClassProperties.ContainsKey("Danger"))
            {
                m_danger = int.Parse(objDef.ClassProperties["Danger"]);
            }
            if (objDef.ClassProperties.ContainsKey("Sprite"))
            {
                file = objDef.ClassProperties["Sprite"];
            }
        }

     public override void LoadResources()
        {
            base.LoadResources();
        }


    }
}
