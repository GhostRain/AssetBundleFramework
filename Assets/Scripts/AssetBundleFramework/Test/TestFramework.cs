using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFramework
{
    public class TestFramework : MonoBehaviour
    {
        //场景名称
        private string _ScenesName = "scenes_1";
        //AB包名称
        private string _AssetBundelName = "scenes_1/prefabs";
        //资源名称
        private string _AssetName = "Cube.prefab";

        void Start()
        {
            StartCoroutine(AssetBundlerManager.GetInstance().LoadAssetBundlePack(_ScenesName, _AssetBundelName, LoadAllABComplete));
        }

        private void LoadAllABComplete(string abName)
        {
            UnityEngine.Object tmpObj = null;

            //提取资源
            tmpObj = AssetBundlerManager.GetInstance().LoadAsset(_ScenesName, _AssetBundelName, _AssetName, false);
            if (tmpObj != null)
            {
                Instantiate(tmpObj);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log(GetType() + " 测试销毁资源");
                AssetBundlerManager.GetInstance().DisposeScene(_ScenesName);
            }
        }
    }
}