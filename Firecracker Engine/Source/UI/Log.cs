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
    class Log : UIObject
    {
        //this class displays a log of messages that gradually fade out.
        //there should only be one of these as the function to log
        //messages is static.
        const float ENTRY_LIFE_TIME = 7.5f;
        static public Log instance;
        List<float> lifetimes;

        public Log()
            : base(Vector2.Zero, Style.LogEntrySize, HAlign.Right, VAlign.Bottom, false, ContentType.Structural, "")
        {
            InnerElements = new List<UIObject>();
            lifetimes = new List<float>();
            instance = this;
        }

        public static void WriteLine(string line)
        {
            if (instance != null)
            {
                foreach (UIObject entry in instance.InnerElements)
                {
                    entry.pos.Y -= Style.LogEntrySize.Y;
                }
                instance.InnerElements.Add(new UIObject(Vector2.Zero, Style.LogEntrySize, HAlign.Left, VAlign.Bottom, false, ContentType.Image, "logentry",
                                            new List<UIObject> { new Label(new Vector2(-20, 0), HAlign.Right, VAlign.Center, line) }));
                instance.lifetimes.Add(ENTRY_LIFE_TIME);
            }
        }

        public static void Clear()
        {
            if (instance != null)
            {
                instance.lifetimes.Clear();
                instance.InnerElements.Clear();
            }
        }

        public override void Update(float deltaT)
        {
            for (int i = 0; i < lifetimes.Count; i++)
            {
                lifetimes[i] -= deltaT;
                if (lifetimes[i] <= 0)
                {
                    lifetimes.RemoveAt(i);
                    InnerElements.RemoveAt(i);
                    i--;
                }
                else
                {
                    InnerElements[i].opacity = Math.Min(1,(lifetimes[i]) / ENTRY_LIFE_TIME * 2);
                }
            }
        }
    }
}
