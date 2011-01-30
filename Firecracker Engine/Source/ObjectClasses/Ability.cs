﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Firecracker_Engine
{

    public enum AbilityType
    {
        Lightning,
        GodAnimal,
        SpiderFire,
        Asteroid,
        CandyCane,
        NumAbilityTypes,

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

                case AbilityType.Lightning:
                    m_cost = 5;
                    m_cooldown = 1;
                    m_iconTextureAssetName = "lightning_icon";
                    break;
                case AbilityType.GodAnimal:
                    m_cost = 40;
                    m_cooldown = 10;
                    m_iconTextureAssetName = "KevinMooseIcon";
                    break;
                case AbilityType.SpiderFire:
                    m_cost = 80;
                    m_cooldown = 10;
                    m_iconTextureAssetName = "fire_icon";
                    break;
                case AbilityType.Asteroid:
                    m_cost = 120;
                    m_cooldown = 1;
                    m_iconTextureAssetName = "meteor_icon";
                    break;

                case AbilityType.CandyCane:
                    m_cost = 40;
                    m_cooldown = 5;
                    m_iconTextureAssetName = "CandyCaneIcon";
                    break;
            }
        }

        public virtual void DoEffects(Vector2 position)
        {
            switch (type)
            {
                case AbilityType.Asteroid:
                    Asteroid newAsteroid = new Asteroid(position);
                    Firecracker.level.addObject(newAsteroid);
                    break;
                case AbilityType.SpiderFire:
                    SpiderFire newSpiderFire = new SpiderFire(position);
                    Firecracker.level.addObject(newSpiderFire);
                    break;
                case AbilityType.Lightning:
                    Lightning newLightning = new Lightning(position);
                    Firecracker.level.addObject(newLightning);
                    break;
                case AbilityType.GodAnimal:
                    KevinMoose newKevinMoose = new KevinMoose(position);
                    Firecracker.level.addObject(newKevinMoose);
                    break;
                case AbilityType.CandyCane:
                    CandyCane newCandyCane = new CandyCane(position);
                    Firecracker.level.addObject(newCandyCane);
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
