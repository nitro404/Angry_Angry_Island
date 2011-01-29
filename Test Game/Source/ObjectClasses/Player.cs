using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firecracker_Engine;
using Microsoft.Xna.Framework;

namespace Test_Game
{
    class Player : CBaseObject
    {
#pragma warning disable 108
        public const string ClassName = "Player";
#pragma warning restore 108

        private const float CREDIT_TRICKLE_RATE = 0.25f;
        private float m_credits;
        private Ability m_selectedAbility;
        private List<Ability> m_abilities;
        public static Player Instance;

        public Player()
        {
            Instance = this;
        }

        public void DoSelectedAbility(Vector2 position) //use an ability where the user clicked
        {
            if (m_selectedAbility.Cost < m_credits)
            {
                m_credits -= m_selectedAbility.Cost;
                m_selectedAbility.DoEffects(position);
            }
        }

        public override void Tick(GameTime gameTime)
        {
            if (Firecracker.engineInstance.m_MouseManager.IsMouseLeftPressed())
            {
                // cause all the sheep objects to gravitate towards this point.
                List<CBaseObject> sheepList = Firecracker.engineInstance.FindObjectsByType("BasicAI");
                foreach (CBaseObject obj in sheepList)
                {
                    //((AIObject)obj).FleeFromPoint(Firecracker.engineInstance.m_MouseManager.GetMousePos());
                }
            }

            m_credits += (float)((float)gameTime.ElapsedGameTime.Milliseconds/1000.0f) * CREDIT_TRICKLE_RATE;
            base.Tick(gameTime);
        }

        public float Credits
        {
            get { return m_credits; }
            set { m_credits = value; }
        }
    }
}
