using System;
using System.Collections.Generic;
using System.Text;
using FarseerPhysics.Dynamics.Contacts;
using JoyOS.Game.Model;
using JoyOS.Game.Model.Factories;
using JoyOS.GameModel;
using JoyOS.GameModel.Maps;
using JoyOS.System.Components;
using JoyOS.System.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JoyOS.Game
{
    public class SomeGame : PhysicsGameScreen, IDemoScreen
    {
        //Farseer objects
        private Rectangle _background;
        private IBird _heroBird;
        private List<ParallaxBackground> layers;
        private List<IGameObject> _obj;
        private List<Text> _textInfo;
        private int _totalPoints;

        #region IDemoScreen Members

        public string GetTitle()
        {
            return "Some game";
        }

        public string GetDetails()
        {
            var sb = new StringBuilder();
            sb.AppendLine("TODO: Add sample description!");
            sb.AppendLine(string.Empty);
            sb.AppendLine("GamePad:");
            sb.AppendLine("  - Exit to menu: Back button");
            sb.AppendLine(string.Empty);
            sb.AppendLine("Keyboard:");
            sb.AppendLine("  - Exit to menu: Escape");
            return sb.ToString();
        }

        #endregion

        public override void LoadContent()
        {
            base.LoadContent();
            World.Gravity = new Vector2(0f, 10f);

            HasCursor = true;
            EnableCameraControl = true;
            HasVirtualStick = true;
            _totalPoints = 0;
            _textInfo = new List<Text>();

            ScreenManager screen = ScreenManager;
            //load textures
            screen.Assets.LoadContent(screen.Content, typeof (BirdType), "Birds");
            screen.Assets.LoadContent(screen.Content, typeof (PigType), "Pigs");

            screen.Assets.LoadContent(screen.Content, typeof (Material), typeof (BlockType), "Blocks");
            screen.Assets.LoadContent(screen.Content, "Materials/blank", "Blank");

            var m = ScreenManager.Content.Load<Map>("map");
            var f = new ObjectFactory(World, m.Settings, this);
            _obj = new List<IGameObject>();

            World.ContactManager.PostSolve += PostSolve;

            var width = (int) ConvertUnits.ToSimUnits(m.MapWidth);
            var height = (int) ConvertUnits.ToSimUnits(m.MapHeigth);
            _heroBird = MapHelper.LoadMapToGame(m, World, f, _obj);
            //Create Floor
            MapHelper.SetBorder(m, World, width, height);

            _background = new Rectangle(0, 0, (int) m.MapWidth, (int) m.MapHeigth);

            //layers = new List<ParallaxBackground>(4);

            //AddLayer(width, height, 0f, 0, "Layers/layer0");
            //AddLayer(width, height, 1f, 0, "Layers/layer1");
            //AddLayer(width, height, 2f, 0, "Layers/layer2");
            //AddLayer(width, height, 3f, 0, "Layers/layer3");

            Camera.TrackingBody = _heroBird.Body;
            SetUserAgent(_heroBird.Body, 50f, 50f);
            //Camera.MaxPosition = new Vector2(width, height);
            Camera.MinPosition = new Vector2(0, 0);

            Camera.EnableTracking = true;
            Camera.EnableRotationTracking = false;

            Camera.Jump2Target();
        }

        private void AddLayer(int width, int height, float speedX, float speedY, string textureName)
        {
            var lst = new List<Texture2D> {ScreenManager.Content.Load<Texture2D>(textureName)};
            layers.Add(new ParallaxBackground(lst, ParallaxDirection.Horizontal)
                           {
                               Height = height,
                               Width = width,
                               SpeedX = speedX,
                               SpeedY = speedY
                           });
        }

        private void PostSolve(Contact contact, ContactConstraint contactConstraint)
        {
            if (contact.IsTouching())
            {
                float maxImpulse = 0.0f;
                for (int i = 0; i < contactConstraint.Manifold.PointCount; ++i)
                    maxImpulse = Math.Max(maxImpulse, contactConstraint.Manifold.Points[i].NormalImpulse);
                if (maxImpulse >= 12)
                {
                    IGameObject aGameObj = contactConstraint.BodyA.UserData as IGameObject,
                                bGameObj = contactConstraint.BodyB.UserData as IGameObject;
                    if (aGameObj != null)
                    {
                        aGameObj.Health -= maxImpulse;
                        if (aGameObj.Health <= 0)
                        {
                            RemoveObject(aGameObj);
                        }
                    }
                    if (bGameObj != null)
                    {
                        bGameObj.Health -= maxImpulse;
                        if (bGameObj.Health <= 0)
                        {
                            RemoveObject(bGameObj);
                        }
                    }
                }
            }
        }

        private void RemoveObject(IGameObject aGameObj)
        {
            if (_obj.Contains(aGameObj))
            {
                var points = (int) Math.Abs(aGameObj.Health);
                _totalPoints += points;
                _textInfo.Add(new Text
                                 {
                                     Output = points.ToString(),
                                     Position = ConvertUnits.ToDisplayUnits(aGameObj.Body.Position),
                                     Time = 500
                                 });
                World.RemoveBody(aGameObj.Body);
                _obj.Remove(aGameObj);
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            ////Update the Background
            //foreach (ParallaxBackground b in layers)
            //{
            //    b.Move();
            //}
            for (int i = 0; i < _textInfo.Count; i++)
            {
                _textInfo[i].Time -= gameTime.ElapsedGameTime.Milliseconds;
                if (_textInfo[i].Time <= 0)
                {
                    _textInfo.RemoveAt(i);
                }
            }
            foreach (IGameObject o in _obj)
            {
                o.Update();
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(InputHelper input, GameTime gameTime)
        {
            //MouseState mouse = input.MouseState;
            //int tp = mouse.ScrollWheelValue/100;
            //Camera.Zoom = tp < 1 ? 1 : tp;
            //KeyboardState keyBoard = input.KeyboardState;
            //int x = 0, y = 0;
            //if (keyBoard.IsKeyDown(Keys.A))
            //{
            //    x -= 40;
            //}
            //if (keyBoard.IsKeyDown(Keys.W))
            //{
            //    y -= 20;
            //}
            //if (keyBoard.IsKeyDown(Keys.S))
            //{
            //    y += 20;
            //}
            //if (keyBoard.IsKeyDown(Keys.D))
            //{
            //    x += 40;
            //}

            //if (x != 0 || y != 0)
            //{
            //    heroBird.Body.ApplyForce(new Vector2(x, y));
            //}

            //if (input.IsNewKeyPress(Keys.Space))
            //{
            //    heroBird.Body.ApplyLinearImpulse(new Vector2(0f, -20f));
            //}
            base.HandleInput(input, gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin(0, null, null, null, null, null, Camera.View);
            ScreenManager.SpriteBatch.Draw(ScreenManager.Assets.GetTexture("Blank"), _background, Color.White);

            foreach (Text t in _textInfo)
            {
                ScreenManager.SpriteBatch.DrawString(ScreenManager.Fonts.FrameRateCounterFont, t.Output, t.Position,
                                                     Color.Tomato);
            }
            //foreach (ParallaxBackground b in layers)
            //{
            //    b.Draw(this.ScreenManager.SpriteBatch);
            //}
            foreach (IGameObject o in _obj)
            {
                o.Draw();
            }

            ScreenManager.SpriteBatch.DrawString(ScreenManager.Fonts.MenuSpriteFont, _totalPoints.ToString(),
                                                 Camera.Position, Color.Tomato);
            ScreenManager.SpriteBatch.End();

            //ScreenManager.LineBatch.Begin(Camera.SimProjection, Camera.SimView);
            //// draw ground
            //for (int i = 0; i < _ground.FixtureList.Count; ++i)
            //{
            //    ScreenManager.LineBatch.DrawLineShape(_ground.FixtureList[i].Shape, Color.Black);
            //}
            //ScreenManager.LineBatch.End();

            base.Draw(gameTime);
        }
    }
}