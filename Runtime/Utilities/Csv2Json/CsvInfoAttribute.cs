using System;

namespace XiheFramework.Runtime.Utility.Csv2Json {
    /// <summary>
    /// Marks a serializable data row type as selectable by the Csv2Json editor tool.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class CsvInfoAttribute : Attribute { }
}
