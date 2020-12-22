using System.Threading.Tasks;

namespace APMControl.Interface {
    public interface IFilter {
        /// <summary>
        /// 过滤器名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 过滤器特征标
        /// </summary>
        public string Identifier { get; }
        /// <summary>
        /// 过滤器开/关状态
        /// </summary>
        public bool IsOn { get; }

        /// <summary>
        /// 更新数据至源
        /// </summary>
        /// <returns></returns>
        public Task<APMCore.UpdateInformation> UpdateToSourceAsync();
        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="filter"></param>
        public Task CopyPropertiesAsync(IFilter filter);
    }
}
