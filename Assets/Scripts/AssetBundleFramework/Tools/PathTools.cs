using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFramework
{
    public class PathTools
    {
        //AssetBundle资源路径
        public const string AB_Resources = "/AB_Resources";

        public static string getABResourcesPath()
        {
            return Application.dataPath + AB_Resources;
        }

        /// <summary>
        /// AssetBundle输出路径
        /// </summary>
        /// <returns></returns>
        public static string getABOutPath(RuntimePlatform platform)
        {
            string platformPath = string.Empty;
            switch (platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    platformPath = Application.streamingAssetsPath;
                    break;
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                    platformPath = Application.persistentDataPath;
                    break;
                default:
                    break;
            }
            return platformPath;
        }

        public static string GetWWWPath()
        {
            string wwwPath = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    wwwPath = "file://" + getABOutPath(Application.platform);
                    break;
                case RuntimePlatform.IPhonePlayer:
                    wwwPath = getABOutPath(Application.platform);
                    break;
                case RuntimePlatform.Android:
                    wwwPath = "jar:file://" + getABOutPath(Application.platform);
                    break;
                default:
                    break;
            }
            return wwwPath;
        }
    }
}