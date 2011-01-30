using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Firecracker_Engine
{
    class KevinMoose : GameObject
    {
        public enum fightStyle
        {
            lazer,
            roleOver,
            Rage
        }
        public fightStyle CreatureStyle;
        public Sprite Model;
        float SummonTime;
        //what we what to kill
        public GameObject theObj,targetObj;
        public bool target;
        public KevinMoose(Vector2 thePos)
            : base()
        {
            m_position = thePos;
            CreatureStyle = fightStyle.Rage;
            Model = Firecracker.spriteSheets.getSpriteSheet("KevinMoose").getSprite("KevinMoose");
            Model.m_SpriteDepth = 0.4f;
            target = false;
           
        }
        public override void update(GameTime gameTime)
        {
            if (target == false)
            {
                Scearch();
            }
            else if (target == true)
            {
                Hunt(gameTime);
            }
        }

        public void Scearch()
        {
            for (int x = 0; x < Firecracker.level.numberOfObjects(); x++)
            {
                theObj = Firecracker.level.objectAt(x);
                if (theObj.GetType() == typeof(NPCObject))
                {
                    if ((theObj.position - position).Length() < 50)
                    {
                        target = true;
                        targetObj = theObj;
                        break;
                    }
                }
            }

        }
             public void Hunt(GameTime gameTime)
             {
                 //once you find you target kill it
                       
                            if (position.X < theObj.position.X)
                            {
                                position = new Vector2(position.X + 15.0f * (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f, position.Y);
                            }
                            if (position.X > theObj.position.X)
                            {
                                position = new Vector2(position.X - 15.0f * (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f, position.Y);
                            }
                            if (position.Y > theObj.position.Y)
                            {
                                position = new Vector2(position.X, position.Y - 15.0f * (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
                            }
                            if (position.Y < theObj.position.Y)
                            {
                                position = new Vector2(position.X, position.Y + 15.0f * (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
                            }

                            if ((theObj.position - position).Length() < 5.0f)
                            {
                                target = false;
                                theObj.toBeDeleted = true;
                                HumanZap newDeath = new HumanZap(theObj.position);
                                Firecracker.level.addObject(newDeath);
                            }
                        
                    }
             
              

        public override void draw(SpriteBatch spriteBatch)
        {
            Model.draw(spriteBatch, m_scale, m_rotation, m_position, SpriteEffects.None);
        }
    }
}
