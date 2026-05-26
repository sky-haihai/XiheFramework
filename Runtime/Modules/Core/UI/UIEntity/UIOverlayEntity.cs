namespace XiheFramework.Runtime.UI.UIEntity {
    public class UIOverlayEntity : UILayoutEntityBase {
        public override string GroupName => "UIOverlayEntity";

        public void Close() {
            Game.GetModule<XiheUIModule>().CloseOverlay(EntityId);
        }
    }
}
