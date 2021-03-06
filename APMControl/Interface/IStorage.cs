﻿using System;
using System.Threading.Tasks;

namespace APMControl.Interface {
    using ContainerCollection = DispatchedObservableCollection<IContainer>;
    using FilterCollection = DispatchedObservableCollection<IFilter>;

    public interface IStorage : IDisposable {
        /// <summary>
        /// 新建Container所用的模板
        /// </summary>
        ContainerTemplate ContainerTemplate { get; }
        /// <summary>
        /// 可视化的Filter列表
        /// </summary>
        FilterCollection Filters { get; }
        /// <summary>
        /// 可视化的Container列表
        /// </summary>
        ContainerCollection Containers { get; }

        /// <summary>
        /// 更新数据至源
        /// </summary>
        Task UpdateToSourceAsync();
        /// <summary>
        /// 重新加载Filters与Containers
        /// </summary>
        Task ReloadAsync();
        /// <summary>
        /// 重新加载Filters
        /// </summary>
        Task ReloadFiltersAsync();
        /// <summary>
        /// 重新加载Containers
        /// </summary>
        Task ReloadContainersAsync();
        /// <summary>
        /// 按key搜索Containers
        /// </summary>
        /// <param name="key"></param>
        Task ReloadContainersByKeywordAsync(string key);
        /// <summary>
        /// 反转所有Filters开关状态
        /// </summary>
        Task InverseFiltersAsync();
        /// <summary>
        /// 打开所有Filters
        /// </summary>
        Task ToggleOnAllFiltersAsync();
        /// <summary>
        /// 聚焦Containers
        /// </summary>
        Task FocusContainersAsync(IFilter filter);

        /// <summary>
        /// 添加一个Filter
        /// </summary>
        /// <returns></returns>
        Task<IFilter> AddFilterAsync();
        /// <summary>
        /// 移除一个Filter
        /// </summary>
        /// <param name="filter"></param>
        Task<bool> RemoveFilterAsync(IFilter filter);
        /// <summary>
        /// 移除所有空Filter
        /// </summary>
        /// <returns>移除的Filter数量</returns>
        Task<int> ClearEmptyFiltersAsync();

        /// <summary>
        /// 添加一个Container
        /// </summary>
        /// <returns></returns>
        Task<IContainer> AddContainerAsync();
        /// <summary>
        /// 移除一个Container
        /// </summary>
        /// <param name="container"></param>
        Task<bool> RemoveContainerAsync(IContainer container);
        /// <summary>
        /// 移除所有空Container
        /// </summary>
        /// <returns>移除的Container数量</returns>
        Task<int> ClearEmptyContainersAsync();
        /// <summary>
        /// 将Container内容复制到Contaienr模板
        /// </summary>
        /// <param name="container">要复制内容的Container</param>
        /// <returns></returns>
        Task SetContainerToTemplateAsync(IContainer container);
    }
}
