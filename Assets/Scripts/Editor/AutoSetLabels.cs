using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace AssetBundleFramework
{
    public class AutoSetLabels
    {
        [MenuItem("AssetBundleTools/Set AssetBundle Label")]
        public static void setAssetBundleLabel()
        {
            //打包资源根目录
            string abRootPath = string.Empty;
            //根目录下的目录信息
            DirectoryInfo[] abDirector;

            AssetDatabase.RemoveUnusedAssetBundleNames();
            //clearAssetBundlesName();
            //获取打包资源根目录
            abRootPath = PathTools.getABResourcesPath();
            DirectoryInfo abRootDirector = new DirectoryInfo(abRootPath);
            abDirector = abRootDirector.GetDirectories();

            //遍历每一个场景目录
            foreach (DirectoryInfo scenesDir in abDirector)
            {
                string assetName = scenesDir.Name;
                findFile(scenesDir, assetName);
            }
            AssetDatabase.Refresh();
            Debug.LogWarning("设置成功");
        }

        /// <summary>
        /// 清除所有标记
        /// </summary>
        public static void clearAssetBundlesName()
        {
            //获取所有的AssetBundle名称  
            string[] abNames = AssetDatabase.GetAllAssetBundleNames();

            //强制删除所有AssetBundle名称  
            for (int i = 0; i < abNames.Length; i++)
            {
                AssetDatabase.RemoveAssetBundleName(abNames[i], true);
            }
        }

        private static void findFile(DirectoryInfo scenesDir, string assetName)
        {
            //所有文件信息
            FileInfo[] fileArr = scenesDir.GetFiles();
            foreach (FileInfo fileInfo in fileArr)
            {
                setFileABLabel(fileInfo, assetName);
            }
            //所有文件夹信息
            DirectoryInfo[] dirArr = scenesDir.GetDirectories();
            foreach (DirectoryInfo dir in dirArr)
            {
                findFile(dir, assetName);
            }
        }

        /// <summary>
        /// 设置ab标记
        /// </summary>
        /// <param name="fileInfo">要处理的文件</param>
        /// <param name="assetName">这个文件所在的场景名</param>
        private static void setFileABLabel(FileInfo fileInfo, string assetName)
        {
            //忽视unity自身生成的meta文件
            if (fileInfo.Extension == ".meta")
                return;
            int index = fileInfo.FullName.IndexOf("Assets");
            //截取Assets之后的路径
            //AssetImporter.GetAtPath必须是unity工程的相对路径
            //所以要Assets开头
            string filePath = fileInfo.FullName.Substring(index);
            //通过AssetImporter指定要标记的文件
            AssetImporter importer = AssetImporter.GetAtPath(filePath);
            //区分场景文件和资源文件后缀名
            if (fileInfo.Extension == ".unity")
                importer.assetBundleVariant = "u3d";
            else
                importer.assetBundleVariant = "assetbundle";
            //包名称
            string bundleName = string.Empty;
            //需要拿到场景目录下面一级目录名称
            //包名=场景目录名+下一级目录名
            int indexScenes = fileInfo.FullName.IndexOf(assetName) + assetName.Length + 1;
            string bundlePath = fileInfo.FullName.Substring(indexScenes);
            //替换win路径里的反斜杠
            bundlePath = bundlePath.Replace(@"\", "/");
            Debug.Log(bundlePath);
            if (bundlePath.Contains("/"))
            {
                string[] strArr = bundlePath.Split('/');
                bundleName = assetName + "/" + strArr[0];
            }
            else
            {
                bundleName = assetName + "/" + assetName;
            }
            importer.assetBundleName = bundleName;
        }

        [MenuItem("AssetBundleTools/BuildAllAssetBundles")]
        public static void buildAllAssetBundle()
        {
            string outPath = string.Empty;
            outPath = PathTools.getABOutPath();
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }
            BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
            AssetDatabase.Refresh();
        }

        [MenuItem("AssetBundleTools/DeleteAllAssetBundles")]
        public static void DeleteAllAssetBundles()
        {
            string outPath = string.Empty;
            outPath = PathTools.getABOutPath();
            if(!string.IsNullOrEmpty(outPath))
            {
                Directory.Delete(outPath, true);
                File.Delete(outPath + ".meta");
            }
            AssetDatabase.Refresh();
        }
    }
}