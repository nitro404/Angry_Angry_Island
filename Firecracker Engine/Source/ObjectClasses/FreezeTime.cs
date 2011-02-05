using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Firecracker_Engine
{
    class FreezeTime :GameObject
    {
        public static float m_fSpeed = 1200.0f;
        float TimeTracker;
        public Sprite SnowMan;
        public Sprite Ice;
        public List<Vector2> peopleThatAreFozen;
        public FreezeTime(Vector2 TargetPosition)
            : base()
        {
            position = new Vector2(TargetPosition.X, TargetPosition.Y);
            SnowMan = Firecracker.spriteSheets.getSpriteSheet("FreezeTime").getSprite("FreezeTime");
            Ice = Firecracker.spriteSheets.getSpriteSheet("Ice").getSprite("Ice");
            peopleThatAreFozen = new List<Vector2>();

        }
        public override void update(GameTime gameTime)
        {
            Vector2 temp = new Vector2();
            for (int i = 0; i < Firecracker.level.numberOfObjects(); i++)
            {
                GameObject theObj = Firecracker.level.objectAt(i);
                if (theObj.GetType() == typeof(NPCObject))
                {
                    if ((theObj.position - position).Length() < 80)
                    {

                        NPCObject person = (NPCObject)theObj;

                        person.wanderType = AIWanderType.AI_Freeze;
                        person.eventTime = 1.2f;
                        temp = person.position;
                       
                    }
                    peopleThatAreFozen.Add(temp);
                }
            }
            TimeTracker += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (TimeTracker > 14.0f)
            {
                this.toBeDeleted = true;
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            foreach(Vector2 location in peopleThatAreFozen)
            {
                Ice.draw(spriteBatch, m_scale, m_rotation, location, SpriteEffects.None); 
           }
            SnowMan.draw(spriteBatch, m_scale, m_rotation, m_position, SpriteEffects.None);
           
        }
    }
}
