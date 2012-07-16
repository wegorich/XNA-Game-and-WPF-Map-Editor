using System.Windows;

namespace LevelEditor.Models
{
    public class Element : NotifyPropertyChanged
    {
        private object _addObject;
        private bool _isDragging;
        private Point _pos;
        private ViewMap _viewMap;
        private ViewSetting _viewSetting;

        public Point Pos
        {
            get { return _pos; }
            set
            {
                if (_pos.Equals(value)) return;
                _pos = value;
                RiseProperty("Pos");
            }
        }

        public ViewMap Map
        {
            get { return _viewMap; }
            set
            {
                if (value != null && value.Equals(_viewMap)) return;

                _viewMap = value;
                RiseProperty("Map");
            }
        }

        public ViewSetting ViewSetting
        {
            get { return _viewSetting; }
            set
            {
                if (_viewSetting == value) return;
                _viewSetting = value;
                /* every time inputElement resets, the draggin stops (you actually don't even need to track it, but it made things easier in the begining, I'll change it next time I get to play with it. */
                RiseProperty("ViewSetting");
            }
        }

        public object AddObject
        {
            get { return _addObject; }
            set
            {
                if (value == _addObject) return;
                _addObject = value;
                RiseProperty("AddObject");
            }
        }

        public bool IsDragging
        {
            get { return _isDragging; }
            set
            {
                if (value == _isDragging) return;
                _isDragging = value;
                RiseProperty("IsDragging");
            }
        }
    }
}