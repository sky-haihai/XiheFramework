using System;
using XiheFramework.Runtime.Serialization.Serializable;

namespace XiheFramework.Runtime.Serialization {
    public struct OnLoadEventArgs {
        public DateTime timeStamp;
        public SerializableEntityData entityData;
    }
}