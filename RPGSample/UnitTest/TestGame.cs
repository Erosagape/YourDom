using Microsoft.Xna.Framework;
namespace RPGSample
{
    public class TestGame : Game
    {
        ScreenManager screenManager;
        GraphicsDeviceManager graphics;
        public TestGame()
        {
            screenManager = new ScreenManager(this);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Components.Add(screenManager);
        }
        protected override void Initialize()
        {
            InputManager.Initialize();
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            Fonts.LoadContent(Content);
            screenManager.AddScreen(new Screen1());
            base.LoadContent();
        }
        protected override void UnloadContent()
        {
            Fonts.UnloadContent();
            base.UnloadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {            
            base.Draw(gameTime);
        }
    }
}
