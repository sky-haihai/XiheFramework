using UnityEngine;
using XiheFramework.Runtime.Entity;

namespace XiheFramework.Runtime.UI.UIEntity {
    public abstract class UIElementEntityBase : GameEntityBase {
        public override string GroupName => "UIElementEntity";

        protected RectTransform rectTransform;

        protected override void OnInitCallback() {
            base.OnInitCallback();

            if (transform is not RectTransform) {
                rectTransform = gameObject.AddComponent<RectTransform>();
            }
            else {
                rectTransform = transform as RectTransform;
            }
        }
    }
}