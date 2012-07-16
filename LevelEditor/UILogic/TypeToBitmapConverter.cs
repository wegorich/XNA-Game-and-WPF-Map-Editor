using System;
using System.Globalization;
using System.Windows.Data;
using LevelEditor.Models;

namespace LevelEditor.UILogic
{
    public class TypeToBitmapMultiValueConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var data = values[0] as ViewMap;
            if (data != null)
            {
                object key = values[1];
                object subkey = values[2];

                key = subkey == null ? key : subkey.ToString() + key;

                return data.ViewObjectSettings[key].Preview;
            }
            return null;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}