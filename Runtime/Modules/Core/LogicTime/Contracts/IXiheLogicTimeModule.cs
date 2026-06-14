namespace XiheFramework.Runtime.LogicTime {
    public interface IXiheLogicTimeModule {
        string OnSetGlobalTimeScaleEventName { get; }
        float GlobalTimeScale { get; }
        float ScaledDeltaTime { get; }
        void SetGlobalTimeScaleInFrame(float timeScale, int duration, bool alsoSetUnityTimeScale = false);
        void SetGlobalTimeScaleInSecond(float timeScale, float duration, bool setUnityTimeScale = false);
        void SetGlobalTimeScalePermanent(float timeScale, bool setUnityTimeScale = false);
    }
}
