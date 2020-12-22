using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace APManagerC2.ViewModel.ValueConverter {
    [ValueConversion(typeof(string), typeof(SolidColorBrush))]
    public class FilterIsOnToLabelColor : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                bool isOn = (bool)value;
                switch (isOn) {
                    case true:
                        return new SolidColorBrush(Color.FromRgb(0x36, 0x36, 0x36));
                    case false:
                        return new SolidColorBrush(Color.FromRgb(0x50, 0x50, 0x50));
                }
            }
            catch (Exception) {
                return new SolidColorBrush(Color.FromRgb(0x50, 0x50, 0x50));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
