using System.Windows;
using System.Windows.Input;

namespace APManagerC2.Command {
    public abstract class WindowCommandHandlerBase<T> where T : Window {
        protected readonly T _window;

        public WindowCommandHandlerBase(T window) {
            _window = window;
        }

        /// <summary>
        /// 最小化窗口
        /// </summary>
        public virtual void MinimumWindow() {
            _window.WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// 最大化窗口
        /// </summary>
        public virtual void MaximumWindow() {
            if (_window.WindowState == WindowState.Normal) {
                _window.WindowState = WindowState.Maximized;
            } else {
                _window.WindowState = WindowState.Normal;
            }
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        public virtual void CloseWindow() {
            _window.Close();
        }
        /// <summary>
        /// 移动窗口
        /// </summary>
        public virtual void MoveWindow() {
            _window.DragMove();
        }
        /// <summary>
        /// 按键相应
        /// </summary>
        /// <param name="key"></param>
        public virtual void KeyDown(Key key) {

        }
    }
}
