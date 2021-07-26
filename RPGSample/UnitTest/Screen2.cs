using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPGSample
{
    public class Screen2 : GameScreen
    {
        public Screen2()
        {
        }
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            SpriteFont captionFont = Fonts.PlayerStatisticsFont;
            string captionText = "This is Second Screen";
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
        }
    }
}
