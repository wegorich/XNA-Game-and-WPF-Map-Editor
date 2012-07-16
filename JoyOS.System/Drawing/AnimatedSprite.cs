using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JoyOS.System.Drawing
{
    public class AnimatedSprite : Sprite
    {
        protected int FrameHeight;
        protected int FrameWidth;

        private Rectangle _scaleRect;

        public AnimatedSprite(Texture2D texture)
            : base(texture)
        {
            TimeForFrame = 100;
            Action = SpriteAction.None;
            FrameWidth =
                FrameHeight = texture.Height/Enum.GetValues(typeof (SpriteAction)).Length;
            FrameCount = texture.Width < FrameWidth ? 1 : texture.Width/FrameWidth;
        }
        public AnimatedSprite(Texture2D texture,Rectangle scaleRect,int frameCount)
            : base(texture,new Vector2(scaleRect.Width/2,scaleRect.Height/2))
        {
            TimeForFrame = 100;
            Action = SpriteAction.None;
            FrameWidth = scaleRect.Width;
            FrameHeight = scaleRect.Height;
            _scaleRect = scaleRect;
            FrameCount = frameCount;
        }

        public int TimeElapsed { get; protected set; }
        public int TimeForFrame { get; set; }

        public int CurrentFrame { get; protected set; }
        public int FrameCount { get; protected set; }

        public SpriteAction Action { get; set; }

        public Rectangle ScaleRect
        {
            get
            {
                //_scaleRect=new Rectangle(CurrentFrame*FrameWidth,((int)Action)*FrameHeight,FrameWidth,FrameHeight);
                return _scaleRect;
            }
        }

        public virtual void Update(TimeSpan gameTime)
        {
            TimeElapsed += gameTime.Milliseconds;
            if (TimeElapsed <= TimeForFrame) return;

            CurrentFrame = (CurrentFrame + 1)%FrameCount;
            TimeElapsed = 0;
        }
    }
}