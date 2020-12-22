using System;
using System.Windows;
using System.Windows.Controls;

namespace APManagerC2.View {
    public class ColorPicker : Control {
        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register("R", typeof(byte), typeof(ColorPicker), new PropertyMetadata((byte)0, OnRGBValueChanged));
        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register("G", typeof(byte), typeof(ColorPicker), new PropertyMetadata((byte)0, OnRGBValueChanged));
        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register("B", typeof(byte), typeof(ColorPicker), new PropertyMetadata((byte)0, OnRGBValueChanged));
        public static readonly DependencyProperty HexValueProperty =
            DependencyProperty.Register("HexValue", typeof(string), typeof(ColorPicker), new PropertyMetadata("", OnHexValueChanged));

        public byte R {
            get { return (byte)GetValue(RProperty); }
            set { SetValue(RProperty, value); }
        }
        public byte G {
            get { return (byte)GetValue(GProperty); }
            set { SetValue(GProperty, value); }
        }
        public byte B {
            get { return (byte)GetValue(BProperty); }
            set { SetValue(BProperty, value); }
        }
        public string HexValue {
            get { return (string)GetValue(HexValueProperty); }
            set { SetValue(HexValueProperty, value); }
        }

        private static void OnHexValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ColorPicker cp = d as ColorPicker;
            string hexValue = cp.HexValue;
            if (hexValue.StartsWith("#")) {
                hexValue = hexValue.Substring(1);
            }
            try {
                if (hexValue.Length == 6) {
                    cp.R = Convert.ToByte(hexValue.Substring(0, 2), 16);
                    cp.G = Convert.ToByte(hexValue.Substring(2, 2), 16);
                    cp.B = Convert.ToByte(hexValue.Substring(4, 2), 16);
                } 
            }
            catch {
                cp.R = 255;
                cp.G = 255;
                cp.B = 255;
            }
        }
        private static void OnRGBValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ColorPicker cp = d as ColorPicker;
            cp.HexValue = $"#{cp.R:X2}{cp.G:X2}{cp.B:X2}";
        }

        static ColorPicker() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }
    }
}
