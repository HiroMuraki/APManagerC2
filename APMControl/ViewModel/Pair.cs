using APMControl.Interface;

namespace APMControl {
    public sealed class Pair : APMCore.ViewModel.PairBase, IPair {
        #region 属性
        #region 公共属性
        public bool IsEmpty {
            get {
                return Title == "" && Detail == "";
            }
        }
        #endregion
        #endregion

        #region 构造函数
        public Pair(APMCore.Model.Pair source) : base(source) {

        }
        #endregion
    }
}
