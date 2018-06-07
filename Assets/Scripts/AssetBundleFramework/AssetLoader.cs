using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoader : System.IDisposable
{
    private AssetBundle curAssetBundle;
    private Hashtable assetHash;

    public AssetLoader(AssetBundle assetBundle)
    {
        if (assetBundle == null)
        {
            Debug.LogError("AssetBundle is null!");
        }
        else
        {
            curAssetBundle = assetBundle;
        }
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assetName"></param>
    /// <param name="isCache"></param>
    /// <returns></returns>
    public T LoadAsset<T>(string assetName, bool isCache = false) where T : UnityEngine.Object
    {
        if(assetHash.Contains(assetName))
        {
            return assetHash[assetName] as T;
        }
        T obj = curAssetBundle.LoadAsset<T>(assetName);
        if (obj == null)
            Debug.LogError("LoadAsset error....AssetBundle:" + curAssetBundle.name + "  assetName:" + assetName);
        if (obj != null && isCache)
        {
            assetHash.Add(assetName, obj);
        }
        return obj;
    }

    /// <summary>
    /// 卸载指定资源
    /// </summary>
    /// <param name="asset"></param>
    /// <returns></returns>
    public bool UnloadAsset(UnityEngine.Object asset)
    {
        if(asset != null)
        {
            Resources.UnloadAsset(asset);
            return true;
        }
        return false;
    }

    public void Dispose()
    {
        curAssetBundle.Unload(false);
    }

    public void DisposeAll()
    {
        curAssetBundle.Unload(true);
    }

    public string[] GetAllAssetName()
    {
        return curAssetBundle.GetAllAssetNames();
    }
}
