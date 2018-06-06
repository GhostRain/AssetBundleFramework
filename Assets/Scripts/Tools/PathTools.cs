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
        public static string getABOutPath()
        {
            return getPlatformPath() + "/" + getPlatformName();
        }

        private static string getPlatformPath()
        {
            string platformPath = string.Empty;
            switch (Application.platform)
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

        private static string getPlatformName()
        {
            string platformName = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    platformName = "windows";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    platformName = "ios";
                    break;
                case RuntimePlatform.Android:
                    platformName = "android";
                    break;
                default:
                    break;
            }
            return platformName;
        }
    }
}