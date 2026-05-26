using UnityEditor;
using UnityEngine;
using XiheFramework.Runtime.Resource;
#if USE_ADDRESSABLE
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
#endif

namespace XiheFramework.Editor.Runtime.Resource {
    [CustomEditor(typeof(ResourceModule))]
    public class ResourceModuleEditor : UnityEditor.Editor {
        private const string AddressableResourcesRoot = "Assets/AddressableResources/";

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (AssetDatabase.IsValidFolder(AddressableResourcesRoot)) {
                GUI.enabled = true;
                GUILayout.Label("Resource Folder: ");
                GUI.enabled = false;
                GUILayout.TextField(AddressableResourcesRoot);
                GUILayout.Space(2f);
                GUI.enabled = true;
            }
        }
    }
}