using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firecracker_Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Test_Game
{
    class AbilityButton : Button
    {
        const string REGULAR_TEX = "abilitybutton";
        const string PRESSED_TEX = "abilitybuttonclicked";
        const string MOUSEOVER_TEX = "abilitybuttonmouseover";
        const string SELECTED_TEX = "abilitybuttonselected";

        AbilityType m_abilityType;

        public AbilityButton(Vector2 pos, Vector2 size, HAlign hAlign, VAlign vAlign, AbilityType abilityType)
            : base(pos, size, hAlign, vAlign, "", REGULAR_TEX, MOUSEOVER_TEX, PRESSED_TEX)
        {
            m_abilityType = abilityType;
            InnerElements.Add(new UIObject(Vector2.Zero, new Vector2(AbilityBar.ICON_WIDTH, AbilityBar.ICON_HEIGHT), HAlign.Center, VAlign.Center, false, ContentType.Image, Player.Instance.GetAbilityByType(m_abilityType).m_iconTextureAssetName));
        }

        public override void Update(float deltaT)
        {
            bool isOnCoolDown = Player.Instance.GetAbilityByType(m_abilityType).m_cooldownTimeLeft > 0;
            if (isOnCoolDown)
            {
                disabled = true;
            }
            else
            {
                disabled = false;
            }
            if (!isOnCoolDown && m_abilityType == Player.Instance.SelectedAbility.type)
            {
                regularImageID = SELECTED_TEX;
                mouseOverImageID = SELECTED_TEX;
                clickImageID = SELECTED_TEX;
            }
            else
            {

                regularImageID = REGULAR_TEX;
                mouseOverImageID = MOUSEOVER_TEX;
                clickImageID = PRESSED_TEX;
            }

            base.Update(deltaT);
        }

        public override void DoAction()
        {
            Player.Instance.SelectedAbility = Player.Instance.GetAbilityByType(m_abilityType);
            //base.DoAction();
        }
    }
}
