namespace XiheFramework.Runtime.UI.UIEntity {
    public class UIPopEntity : UILayoutEntityBase {
        public override string GroupName => "UIPopEntity";

        public void Close() {
            Game.GetModule<XiheUIModule>().ClosePop(EntityId);
        }
    }
}
