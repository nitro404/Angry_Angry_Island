using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firecracker_Engine;
using Microsoft.Xna.Framework;
using System.IO;

namespace Firecracker_Engine
{
    public class Player : GameObject
    {

        private const float CREDIT_TRICKLE_RATE = 0.5f;
        private float m_credits;
        private Ability m_selectedAbility;
        private List<Ability> m_abilities = new List<Ability>();
        public static Player Instance;

        public Player()
            : base()
        {
            for (int i = 0; i < (int)AbilityType.NumAbilityTypes; i++)
            {
                m_abilities.Add(new Ability((AbilityType)i));
            }
            m_selectedAbility = m_abilities[0];
            Instance = this;
		}
        public Player(Sprite theSprite)
            : base()
        {
            for (int i = 0; i < (int)AbilityType.NumAbilityTypes; i++)
            {
                m_abilities.Add(new Ability((AbilityType)i));
            }
            m_selectedAbility = m_abilities[0];
            Instance = this;
            m_sprite = theSprite;
        }

        public void DoSelectedAbility(Vector2 position) //use an ability where the user clicked
        {
            if (m_selectedAbility.Cost < m_credits && m_selectedAbility.m_cooldownTimeLeft == 0)
            {
                m_credits -= m_selectedAbility.Cost;
                m_selectedAbility.DoEffects(position); //eric has acceleration penis
            }
        }

        public override void update(GameTime gameTime)
        {

            position = Firecracker.engineInstance.m_MouseManager.GetMousePos();

            if (Firecracker.engineInstance.m_MouseManager.IsMouseLeftPressed() && !UIScreenManager.Instance.mouseBlocked)
            {
                DoSelectedAbility(Firecracker.engineInstance.m_MouseManager.GetMousePos()+Firecracker.engineInstance.theCamera.GetCameraPos());
            }

            m_credits += (float)((float)gameTime.ElapsedGameTime.TotalSeconds) * CREDIT_TRICKLE_RATE;
            foreach (Ability ability in m_abilities)
            {
                ability.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            base.update(gameTime);
        }

        public static Player parseFrom(StreamReader input, SpriteSheetCollection spriteSheets)
        {
            // create the object
            Player newObject = new Player();
            newObject.updateInitialValues();

            return newObject;

        }

        public Ability GetAbilityByType(AbilityType ability)
        {
            return m_abilities[(int)ability];
        }

        public override void OnDestroyed()
        {
            base.OnDestroyed();
        }

        public float Credits
        {
            get { return m_credits; }
            set { m_credits = value; }
        }
        public Ability SelectedAbility
        {
            get { return m_selectedAbility; }
            set { m_selectedAbility = value; }
        }
        public List<Ability> Abilities
        {
            get { return m_abilities; }
            set { m_abilities = value; }
        }
    }
}
