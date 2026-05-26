using XiheFramework.Runtime.UI.UIEntity;

namespace XiheFramework.Runtime.UI {
    public interface IXiheUIModule {
        uint OpenPage(string address);
        uint ReturnPrevPage();
        uint HomePage();
        uint OpenPop(string address);
        T OpenPop<T>(string address) where T : UIPopEntity;
        void ClosePop(uint popEntityId);
        uint OpenOverlay(string address);
        T OpenOverlay<T>(string address) where T : UIOverlayEntity;
        void CloseOverlay(uint overlayEntityId);
    }
}
