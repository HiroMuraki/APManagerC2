﻿using APManagerC2.View;
using APMControl;
using APMControl.Interface;
using Microsoft.Win32;
using System;
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
        public async void Login() {
            try {
                await _userData.OpenStorageAsync(_window.InputPassword);
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
        /// <summary>
        /// 打包用户文件
        /// </summary>
        public async void PackData() {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "ZIP压缩文件|*.zip";
            sfd.FileName = _userData.UserName;
            sfd.Title = "导出数据";
            if (sfd.ShowDialog() == true) {
                try {
                    await APMPackager.PackAsync(sfd.FileName);
                    Message.Show($"文件已保存至{sfd.FileName}", "保存成功", MessageType.Notice);
                }
                catch (Exception e) {
                    Message.Show(e.Message, "保存错误", MessageType.Warning);
                }
            }
        }
        /// <summary>
        /// 导入用户文件
        /// </summary>
        public async void UnpackData() {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "ZIP压缩文件|*.zip";
            ofd.FileName = "";
            ofd.Title = "导入数据";
            if (ofd.ShowDialog() == true) {
                Message message = new Message("导入用户文件将会移除当前的用户数据，是否继续?", "警告", MessageType.Warning | MessageType.Select);
                if (message.ShowDialog() == false) {
                    return;
                }
                try {
                    await APMPackager.UnpackAsync(ofd.FileName);
                    Message.Show("导入数据成功，重启以生效", "导入完成", MessageType.Notice);
                    Application.Current.Shutdown();
                }
                catch (Exception e) {
                    Message.Show(e.Message, "导入错误", MessageType.Warning);
                }
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
