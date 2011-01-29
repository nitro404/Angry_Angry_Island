using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firecracker_Engine;

namespace Test_Game
{
    class Player : CBaseObject
    {
        private const float CREDIT_TRICKLE_RATE = 0.25f;
        private float m_credits; 

        public Player()
        {
            m_sObjectName = "Player";
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
