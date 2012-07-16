using JoyOS.GameModel;
using JoyOS.GameModel.Setting;

namespace LevelEditor.Models
{
    public class ViewBlockSetting : ViewSetting
    {
        protected BlockSetting BlockSetting;
        protected ViewMap CurrentMap;

        public ViewBlockSetting(BlockSetting blockSetting, ViewMap map)
            : base(
                blockSetting, map.ViewObjectSettings[blockSetting.Material.ToString() + blockSetting.Type],
                blockSetting.Type)
        {
            BlockSetting = blockSetting;
            CurrentMap = map;
        }

        public BlockSetting Setting
        {
            get { return BlockSetting; }
        }

        public Material Material
        {
            get { return BlockSetting.Material; }
            set
            {
                if (value == BlockSetting.Material) return;
                BlockSetting.Material = value;
                ObjectSetting = CurrentMap.ViewObjectSettings[value.ToString() + BlockSetting.Type];
            }
        }
    }
}