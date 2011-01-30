using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Firecracker_Engine {

    public delegate CBaseObject CreateObjectDelegate(ObjectDefinition objDef, ObjectDefinition objOverwriteDefinition);

	public class Firecracker : Microsoft.Xna.Framework.Game {

		public static Firecracker engineInstance;
		public static GameSettings settings;
		public static ScreenManager screenManager;
		public static CommandInterpreter interpreter;
		public static ControlSystem controlSystem;
		public static Menu menu;
		public static GameConsole console;
		public GraphicsDeviceManager graphics;
		public SpriteBatch spriteBatch;
		public static SpriteSheetCollection spriteSheets;
		public static SpriteAnimationCollection animations;
		public static Level level;
		public static Random random;
        public CameraBase theCamera;
        public Forest m_Forest;

        //temp values. delete before submission
        public KeyboardState thekeyboard;
        public int baseticks = 0;
        public int maxticks = 100;

        public int numPeoples = 1;
        public int timeInSeconds = 0;
        public int timeInMinutes = 0;

        public double elapsedTime = 0;
        public double[] maxTime = new double[2];



        public static Random theRandom = new Random();

        public MouseManager m_MouseManager;

        public static CreateObjectDelegate CreateObjectDelegate = null;
        private static CreateObjectDelegate EngineCreateObjectDelegate = new CreateObjectDelegate(CreateObject);

        public List<CBaseObject> m_lObjectList;
        // Add any lists here that hold references to this one.
        // Examples of engine level object lists would be
        // List<CObjectProxy> m_lMoveableList; // The list of all objects with the moveable type.
        // List<CObjectProxy> m_lPlayerList; // The list of all objects with the Player type

		public Firecracker() {
			engineInstance = this;

			settings = new GameSettings();
			screenManager = new ScreenManager();
			graphics = new GraphicsDeviceManager(this);
			interpreter = new CommandInterpreter();
			controlSystem = new ControlSystem();
			menu = new Menu();
			console = new GameConsole();
			random = new Random();
			Content.RootDirectory = "Content";

            m_lObjectList = new List<CBaseObject>();
            m_MouseManager = new MouseManager();

            //more temp
            thekeyboard = new KeyboardState();

            m_Forest = new Forest();

		}

		protected override void Initialize() {
			// load the game settings from file
			settings.loadFrom(Content.RootDirectory + "/" + GameSettings.defaultFileName);

			// set the screen resolution
			graphics.PreferredBackBufferWidth = settings.screenWidth;
			graphics.PreferredBackBufferHeight = settings.screenHeight;
			graphics.ApplyChanges();

			// set the screen attributes / full screen mode
			Window.AllowUserResizing = false;
			if(settings.fullScreen) {
				// NOTE: may cause access violations in dual screen situations
				graphics.ToggleFullScreen();
			}

			controlSystem.initialize();
			menu.initialize();
			console.initialize();
            m_MouseManager.Initialize();
            UIScreenManager.CreateInstance();

            DefinitionManager.LoadDefinitions("Content\\Objects");

            theCamera = new CameraBase();
            theCamera.initialize();

            maxTime[0] = 2;
            maxTime[1] = 60;

			base.Initialize();
		}

		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// parse and load all sprite sheets
			spriteSheets = SpriteSheetCollection.parseFrom(Content.RootDirectory + "/" + settings.spriteSheetFileName, Content);

			// parse and load all sprite animations
			animations = SpriteAnimationCollection.readFrom(Content.RootDirectory + "/" + settings.animationsFileName, spriteSheets);


			// load game content
			menu.loadContent(Content);

			console.loadContent(Content);
		}

		public void toggleFullScreen() {
			graphics.ToggleFullScreen();
			settings.fullScreen = graphics.IsFullScreen;
		}

		public bool loadLevel(string levelName) {
			if(levelName == null) { return false; }

			Level newLevel = Level.readFrom(Content.RootDirectory + "\\Levels\\" + levelName + ".2d");

            PopulationManager tempPopulationManagerRef = new PopulationManager();

			if(newLevel != null) {
                
                level = newLevel;
                elapsedTime = 0;
                numPeoples = 1;
                List<GameObject> Sky = new List<GameObject>();
                for (int i = 1; i < 4; i++)
                {
                    Cloud fluffyAndWhite = new Cloud(i);
                    fluffyAndWhite.position = new Vector2(Firecracker.random.Next(0, Firecracker.level.dimensions.X * Firecracker.level.gridSize), Firecracker.random.Next(0, Firecracker.level.dimensions.Y * Firecracker.level.gridSize));
                    Sky.Add(fluffyAndWhite);
                }

                level.addObjects(Sky);
                m_Forest.Initialize(Firecracker.spriteSheets);
                Ocean water = new Ocean();
                water.position = new Vector2(10, 10);
                level.addObject(water);

				return true;
			}
			return false;
		}

		public bool levelLoaded() {
			return level != null;
		}

		public virtual void handleInput(GameTime gameTime) {

            controlSystem.handleInput(gameTime);
            m_MouseManager.UpdateMouse();
		}

		public virtual void updateGame(GameTime gameTime) {
			if(levelLoaded()) {
				level.update(gameTime);
			}

			foreach(CBaseObject objRef in m_lObjectList) {
				objRef.Tick(gameTime);
			}
		}

		protected override void Update(GameTime gameTime) {

            
            

            if (baseticks < maxticks)
                thekeyboard = new KeyboardState();
            else
                baseticks = 0;

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			base.Draw(gameTime);
		}

		protected override void OnExiting(object sender, EventArgs args) {
			// update the game settings file with changes
			settings.saveTo(Content.RootDirectory + "/" + GameSettings.defaultFileName);

			base.OnExiting(sender, args);
		}

		public bool CreateObjectByName(string sObjectName) {
			// TODO: implement this.
			// similar to CreateObjectByDefinition, except that this
			// no properties are overloaded.
			ObjectDefinition objDef = DefinitionManager.QueryDefinitionByName(sObjectName);
			if(objDef.ObjectName.Length != 0 && objDef.ObjectClassType.Length != 0)
				return CreateObjectByDefinition(objDef);
			return false;
		}
        /// <summary>
        /// TimeRemaining takes a digit from the time and subtracts it from one to the other lol
        /// </summary>
        /// <param name="maxNum">The maximum amount of time</param>
        /// <param name="minNum">The current amount of time</param>
        /// <returns>0 if the number is negative, or the difference between the maximum and the current</returns>
        public int TimeRemaining(double maxNum, double curNum)
        {
            //if (maxNum < curNum)
            //    return 0;
            //else
                return (int)(maxNum - curNum);
        }

		public bool CreateObjectByDefinition(ObjectDefinition ObjectDefinition) {
			CBaseObject newObject = null;
			ObjectDefinition DefaultDef = DefinitionManager.QueryDefinitionByName(ObjectDefinition.ObjectClassType);
			// Try to load the game object first.
			if(CreateObjectDelegate != null) {
				newObject = CreateObjectDelegate(DefaultDef, ObjectDefinition);
			}

			// if the game object could not be loaded then this is likely a engine defined object.
			if(newObject == null) {
				newObject = EngineCreateObjectDelegate(DefaultDef, ObjectDefinition);
			}

			if(newObject != null) {
				Firecracker.engineInstance.AddObjectToList(newObject);
			}
			else {
				// This is not an object that actually exists.
				System.Diagnostics.Debug.Assert(false, "Uh oh. The level defined an object that doesn't exist.");
				return false;
			}
			return true;
		}

		public CBaseObject FindObjectByName(string sObjectName) {
			foreach(CBaseObject obj in m_lObjectList) {
				if(obj.ObjectName.Equals(sObjectName))
					return obj;
			}
			return null;
		}
		public List<CBaseObject> FindObjectsByType(string sType) {
			List<CBaseObject> returnList = new List<CBaseObject>();
			foreach(CBaseObject obj in m_lObjectList) {
				if(obj.ObjectType.Equals(sType))
					returnList.Add(obj);
			}
			return returnList;
		}

		public void AddObjectToList(CBaseObject obj) {
			m_lObjectList.Add(obj);
		}

		/*public bool loadLevel(string levelName) {
			if(levelName == null) { return false; }

            LevelLoader.LoadLevel(levelName);

            foreach (CBaseObject obj in m_lObjectList)
            {
                obj.LoadResources();
            }

            foreach (CBaseObject obj in m_lObjectList)
            {
                obj.OnBeginGameplay();
            }

			return false;
		}

		public bool levelLoaded() {
			return false;
		}
		*/

        public static CBaseObject CreateObject(ObjectDefinition objDef, ObjectDefinition objOverwriteDefinition)
        {
            // This is a list of all the object types in the engine. 
            // To complete this list with the GameDefined types overload
            // This class and this method in the game.
            CBaseObject returnObject = null;
            switch (objDef.ObjectClassType)
            {
                case CBaseObject.ClassName:
                    {
                        CBaseObject newObject = new CBaseObject();
                        newObject.LoadPropertiesList(objDef);
                        newObject.LoadPropertiesList(objOverwriteDefinition);
                        returnObject = newObject;
                    }
                    break;
                case CMoveableObject.ClassName:
                    {
                        CMoveableObject newObject = new CMoveableObject();
                        newObject.LoadPropertiesList(objDef);
                        newObject.LoadPropertiesList(objOverwriteDefinition);
                        returnObject = newObject;
                    }
                    break;

                

                case CameraBase.ClassName:
                    {
                        CameraBase newObject = new CameraBase();
                        newObject.LoadPropertiesList(objDef);
                        newObject.LoadPropertiesList(objOverwriteDefinition);
                        returnObject = newObject;
                    }
                    break;
					/*
                case Level.ClassName:
                    {
                        Level newObject = new Level();
                        newObject.LoadPropertiesList(objDef);
                        newObject.LoadPropertiesList(objOverwriteDefinition);
                        returnObject = newObject;
                    }
                    break;
					 * */
                default:
                    return null;
            }

            return returnObject;
        }

        /// <summary>
        /// Forces the time to output itself as a string.
        /// </summary>
        /// <param name="input">The number of seconds as a double</param>
        /// <returns>The formatted time string</returns>
        public string makeTimeString(double input)
        {
            int minutes = 0;
            double d_Seconds;

            if (input > 60)
            {
                d_Seconds = input;
                d_Seconds = d_Seconds / 60;

                if (d_Seconds > 1)
                {
                    minutes = (int)d_Seconds;
                }
                d_Seconds = input - (60 * minutes);
            }
            else
                d_Seconds = input;

            return minutes.ToString() + ":" + ((int)d_Seconds).ToString();
        }

	};
}
