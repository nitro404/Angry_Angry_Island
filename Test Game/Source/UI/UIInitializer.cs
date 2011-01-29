﻿using System;
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
    public static class UIInitializer
    {
        public static void InitializeUI()
        {
            Style.BaseHeight = 1080;
            Style.font = GlobalFirecrackerRef.Instance.Content.Load<SpriteFont>(@"UI\font");
            Style.fontColor = Color.White;
            Style.LogEntry = "testbutton";
            Style.LogEntrySize = new Vector2(300, 20);
            Style.PopupButtonMouseOver = "testbuttonhover";
            Style.PopupButtonNormal = "testbutton";
            Style.PopupButtonPressed = "testbuttonclicked";
            Style.PopupWindowBox = "testbutton";
            Style.PopupWindowTitle = "testbutton";

            UIScreenManager.Instance.currentScreen = new Screen(
                new List<UIObject>{new AbilityBar(), new InfoBar(), new PopupNotification()});

        }
    }
}