using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using JoyOS.GameModel;

namespace LevelEditor.Helper
{
    public static class LoadTexture
    {
        private static string _baseFolder;
        private static Dictionary<object, BitmapSource> _bitmapSources;

        public static Dictionary<object, BitmapSource> Load(string baseFolder)
        {
            if (!Directory.Exists(baseFolder)) return null;

            _baseFolder = baseFolder;
            _bitmapSources = new Dictionary<object, BitmapSource>();
            LoadTextures("Birds", typeof (BirdType));
            LoadTextures("Pigs", typeof (PigType));
            LoadTextures("Blocks", typeof (Material), typeof (BlockType));
            return _bitmapSources;
        }

        /// <summary>
        /// Load image data to dictionary
        /// </summary>
        private static void LoadImage(object value, string path)
        {
            var uri = new Uri(path);
            _bitmapSources.Add(value, new BitmapImage(uri));
        }

        /// <summary>
        /// Load left menu btn and set up setting dictionary
        /// </summary>
        private static void LoadTextures(string folder, Type baseType, Type mainType)
        {
            foreach (object baseName in Enum.GetValues(baseType))
            {
                foreach (object mainName in Enum.GetValues(mainType))
                {
                    string str = baseName.ToString() + mainName;
                    LoadImage(str, Path.Combine(_baseFolder, folder, baseName.ToString(), mainName + ".png"));
                }
            }
        }

        private static void LoadTextures(string folder, Type mainType)
        {
            foreach (object value in Enum.GetValues(mainType))
            {
                LoadImage(value, Path.Combine(_baseFolder, folder, value + ".png"));
            }
        }
    }
}