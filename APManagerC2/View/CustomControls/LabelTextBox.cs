using System.Windows;
using System.Windows.Controls;

namespace APManagerC2.View {
    /// <summary>
    /// LabelTextBox.xaml 的交互逻辑
    /// </summary>
    [TemplatePart(Name = "PART_Label", Type = typeof(Label))]
    public partial class LabelTextBox : TextBox {
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(LabelTextBox), new PropertyMetadata(""));

        public string Label {
            get {
                return (string)GetValue(LabelProperty);
            }
            set {
                SetValue(LabelProperty, value);
            }
        }
    }
}
