using System;
using XiheFramework.Runtime.Base;
using XiheFramework.Runtime.Event;

namespace XiheFramework.Runtime.Serialization {
    public abstract class XiheSerializationModuleBase : GameModuleBase, IXiheSerializationModule {
        public string OnSaveEventName => "Serialization.OnSave";
        public string OnLoadEventName => "Serialization.OnLoad";

        public void SaveGame() {
            OnSaveCallback();

            var args = new OnSaveEventArgs();
            args.timeStamp = DateTime.Now;
            GameManager.GetModule<IXiheEventModule>().InvokeNow(OnSaveEventName, args);
        }

        public void LoadGame() {
            OnLoadCallback();

            var args = new OnLoadEventArgs();
            GameManager.GetModule<IXiheEventModule>().InvokeNow(OnLoadEventName, args);
        }

        protected override void OnInstantiated() {
        }

        protected virtual void OnSaveCallback() { }
        protected virtual void OnLoadCallback() { }
    }
}
