using JoyOS.GameModel.Setting;

namespace LevelEditor.Models
{
    public class ViewBirdSetting : ViewSetting
    {
        protected BirdSetting BirdSetting;

        public ViewBirdSetting(BirdSetting birdSetting, ViewObjectSetting setting)
            : base(birdSetting, setting, birdSetting.Type)
        {
            BirdSetting = birdSetting;
        }

        public BirdSetting Setting
        {
            get { return BirdSetting; }
        }

        public int ZIndex
        {
            get { return BirdSetting.ZIndex; }
            set
            {
                if (value == BirdSetting.ZIndex) return;
                BirdSetting.ZIndex = value;
                RiseProperty("ZIndex");
            }
        }
    }
}