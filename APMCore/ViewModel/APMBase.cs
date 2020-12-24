using System;
using System.Data.SQLite;

namespace APMCore.ViewModel {
    public abstract class APMBase<T> : ViewModelBase {
        public event EventHandler<RecordUpdatedEventArgs> Updated;
        protected readonly T _dataSource;

        public virtual SQLiteConnection DataBase {
            get {
                return _dataBase;
            }
            set {
                _dataBase = value;
                OnPropertyChanged(nameof(DataBase));
            }
        }
        public virtual UpdateMethod UpdateMethod {
            get {
                return _updateMethod;
            }
            set {
                _updateMethod = value;
                OnPropertyChanged(nameof(DataBase));
            }
        }

        private SQLiteConnection _dataBase;
        private UpdateMethod _updateMethod;

        public virtual UpdateInformation UpdateToSource() {
            return UpdateToSource(UpdateMethod);
        }
        public virtual UpdateInformation UpdateToSource(UpdateMethod updateMethod) {
            UpdateInformation result;
            switch (updateMethod) {
                case UpdateMethod.Insert:
                    result = InsertInto();
                    break;
                case UpdateMethod.Update:
                    result = UpdateTo();
                    break;
                case UpdateMethod.Delete:
                    result = DeleteFrom();
                    break;
                default:
                    throw new InvalidOperationException($"无效的操作: {updateMethod}");
            }
            if (result.Impacts > 0) {
                if (UpdateMethod == UpdateMethod.Insert) {
                    UpdateMethod = UpdateMethod.Update;
                }
                OnUpdated(result);
            }
            return result;
        }
        protected abstract UpdateInformation InsertInto();
        protected abstract UpdateInformation UpdateTo();
        protected abstract UpdateInformation DeleteFrom();

        protected virtual void OnUpdated(UpdateInformation updateInformation) {
            Updated?.Invoke(this, new RecordUpdatedEventArgs(updateInformation));
        }
    }
}
