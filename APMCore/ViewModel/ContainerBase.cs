using APMCore.ViewModel.Helper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace APMCore.ViewModel {
    /// <summary>
    /// 用于储存Pair记录组
    /// </summary>
    public class ContainerBase : APMBase<Model.Container> {
        #region 属性
        #region 公共属性
        /// <summary>
        /// 关联数据模型的唯一标识符，为只读属性
        /// </summary>
        public long ContainerUID {
            get {
                return _dataSource.ContainerUID;
            }
        }
        /// <summary>
        /// 所属过滤器唯一标识符
        /// </summary>
        public long FilterUID {
            get {
                return _dataSource.FilterUID;
            }
            set {
                _dataSource.FilterUID = value;
            }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Header {
            get {
                return _dataSource.Header;
            }
            set {
                _dataSource.Header = value;
                OnPropertyChanged(nameof(Header));
            }
        }
        /// <summary>
        /// 描述
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
        #endregion

        #region 私有字段
        /// <summary>
        /// 关联数据
        /// </summary>
        protected readonly new Model.Container _dataSource;
        #endregion
        #endregion

        #region 构造函数
        /// <summary>
        /// 表示一个数据容器
        /// </summary>
        /// <param name="source">数据源</param>
        public ContainerBase(Model.Container source){
            _dataSource = source;
        }
        #endregion

        #region 方法
        #region 保护方法
        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="container">复制源</param>
        protected virtual void CopyProperties(ContainerBase container) {
            FilterUID = container.FilterUID;
            Header = container.Header;
            Description = container.Description;
            Avatar = container.Avatar;
        }
        /// <summary>
        /// 获取Pairs
        /// </summary>
        /// <param name="conn">指定的数据库</param>
        protected virtual IEnumerable<Model.Pair> FetchPairsHelper(Predicate<Model.Pair> predicate) {
            SQLiteCommand cmd = new SQLiteCommand(DataBase);
            cmd.CommandText = $@"Select *
                                 From {APM.PairsTable}
                                 Where {APM.PairContainer} == {ContainerUID} ";

            using (SQLiteDataReader results = cmd.ExecuteReader()) {
                while (results.Read()) {
                    Model.Pair pair = PairHelper.FetchFrom(results);
                    if (predicate(pair)) {
                        yield return pair;
                    }
                }
            }
        }
        /// <summary>
        /// 获取关联的Pair数量
        /// </summary>
        /// <returns></returns>
        protected virtual long CountPairs() {
            SQLiteCommand cmd = new SQLiteCommand(DataBase);
            cmd.CommandText = $@"Select Count(*)
                                 From {APM.PairsTable}
                                 Where {APM.PairContainer} == {ContainerUID}";
            return (long)cmd.ExecuteScalar();
        }
        #endregion

        #region 私有方法
        protected override UpdateInformation InsertInto() {
            return ContainerHelper.Insert(_dataSource, DataBase);
        }
        protected override UpdateInformation UpdateTo() {
            return ContainerHelper.Update(_dataSource, DataBase);
        }
        protected override UpdateInformation DeleteFrom() {
            return ContainerHelper.Delete(_dataSource, DataBase);
        }
        #endregion
        #endregion
    }
}
