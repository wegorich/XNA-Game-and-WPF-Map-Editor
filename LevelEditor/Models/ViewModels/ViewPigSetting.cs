using JoyOS.GameModel.Setting;

namespace LevelEditor.Models
{
    public class ViewPigSetting : ViewSetting
    {
        protected PigSetting PigSetting;

        public ViewPigSetting(PigSetting pigSetting, ViewObjectSetting setting)
            : base(pigSetting, setting, pigSetting.Type)
        {
            PigSetting = pigSetting;
        }

        public PigSetting Setting
        {
            get { return PigSetting; }
        }
    }
}