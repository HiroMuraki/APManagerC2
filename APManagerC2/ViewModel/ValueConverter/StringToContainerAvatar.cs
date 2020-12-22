using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static APMControl.APM;

namespace APManagerC2.ViewModel.ValueConverter {
    [ValueConversion(typeof(string), typeof(Brush))]
    public class StringToContainerAvatar : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string filename = (string)value;
            string filePath;
            if (Path.IsPathRooted(filename)) {
                filePath = filename;
            } else {
                filePath = $@"{DataAvatarsFolderName}\{filename}";
            }
            BitmapImage image;

            try {
                image = new BitmapImage();
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = file;
                    image.EndInit();
                }
            }
            catch {
                image = ResDict.PreSetting["DefaultAvatar"] as BitmapImage;
            }

            return new ImageBrush(image) { Stretch = Stretch.UniformToFill };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
