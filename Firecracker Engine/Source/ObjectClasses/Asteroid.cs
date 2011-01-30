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
        public static float m_fSpeed = 1000.0f;

        public Asteroid()
            : base()
        {
            
        }

        public Asteroid(Vector2 TargetPosition) 
            : base()
        {
            m_vTargetPosition = TargetPosition;
            position = m_vTargetPosition + new Vector2(400, -500);

            SpriteSheet spriteSheet = Firecracker.spriteSheets.getSpriteSheet("NPCObject");
            if (spriteSheet != null)
            {
                Sprite sprite = spriteSheet.getSprite("Sheep 6");
                if (sprite != null)
                {
                    m_sprite = sprite;
                }
            }

        }

        public override void update(GameTime gameTime)
        {

            if (Math.Abs((position - m_vTargetPosition).Length()) <= 10)
            {
                // Asplode.
                Explosion exp = new Explosion(position + new Vector2(16.0f, 16.0f));
                Firecracker.level.addObject(exp);
                this.toBeDeleted = true;

                // find all the NPCs nearby and kill 'em all. MUAUAhahah.....ha
                for (int i = 0; i < Firecracker.level.numberOfObjects(); i++)
                {
                    GameObject theObj = Firecracker.level.objectAt(i);
                    if (theObj.GetType() == typeof(NPCObject))
                    {
                        if ((theObj.position - position).Length() < 150)
                        {
                            // kill this guy.
                            theObj.toBeDeleted = true;
                            HumanDeath newDeath = new HumanDeath(theObj.position);
                            Firecracker.level.addObject(newDeath);
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
            base.draw(spriteBatch);
        }
    }
}
