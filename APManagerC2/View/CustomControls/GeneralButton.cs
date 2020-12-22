using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace APManagerC2.View {
    [TemplatePart(Name = "PART_FocusColor", Type = typeof(Border))]
    [TemplatePart(Name = "PART_Content", Type = typeof(Label))]
    public class GeneralButton : Button {
        public CornerRadius CornerRadius {
            get {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            set {
                SetValue(CornerRadiusProperty, value);
            }
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(GeneralButton), new PropertyMetadata(new CornerRadius(0)));

        public SolidColorBrush FocusColor {
            get {
                return (SolidColorBrush)GetValue(FocusColorProperty);
            }
            set {
                SetValue(FocusColorProperty, value);
            }
        }
        public static readonly DependencyProperty FocusColorProperty =
            DependencyProperty.Register("FocusColor", typeof(SolidColorBrush), typeof(GeneralButton), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public double InitOpacity {
            get {
                return (double)GetValue(InitOpacityProperty);
            }
            set {
                SetValue(InitOpacityProperty, value);
            }
        }
        public static readonly DependencyProperty InitOpacityProperty =
            DependencyProperty.Register("InitOpacity", typeof(double), typeof(GeneralButton), new PropertyMetadata((double)1));

        public double FocusOpacity {
            get {
                return (double)GetValue(FocusOpacityProperty);
            }
            set {
                SetValue(FocusOpacityProperty, value);
            }
        }
        public static readonly DependencyProperty FocusOpacityProperty =
            DependencyProperty.Register("FocusOpacity", typeof(double), typeof(GeneralButton), new PropertyMetadata((double)1));

        static GeneralButton() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeneralButton), new FrameworkPropertyMetadata(typeof(GeneralButton)));
        }
    }
}
