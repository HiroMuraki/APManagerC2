using System;
using System.Data.SQLite;

namespace APMCore.ViewModel {
    public abstract class APMBase<T> : ViewModelBase, IDataBaseHandler {
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
        private SQLiteConnection _dataBase;
        public virtual UpdateMethod UpdateMethod {
            get {
                return _updateMethod;
            }
            set {
                _updateMethod = value;
                OnPropertyChanged(nameof(DataBase));
            }
        }
        private UpdateMethod _updateMethod;

        public virtual UpdateInformation UpdateToSource() {
            return UpdateToSource(DataBase, UpdateMethod);
        }
        public virtual UpdateInformation UpdateToSource(UpdateMethod updateMethod) {
            return UpdateToSource(DataBase, updateMethod);
        }
        public virtual UpdateInformation UpdateToSource(SQLiteConnection conn) {
            return UpdateToSource(conn, UpdateMethod);
        }
        public virtual UpdateInformation UpdateToSource(SQLiteConnection conn, UpdateMethod updateMethod) {
            UpdateInformation result;
            switch (updateMethod) {
                case UpdateMethod.Insert:
                    result = InsertInto(conn);
                    break;
                case UpdateMethod.Update:
                    result = UpdateTo(conn);
                    break;
                case UpdateMethod.Delete:
                    result = DeleteFrom(conn);
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

        protected abstract UpdateInformation InsertInto(SQLiteConnection conn);
        protected abstract UpdateInformation UpdateTo(SQLiteConnection conn);
        protected abstract UpdateInformation DeleteFrom(SQLiteConnection conn);

        protected virtual void OnUpdated(UpdateInformation updateInformation) {
            Updated?.Invoke(this, new RecordUpdatedEventArgs(updateInformation));
        }
    }
}
