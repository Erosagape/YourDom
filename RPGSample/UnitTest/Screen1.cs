using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPGSample
{
    class Screen1 : MenuScreen
    {
        MenuEntry startMenu;
        string captionText;
        public Screen1()
        {
            startMenu = new MenuEntry("Press Start Button");
            startMenu.Description = "";
            startMenu.Font = Fonts.CaptionFont;
            startMenu.Selected += StartMenu_Selected;
            captionText = "This is First Screen";
        }

        private void StartMenu_Selected(object sender, EventArgs e)
        {
            captionText = "Welcome Back";
            ScreenManager.AddScreen(new Screen2());
        }        
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch=ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            SpriteFont captionFont = Fonts.HeaderFont;
            graphics.Clear(Color.Transparent);
            Vector2 captionPos = new Vector2(
                (viewport.Width - captionFont.MeasureString(captionText).X) / 2,
                ((viewport.Height - captionFont.MeasureString(captionText).Y) / 2)-200f
                );
            startMenu.Position = new Vector2(
                (viewport.Width - Fonts.CaptionFont.MeasureString(startMenu.Text).X) / 2,
                (viewport.Height - Fonts.CaptionFont.MeasureString(startMenu.Text).Y) / 2
                );
            spriteBatch.Begin();
            spriteBatch.DrawString(captionFont, captionText, captionPos, Color.White);
            startMenu.Draw(this, true, gameTime);
            spriteBatch.End();
        }
        public override void HandleInput()
        {
            if (InputManager.IsActionTriggered(InputManager.Action.Ok))
            {
                startMenu.OnSelectEntry();
            }
            if (InputManager.IsActionTriggered(InputManager.Action.Back))
                ScreenManager.Game.Exit();
        }
    }
}
