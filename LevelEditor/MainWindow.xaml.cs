using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using JoyOS.GameModel;
using LevelEditor.Models;

namespace LevelEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowBase
    {
        private readonly string _baseFolder = @"..\..\..\TestGame\TestGameContent\";

        public MainWindow()
        {
            Current = new Element();
            _baseFolder = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + _baseFolder);
            InitializeComponent();

            objectShape.ItemsSource = Enum.GetValues(typeof (GameShape));
            objectShape.SelectedIndex = 0;

            material.ItemsSource = Enum.GetValues(typeof (Material));
            material.SelectedIndex = 0;

            _birdsTypes.DataContext = Enum.GetValues(typeof (BirdType));
            _pigsTypes.DataContext = Enum.GetValues(typeof (PigType));
            _blocksTypes.DataContext = Enum.GetValues(typeof (BlockType));

            //удалить потом
            Current.Map = LoadMapFromXML(_baseFolder + "map.xml");
            mapsTab.ItemsSource = ViewMap.Maps;
        }

        public Element Current { get; set; }

        private void MapPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition((FrameworkElement) sender);
            AddNewItemToMap(p.X, p.Y, 0);
        }

        private void DeleteMapClick(object sender, RoutedEventArgs e)
        {
            var control = (FrameworkElement) sender;
            ViewMap.Maps.Remove((ViewMap) control.Tag);
        }

        /// <summary>
        /// Крутит текущую картинку колесиком
        /// </summary>
        private void MapItemPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.GetKeyStates(Key.LeftCtrl) == KeyStates.Down && Current.ViewSetting != null)
                Current.ViewSetting.Rotation += e.Delta/10f;
        }

        #region Move object on canvas logic

        private void MapMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed || !Current.IsDragging || Current.ViewSetting == null) return;

            ViewSetting cur = Current.ViewSetting;
            Point pos = Mouse.GetPosition((IInputElement) sender);
            cur.CenterX += (float) (pos.X - Current.Pos.X);
            cur.CenterY += (float) (pos.Y - Current.Pos.Y);
            Current.Pos = pos;
        }

        //Висит на картинке из-за того что ListItem selected глуши события клика
        private void MapItemMouseDown(object sender, MouseButtonEventArgs e)
        {
            Current.IsDragging = ((FrameworkElement) e.OriginalSource).CaptureMouse();
        }

        //Костыль висит на ScrollView 
        private void MapItemMouseUp(object sender, MouseButtonEventArgs e)
        {
            Current.IsDragging = false;
            ((FrameworkElement) e.OriginalSource).ReleaseMouseCapture();
        }

        private void MapPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Current.Pos = Mouse.GetPosition((FrameworkElement) ((ContentControl) sender).Content);
        }

        #endregion
    }
}