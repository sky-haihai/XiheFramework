using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using XiheFramework.Runtime.Base;

namespace XiheFramework.Runtime.Localization {
    public class LocalizationModule : GameModuleBase {
        public override int Priority => 0;
        public Languages language = Languages.English;

        public string streamingPath = "Localization.json";

        private readonly Dictionary<string, string[]> m_LocalizationDictionary = new();

        private void Start() {
            //ReadLocalizationFile();
            //Debug.LogInfo(GetValue("上午"));
        }
        
        public string GetValue(string key) {
            return m_LocalizationDictionary[key][(int)language];
        }

        private void ReadLocalizationFile() {
            var strPath = Application.streamingAssetsPath + "/" + streamingPath;
            var fsInfo = new FileStream(strPath, FileMode.Open, FileAccess.Read);
            var srInfo = new StreamReader(fsInfo, Encoding.GetEncoding("UTF-8"));
            srInfo.BaseStream.Seek(0, SeekOrigin.Begin);
            var strResult = srInfo.ReadToEnd();

            //TODO: Newtonsoft.Json is not supported in Unity 2022.3.0f1, need to change to another json parser
            // var jObject = (JObject)JsonConvert.DeserializeObject(strResult);
            // foreach (var i in jObject) {
            //     string[] strArray = { jObject[i.Key]["eng"].ToString(), jObject[i.Key]["jap"].ToString() };
            //     m_LocalizationDictionary.Add(i.Key, strArray);
            // }
        }
    }
}