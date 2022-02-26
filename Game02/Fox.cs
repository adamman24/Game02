using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using tainicom.Aether.Physics2D.Dynamics;
using Game02.Collisions;

namespace Game02
{
    public enum Direction
    {
        Idle = 0,
        Right = 2,
        Jump = 3
    };

    public class Fox
    {
        // The position of the player
        public Vector2 Position = new Vector2(100, 390);
        // The texture atlas for the fox sprite
        private Texture2D _texture;

        //timer for animation work
        private double animationTimer;

        //timer for animation frames
        private short animationFrame = 1;

        public Direction Direction;

        private SoundEffect jumpSound;

        private bool flip = false;
        private bool walking = false;
        private KeyboardState previousState;
        private KeyboardState currentState;
        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;

        public Color color { get; set; }

        public Fox()
        {
            this.bounds = new BoundingRectangle(Position + new Vector2(32, 32), 32, 32);
        }

        /// <summary>
        /// Loads the player texture atlas
        /// </summary>
        /// <param name="content">The ContentManager to use to load the content</param>
        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("BrownFox");
            jumpSound = content.Load<SoundEffect>("Jump");
        }

        public void Update(GameTime gameTime)
        {
            walking = false;
            //jumpTimer += gameTime.ElapsedGameTime.TotalSeconds;
            previousState = currentState;
            currentState = Keyboard.GetState();
            var keyboardState = Keyboard.GetState();
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                Position -= Vector2.UnitX * 200 * t;
                flip = true;
                walking = true;
                Direction = Direction.Right;
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                Position += Vector2.UnitX * 200 * t;
                flip = false;
                walking = true;
                Direction = Direction.Right;
            }
            if (previousState.IsKeyUp(Keys.Up) && keyboardState.IsKeyDown(Keys.Up) || 
                previousState.IsKeyUp(Keys.W) && keyboardState.IsKeyDown(Keys.W))
            {
                Direction = Direction.Jump;
                jumpSound.Play();
            }
            if (keyboardState.IsKeyDown(Keys.Up) ||
                keyboardState.IsKeyDown(Keys.W))
            {
                Direction = Direction.Jump;
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                //Position += Vector2.UnitY * 200 * t;
                walking = true;
                Direction = Direction.Right;
            }
            
            if(walking)
            {
                Direction = Direction.Right;
            }else
                Direction = Direction.Idle;
            //updating bounds
            bounds.X = Position.X;
            bounds.Y = Position.Y;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > 0.1)
            {
                animationFrame++;
                if (animationFrame >= 5)
                {
                    animationFrame = 1;
                }
                animationTimer -= 0.1;
            }

            var source = new Rectangle(animationFrame * 32, (int)Direction * 32, 32, 32);
            spriteBatch.Begin();
            if(flip)
            {
                spriteBatch.Draw(_texture, Position, source, Color.White, 0.0f, Vector2.Zero, 2.5f, SpriteEffects.FlipHorizontally, 0);
            }else
            {
                spriteBatch.Draw(_texture, Position, source, Color.White, 0.0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0);
            }
            
            spriteBatch.End();
        }
    }
}
