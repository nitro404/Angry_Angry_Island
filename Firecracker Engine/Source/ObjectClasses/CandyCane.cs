using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine
{
    class CandyCane : GameObject
    {
        
        public Vector2 m_vTargetPosition;
        public static float m_fSpeed = 1200.0f;
        public Sprite Candy;
        public CandyCane(Vector2 TargetPosition)
            : base()
        {
            position = new Vector2(TargetPosition.X,TargetPosition.Y);
            Candy = Firecracker.spriteSheets.getSpriteSheet("candyCane").getSprite("candyCane");   
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
                        person.wanderType = AIWanderType.AI_drawn;
                        person.PointOfTerror = new Vector2(position.X, position.Y);
                        person.eventTime = 1.2f;
                        person.SwarmPoint(person.PointOfTerror);
                    }
                }
            }
        }
        public override void draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Candy.draw(spriteBatch, m_scale, m_rotation, m_position, SpriteEffects.None);
        }
    }
}
