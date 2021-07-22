using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RPGSample
{
    public class TitleScreen : GameScreen
    {
        private SpriteFont titleFont;   //font title
        private Texture2D titleImage;   //image title

        string titleText;
        int textID;
        string[] arrayText;
        float totalSec = 0;
        string second = "";

        bool isClick = false;
        MainMenuScreen mainMenu;

        public TitleScreen()
            :base()
        {
            //set default value
            arrayText = new string[] {
                "DHRUVA INTERACTIVE",
                "Presents",
                "A Game Developed by MonoGame Framework",
                @"""RPG SAMPLE"""
            };
        }
        //Function called for current text display
        string GetTitle()
        {
            string str = arrayText[textID - 1];
            return str;
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            var graphics = ScreenManager.GraphicsDevice;
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
                Rectangle rect = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);
                spriteBatch.Draw(titleImage, rect, Color.White);
                spriteBatch.End();
            }
            else
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
                            ExitScreen();
                        }
                    }
                }
                spriteBatch.Begin();
                titleText = GetTitle();
                float fontX = titleFont.MeasureString(titleText).X;
                float fontY = titleFont.MeasureString(titleText).Y;
                spriteBatch.DrawString(titleFont, titleText, new Vector2((graphics.Viewport.Width - fontX) / 2, (graphics.Viewport.Height - fontY) / 2), Color.White);
                spriteBatch.End();
            }
        }
        public override void HandleInput()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                isClick = true;
            }
        }
        public override void LoadContent()
        {
            titleFont = ScreenManager.Game.Content.Load<SpriteFont>("Fonts/Title");
            titleImage = ScreenManager.Game.Content.Load<Texture2D>("Texture/splashscreen");
        }
    }
}
