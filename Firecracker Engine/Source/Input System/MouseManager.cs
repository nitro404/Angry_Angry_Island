using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace Firecracker_Engine.Source.Input_System
{
    public class MouseManager
    {
        MouseState m_MouseState;
        MouseState m_OldMouseState;

        /// <summary>
        /// Initialization. Polls the mouse state for the first time.
        /// </summary>
        public void Initialize()
        {
            //m_OldMouseState = new MouseState();
            m_MouseState = Mouse.GetState();
        }

        /// <summary>
        /// Update loop. Stores the mouse state in a holder and polls the system for the current one.
        /// </summary>
        public void UpdateMouse()
        {
            m_OldMouseState = m_MouseState;
            m_MouseState = Mouse.GetState();
        }
        
        /// <summary>
        /// Returns the X value of the mouse position as a float
        /// </summary>
        /// <returns>Mouse position (X axis)</returns>
        public float GetMousePosX()
        {
            return m_MouseState.X;
        }
        
        /// <summary>
        /// Returns the Y value of the mouse position as a float
        /// </summary>
        /// <returns>Mouse position (Y axis)</returns>
        public float GetMousePosY()
        {
            return m_MouseState.Y;
        }

        /// <summary>
        /// Returns the mouse position as a vector2
        /// </summary>
        /// <returns>Mouse Position</returns>
        public Vector2 GetMousePos()
        {
            return new Vector2(m_MouseState.X, m_MouseState.Y);
        }
        
        /// <summary>
        /// Checks if the Left Mouse Button is currently down
        /// </summary>
        /// <returns>True if the Left Mouse Button is down, false if it's not</returns>
        public bool isMouseLeftDown()
        {
            if (m_MouseState.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            else { return false; }
        }

        /// <summary>
        /// Checks if the Right Mouse Button is currently down
        /// </summary>
        /// <returns>True if the Right Mouse Button is down, false if it's not</returns>
        public bool isMouseRightDown()
        {
            if (m_MouseState.RightButton == ButtonState.Pressed)
            {
                return true;
            }
            else { return false; }
        }

        /// <summary>
        /// Checks if the Middle Mouse Button is currently down
        /// </summary>
        /// <returns>True if the Middle Mouse Button is down, false if it's not</returns>
        public bool IsMouseMiddleDown()
        {
            if (m_MouseState.MiddleButton == ButtonState.Pressed)
            {
                return true;
            }
            else { return false; }
        }

        /// <summary>
        /// Returns the change in position of the mouse wheel
        /// </summary>
        /// <returns>The change in position of the mouse wheel. If there is no change, it'll display 0.0</returns>
        public float MouseWheelChanged()
        {
            return m_MouseState.ScrollWheelValue - m_OldMouseState.ScrollWheelValue;
        }

        /// <summary>
        /// Checks if the Left Mouse Button has just been pressed within the last update
        /// </summary>
        /// <returns>True if the Left Mouse Button was pressed in the last update, false if it was not</returns>
        public bool IsMouseLeftPressed()
        {
            if (m_MouseState.LeftButton == ButtonState.Pressed)
            {
                if (m_OldMouseState.LeftButton == ButtonState.Released)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if the Right Mouse Button has just been pressed within the last update
        /// </summary>
        /// <returns>True if the Right Mouse Button was pressed in the last update, false if it was not</returns>
        public bool IsMouseRightPressed()
        {
            if (m_MouseState.RightButton == ButtonState.Pressed)
            {
                if (m_OldMouseState.RightButton == ButtonState.Released)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if the Middle Mouse Button has just been pressed within the last update
        /// </summary>
        /// <returns>True if the Middle Mouse Button was pressed in the last update, false if it was not</returns>
        public bool IsMouseMiddlePressed()
        {
            if (m_MouseState.MiddleButton == ButtonState.Pressed)
            {
                if (m_OldMouseState.MiddleButton == ButtonState.Released)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if the Left Mouse Button has just been released within the last update
        /// </summary>
        /// <returns>True if the Left Mouse Button was released in the last update, false if it was not</returns>
        public bool IsMouseLeftReleased()
        {
            if (m_MouseState.LeftButton == ButtonState.Released)
            {
                if (m_OldMouseState.LeftButton == ButtonState.Pressed)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if the Right Mouse Button has just been pressed within the last update
        /// </summary>
        /// <returns>True if the Right Mouse Button was pressed in the last update, false if it was not</returns>
        public bool IsMouseRightReleased()
        {
            if (m_MouseState.RightButton == ButtonState.Released)
            {
                if (m_OldMouseState.RightButton == ButtonState.Pressed)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if the Middle Mouse Button has just been pressed within the last update
        /// </summary>
        /// <returns>True if the Middle Mouse Button was pressed in the last update, false if it was not</returns>
        public bool IsMouseMiddleReleased()
        {
            if (m_MouseState.MiddleButton == ButtonState.Released)
            {
                if (m_OldMouseState.MiddleButton == ButtonState.Pressed)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
