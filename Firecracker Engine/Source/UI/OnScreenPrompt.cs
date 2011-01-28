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
    class OnScreenPrompt : Label
    {
        //this is simply a label that shows up
        //near the bottom of the screen. It can be accessed from anywhere by the static instance variable.

        public static OnScreenPrompt instance;

        public OnScreenPrompt(Vector2 pos, HAlign hAlign, VAlign vAlign)
            : base(pos, hAlign, vAlign, "")
        {
            instance = this;
            hidden = true;
        }

        public void setPrompt(string prompt)
        {
            SetText(prompt);
        }
    }
}
