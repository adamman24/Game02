using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Game02.Collisions;

namespace Game02
{
    public class Dog
    {
        private Texture2D texture;
        private double animationTimer;
        private short animationFrame = 5;
        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;

        public Vector2 Position { get; set; }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("LazyDog");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if(animationTimer > .5)
            {
                animationFrame++;
                if(animationFrame > 9)
                {
                    animationFrame = 4;
                }
                animationTimer -= .5;
            }
            this.bounds = new BoundingRectangle(new Vector2(400,400), 128, 128);
            var source = new Rectangle(animationFrame * 128, 0, 128, 128);
            spriteBatch.Begin();
            spriteBatch.Draw(texture, Position, source, Color.White, 0, Vector2.Zero, .6f, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
