using System;
using FarseerPhysics.Dynamics;
using JoyOS.GameModel;
using JoyOS.System.Drawing;
using JoyOS.System.Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JoyOS.Game.Model
{
    public class Bird : IBird
    {
        private readonly AnimatedSprite _birdSprite;
        private readonly PhysicsGameScreen _screen;
        private Category _collidesWith;
        private Category _collisionCategories;
        private float _health;
        private Rectangle _rect;

        public Bird(Body body, float health, Rectangle rect, Rectangle scaleRect, int frameCount, Texture2D texture,
                    PhysicsGameScreen screen)
        {
            _collidesWith = Category.All;
            _collisionCategories = Category.All;

            Body = body;
            _rect = rect;
            _screen = screen;
            _health = health;

            _birdSprite = new AnimatedSprite(texture, scaleRect, frameCount);
        }

        #region IBird Members

        public Category CollisionCategories
        {
            get { return _collisionCategories; }
            set
            {
                _collisionCategories = value;
                Body.CollisionCategories = value;
            }
        }

        public Category CollidesWith
        {
            get { return _collidesWith; }
            set
            {
                _collidesWith = value;
                Body.CollidesWith = value;
            }
        }

        public Body Body { get; private set; }

        public float Health
        {
            get { return _health; }
            set
            {
                if (Math.Abs(_health - value) < 0.1f) return;
                _health = value;
            }
        }

        public void Draw()
        {
            SpriteBatch batch = _screen.ScreenManager.SpriteBatch;
            //cross
            Vector2 v = ConvertUnits.ToDisplayUnits(Body.Position);
            _rect.Location = new Point((int) (v.X), (int) (v.Y));
            batch.Draw(_birdSprite.Texture, _rect, _birdSprite.ScaleRect,
                       Color.White, Body.Rotation, _birdSprite.Origin, SpriteEffects.None, 0f);
        }

        public void Update()
        {
            _birdSprite.Update(_screen.ScreenManager.Game.TargetElapsedTime);
        }

        public void Input(InputHelper input)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}