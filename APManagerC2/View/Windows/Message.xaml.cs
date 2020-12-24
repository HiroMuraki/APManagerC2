using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace APManagerC2.View {
    /// <summary>
    /// MessageDisplay.xaml 的交互逻辑
    /// </summary>
    [Flags]
    public enum MessageType {
        Warning = 0b_0000_0000,
        Notice = 0b_0000_0010,
        Select = 0b_0000_0100
    }

    [ValueConversion(typeof(MessageType), typeof(BitmapImage))]
    public class MessageTypeToImage : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            MessageType messageType = (MessageType)value;
            if ((messageType & MessageType.Notice) == MessageType.Notice) {
                return ResDict.PreSetting["Notice"] as BitmapImage;
            }
            if ((messageType & MessageType.Warning) == MessageType.Warning) {
                return ResDict.PreSetting["Warning"] as BitmapImage;

            }
            return ResDict.PreSetting["Notice"] as BitmapImage;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(MessageType), typeof(Visibility))]
    public class MessageTypeToButtons : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            MessageType messageType = (MessageType)value;
            if ((messageType & MessageType.Select) == MessageType.Select) {
                return Visibility.Visible;
            } else {
                return Visibility.Hidden;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    public partial class Message : Window {
        #region 属性
        #region 后备字段
        private readonly string _messageTitle;
        private readonly string _messageDetail;
        private readonly MessageType _messageType;
        #endregion
        public string MessageTitle {
            get {
                return _messageTitle;
            }
        }
        public string MessageDetail {
            get {
                return _messageDetail;
            }

        }
        public MessageType MessageType {
            get {
                return _messageType;
            }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messageTitle">消息标题</param>
        /// <param name="messageDetail">消息内容</param>
        /// <param name="messageType">消息类型</param>
        public Message(string messageDetail, string messageTitle, MessageType messageType) {
            _messageTitle = messageTitle;
            _messageDetail = messageDetail;
            _messageType = messageType;
            InitializeComponent();
        }

        /// <summary>
        /// 用于静态调用显示提示窗口，该窗口不会设置DialogResult，
        /// </summary>
        /// <param name="title"></param>
        /// <param name="detail"></param>
        /// <param name="type"></param>
        public static void Show(string detail, string title, MessageType type) {
            new Message(detail, title, type).ShowDialog();
        }
        public static void Show(string detail) {
            new Message(detail, "", MessageType.Notice).ShowDialog();
        }

        /// <summary>
        /// 关闭窗口，并设置DialogResult
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Close(object sender, RoutedEventArgs e) {
            switch ((sender as Button).Name) {
                case "btnA":
                    DialogResult = true;
                    break;
                case "btnB":
                    DialogResult = false;
                    break;
                default:
                    DialogResult = null;
                    break;
            }
            Close();
            e.Handled = true;
        }
        private void Window_Move(object sender, MouseButtonEventArgs e) {
            DragMove();
            e.Handled = true;
        }
    }
}
