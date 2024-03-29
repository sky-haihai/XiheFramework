using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using XiheFramework.Combat.Animation;
using XiheFramework.Combat.Base;
using XiheFramework.Combat.Particle;
using XiheFramework.Combat.Projectile;

namespace XiheFramework.Combat.Editor {
    public class BatchAddressableWindow : EditorWindow {
        [MenuItem("Window/Batch Addressable")]
        public static void ShowWindow() {
            GetWindow(typeof(BatchAddressableWindow));
        }

        void OnGUI() {
            EditorGUILayout.LabelField("Addressable Addresses Batch Modifier", EditorStyles.boldLabel);

            if (Application.isPlaying) {
                EditorGUILayout.HelpBox("Disabled During Play Mode", MessageType.Warning);
                return;
            }

            // get current selected path
            string fullPath = "";
            if (Selection.activeObject != null) {
                fullPath = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (string.IsNullOrEmpty(fullPath)) {
                    EditorGUILayout.HelpBox("Select a Prefab to Start!", MessageType.Warning);
                    return;
                }

                fullPath = Path.GetDirectoryName(fullPath);
                fullPath = fullPath?.Replace('\\', '/');
            }
            else {
                EditorGUILayout.HelpBox("Please Select a Prefab First!", MessageType.Warning);
                return;
            }

            EditorGUILayout.LabelField("Full Path: ", fullPath);

            if (GUILayout.Button("Generate")) {
                if (string.IsNullOrEmpty(fullPath) || fullPath == "Nothing selected!") {
                    Debug.LogError("Full Path cannot be empty!");
                    return;
                }

                if (!AssetDatabase.IsValidFolder(fullPath)) {
                    Debug.LogError("Path does not exist!");
                    return;
                }

                // get all prefabs in the folder
                string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[] { fullPath });
                string[] assetPaths = guids.Select(g => AssetDatabase.GUIDToAssetPath(g)).ToArray();

                // get Addressable Asset Settings
                AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

                foreach (string assetPath in assetPaths) {
                    var guid = AssetDatabase.AssetPathToGUID(assetPath);
                    CombatEntityBase entity = AssetDatabase.LoadAssetAtPath<CombatEntityBase>(assetPath);
                    if (entity == null) {
                        settings.RemoveAssetEntry(guid);
                        continue;
                    }

                    if (string.IsNullOrEmpty(entity.entityName)) {
                        settings.RemoveAssetEntry(guid);
                        continue;
                    }

                    string groupName = "Default";
                    if (entity is Action.ActionEntity) {
                        groupName = "ActionEntity";
                    }

                    if (entity is Animation2DEntity) {
                        groupName = "Animation2DEntity";
                    }

                    if (entity is Buff.BuffEntity) {
                        groupName = "BuffEntity";
                    }

                    if (entity is ParticleEntity) {
                        groupName = "ParticleEntity";
                    }

                    if (entity is CombatEntity) {
                        groupName = "CombatEntity";
                    }

                    if (entity is ProjectileEntity) {
                        groupName = "ProjectileEntity";
                    }

                    //find group
                    AddressableAssetGroup group = settings.FindGroup(groupName);
                    if (group == null) {
                        group = settings.CreateGroup(groupName, false, false, true, null);
                    }

                    string assetName = groupName + "_" + entity.entityName;

                    // actually set address
                    AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, group);
                    entry.address = assetName;
                }

                settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, null, true);
            }
        }
    }
}