using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    //用于创建对象的委托方法
    public delegate T OnCreateObject<T>();
    //用于放入对象池的初始化委托方法
    public delegate void OnPutObject<T>(T obj);
    //对象池
    public class ObjectPool<T> where T:class
    {
        public OnCreateObject<T> onCreateFun;//用于创建对象的方法
        private OnPutObject<T> onPutFun;//用于保存放入对象池的初始化委托方法
        private Queue<T> Pool;//对象池
        public ObjectPool(OnCreateObject<T> createFun = null, OnPutObject<T> putFun=null)
        {
            Pool = new Queue<T>();
            onCreateFun = createFun;//保存用于创建对象的方法
            onPutFun = putFun;//保存放入对象池的初始化方法
        }
        //放入对象池
        public void Put(T item)
        {
            if (onPutFun!=null)
            {
                onPutFun(item);//对象的初始化方法
            }
            Pool.Enqueue(item);//加入对象池
        }
        //从对象池中获取一个对象
        public T Get()
        {
            if (Pool.Count > 0)
            {
                return Pool.Dequeue();//出队
            }
            if (onCreateFun != null)
            {
                return onCreateFun();//创建一个对象
            }
            return default(T);//返回T类型的默认值
        }

    }
}
