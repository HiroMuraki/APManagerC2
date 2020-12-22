using System.Threading.Tasks;

namespace APMControl.Interface {
    using PairCollection = DispatchedObservableCollection<Pair>;
    public interface IContainer {
        /// <summary>
        /// 所属过滤器唯一标识符
        /// </summary>
        long FilterUID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        string Header { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// 头像路径
        /// </summary>
        string Avatar { get; }
        /// <summary>
        /// Filter
        /// </summary>
        Filter Filter { get; set; }
        /// <summary>
        /// Pair组
        /// </summary>
        PairCollection Pairs { get; }

        /// <summary>
        /// 打开容器
        /// </summary>
        Task OpenAsync();
        /// <summary>
        /// 关闭容器
        /// </summary>
        Task CloseAsync();
        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="source">数据源</param>
        Task CopyPropertiesAsync(IContainer source);
        /// <summary>
        /// 更新数据至源
        /// </summary>
        /// <returns></returns>
        Task<APMCore.UpdateInformation> UpdateToSourceAsync();
        /// <summary>
        /// 添加Pair
        /// </summary>
        /// <returns>添加的Pair</returns>
        Task<Pair> AddPairAsync();
        /// <summary>
        /// 移除Pair
        /// </summary>
        /// <param name="pair">要移除的Pair</param>
        Task<bool> RemovePairAsync(Pair pair);
        /// <summary>
        /// 设置Container头像
        /// </summary>
        /// <param name="filePath">头像文件路径</param>
        Task<bool> SetAvatarAsync(string filePath);
        /// <summary>
        /// 移除空Container
        /// </summary>
        /// <returns>移除的Cotnainer数量</returns>
        Task<int> ClearEmptyPairsAsync();
    }
}
