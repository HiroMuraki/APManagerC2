using APManagerC2.View;
using APMControl.Interface;
using System.Windows.Input;

namespace APManagerC2.Command {
    public class FilterWindowCommandHandler : WindowCommandHandlerBase<FilterWindow> {
        private IFilter _filter {
            get {
                return _window.Filter;
            }
        }

        public FilterWindowCommandHandler(FilterWindow window) : base(window) {

        }

        /// <summary>
        /// 提交修改
        /// </summary>
        public async void Save() {
            await _filter.UpdateToSourceAsync();
            _window.Close();
        }

        public override async void CloseWindow() {
            await _filter.CopyPropertiesAsync(_window.Backup);
            base.CloseWindow();
        }
        public override void KeyDown(Key key) {
            base.KeyDown(key);
            if (Keyboard.IsKeyDown(Key.LeftCtrl)) {
                switch (key) {
                    case Key.S:
                        Save();
                        break;
                }
            }
        }
    }
}
