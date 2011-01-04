using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Firecracker_Engine
{
    public enum Direction
    {
        Down,
        Up,
        Left,
        Right
    }

    /** @class  InputManager
     *  @brief  update game input...
     */
    public class InputManager
    {
        List<GamePadState> m_GamePadStates;         ///< The current gamepad states
        List<GamePadState> m_OldGamePadStates;      ///< The previous gamepad states

        Texture2D m_mouseTexture;       
        MouseState m_MouseState;
        MouseState m_OldMouseState;
        bool m_IsScreenPanning;

        KeyboardState m_KeyState;                   ///< The current keyboard state
        KeyboardState m_OldKeyState;                ///< The previous keybaord state
                                                    ///
        Vector2 m_MapDisplacement;
        Vector2 m_OldMapDisplacement;

        bool isThereAnExplosion;

        int m_Delay;              //<! A delay called after the Reset, that locks the controls for a second, to prevent quick turn spasms
        Keys m_EnableKey;         //<! A Key that will unlock the controller, if locked, LockControls(Key) will lock the Control

        bool m_Locked;          ///< Are the controller inputs locked?
        float m_fKeyIdleTime;      ///< The Keyboards Idle Time
        List<float> m_fControllerIdleTime; ///<The Controllers Idle Time
        
        public int Delay
        {
            set
            {
                m_OldGamePadStates = m_GamePadStates;
                m_OldKeyState = m_KeyState;
                m_OldMouseState = m_MouseState;
                m_Delay = value;
            }
        }

        public float KeyIdleTime
        {
            get
            {
                return m_fKeyIdleTime;
            }
        }

        public List<float> ControllerIdleTime
        {
            get
            {
                return m_fControllerIdleTime;
            }
        }

        /** @fn     InputManager()
         *  @brief  constructor
         *  @author Tom
         *  @edit   Mike, moved the init stuff to a reset function
         */
        public InputManager()
        {

            m_MouseState = new MouseState();
            m_OldMouseState = new MouseState();
            m_IsScreenPanning = false;
            isThereAnExplosion = false;
            
            m_GamePadStates = new List<GamePadState>();
            m_OldGamePadStates = new List<GamePadState>();
            m_Delay = 0;
            
            m_fKeyIdleTime = 0.0f;
            m_fControllerIdleTime = new List<float>();

            ResetInput();
        }

        /** @fn     void ResetInput()
         *  @brief  throw out any current input states, we probably want this to happen at certain times (like module switches)
         *  @author Mike
         */
        public void ResetInput()
        {
            m_Delay = 25;
            m_Locked = false;
            m_fKeyIdleTime = 0.0f;

            m_GamePadStates.Clear();
            m_OldGamePadStates.Clear();
            m_fControllerIdleTime.Clear();

            m_GamePadStates.Add(new GamePadState());
            m_GamePadStates.Add(new GamePadState());
            m_GamePadStates.Add(new GamePadState());
            m_GamePadStates.Add(new GamePadState());
            m_fControllerIdleTime.Add(0.0f);
            m_fControllerIdleTime.Add(0.0f);
            m_fControllerIdleTime.Add(0.0f);
            m_fControllerIdleTime.Add(0.0f);

            m_OldGamePadStates.Add(new GamePadState());
            m_OldGamePadStates.Add(new GamePadState());
            m_OldGamePadStates.Add(new GamePadState());
            m_OldGamePadStates.Add(new GamePadState());


            //////////////////////////
            //Stuff we can delete once we go Xbox
            m_KeyState = new KeyboardState();
            m_OldKeyState = new KeyboardState();

            m_MouseState = new MouseState();
            m_OldMouseState = new MouseState();
        }

        /** @fn     void Update()
         *  @brief  Checks all Inputs and stores old Inputs
         *  @author Tom
         */
        public void Update(float fElapsedTime)
        {
            if (m_Delay < 1)
            {

                m_OldKeyState = m_KeyState;
                m_KeyState = Keyboard.GetState();

                m_OldGamePadStates[0] = m_GamePadStates[0];
                m_OldGamePadStates[1] = m_GamePadStates[1];
                m_OldGamePadStates[2] = m_GamePadStates[2];
                m_OldGamePadStates[3] = m_GamePadStates[3];

                m_GamePadStates[0] = GamePad.GetState(PlayerIndex.One);
                m_GamePadStates[1] = GamePad.GetState(PlayerIndex.Two);
                m_GamePadStates[2] = GamePad.GetState(PlayerIndex.Three);
                m_GamePadStates[3] = GamePad.GetState(PlayerIndex.Four);

                m_OldMouseState = m_MouseState;
                m_MouseState = Mouse.GetState();

                m_OldMapDisplacement = m_MapDisplacement;
//                m_MapDisplacement = new Vector2();
            }

            for (int i = 0; i < m_fControllerIdleTime.Count; i++ )
            {
                m_fControllerIdleTime[i] += fElapsedTime;
            }

            if (m_KeyState.GetPressedKeys().Length > 0)
                m_fKeyIdleTime = 0.0f;
            else
                m_fKeyIdleTime += fElapsedTime;

            if (m_KeyState.IsKeyDown(m_EnableKey))
            {
                m_Locked = false;
            }

            //Error.Trace(m_fControllerIdleTime[0].ToString());
            m_Delay--;
        }

        ////////////////////////////////////////
        //XBOX + WINDOWS Controls

        /** @fn     bool IsButtonDown(int ControllerIndex, Buttons Button)
         *  @brief  check if a button is down
         *  @return true if the button is down, false otherwise
         *  @param  ControllerIndex [in] the index of the controller [ 0 - 3 ]
         *  @param  Button [in] the button to check
         */
        public bool IsButtonDown(int ControllerIndex, Buttons Button)
        {
            if (m_Locked == false)
            {
                if (m_GamePadStates[ControllerIndex].IsButtonDown(Button))
                {
                    m_fControllerIdleTime[ControllerIndex] = 0.0f;
                    return true;
                }
                else
                    return false;
            }

            return false;
        }

        /** @fn     bool IsButtonUp(int ControllerIndex, Buttons Button)
         *  @brief  check if a button is up
         *  @return true if the button is up, false otherwise
         *  @param  ControllerIndex [in] the index of the controller [ 0 - 3 ]
         *  @param  Button [in] the button to check
         */
        public bool IsButtonUp(int ControllerIndex, Buttons Button)
        {
            if (m_Locked == false)
            {
                if (m_GamePadStates[ControllerIndex].IsButtonUp(Button))
                {
                    m_fControllerIdleTime[ControllerIndex] = 0.0f;
                    return true;
                }
                else
                    return false;
            }
            
            return false;
        }

        /** @fn     bool IsButtonPressed(int ControllerIndex, Buttons Button)
         *  @brief  check if a button is pressed
         *  @return true if the button is pressed, false otherwise
         *  @param  ControllerIndex [in] the index of the controller [ 0 - 3 ]
         *  @param  Button [in] the button to check
         */
        public bool IsButtonPressed(int ControllerIndex, Buttons Button)
        {
            if (m_Locked == false)
            {
                if (m_GamePadStates[ControllerIndex].IsButtonDown(Button))
                {
                    if (m_OldGamePadStates[ControllerIndex].IsButtonUp(Button))
                    {
                        m_fControllerIdleTime[ControllerIndex] = 0.0f;
                        return true;
                    }

                    return false;
                }
            }

            return false;
        }

        /** @fn     bool IsButtonReleased(int ControllerIndex, Buttons Button)
         *  @brief  check if a button is released
         *  @return true if the button is released, false otherwise
         *  @param  ControllerIndex [in] the index of the controller [ 0 - 3 ]
         *  @param  Button [in] the button to check
         */
        public bool IsButtonReleased(int ControllerIndex, Buttons Button)
        {
            if (m_Locked == false)
            {
                if (m_GamePadStates[ControllerIndex].IsButtonUp(Button))
                    if (m_OldGamePadStates[ControllerIndex].IsButtonDown(Button))
                    {
                        m_fControllerIdleTime[ControllerIndex] = 0.0f;
                        return true;
                    }
                return false;
            }
            return false;
        }


        /** @fn     bool IsDirectionPressed( int nControllerIndex, Direction dir )
         *  @brief  check if a controller direction is pressed
         *  @return true if the direction was just pressed, false otherwise
         */
        public bool IsDirectionPressed( int nControllerIndex, Direction dir )
        {
            if (m_Locked == false)
            {
                switch( dir )
                {
                    case Direction.Down:
                    {
                        if (m_GamePadStates[nControllerIndex].DPad.Down == ButtonState.Pressed)
                        {
                            if (m_OldGamePadStates[nControllerIndex].DPad.Down == ButtonState.Released)
                            {
                                m_fControllerIdleTime[nControllerIndex] = 0.0f;
                                return true;
                            }
                        }

                        if (m_GamePadStates[nControllerIndex].ThumbSticks.Left.Y < -0.8f)
                        {
                            if (m_OldGamePadStates[nControllerIndex].ThumbSticks.Left.Y >= -0.8f)
                            {
                                m_fControllerIdleTime[nControllerIndex] = 0.0f;
                                return true;
                            }
                        }

                        break;
                    }
                    case Direction.Up:
                    {
                        if (m_GamePadStates[nControllerIndex].DPad.Up == ButtonState.Pressed)
                        {
                            if (m_OldGamePadStates[nControllerIndex].DPad.Up == ButtonState.Released)
                            {
                                m_fControllerIdleTime[nControllerIndex] = 0.0f;
                                return true;
                            }
                        }

                        if (m_GamePadStates[nControllerIndex].ThumbSticks.Left.Y > 0.8f)
                        {
                            if (m_OldGamePadStates[nControllerIndex].ThumbSticks.Left.Y <= 0.8f)
                            {
                                m_fControllerIdleTime[nControllerIndex] = 0.0f;
                                return true;
                            }
                        }

                        break;
                    }
                    case Direction.Left:
                    {
                        if (m_GamePadStates[nControllerIndex].DPad.Left == ButtonState.Pressed)
                        {
                            if (m_OldGamePadStates[nControllerIndex].DPad.Left == ButtonState.Released)
                            {
                                m_fControllerIdleTime[nControllerIndex] = 0.0f;
                                return true;
                            }
                        }

                        if (m_GamePadStates[nControllerIndex].ThumbSticks.Left.X < -0.8f)
                        {
                            if (m_OldGamePadStates[nControllerIndex].ThumbSticks.Left.X >= -0.8f)
                            {
                                m_fControllerIdleTime[nControllerIndex] = 0.0f;
                                return true;
                            }
                        }

                        break;
                    }
                    case Direction.Right:
                    {
                        if (m_GamePadStates[nControllerIndex].DPad.Right == ButtonState.Pressed)
                        {
                            if (m_OldGamePadStates[nControllerIndex].DPad.Right == ButtonState.Released)
                            {
                                m_fControllerIdleTime[nControllerIndex] = 0.0f;
                                return true;
                            }
                        }

                        if (m_GamePadStates[nControllerIndex].ThumbSticks.Left.X > 0.8f)
                        {
                            if (m_OldGamePadStates[nControllerIndex].ThumbSticks.Left.X <= 0.8f)
                            {
                                m_fControllerIdleTime[nControllerIndex] = 0.0f;
                                return true;
                            }
                        }
                        break;
                    }
                }                
            }

            return false;
        }

        /** @fn     bool IsDirectionHeld( int nControllerIndex, Direction dir )
         *  @brief  check if a direction is being held on the controller
         *  @return true if the direction is held, false otherwise
         */
        public bool IsDirectionHeld( int nControllerIndex, Direction dir )
        {
            if (m_Locked == false)
            {
                switch( dir )
                {
                    case Direction.Up:
                        if( m_GamePadStates[ nControllerIndex ].DPad.Up == ButtonState.Pressed )
                        {
                            m_fControllerIdleTime[nControllerIndex] = 0.0f;
                            return true;
                        }
                        if (m_GamePadStates[nControllerIndex].ThumbSticks.Left.Y > 0.8f)
                        {
                            m_fControllerIdleTime[nControllerIndex] = 0.0f;
                            return true;
                        }
                        break;
                    case Direction.Down:
                        if (m_GamePadStates[nControllerIndex].DPad.Down == ButtonState.Pressed)
                        {
                            m_fControllerIdleTime[nControllerIndex] = 0.0f;
                            return true;
                        }
                        if (m_GamePadStates[nControllerIndex].ThumbSticks.Left.Y < -0.8f)
                        {
                            m_fControllerIdleTime[nControllerIndex] = 0.0f;
                            return true;
                        }
                        break;
                    case Direction.Right:
                        if (m_GamePadStates[nControllerIndex].DPad.Right == ButtonState.Pressed)
                        {
                            m_fControllerIdleTime[nControllerIndex] = 0.0f;
                            return true;
                        }
                        if (m_GamePadStates[nControllerIndex].ThumbSticks.Left.X > 0.8f)
                        {
                            m_fControllerIdleTime[nControllerIndex] = 0.0f;
                            return true;
                        }
                        break;
                    case Direction.Left:
                        if (m_GamePadStates[nControllerIndex].DPad.Left == ButtonState.Pressed)
                        {
                            m_fControllerIdleTime[nControllerIndex] = 0.0f;
                            return true;
                        }
                        if (m_GamePadStates[nControllerIndex].ThumbSticks.Left.X < -0.8f)
                        {
                            m_fControllerIdleTime[nControllerIndex] = 0.0f;
                            return true;
                        }
                        break;
                }
            }

            return false;
        }

        /** @fn     GamePadDPad CheckDPad(int ControllerIndex)
         *  @brief  check the Directional pad state of a controller
         *  @return the state of the D-pad on a given controller
         *  @param  ControllerIndex [in] the index of the controller to check [ 0 - 3 ]
         */
        public GamePadDPad CheckDPad(int ControllerIndex)
        {
            if (m_GamePadStates[ControllerIndex].DPad.Down == ButtonState.Pressed)
            {
                m_fControllerIdleTime[ControllerIndex] = 0.0f;
            }
            if (m_GamePadStates[ControllerIndex].DPad.Left == ButtonState.Pressed)
            {
                m_fControllerIdleTime[ControllerIndex] = 0.0f;
            }
            if (m_GamePadStates[ControllerIndex].DPad.Right == ButtonState.Pressed)
            {
                m_fControllerIdleTime[ControllerIndex] = 0.0f;
            }
            if (m_GamePadStates[ControllerIndex].DPad.Up == ButtonState.Pressed)
            {
                m_fControllerIdleTime[ControllerIndex] = 0.0f;
            }
            return m_GamePadStates[ControllerIndex].DPad;
        }

        /** @fn     Vector2 CheckLStick(int ControllerIndex)
         *  @brief  check the position of the left thumbstick, I believe this is a 2dimensional value ranging from -1 to 1 on either axis, where idle is 0, 0
         *  @return the 2D position of the thumbstick
         *  @param  ControllerIndex [in] the index of the controller to check
         */
        public Vector2 CheckLStick(int ControllerIndex)
        {
            if (m_GamePadStates[ControllerIndex].ThumbSticks.Left.Length() > 0.1f)
                m_fControllerIdleTime[ControllerIndex] = 0.0f;
            return m_GamePadStates[ControllerIndex].ThumbSticks.Left;
        }

        /** @fn     Vector2 CheckRStick(int ControllerIndex)
         *  @brief  check the position of the right thumbstick, I believe this is a 2dimensional value ranging from -1 to 1 on either axis, where idle is 0, 0
         *  @return the 2D position of the thumbstick
         *  @param  ControllerIndex [in] the index of the controller to check
         */   
        public Vector2 CheckRStick(int ControllerIndex)
        {
            if (m_GamePadStates[ControllerIndex].ThumbSticks.Right.Length() > 0.1f)
                m_fControllerIdleTime[ControllerIndex] = 0.0f;
            return m_GamePadStates[ControllerIndex].ThumbSticks.Right;
        }

        /** @fn     float CheckLTrigger(int ControllerIndex)
         *  @brief  check the pressed value of the left trigger of a given controller
         *  @return a float, I think between 0 and 1, where 1 is fully pressed, and 0 is fully released
         *  @param  ControllerIndex [in] the controller to check
         */
        public float CheckLTrigger(int ControllerIndex)
        {
            return m_GamePadStates[ControllerIndex].Triggers.Left;
        }

        /** @fn     float CheckRTrigger(int ControllerIndex)
         *  @brief  check the pressed value of the right trigger of a given controller
         *  @return a float, I think between 0 and 1, where 1 is fully pressed, and 0 is fully released
         *  @param  ControllerIndex [in] the controller to check
         */
        public float CheckRTrigger(int ControllerIndex)
        {
            return m_GamePadStates[ControllerIndex].Triggers.Right;
        }

        /////////////////////////////////////////
        //WINDOWS ONLY SECTION

        /** @fn     void LockKeyboard(Key)
         *  @brief  Checks all Inputs and stores old Inputs
         *  @author Tom
         */
        public void LockKeyboard(Keys Key)
        {
            m_EnableKey = Key;
            m_Locked = true;
        }

        /** @fn     bool IsKeyDown( Keys Key )  
         *  @brief  Check if a key is down
         *  @return true if it is, false otherwise
         *  @param  Key [in] the key to check
         */
        public bool IsKeyDown(Keys Key)
        {
            if (m_Locked == false)
            {
                return m_KeyState.IsKeyDown(Key);
            }
            return false;
        }

        /** @fn     bool IsKeyUp( Keys Key )
         *  @brief  check if a key is up
         *  @return true if the key is up, false otherwise
         *  @param  Key [in] the key to check
         */
        public bool IsKeyUp(Keys Key)
        {
            if (m_Locked == false)
            {
                return m_KeyState.IsKeyUp(Key);
            }
            return false;
        }

        /** @fn     bool IsKeyPressed( Keys Key )
         *  @brief  check if a key has just been pressed
         *  @return true on the one time when the previous state was up, and the new state is down
         *  @param  Key [in] the key to check
         */
        public bool IsKeyPressed(Keys Key)
        {
            if (m_Locked == false)
            {
                if (m_KeyState.IsKeyDown(Key))
                    if (m_OldKeyState.IsKeyUp(Key))
                        return true;
                return false;
            }
            return false;
        }

        /** @fn     bool IsKeyReleased( Keys Key )
         *  @brief  check if a key has just been released
         *  @return true if the previous state was down, and the new state is up
         *  @param  Key [in] the key to check
         */
        public bool IsKeyReleased(Keys Key)
        {
            if (m_Locked == false)
            {
                if (m_KeyState.IsKeyUp(Key))
                    if (m_OldKeyState.IsKeyDown(Key))
                        return true;
                return false;
            }
            return false;
        }
        /** @fn     bool IsKeyReleased( Keys Key )
            *  @brief  check if a key has just been released
            *  @return true if the previous state was down, and the new state is up
            *  @param  Key [in] the key to check
            */
        public bool IsAnyKeyPressed()
        {
            if (m_Locked == false)
            {
                if (m_Delay < 1)
                {
                    List<Keys> KeysPressed = new List<Keys>();
                    KeysPressed.AddRange(m_KeyState.GetPressedKeys());
                    List<Keys> OldKeysPressed = new List<Keys>();
                    OldKeysPressed.AddRange(m_OldKeyState.GetPressedKeys());
                    foreach (Keys key in OldKeysPressed)
                    {
                        if (KeysPressed.Contains(key))
                            KeysPressed.Remove(key);
                    }
                    if (KeysPressed.Count > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        //Returns the mouse position as a vector
        public Vector2 GetMousePosition() {
            Vector2 thePosition;
            thePosition.X = m_MouseState.X;
            thePosition.Y = m_MouseState.Y;
            return thePosition;
        }
        public Vector2 GetPrevMousePosition() {
            Vector2 thePosition;
            thePosition.X = m_OldMouseState.X;
            thePosition.Y = m_OldMouseState.Y;
            return thePosition;
        }
        //Returns the X value of the mouse position as a float
        public float GetMousePositionX() {
            return m_MouseState.X;
        }
        //Returns the Y value of the mouse position as a float
        public float GetMousePositionY() {
            return m_MouseState.Y;
        }
        //Checks if the Left Mouse Button is currently down
        public bool IsMouseLeftDown() 
        {
            if (m_MouseState.LeftButton == ButtonState.Pressed) {
                return true;
            }
            else { return false; }
        }
        //Checks if the Right Mouse Button is currently down
        public bool IsMouseRightDown() {
            if (m_MouseState.RightButton == ButtonState.Pressed) {
                return true;
            }
            else { return false; }
        }
        //Checks if the Middle Mouse Button is currently down
        public bool IsMouseMiddleDown() {
            if (m_MouseState.MiddleButton == ButtonState.Pressed) {
                return true;
            }
            else { return false; }
        }
        //Checks the current value of the mouse scroll wheel compared to the previous update
        public float MouseWheelChanged() {
            return m_MouseState.ScrollWheelValue - m_OldMouseState.ScrollWheelValue;
        }
        //Checks if the Left Mouse Button has just been pressed within the last update
        public bool IsMouseLeftPressed() {
            if (m_MouseState.LeftButton == ButtonState.Pressed) {
                if (m_OldMouseState.LeftButton == ButtonState.Released) {
                    return true;
                }
            }
            return false;
        }
        //Checks if the Right Mouse Button has just been pressed within the last update
        public bool IsMouseRightPressed() {
            if (m_MouseState.RightButton == ButtonState.Pressed) {
                if (m_OldMouseState.RightButton == ButtonState.Released) {
                    return true;
                }
            }
            return false;
        }
        //Checks if the Middle Mouse Button has just been pressed within the last update
        public bool IsMouseMiddlePressed() {
            if (m_MouseState.MiddleButton == ButtonState.Pressed) {
                if (m_OldMouseState.MiddleButton == ButtonState.Released) {
                    return true;
                }
            }
            return false;
        }
        //Checks if the Left Mouse Button has just been released within the last update
        public bool IsMouseLeftReleased() {
            if (m_MouseState.LeftButton == ButtonState.Released) {
                if (m_OldMouseState.LeftButton == ButtonState.Pressed) {
                    return true;
                }
            }
            return false;
        }
        //Checks if the Right Mouse Button has just been released within the last update
        public bool IsMouseRightReleased() {
            if (m_MouseState.RightButton == ButtonState.Released) {
                if (m_OldMouseState.RightButton == ButtonState.Pressed) {
                    return true;
                }
            }
            return false;
        }
        //Checks if the Middle Mouse Button has just been released within the last update
        public bool IsMouseMiddleReleased() {
            if (m_MouseState.MiddleButton == ButtonState.Released) {
                if (m_OldMouseState.MiddleButton == ButtonState.Pressed) {
                    return true;
                }
            }
            return false;
        }
        //Gets/sets the texture displayed as the mouse cursor
        public Texture2D theMouseTexture{
            get { return m_mouseTexture; }
            set { m_mouseTexture = value; }
        }
        //Gets/sets the current state of the mouse itself
        public MouseState theMouseState {
            get { return m_MouseState; }
            set { m_MouseState = value; }
        }
    }
}