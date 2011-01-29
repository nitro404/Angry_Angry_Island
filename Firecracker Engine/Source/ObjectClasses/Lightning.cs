using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine
{
    public class Lightning : GameObject
    {
        SpriteAnimation m_Anim;

        public Lightning()
            : base()
        {
			m_Anim = Firecracker.animations.getAnimation("Lightning");
            position = Vector2.Zero;
        }

        public Lightning(Vector2 vPosition)
            : base()
        {
			m_Anim = Firecracker.animations.getAnimation("Lightning");
            position = vPosition;
        }

        public override void update(GameTime gameTime)
        {
            m_Anim.update(gameTime);
            if (m_Anim.finished()) toBeDeleted = true;

            // check the collisions.
            for (int i = 0; i < Firecracker.level.numberOfObjects(); i++)
            {
                GameObject theObj = Firecracker.level.objectAt(i);
                if (theObj.GetType() == typeof(NPCObject))
                {
                    if ((theObj.position - position).Length() < 10)
                    {
                        // kill this guy.
                        theObj.toBeDeleted = true;
                        HumanZap newDeath = new HumanZap(theObj.position);
                        Firecracker.level.addObject(newDeath);
                    }
                }
            }

            base.update(gameTime);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            m_Anim.draw(spriteBatch, Vector2.One, 0, position, SpriteEffects.None);
            base.draw(spriteBatch);
        }
    }
}
