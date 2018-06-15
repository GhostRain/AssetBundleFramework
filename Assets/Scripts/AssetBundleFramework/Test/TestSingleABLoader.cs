using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFramework
{
    public class TestSingleABLoader : MonoBehaviour
    {
        private SingleAssetBundleLoader loader;
        private SingleAssetBundleLoader loader1;
        public string abName;
        public string assetName;
        void Start()
        {
            loader = new SingleAssetBundleLoader(abName, OnLoadComplete);
            StartCoroutine(loader.LoadAssetBundle());
        }

        private void OnLoadComplete(string abName)
        {
            loader.DisposeAll();
            loader1 = new SingleAssetBundleLoader(abName, OnLoadComplete1);
            StartCoroutine(loader1.LoadAssetBundle());
        }

        private void OnLoadComplete1(string abName)
        {
            UnityEngine.Object obj = loader.LoadAsset(assetName);
            Instantiate(obj);
        }
        
    }
}