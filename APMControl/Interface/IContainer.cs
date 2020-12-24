using APMCore;
using System.Threading.Tasks;

namespace APMControl.Interface {
    using PairCollection = DispatchedObservableCollection<IPair>;
    public interface IContainer {
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
        IFilter Filter { get; set; }
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
        Task<UpdateInformation> UpdateToSourceAsync();
        /// <summary>
        /// 添加Pair
        /// </summary>
        /// <returns>添加的Pair</returns>
        Task<IPair> AddPairAsync();
        /// <summary>
        /// 移除Pair
        /// </summary>
        /// <param name="pair">要移除的Pair</param>
        Task<bool> RemovePairAsync(IPair pair);
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
