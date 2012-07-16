using System;
using System.Globalization;
using System.Windows.Data;

namespace LevelEditor.UILogic
{
    [ValueConversion(typeof (double), typeof (double))]
    public class NegativeDoubleConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var original = (float) value;
            return -original;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var original = (float) value;
            return -original;
        }

        #endregion
    }
}