using APManagerC2.Command;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace APManagerC2.View {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window{
        #region 属性
        #region 后备字段
        private readonly MainWindowCommandHandler _commandHandler;
        private readonly APMControl.UserData _userData;
        #endregion
        public APMControl.UserData UserData {
            get {
                return _userData;
            }
        }
        #endregion

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MainWindow(APMControl.UserData userData) {
            _userData = userData;

            InitializeComponent();
            GridRoot.MaxWidth = SystemParameters.WorkArea.Width;
            GridRoot.MaxHeight = SystemParameters.WorkArea.Height;

            _commandHandler = new MainWindowCommandHandler(this);
        }

        #region 窗口操作
        /// <summary>
        /// 窗口的最大化，最小化，关闭和移动操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Maximum(object sender, RoutedEventArgs e) {
            _commandHandler.MaximumWindow();
        }
        private void Window_Minimum(object sender, RoutedEventArgs e) {
            _commandHandler.MinimumWindow();
        }
        private void Window_Close(object sender, RoutedEventArgs e) {
            _commandHandler.CloseWindow();
        }
        private void Window_Move(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 2) {
                _commandHandler.MaximumWindow();
            }
            _commandHandler.MoveWindow();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e) {
            _commandHandler.KeyDown(e.Key);
        }
        #endregion

        #region 面板控件交互
        //用户数据相关
        /// <summary>
        /// 点击用户头像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserLabelAvatar_Click(object sender, RoutedEventArgs e) {
            _commandHandler.DisplayUser();
        }

        //数据标签相关
        /// <summary>
        /// 点击数据标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleFilter_Click(object sender, MouseButtonEventArgs e) {
            _commandHandler.ToggleFilter(GetFilterFromControl(sender));
        }
        /// <summary>
        /// 添加数据标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFilter_Click(object sender, RoutedEventArgs e) {
            _commandHandler.AddFilter();
        }
        /// <summary>
        /// 删除数据标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveFilter_Click(object sender, RoutedEventArgs e) {
            _commandHandler.RemoveFilter(GetFilterFromControl(sender));
        }
        /// <summary>
        /// 修改数据标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModifyFilter_Click(object sender, RoutedEventArgs e) {
            _commandHandler.ModifyFilter(GetFilterFromControl(sender));
        }
        /// <summary>
        /// 打开所有过滤器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleOnAllFilters_Click(object sender, RoutedEventArgs e) {
            _commandHandler.ToggleOnAllFilters();
        }
        /// <summary>
        /// 反转所有过滤器开关状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InverseFiltersStatus_Click(object sender, RoutedEventArgs e) {
            _commandHandler.InverseFiltersStatus();
        }
        /// <summary>
        /// 当鼠标悬停于数据标签时，仅显示该标签下的数据容器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataFilter_MouseEnter(object sender, MouseEventArgs e) {
            _commandHandler.FocusFilter(GetFilterFromControl(sender));
        }
        /// <summary>
        /// 鼠标移出数据标签时，恢复标签状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataFilter_MouseLeave(object sender, MouseEventArgs e) {
            _commandHandler.FocusFilter(null);
        }
        /// <summary>
        /// 清理所有空Filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearEmptyFilters_Click(object sender, RoutedEventArgs e) {
            _commandHandler.ClearEmptyFilters();
        }

        //数据容器相关
        /// <summary>
        /// 点击数据容器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenContainer_Click(object sender, MouseButtonEventArgs e) {
            _commandHandler.OpenContainer(GetContainerFromControl(sender));
        }
        /// <summary>
        /// 添加数据容器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddContainer_Click(object sender, RoutedEventArgs e) {
            _commandHandler.AddContainer();
        }
        /// <summary>
        /// 删除数据容器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveContainer_Click(object sender, RoutedEventArgs e) {
            _commandHandler.RemoveContainer(GetContainerFromControl(sender));
        }
        /// <summary>
        /// 移除空数据容器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearEmptyContainers_Click(object sender, RoutedEventArgs e) {
            _commandHandler.ClearEmptyContainers();
        }
        /// <summary>
        /// 刷新数据面板（将标签被打开的数据排前）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reload_Click(object sender, RoutedEventArgs e) {
            _commandHandler.Reload();
        }
        /// <summary>
        /// 拖拽载入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataLabelArea_Drop(object sender, DragEventArgs e) {
            Message.Show("YZTXDY!");
        }
        #endregion

        #region 工具栏
        private void SetContainerTemplate_Click(object sender, RoutedEventArgs e) {
            _commandHandler.SetContainerTemplate();
        }
        /// <summary>
        /// 搜索栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e) {
            _commandHandler.SearchContainers((sender as TextBox).Text);
        }
        /// <summary>
        /// 保存数据包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e) {
            _commandHandler.SaveStorage();
        }
        #endregion

        #region 其他方法
        /// <summary>
        /// 展开合并菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenMenu_Click(object sender, RoutedEventArgs e) {
            Button menuButton = (sender as Button);
            menuButton.ContextMenu.IsOpen = !menuButton.ContextMenu.IsOpen;
        }
        private static APMControl.Filter GetFilterFromControl(object sender) {
            return (sender as FrameworkElement).Tag as APMControl.Filter;
        }
        private static APMControl.Container GetContainerFromControl(object sender) {
            return (sender as FrameworkElement).Tag as APMControl.Container;
        }
        #endregion
    }
}

