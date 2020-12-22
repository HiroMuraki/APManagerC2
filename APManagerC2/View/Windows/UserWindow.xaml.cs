using APManagerC2.Command;
using System.Windows;
using System.Windows.Input;

namespace APManagerC2.View {
    /// <summary>
    /// StatisticsMessage.xaml 的交互逻辑
    /// </summary>
    public partial class UserWindow : Window {
        #region 属性
        #region 公共属性
        public APMControl.UserData UserData {
            get {
                return _userData;
            }
        }
        public APMControl.UserData Backup {
            get {
                return _backup;
            }
        }
        #endregion

        #region 后备字段
        private readonly UserWindowCommandHandler _commandHandler;
        private readonly APMControl.UserData _userData;
        private readonly APMControl.UserData _backup;
        #endregion
        #endregion

        #region 构造函数
        public UserWindow(APMControl.UserData userData) {
            _backup = new APMControl.UserData(new APMCore.Model.UserData());
            _backup.CopyProperties(userData);
            _userData = userData;
            _commandHandler = new UserWindowCommandHandler(this);
            InitializeComponent();
        }
        #endregion

        #region 修改信息
        /// <summary>
        /// 点击头像操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeAvatar_Click(object sender, RoutedEventArgs e) {
            _commandHandler.ChangeAvatar();
        }
        /// <summary>
        /// 保存修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e) {
            _commandHandler.Save();
        }
        #endregion

        #region 窗口操作
        private void Window_Move(object sender, MouseButtonEventArgs e) {
            _commandHandler.MoveWindow();
        }
        private void Window_Minimum(object sender, RoutedEventArgs e) {
            _commandHandler.MinimumWindow();
        }
        private void Window_Close(object sender, RoutedEventArgs e) {
            _commandHandler.CloseWindow();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e) {
            _commandHandler.KeyDown(e.Key);
        }
        #endregion
    }
}
