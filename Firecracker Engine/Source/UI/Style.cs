using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Firecracker_Engine
{
    public static class Style
    {
        //THESE SHOULD BE SET TO THEIR DESIRED VALUES BEFORE INITIALIZING UISCREENMANAGER
        public static int BaseHeight;
        public static SpriteFont font;
        public static Color fontColor;
        public static string SwitcherLeftPressed;
        public static string SwitcherLeftNormal;
        public static string SwitcherLeftMouseOver;
        public static string SwitcherRightPressed;
        public static string SwitcherRightNormal;
        public static string SwitcherRightMouseOver;
        public static Vector2 SwitcherButtonSize;
        public static string SliderBar;
        public static string SliderBarMouseOver;
        public static string SliderHandle;
        public static string SliderHandleMouseOver;
        public static Vector2 SliderHandleSize;
        public static string PopupWindowTitle;
        public static string PopupButtonNormal;
        public static string PopupButtonPressed;
        public static string PopupButtonMouseOver;
        public static Vector2 PopupTitleSize;
        public static string PopupWindowBox;
        public static string LogEntry;
        public static Vector2 LogEntrySize;
    }
}
