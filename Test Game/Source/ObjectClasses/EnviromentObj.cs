using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Firecracker_Engine
{
    class CEnviromentObj : CBaseObject
    {
#pragma warning disable 108
        public const string ClassName = "Environment";
#pragma warning restore 108

        protected int m_Sold;
        protected float m_fMaxAge;
        protected int m_danger;

        public CEnviromentObj()
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
        public override void Tick(GameTime gameTime)
        {

            base.Tick(gameTime);
        }
        public override void LoadPropertiesList(ObjectDefinition objDef)
        {
           base.LoadPropertiesList(objDef);

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
        }

     public override void LoadResources()
        {
            base.LoadResources();
        }

    }
}
