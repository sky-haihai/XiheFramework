namespace XiheFramework.Runtime.Blackboard {
    public interface IXiheBlackboardModule {
        /// <summary>
        /// Create a new blackboard
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T CreateBlackboard<T>(string blackboardName) where T : IBlackboard, new();

        /// <summary>
        /// Get the reference of name-specific blackboard
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetBlackboard<T>(string blackboardName) where T : IBlackboard;

        IBlackboard GetBlackboard(string blackboardName);

        /// <summary>
        /// Release/Remove name-specific blackboard from memory
        /// </summary>
        void ReleaseBlackboard(string blackboardName);
    }
}