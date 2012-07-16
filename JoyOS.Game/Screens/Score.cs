using System;
using JoyOS.System.Controls;
using JoyOS.System.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JoyOS.Game.Screens
{
    public class HighScoreScreen : GameScreen
    {
        #region Properties & Variables

        private Texture2D _background;
        private ContentManager _content;
        private Rectangle _viewport;

        private ScrollingPanelControl _pnl;

        #endregion

        #region Initialization and Load

        public HighScoreScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            EnabledGestures = ScrollTracker.GesturesNeeded;
        }

        public override void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _background = ScreenManager.Content.Load<Texture2D>("Common/gradient");

            UpdateScreen();
            //ScreenManager..ProjectionUpdated += UpdateScreen;

            //Add Header
            var foregroundColor = new Color(0, 128, 192);
            _pnl = new ScrollingPanelControl();

            int i = 10;
            _pnl.AddChild(new TextControl("HIGHSCORE", ScreenManager.Fonts.MenuSpriteFont, foregroundColor,
                                         new Vector2(
                                             _viewport.Width/2f -
                                             ScreenManager.Fonts.MenuSpriteFont.MeasureString("HIGHSCORE").X/2, i)));
            i += 60;

            //Add Titles
            _pnl.AddChild(new TextControl("PLAYER", ScreenManager.Fonts.MenuSpriteFont, foregroundColor,
                                         new Vector2(30, i)));
            _pnl.AddChild(new TextControl("SCORE", ScreenManager.Fonts.MenuSpriteFont, foregroundColor,
                                         new Vector2(200, i)));
            _pnl.AddChild(new TextControl("DATE", ScreenManager.Fonts.MenuSpriteFont, foregroundColor,
                                         new Vector2(400, i)));
            i += 45;

            //Add Random scores
            var r = new Random();
            for (int j = 0; j < 50; j++)
            {
                _pnl.AddChild(new TextControl(j.ToString(), ScreenManager.Fonts.MenuSpriteFont, foregroundColor,
                                             new Vector2(30, i)));
                _pnl.AddChild(new TextControl(r.Next(10000, 99999).ToString(), ScreenManager.Fonts.MenuSpriteFont,
                                             foregroundColor, new Vector2(200, i)));
                _pnl.AddChild(
                    new TextControl(
                        DateTime.Now.Subtract(TimeSpan.FromDays(r.Next(1, 30))).ToString("dd MMM yyyy HH:MM"),
                        ScreenManager.Fonts.MenuSpriteFont, foregroundColor, new Vector2(400, i)));
                i += 30;
            }

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

        public override void HandleInput(InputHelper input, GameTime gameTime)
        {
            _pnl.HandleInput(input);
            base.HandleInput(input, gameTime);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            // Allows popup to be closed by back button
            if (IsActive && GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                ExitScreen();
            }

            _pnl.Update(gameTime);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(_background, _viewport, Color.White);
            ScreenManager.SpriteBatch.End();

            Control.BatchDraw(_pnl, ScreenManager.GraphicsDevice, ScreenManager.SpriteBatch, Vector2.Zero, gameTime);
        }

        #endregion
    }
}