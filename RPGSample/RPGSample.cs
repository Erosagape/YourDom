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
        bool isLoadFinished;
        float totalSec = 0;
        string second = "";
        bool isClick = false;
        #endregion
        //Contructor of this class
        public RPGSample()
        {
            //set instance of variables
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //set default value
            isLoadFinished = false;

            arrayText = new string[] {
                "Agapesoft Game Studio",
                "Presents",
                "A Game by Erosagape",
                "Project Yourdom"
            };
            titleText = GetTitle();            
        }
        //Function called for current text display
        string GetTitle()
        {
            string str = arrayText[textID];
            return str;
        }
        //initial value
        protected override void Initialize()
        {
            //set full screen
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            base.Initialize();
        }
        //when game exit
        protected override void UnloadContent()
        {
            base.UnloadContent();   
        }
        //when games load
        protected override void LoadContent()
        {
            //inital content
            spriteBatch = new SpriteBatch(GraphicsDevice);
            titleFont = Content.Load<SpriteFont>("Fonts/Title");
            titleImage = Content.Load<Texture2D>("Texture/splashscreen");
            base.LoadContent();
        }
        //when users take any actions
        protected override void Update(GameTime gameTime)
        {
            //check input state changes
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Mouse.GetState().LeftButton==ButtonState.Pressed||Keyboard.GetState().IsKeyDown(Keys.Enter))
            {                
                isClick = true;                                
            }
            
            base.Update(gameTime);
        }
        //after update value then draw content
        protected override void Draw(GameTime gameTime)
        {            
            GraphicsDevice.Clear(Color.Transparent);
            
            if (!isLoadFinished)
            {
                totalSec += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (second != totalSec.ToString("0"))
                {
                    second = totalSec.ToString("0");
                    if (isClick)
                    {
                        isClick = false;
                        if (textID < arrayText.Length - 1)
                        {
                            textID += 1;
                        }
                        else
                        {
                            isLoadFinished = true;
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
            else
            {
                if (!isClick)
                {
                    isClick = false;
                    spriteBatch.Begin();
                    Rectangle rect = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
                    spriteBatch.Draw(titleImage, rect, Color.White);
                    spriteBatch.End();
                } else
                {
                    this.Exit();
                }
            }            
            base.Draw(gameTime);
        }
    }
}
