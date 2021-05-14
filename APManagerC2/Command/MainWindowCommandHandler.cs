using APManagerC2.View;
using APMControl;
using APMControl.APMException;
using APMControl.Interface;
using System;
using System.Windows;
using System.Windows.Input;

namespace APManagerC2.Command {
    public class MainWindowCommandHandler : WindowCommandHandlerBase<MainWindow> {
        private IUserData _userData {
            get {
                return _window.UserData;
            }
        }

        public MainWindowCommandHandler(MainWindow window) : base(window) {

        }

        #region 面板控件交互
        //用户数据相关
        /// <summary>
        /// 点击用户头像
        /// </summary>
        public void DisplayUser() {
            UserWindow window = new UserWindow(_userData as UserData) { Owner = _window };//TODO
            FocusedAction(() => {
                window.ShowDialog();
            });
        }

        //数据标签相关
        /// <summary>
        /// 点击数据标签
        /// </summary>
        public async void ToggleFilter(Filter filter) {
            await filter.ToggleAsync();
        }
        /// <summary>
        /// 添加数据标签
        /// </summary>
        public async void AddFilter() {
            IFilter filter = await _userData.Storage.AddFilterAsync();
        }
        /// <summary>
        /// 删除数据标签
        /// </summary>
        public async void RemoveFilter(Filter filter) {
            if (!_userData.IsEditMode) {
                Message message = new Message($"确认删除过滤器：{filter}？\n该过滤器下所有的容器都将被移除。", "警告", MessageType.Warning | MessageType.Select);
                if (message.ShowDialog() == true) {
                    await _userData.Storage.RemoveFilterAsync(filter);
                }
            }
            else {
                await _userData.Storage.RemoveFilterAsync(filter);
            }
        }
        /// <summary>
        /// 修改数据标签
        /// </summary>
        public void ModifyFilter(Filter filter) {
            FilterWindow inputWindow = new FilterWindow(filter) { Owner = _window };
            FocusedAction(() => {
                inputWindow.ShowDialog();
            });
        }
        /// <summary>
        /// 打开所有过滤器
        /// </summary>
        public async void ToggleOnAllFilters() {
            await _userData.Storage.ToggleOnAllFiltersAsync();
        }
        /// <summary>
        /// 反转所有过滤器开关状态
        /// </summary>
        public async void InverseFiltersStatus() {
            await _userData.Storage.InverseFiltersAsync();
        }
        /// <summary>
        /// 当鼠标悬停于数据标签时，仅显示该标签下的数据容器
        /// </summary>
        public async void FocusFilter(Filter filter) {
            await _userData.Storage.FocusContainersAsync(filter);
        }
        /// <summary>
        /// 清理所有空Filter
        /// </summary>
        public async void ClearEmptyFilters() {
            int impacts = await _userData.Storage.ClearEmptyFiltersAsync();
            Message.Show($"清理了{impacts}个空过滤器", "清理完成", MessageType.Notice);
        }

        //数据容器相关
        /// <summary>
        /// 点击数据容器
        /// </summary>
        public void OpenContainer(Container container) {
            ContainerWindow window = new ContainerWindow(container, _userData.Storage.Filters) {
                Owner = _window
            };

            FocusedAction(() => {
                window.ShowDialog();
            });
        }
        /// <summary>
        /// 添加数据容器
        /// </summary>
        public async void AddContainer() {
            IContainer container = await _userData.Storage.AddContainerAsync();
        }
        /// <summary>
        /// 删除数据容器
        /// </summary>
        public async void RemoveContainer(Container container) {
            if (!_userData.IsEditMode) {
                Message message = new Message($"确认删除容器：{container}", "警告", MessageType.Warning | MessageType.Select);
                if (message.ShowDialog() == true) {
                    await _userData.Storage.RemoveContainerAsync(container);
                }
            }
            else {
                await _userData.Storage.RemoveContainerAsync(container);
            }

        }
        /// <summary>
        /// 移除空数据容器
        /// </summary>
        public async void ClearEmptyContainers() {
            int impacts = await _userData.Storage.ClearEmptyContainersAsync();
            Message.Show($"清理了{impacts}个空容器", "清理完成", MessageType.Notice);
        }
        /// <summary>
        /// 刷新数据面板（将标签被打开的数据排前）
        /// </summary>
        public async void Reload() {
            await _userData.Storage.ReloadAsync();
        }
        /// <summary>
        /// 拖拽载入数据
        /// </summary>
        public void DataLabelArea_Drop() {

        }
        /// <summary>
        /// 将指定Container内容复制为模板内容
        /// </summary>
        /// <param name="container">要复制的Contaienr</param>
        public async void SetContainerToTemplate(Container container) {
            await _userData.Storage.SetContainerToTemplateAsync(container);
        }
        #endregion

        #region 工具栏
        /// <summary>
        /// 设置新数据容器模板
        /// </summary>
        public void SetContainerTemplate() {
            ContainerWindow window = new ContainerWindow(_userData.Storage.ContainerTemplate, _userData.Storage.Filters) {
                Owner = _window
            };
            FocusedAction(() => {
                window.ShowDialog();
            });
        }
        /// <summary>
        /// 搜索栏
        /// </summary>
        public async void SearchContainers(string key) {
            await _userData.Storage.ReloadContainersByKeywordAsync(key);
        }
        /// <summary>
        /// 保存数据包
        /// </summary>
        public async void SaveStorage() {
            try {
                await _userData.SaveUserDataAsync(APM.UserDataFileName);
                await _userData.SaveStorageAsync();
            }
            catch (StorageFileIOException) {
                Message.Show("当前无法保存文件，请稍后再试", $"保存错误", MessageType.Warning);
            }
            catch (Exception e) {
                Message.Show(e.Message, $"保存错误", MessageType.Warning);
            }
        }
        #endregion

        public override void CloseWindow() {
            base.CloseWindow();
            Application.Current.Shutdown();
        }
        public override void KeyDown(Key key) {
            base.KeyDown(key);
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S)) {
                SaveStorage();
            }
            switch (key) {
                case Key.F1:
                    AddFilter();
                    break;
                case Key.F2:
                    AddContainer();
                    break;
                case Key.F5:
                    Reload();
                    break;
            }
        }
        private void FocusedAction(Action action) {
            _window.FocusCover.Visibility = Visibility.Visible;
            action?.Invoke();
            _window.FocusCover.Visibility = Visibility.Collapsed;
        }
    }
}
