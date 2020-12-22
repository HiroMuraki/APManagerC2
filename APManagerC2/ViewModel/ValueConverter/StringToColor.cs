using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace APManagerC2.ViewModel.ValueConverter {
    /// <summary>
    /// 字符串转化为纯色画刷
    /// </summary>
    [ValueConversion(typeof(string), typeof(SolidColorBrush))]
    public class StringToColor : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string colorString = value as string;
            if (colorString is null) {
                return null;
            }
            if (!colorString.StartsWith("#")) {
                colorString = $"#{colorString}";
            }

            Color color;
            try {
                color = (Color)ColorConverter.ConvertFromString(colorString);
            }
            catch (FormatException) {
                color = Colors.White;
            }
            return new SolidColorBrush(color);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
