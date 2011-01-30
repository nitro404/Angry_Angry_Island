using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Firecracker_Engine
{
    public class CameraBase : CBaseObject
    {
#pragma warning disable 108
        public const string ClassName = "CameraBase";
#pragma warning restore 108

        Vector2 m_cameraPos;
        Vector2 m_cameraLookAtPos;

        float m_cameraLimitDown;
        float m_cameraLimitUp;
        float m_cameraLimitLeft;
        float m_cameraLimitRight;

        /// <summary>
        /// Initializes the Camera items
        /// </summary>
        public void initialize()
        {
            m_cameraPos = new Vector2(0.0f, 0.0f);
            m_cameraLookAtPos = new Vector2(0.0f, 0.0f);

            m_cameraLimitUp = -100.0f;
			m_cameraLimitDown = 2148.0f;
            m_cameraLimitLeft = -100.0f;
			m_cameraLimitRight = 2148.0f;
        }

        /// <summary>
        /// Get/Set function for upper camera limit
        /// </summary>
        public float CameraLimitUp
        {
            get { return m_cameraLimitUp; }
            set { m_cameraLimitUp = value; }
        }

        /// <summary>
        /// Get/Set function for down camera limit
        /// </summary>
        public float CameraLimitDown
        {
            get { return m_cameraLimitDown; }
            set { m_cameraLimitDown = value; }
        }

        /// <summary>
        /// Get/Set function for left camera limit
        /// </summary>
        public float CameraLimitLeft
        {
            get { return m_cameraLimitLeft; }
            set { m_cameraLimitLeft = value; }
        }

        /// <summary>
        /// Get/Set function for right camera limit
        /// </summary>
        public float CameraLimitRight
        {
            get { return m_cameraLimitRight; }
            set { m_cameraLimitRight = value; }
        }

        /// <summary>
        /// Moves the Camera Left on the X axis. If it's not able to, it sets the x value to the left limit.
        /// </summary>
        public void MoveCameraLeft()
        {
            if (m_cameraPos.X > m_cameraLimitLeft)
                m_cameraPos.X -= 1.0f;
            else
                m_cameraPos.X = m_cameraLimitLeft;
        }

        /// <summary>
        /// Moves the Camera Right on the X axis. If it's not able to, it sets the x value to the right limit.
        /// </summary>
        public void MoveCameraRight()
        {
            if (m_cameraPos.X < m_cameraLimitRight)
                m_cameraPos.X += 1.0f;
            else
                m_cameraPos.X = m_cameraLimitRight;
        }

        /// <summary>
        /// Moves the Camera Up on the Y axis. If it's not able to, it sets the y value to the up limit.
        /// </summary>
        public void MoveCameraUp()
        {
            if (m_cameraPos.Y > m_cameraLimitUp)
                m_cameraPos.Y -= 1.0f;
            else
                m_cameraPos.Y = m_cameraLimitUp;
        }

        /// <summary>
        /// Moves the Camera Down on the Y axis. If it's not able to, it sets the y value to the down limit.
        /// </summary>
        public void MoveCameraDown()
        {
            if (m_cameraPos.Y < m_cameraLimitDown)
                m_cameraPos.Y += 1.0f;
            else
                m_cameraPos.Y = m_cameraLimitDown;
        }

        /// <summary>
        /// Sets the camera position to an x/y location on the world
        /// </summary>
        /// <param name="xPos">Sets the x position of the camera</param>
        /// <param name="yPos">Sets the y position of the camera</param>
        public void SetCameraPos(float xPos, float yPos)
        {
            if(xPos > m_cameraLimitLeft && xPos < m_cameraLimitRight)
                m_cameraPos.X = xPos;

            if (yPos > m_cameraLimitUp && yPos < m_cameraLimitDown)
                m_cameraPos.Y = yPos;
        }

        /// <summary>
        /// Returns the current camera position
        /// </summary>
        /// <returns>Returns the current camera position</returns>
        public Vector2 GetCameraPos()
        {
            return m_cameraPos;
        }

        ///// <summary>
        ///// Checks whether or not the Camera can move Up
        ///// </summary>
        ///// <returns>False if it's unable to, true if it's able to</returns>
        //public bool CanCameraMoveUp()
        //{
        //    if (m_cameraPos.Y <= m_cameraLimitUp)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        ///// <summary>
        ///// Checks whether or not the camera can move down
        ///// </summary>
        ///// <returns>False if it's unable to, true if it's able to </returns>
        //public bool CanCameraMoveDown()
        //{
        //    if (m_cameraPos.Y >= m_cameraLimitDown)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        ///// <summary>
        ///// Checks whether or not the camera can move left
        ///// </summary>
        ///// <returns>False if it's unable to, true if it's able to</returns>
        //public bool CanCameraMoveLeft()
        //{
        //    if (m_cameraPos.X <= m_cameraLimitLeft)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        ///// <summary>
        ///// Checks whether or not the camera can move right
        ///// </summary>
        ///// <returns>False if it's unable to, true if it's able to</returns>
        //public bool CanCameraMoveRight()
        //{
        //    if (m_cameraPos.X >= m_cameraLimitRight)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        public override void LoadPropertiesList(ObjectDefinition objDef)
        {
            base.LoadPropertiesList(objDef);

            if (objDef.ClassProperties.ContainsKey("LeftLimit"))
            {
                m_cameraLimitLeft = Helpers.ParseFloat(objDef.ClassProperties["LeftLimit"]);
            }
            else if (objDef.ClassProperties.ContainsKey("RightLimit"))
            {
                m_cameraLimitRight = Helpers.ParseFloat(objDef.ClassProperties["RightLimit"]);
            }
            else if (objDef.ClassProperties.ContainsKey("UpLimit"))
            {
                m_cameraLimitUp = Helpers.ParseFloat(objDef.ClassProperties["UpLimit"]);
            }
            else if (objDef.ClassProperties.ContainsKey("DownLimit"))
            {
                m_cameraLimitDown = Helpers.ParseFloat(objDef.ClassProperties["DownLimit"]);
            }
        }
    }
}
