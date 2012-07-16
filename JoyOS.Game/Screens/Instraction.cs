using System;
using JoyOS.System.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JoyOS.Game.Screens
{
    public class InstructionsScreen : GameScreen
    {
        #region Properties & Variables

        private ContentManager _content;

        private Texture2D _texture;
        private Rectangle _viewport;

        #endregion

        #region Initialization and Load

        public InstructionsScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            IsPopup = true;
        }

        public override void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _texture = ScreenManager.Content.Load<Texture2D>("Common/gradient");

            UpdateScreen();

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            _content.Unload();
        }

        private void UpdateScreen()
        {
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            _viewport = new Rectangle(0, 0, viewport.Width, viewport.Height);
        }

        #endregion

        #region Game Methods

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            // Allows popup to be closed by back button
            if (IsActive && GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                ExitScreen();
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(_texture,
                                           new Vector2(_viewport.Width/2 - _texture.Width/2,
                                                       _viewport.Height/2 - _texture.Height/2), Color.White);
            ScreenManager.SpriteBatch.End();
        }

        #endregion
    }
}