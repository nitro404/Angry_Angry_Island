using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Test_Game
{
    public abstract class Ability
    {
        float m_cost;

        public virtual void DoEffects(Vector2 position)
        {
        }

        public float Cost
        {
            get { return m_cost; }
        }
    }
}
