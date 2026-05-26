using XiheFramework.Runtime.Base;

namespace XiheFramework.Runtime.Blackboard {
    public abstract class XiheBlackboardModuleBase : GameModuleBase, IXiheBlackboardModule {
        public abstract T CreateBlackboard<T>(string blackboardName) where T : IBlackboard, new();
        public abstract T GetBlackboard<T>(string blackboardName) where T : IBlackboard;
        public abstract IBlackboard GetBlackboard(string blackboardName);
        public abstract void ReleaseBlackboard(string blackboardName);
    }
}