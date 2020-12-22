using System;
using System.Windows;

namespace APManagerC2 {
    public static class ResDict {
        public readonly static ResourceDictionary PreSetting = new ResourceDictionary() {
            Source = new Uri("/APManagerC2;component/Resources/Styles/PreSetting.xaml", UriKind.Relative)
        };
    }
}
