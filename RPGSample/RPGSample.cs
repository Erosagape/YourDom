using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
namespace RPGSample
{
    public class RPGSample : Game
    {
        #region public variables
        public GraphicsDeviceManager graphics;  //interface of Graphics Device
        public SpriteBatch spriteBatch;  //interface of sprite drawing engine
        #endregion

        #region private variables
        private SpriteFont titleFont;   //font title
        private Texture2D titleImage;   //image title

        string titleText;
        int textID;
        string[] arrayText;
        float totalSec = 0;
        string second = "";

        bool isClick = false;

        ScreenManager screenManager;
        #endregion
        //Contructor of this class
        public RPGSample()
        {
            //set instance of variables
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //set default value
            arrayText = new string[] {           
                "DHRUVA INTERACTIVE",
                "Presents",
                "A Game Developed by MonoGame Framework",
                @"""RPG SAMPLE"""
            };

            // add the audio manager
            AudioManager.Initialize(this, @"Content\Audio\RPGAudio.xgs",
                @"Content\Audio\Wave Bank.xwb", @"Content\Audio\Sound Bank.xsb");

            // add the screen manager
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
        }
        //Function called for current text display
        string GetTitle()
        {
            string str = arrayText[textID-1];
            return str;
        }
        //initial value
        protected override void Initialize()
        {
            InputManager.Initialize();
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
            base.Initialize();
        }
        //when game exit
        protected override void UnloadContent()
        {
            Fonts.UnloadContent();
            base.UnloadContent();   
        }
        //when games load
        protected override void LoadContent()
        {
            Fonts.LoadContent(Content);
            //inital content
            spriteBatch = new SpriteBatch(GraphicsDevice);
            titleFont = Content.Load<SpriteFont>("Fonts/Title");
            titleImage = Content.Load<Texture2D>("Texture/splashscreen");
            base.LoadContent();
        }
        //when users take any actions
        protected override void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton==ButtonState.Pressed||Keyboard.GetState().IsKeyDown(Keys.Enter))
            {                
                isClick = true;                                
            }
            InputManager.Update();
            base.Update(gameTime);
        }
        //after update value then draw content
        protected override void Draw(GameTime gameTime)
        {            
            GraphicsDevice.Clear(Color.Transparent);
            if (textID == 0)
            {
                totalSec += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (second != totalSec.ToString("0"))
                {
                    second = totalSec.ToString("0");
                    if (isClick)
                    {
                        textID = 1;
                        isClick = false;
                    }
                }
                spriteBatch.Begin();
                Rectangle rect = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
                spriteBatch.Draw(titleImage, rect, Color.White);
                spriteBatch.End();
            } else
            {
                totalSec += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (second != totalSec.ToString("0"))
                {
                    second = totalSec.ToString("0");
                    if (isClick)
                    {
                        isClick = false;
                        if (textID < arrayText.Length)
                        {
                            textID += 1;
                        }
                        else
                        {
                            if (screenManager.GetScreens().Length == 0)
                            {
                                screenManager.AddScreen(new MainMenuScreen());
                                return;
                            }
                        }
                    }                    
                }
                spriteBatch.Begin();
                titleText = GetTitle();
                float fontX = titleFont.MeasureString(titleText).X;
                float fontY = titleFont.MeasureString(titleText).Y;
                spriteBatch.DrawString(titleFont, titleText, new Vector2((GraphicsDevice.Viewport.Width - fontX) / 2, (GraphicsDevice.Viewport.Height - fontY) / 2), Color.White);
                spriteBatch.End();
            }            
            base.Draw(gameTime);
        }
    }
}
