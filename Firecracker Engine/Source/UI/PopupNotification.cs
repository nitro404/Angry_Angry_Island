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
    public class PopupNotification : UIObject
    {
        //this class is for displaying popup windows with a message and an OK button.
        //the textures for the window and for the buttons are hard-coded for now.
        //depending on the size of the message, different textures are used for the window
        //so it doesn't look too stretched.

        public static PopupNotification instance;

        public PopupNotification()
            : base(Vector2.Zero, Vector2.Zero, HAlign.Center, VAlign.Center, false, ContentType.Image, Style.PopupWindowBox, 
                    new List<UIObject>(){new Label(Vector2.Zero, HAlign.Center, VAlign.Center, ""), new UIObject(new Vector2(0, -Style.PopupTitleSize.Y), Style.PopupTitleSize, UIObject.HAlign.Left, UIObject.VAlign.Top, false, UIObject.ContentType.Image, Style.PopupWindowTitle, new List<UIObject>{new Label(new Vector2(10, 0), UIObject.HAlign.Left, UIObject.VAlign.Center, "")}),
                        new UIObject(new Vector2(0, 0), new Vector2(250,100), UIObject.HAlign.Center, UIObject.VAlign.Center, false, UIObject.ContentType.Structural, "" ,
                                        new List<UIObject>{new Button(new Vector2(0, 0), new Vector2(200, 65), HAlign.Center, VAlign.Center, "NotificationOkButton", Style.PopupButtonNormal, Style.PopupButtonMouseOver, Style.PopupButtonPressed,
                                                                   new List<UIObject> { new Label(Vector2.Zero, UIObject.HAlign.Center, UIObject.VAlign.Center, "OK") })})})
        {
            instance = this;
            hidden = true;
        }

        public void ShowNotification(string notificationTitle, string notification)
        {
            ((Label)InnerElements[0]).SetText(notification);
            ((Label)(InnerElements[1].InnerElements[0])).SetText(notificationTitle);
            size = InnerElements[0].size + new Vector2(120, 120);
            InnerElements[2].pos.Y = (size.Y / 2) + 65;
            hidden = false;
        }

        public void HideNotification()
        {
            hidden = true;
        }

    }
}
