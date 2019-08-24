using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    //特效对象池
    public static class EffectPool
    {
        static GameObject EffectRoot;
        //每一个特效路径对应一个特效物体的池子
        static Dictionary<string, ObjectPool<GameObject>> EffectPools=new Dictionary<string, ObjectPool<GameObject>>();
        static EffectPool()
        {
            EffectRoot = new GameObject("EffectRoot");
            GameObject.DontDestroyOnLoad(EffectRoot);
        }
        //获取某个路径的特效物体
        public static GameObject Get(string effectPath)//路径1
        {
            ObjectPool<GameObject> pool;
            if (!EffectPools.TryGetValue(effectPath, out pool))
            {
                //创建一个新的对象池
                pool = new ObjectPool<GameObject>(()=> 
                {
                    GameObject loadObj= Resources.Load<GameObject>(effectPath);
                    if (loadObj!=null)
                    {
                        loadObj = GameObject.Instantiate(loadObj, EffectRoot.transform);
                       //NGUITools.SetLayer(loadObj,LayerMask.NameToLayer("Default"));
                    }
                    return loadObj;
                },(go)=> 
                {
                    go.SetActive(false);
                });
                //将该对象池加入到字典
                EffectPools.Add(effectPath, pool);
            }
            GameObject effectObj = pool.Get();
            return effectObj;//从对象池中获取一个对象并返回
        }
        public static void Put(string effectPath,GameObject obj)
        {
            ObjectPool<GameObject> pool;
            if (EffectPools.TryGetValue(effectPath, out pool))
            {
                pool.Put(obj);//特效物体Obj放入该路径对应的池子中
            }
        }
    }
}

