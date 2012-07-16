using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JoyOS.System.Components
{
    public class ParallaxBackground
    {
        private readonly ParallaxDirection _direction;
        private Vector2 _currentPosition;
        private Vector2 _defaultMovementSpeed;
        private float _speedX = 1f;
        private float _speedY = 1f;

        public ParallaxBackground(List<Texture2D> scrollingTextures, ParallaxDirection direction)
        {
            _direction = direction;
            ScrollingTextures = scrollingTextures;
            Initialize();
        }

        public float SpeedX
        {
            get { return _speedX; }
            set { _speedX = value; }
        }

        public float SpeedY
        {
            get { return _speedY; }
            set { _speedY = value; }
        }

        public int Height { get; set; }
        public int Width { get; set; }
        public List<Texture2D> ScrollingTextures { get; set; }
        private List<TextureDetails> Textures { get; set; }

        public Vector2 Position
        {
            get { return _currentPosition; }
            set { _currentPosition = value; }
        }

        private void Initialize()
        {
            switch (_direction)
            {
                case ParallaxDirection.Horizontal:
                    SpeedX = 1f;
                    SpeedY = 0f;
                    break;
                case ParallaxDirection.Vertical:
                    SpeedX = 0f;
                    SpeedY = 1f;
                    break;
            }
            _currentPosition = Vector2.Zero;
            _defaultMovementSpeed = new Vector2(SpeedX, SpeedY);
            int num = 0;
            int num2 = 0;
            Textures = new List<TextureDetails>();
            int num3 = 1;
            using (List<Texture2D>.Enumerator enumerator = ScrollingTextures.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Texture2D current = enumerator.Current;
                    Textures.Add(new TextureDetails(current, current.Width, current.Height, num, num2, num3.ToString()));
                    num += current.Width;
                    num2 += current.Height;
                    num3++;
                }
            }
        }

        public void Move(Vector2 distance)
        {
            _currentPosition.X = _currentPosition.X + SpeedX*distance.X;
            _currentPosition.Y = _currentPosition.Y + SpeedY*distance.Y;
            if (_currentPosition.X >=
                (Textures[Textures.Count - 1].BeltPositionX + Textures[Textures.Count - 1].Width))
            {
                _currentPosition.X = 0f;
            }
            if (_currentPosition.Y >=
                (Textures[Textures.Count - 1].BeltPositionY + Textures[Textures.Count - 1].Height))
            {
                _currentPosition.Y = 0f;
            }
        }

        public void Move()
        {
            Move(_defaultMovementSpeed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float num = _currentPosition.X;
            float num2 = _currentPosition.Y;
            foreach (TextureDetails t in Textures)
            {
                if (_direction == ParallaxDirection.Horizontal && num >= t.BeltPositionX &&
                    num <= (t.BeltPositionX + t.Width))
                {
                    num += Math.Min(t.Width, Width);
                    spriteBatch.Draw(t.Texture,
                                     new Vector2(t.BeltPositionX - _currentPosition.X, 0f), Color.White);
                }
                if (_direction == ParallaxDirection.Vertical && num2 >= t.BeltPositionY &&
                    num2 <= (t.BeltPositionY + t.Height))
                {
                    num2 += Math.Min(t.Height, Height);
                    spriteBatch.Draw(t.Texture,
                                     new Vector2(0f, t.BeltPositionY - _currentPosition.Y), Color.White);
                }
            }
            if (_direction == ParallaxDirection.Horizontal &&
                _currentPosition.X >=
                (Textures[Textures.Count - 1].BeltPositionX + Textures[Textures.Count - 1].Width - Width))
            {
                spriteBatch.Draw(Textures[0].Texture,
                                 new Vector2(
                                     (Textures[Textures.Count - 1].BeltPositionX +
                                      Textures[Textures.Count - 1].Width) - _currentPosition.X, 0f), Color.White);
            }
            if (_direction == ParallaxDirection.Vertical &&
                _currentPosition.Y >=
                (Textures[Textures.Count - 1].BeltPositionY + Textures[Textures.Count - 1].Height - Height))
            {
                spriteBatch.Draw(Textures[0].Texture,
                                 new Vector2(0f,
                                             (Textures[Textures.Count - 1].BeltPositionY +
                                              Textures[Textures.Count - 1].Height) - _currentPosition.Y), Color.White);
            }
        }

        #region Nested type: TextureDetails

        private class TextureDetails
        {
            public readonly int BeltPositionX;
            public readonly int BeltPositionY;
            public readonly int Height;
            public readonly Texture2D Texture;
            public readonly int Width;
            private string _name;

            public TextureDetails(Texture2D texture, int width, int height, int posX, int posY, string name)
            {
                Texture = texture;
                Width = width;
                Height = height;
                BeltPositionX = posX;
                BeltPositionY = posY;
                _name = name;
            }
        }

        #endregion
    }
}