namespace XiheFramework.Runtime.UI.UIEntity {
    public class UIPageEntity : UILayoutEntityBase {

        public override string GroupName => "UIPageEntity";
        /// <summary>
        /// Is this page home page
        /// </summary>
        public bool homePage = false;

        /// <summary>
        /// destroy instead of hide
        /// </summary>
        // public bool destroyOnClose = true;

        public void Show() {
            gameObject.SetActive(true);
            OnShow();
        }

        public void Hide() {
            gameObject.SetActive(false);
            OnHide();
        }

        protected virtual void OnShow() { }

        protected virtual void OnHide() { }
    }
}