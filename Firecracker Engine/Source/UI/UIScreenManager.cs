﻿using System;
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

    //This is the main UI class that will be interacted
    //with by the game.

    public class UIScreenManager
    {
        #region Singleton Definition
        private static UIScreenManager instance;
        public static UIScreenManager Instance 
        {
            get
            {
                return instance;
            }
        }
       
        public static UIScreenManager CreateInstance()
        {
            if(instance == null)
            {
                 instance = new UIScreenManager();
            }
            return instance;
        }


        #endregion

        public bool mouseBlocked;

        //this should be called each frame during update
        public void Update(float deltaT)
        {
            mouseBlocked = false;
            if (currentScreen != null)
            {
                currentScreen.Update(deltaT);
                currentScreen.TestMouse(Firecracker.engineInstance.m_MouseManager.MouseState, Firecracker.engineInstance.m_MouseManager.OldMouseState);
            }

        }

        //this should be called each frame during update, assuming the current UI needs mouse interaction
        public void TestMouse(MouseState mouseState, MouseState lastMouseState)
        {
            currentScreen.TestMouse(mouseState, lastMouseState);
        }

        //this should be called each frame during draw
        public void Draw(GraphicsDevice g, SpriteBatch sb)
        {
            if (currentScreen != null)
            {
                currentScreen.Draw(g, sb);
            }
        }

        //this can be called to change the current screen.
        public void SetScreen(Screen screen)
        {
            lastScreen = currentScreen;
            currentScreen = screen;
        }

        public void GoToLastScreen()
        {
            SetScreen(lastScreen);
        }


        //for now this contains all of the setup for the different screens
        //ideally this would be contained in text files instead.
        //everything is done through constructor parameters.
        private UIScreenManager()
        {
        }

        public Screen currentScreen;
        public Screen lastScreen;
    }
}
