using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Game02
{
    public class Background
    {
        //layer textures
        private Texture2D _background;

        public void LoadContent(ContentManager content)
        {
            _background = content.Load<Texture2D>("FullBackground");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(0, 0, 2048, 1546);
            Vector2 position = new Vector2(0, 100);
            spriteBatch.Begin();
            spriteBatch.Draw(_background, Vector2.Zero, rect, Color.White, 0, position, .4f, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
