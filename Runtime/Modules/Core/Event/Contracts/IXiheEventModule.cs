using System;

namespace XiheFramework.Runtime.Event {
    /// <summary>
    /// Event bus contract used by framework modules.
    /// </summary>
    public interface IXiheEventModule {
        string Subscribe(string eventName, EventHandler<object> handler);
        void Unsubscribe(string eventName, string handlerId);
        void InvokeNow(string eventName, object eventArg = null);
        void Invoke(string eventName, object eventArg = null, object sender = null);
    }
}
