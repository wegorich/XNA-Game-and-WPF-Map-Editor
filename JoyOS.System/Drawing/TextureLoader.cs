using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JoyOS.System.Drawing
{
    public class TextureLoader
    {
        private readonly Dictionary<object, Texture2D> _textures = new Dictionary<object, Texture2D>();

        public void LoadContent(ContentManager contentManager, Type enumType, string basicFolder)
        {
            foreach (object key in Enum.GetValues(enumType))
            {
                LoadContent(contentManager, string.Format("{0}/{1}", basicFolder, key), key);
            }
        }

        public void LoadContent(ContentManager contentManager, Type basicType, Type mainType, string basicFolder)
        {
            foreach (object bkey in Enum.GetValues(basicType))
            {
                foreach (object mkey in Enum.GetValues(mainType))
                {
                    LoadContent(contentManager, string.Format("{0}/{1}/{2}", basicFolder, bkey, mkey),
                                bkey.ToString() + mkey);
                }
            }
        }

        public void LoadContent(ContentManager contentManager, string path, object key)
        {
            _textures[key] = contentManager.Load<Texture2D>(path);
        }

        public Texture2D GetTexture(object enumItem)
        {
            return _textures[enumItem];
        }
    }
}