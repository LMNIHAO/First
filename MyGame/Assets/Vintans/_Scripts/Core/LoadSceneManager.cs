using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//加载场景进度接口
public interface ILoadSceneProgress
{
    void OnProgress(float progress);
}
//加载场景管理器
public class LoadSceneManager : MonoBehaviour
{
    static LoadSceneManager mInstance;//保存唯一的实例
    static AsyncOperation asyncOperation;//异步对象
    static ILoadSceneProgress progressListener;//监听异步加载场景进度的接口对象
    //静态构造函数
    static LoadSceneManager()
    {
        //第一次访问该类的任何成员之前调用,有且只执行一次.
        GameObject obj = new GameObject("LoadSceneManager");
        mInstance = obj.AddComponent<LoadSceneManager>();//创建唯一的实例
        GameObject.DontDestroyOnLoad(obj);//标记该物体obj加载场景时不会被销毁.
    }
    //加载场景
    public static void Load(string sceneName, ILoadSceneProgress listener=null) //UILoading
    {
        progressListener = listener;//保存传入的接口对象
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);//开始异步加载场景
    }
    // Update is called once per frame
    void Update ()
    {
        if (asyncOperation!=null)
        {
            if (progressListener!=null)
            {
                progressListener.OnProgress(asyncOperation.progress);//通知异步对象的进度.
            }
            if (asyncOperation.isDone)
            {
                asyncOperation = null;//加载完成将异步对象变量置为空.
            }
        }
       
	}
}
