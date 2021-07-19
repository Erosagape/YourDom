using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RPGSample
{
    class Session
    {
        private static Session singleton;
        /// <summary>
        /// The ScreenManager used to manage all UI in the game.
        /// </summary>
        private ScreenManager screenManager;

        /// <summary>
        /// The ScreenManager used to manage all UI in the game.
        /// </summary>
        public static ScreenManager ScreenManager
        {
            get { return (singleton == null ? null : singleton.screenManager); }
        }
        /// <summary>
        /// Returns true if there is an active session.
        /// </summary>
        public static bool IsActive
        {
            get { return singleton != null; }
        }

        /// <summary>
        /// The GameplayScreen object that created this session.
        /// </summary>
        private GameplayScreen gameplayScreen;
        /// <summary>
        /// Save game descriptions for the current set of save games.
        /// </summary>
        private static List<SaveGameDescription> saveGameDescriptions = null;

        /// <summary>
        /// Save game descriptions for the current set of save games.
        /// </summary>
        public static List<SaveGameDescription> SaveGameDescriptions
        {
            get { return saveGameDescriptions; }
        }


        /// <summary>
        /// The maximum number of save-game descriptions that the list may hold.
        /// </summary>
        public const int MaximumSaveGameDescriptions = 5;
        public Session(ScreenManager screenManager, GameplayScreen gameplayScreen)
        {
            this.screenManager = screenManager;
            this.gameplayScreen = gameplayScreen;
        }

        /// <summary>
        /// End the current session.
        /// </summary>
        public static void EndSession()
        {
            // exit the gameplay screen
            // -- store the gameplay session, for re-entrance
            if (singleton != null)
            {
                GameplayScreen gameplayScreen = singleton.gameplayScreen;
                singleton.gameplayScreen = null;

                // pop the music
                AudioManager.PopMusic();

                // clear the singleton
                singleton = null;

                if (gameplayScreen != null)
                {
                    gameplayScreen.ExitScreen();
                }
            }
        }

        /// <summary>
        /// XML serializer for SaveGameDescription objects.
        /// </summary>
        private static XmlSerializer saveGameDescriptionSerializer =
            new XmlSerializer(typeof(SaveGameDescription));


        /// <summary>
        /// Refresh the list of save-game descriptions.
        /// </summary>
        public static void RefreshSaveGameDescriptions()
        {
            // clear the list
            saveGameDescriptions = null;

            // retrieve the storage device, asynchronously
            //GetStorageDevice(RefreshSaveGameDescriptionsResult);
        }

        /// <summary>
        /// Start a new session based on the data provided.
        /// </summary>
        public static void StartNewSession(GameStartDescription gameStartDescription,
            ScreenManager screenManager, GameplayScreen gameplayScreen)
        {
            /*
            // check the parameters
            if (gameStartDescription == null)
            {
                throw new ArgumentNullException("gameStartDescripton");
            }
            if (screenManager == null)
            {
                throw new ArgumentNullException("screenManager");
            }
            if (gameplayScreen == null)
            {
                throw new ArgumentNullException("gameplayScreen");
            }

            // end any existing session
            EndSession();

            // create a new singleton
            singleton = new Session(screenManager, gameplayScreen);

            // set up the initial map
            ChangeMap(gameStartDescription.MapContentName, null);

            // set up the initial party
            ContentManager content = singleton.screenManager.Game.Content;
            singleton.party = new Party(gameStartDescription, content);

            // load the quest line
            singleton.questLine = content.Load<QuestLine>(
                Path.Combine(@"Quests\QuestLines",
                gameStartDescription.QuestLineContentName)).Clone() as QuestLine;
            */
        }
        /// <summary>
        /// Start a new session, using the data in the given save game.
        /// </summary>
        /// <param name="saveGameDescription">The description of the save game.</param>
        /// <param name="screenManager">The ScreenManager for the new session.</param>
        public static void LoadSession(SaveGameDescription saveGameDescription,
            ScreenManager screenManager, GameplayScreen gameplayScreen)
        {
            /*
            // check the parameters
            if (saveGameDescription == null)
            {
                throw new ArgumentNullException("saveGameDescription");
            }
            if (screenManager == null)
            {
                throw new ArgumentNullException("screenManager");
            }
            if (gameplayScreen == null)
            {
                throw new ArgumentNullException("gameplayScreen");
            }

            // end any existing session
            EndSession();

            // create the new session
            singleton = new Session(screenManager, gameplayScreen);

            // get the storage device and load the session
            GetStorageDevice(
                delegate (StorageDevice storageDevice)
                {
                    LoadSessionResult(storageDevice, saveGameDescription);
                });
            */
        }

        internal static void Draw(GameTime gameTime)
        {
            /*
            SpriteBatch spriteBatch = singleton.screenManager.SpriteBatch;

            if (CombatEngine.IsActive)
            {
                // draw the combat background
                if (TileEngine.Map.CombatTexture != null)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(TileEngine.Map.CombatTexture, Vector2.Zero,
                        Color.White);
                    spriteBatch.End();
                }

                // draw the combat itself
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                CombatEngine.Draw(gameTime);
                spriteBatch.End();
            }
            else
            {
                singleton.DrawNonCombat(gameTime);
            }

            singleton.hud.Draw();
            */
        }

        private void DrawNonCombat(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// Update the session for this frame.
        /// </summary>
        /// <remarks>This should only be called if there are no menus in use.</remarks>
        public static void Update(GameTime gameTime)
        {
            /*
            // check the singleton
            if (singleton == null)
            {
                return;
            }

            if (CombatEngine.IsActive*)
            {
                CombatEngine.Update(gameTime);
            }
            else
            {
                singleton.UpdateQuest();
                TileEngine.Update(gameTime);
            }
            */
        }

        private void UpdateQuest()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Delete the save game specified by the description.
        /// </summary>
        /// <param name="saveGameDescription">The description of the save game.</param>
        public static void DeleteSaveGame(SaveGameDescription saveGameDescription)
        {
            /*
            // check the parameters
            if (saveGameDescription == null)
            {
                throw new ArgumentNullException("saveGameDescription");
            }

            // get the storage device and load the session
            GetStorageDevice(
                delegate (StorageDevice storageDevice)
                {
                    DeleteSaveGameResult(storageDevice, saveGameDescription);
                });
            */
        }
        /// <summary>
        /// Save the current state of the session.
        /// </summary>
        /// <param name="overwriteDescription">
        /// The description of the save game to over-write, if any.
        /// </param>
        public static void SaveSession(SaveGameDescription overwriteDescription)
        {
            // retrieve the storage device, asynchronously
            /*
            GetStorageDevice(delegate (StorageDevice storageDevice)
            {
                SaveSessionResult(storageDevice, overwriteDescription);
            });
            */
        }
    }
}
