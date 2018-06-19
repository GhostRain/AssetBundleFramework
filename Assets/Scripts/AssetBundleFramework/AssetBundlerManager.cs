using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 加载场景
 */
namespace AssetBundleFramework
{
    public class AssetBundlerManager: MonoBehaviour
    {
        private static AssetBundlerManager instance;
        private Dictionary<string, MultiABManager> dicAllScenes = new Dictionary<string, MultiABManager>();
        private AssetBundleManifest mainfest;

        public static AssetBundlerManager GetInstance()
        {
            if (instance == null)
            {
                instance = new GameObject("AssetBundleMgr").AddComponent<AssetBundlerManager>();
            }
            return instance;
        }

        void Awake()
        {
            //加载Manifest清单文件
            StartCoroutine(AssetBundleManifestLoader.GetInstance().LoadMainifestFile());
        }

        public IEnumerator LoadAssetBundlePack(string sceneName,string abName, ABLoadComplete loadComplete)
        {
            //循环等待manifest加载完成
            //TODO  需要优化(加载队列,回调)
            while (!AssetBundleManifestLoader.GetInstance().IsLoadFinish)
            {
                yield return null;
            }
            mainfest = AssetBundleManifestLoader.GetInstance().GetMainfest();

            if(!dicAllScenes.ContainsKey(sceneName))
            {
                MultiABManager multiTmpManager = new MultiABManager(abName, loadComplete);
                dicAllScenes.Add(sceneName, multiTmpManager);
            }
            MultiABManager multiManager = dicAllScenes[sceneName];
            yield return multiManager.LoadAssetBundle(abName);
        }

        public Object LoadAsset(string sceneName,string abName,string assetName,bool isCache)
        {
            if(dicAllScenes.ContainsKey(sceneName))
            {
                MultiABManager multiMgr = dicAllScenes[sceneName];
                return multiMgr.LoadAsset(abName, assetName, isCache);
            }
            return null;
        }

        public void DisposeScene(string sceneName)
        {
            if(dicAllScenes.ContainsKey(sceneName))
            {
                dicAllScenes[sceneName].DisposeAllAsset();
            }
        }
    }
}