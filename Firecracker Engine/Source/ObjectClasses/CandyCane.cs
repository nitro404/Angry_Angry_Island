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
        
        public static float m_fSpeed = 1200.0f;
        public Sprite Candy;
        public bool PingCall;
        public int Life; // makes it pig only 3 time then it dies
        float TimeTracker;
        public CandyCane(Vector2 TargetPosition)
            : base()
        {
            position = new Vector2(TargetPosition.X,TargetPosition.Y);
            Candy = Firecracker.spriteSheets.getSpriteSheet("candyCane").getSprite("candyCane");
            PingCall = false;
            Life = 0;
        }
        public override void update(GameTime gameTime)
        {
           
            if (PingCall == false)
            {
                ping();
                PingCall = true;
                Life++;
            }

            if (TimeTracker > 12.0f)
            {
                TimeTracker = 0.0f;
                PingCall = false;
            }

            TimeTracker += 0.01f;

            if (Life == 3)
            {
                this.toBeDeleted = true;
            }
           
        }
        public void ping()
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
