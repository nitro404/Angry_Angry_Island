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
    //switchers have left and right buttons
    //and allow the user to cycle through
    //multiple text options. the selected option
    //is displayed between the left and right buttons.
    public class Switcher : UIObject
    {
        //texture asset names for the left and right buttons are hard-coded for now.

        public string ID;
        public List<string> options;
        public int selectedIndex = 0;

        public Switcher(Vector2 pos, Vector2 size, HAlign hAlign, VAlign vAlign, string ID, List<string> options)
            : base(pos, size, hAlign, vAlign, true, ContentType.Structural, "")
        {
            this.options = options;
            this.ID = ID;
            InnerElements.Add(new Button(Vector2.Zero, new Vector2(size.Y, size.Y), HAlign.Left, VAlign.Top, "SwitcherLeft", Style.SwitcherLeftNormal, Style.SwitcherLeftMouseOver, Style.SwitcherLeftPressed));
            InnerElements.Add(new Button(Vector2.Zero, new Vector2(size.Y, size.Y), HAlign.Right, VAlign.Top, "SwitcherRight", Style.SwitcherRightNormal, Style.SwitcherRightMouseOver, Style.SwitcherRightPressed));
            InnerElements.Add(new Label(Vector2.Zero, HAlign.Center, VAlign.Center, ""));
            Init();
            foreach (UIObject element in InnerElements)
            {
                element.parent = this;
            }
        }

        public void Increment()
        {
            selectedIndex = (selectedIndex + 1) % options.Count;
            ((Label)InnerElements[2]).SetText(options[selectedIndex]);
            DoAction();
        }

        public void Decrement()
        {
            selectedIndex = (selectedIndex + options.Count - 1) % options.Count;
            ((Label)InnerElements[2]).SetText(options[selectedIndex]);
            DoAction();
        }

        public override void Init()
        {
            //the initial values of the switcher are set here.
            //for now this is all done with code, but ideally
            //it would be possible to bind a switcher to
            //boolean or enum game options. This would require
            //an overhaul of how settings work though.

            //default index is 0.
            selectedIndex = 0;

            //find the index.

            //if there aren't enough elements to allow switching, disable the buttons.
            ((Label)InnerElements[2]).SetText(this.options[selectedIndex]);

            if (options.Count < 2)
            {
                ((Button)InnerElements[0]).disabled = true;
                ((Button)InnerElements[1]).disabled = true;
            }
            else
            {
                ((Button)InnerElements[0]).disabled = false;
                ((Button)InnerElements[1]).disabled = false;
            }

            base.Init();
        }

        public void DoAction()
        {
            //actions are hard-coded. again, if the switchers could
            //be bound to a game setting this wouldn't be needed.
        }
    }
}
