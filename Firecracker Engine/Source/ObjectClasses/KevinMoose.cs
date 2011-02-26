﻿using System;
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
        float timeLived = 0;
        //what we what to kill
        public GameObject theObj,targetObj;
        public bool target, eating, Die;
        public SpriteAnimation Moose, MooseWalk,MooseDie; 
        public KevinMoose(Vector2 thePos)
            : base()
        {
            m_position = thePos;
            CreatureStyle = fightStyle.Rage;
            Moose = Firecracker.animations.getAnimation("KevinMoose");
            MooseWalk = Firecracker.animations.getAnimation("KevinMooseWalk");
            MooseDie = Firecracker.animations.getAnimation("KevinMooseDie");

            
            Moose.sprite.m_SpriteDepth = 0.4f;

            //Moose.SetAnimDepthLayer(0.4f);
            //MooseWalk.SetAnimDepthLayer(0.4f);
            //MooseDie.SetAnimDepthLayer(0.4f);

            target = false;
            eating = false;
            Die = false;
            
        }
        public override void update(GameTime gameTime)
        {
            
          
            if (target == false)
            {
                Search(gameTime);
            }
            else if (target == true)
            {
                Hunt(gameTime);
            }

            if (eating == true)
            {
                Moose.update(gameTime);
            }
            else if(Die == false)
            {
                MooseWalk.update(gameTime);
            }

            if (Moose.finished())
            {
                target = false;
                theObj.toBeDeleted = true;
                eating = false;
                Moose.reset();
            }
            //hacked timer
            timeLived += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeLived > 50f)
            {
                Die = true;
                MooseDie.update(gameTime);
                if (MooseDie.finished())
                {
                    this.toBeDeleted = true;
                }
            }
            
        }

        public void Search(GameTime gameTime)
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
            wander(gameTime);
            

        }
             public void Hunt(GameTime gameTime)
             {
                 //once you find you target kill it
                       
                            if (position.X < theObj.position.X)
                            {
                                position = new Vector2(position.X + 40.0f * (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f, position.Y);
                            }
                            if (position.X > theObj.position.X)
                            {
                                position = new Vector2(position.X - 40.0f * (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f, position.Y);
                            }
                            if (position.Y > theObj.position.Y)
                            {
                                position = new Vector2(position.X, position.Y - 40.0f * (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
                            }
                            if (position.Y < theObj.position.Y)
                            {
                                position = new Vector2(position.X, position.Y + 40.0f * (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
                            }

                            if ((theObj.position - position).Length() < 0.5f)
                            {
                                //Moose.update(gameTime);
                                eating = true;
                            }
                            if (theObj.toBeDeleted)
                            {
                                target = false;
                            }
                        
                    }
             
              public void wander(GameTime gameTime)
              {
                  int derection =Firecracker.random.Next(0, 4);
                  switch (derection)
                  {
                      case 1:
                            position = new Vector2(position.X + 20.0f * (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f, position.Y);
                          break;
                      case 2:
                          position = new Vector2(position.X - 20.0f * (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f, position.Y);
                          break;
                      case 3:
                         position = new Vector2(position.X, position.Y - 20.0f * (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
                          break;
                      case 4:
                          position = new Vector2(position.X, position.Y + 20.0f * (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
                          break;
                  }
              }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (eating == true)
            {
                Moose.drawCentered(spriteBatch, m_scale, m_rotation, m_position, SpriteEffects.None);
            }
            else if (Die == false) 
            {
				MooseWalk.drawCentered(spriteBatch, m_scale, m_rotation, m_position, SpriteEffects.None);
            }
            else if (Die == true)
            {
				MooseDie.drawCentered(spriteBatch, m_scale, m_rotation, m_position, SpriteEffects.None);
            }
        }
    }
}
