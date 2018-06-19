using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace AssetBundleFramework
{
    public class SingleAssetBundleLoader : System.IDisposable
    {
        private AssetLoader assetLoader;
        private string abName;
        private string abPath;
        //引用计数
        private int refCount;

        //下载完成委托
        private ABLoadComplete loadCompleteHandler;

        public ABRelation Relation { get; private set; }

        public SingleAssetBundleLoader(string abName, ABLoadComplete loadCompleteHandler = null)
        {
            this.abName = abName;
            this.abPath = PathTools.GetUrlPath() + "/" + this.abName;
            this.loadCompleteHandler = loadCompleteHandler;
            this.Relation = new ABRelation(abName);
        }

        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadAssetBundle()
        {
            using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(this.abPath))
            {
                yield return request.SendWebRequest();
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
                if (bundle != null)
                {
                    assetLoader = new AssetLoader(bundle);
                    if (loadCompleteHandler != null)
                        loadCompleteHandler(this.abName);
                }
                else
                {
                    Debug.Log("load assetbundle Error:" + request.error);
                }
            }
        }

        /// <summary>
        /// 加载ab包里的资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="isCache"></param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string assetName, bool isCache = false)
        {
            if(assetLoader != null)
                return assetLoader.LoadAsset<UnityEngine.Object>(assetName, isCache);
            return null;
        }

        public void UnloadAsset(UnityEngine.Object asset)
        {
            if(assetLoader != null)
            {
                assetLoader.UnloadAsset(asset);
            }
        }

        public void Dispose()
        {
            if (assetLoader != null)
            {
                assetLoader.Dispose();
            }
        }

        public void DisposeAll()
        {
            if(assetLoader != null)
            {
                assetLoader.DisposeAll();
            }
        }

        public string[] GetAllAssetName()
        {
            if (assetLoader != null)
                return assetLoader.GetAllAssetName();
            return null;
        }

        public void Retain()
        {
            refCount++;
        }

        public void Release()
        {
            refCount--;
        }
    }
}