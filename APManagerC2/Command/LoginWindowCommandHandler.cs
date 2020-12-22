using APManagerC2.View;
using APMControl.Interface;
using System.Windows;
using System.Windows.Input;

namespace APManagerC2.Command {
    public class LoginWindowCommandHandler : WindowCommandHandlerBase<LoginWindow> {
        private IUserData _userData {
            get {
                return _window.UserData;
            }
        }

        public LoginWindowCommandHandler(LoginWindow window) : base(window) {

        }

        /// <summary>
        /// 登录
        /// </summary>
        public void Login() {
            try {
                _userData.OpenStorageAsync(_window.InputPassword);
                _window.DialogResult = true;
            }
            catch (APMControl.APMException.IncorrectUserPasswordException) {
                Message.Show("确定是本人？", "密码错误", MessageType.Warning);
            }
            catch (APMControl.APMException.UnableToLoadStorageException e) {
                Message.Show("无法读取储存库，原因可能是用户配置文件中密码被外部修改。\n" +
                             "要修正此问题，请重新生成密码或尝试使用恢复工具进行恢复",
                             $"载入错误: {e.Message}", MessageType.Warning);
            }
        }

        public override void CloseWindow() {
            base.CloseWindow();
            Application.Current.Shutdown();
        }
        public override void KeyDown(Key key) {
            base.KeyDown(key);
            switch (key) {
                case Key.Enter:
                    Login();
                    break;
            }
        }
    }
}
