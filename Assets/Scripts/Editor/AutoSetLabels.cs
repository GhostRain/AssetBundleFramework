using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AutoSetLabels {

    [MenuItem("AssetBundleTools/Set AssetBundle Label")]
    public static void setAssetBundleLabel()
    {
        //打包资源根目录
        string abRootPath = string.Empty;
        //根目录下的目录信息
        DirectoryInfo[] abDirector;

        //清除无用标记
        AssetDatabase.RemoveUnusedAssetBundleNames();
        //获取打包资源根目录
        abRootPath = Application.dataPath + "/AB_Resources";
        DirectoryInfo abRootDirector = new DirectoryInfo(abRootPath);
        abDirector = abRootDirector.GetDirectories();

        //遍历每一个场景目录
        foreach(DirectoryInfo scenesDir in abDirector)
        {
            string assetName = scenesDir.Name;
            findFile(scenesDir, assetName);
        }
    }

    public static void findFile(DirectoryInfo scenesDir, string assetName)
    {
        //所有文件信息
        FileInfo[] fileArr = scenesDir.GetFiles();
        foreach(FileInfo fileInfo in fileArr)
        {
            Debug.Log(fileInfo.FullName);
        }
        //所有文件夹信息
        DirectoryInfo[] dirArr = scenesDir.GetDirectories();
        foreach(DirectoryInfo dir in dirArr)
        {
            findFile(dir, assetName);
        }
    }
}
