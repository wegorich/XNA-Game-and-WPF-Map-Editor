using System;
using JoyOS.GameModel.Setting;
using Microsoft.Xna.Framework;

namespace LevelEditor.Models
{
    public class ViewSetting : NotifyPropertyChanged
    {
        private const float Radian = (float) Math.PI/180;
        private readonly Setting _setting;

        private readonly object _value;
        private ViewObjectSetting _objectSetting;

        public ViewSetting(Setting setting, ViewObjectSetting objectSetting, object value)
        {
            _setting = setting;
            _objectSetting = objectSetting;
            _value = value;
        }

        public Setting SomeSetting
        {
            get { return _setting; }
        }

        public ViewObjectSetting ObjectSetting
        {
            get { return _objectSetting; }
            set
            {
                if (_objectSetting.Equals(value)) return;
                _objectSetting = value;
                RiseProperty("ObjectSetting");
            }
        }

        public object Name
        {
            get { return _value; }
        }

        public string Type
        {
            get { return _value.GetType().Name; }
        }

        public float CenterX
        {
            get { return _setting.Position.X; }
            set
            {
                if (Math.Abs(value - _setting.Position.X) < 0.1f) return;
                _setting.Position = new Vector2(value, _setting.Position.Y);
                RiseProperty("CenterX");
            }
        }

        public float CenterY
        {
            get { return _setting.Position.Y; }
            set
            {
                if (Math.Abs(value - _setting.Position.Y) < 0.1f) return;
                _setting.Position = new Vector2(_setting.Position.X, value);
                RiseProperty("CenterY");
            }
        }

        public float OffsetY
        {
            get { return _objectSetting.Height/2f; }
        }

        public float OffsetX
        {
            get { return _objectSetting.Width/2f; }
        }

        public float Rotation
        {
            get { return _setting.Rotation/Radian; }
            set
            {
                if (Math.Abs(value - _setting.Rotation) < 0.1f) return;

                value = value > 360 ? value%360 : value;
                _setting.Rotation = value*Radian;
                RiseProperty("Rotation");
            }
        }
    }
}