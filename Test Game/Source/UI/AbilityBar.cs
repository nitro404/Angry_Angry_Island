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
    public class AbilityBar : UIObject
    {
        public const int ICON_HEIGHT = 64;
        public const int ICON_WIDTH = 64;
        const int BUTTON_PADDING = 5;
        const int BAR_PADDING = 10;
        const int CREDITS_HEIGHT = 40;
        const int CREDITS_WIDTH = 150;
        static AbilityBar Instance;
        bool initializedYet = false;

        private Label creditLabel;

        public AbilityBar()
            : base(Vector2.Zero, new Vector2((ICON_WIDTH + BUTTON_PADDING * 2) * 5 + (BAR_PADDING*2), ICON_HEIGHT + BUTTON_PADDING * 2 + BAR_PADDING * 2), HAlign.Left, VAlign.Bottom, false, ContentType.Image, "transparentgrey")
        {
            InnerElements = new List<UIObject>();
            creditLabel = new Label(new Vector2(BAR_PADDING, 0), HAlign.Left, VAlign.Center, "Credits:");
            InnerElements.Add(new UIObject(new Vector2(0, -CREDITS_HEIGHT), new Vector2(CREDITS_WIDTH, CREDITS_HEIGHT), HAlign.Left, VAlign.Top, false, ContentType.Image, "transparentgrey",
                new List<UIObject>{creditLabel}));
            Instance = this;
        }

        public override void Update(float deltaT)
        {
            if (Player.Instance != null)
            {
                if (!initializedYet)
                {
                    Init();
                    initializedYet = true;
                }
                creditLabel.SetText("Credits: " + (int)Player.Instance.Credits);
            }
            base.Update(deltaT);
        }

        public override void Init()
        {
            for (int i = 0; i < Player.Instance.Abilities.Count; i++)
            {
                InnerElements.Add(new AbilityButton(new Vector2(BAR_PADDING + (ICON_WIDTH + BUTTON_PADDING * 2) * i, 0), new Vector2(ICON_WIDTH + BUTTON_PADDING * 2, ICON_HEIGHT + BUTTON_PADDING * 2), HAlign.Left, VAlign.Center, Player.Instance.Abilities[i].type));
            }
            base.Init();
        }
    }
}
