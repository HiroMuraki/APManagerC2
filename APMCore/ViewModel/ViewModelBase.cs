using System.ComponentModel;

namespace APMCore.ViewModel {
    /// <summary>
    /// VM的基类
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged {
        #region 事件
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region 方法
        #region 公共方法
        public virtual void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #endregion
    }
}
