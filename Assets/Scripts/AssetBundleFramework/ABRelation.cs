using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFramework
{
    public class ABRelation
    {
        //当前AsseetBundel 名称
        private string abName;
        //本包所有的依赖包集合
        private List<string> listAllDependence;
        //本包所有的引用包集合
        private List<string> listAllReference;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="abName"></param>
        public ABRelation(string abName)
        {
            if (!string.IsNullOrEmpty(abName))
            {
                this.abName = abName;
            }
            listAllDependence = new List<string>();
            listAllReference = new List<string>();
        }

        /* 依赖关系 */
        /// <summary>
        /// 增加依赖关系
        /// </summary>
        /// <param name="abName">AssetBundle 包名称</param>
        public void AddDependence(string abName)
        {
            if (!listAllDependence.Contains(abName))
            {
                listAllDependence.Add(abName);
            }
        }

        /// <summary>
        /// 移除依赖关系
        /// </summary>
        /// <param name="abName">移除的包名称</param>
        /// <returns>
        /// true；　此AssetBundel 没有依赖项
        /// false;  此AssetBundel 还有其他的依赖项
        /// 
        /// </returns>
        public bool RemoveDependece(string abName)
        {
            if (listAllDependence.Contains(abName))
            {
                listAllDependence.Remove(abName);
            }
            if (listAllDependence.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //获取所有依赖关系
        public List<string> GetAllDependence()
        {
            return listAllDependence;
        }



        /* 引用关系 */
        /// <summary>
        /// 引用依赖关系
        /// </summary>
        /// <param name="abName">AssetBundle 包名称</param>
        public void AddReference(string abName)
        {
            if (!listAllReference.Contains(abName))
            {
                listAllReference.Add(abName);
            }
        }

        /// <summary>
        /// 移除引用关系
        /// </summary>
        /// <param name="abName">移除的包名称</param>
        /// <returns>
        /// true；　此AssetBundel 没有引用项
        /// false;  此AssetBundel 还有其他的引用项
        /// 
        /// </returns>
        public bool RemoveReference(string abName)
        {
            if (listAllReference.Contains(abName))
            {
                listAllReference.Remove(abName);
            }
            if (listAllReference.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //获取所有引用关系
        public List<string> GetAllReference()
        {
            return listAllReference;
        }
    }
}