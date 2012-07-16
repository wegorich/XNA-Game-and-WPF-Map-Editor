using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JoyOS.System.Controls
{
    public struct DrawContext
    {
        public GraphicsDevice Device;
        public GameTime GameTime;
        public SpriteBatch SpriteBatch;
        public Texture2D BlankTexture;
        public Vector2 DrawOffset;
    }
}
