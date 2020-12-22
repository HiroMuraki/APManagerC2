using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace APManagerC2.ViewModel.ValueConverter {
    /// <summary>
    /// 布尔值转化为Visiblity枚举
    /// </summary>
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class BooleanToVisibility : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                bool status = (bool)value;
                switch (status) {
                    case true:
                        return Visibility.Visible;
                    case false:
                        return Visibility.Hidden;
                }
            }
            catch {
                return Visibility.Visible;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
