using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JoyOS.System.Controls
{

    public class TextControl : Control
    {
        private SpriteFont font;
        private string text;
        public Color Color;
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    base.InvalidateAutoSize();
                }
            }
        }
        public SpriteFont Font
        {
            get
            {
                return this.Font;
            }
            set
            {
                if (this.font != value)
                {
                    this.font = value;
                    base.InvalidateAutoSize();
                }
            }
        }
        public TextControl()
            : this(string.Empty, null, Color.White, Vector2.Zero)
        {
        }
        public TextControl(string text, SpriteFont font)
            : this(text, font, Color.White, Vector2.Zero)
        {
        }
        public TextControl(string text, SpriteFont font, Color color)
            : this(text, font, color, Vector2.Zero)
        {
        }
        public TextControl(string text, SpriteFont font, Color color, Vector2 position)
        {
            this.text = text;
            this.font = font;
            base.Position = position;
            this.Color = color;
        }
        public override void Draw(DrawContext context)
        {
            base.Draw(context);
            context.SpriteBatch.DrawString(this.font, this.Text, context.DrawOffset, this.Color);
        }
        public override Vector2 ComputeSize()
        {
            return this.font.MeasureString(this.Text);
        }
    }
}
