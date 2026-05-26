using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XiheFramework.Runtime.Event;

namespace XiheFramework.Runtime.Resource {
    public class PreloadDefaultAddressable : MonoBehaviour {
#if USE_TMP
        public TMP_Text display;
#else
        public Text display;
#endif
        public string[] labels = { "default" };

        private void Start() {
            StartCoroutine(LoadLabelAsync(labels));
        }

        IEnumerator LoadLabelAsync(IEnumerable<string> label) {
            // if (display) {
            //     display.text = "loading default resources...";
            // }

            var loadedAssetNames = new StringBuilder();

            yield return Game.GetModule<ResourceModule>().LoadAssetsAsyncCoroutine(label,
                onProgress: (progress) => {
                    if (display != null) display.text = $"loading: {(progress * 100):F2}%\n\n{loadedAssetNames}";
                },
                onLoaded: (address) => { loadedAssetNames.Append($"loaded: {address}\n"); });

            Game.GetModule<IXiheEventModule>().Invoke(ResourceModuleEvents.OnDefaultResourcesLoadedEvtName);
        }
    }
}
