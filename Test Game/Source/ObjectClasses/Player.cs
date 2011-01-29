﻿using System;
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

        public Player()
        {
        }

        public void DoSelectedAbility(Vector2 position) //use an ability where the user clicked
        {
            if (m_selectedAbility.Cost < m_credits)
            {
                m_credits -= m_selectedAbility.Cost;
                m_selectedAbility.DoEffects(position);
            }
        }

        public override void  Tick(float fTime)
        {
            m_credits += fTime * CREDIT_TRICKLE_RATE;
 	        base.Tick(fTime);
        }

        public float M_Credits
        {
            get { return m_credits; }
            set { m_credits = value; }
        }
    }
}
