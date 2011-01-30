using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Firecracker_Engine
{

    public enum AbilityType
    {
        Lightning,
        Fireball,
        Earthquake,
        NumAbilityTypes
    }

    public class Ability
    {
        public AbilityType type;
        public float m_cost;
        public float m_cooldown;
        public float m_cooldownTimeLeft = 0;
        public string m_iconTextureAssetName;

        public Ability(AbilityType whichAbility)
        {
            type = whichAbility;
            switch (type)
            {
                case AbilityType.Fireball:
                    m_cost = 5;
                    m_cooldown = 5;
                    m_iconTextureAssetName = "testicon";
                    break;
                case AbilityType.Earthquake:
                    m_cost = 5;
                    m_cooldown = 5;
                    m_iconTextureAssetName = "testicon";
                    break;
                case AbilityType.Lightning:
                    m_cost = 5;
                    m_cooldown = 1;
                    m_iconTextureAssetName = "testicon";
                    break;
            }
        }

        public virtual void DoEffects(Vector2 position)
        {
            switch (type)
            {
                case AbilityType.Fireball:
                    Asteroid newAsteroid = new Asteroid(position);
                    Firecracker.level.addObject(newAsteroid);
                    break;
                case AbilityType.Earthquake:
                    break;
                case AbilityType.Lightning:
                    Lightning newLightning = new Lightning(position);
                    Firecracker.level.addObject(newLightning);
                    break;
            }
            m_cooldownTimeLeft = m_cooldown;
        }

        public void Update(float fTime)
        {
            if (m_cooldownTimeLeft > 0)
            {
                m_cooldownTimeLeft = (float)Math.Max(0, m_cooldownTimeLeft - fTime);
            }
        }

        public float Cost
        {
            get { return m_cost; }
        }
    }
}
