using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPGSample
{
    public class Screen2 : GameScreen
    {
        string captionText = "This is Second Screen";
        public Screen2()
        {
        }
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            SpriteFont captionFont = Fonts.PlayerStatisticsFont;

            Vector2 pos= new Vector2(
                (viewport.Width - Fonts.CaptionFont.MeasureString(captionText).X) / 2,
                (viewport.Height - Fonts.CaptionFont.MeasureString(captionText).Y) / 2
                );
            graphics.Clear(Color.Transparent);
            spriteBatch.Begin();
            spriteBatch.DrawString(captionFont,captionText,pos,Color.White);
            spriteBatch.End();
        }
        public override void HandleInput()
        {
            if (InputManager.IsActionTriggered(InputManager.Action.Back))
                ExitScreen();
            if (InputManager.IsActionTriggered(InputManager.Action.Ok))
                StartGame();
        }
        private void StartGame()
        {
            UnitTest.TestSession.StartNewGame(ScreenManager.Game.Content.RootDirectory);
            GameStartDescription gameStartDescription = UnitTest.TestSession.GetStartDescription("MainGameDescription");
            captionText = "Hello," + gameStartDescription.PlayerContentNames[0];
            Map startMap = UnitTest.TestSession.GetMapData("Maps/Map001");
            captionText += ". Welcome to " + startMap.Name;
        }
        private void TestStartGame()
        {
            string contentPath = ScreenManager.Game.Content.RootDirectory + "/GameData";
            UnitTest.TestSession.StartNewGame(contentPath);
            UnitTest.TestSession.TestSaveMapPosition();
            if (!System.IO.File.Exists(contentPath + "/StartGame.xml"))
            {
                UnitTest.TestSession.TestSaveGameDescription("StartGame");
            }            
            GameStartDescription startParams = new UnitTest.TestReader<GameStartDescription>(contentPath).Read("StartGame");
            //Map startMap = new UnitTest.TestReader<Map>(contentPath + "/Map").Read(startParams.MapContentName);
            //captionText = "Hello," + startParams.PlayerContentNames[0] + ". Welcome to " + startMap.Name;
            Map startMap = UnitTest.TestSession.TestGetMapData("Maps/"+startParams.MapContentName);
            captionText = "Hello," + startParams.PlayerContentNames[0] + ". Welcome to " + startMap.Name;
        }
    }
}
