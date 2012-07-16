using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JoyOS.System.Drawing
{
    public class Sprite
    {
        public Vector2 Origin { get; set; }
        public Texture2D Texture { get; set; }

        public Sprite(Texture2D texture, Vector2 origin)
        {
            Texture = texture;
            Origin = origin;
        }

        public Sprite(Texture2D sprite)
        {
            Texture = sprite;
            Origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
        }
    }
}