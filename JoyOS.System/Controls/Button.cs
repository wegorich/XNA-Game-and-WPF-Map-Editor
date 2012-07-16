using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using JoyOS.System.Screen;

namespace JoyOS.System.Controls
{

    public class Button : Control
    {
        public delegate void ClickHandler(Button sender);
        public enum ButtonState
        {
            Normal,
            Clicked
        }
        public enum TextAlign
        {
            Left,
            Right,
            Centre
        }
        public enum FontSize
        {
            Small,
            Medium,
            Big
        }
        private SpriteFont _font;
        private bool _textVisible = true;
        private string _displaytext;
        private float _textRotation = 0f;
        private Button.FontSize _textSize = Button.FontSize.Medium;
        private Button.TextAlign _textAlignment = Button.TextAlign.Centre;
        private bool _clickAreaSpecific = false;
        private Rectangle _clickArea;
        private Texture2D _txNormalButton;
        private Texture2D _txClickedButton;
        private Button.ButtonState _state;
        private int _height = 50;
        private int _width = 0;
        private Color _clickedForground = Color.Black;
        private Color _forground = Color.Black;
        public Vector2 TapPosition = Vector2.Zero;
        private TimeSpan _lastTap = new TimeSpan(0L);
        private Vector2 LastDrawOffset = Vector2.Zero;
        private SpriteBatch _sBatch;
        private Game _Game;
        private ContentManager _content;
        private int TimeToShowClickedTexture = 200;
        public event Button.ClickHandler OnClicked;
        public SpriteFont Font
        {
            get
            {
                return this._font;
            }
            set
            {
                this._font = value;
            }
        }
        public bool TextVisible
        {
            get
            {
                return this._textVisible;
            }
            set
            {
                this._textVisible = value;
            }
        }
        public string DisplayText
        {
            get
            {
                return this._displaytext;
            }
            set
            {
                this._displaytext = value;
            }
        }
        public float TextRotation
        {
            get
            {
                return this._textRotation;
            }
            set
            {
                this._textRotation = value;
            }
        }
        public Button.FontSize TextSize
        {
            get
            {
                return this._textSize;
            }
            set
            {
                this._textSize = value;
            }
        }
        public Button.TextAlign TextAlignment
        {
            get
            {
                return this._textAlignment;
            }
            set
            {
                this._textAlignment = value;
            }
        }
        public bool ClickAreaSpecific
        {
            get
            {
                return this._clickAreaSpecific;
            }
            set
            {
                this._clickAreaSpecific = value;
            }
        }
        public Rectangle ClickArea
        {
            get
            {
                if (!this._clickAreaSpecific)
                {
                    if (this._txNormalButton != null && this._clickArea == Rectangle.Empty)
                    {
                        this._clickArea = new Rectangle((int)base.Position.X, (int)base.Position.Y, this.Width, this.Height);
                    }
                }
                return this._clickArea;
            }
            set
            {
                this._clickArea = value;
                this._clickAreaSpecific = true;
            }
        }
        public Texture2D NormalButtonTexture
        {
            get
            {
                return this._txNormalButton;
            }
            set
            {
                this._txNormalButton = value;
            }
        }
        public Texture2D ClickedButtonTexture
        {
            get
            {
                return this._txClickedButton;
            }
            set
            {
                this._txClickedButton = value;
            }
        }
        public Button.ButtonState State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }
        public int Height
        {
            get
            {
                if (this._height == 0)
                {
                    this._height = this.TextHeight + 10;
                }
                return this._height;
            }
            set
            {
                this._height = value;
            }
        }
        public int Width
        {
            get
            {
                if (this._width == 0)
                {
                    this._width = this.TextWidth + 10;
                }
                return this._width;
            }
            set
            {
                this._width = value;
            }
        }
        public Color ClickedForground
        {
            get
            {
                return this._clickedForground;
            }
            set
            {
                this._clickedForground = value;
            }
        }
        public Color Forground
        {
            get
            {
                return this._forground;
            }
            set
            {
                this._forground = value;
            }
        }
        public int TextWidth
        {
            get
            {
                int result;
                if (string.IsNullOrEmpty(this.DisplayText))
                {
                    result = 0;
                }
                else
                {
                    result = (int)this.Font.MeasureString(this.DisplayText).X;
                }
                return result;
            }
        }
        public int TextHeight
        {
            get
            {
                int result;
                if (string.IsNullOrEmpty(this.DisplayText))
                {
                    result = 0;
                }
                else
                {
                    result = (int)this.Font.MeasureString(this.DisplayText).Y;
                }
                return result;
            }
        }
        public object Tag
        {
            get;
            set;
        }
        public Button(Game Game)
        {
            this._Game = Game;
        }
        private Button(Game Game, Point Position, int Height, int Width, Texture2D Normal, Texture2D Clicked, SpriteFont Font, string DiscplayText)
        {
            this._Game = Game;
            base.Position = new Vector2((float)Position.X, (float)Position.Y);
            if (Height != 0)
            {
                this.Height = Height;
            }
            if (Width != 0)
            {
                this.Width = Width;
            }
            this.NormalButtonTexture = Normal;
            this.ClickedButtonTexture = Clicked;
            this.Font = Font;
            this.DisplayText = this._displaytext;
        }
        public void Initialize()
        {
            this._state = Button.ButtonState.Normal;
        }
        public void LoadContent()
        {
            this._content = this._Game.Content;
            if (this.NormalButtonTexture == null)
            {
                throw new Exception("No Texture provided for Button.");
            }
            if (this.ClickedButtonTexture == null)
            {
                this.ClickedButtonTexture = this.NormalButtonTexture;
            }
            if (!string.IsNullOrEmpty(this.DisplayText) && this.Font == null)
            {
                throw new Exception("No Font provided for Button.");
            }
            this._sBatch = new SpriteBatch(this._Game.GraphicsDevice);
        }
        public override void HandleInput(InputHelper input)
        {
            //this.HandleInput(input.Gestures);
            base.HandleInput(input);
        }
        public override void Update(GameTime gameTime)
        {
            if (this.TapPosition != Vector2.Zero)
            {
                this._lastTap = gameTime.TotalGameTime;
                this.TapPosition = Vector2.Zero;
            }
            if (this._state == Button.ButtonState.Clicked && gameTime.TotalGameTime.Subtract(this._lastTap).Milliseconds > this.TimeToShowClickedTexture)
            {
                this._state = Button.ButtonState.Normal;
                if (this.OnClicked != null)
                {
                    this.OnClicked(this);
                }
            }
            base.Update(gameTime);
        }
        public override void Draw(DrawContext context)
        {
            if (this.Visible)
            {
                if (context.SpriteBatch == null)
                {
                    SpriteBatch spriteBatch = this._sBatch;
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    this.Draw(context);
                    spriteBatch.End();
                }
                else
                {
                    Texture2D texture = this.StateToTexture();
                    Vector2 drawOffset = context.DrawOffset;
                    this.LastDrawOffset = context.DrawOffset;
                    SpriteBatch spriteBatch = context.SpriteBatch;
                    spriteBatch.Draw(texture, new Rectangle((int)drawOffset.X, (int)drawOffset.Y, this.Width, this.Height), Color.White);
                    this.DrawText(spriteBatch, context.DrawOffset);
                }
            }
            base.Draw(context);
        }
        private void DrawText(SpriteBatch spriteBatch, Vector2 DrawOffset)
        {
            if (this.TextVisible)
            {
                if (!string.IsNullOrEmpty(this.DisplayText))
                {
                    Vector2 position = new Vector2(this.StartPositionX(DrawOffset), DrawOffset.Y + (float)(this.Height / 2) - (float)this.TextHeight * this.ScaleText() / 2f);
                    if (this._state == Button.ButtonState.Clicked)
                    {
                        spriteBatch.DrawString(this.Font, this.DisplayText, position, this.ClickedForground, this.TextRotation, Vector2.Zero, this.ScaleText(), SpriteEffects.None, 0f);
                    }
                    else
                    {
                        spriteBatch.DrawString(this.Font, this.DisplayText, position, this.Forground, this.TextRotation, Vector2.Zero, this.ScaleText(), SpriteEffects.None, 0f);
                    }
                }
            }
        }
        public override Vector2 ComputeSize()
        {
            return new Vector2((float)this._width, (float)this._height);
        }
        private Texture2D StateToTexture()
        {
            Texture2D result = null;
            switch (this._state)
            {
                case Button.ButtonState.Normal:
                    result = this._txNormalButton;
                    break;
                case Button.ButtonState.Clicked:
                    result = this._txClickedButton;
                    break;
            }
            return result;
        }
        private float ScaleText()
        {
            float result;
            switch (this.TextSize)
            {
                case Button.FontSize.Small:
                    result = 0.75f;
                    break;
                case Button.FontSize.Medium:
                    result = 1f;
                    break;
                case Button.FontSize.Big:
                    result = 1.25f;
                    break;
                default:
                    result = 1f;
                    break;
            }
            return result;
        }
        private float StartPositionX(Vector2 DrawOffset)
        {
            float result;
            switch (this.TextAlignment)
            {
                case Button.TextAlign.Left:
                    result = DrawOffset.X + 50f;
                    break;
                case Button.TextAlign.Right:
                    result = DrawOffset.X + (float)this.Width - (float)this.TextWidth - 50f;
                    break;
                case Button.TextAlign.Centre:
                    result = DrawOffset.X + (float)(this.Width / 2) - (float)(this.TextWidth / 2);
                    break;
                default:
                    result = DrawOffset.X + (float)(this.Width / 2) - (float)(this.TextWidth / 2);
                    break;
            }
            return result;
        }
        public void HandleInput(List<GestureSample> Gestures)
        {
            if (Gestures.Count > 0)
            {
                GestureType gestureType = Gestures[0].GestureType;
                if (gestureType == GestureType.Tap)
                {
                    if (this.HandleTap(Gestures[0].Position))
                    {
                        Gestures.RemoveAt(0);
                    }
                }
            }
        }
        private bool HandleTap(Vector2 tapPosition)
        {
            this.TapPosition = tapPosition;
            Rectangle clickArea;
            if (this.ClickAreaSpecific)
            {
                clickArea = this.ClickArea;
            }
            else
            {
                clickArea = new Rectangle((int)this.LastDrawOffset.X, (int)this.LastDrawOffset.Y, this.Width, this.Height);
            }
            Point value = new Point((int)this.TapPosition.X, (int)this.TapPosition.Y);
            bool result;
            if (clickArea.Contains(value))
            {
                this._state = Button.ButtonState.Clicked;
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
