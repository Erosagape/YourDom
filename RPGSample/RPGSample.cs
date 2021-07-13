using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
namespace RPGSample
{
    public class RPGSample : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont titleFont;
        string titleText;
        int textID;
        string[] arrayText;
        public RPGSample()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            arrayText = new string[] {
                "Agapesoft Game Studio",
                "Presents",
                "A Game by Erosagape",
                "Project Yourdom"
            };
            titleText = GetTitle();
        }
        string GetTitle()
        {

            string str = arrayText[textID];
            return str;
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            base.Initialize();
        }
        protected override void UnloadContent()
        {
            base.UnloadContent();   
        }
        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            titleFont = Content.Load<SpriteFont>("Fonts/Title");
            // TODO: use this.Content to load your game content here
            base.LoadContent();
        }
        float totalSec = 0;
        string second = "";
        bool isClick = false;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Mouse.GetState().LeftButton==ButtonState.Pressed||Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                isClick = true;
            }
            totalSec += (float)gameTime.ElapsedGameTime.TotalSeconds;
            // TODO: Add your update logic here           
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            if (second != totalSec.ToString("0"))
            {
                second = totalSec.ToString("0");
                if (isClick)
                {
                    if (textID < arrayText.Length-1)
                        textID += 1;
                    isClick = false;
                }
            }
            titleText = GetTitle();
            float fontX = titleFont.MeasureString(titleText).X;
            float fontY = titleFont.MeasureString(titleText).Y;
            spriteBatch.DrawString(titleFont, titleText, new Vector2((GraphicsDevice.Viewport.Width-fontX)/2, (GraphicsDevice.Viewport.Height-fontY)/2), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
