using APManagerC2.View;
using APMControl;
using System;
using System.IO;
using System.Windows;
using static APMControl.APM;

namespace APManagerC2 {
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application {
        private UserData _userData;
        /// <summary>
        /// 启动操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Application_Startup(object sender, StartupEventArgs e) {
            try {
                //检查是否为首次启动，创建数据
                CheckForStartUp();

                //读取数据
                _userData = await UserData.LoadFromFileAsync(UserDataFileName);
                _userData.StorageEncrypted += UserData_StorageEncrypted;

                //建立窗口
                MainWindow window = new MainWindow(_userData);
                LoginWindow loginWindow = new LoginWindow(_userData);

                //登陆验证
                if (loginWindow.ShowDialog() == true) {
                    window.Show();
                }
            }
            catch (Exception exp) {
                Message.Show(exp.Message, "载入错误", MessageType.Warning);
            }
        }
        /// <summary>
        /// 关闭操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Application_Exit(object sender, ExitEventArgs e) {
            await _userData.CloseStorageAsync();
        }
        /// <summary>
        /// 储存库加密时引发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserData_StorageEncrypted(object sender, APMControl.StorageEncryptedEventArgs e) {
            Message.Show($"已保存并对储存库进行了重新加密\n密钥为：{e.Password}", "保存成功", MessageType.Notice);
        }
        /// <summary>
        /// 启动检查
        /// </summary>
        private static void CheckForStartUp() {
            //检查头像文件夹
            if (!Directory.Exists(DataAvatarsFolderName)) {
                Directory.CreateDirectory(DataAvatarsFolderName);
            }
            //检查用户配置文件夹
            if (!Directory.Exists(UserProfileFolderName)) {
                Directory.CreateDirectory(UserProfileFolderName);
            }
            //检查用户文件是否存在
            if (!File.Exists(UserDataFileName)) {
                UserData.CreateEmptyUserData(UserDataFileName);
            }
        }
    }
}
