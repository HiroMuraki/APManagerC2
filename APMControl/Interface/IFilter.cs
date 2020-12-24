using System.Threading.Tasks;

namespace APMControl.Interface {
    public interface IFilter {
        /// <summary>
        /// 过滤器名
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 过滤器特征标
        /// </summary>
        string Identifier { get; }
        /// <summary>
        /// 过滤器开/关状态
        /// </summary>
        bool IsOn { get; }

        Task ToggleAsync();
        /// <summary>
        /// 更新数据至源
        /// </summary>
        /// <returns></returns>
        Task<APMCore.UpdateInformation> UpdateToSourceAsync();
        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="filter"></param>
        Task CopyPropertiesAsync(IFilter filter);
    }
}
