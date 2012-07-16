using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JoyOS.System.Controls
{
    public class ImageControl : Control
    {
        private Texture2D texture;
        public Vector2 origin;
        public Vector2? SourceSize;
        public Color Color;
        public Texture2D Texture
        {
            get
            {
                return this.texture;
            }
            set
            {
                if (this.texture != value)
                {
                    this.texture = value;
                    base.InvalidateAutoSize();
                }
            }
        }
        public ImageControl()
            : this(null, Vector2.Zero)
        {
        }
        public ImageControl(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            base.Position = position;
            this.Color = Color.White;
        }
        public override void Draw(DrawContext context)
        {
            base.Draw(context);
            Texture2D texture2D = this.texture ?? context.BlankTexture;
            Vector2? sourceSize = this.SourceSize;
            Vector2 vector = sourceSize.HasValue ? sourceSize.GetValueOrDefault() : base.Size;
            Rectangle rectangle = new Rectangle
            {
                X = (int)this.origin.X,
                Y = (int)this.origin.Y,
                Width = (int)vector.X,
                Height = (int)vector.Y
            };
            Rectangle destinationRectangle = new Rectangle
            {
                X = (int)context.DrawOffset.X,
                Y = (int)context.DrawOffset.Y,
                Width = (int)base.Size.X,
                Height = (int)base.Size.Y
            };
            context.SpriteBatch.Draw(texture2D, destinationRectangle, new Rectangle?(rectangle), this.Color);
        }
        public override Vector2 ComputeSize()
        {
            Vector2 result;
            if (this.texture != null)
            {
                result = new Vector2((float)this.texture.Width, (float)this.texture.Height);
            }
            else
            {
                result = Vector2.Zero;
            }
            return result;
        }
    }
}
