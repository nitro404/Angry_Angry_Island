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
		public static Level level;

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
			Content.RootDirectory = "Content";

            m_lObjectList = new List<CBaseObject>();
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
            UIScreenManager.CreateInstance();

            DefinitionManager.LoadDefinitions("Content\\Objects");

			base.Initialize();
		}

        public bool CreateObjectByName(string sObjectName)
        {
            // TODO: implement this.
            // similar to CreateObjectByDefinition, except that this
            // no properties are overloaded.
            ObjectDefinition objDef = DefinitionManager.QueryDefinitionByName(sObjectName);
            if (objDef.ObjectName.Length != 0 && objDef.ObjectClassType.Length != 0)
                return CreateObjectByDefinition(objDef);
            return false;
        }

        public bool CreateObjectByDefinition(ObjectDefinition ObjectDefinition)
        {
            CBaseObject newObject = null;
            ObjectDefinition DefaultDef = DefinitionManager.QueryDefinitionByName(ObjectDefinition.ObjectClassType);
            // Try to load the game object first.
            if (CreateObjectDelegate != null)
            {
                newObject = CreateObjectDelegate(DefaultDef, ObjectDefinition);
            }

            // if the game object could not be loaded then this is likely a engine defined object.
            if (newObject == null)
            {
                newObject = EngineCreateObjectDelegate(DefaultDef, ObjectDefinition);
            }

            if (newObject != null)
            {
                Firecracker.engineInstance.AddObjectToList(newObject);
            }
            else
            {
                // This is not an object that actually exists.
                System.Diagnostics.Debug.Assert(false, "Uh oh. The level defined an object that doesn't exist.");
                return false;
            }
            return true;
        }

        public CBaseObject FindObjectByName(string sObjectName)
        {
            foreach (CBaseObject obj in m_lObjectList)
            {
                if (obj.ObjectName.Equals(sObjectName))
                    return obj;
            }
            return null;
        }

        public void AddObjectToList(CBaseObject obj)
        {
            m_lObjectList.Add(obj);
        }

		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// parse and load all sprite sheets
			spriteSheets = SpriteSheetCollection.parseFrom(Content.RootDirectory + "/" + settings.spriteSheetFileName, Content);

			// load game content
			menu.loadContent(Content);

			console.loadContent(Content);
		}

		public void toggleFullScreen() {
			graphics.ToggleFullScreen();
			settings.fullScreen = graphics.IsFullScreen;
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

		public bool loadLevel(string levelName) {
			if(levelName == null) { return false; }

			Level newLevel = Level.readFrom(Content.RootDirectory + "\\Levels\\" + levelName + ".2d");

			if(newLevel != null) {
				level = newLevel;
				return true;
			}
			return false;
		}

		public bool levelLoaded() {
			return level != null;
		}

		public virtual void handleInput(GameTime gameTime) {
			// handle game-related input, and update the game
			controlSystem.handleInput(gameTime);
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

	};
}
