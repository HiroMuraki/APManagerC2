using System.Windows;
using System.Windows.Controls;

namespace APManagerC2.View {
    /// <summary>
    /// UserInfromationLabel.xaml 的交互逻辑
    /// </summary>
    public partial class UserInfromationLabel : UserControl {
        public static readonly RoutedEvent ClickAvatarEvent = EventManager.RegisterRoutedEvent(
            "ClickAvatar", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UserInfromationLabel));

        public static readonly DependencyProperty LinkedDataProperty =
            DependencyProperty.Register("LinkedData", typeof(APMControl.UserData), typeof(UserInfromationLabel), new PropertyMetadata(null));

        /// <summary>
        /// 数据关联
        /// </summary>
        public APMControl.UserData LinkedData {
            get {
                return (APMControl.UserData)GetValue(LinkedDataProperty);
            }
            set {
                SetValue(LinkedDataProperty, value);
            }
        }
        /// <summary>
        /// 点击头像事件
        /// </summary>
        public event RoutedEventHandler ClickAvatar {
            add {
                AddHandler(ClickAvatarEvent, value);
            }
            remove {
                RemoveHandler(ClickAvatarEvent, value);
            }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public UserInfromationLabel() {
            InitializeComponent();
        }

        private void AvatarButton_Click(object sender, RoutedEventArgs e) {
            RoutedEventArgs args = new RoutedEventArgs(ClickAvatarEvent, this);
            RaiseEvent(args);
        }
    }
}
