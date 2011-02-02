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
        public FreezeTime(Vector2 TargetPosition)
            : base()
        {
            position = new Vector2(TargetPosition.X, TargetPosition.Y);
            SnowMan = Firecracker.spriteSheets.getSpriteSheet("FreezeTime").getSprite("FreezeTime");

        }
        public override void update(GameTime gameTime)
        {
            for (int i = 0; i < Firecracker.level.numberOfObjects(); i++)
            {
                GameObject theObj = Firecracker.level.objectAt(i);
                if (theObj.GetType() == typeof(NPCObject))
                {
                    if ((theObj.position - position).Length() < 120)
                    {

                        NPCObject person = (NPCObject)theObj;

                        person.wanderType = AIWanderType.AI_Freeze;
                        person.PointOfTerror = new Vector2(position.X, position.Y);
                        person.eventTime = 1.2f;
                        person.SwarmPoint(person.PointOfTerror);
                    }
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
            SnowMan.draw(spriteBatch, m_scale, m_rotation, m_position, SpriteEffects.None);
        }
    }
}
