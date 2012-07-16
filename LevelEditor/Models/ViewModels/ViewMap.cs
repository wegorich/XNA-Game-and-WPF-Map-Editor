using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media.Imaging;
using JoyOS.GameModel.Setting;
using Microsoft.Xna.Framework;

namespace LevelEditor.Models
{
    public class ViewMap : NotifyPropertyChanged
    {
        public static readonly ObservableCollection<ViewMap> Maps = new ObservableCollection<ViewMap>();

        #region Defaul setting and source collections prop

        //TODO: не делал проверок по ключу а надо мол нету такого эелемента добавить автоматом
        public Dictionary<object, ViewObjectSetting> ViewObjectSettings
        {
            get { return _viewSetting; }
        }

        public Dictionary<object, ObjectSetting> ObjectSetting
        {
            get { return _baseSettings; }
            set
            {
                if (value.Equals(_baseSettings)) return;
                _baseSettings = value;
                if (_bitmapSources == null)
                    throw new AccessViolationException("ошибка загрузи текстуры в начале придурок");

                _viewSetting.Clear();
                foreach (var keyValue in _baseSettings)
                {
                    //TODO: проверка на безопасность таких действий, а лучше если нету элемента в дикшенари добавить туда дефолтный
                    if (_bitmapSources.ContainsKey(keyValue.Key))
                    {
                        _viewSetting[keyValue.Key] = new ViewObjectSetting(keyValue.Value, _bitmapSources[keyValue.Key]);
                    }
                }
                RiseProperty("ObjectSetting");
            }
        }

        public Dictionary<object, BitmapSource> BitmapSources
        {
            get { return _bitmapSources; }
            set
            {
                if (value.Equals(_bitmapSources)) return;
                _bitmapSources = value;
                RiseProperty("BitmapSources");
            }
        }

        #endregion

        #region Map settings prop

        public float Width
        {
            get { return _width; }
            set
            {
                if (Math.Abs(value - _width) > 0.1f || value <= 0) return;
                _width = value;
                RiseProperty("Width");
            }
        }

        public float Height
        {
            get { return _height; }
            set
            {
                if (Math.Abs(value - _height) > 0.1f || value <= 0) return;
                _height = value;
                RiseProperty("Height");
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value.Equals(_name)) return;
                _name = value;
                RiseProperty("Name");
            }
        }

        public string Path
        {
            get { return _path; }
            set
            {
                if (value.Equals(_path)) return;
                _path = value;
                _name = System.IO.Path.GetFileNameWithoutExtension(_path);
                RiseProperty("Path");
                RiseProperty("Name");
            }
        }

        public float GravityXPos
        {
            get { return _gravity.X; }
            set
            {
                if (Math.Abs(value - _gravity.X) > 0.1f) return;
                _gravity.X = value;
                RiseProperty("GravityXPos");
            }
        }

        public Vector2 Gravity
        {
            get { return _gravity; }
            set
            {
                if (value == _gravity) return;
                _gravity = value;

                RiseProperty("GravityXPos");
                RiseProperty("GravityYPos");
                RiseProperty("Gravity");
            }
        }

        public float GravityYPos
        {
            get { return _gravity.Y; }
            set
            {
                if (Math.Abs(value - _gravity.Y) > 0.1f) return;
                _gravity.Y = value;
                RiseProperty("GravityYPos");
            }
        }

        #endregion

        #region Map items prop

        public ObservableCollection<ViewBirdSetting> Birds
        {
            get { return _birds; }
        }

        public ObservableCollection<ViewPigSetting> Pigs
        {
            get { return _pigs; }
        }

        public ObservableCollection<ViewBlockSetting> Blocks
        {
            get { return _blocks; }
        }

        public ObservableCollection<ViewSetting> Items
        {
            get { return _items; }
        }

        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ViewSetting item in e.NewItems)
                {
                    _items.Add(item);
                }
            }
            if (e.OldItems == null) return;
            foreach (ViewSetting item in e.OldItems)
            {
                _items.Remove(item);
            }
        }

        public void Delete(ViewSetting s)
        {
            switch (s.Type)
            {
                default:
                    _birds.Remove((ViewBirdSetting) s);
                    break;
                case "PigType":
                    _pigs.Remove((ViewPigSetting) s);
                    break;
                case "BlockType":
                    _blocks.Remove((ViewBlockSetting) s);
                    break;
            }
        }

        #endregion

        private readonly ObservableCollection<ViewBirdSetting> _birds;
        private readonly ObservableCollection<ViewBlockSetting> _blocks;
        private readonly ObservableCollection<ViewSetting> _items;
        private readonly ObservableCollection<ViewPigSetting> _pigs;

        private readonly Dictionary<object, ViewObjectSetting> _viewSetting =
            new Dictionary<object, ViewObjectSetting>();

        private Dictionary<object, ObjectSetting> _baseSettings = new Dictionary<object, ObjectSetting>();

        private Dictionary<object, BitmapSource> _bitmapSources = new Dictionary<object, BitmapSource>();
        private Vector2 _gravity;
        private float _height;
        private string _name;
        private string _path;

        private float _width;

        public ViewMap() : this(800, 480, new Vector2(0, 10), "new map*")
        {
        }

        public ViewMap(int width, int height, Vector2 gravity, string name)
        {
            _width = width;
            _height = height;
            _gravity = gravity;
            _name = name;

            _birds = new ObservableCollection<ViewBirdSetting>();
            _birds.CollectionChanged += ItemsCollectionChanged;

            _blocks = new ObservableCollection<ViewBlockSetting>();
            _blocks.CollectionChanged += ItemsCollectionChanged;

            _pigs = new ObservableCollection<ViewPigSetting>();
            _pigs.CollectionChanged += ItemsCollectionChanged;

            _items = new ObservableCollection<ViewSetting>();

            Maps.Add(this);
        }
    }
}