namespace XiheFramework.Runtime.Serialization {
    public interface IXiheSerializationModule {
        public string OnSaveEventName { get; }
        public string OnLoadEventName { get; }
        public void SaveGame();
        public void LoadGame();
    }
}