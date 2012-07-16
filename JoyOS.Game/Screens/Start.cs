using System;
using JoyOS.System.Controls;
using JoyOS.System.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace JoyOS.Game.Screens
{
    public class StartScreen : GameScreen
    {
        #region Properties & Variables

        private Texture2D _background;
        private ContentManager _content;
        private Texture2D _highScoreClicked;
        private Texture2D _highScoreNormal;
        private Texture2D _instructionsClicked;
        private Texture2D _instructionsNormal;
        private Rectangle _viewport;
        private PanelControl _pnlMenu;

        #endregion

        #region Initialization and Load

        public StartScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            EnabledGestures = GestureType.Tap;
        }

        public override void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _background = ScreenManager.Content.Load<Texture2D>("Common/gradient");
            _instructionsNormal = ScreenManager.Content.Load<Texture2D>("instructions_off");
            _instructionsClicked = ScreenManager.Content.Load<Texture2D>("instructions_on");
            _highScoreNormal = ScreenManager.Content.Load<Texture2D>("highscore_off");
            _highScoreClicked = ScreenManager.Content.Load<Texture2D>("highscore_on");

            UpdateScreen();

            CreateMenu();

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
            _pnlMenu.HandleInput(input);
            base.HandleInput(input, gameTime);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            //Update Menu states
            _pnlMenu.Update(gameTime);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(_background, _viewport, Color.White);
            ScreenManager.SpriteBatch.End();

            Control.BatchDraw(_pnlMenu, ScreenManager.GraphicsDevice, ScreenManager.SpriteBatch, Vector2.Zero, gameTime);
        }

        #endregion

        #region Menu

        private void CreateMenu()
        {
            //Initialize MenuPanel
            _pnlMenu = new PanelControl {Position = new Vector2(_viewport.Width/2 - 160, _viewport.Height/2 - 100)};

            //Add MenuItems
            //Instructions
            var miInstructions = new Button(ScreenManager.Game)
                                     {
                                         Width = 320,
                                         Height = 100,
                                         NormalButtonTexture = _instructionsNormal,
                                         ClickedButtonTexture = _instructionsClicked,
                                         Position = new Vector2(0, 0)
                                     };

            //Highscore
            var miHighScore = new Button(ScreenManager.Game)
                                  {
                                      Width = 320,
                                      Height = 100,
                                      NormalButtonTexture = _highScoreNormal,
                                      ClickedButtonTexture = _highScoreClicked,
                                      Position = new Vector2(0, 100)
                                  };

            //Event Handlers
            miInstructions.OnClicked += miInstructions_OnClicked;
            miHighScore.OnClicked += miHighScore_OnClicked;

            //Add MenuItems to Menupanel
            _pnlMenu.AddChild(miInstructions);
            _pnlMenu.AddChild(miHighScore);
        }


        private void miInstructions_OnClicked(Button sender)
        {
            ScreenManager.AddScreen(new InstructionsScreen());
        }

        private void miHighScore_OnClicked(Button sender)
        {
            ScreenManager.AddScreen(new HighScoreScreen());
        }

        #endregion
    }
}