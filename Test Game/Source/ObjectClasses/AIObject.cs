using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firecracker_Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;

namespace Test_Game
{
    public enum AIWanderType
    {
        AI_Random,
        AI_RandomTowardsNearest,
        AI_Path,
        AI_None,
    }

    public class AIObject : CBaseObject
    {
#pragma warning disable 108
        public const string ClassName = "AIObject";
#pragma warning restore 108

        // Note Loaded
        protected float m_fAge;

        // Loaded.
        protected float m_fMaxAge;
        protected AIWanderType m_eWanderType;

        public Sprite sheep,Sshadow;
        public GamePadState gamePadStatus;
        

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
            PositionPoint = new Point(200, 200);
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

        public void sheepUpdate(GameTime gameTime)
        {
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
            sheep = new Sprite(new Sprite("Sprites/sheep_sheet01", TestGame.GameInstance.Content), new Rectangle(0, 102, 34, 34));
            Sshadow = new Sprite("Sprites/sheep_shadow01", TestGame.GameInstance.Content); 
            base.LoadResources();
        }

        public override void Render()
        {
            TestGame.GameInstance.spriteBatch.Begin();
            Sshadow.draw(TestGame.GameInstance.spriteBatch, new Vector2(1.5f), 0.0f, PositionAbsolute, SpriteEffects.None);
            sheep.draw(TestGame.GameInstance.spriteBatch, Vector2.One, 0.0f, PositionAbsolute, SpriteEffects.None);
            //TestGame.GameInstance.spriteBatch.Draw();
            TestGame.GameInstance.spriteBatch.End();

            base.Render();
        }
    }
}
