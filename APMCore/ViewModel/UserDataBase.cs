using System.Data.SQLite;
using System.IO;
using System.Runtime.Serialization.Json;

namespace APMCore.ViewModel {
    /// <summary>
    /// 用于使用用户配置信息
    /// </summary>
    public class UserDataBase : ViewModelBase {
        #region 属性
        #region 公共属性
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName {
            get {
                return _dataSource.UserName;
            }
            set {
                _dataSource.UserName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string UserPassword {
            get {
                return _dataSource.UserPassword;
            }
            set {
                _dataSource.UserPassword = value;
                OnPropertyChanged(nameof(UserPassword));
            }
        }
        /// <summary>
        /// 附加描述
        /// </summary>
        public string Description {
            get {
                return _dataSource.Description;
            }
            set {
                _dataSource.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        /// <summary>
        /// 头像路径
        /// </summary>
        public string Avatar {
            get {
                return _dataSource.Avatar;
            }
            set {
                _dataSource.Avatar = value;
                OnPropertyChanged(nameof(Avatar));
            }
        }
        /// <summary>
        /// 是否为编辑模式
        /// </summary>
        public bool IsEditMode {
            get {
                return _dataSource.IsEditMode;
            }
            set {
                _dataSource.IsEditMode = value;
                OnPropertyChanged(nameof(IsEditMode));
            }
        }
        /// <summary>
        /// 指示容器列数
        /// </summary>
        public int ColumnSize {
            get {
                return _dataSource.ColumnSize;
            }
            set {
                _dataSource.ColumnSize = value;
                OnPropertyChanged(nameof(ColumnSize));
            }
        }
        /// <summary>
        /// Storage文件
        /// </summary>
        public string StorageFile {
            get {
                return _dataSource.Storage;
            }
            private set {
                _dataSource.Storage = value;
            }
        }
        #endregion

        #region 私有字段
        /// <summary>
        /// 数据源
        /// </summary>
        private readonly Model.UserData _dataSource;
        #endregion

        #region 私有静态字段
        protected readonly static DataContractJsonSerializer SourceSerializer = new DataContractJsonSerializer(typeof(Model.UserData));
        #endregion
        #endregion

        #region 构造函数
        /// <summary>
        /// 表示一个用户配置信息
        /// </summary>
        /// <param name="source">数据源</param>
        protected UserDataBase(Model.UserData source) {
            _dataSource = source;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public void CopyProperties(UserDataBase source) {
            UserName = source.UserName;
            UserPassword = source.UserPassword;
            Avatar = source.Avatar;
            Description = source.Description;
            ColumnSize = source.ColumnSize;
            IsEditMode = source.IsEditMode;
        }
        /// <summary>
        /// 从数据库中获取Storage
        /// </summary>
        /// <returns></returns>
        protected static SQLiteConnection GetStorageConnection(string filePath) {
            SQLiteConnection conn = new SQLiteConnection("data source = " + filePath);
            return conn;
        }
        /// <summary>
        /// 保存至文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        protected virtual void SaveToFile(string filePath) {
            using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write)) {
                SourceSerializer.WriteObject(file, _dataSource);
            }
        }
        #endregion
    }
}
