using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace APManagerC2.ViewModel.ValueConverter {
    public class GetRectangle : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            double width = (double)values[0];
            double height = (double)values[1];
            Thickness margin = (Thickness)values[2];

            Rect rect = new Rect(margin.Left, margin.Top, width, height);

            return rect;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
