using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 加载单个主包，递归加载依赖项
 */
namespace AssetBundleFramework
{
    public class MultiABManager
    {
        //记录缓存，防止重复加载
        //每个场景对应一个MultiABManager，不会存在同一个资源在两个不同的MultiABManager里被缓存
        private Dictionary<string, SingleAssetBundleLoader> dicLoadCache;
        private string curABName;
        private ABLoadComplete loadAllCompleteHandle;

        public MultiABManager(string abName,ABLoadComplete loadAllCompleteHandle)
        {
            this.curABName = abName;
            this.loadAllCompleteHandle = loadAllCompleteHandle;
            this.dicLoadCache = new Dictionary<string, SingleAssetBundleLoader>();
        }

        public IEnumerator LoadAssetBundle(string abName)
        {
            Debug.Log("Start load:" + abName);
            //已经加载过直接返回, 执行回调
            if (dicLoadCache.ContainsKey(abName))
            {
                CompleteLoadAB(abName);
                //yield return dicLoadCache[abName].LoadAssetBundle();
            }
            else
            {
                SingleAssetBundleLoader loader = new SingleAssetBundleLoader(abName, CompleteLoadAB);
                dicLoadCache.Add(abName, loader);
                //获取依赖关系
                string[] strDependeceArray = AssetBundleManifestLoader.GetInstance().GetDependce(abName);
                foreach (string item_depend in strDependeceArray)
                {
                    loader.Relation.AddDependence(item_depend);
                    //加载依赖项
                    yield return LoadDepend(item_depend, loader);
                }
                
                yield return loader.LoadAssetBundle();
            }
        }

        /// <summary>
        /// 加载依赖包，并添加到引用
        /// </summary>
        /// <param name="loadName">要加载的包</param>
        /// <param name="refName">引用他的包</param>
        /// <returns></returns>
        public IEnumerator LoadDepend(string loadName, SingleAssetBundleLoader refABLoader)
        {
            refABLoader.Relation.AddReference(loadName);
            yield return LoadAssetBundle(loadName);
        }

        private void CompleteLoadAB(string abName)
        {
            Debug.Log("Load AssetBundle complete:" + abName);
            if(curABName.Equals(abName))
            {
                if (loadAllCompleteHandle != null)
                    loadAllCompleteHandle(abName);
            }
        }

        /// <summary>
        /// 加载包中的资源
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="assetName"></param>
        /// <param name="isCache"></param>
        /// <returns></returns>
        public Object LoadAsset(string abName,string assetName,bool isCache)
        {
            if(dicLoadCache.ContainsKey(abName))
            {
                return dicLoadCache[abName].LoadAsset(assetName, isCache);
            }
            Debug.LogError(GetType() + "/Load asset error");
            return null;
        }

        /// <summary>
        /// 释放所有资源
        /// </summary>
        public void DisposeAllAsset()
        {
            foreach (SingleAssetBundleLoader item_loader in dicLoadCache.Values)
            {
                item_loader.DisposeAll();
            }
            dicLoadCache.Clear();
            dicLoadCache = null;

            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
    }
}