using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFramework
{
    //AssetBundle下载完成回调
    public delegate void ABLoadComplete(string abName);
    //场景全部资源加载完的回调
    public delegate void SceneLoadComplete(object param);

    public class AssetBundleDefine
    {
        public static string ASSETBUNDLE_MANIFEST = "AssetBundleManifest";
        public static string[] AB_SUB_NAME = { "Audio", "Effect", "Materials", "Models", "Prefabs", "Texture"};
    }
}