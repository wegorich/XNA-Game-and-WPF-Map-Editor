using System;
using System.Windows.Media.Imaging;
using JoyOS.GameModel;
using JoyOS.GameModel.Setting;
using LevelEditor.Helper;
using Microsoft.Xna.Framework;

namespace LevelEditor.Models
{
    public class ViewObjectSetting : NotifyPropertyChanged
    {
        private readonly ObjectSetting _objectSetting;
        private readonly BitmapSource _sourcePreview;
        private BitmapSource _croppedPreview;

        public ViewObjectSetting(ObjectSetting objectSetting, BitmapSource sourcePreview)
        {
            _objectSetting = objectSetting;
            _sourcePreview = sourcePreview;
            _croppedPreview = _sourcePreview.Crop(_objectSetting.ScaleRect);
        }

        public ViewObjectSetting(ObjectSetting objectSetting)
        {
            _objectSetting = objectSetting;
        }

        #region Texture prop

        public BitmapSource Preview
        {
            get { return _croppedPreview; }
        }

        public int ScaleLeft
        {
            get { return _objectSetting.ScaleRect.Left; }
            set
            {
                if (value == _objectSetting.ScaleRect.Left) return;

                Rectangle rect = _objectSetting.ScaleRect;
                value = value + rect.Width > _sourcePreview.PixelWidth ? _sourcePreview.PixelWidth - rect.Width : value;
                _objectSetting.ScaleRect = new Rectangle(value, rect.Top, rect.Width, rect.Height);
                _croppedPreview = _sourcePreview.Crop(_objectSetting.ScaleRect);

                RiseProperty("ScaleLeft");
                RiseProperty("Preview");
            }
        }

        public int ScaleTop
        {
            get { return _objectSetting.ScaleRect.Top; }
            set
            {
                if (value == _objectSetting.ScaleRect.Top) return;

                Rectangle rect = _objectSetting.ScaleRect;
                value = value + rect.Height > _sourcePreview.PixelHeight
                            ? _sourcePreview.PixelHeight - rect.Height
                            : value;
                _objectSetting.ScaleRect = new Rectangle(rect.Left, value, rect.Width, rect.Height);
                _croppedPreview = _sourcePreview.Crop(_objectSetting.ScaleRect);

                RiseProperty("ScaleTop");
                RiseProperty("Preview");
            }
        }

        public int ScaleWidth
        {
            get { return _objectSetting.ScaleRect.Width; }
            set
            {
                if (value == _objectSetting.ScaleRect.Width) return;

                Rectangle rect = _objectSetting.ScaleRect;
                value = value > _sourcePreview.PixelWidth ? _sourcePreview.PixelWidth : value;
                _objectSetting.ScaleRect = new Rectangle(rect.Left, rect.Top, value, rect.Height);
                _croppedPreview = _sourcePreview.Crop(_objectSetting.ScaleRect);

                RiseProperty("ScaleWidth");
                RiseProperty("Preview");
            }
        }

        public int ScaleHeight
        {
            get { return _objectSetting.ScaleRect.Height; }
            set
            {
                if (value == _objectSetting.ScaleRect.Height) return;

                Rectangle rect = _objectSetting.ScaleRect;
                value = value > _sourcePreview.PixelHeight ? _sourcePreview.PixelHeight : value;
                _objectSetting.ScaleRect = new Rectangle(rect.Left, rect.Top, rect.Width, value);
                _croppedPreview = _sourcePreview.Crop(_objectSetting.ScaleRect);

                RiseProperty("ScaleHeight");
                RiseProperty("Preview");
            }
        }

        public int FrameCount
        {
            get { return _objectSetting.FrameCount; }
            set
            {
                if (value == _objectSetting.FrameCount) return;

                _objectSetting.FrameCount = value < 1 ? 1 : value;
                RiseProperty("FrameCount");
            }
        }

        #endregion

        #region Common prop

        public int Width
        {
            get { return _objectSetting.Width; }
            set
            {
                if (value == _objectSetting.Width) return;

                _objectSetting.Width = value;
                RiseProperty("Width");
            }
        }

        public int Height
        {
            get { return _objectSetting.Height; }
            set
            {
                if (value == _objectSetting.Height) return;

                _objectSetting.Height = value;
                RiseProperty("Height");
            }
        }

        public float Mass
        {
            get { return _objectSetting.Mass; }
            set
            {
                if (Math.Abs(value- _objectSetting.Mass)>0.1f) return;
                _objectSetting.Mass = value;
                RiseProperty("Mass");
            }
        }

        public float Friction
        {
            get { return _objectSetting.Friction; }
            set
            {
                if (Math.Abs(value - _objectSetting.Friction)>0.1f) return;
                _objectSetting.Friction = value;
                RiseProperty("Friction");
            }
        }

        public float Restitution
        {
            get { return _objectSetting.Restitution; }
            set
            {
                if (Math.Abs(value - _objectSetting.Restitution) > 0.1f) return;
                _objectSetting.Restitution = value;
                RiseProperty("Restitution");
            }
        }

        public float Health
        {
            get { return _objectSetting.Health; }
            set
            {
                if (Math.Abs(value - _objectSetting.Health) > 0.1f) return;
                _objectSetting.Health = value;
                RiseProperty("Health");
            }
        }

        public GameShape Shape
        {
            get { return _objectSetting.Shape; }
            set
            {
                if (value == _objectSetting.Shape) return;
                _objectSetting.Shape = value;
                RiseProperty("Shape");
            }
        }

        #endregion
    }
}