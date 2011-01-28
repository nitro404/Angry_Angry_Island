using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;

namespace Firecracker_Engine
{
    //this represents a text element to display on the screen.
    //The difference between this and a base UIObject text type
    //is that this has a SetText() function which automatically
    //sets the size of the object based on the text input,
    //and also Labels allow different styles to be applied
    //in the constructor.
    public class Label : UIObject
    {
        public int textOffset = 0;


        public Label(Vector2 pos, HAlign hAlign, VAlign vAlign, string text)
            : base(pos, Vector2.Zero, hAlign, vAlign, false, ContentType.Text, text) 
        {
            this.size = font.MeasureString(text);
            size.Y += textOffset;
            color = Style.fontColor;
        }

        public void SetText(string text)
        {
            param = text;
            size = font.MeasureString(text);
            size.Y += textOffset;
        }
    }
}
