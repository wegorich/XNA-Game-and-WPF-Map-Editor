using System;
using System.IO;
using System.Linq;
using System.Xml;
using JoyOS.GameModel.Maps;
using JoyOS.GameModel.Setting;
using LevelEditor.Helper;
using LevelEditor.Models;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace LevelEditor
{
    public partial class MainWindow
    {
        #region Add item to the map

        private void AddNewItemToMap(double x, double y, float angle)
        {
            object obj = Current.AddObject;
            if (obj == null) return;

            Type type = obj.GetType();
            if (!EditorHelper.CreateSetting.ContainsKey(type)) return;

            tabControl.SelectedIndex = 1;
            Current.ViewSetting = EditorHelper.CreateSetting[type](Current.Map, obj, x, y, angle, material.SelectedValue);

            Current.AddObject = null;
        }

        #endregion

        #region Map serialization/desirialization

        public static void Serialize(Map m, string fileName)
        {
            var settings = new XmlWriterSettings {Indent = true};

            using (XmlWriter writer = XmlWriter.Create(fileName, settings))
            {
                IntermediateSerializer.Serialize(writer, m, null);
            }
        }

        public static Map Deserialize(string fileName)
        {
            Map data;

            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    data = IntermediateSerializer.Deserialize<Map>(reader, null);
                }
            }

            return data;
        }

        #endregion

        #region Save Map

        /// <summary>
        /// Save runing map to xml file
        /// </summary>
        private void SaveMapToXML(ViewMap map)
        {
            var m = new Map
                        {
                            Gravity = map.Gravity,
                            MapHeigth = map.Height,
                            MapWidth = map.Width,
                            Birds = map.Birds.Select(x => x.Setting).ToArray(),
                            Pigs = map.Pigs.Select(x => x.Setting).ToArray(),
                            Blocks = map.Blocks.Select(x => x.Setting).ToArray(),
                            Settings = map.ObjectSetting
                        };
            Serialize(m, map.Path);
        }

        #endregion

        #region Load Map

        /// <summary>
        /// Load the map form xml file
        /// </summary>
        private ViewMap LoadMapFromXML(string path)
        {
            var map = new ViewMap();
            Map m = Deserialize(path);

            map.BitmapSources = LoadTexture.Load(_baseFolder);
            map.ObjectSetting = m.Settings;

            foreach (BirdSetting o in m.Birds)
            {
                map.Birds.Add(new ViewBirdSetting(o, map.ViewObjectSettings[o.Type]));
            }
            foreach (PigSetting o in m.Pigs)
            {
                map.Pigs.Add(new ViewPigSetting(o, map.ViewObjectSettings[o.Type]));
            }
            foreach (BlockSetting o in m.Blocks)
            {
                map.Blocks.Add(new ViewBlockSetting(o, map));
            }

            map.Width = m.MapWidth;
            map.Height = m.MapHeigth;
            map.Gravity = m.Gravity;

            map.Path = path;

            return map;
        }

        #endregion
    }
}