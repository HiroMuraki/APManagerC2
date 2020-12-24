using APMCore.ViewModel.Helper;

namespace APMCore.ViewModel {
    /// <summary>
    /// Container所储存信息的单元
    /// </summary>
    public class PairBase : APMBase<Model.Pair> {
        #region 属性
        #region 公共属性
        /// <summary>
        /// 关联数据模型唯一标识符
        /// </summary>
        public long PairUID {
            get {
                return _dataSource.PairUID;
            }
        }
        /// <summary>
        /// 所属数据容器的UID
        /// </summary>
        public long ContainerUID {
            get {
                return _dataSource.ContainerUID;
            }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title {
            get {
                return _dataSource.Title;
            }
            set {
                _dataSource.Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        /// <summary>
        /// 细节
        /// </summary>
        public string Detail {
            get {
                return _dataSource.Detail;
            }
            set {
                _dataSource.Detail = value;
                OnPropertyChanged(nameof(Detail));
            }
        }
        #endregion

        #region 私有字段
        /// <summary>
        /// 数据源
        /// </summary>
        protected readonly new Model.Pair _dataSource;
        #endregion
        #endregion

        #region 构造函数
        /// <summary>
        /// 表示一个记录组
        /// </summary>
        /// <param name="source">数据源</param>
        protected PairBase(Model.Pair source) {
            _dataSource = source;
        }
        #endregion

        #region 方法
        #region 保护方法
        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="pair">复制源</param>
        protected virtual void CopyProperties(PairBase pair) {
            Title = pair.Title;
            Detail = pair.Detail;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 添加到数据库
        /// </summary>
        /// <param name="conn">目标数据库</param>
        /// <returns>受影响的记录数</returns>
        protected override UpdateInformation InsertInto() {
            return PairHelper.Insert(_dataSource, DataBase);
        }
        /// <summary>
        /// 从数据库中修改
        /// </summary>
        /// <param name="conn">目标数据库</param>
        /// <returns>受影响的记录数</returns>
        protected override UpdateInformation UpdateTo() {
            return PairHelper.Update(_dataSource, DataBase);
        }
        /// <summary>
        /// 从数据库中删除
        /// </summary>
        /// <param name="conn">指定数据库</param>
        /// <returns>受影响的记录数</returns>
        protected override UpdateInformation DeleteFrom() {
            return PairHelper.Insert(_dataSource, DataBase);
        }
        #endregion
        #endregion
    }
}
