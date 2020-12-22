using System.Windows;
using System.Windows.Controls;

namespace APManagerC2.View {
    /// <summary>
    /// RGBValueSlider.xaml 的交互逻辑
    /// </summary>
    [TemplatePart(Name = "PART_RGBLabel", Type = typeof(Label))]
    [TemplatePart(Name = "PART_RGBValue", Type = typeof(TextBox))]
    public partial class RGBValueSlider : Slider {
        public static readonly DependencyProperty RGBLabelProperty =
            DependencyProperty.Register("RGBLabel", typeof(string), typeof(RGBValueSlider), new PropertyMetadata(""));
        public static readonly DependencyProperty RGBValueProperty =
            DependencyProperty.Register("RGBValue", typeof(byte), typeof(RGBValueSlider), new PropertyMetadata((byte)0));

        public byte RGBValue {
            get {
                return (byte)GetValue(RGBValueProperty);
            }
            set {
                SetValue(RGBValueProperty, value);
            }
        }
        public string RGBLabel {
            get {
                return (string)GetValue(RGBLabelProperty);
            }
            set {
                SetValue(RGBLabelProperty, value);
            }
        }
    }
}
