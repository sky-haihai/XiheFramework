using XiheFramework.Runtime.Base;

namespace XiheFramework.Runtime.UI.UIEntity {
    public class UIPopEntity : UILayoutEntityBase {
        public override string GroupName => "UIPopEntity";

        public void Close() {
            GameManager.GetModule<XiheUIModule>().ClosePop(EntityId);
        }
    }
}
