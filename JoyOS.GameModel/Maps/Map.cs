using System.Collections.Generic;
using JoyOS.GameModel.Setting;
using Microsoft.Xna.Framework;

namespace JoyOS.GameModel.Maps
{
    public class Map
    {
        public float MapWidth { get; set; }
        public float MapHeigth { get; set; }
        public Vector2 Gravity { get; set; }
        public BirdSetting[] Birds { get; set; }
        public PigSetting[] Pigs { get; set; }
        public BlockSetting[] Blocks { get; set; }
        public Dictionary<object, ObjectSetting> Settings { get; set; }
    }
}