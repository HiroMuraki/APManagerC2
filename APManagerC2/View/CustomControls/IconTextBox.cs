using System.Windows;
using System.Windows.Controls;

namespace APManagerC2.View {
    /// <summary>
    /// IconTextBox.xaml 的交互逻辑
    /// </summary>
    [TemplatePart(Name = "PART_Icon", Type = typeof(Label))]
    public partial class IconTextBox : TextBox {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), typeof(IconTextBox), new PropertyMetadata(null));
        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register("IconSource", typeof(string), typeof(IconTextBox), new PropertyMetadata(""));

        /// <summary>
        /// 图标图片
        /// </summary>
        public object Icon {
            get {
                return GetValue(IconProperty);
            }
            set {
                SetValue(IconProperty, value);
            }
        }
        /// <summary>
        /// 图标文件路径
        /// </summary>
        public string IconSource {
            get {
                return (string)GetValue(IconSourceProperty);
            }
            set {
                SetValue(IconSourceProperty, value);
            }
        }
    }
}
