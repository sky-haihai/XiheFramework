using System;

namespace XiheFramework.Runtime.Base {
    /// <summary>
    /// Serialized selection for a core module contract and the implementation used by GameManager.
    /// </summary>
    [Serializable]
    public sealed class XiheCoreModuleSelection {
        public bool enabled = true;
        public string contractTypeName;
        public string implementationTypeName;
    }
}
