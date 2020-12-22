using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace APMControl {
    public class DispatchedObservableCollection<T> : ObservableCollection<T> {
        private readonly Dispatcher _dispatcher;
        public DispatchedObservableCollection() {
            _dispatcher = Application.Current.Dispatcher;
        }

        protected override void InsertItem(int index, T item) {
            if (_dispatcher.HasShutdownStarted) {
                return;
            }
            _dispatcher.Invoke(() => {
                base.InsertItem(index, item);
            });
        }
        protected override void RemoveItem(int index) {
            if (_dispatcher.HasShutdownStarted) {
                return;
            }
            _dispatcher.Invoke(() => {
                base.RemoveItem(index);
            });
        }
        protected override void ClearItems() {
            if (_dispatcher.HasShutdownStarted) {
                return;
            }
            _dispatcher.Invoke(() => {
                base.ClearItems();
            });
        }
    }
}
