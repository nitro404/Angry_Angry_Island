using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Firecracker_Engine
{
    public class Asteroid : GameObject
    {

        public Vector2 m_vTargetPosition;
        public static float m_fSpeed = 1200.0f;

        public Asteroid()
            : base()
        {
            
        }

        public Asteroid(Vector2 TargetPosition) 
            : base()
        {
            m_vTargetPosition = TargetPosition;
            position = m_vTargetPosition + new Vector2(400, -500);

            SpriteSheet spriteSheet = Firecracker.spriteSheets.getSpriteSheet("Asteroid");
            if (spriteSheet != null)
            {
                Sprite sprite = spriteSheet.getSprite("Asteroid");
                if (sprite != null)
                {
                    m_sprite = sprite;
                    m_sprite.m_SpriteDepth = 0.1f;
                }
            }

        }

        public override void update(GameTime gameTime)
        {

            if (Math.Abs((position - m_vTargetPosition).Length()) <= 20)
            {
                // Asplode.
                BigExplosion exp = new BigExplosion(position);
                Firecracker.level.addObject(exp);
                this.toBeDeleted = true;

                // find all the NPCs nearby and kill 'em all. MUAUAhahah.....ha
                for (int i = 0; i < Firecracker.level.numberOfObjects(); i++)
                {
                    GameObject theObj = Firecracker.level.objectAt(i);
                    if (theObj.GetType() == typeof(NPCObject))
                    {
                        if ((theObj.position - position).Length() < 120)
                        {
                            // kill this guy.
                            theObj.toBeDeleted = true;
                            HumanDeath newDeath = new HumanDeath(theObj.position);
                            Firecracker.level.addObject(newDeath);
                            if ((theObj.position - position).Length() < 60)
                            {
                                Explosion explosionDeath = new Explosion(theObj.position);
                                Firecracker.level.addObject(explosionDeath);
                            }
                        }
                        if ((theObj.position - position).Length() < 180)
                        {
                            NPCObject person = (NPCObject)theObj;
                            person.wanderType = AIWanderType.AI_Scatter;
                            person.PointOfTerror = new Vector2(position.X, position.Y);
                            person.eventTime = 1.2f;
                            person.FleeFromPoint(person.PointOfTerror);
                        }

                    }
                }
            }
            else
            {
                // Fly in
                Vector2 newPos = m_vTargetPosition - position;
                newPos.Normalize();
                newPos *= (float)(m_fSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                position += newPos;
            }

            base.update(gameTime);
        }

        public override void draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            float scale = (position.X - m_vTargetPosition.X + 100)/500;
            m_sprite.drawWithOffset(spriteBatch, new Vector2(scale, scale), 0, position, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, new Vector2(45, 270));
        }
    }
}
