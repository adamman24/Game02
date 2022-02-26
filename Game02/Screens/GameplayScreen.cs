using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Game02.StateManagement;
using Game02.Collisions;

namespace Game02.Screens
{
    //this will be a game where a quick brown fox jumps over a lazy dog
    class GameplayScreen : GameScreen
    {
        private ContentManager _content;
        private Fox _fox;
        private Dog _dog;
        private Background _background;

        private Vector2 _playerPosition = new Vector2(100, 390);
        private Vector2 _enemyPosition = new Vector2(400, 400);

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;
        private Song backgroundMusic;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);

            _fox = new Fox();
            _dog = new Dog();
            _background = new Background();
        }

        public override void Activate()
        {
            if (_content == null)
            {
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            }
            _fox.LoadContent(_content);
            //_fox.Position = _playerPosition;
            _dog.LoadContent(_content);
            _dog.Position = _enemyPosition;
            _background.LoadContent(_content);

            ScreenManager.Game.ResetElapsedTime();
            backgroundMusic = _content.Load<Song>("BackgroundMusic");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);
            _fox.Update(gameTime);

            if(_fox.Bounds.CollidesWith(_dog.Bounds))
            {
                _fox.color = Color.Red;
            }

        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                var movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.Right))
                    movement.X++;

                var thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;

                if (movement.Length() > 1)
                    movement.Normalize();

                _playerPosition += movement * 8f;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            var _spriteBatch = ScreenManager.SpriteBatch;
            _background.Draw(gameTime, _spriteBatch);
            _fox.Draw(gameTime, _spriteBatch);
            _dog.Draw(gameTime, _spriteBatch);
        }
    }
}
