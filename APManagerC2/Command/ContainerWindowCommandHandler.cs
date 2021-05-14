using APManagerC2.View;
using APMControl;
using APMControl.Interface;
using Encrypter;
using Microsoft.Win32;
using System;
using System.Windows.Input;

namespace APManagerC2.Command {
    public class ContainerWindowCommandHandler : WindowCommandHandlerBase<ContainerWindow> {
        private IContainer _container {
            get {
                return _window.Container;
            }
        }

        public ContainerWindowCommandHandler(ContainerWindow window) : base(window) {

        }

        /// <summary>
        /// 保存
        /// </summary>
        public async void Save() {
            await _container.UpdateToSourceAsync();
            _window.Close();
        }

        /// <summary>
        /// 添加数据条目
        /// </summary>
        public async void AddPair(string pairType) {
            IPair pair = await _container.AddPairAsync();
            switch (pairType) {
                case "empty":
                    break;
                case "time":
                    pair.Title = "时间";
                    pair.Detail = $"{DateTime.Now:G}";
                    break;
                case "random":
                    pair.Detail = await RandomString.GetStringAsync(16, StringType.UpperAlpha | StringType.LowerAlpha | StringType.Number);
                    break;
                default:
                    throw new InvalidOperationException($"无效的操作: {pairType}");
            }
        }
        /// <summary>
        /// 删除数据条目
        /// </summary>
        public async void RemovePair(Pair pair) {
            await _container.RemovePairAsync(pair);
        }
        /// <summary>
        /// 添加数据组
        /// </summary>
        public async void AddPairs(string pairListType) {
            switch (pairListType) {
                case "account":
                    IPair pair1 = await _container.AddPairAsync();
                    pair1.Title = "用户名";
                    IPair pair2 = await _container.AddPairAsync();
                    pair2.Title = "密码";
                    IPair pair3 = await _container.AddPairAsync();
                    pair3.Title = "邮箱";
                    IPair pair4 = await _container.AddPairAsync();
                    pair4.Title = "手机号";
                    break;
                default:
                    throw new InvalidOperationException($"无效的操作: {pairListType}"); ;
            }
        }

        /// <summary>
        /// 更换数据头像
        /// </summary>
        public async void SetAvatar() {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "图像文件|*.jpg;*.png;*.gif;*.bmp|JPG图像|*.jpg;*.jpeg|PNG图像|*.png|BMP图像|*.bmp|GIF图像|*.gif";
            ofd.Title = $"选择{_container}的头像";
            if (ofd.ShowDialog() == true) {
                await _container.SetAvatarAsync(ofd.FileName);
            }
        }
        /// <summary>
        /// 设置过滤器
        /// </summary>
        public void SetFilter(Filter filter) {
            _container.Filter = filter;
        }
        /// <summary>
        /// 清理空Pairs
        /// </summary>
        public async void ClearEmptyPairs() {
            int impacts = await _container.ClearEmptyPairsAsync();
        }
        /// <summary>
        /// 重新加载Pair组
        /// </summary>
        public async void Refresh() {
            await _container.OpenAsync();
        }

        public override void CloseWindow() {
            base.CloseWindow();
        }
        public override void KeyDown(Key key) {
            base.KeyDown(key);
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S)) {
                Save();
            }
            switch (key) {
                case Key.F1:
                    AddPair("empty");
                    break;
                case Key.F5:
                    Refresh();
                    break;
                case Key.Escape:
                    CloseWindow();
                    break;
            }
        }

    }
}
