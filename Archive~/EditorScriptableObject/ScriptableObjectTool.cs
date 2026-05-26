using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectTool
{
    public static void CreatScriptableObject<T>(string mainDirectory,string dataName) where T : ScriptableObject
    {
        //创建数据资源文件
        //泛型是继承自ScriptableObject的类
        T asset = ScriptableObject.CreateInstance<T>();
        //前一步创建的资源只是存在内存中，现在要把它保存到本地
        //通过编辑器API，创建一个数据资源文件，第二个参数为资源文件在Assets目录下的路径
        if (!AssetDatabase.IsValidFolder($"Assets/ScriptableObject/{mainDirectory}"))
        {
            AssetDatabase.CreateFolder($"Assets/ScriptableObject", $"{mainDirectory}");
        }

        string path =
            AssetDatabase.GenerateUniqueAssetPath($"Assets/ScriptableObject/{mainDirectory}/{dataName}.asset");
        AssetDatabase.CreateAsset(asset, path);
        //保存创建的资源
        AssetDatabase.SaveAssets();
        //刷新界面
        AssetDatabase.Refresh();
    }
}
