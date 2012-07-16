using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using JoyOS.GameModel;
using JoyOS.GameModel.Setting;
using LevelEditor.Models;
using Microsoft.Xna.Framework;

namespace LevelEditor.Helper
{
    public static class EditorHelper
    {
        public static BitmapSource Crop(this BitmapSource bit, Rectangle r)
        {
            int l = r.Left, t = r.Top, w = r.Width, h = r.Height;

            w = w > bit.PixelWidth ? bit.PixelWidth : w;
            h = h > bit.PixelHeight ? bit.PixelHeight : h;
            l = l + w > bit.PixelWidth ? bit.PixelWidth - w : l;
            t = h + t > bit.PixelHeight ? bit.PixelHeight - h : t;

            var rect = new Int32Rect(l, t, w, h);
            return new CroppedBitmap(bit, rect);
        }

        #region CreateSetting

        public static readonly Dictionary<Type, Func<ViewMap, object, double, double, float, object, ViewSetting>>
            CreateSetting =
                new Dictionary<Type, Func<ViewMap, object, double, double, float, object, ViewSetting>>
                    {
                        {typeof (BirdType), CreateBirdSetting},
                        {typeof (PigType), CreatePigSetting},
                        {typeof (BlockType), CreateBlockSetting}
                    };

        public static ViewSetting CreateBirdSetting(ViewMap map, object type, double x, double y, float angle,
                                                    object source)
        {
            var o = new ViewBirdSetting(new BirdSetting
                                            {
                                                Type = (BirdType) type,
                                                Position = new Vector2((float) x, (float) y),
                                                Rotation = angle
                                            }, map.ViewObjectSettings[type]);
            map.Birds.Add(o);
            return o;
        }

        public static ViewSetting CreatePigSetting(ViewMap map, object type, double x, double y, float angle,
                                                   object source)
        {
            var o = new ViewPigSetting(new PigSetting
                                           {
                                               Type = (PigType) type,
                                               Position = new Vector2((float) x, (float) y),
                                               Rotation = angle
                                           }, map.ViewObjectSettings[type]);
            map.Pigs.Add(o);
            return o;
        }

        public static ViewSetting CreateBlockSetting(ViewMap map, object type, double x, double y, float angle,
                                                     object material)
        {
            var o = new ViewBlockSetting(new BlockSetting
                                             {
                                                 Type = (BlockType) type,
                                                 Position = new Vector2((float) x, (float) y),
                                                 Rotation = angle,
                                                 Material = (Material) material
                                             }, map);
            map.Blocks.Add(o);
            return o;
        }

        #endregion
    }
}