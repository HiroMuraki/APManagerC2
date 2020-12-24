using APManagerC2.Command;
using System.Windows;
using System.Windows.Input;

namespace APManagerC2.View {
    /// <summary>
    /// InputWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FilterWindow : Window {
        #region 属性
        #region 公共属性
        public APMControl.Filter Filter {
            get {
                return _filter;
            }
        }
        public APMControl.Filter Backup {
            get {
                return _backup;
            }
        }
        #endregion

        #region 后备字段
        private readonly APMControl.Filter _backup;
        private readonly APMControl.Filter _filter;
        private readonly FilterWindowCommandHandler _commandHandler;
        #endregion
        #endregion

        #region 构造函数
        public FilterWindow(APMControl.Filter filter) {
            _filter = filter;
            _backup = new APMControl.Filter(new APMCore.Model.Filter(-1));
            _commandHandler = new FilterWindowCommandHandler(this);

            InitializeComponent();
        }
        #endregion

        #region 控件交互
        /// <summary>
        /// 提交修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e) {
            _commandHandler.Save();
            e.Handled = true;
        }
        #endregion

        #region 窗口操作
        private async void WindowSelf_Loaded(object sender, RoutedEventArgs e) {
            await _backup.CopyPropertiesAsync(_filter);
        }
        private void Window_Close(object sender, RoutedEventArgs e) {
            _commandHandler.CloseWindow();
            e.Handled = true;
        }
        private void Window_Move(object sender, MouseButtonEventArgs e) {
            _commandHandler.MoveWindow();
            e.Handled = true;
        }
        private void Window_KeyDown(object sender, KeyEventArgs e) {
            _commandHandler.KeyDown(e.Key);
        }
        #endregion

        #region 包装方法
        /// <summary>
        /// 私有辅助方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Identifier_Click(object sender, RoutedEventArgs e) {
            ColorPickerPopup.IsOpen = true;
            e.Handled = true;
        }
        #endregion 
    }
}
