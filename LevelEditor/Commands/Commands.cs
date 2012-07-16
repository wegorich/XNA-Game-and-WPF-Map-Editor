using System.Windows.Input;
using LevelEditor.Models;
using Microsoft.Win32;

namespace LevelEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Delete object command

        private void DeleteCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Current.ViewSetting != null && Current.Map != null;
        }

        private void DeleteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Current.Map.Delete(Current.ViewSetting);
            Current.ViewSetting = null;
        }

        #endregion

        #region Paste object

        private void PasteCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Current.ViewSetting != null && Current.Map != null;
        }

        private void PasteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewSetting s = Current.ViewSetting;
            Current.AddObject = s.Name;
            AddNewItemToMap(s.CenterX + s.OffsetX/2, s.CenterY + s.OffsetY/2, s.Rotation);
        }

        #endregion

        #region Map Save

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveMapToXML(Current.Map);
        }

        private void SaveCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Current.Map != null && Current.Map.Path != null;
        }

        #endregion

        #region Map SaveAs

        private void SaveAsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Current.Map != null;
        }

        private void SaveAsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
                          {
                              InitialDirectory = _baseFolder,
                              DefaultExt = ".xml",
                              Filter = "Map documents (.xml)|*.xml"
                          };
            if (dlg.ShowDialog() == true)
            {
                Current.Map.Path = dlg.FileName;
                SaveExecuted(sender, e);
            }
        }

        #endregion

        #region Map Open

        private void OpenExecute(object sender, ExecutedRoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
                          {
                              InitialDirectory = _baseFolder,
                              DefaultExt = ".xml",
                              Filter = "Map documents (.xml)|*.xml"
                          };
            if (dlg.ShowDialog() == true)
            {
                Current.Map = LoadMapFromXML(dlg.FileName);
            }
        }

        #endregion
    }
}