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
        float m_cooldown;
        float m_cooldownTimeLeft;
        string m_iconTextureAssetName;

        public Ability(float cost, float cooldown, float cooldownTimeLeft, string iconTextureAssetName)
        {
            m_cost = cost;
            m_cooldownTimeLeft = cooldownTimeLeft;
            m_cooldown = cooldown;
            m_iconTextureAssetName = iconTextureAssetName;
        }

        public virtual void DoEffects(Vector2 position)
        {

        }

        public void Update(float fTime)
        {
            if (m_cooldownTimeLeft > 0)
            {
                m_cooldownTimeLeft = (float)Math.Max(0, m_cooldown - fTime);
            }
        }

        public float Cost
        {
            get { return m_cost; }
        }
    }
}
