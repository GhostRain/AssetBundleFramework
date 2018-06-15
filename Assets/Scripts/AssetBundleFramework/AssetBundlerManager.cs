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
        //当前加载的主ab包
        private SingleAssetBundleLoader curABLoader;
        //缓存所有加载过的ab包(防止重复加载)
        private Dictionary<string, SingleAssetBundleLoader> dicABLoaderCache = new Dictionary<string, SingleAssetBundleLoader>();
        private string curABName;
        private string curScene;
        //已经加载过的场景
        private List<string> loadCompleteScene = new List<string>();
        //加载完场景的回调字典
        private Dictionary<string, SceneLoadVo> loadCallBackList = new Dictionary<string, SceneLoadVo>();
        //场景内资源加载队列
        private List<string> loadPathList = new List<string>();

        public static AssetBundlerManager GetInstance()
        {
            if (instance == null)
            {
                instance = new GameObject("AssetBundleMgr").AddComponent<AssetBundlerManager>();
            }
            return instance;
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="loadSceneCompleteHandle"></param>
        public IEnumerator Load(string sceneName, SceneLoadComplete loadSceneCompleteHandle,object param = null)
        {
            this.curScene = sceneName;
            //已经加载过直接执行回调
            if (loadCompleteScene.Contains(sceneName))
            {
                loadSceneCompleteHandle(param);
                yield return null;
            }
            SceneLoadVo loadComVo = new SceneLoadVo();
            loadComVo.callback = loadSceneCompleteHandle;
            loadComVo.param = param;
            loadCallBackList[sceneName] = loadComVo;

            //把这个场景的资源添加到加载队列(先不考虑优先级)
            string path = sceneName + "/";
            loadPathList.Add(path + sceneName);
            foreach (string subName in AssetBundleDefine.AB_SUB_NAME)
            {
                loadPathList.Add(path + subName);
            }
            yield return StartLoad();
        }

        /// <summary>
        /// 开始加载
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartLoad()
        {
            //队列全部加载完
            if(loadPathList.Count == 0)
            {
                //标记已加载
                loadCompleteScene.Add(this.curScene);
                //回调
                SceneLoadVo loadComVo = loadCallBackList[curScene];
                loadComVo.callback(loadComVo.param);
                //清空回调，也可以不请，不影响
                loadCallBackList[curScene] = null;
                yield return null;
            }
            string str = loadPathList[0];
            loadPathList.RemoveAt(1);
            yield return LoadOne(str);
        }

        /// <summary>
        /// 从队列中加载单个包
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadOne(string abName)
        {
            curABName = abName;
            string[] dependce = AssetBundleManifestLoader.GetInstance().GetDependce(abName);
            //获取依赖项，如果有，则先加载
            if(dependce.Length > 0)
            {
                foreach (string itemStr in dependce)
                {
                    yield return LoadOne(itemStr);
                }
            }
            else
            {
                curABLoader = new SingleAssetBundleLoader(curABName, CompleteLoadAB);
                dicABLoaderCache.Add(curABName, curABLoader);
                yield return curABLoader.LoadAssetBundle();
            }
        }

        /// <summary>
        /// 单个主包加载完成回调
        /// </summary>
        /// <param name="abName"></param>
        private void CompleteLoadAB(string abName)
        {
            if(curABName.Equals(abName))
            {
                StartLoad();
            }
        }
    }
}