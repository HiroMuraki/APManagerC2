using System.Threading.Tasks;

namespace APMControl.Interface {
    public interface IUserData {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string UserPassword { get; set; }
        /// <summary>
        /// 附加描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 头像路径
        /// </summary>
        public string Avatar { get;  }
        /// <summary>
        /// 是否为编辑模式
        /// </summary>
        public bool IsEditMode { get; set; }
        /// <summary>
        /// 指示容器列数
        /// </summary>
        public int ColumnSize { get; set; }
        /// <summary>
        /// Storage文件
        /// </summary>
        public string StorageFile { get; }
        /// <summary>
        /// 储存库
        /// </summary>
        public IStorage Storage { get; }

        /// <summary>
        /// 打开储存库
        /// </summary>
        /// <param name="password">密码</param>
        Task OpenStorageAsync(string password);
        /// <summary>
        /// 关闭储存库
        /// </summary>
        Task CloseStorageAsync();
        /// <summary>
        /// 保存至文件
        /// </summary>
        /// <param name="filePath">保存的文件路径</param>
        Task SaveUserDataAsync(string filePath);
        /// <summary>
        /// 保存储存库
        /// </summary>
        /// <returns></returns>
        Task SaveStorageAsync();
        /// <summary>
        /// 设置用户头像
        /// </summary>
        /// <param name="avatarFilePath"></param>
        /// <returns></returns>
        Task SetAvatarAsync(string avatarFilePath);
        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="source"></param>
        void CopyProperties(IUserData source);
    }
}
