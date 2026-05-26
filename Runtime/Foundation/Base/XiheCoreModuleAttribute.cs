using System;

namespace XiheFramework.Runtime.Base {
    /// <summary>
    /// Marks a module as part of the default runtime core set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class XiheCoreModuleAttribute : Attribute {
        public XiheCoreModuleAttribute(Type contractType) {
            ContractType = contractType;
        }

        public Type ContractType { get; }
    }
}
