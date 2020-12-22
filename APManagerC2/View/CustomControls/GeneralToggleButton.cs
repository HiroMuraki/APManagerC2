using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace APManagerC2.View {
    [TemplatePart(Name = "PART_FocusColor", Type = typeof(Border))]
    [TemplatePart(Name = "PART_Content", Type = typeof(Label))]
    public class GeneralToggleButton : ToggleButton {
        public SolidColorBrush FocusColor {
            get {
                return (SolidColorBrush)GetValue(FocusColorProperty);
            }
            set {
                SetValue(FocusColorProperty, value);
            }
        }
        public static readonly DependencyProperty FocusColorProperty =
            DependencyProperty.Register("FocusColor", typeof(SolidColorBrush), typeof(GeneralToggleButton), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public double InitOpacity {
            get {
                return (double)GetValue(InitOpacityProperty);
            }
            set {
                SetValue(InitOpacityProperty, value);
            }
        }
        public static readonly DependencyProperty InitOpacityProperty =
            DependencyProperty.Register("InitOpacity", typeof(double), typeof(GeneralToggleButton), new PropertyMetadata((double)1));

        public double FocusOpacity {
            get {
                return (double)GetValue(FocusOpacityProperty);
            }
            set {
                SetValue(FocusOpacityProperty, value);
            }
        }
        public static readonly DependencyProperty FocusOpacityProperty =
            DependencyProperty.Register("FocusOpacity", typeof(double), typeof(GeneralToggleButton), new PropertyMetadata((double)1));

        public CornerRadius CornerRadius {
            get {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            set {
                SetValue(CornerRadiusProperty, value);
            }
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(GeneralToggleButton), new PropertyMetadata(new CornerRadius(0)));

        public object ToggleOnContent {
            get {
                return GetValue(ToggleOnContentProperty);
            }
            set {
                SetValue(ToggleOnContentProperty, value);
            }
        }
        public static readonly DependencyProperty ToggleOnContentProperty =
            DependencyProperty.Register("ToggleOnContent", typeof(object), typeof(GeneralToggleButton), new PropertyMetadata(null));

        static GeneralToggleButton() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GeneralToggleButton), new FrameworkPropertyMetadata(typeof(GeneralToggleButton)));
        }
    }
}
