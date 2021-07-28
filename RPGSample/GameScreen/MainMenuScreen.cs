using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RPGSample
{
    class MainMenuScreen : MenuScreen
    {
        private Texture2D backgroundTexture;
        private Vector2 backgroundPosition;
        private Texture2D descriptionAreaTexture;
        private Vector2 descriptionAreaPosition;
        private Vector2 descriptionAreaTextPosition;

        private Texture2D iconTexture;
        private Vector2 iconPosition;

        private Texture2D backTexture;
        private Vector2 backPosition;

        private Texture2D selectTexture;
        private Vector2 selectPosition;

        private Texture2D plankTexture1, plankTexture2, plankTexture3;
        MenuEntry newGameMenuEntry, exitGameMenuEntry;
        MenuEntry saveGameMenuEntry, loadGameMenuEntry;
        MenuEntry controlsMenuEntry, helpMenuEntry;

        public MainMenuScreen()
            : base()
        {
            // add the New Game entry
            newGameMenuEntry = new MenuEntry("New Game");
            newGameMenuEntry.Description = "Start a New Game";
            newGameMenuEntry.Font = Fonts.HeaderFont;
            newGameMenuEntry.Position = new Vector2(715, 0f);
            newGameMenuEntry.Selected += NewGameMenuEntrySelected;
            MenuEntries.Add(newGameMenuEntry);

            // add the Save Game menu entry, 
            // if the game has started but is not in combat
            if (Session.IsActive && !CombatEngine.IsActive)
            {
                saveGameMenuEntry = new MenuEntry("Save Game");
                saveGameMenuEntry.Description = "Save the Game";
                saveGameMenuEntry.Font = Fonts.HeaderFont;
                saveGameMenuEntry.Position = new Vector2(730, 0f);
                saveGameMenuEntry.Selected += SaveGameMenuEntrySelected;
                MenuEntries.Add(saveGameMenuEntry);
            }
            else
            {
                saveGameMenuEntry = null;
            }

            // add the Load Game menu entry
            loadGameMenuEntry = new MenuEntry("Load Game");
            loadGameMenuEntry.Description = "Load the Game";
            loadGameMenuEntry.Font = Fonts.HeaderFont;
            loadGameMenuEntry.Position = new Vector2(700, 0f);
            loadGameMenuEntry.Selected += LoadGameMenuEntrySelected;
            MenuEntries.Add(loadGameMenuEntry);

            // add the Controls menu entry
            controlsMenuEntry = new MenuEntry("Controls");
            controlsMenuEntry.Description = "View Game Controls";
            controlsMenuEntry.Font = Fonts.HeaderFont;
            controlsMenuEntry.Position = new Vector2(720, 0f);
            controlsMenuEntry.Selected += ControlsMenuEntrySelected;
            MenuEntries.Add(controlsMenuEntry);

            // add the Help menu entry
            helpMenuEntry = new MenuEntry("Help");
            helpMenuEntry.Description = "View Game Help";
            helpMenuEntry.Font = Fonts.HeaderFont;
            helpMenuEntry.Position = new Vector2(700, 0f);
            helpMenuEntry.Selected += HelpMenuEntrySelected;
            MenuEntries.Add(helpMenuEntry);

            // create the Exit menu entry
            exitGameMenuEntry = new MenuEntry("Exit");
            exitGameMenuEntry.Description = "Quit the Game";
            exitGameMenuEntry.Font = Fonts.HeaderFont;
            exitGameMenuEntry.Position = new Vector2(720, 0f);
            exitGameMenuEntry.Selected += OnCancel;
            MenuEntries.Add(exitGameMenuEntry);
            AudioManager.PushMusic("MainTheme");
        }

        private void SaveGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new SaveLoadScreen(SaveLoadScreen.SaveLoadScreenMode.Save));
        }

        private void NewGameMenuEntrySelected(object sender, EventArgs e)
        {
            if (Session.IsActive)
            {
                ExitScreen();
            }

            ContentManager content = ScreenManager.Game.Content;            
            var startGameDesc = new XmlManager<GameStartDescription>("StartGame").Get();
            if (startGameDesc == null)
            {
                ExitScreen();
            }

            LoadingScreen.Load(ScreenManager, true, new GameplayScreen(startGameDesc));
        }

        private void LoadGameMenuEntrySelected(object sender, EventArgs e)
        {
            SaveLoadScreen loadGameScreen = new SaveLoadScreen(SaveLoadScreen.SaveLoadScreenMode.Load);
            loadGameScreen.LoadingSaveGame += new SaveLoadScreen.LoadingSaveGameHandler(loadGameScreen_LoadingSaveGame);
            ScreenManager.AddScreen(loadGameScreen);
        }

        private void loadGameScreen_LoadingSaveGame(SaveGameDescription saveGameDescription)
        {
            if (Session.IsActive)
            {
                ExitScreen();
            }
            LoadingScreen.Load(ScreenManager, true,
                new GameplayScreen(saveGameDescription));
        }

        private void ControlsMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new ControlsScreen());
        }

        private void HelpMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new HelpScreen());
        }

        private void OnCancel(object sender, EventArgs e)
        {
            ExitGameScreen();
        }
        private void ExitGameScreen()
        {
            // add a confirmation message box
            string message = String.Empty;
            if (Session.IsActive)
            {
                message = "Are you sure you want to exit?  All unsaved progress will be lost.";
            }
            else
            {
                message = "Are you sure you want to exit?";
            }
            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;
            ScreenManager.AddScreen(confirmExitMessageBox);
        }
        private void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            AudioManager.PopMusic();
            ScreenManager.Game.Exit();
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;
            backgroundTexture = content.Load<Texture2D>(@"Texture\MainMenu\MainMenu");
            // calculate the texture positions
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            backgroundPosition = new Vector2(
                (viewport.Width - backgroundTexture.Width) / 2,
                (viewport.Height - backgroundTexture.Height) / 2);

            descriptionAreaTexture =
                content.Load<Texture2D>(@"Texture\MainMenu\MainMenuInfoSpace");
            iconTexture = content.Load<Texture2D>(@"Texture\MainMenu\GameLogo");
            plankTexture1 =
                content.Load<Texture2D>(@"Texture\MainMenu\MainMenuPlank");
            plankTexture2 =
                content.Load<Texture2D>(@"Texture\MainMenu\MainMenuPlank02");
            plankTexture3 =
                content.Load<Texture2D>(@"Texture\MainMenu\MainMenuPlank03");
            backTexture = content.Load<Texture2D>(@"Texture\Buttons\BButton");
            selectTexture = content.Load<Texture2D>(@"Texture\Buttons\AButton");
           
            descriptionAreaPosition = backgroundPosition + new Vector2(158, 130);
            descriptionAreaTextPosition = backgroundPosition + new Vector2(158, 350);

            iconPosition = backgroundPosition + new Vector2(170, 80);
            backPosition = backgroundPosition + new Vector2(225, 610);
            selectPosition = backgroundPosition + new Vector2(1120, 610);

            // set the textures on each menu entry
            newGameMenuEntry.Texture = plankTexture3;

            if (saveGameMenuEntry != null)
            {
                saveGameMenuEntry.Texture = plankTexture2;
            }

            loadGameMenuEntry.Texture = plankTexture1;
            controlsMenuEntry.Texture = plankTexture2;
            helpMenuEntry.Texture = plankTexture3;
            exitGameMenuEntry.Texture = plankTexture1;

            // now that they have textures, set the proper positions on the menu entries
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntries[i].Position = new Vector2(
                    MenuEntries[i].Position.X,
                    500f - ((MenuEntries[i].Texture.Height - 10) *
                        (MenuEntries.Count - 1 - i)));
            }
            base.LoadContent();
        }
        /// <summary>
        /// Handles user input.
        /// </summary>
        public override void HandleInput()
        {
            if (InputManager.IsActionTriggered(InputManager.Action.Back))
            {
                ExitGameScreen();
                return;
            }

            base.HandleInput();
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            // draw the background images
            spriteBatch.Draw(backgroundTexture, backgroundPosition, Color.White);
            spriteBatch.Draw(descriptionAreaTexture, descriptionAreaPosition,
                Color.White);
            spriteBatch.Draw(iconTexture, iconPosition, Color.White);

            // Draw each menu entry in turn.
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntry menuEntry = MenuEntries[i];
                bool isSelected = IsActive && (i == selectedEntry);
                menuEntry.Draw(this, isSelected, gameTime);
            }

            // draw the description text for the selected entry
            MenuEntry selectedMenuEntry = SelectedMenuEntry;
            if ((selectedMenuEntry != null) &&
                !String.IsNullOrEmpty(selectedMenuEntry.Description))
            {
                Vector2 textSize =
                    Fonts.DescriptionFont.MeasureString(selectedMenuEntry.Description);
                Vector2 textPosition = descriptionAreaTextPosition + new Vector2(
                    (float)Math.Floor((descriptionAreaTexture.Width - textSize.X) / 2),
                    0f);
                spriteBatch.DrawString(Fonts.DescriptionFont,
                    selectedMenuEntry.Description, textPosition, Color.White);
            }

            // draw the select instruction
            spriteBatch.Draw(selectTexture, selectPosition, Color.White);
            spriteBatch.DrawString(Fonts.ButtonNamesFont, "Select",
                new Vector2(
                selectPosition.X - Fonts.ButtonNamesFont.MeasureString("Select").X - 5,
                selectPosition.Y + 5), Color.White);


            // if we are in-game, draw the back instruction
            if (Session.IsActive)
            {
                spriteBatch.Draw(backTexture, backPosition, Color.White);
                spriteBatch.DrawString(Fonts.ButtonNamesFont, "Resume",
                    new Vector2(backPosition.X + 55, backPosition.Y + 5), Color.White);
            }
            spriteBatch.End();
        }
    }
}
