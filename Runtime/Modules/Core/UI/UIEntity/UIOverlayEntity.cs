using XiheFramework.Runtime.Base;

namespace XiheFramework.Runtime.UI.UIEntity {
    public class UIOverlayEntity : UILayoutEntityBase {
        public override string GroupName => "UIOverlayEntity";

        public void Close() {
            GameManager.GetModule<XiheUIModule>().CloseOverlay(EntityId);
        }
    }
}
