using APManagerC2.View;
using APMControl.APMException;
using APMControl.Interface;
using Microsoft.Win32;
using System;
using System.Windows.Input;
using static APMControl.APM;

namespace APManagerC2.Command {
    public class UserWindowCommandHandler : WindowCommandHandlerBase<UserWindow> {
        private IUserData _userData {
            get {
                return _window.UserData;
            }
        }

        public UserWindowCommandHandler(UserWindow window) : base(window) {

        }

        /// <summary>
        /// 点击头像操作
        /// </summary>
        public async void ChangeAvatar() {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "图像文件|*.jpg;*.png;*.gif;*.bmp|JPG图像|*.jpg;*.jpeg|PNG图像|*.png|BMP图像|*.bmp|GIF图像|*.gif";
            ofd.Title = "设置头像";
            if (ofd.ShowDialog() == true) {
                try {
                    await _userData.SetAvatarAsync(ofd.FileName);
                }
                catch (Exception e) {
                    Message.Show($"设置头像出现错误, {e.Message}", "设置错误", MessageType.Warning);
                }
            }
        }
        /// <summary>
        /// 保存修改
        /// </summary>
        public async void Save() {
            try {
                await _userData.SaveUserDataAsync(UserDataFileName);
                _window.Close();
            }
            catch (StorageFileIOException) {
                Message.Show("当前无法保存文件，请稍后再试", "保存错误", MessageType.Warning);
            }
            catch (InvalidUserPasswordException) {
                Message.Show("密码长度应为4-16位，且只含除空格外的ASCII可打印字符", "保存错误", MessageType.Warning);
            }
            catch (Exception e) {
                Message.Show(e.Message, $"保存错误", MessageType.Warning);
            }
        }

        public override void CloseWindow() {
            _userData.CopyProperties(_window.Backup);
            base.CloseWindow();
        }
        public override void KeyDown(Key key) {
            base.KeyDown(key);
        }
    }
}
