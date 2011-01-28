using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test_Game
{
    class TestGameObject : Firecracker_Engine.CBaseObject
    {

#pragma warning disable 108
        public const string ClassName = "TestGameObject";
#pragma warning restore 108

        public TestGameObject()
            : base()
        {
        }

        public override bool IsA(string ObjectType)
        {
            return base.IsA(ObjectType);
        }

        public override void Tick(float fTime)
        {
            base.Tick(fTime);
        }

        public override void LoadPropertiesList(Firecracker_Engine.ObjectDefinition objDef)
        {
            base.LoadPropertiesList(objDef);
        }

        public override void Render()
        {
            base.Render();
        }
    }
}
