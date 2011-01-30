using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace Firecracker_Engine
{
    //represents a clickable button, supporting different images for idle, hover and clicked
    public class Button : UIObject
    {
        //this ID indicates which action should be taken on a click
        public string ID;

        //these ID's should store the asset names of the different images required for the button
        public string regularImageID;
        public string mouseOverImageID;
        public string clickImageID;
        public bool disabled = false;
        public bool pressed = false;

        public Button(Vector2 pos, Vector2 size, HAlign hAlign, VAlign vAlign, string ID, string regularImageID, string mouseOverImageID, string clickImageID)
            : base(pos, size, hAlign, vAlign, true, ContentType.Image, regularImageID)
        {
            this.ID = ID;
            this.regularImageID = regularImageID;
            this.mouseOverImageID = mouseOverImageID;
            this.clickImageID = clickImageID;
        }
        public Button(Vector2 pos, Vector2 size, HAlign hAlign, VAlign vAlign, string ID, string regularImageID, string mouseOverImageID, string clickImageID, List<UIObject> InnerElements)
            : base(pos, size, hAlign, vAlign, true, ContentType.Image, regularImageID, InnerElements)
        {
            this.ID = ID;
            this.regularImageID = regularImageID;
            this.mouseOverImageID = mouseOverImageID;
            this.clickImageID = clickImageID;
        }

        public override void OnMouseOver(MouseState mouseState, MouseState lastMouseState, bool isMouseOver)
        {
            if (!disabled && (PopupNotification.instance.hidden || ID == "NotificationOkButton"))
            {
                opacity = 1;
                if (isMouseOver)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed && (lastMouseState.LeftButton == ButtonState.Released || pressed))
                    {
                        param = clickImageID;
                        pressed = true;
                    }
                    else
                    {
                        if (mouseState.LeftButton == ButtonState.Released && pressed)
                        {
                            pressed = false;
                            DoAction();
                        }
                        if (mouseState.LeftButton != ButtonState.Pressed)
                        {
                            param = mouseOverImageID;
                        }
                    }
                }
                else
                {
                    param = regularImageID;
                    if (mouseState.LeftButton == ButtonState.Released)
                    {
                        pressed = false;
                    }
                }
            }
            else
            {
                opacity = 0.3f;
                param = regularImageID;
            }
        }

        public virtual void DoAction()
        {
            //All the different actions for different buttons are hard-coded here.
            //TODO: Add some standard actions like switch screen, exit, back etc. that can take parameters.
            //for now it's all hard coded, but to acheive greater separation between engine and game
            //this could be changed.
            if (ID == "BackButton")
            {
                UIScreenManager.Instance.GoToLastScreen();
            }
            else if (ID == "NotificationOkButton")
            {
                PopupNotification.instance.HideNotification();
                if (PopupNotification.instance.backToMenu)
                {
                    Firecracker.interpreter.execute("menu on");
                }
            }
            else if (ID == "SwitcherLeft")
            {
                    ((Switcher)parent).Decrement();
            }
            else if (ID == "SwitcherRight")
            {
                ((Switcher)parent).Increment();
            }
            else
            {
                UICallBackInfo info = new UICallBackInfo();
                info.eventType = UIEventType.ButtonAction;
                info.ID = ID;
                IDRegistrar.ExecuteCallBack(info);
            }
        }


    }
}
