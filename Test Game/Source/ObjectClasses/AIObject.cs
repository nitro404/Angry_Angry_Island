using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firecracker_Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;

namespace Angry_Angry_Island
{
    
#if false
    public class AIObject : CBaseObject
    {
#pragma warning disable 108
        public const string ClassName = "AIObject";
#pragma warning restore 108
        

        //-------------------
        public AIObject()
            : base()
        {
        }

        public override bool IsA(string ObjectType)
        {
            if (ObjectType.Equals(ObjectType))
            {
                return true;
            }
            return base.IsA(ObjectType);
        }

        public override void OnBeginGameplay()
        {
            base.OnBeginGameplay();
            //PositionPoint = new Point(200, 200);
            m_bIsMoving = false;
            m_vTargetLocation = new Vector2();
            m_fSpeed = 50.0f;
            m_fWaitTime = 1.0f;
            m_fIdleTime = 0.0f;
        }

        public override void Tick(GameTime gameTime)
        {
            sheepUpdate(gameTime);
            base.Tick(gameTime);
        }
        public override void LoadPropertiesList(ObjectDefinition objDef)
        {
            base.LoadPropertiesList(objDef);

            if (objDef.ClassProperties.ContainsKey("MaxAge"))
            {
                m_fMaxAge = float.Parse(objDef.ClassProperties["MaxAge"]);
            }
            if (objDef.ClassProperties.ContainsKey("AI_Type"))
            {
                m_eWanderType = (AIWanderType)Helpers.StringToEnum<AIWanderType>(objDef.ClassProperties["AI_Type"]);
            }

        }

        public void FleeFromPoint(Vector2 fleePoint)
        {
            m_vTargetLocation = fleePoint - PositionAbsolute;
            if (m_vTargetLocation.Length() <= 50.0f && m_vTargetLocation.Length() >= -50.0f)
            {
                m_vTargetLocation.Normalize();
                m_vTargetLocation *= 100.0f;
                m_vTargetLocation += PositionAbsolute;
                m_fIdleTime = 0.0f;
                m_bIsMoving = true;
            }
        }

        public void sheepUpdate(GameTime gameTime)
        {
            if (!m_bIsMoving)
            {
                m_fIdleTime += (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                if (m_fIdleTime >= m_fWaitTime)
                {
                    m_fIdleTime = 0.0f;
                    m_bIsMoving = true;
                    m_vTargetLocation.X = PositionAbsolute.X + Firecracker.random.Next(-30, 30);
                    m_vTargetLocation.Y = PositionAbsolute.Y + Firecracker.random.Next(-30, 30);
                }
            }
            else
            {
                Vector2 diffVec = m_vTargetLocation - PositionAbsolute;
                if (diffVec.Length() <= 1.0f)
                {
                    m_bIsMoving = false;
                    m_fIdleTime = 0.0f;
                }
                else
                {
                    diffVec.Normalize();
                    diffVec *= m_fSpeed ;
                    PositionAbsolute = PositionAbsolute + (diffVec * ((float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f));
                }
            }

            // This is debug stuff.
            gamePadStatus = GamePad.GetState(PlayerIndex.One);

            if (gamePadStatus.ThumbSticks.Left.X > 0)
            {
                m_Position.X += 10*(float)gameTime.ElapsedRealTime.Milliseconds/1000.0f;
            }
            else if (gamePadStatus.ThumbSticks.Left.X < 0)
            {
                m_Position.X -= 10 * (float)gameTime.ElapsedRealTime.Milliseconds / 1000.0f;
            }
            else if (gamePadStatus.ThumbSticks.Left.Y > 0)
            {
                m_Position.Y -= 10 * (float)gameTime.ElapsedRealTime.Milliseconds / 1000.0f;
            }
            else if (gamePadStatus.ThumbSticks.Left.Y < 0)
            {
                m_Position.Y += 10 * (float)gameTime.ElapsedRealTime.Milliseconds / 1000.0f;
            }
        }

        public override void LoadResources()
        {
            sheep = new Sprite(new Sprite("Sprites/sheep_sheet01", TestGame.gameInstance.Content), new Rectangle(0, 102, 34, 34));
            Sshadow = new Sprite("Sprites/sheep_shadow01", TestGame.gameInstance.Content); 
            base.LoadResources();
        }

        public override void Render()
        {
            TestGame.gameInstance.spriteBatch.Begin();
            Sshadow.draw(TestGame.gameInstance.spriteBatch, new Vector2(1.5f), 0.0f, PositionAbsolute, SpriteEffects.None);
            sheep.draw(TestGame.gameInstance.spriteBatch, Vector2.One, 0.0f, PositionAbsolute, SpriteEffects.None);
            //TestGame.GameInstance.spriteBatch.Draw();
            TestGame.gameInstance.spriteBatch.End();

            base.Render();
        }
    }
#endif
}
