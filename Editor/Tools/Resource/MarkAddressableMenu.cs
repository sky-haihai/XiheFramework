using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
#if USE_ADDRESSABLE
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using XiheFramework.Runtime.Entity;
#endif

namespace XiheFramework.Editor.Tools.Resource {
    public static class MarkAddressableMenu {
        private const string AddressableResourcesRoot = "Assets/AddressableResources/";
        private const string TemplatePath = "Assets/XiheFramework/Editor/Tools/Resource/AddressWrapperTemplate.txt";
        private const string LinkScriptsDirectory = "Assets/Scripts/GameConstant/";

        [MenuItem("XiheFramework/Resource/Create Addressable Folder")]
        public static void CreateAddressableFolder() {
            AssetDatabase.CreateFolder("Assets", "AddressableResources");
        }

        [MenuItem("XiheFramework/Resource/Create Addressable Folder", true)]
        public static bool CreateAddressableFolderValidate() {
            return !AssetDatabase.IsValidFolder(AddressableResourcesRoot);
        }

        [MenuItem("XiheFramework/Resource/Mark All Addressable")]
        public static void MarkAddressableAll() {
            GenerateAddressWrapper();
        }

        [MenuItem("XiheFramework/Resource/Mark All Addressable", true)]
        public static bool MarkAddressableAllValidate() {
            return AssetDatabase.IsValidFolder(AddressableResourcesRoot) && File.Exists(TemplatePath);
        }

        private static void GenerateAddressWrapper() {
#if USE_ADDRESSABLE
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null) {
                Debug.LogWarning("Addressable Asset Settings not found! Go to: Window->Asset Management->Addressables->Groups, Create Addressable Asset Settings.");
                return;
            }

            var addressInfoArrays = new List<AssetNameAddressPair>();
            MarkAssetsAddressable(AddressableResourcesRoot, "Default Local Group", SearchOption.TopDirectoryOnly, out var validAddressInfos);
            addressInfoArrays.AddRange(validAddressInfos);

            var subFolders = Directory.GetDirectories(AddressableResourcesRoot, "*", SearchOption.TopDirectoryOnly);
            foreach (var path in subFolders) {
                var groupName = new DirectoryInfo(path).Name;
                MarkAssetsAddressable(path, groupName, SearchOption.AllDirectories, out validAddressInfos);
                var combinedAddresses = new List<AssetNameAddressPair>();
                combinedAddresses.AddRange(addressInfoArrays);
                combinedAddresses.AddRange(validAddressInfos);
                addressInfoArrays = combinedAddresses;
            }

            //detect if there are empty addressable groups
            foreach (var group in settings.groups) {
                if (group.entries.Count == 0 && !group.IsDefaultGroup()) {
                    Debug.LogWarning($"AddressableGroup {group.Name} is empty, consider deleting it.");
                }
            }

            if (!Directory.Exists(LinkScriptsDirectory)) {
                Directory.CreateDirectory(LinkScriptsDirectory);
            }

            if (addressInfoArrays.Count == 0) {
                Debug.LogWarning("No addressable assets found! Empty ResourceAddresses script generated.");
                WriteWrapperScript("Resource", null, false);
            }

            WriteWrapperScript("Resource", addressInfoArrays.ToArray());

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Generated Address Wrapper Script at " + LinkScriptsDirectory);
#else
            Debug.LogError("Please import Addressable Package and define USE_ADDRESSABLE in your project settings: Player->Other Settings->Scripting Define Symbols");
#endif
        }
#if USE_ADDRESSABLE
        private static void MarkAssetsAddressable(string path, string addressableGroupName, SearchOption searchOption, out AssetNameAddressPair[] addressInfos) {
            var addressesResult = new List<AssetNameAddressPair>();
            var files = Directory.GetFiles(path, "*.*", searchOption);

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            AddressableAssetGroup group = settings.FindGroup(addressableGroupName);
            if (group == null) {
                group = settings.CreateGroup(addressableGroupName, false, false, true, null);
                group.AddSchema<BundledAssetGroupSchema>();
                group.AddSchema<ContentUpdateGroupSchema>();
                Debug.Log("Created Addressable Group: " + addressableGroupName);
            }

            foreach (var file in files) {
                if (!File.Exists(file)) continue;

                var success = MarkAssetAddressable(addressableGroupName, file, out var addressInfo);
                if (success) {
                    addressesResult.Add(addressInfo);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            addressInfos = addressesResult.ToArray();
        }

        private static bool MarkAssetAddressable(string folderName, string assetPath, out AssetNameAddressPair addressInfo) {
            addressInfo = new AssetNameAddressPair();
            if (string.IsNullOrEmpty(folderName)) {
                return false;
            }

            var guid = AssetDatabase.AssetPathToGUID(assetPath);
            if (string.IsNullOrEmpty(guid)) {
                return false;
            }

            var type = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
            var asset = AssetDatabase.LoadAssetAtPath(assetPath, type);
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (asset == null) {
                settings.RemoveAssetEntry(guid);
                return false;
            }

            string assetAddress = "";

            switch (asset) {
                case GameObject gameObject:
                    var gameEntity = gameObject.GetComponent<GameEntityBase>();
                    if (gameEntity != null) {
                        assetAddress = $"{gameEntity.GroupName}_{asset.name}";
                    }
                    else {
                        assetAddress = $"{type.Name}_{asset.name}";
                    }

                    break;
                default:
                    assetAddress = $"{type.Name}_{asset.name}";
                    break;
            }

            var group = settings.FindGroup(folderName);
            AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, group);
            entry.address = assetAddress;
            addressInfo.assetName = $"{folderName}_{asset.name}";
            addressInfo.address = assetAddress;
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, null, true);
            return true;
        }

        private static void WriteWrapperScript(string groupName, AssetNameAddressPair[] addressInfo, bool ignoreEmpty = true) {
            string scriptTemplate = File.ReadAllText(TemplatePath);
            var scriptName = RemoveSpecialCharacters(groupName) + "Addresses";
            scriptTemplate = scriptTemplate.Replace("#GENERATETIME#", DateTime.UtcNow.ToUniversalTime().ToString(CultureInfo.CurrentCulture));
            scriptTemplate = scriptTemplate.Replace("#SCRIPTNAME#", scriptName);
            string content = string.Empty;
            if (addressInfo != null && addressInfo.Length > 0) {
                content = GetContentString(addressInfo);
            }

            if (ignoreEmpty && string.IsNullOrEmpty(content)) {
                return;
            }

            scriptTemplate = scriptTemplate.Replace("#ADDRESSCONTENT#", content);

            File.WriteAllText($"{LinkScriptsDirectory}{scriptName}.cs", scriptTemplate);
        }

        private static string GetContentString(AssetNameAddressPair[] addressInfo) {
            var result = "";
            for (var i = 0; i < addressInfo.Length; i++) {
                var assetName = addressInfo[i].assetName;
                var address = addressInfo[i].address;
                assetName = RemoveSpecialCharacters(assetName);
                result += $"\t\tpublic const string {assetName} = \"{address}\";\n";
            }

            if (string.IsNullOrEmpty(result)) {
                return null;
            }

            result = result[..^1];

            return result;
        }

        private static string RemoveSpecialCharacters(string input) {
            if (string.IsNullOrEmpty(input)) {
                return "_";
            }

            //if first char is a number
            string output = Regex.Replace(input, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
            if (string.IsNullOrEmpty(output)) {
                return "_";
            }

            if (char.IsDigit(output[0])) {
                output = "_" + output;
            }

            return output;
        }


        public struct AssetNameAddressPair {
            public string assetName;
            public string address;
        }
#endif
    }
}
