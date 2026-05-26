using XiheFramework.Runtime.Base;

namespace XiheFramework.Runtime.Audio {
    public abstract class XiheAudioModuleBase : GameModuleBase {
        public override int Priority => CoreModulePriority.Audio;

        public T As<T>() where T : XiheAudioModuleBase {
            if (this is T audioModule) {
                return audioModule;
            }

            Game.LogError($"[AUDIO] Current Audio Module is not a {typeof(T).Name}. Actual type is {GetType().Name}");
            return null;
        }
    }
}
