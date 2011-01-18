using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Firecracker_Engine {

	public class GameConsole {

		// special locking booleans
		private bool m_active = false;
		private bool m_consoleKeyPressed = false;
		private bool m_enterKeyPressed = false;
		private bool m_backspaceKeyPressed = false;
		private bool m_deleteKeyPressed = false;
		private bool m_leftKeyPressed = false;
		private bool m_rightKeyPressed = false;
		private bool m_homeKeyPressed = false;
		private bool m_endKeyPressed = false;
		private bool m_escKeyPressed = false;
		private bool m_pasteKeyPressed = false;

		// input and history variables
		private string m_input;
		private List<string> m_outputHistory;
		private List<string> m_inputHistory;
		private int m_maxOutputHistory = 512;
		private int m_maxInputHistory = 128;
		
		// a list of valid input keys for the console, along with their character representations and a locking boolean to prevent key repeating
		private static Keys[] m_inputKeys = new Keys[] {
			Keys.Space, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0,
			Keys.OemMinus, Keys.OemPlus, Keys.OemOpenBrackets, Keys.OemCloseBrackets, Keys.OemBackslash,
			Keys.OemSemicolon, Keys.OemQuotes, Keys.OemComma, Keys.OemPeriod, Keys.OemQuestion,
			Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I,
			Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R,
			Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z
		};
		private static string m_upperCaseChar = " !@#$%^&*()_+{}|:\"<>?ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private static string m_lowerCaseChar = " 1234567890-=[]\\;\',./abcdefghijklmnopqrstuvwxyz";
		private bool[] m_inputKeyPressed;

		// console gui-related variables
		private Rectangle m_dimensions;
		private int m_padding = 10;
		private Vector2 m_charSize;

		// console cursor animation variables
		private int m_cursorPos = 0;
		private float m_cursorBlinkSpeed = 0.7f;
		private float m_cursorBlinkTime = 0.0f;
		private bool m_cursorVisible = true;

		// console font and colour variables
		private SpriteFont m_font;
		private Color m_fontColour = new Color(256, 256, 256);
		private Color m_borderColour = new Color(255, 0, 0);
		private Color m_backgroundColour = new Color(0, 0, 0, 128);

		public GameConsole() {
			m_input = "";
			m_outputHistory = new List<string>();
			m_inputHistory = new List<string>();
		}

		public bool active {
			get { return m_active; }
			set { m_active = value; }
		}

		public void toggle() { m_active = !m_active; }

		public void open() { m_active = true; }

		public void close() { m_active = false; }

		// initialize the game console
		public void initialize() {
			m_dimensions = new Rectangle(m_padding,
										 m_padding,
										 Firecracker.settings.screenWidth - (m_padding * 2),
										 Firecracker.settings.screenHeight - (m_padding * 2));
			m_inputKeyPressed = new bool[m_inputKeys.Length];
		}

		// load console-related content
		public void loadContent(ContentManager content) {
			m_font = content.Load<SpriteFont>("Fonts/ConsoleFont");
			m_charSize = m_font.MeasureString("M");
		}

		// write a line to the console
		public void writeLine(string text) {
			if(text == null) { return; }
			m_outputHistory.Add(text);
		}

		// reset the cursor animation
		public void resetCursorAnimation() {
			m_cursorBlinkTime = 0.0f;
			m_cursorVisible = true;
		}

		public void handleInput(GameTime gameTime) {
			KeyboardState keyboard = Keyboard.GetState();
			MouseState mouse = Mouse.GetState();

			// remember if the console was already open or not
			bool consoleWasOpen = m_active;

			// allow the console to be toggled with tilde
			if(keyboard.IsKeyDown(Keys.OemTilde)) {
				if(!m_consoleKeyPressed) {
					Firecracker.interpreter.execute("console toggle");
					m_consoleKeyPressed = true;
				}
			} else { m_consoleKeyPressed = false; }

			// allow the console to be closed with escape
            if(keyboard.IsKeyDown(Keys.Escape)) {
                if(!m_escKeyPressed){
					Firecracker.interpreter.execute("console hide");
                    m_escKeyPressed = true;
                }
            } else { m_escKeyPressed = false; }

			// if the console was already open, do not handle any other input
			if(!consoleWasOpen) { return; }

			// if the user pressed enter, allow the current input string to be parsed and interpreted (and store it in the history)
			if(keyboard.IsKeyDown(Keys.Enter)) {
				if(!m_enterKeyPressed) {
					// trim history if it is full
					if(m_outputHistory.Count() >= m_maxOutputHistory) {
						m_outputHistory.RemoveAt(0);
					}

					if(m_inputHistory.Count() >= m_maxInputHistory) {
						m_inputHistory.RemoveAt(0);
					}

					m_outputHistory.Add(m_input);
					m_inputHistory.Add(m_input);

					// execute the command
					Firecracker.interpreter.execute(m_input);

					// clear the input and reset animations
					m_input = "";
					m_cursorPos = 0;
					resetCursorAnimation();

					m_enterKeyPressed = true;

					return;
				}
			}
			else { m_enterKeyPressed = false; }
			
			// allow the user to navigate the cursor left
			if(keyboard.IsKeyDown(Keys.Left)) {
				if(!m_leftKeyPressed) {
					if(m_cursorPos > 0) { m_cursorPos--; }
					resetCursorAnimation();
					m_leftKeyPressed = true;
				}
			} else { m_leftKeyPressed = false; }

			// allow the user to navigate the cursor right
			if(keyboard.IsKeyDown(Keys.Right)) {
				if(!m_rightKeyPressed) {
					if(m_cursorPos < m_input.Length) { m_cursorPos++; }
					resetCursorAnimation();
					m_rightKeyPressed = true;
				}
			} else { m_rightKeyPressed = false; }

			// allow the user to navigate the cursor to the beginning of the line
			if(keyboard.IsKeyDown(Keys.Home)) {
				if(!m_homeKeyPressed) {
					m_cursorPos = 0;
					resetCursorAnimation();
					m_homeKeyPressed = true;
				}
			} else { m_homeKeyPressed = false; }

			// allow the user to navigate the cursor to the end of the line
			if(keyboard.IsKeyDown(Keys.End)) {
				if(!m_endKeyPressed) {
					m_cursorPos = m_input.Length;
					resetCursorAnimation();
					m_endKeyPressed = true;
				}
			} else { m_endKeyPressed = false; }

			// allow the user to backspace at the cursor's current position
			if(keyboard.IsKeyDown(Keys.Back)) {
				if(!m_backspaceKeyPressed) {
					if(m_input.Length > 0 && m_cursorPos > 0) {
						m_input = m_input.Substring(0, m_cursorPos - 1)
								+ m_input.Substring(m_cursorPos, m_input.Length - m_cursorPos);
						m_cursorPos--;
					}
					resetCursorAnimation();
					m_backspaceKeyPressed = true;
				}
			} else { m_backspaceKeyPressed = false; }

			// allow the user to delete at the cursor's current position
			if(keyboard.IsKeyDown(Keys.Delete)) {
				if(!m_deleteKeyPressed) {
					if(m_input.Length > 0 && m_cursorPos < m_input.Length) {
						m_input = m_input.Substring(0, m_cursorPos)
								+ m_input.Substring(m_cursorPos + 1, m_input.Length - m_cursorPos - 1);
					}
					resetCursorAnimation();
					m_deleteKeyPressed = true;
				}
			} else { m_deleteKeyPressed = false; }

            // if ctrl + v or insert is pressed, paste the text from the keyboard
			if(keyboard.IsKeyDown(Keys.Insert) ||
			   ((keyboard.IsKeyDown(Keys.LeftControl) || keyboard.IsKeyDown(Keys.RightControl)) && keyboard.IsKeyDown(Keys.V))) {
				if(!m_pasteKeyPressed) {
					// append the text to the appropriate place in the string (based on where the cursor is located)
					if(m_cursorPos == m_input.Length) {
						m_input += ClipboardManager.getClipboardText();
					}
					else {
						m_input = m_input.Substring(0, m_cursorPos)
                                + ClipboardManager.getClipboardText()
                                + m_input.Substring(m_cursorPos, m_input.Length - m_cursorPos);
					}
					// increment cursor position
					m_cursorPos += ClipboardManager.getClipboardText().Length;
					m_pasteKeyPressed = true;
				}
			} else { m_pasteKeyPressed = false; }

			// prevent v from being registered
			if(m_pasteKeyPressed) {
				return;
			}

			bool upperCase = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);

			// TODO: fix backslash key not registering

			// allow the keys the user presses to show up as input in the console using brute force checks
			for(int i=0;i<m_inputKeys.Length;i++) {
				// check to see if a corresponding input key is pressed
				if(keyboard.IsKeyDown(m_inputKeys[i])) {
					// ensure that the key is not already pressed
					if(!m_inputKeyPressed[i]) {
						// get the input character (and check to see if shift is pressed, and modify the char appropriately)
						char inputChar = (upperCase) ? m_upperCaseChar[i] : m_lowerCaseChar[i];

						// append the char to the appropriate place in the string (based on where the cursor is located)
						if(m_cursorPos == m_input.Length) {
							m_input += inputChar;
						}
						else {
							m_input = m_input.Substring(0, m_cursorPos)
									+ inputChar
									+ m_input.Substring(m_cursorPos, m_input.Length - m_cursorPos);
						}

						// increment the cursor position and set the key as pressed
						m_cursorPos++;
						m_inputKeyPressed[i] = true;
					}
				}
				else { m_inputKeyPressed[i] = false; }
			}
		}

		public void update(GameTime gameTime) {
			if(!m_active) { return; }

			// animate the cursor
			m_cursorBlinkTime += (float) gameTime.ElapsedGameTime.TotalSeconds;

			if(m_cursorBlinkTime >= m_cursorBlinkSpeed) {
				m_cursorVisible = !m_cursorVisible;
				m_cursorBlinkTime = 0;
			}
		}

		public void clear() {
			m_outputHistory.Clear();
		}

		public void reset() {
			m_cursorPos = 0;
			m_input = "";

			resetCursorAnimation();

			m_inputHistory.Clear();
			m_outputHistory.Clear();
		}

		public void draw(SpriteBatch spriteBatch) {
			if(!m_active) { return; }

			// the bottom-left starting position of the console
			Vector2 pos = new Vector2(m_dimensions.Left, m_dimensions.Bottom - m_charSize.Y);

			// render the input query character
			spriteBatch.DrawString(m_font, ">", pos, m_borderColour);
			spriteBatch.DrawString(m_font, m_input, pos + new Vector2(8, 0), m_fontColour);

			// render the cursor (if it is visible)
			if(m_cursorVisible) {
				spriteBatch.DrawString(m_font, "_", pos + new Vector2(8 + (m_cursorPos * m_charSize.X), 0), m_borderColour);
			}

			// render the commands that have already been input into the console
			pos.Y -= 2;
			pos.X += 8;
			for(int i=m_outputHistory.Count()-1;i>=0;i--) {
				pos.Y -= m_charSize.Y;
				if(pos.Y < m_dimensions.Top) { break; }
				spriteBatch.DrawString(m_font, m_outputHistory[i], pos, m_fontColour);
			}
		}

	}

}
