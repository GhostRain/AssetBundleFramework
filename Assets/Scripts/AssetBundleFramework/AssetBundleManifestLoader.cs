using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace AssetBundleFramework
{
    public class AssetBundleManifestLoader : System.IDisposable
    {
        private static AssetBundleManifestLoader instance;
        private AssetBundleManifest bundleManifest;
        private string url;
        private AssetBundle assetBundle;
        private ABLoadComplete loadCompleteHandler;
        public bool IsLoadFinish { get; private set; }

        public static AssetBundleManifestLoader GetInstance()
        {
            if (instance == null)
            {
                instance = new AssetBundleManifestLoader();
            }
            return instance;
        }

        public AssetBundleManifestLoader()
        {
            IsLoadFinish = false;
            url = PathTools.GetUrlPath() + "/" + PathTools.GetABPathName();
        }

        /// <summary>
        /// 加载manifest
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadMainifestFile()
        {
            using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(this.url))
            {
                yield return request.SendWebRequest();
                assetBundle = DownloadHandlerAssetBundle.GetContent(request);
                if (assetBundle != null)
                {
                    IsLoadFinish = true;
                    if (loadCompleteHandler != null)
                        loadCompleteHandler(AssetBundleDefine.ASSETBUNDLE_MANIFEST);
                }
                else
                {
                    Debug.Log("load assetbundle manifest Error:" + request.error);
                }
            }
        }

        /// <summary>
        /// 获取AssetBundleManifest实例
        /// </summary>
        /// <returns></returns>
        public AssetBundleManifest GetMainfest()
        {
            if(IsLoadFinish)
            {
                return bundleManifest;
            }
            return null;
        }

        /// <summary>
        /// 获取指定ab包的依赖项
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public string[] GetDependce(string abName)
        {
            if(bundleManifest != null)
            {
                return bundleManifest.GetAllDependencies(abName);
            }
            return null;
        }

        public void Dispose()
        {
            if (assetBundle != null)
            {
                assetBundle.Unload(true);
            }
        }
    }
}