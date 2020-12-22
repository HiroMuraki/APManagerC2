using APManagerC2.Command;
using APMControl.Interface;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace APManagerC2.View {
    /// <summary>
    /// AccountDetails.xaml 的交互逻辑
    /// </summary>
    public partial class ContainerWindow : Window {
        #region 属性
        #region 公共属性
        /// <summary>
        /// 允许的过滤器选择列表
        /// </summary>
        public IEnumerable<APMControl.Filter> AllowedFilterList {
            get {
                return _allowedFilterList;
            }
        }
        /// <summary>
        /// 关联的数据
        /// </summary>
        public IContainer Container {
            get {
                return _container;
            }
        }
        public IContainer Backup {
            get {
                return _backup;
            }
        }
        #endregion

        #region 后备字段
        private IEnumerable<APMControl.Filter> _allowedFilterList;
        private readonly IContainer _backup;
        private readonly IContainer _container;
        #endregion

        #region 私有字段
        private readonly ContainerWindowCommandHandler _commandHandler;
        #endregion
        #endregion

        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ContainerWindow(IContainer container, IEnumerable<APMControl.Filter> allowedFilterList) {
            _container = container;
            _backup = new APMControl.Container(new APMCore.Model.Container(-1));
            _allowedFilterList = allowedFilterList;
            _commandHandler = new ContainerWindowCommandHandler(this);

            InitializeComponent();
            GridRoot.MaxWidth = SystemParameters.WorkArea.Width;
            GridRoot.MaxHeight = SystemParameters.WorkArea.Height;
        }
        #endregion

        #region 控件交互
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveData_Click(object sender, RoutedEventArgs e) {
            _commandHandler.Save();
        }

        /// <summary>
        /// 添加数据条目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPair_Click(object sender, RoutedEventArgs e) {
            _commandHandler.AddPair((sender as FrameworkElement).Tag as string);
        }
        /// <summary>
        /// 删除数据条目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveDataPair_Click(object sender, RoutedEventArgs e) {
            _commandHandler.RemovePair(GetPairFromControl(sender));
        }

        /// <summary>
        /// 更换数据头像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModifiyAvatar_Click(object sender, RoutedEventArgs e) {
            _commandHandler.ModifyAvatar();
        }
        /// <summary>
        /// 设置过滤器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetDataFilter_Click(object sender, RoutedEventArgs e) {
            _commandHandler.SetFilter(GetFilterFromControl(sender));
        }
        /// <summary>
        /// 清理空Pairs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearEmptyPairs_Click(object sender, RoutedEventArgs e) {
            _commandHandler.ClearEmptyPairs();
        }
        /// <summary>
        /// 重新加载Pair组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh_Click(object sender, RoutedEventArgs e) {
            _commandHandler.Refresh();
        }
        #endregion

        #region 窗口操作
        private void Window_Minimum(object sender, RoutedEventArgs e) {
            _commandHandler.MinimumWindow();
        }
        private void Window_Maximum(object sender, RoutedEventArgs e) {
            _commandHandler.MaximumWindow();
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
        private async void WindowSelf_Loaded(object sender, RoutedEventArgs e) {
            await _container.OpenAsync();
            await _backup.CopyPropertiesAsync(_container);
        }
        private async void WindowSelf_Closed(object sender, EventArgs e) {
            await Container.CloseAsync();
        }
        #endregion

        private static APMControl.Pair GetPairFromControl(object sender) {
            return (sender as FrameworkElement).Tag as APMControl.Pair;
        }
        private static APMControl.Filter GetFilterFromControl(object sender) {
            return (sender as FrameworkElement).Tag as APMControl.Filter;
        }

        
    }
}
