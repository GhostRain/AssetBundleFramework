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
            return getPlatformPath(platform);
        }

        private static string getPlatformPath(RuntimePlatform platform)
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
    }
}