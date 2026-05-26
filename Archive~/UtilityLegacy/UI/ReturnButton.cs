using XiheFramework.Runtime;

namespace XiheFramework.Utility.UI {
    public class ReturnButton : Button {
        protected override void Start() {
            base.Start();

            onClick.AddListener(() => { Game.UI.ReturnPrevPage(); });
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            onClick.RemoveAllListeners();
        }
    }
}