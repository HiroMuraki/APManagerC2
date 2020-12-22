using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static APMControl.APM;

namespace APManager2.Test {
    /// <summary>
    /// 字符串转化为ImageBrush
    /// </summary>
    [ValueConversion(typeof(string), typeof(SolidColorBrush))]
    public class StringToImageBrush : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string filePath = (string)value;

            //如果文件不是头像文件，则将文件路径转化为绝对路径
            if (filePath != "Avatar" && !Path.IsPathRooted(filePath)) {
                filePath = $@"{Directory.GetCurrentDirectory()}\{DataAvatarsFolderName}\{filePath}";
            }
            BitmapImage image = new BitmapImage();

            //如果文件不存在，使用默认头像
            try {
                //读取图片到内存，并设置ImageBrush
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.BeginInit();
                image.StreamSource = new MemoryStream(File.ReadAllBytes(filePath));
                image.EndInit();
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
