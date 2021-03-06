﻿using APManagerC2.Command;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace APManagerC2.View {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region 属性
        #region 公共属性
        public APMControl.UserData UserData {
            get {
                return _userData;
            }
        }
        public string InputPassword {
            get {
                return InputBox.Password;
            }
            set {
                InputBox.Password = value;
            }
        }
        #endregion

        #region 私有字段
        private LoginWindowCommandHandler _commandHandler;
        private readonly APMControl.UserData _userData;
        #endregion
        #endregion

        #region 构造函数
        public LoginWindow(APMControl.UserData userData) {
            _userData = userData;
            _commandHandler = new LoginWindowCommandHandler(this);
            InitializeComponent();
            InputPassword = "";
        }
        #endregion

        #region 控件操作
        private void Login_Click(object sender, RoutedEventArgs e) {
            _commandHandler.Login();
            e.Handled = true;
        }
        private void PackData_Click(object sender, RoutedEventArgs e) {
            _commandHandler.PackData();
            e.Handled = true;
        }
        private void UnpackData_Click(object sender, RoutedEventArgs e) {
            _commandHandler.UnpackData();
            e.Handled = true;
        }
        private void OpenMenu_Click(object sender, RoutedEventArgs e) {
            FrameworkElement element = sender as FrameworkElement;
            element.ContextMenu.IsOpen = !element.ContextMenu.IsOpen;
            e.Handled = true;
        }
        #endregion

        #region 窗口操作
        private void Window_Move(object sender, MouseButtonEventArgs e) {
            _commandHandler.MoveWindow();
            e.Handled = true;
        }
        private void Window_Minimum(object sender, RoutedEventArgs e) {
            _commandHandler.MinimumWindow();
            e.Handled = true;
        }
        private void Window_Close(object sender, RoutedEventArgs e) {
            _commandHandler.CloseWindow();
            e.Handled = true;
        }
        private void Window_KeyDown(object sender, KeyEventArgs e) {
            _commandHandler.KeyDown(e.Key);
        }
        #endregion

    }
}
