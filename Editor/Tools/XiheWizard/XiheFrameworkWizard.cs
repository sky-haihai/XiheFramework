using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace XiheFramework.Editor.Utility.XiheWizard {
    public class XiheFrameworkWizard : UnityEditor.EditorWindow {
        private bool m_UseAddressable = false;
        private bool m_UseCinemachine = false;
        private bool m_UseRewired = false;
        private bool m_UseTMP = false;
        private bool m_UseWwise = false;

        private string m_AsmdefPath = "Assets/XiheFramework/XiheFramework.asmdef";

        [MenuItem("XiheFramework/Setup Wizard", priority = -1)]
        public static void ShowWindow() {
            GetWindow<XiheFrameworkWizard>("XiheFramework Wizard");
        }

        private void OnEnable() {
            BuildTarget currentBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            BuildTargetGroup currentBuildTargetGroup = BuildPipeline.GetBuildTargetGroup(currentBuildTarget);
#if UNITY_2021_2_OR_NEWER
            RefreshSymbols(NamedBuildTarget.FromBuildTargetGroup(currentBuildTargetGroup));
#else
            RefreshSymbols(currentBuildTargetGroup);
#endif
        }

        private void OnGUI() {
            GUILayout.Label("XiheFramework Setup Wizard", new GUIStyle(GUI.skin.label) { fontSize = 20, fixedHeight = 40 });
            GUILayout.Space(5);
            GUILayout.Label("Current Build Target:", EditorStyles.boldLabel);
            GUILayout.Label(EditorUserBuildSettings.activeBuildTarget.ToString(), new GUIStyle(GUI.skin.label) { fontSize = 15, fixedHeight = 30 });

            GUILayout.Space(10);
            GUILayout.Label("ASMDEF File Path:", EditorStyles.boldLabel);
            m_AsmdefPath = GUILayout.TextField(m_AsmdefPath);
            GUILayout.Space(10);

            GUILayout.Label("Enable AFTER Importing Plugins:", EditorStyles.boldLabel);

            // Display checkboxes and check for changes
            bool anyChange = false;

            bool newUseAddressable = GUILayout.Toggle(m_UseAddressable, "USE_ADDRESSABLE");
            if (newUseAddressable != m_UseAddressable) {
                m_UseAddressable = newUseAddressable;
                anyChange = true;
            }

            bool newUseCinemachine = GUILayout.Toggle(m_UseCinemachine, "USE_CINEMACHINE");
            if (newUseCinemachine != m_UseCinemachine) {
                m_UseCinemachine = newUseCinemachine;
                anyChange = true;
            }

            bool newUseRewired = GUILayout.Toggle(m_UseRewired, "USE_REWIRED");
            if (newUseRewired != m_UseRewired) {
                m_UseRewired = newUseRewired;
                anyChange = true;
            }

            bool newUseTMP = GUILayout.Toggle(m_UseTMP, "USE_TMP");
            if (newUseTMP != m_UseTMP) {
                m_UseTMP = newUseTMP;
                anyChange = true;
            }

            bool newUseWwise = GUILayout.Toggle(m_UseWwise, "USE_WWISE");
            if (newUseWwise != m_UseWwise) {
                m_UseWwise = newUseWwise;
                anyChange = true;
            }

            if (anyChange) {
                ApplySymbols();
            }

            GUILayout.Space(10);
            if (GUILayout.Button("Create Game Folders")) {
                var folderList = new List<string> { "ArtResources", "AddressableResources", "Scripts", "Scenes", "Settings" };
                foreach (var folder in folderList) {
                    if (!AssetDatabase.IsValidFolder("Assets/" + folder)) {
                        AssetDatabase.CreateFolder("Assets", folder);
                    }
                }
            }
        }

        private void ApplySymbols() {
            BuildTarget currentBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            BuildTargetGroup currentBuildTargetGroup = BuildPipeline.GetBuildTargetGroup(currentBuildTarget);

#if UNITY_2021_2_OR_NEWER
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(currentBuildTargetGroup);
            string currentSymbols = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
#else
            string currentSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentBuildTargetGroup);
#endif
            List<string> newSymbols = currentSymbols.Split(';').ToList();

            // Update the symbols managed by XiheFramework
            UpdateSymbol(newSymbols, m_UseAddressable, "USE_ADDRESSABLE");
            UpdateSymbol(newSymbols, m_UseCinemachine, "USE_CINEMACHINE");
            UpdateSymbol(newSymbols, m_UseRewired, "USE_REWIRED");
            UpdateSymbol(newSymbols, m_UseTMP, "USE_TMP");
            UpdateSymbol(newSymbols, m_UseWwise, "USE_WWISE");

            var symbolString = string.Join(";", newSymbols.ToArray());

            // Set new symbols
#if UNITY_2021_2_OR_NEWER
            PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, symbolString);
#else
            PlayerSettings.SetScriptingDefineSymbolsForGroup(currentBuildTargetGroup, symbolString);
#endif
        }

        private void UpdateSymbol(List<string> symbols, bool isEnabled, string symbol) {
            if (isEnabled && !symbols.Contains(symbol)) {
                symbols.Add(symbol);
            }
            else if (!isEnabled && symbols.Contains(symbol)) {
                symbols.Remove(symbol);
            }
        }

#if UNITY_2021_2_OR_NEWER
        private void RefreshSymbols(NamedBuildTarget buildTarget) {
            string currentSymbols = PlayerSettings.GetScriptingDefineSymbols(buildTarget);
#else
        private void RefreshSymbols(BuildTargetGroup buildTargetGroup) {
            string currentSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
#endif
            List<string> splitSymbols = currentSymbols.Split(';').ToList();

            m_UseAddressable = splitSymbols.Contains("USE_ADDRESSABLE");
            m_UseCinemachine = splitSymbols.Contains("USE_CINEMACHINE");
            m_UseRewired = splitSymbols.Contains("USE_REWIRED");
            m_UseTMP = splitSymbols.Contains("USE_TMP");
            m_UseWwise = splitSymbols.Contains("USE_WWISE");
        }
    }
}
