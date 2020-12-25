using APMControl.APMException;
using APMControl.Interface;
using Encrypter;
using System;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static APMControl.APM;


namespace APMControl {
    public class StorageEncryptedEventArgs : EventArgs {
        public string Password { get; }
        public StorageEncryptedEventArgs(string password) {
            Password = password;
        }
    }

    public class UserData : APMCore.ViewModel.UserDataBase, IUserData {
        public event EventHandler<StorageEncryptedEventArgs> StorageEncrypted;

        #region 属性
        #region 公共属性
        /// <summary>
        /// 储存库实例
        /// </summary>
        public IStorage Storage {
            get {
                return _storage;
            }
            private set {
                _storage = value;
                OnPropertyChanged(nameof(Storage));
            }
        }
        #endregion

        #region 后备字段
        private IStorage _storage;
        #endregion

        #region 私有字段
        private SQLiteConnection _dataBase;
        private readonly object _storageFileLocker = new object();
        private readonly object _userdataFileLocker = new object();
        #endregion
        #endregion

        #region 构造函数
        /// <summary>
        /// 新建一个UserData，传入数据源
        /// </summary>
        /// <param name="source"></param>
        public UserData(APMCore.Model.UserData source) : base(source) {

        }
        #endregion

        #region 方法
        #region 公共方法
        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="source"></param>
        public void CopyProperties(IUserData source) {
            base.CopyProperties(source as APMCore.ViewModel.UserDataBase);
        }
        /// <summary>
        /// 创建默认数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static void CreateEmptyUserData(string filePath) {
            APMCore.Model.UserData source = new APMCore.Model.UserData() {
                Avatar = UserAvatarFileName,
                UserName = "YZTXDY",
                UserPassword = HashString.SHA("YZTXDY"),
                Description = "The first motto",
                ColumnSize = 3,
                IsEditMode = false,
                Storage = UserStorageFileName
            };

            UserData userData = new UserData(source);
            userData.SaveToFile(filePath);

            APMControl.Storage.CreateEmptyStorage(UserStorageFileName);
            File.Copy(UserStorageFileName, RuntimeStorageFileName);
            FileEncrypter encrypter = new FileEncrypter(new XOREncrypter("YZTXDY"));
            encrypter.Encrypt(RuntimeStorageFileName);
            File.Copy(RuntimeStorageFileName, UserStorageFileName, true);
        }
        /// <summary>
        /// 从文件中读取数据并创建UserData实例
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>根据文件创建的UserData实例</returns>
        public async static Task<UserData> LoadFromFileAsync(string filePath) {
            APMCore.Model.UserData source = await Task.Run(() => {
                return LoadFromFile(filePath);
            });

            if (source == null) {
                throw new NullReferenceException();
            }

            return new UserData(source);
        }

        /// <summary>
        /// 打开储存库
        /// </summary>
        /// <param name="password">密码</param>
        public async Task OpenStorageAsync(string password) {
            if (HashString.SHA(password) != UserPassword) {
                throw new IncorrectUserPasswordException();
            }

            await DecryptDataBaseAsync(password);
            _dataBase = GetStorageConnection(RuntimeStorageFileName);
            lock (_storageFileLocker) {
                _dataBase.Open();
            }
            Storage = new Storage(_dataBase);

            try {
                await Storage.ReloadAsync();
            }
            catch {
                throw new UnableToLoadStorageException();
            }

            UserPassword = password;
        }
        /// <summary>
        /// 关闭储存库
        /// </summary>
        public async Task CloseStorageAsync() {
            await Task.Run(() => {
                lock (_storageFileLocker) {
                    _storage?.Dispose();
                    if (_dataBase != null && _dataBase.State == System.Data.ConnectionState.Open) {
                        APMCore.ViewModel.StorageBase.EmptyStorage(_dataBase);
                    }
                    _dataBase?.Close();
                }
            });
        }
        /// <summary>
        /// 保存至文件
        /// </summary>
        /// <param name="filePath">保存的文件路径</param>
        public async Task SaveUserDataAsync(string filePath) {
            if (!Regex.IsMatch(UserPassword, APM.UserPasswordRegular)) {
                throw new InvalidUserPasswordException();
            }
            string passwordBackup = UserPassword;
            UserPassword = HashString.SHA(UserPassword);
            try {
                await Task.Run(() => {
                    lock (_userdataFileLocker) {
                        base.SaveToFile(filePath);
                    }
                });
                await EncryptDataBaseAsync(passwordBackup);
            }
            catch {
                throw new StorageFileIOException();
            }
            UserPassword = passwordBackup;
        }
        /// <summary>
        /// 保存储存库
        /// </summary>
        /// <returns></returns>
        public async Task SaveStorageAsync() {
            try {
                await Storage.UpdateToSourceAsync();
                await EncryptDataBaseAsync(UserPassword);
            }
            catch {
                throw new StorageFileIOException();
            }
        }

        /// <summary>
        /// 设置用户头像
        /// </summary>
        /// <param name="avatarFilePath"></param>
        /// <returns></returns>
        public async Task SetAvatarAsync(string avatarFilePath) {
            await Task.Run(() => {
                File.Copy(avatarFilePath, UserAvatarFileName, true);
            });
            OnPropertyChanged(nameof(Avatar));
        }
        #endregion

        #region 特殊方法
        public override string ToString() {
            return $"[{UserName}]\n{Description}";
        }
        #endregion

        #region 辅助方法
        private async Task EncryptDataBaseAsync(string password) {
            FileEncrypter encrypter = new FileEncrypter(new XOREncrypter(password));
            if (File.Exists(StorageFile)) {
                File.Delete(StorageFile);
                await Task.Delay(TimeSpan.FromMilliseconds(100)); //等待100毫秒，确定文件处于空闲状态
            }
            //向储存库写入数据
            lock (_storageFileLocker) {
                File.Copy(RuntimeStorageFileName, StorageFile);
                encrypter.Encrypt(StorageFile);
                Thread.Sleep(TimeSpan.FromMilliseconds(100));
                StorageEncrypted?.Invoke(this, new StorageEncryptedEventArgs(password));
            }
        }
        private async Task DecryptDataBaseAsync(string password) {
            FileEncrypter encrypter = new FileEncrypter(new XOREncrypter(password));
            if (File.Exists(RuntimeStorageFileName)) {
                File.Delete(RuntimeStorageFileName);
                await Task.Delay(TimeSpan.FromMilliseconds(100));//等待109毫秒，确定文件处于空闲状态
            }
            //解密储存库
            lock (_storageFileLocker) {
                File.Copy(StorageFile, RuntimeStorageFileName);
                encrypter.Decrypt(RuntimeStorageFileName);
                Thread.Sleep(TimeSpan.FromMilliseconds(100));
                File.SetAttributes(RuntimeStorageFileName, FileAttributes.Hidden);
            }
        }
        #endregion
        #endregion
    }
}
