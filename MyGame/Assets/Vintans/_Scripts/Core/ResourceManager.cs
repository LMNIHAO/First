using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//资源加载任务状态;
public enum ResourceLoadState
{
    Waitting = 0,//等待加载;
    Loading,//加载中;
    Finished,//加载完成;
};
//资源加载任务（基类）;
abstract public class ResourceLoadTask
{
    public object pCallBackParam { get; set; }
    public string pAssetFileName { get; set; }
    public Type pAssetObjectType { get; set; }
    public string pAssetObjectName { get; set; }
    public ResourceLoadCallback pCallBack { get; set; }
    public UnityEngine.Object pLoadObject { get; set; }
    public string pError { get; set; }
    public ResourceLoadState pLoadState { get; set; }
    public AssetBundle pAssetBundle { get; set; }
    public void Callback()
    {
        if (pCallBack != null)
        {
            pCallBack(pLoadObject, pCallBackParam);
        }
    }

    public ResourceLoadTask(string AssetFileName, string AssetObjectName,
                             Type AssetObjectType, ResourceLoadCallback callback,
                             object varParam
    )
    {

        pCallBackParam = varParam;
        pAssetFileName = AssetFileName;
        pAssetObjectName = AssetObjectName;
        pAssetObjectType = AssetObjectType;
        pCallBack = callback;
        pLoadObject = null;
    }


    public void Reset(UnityEngine.Object varObj, string AssetObjectName, Type AssetObjectType)
    {
        pAssetObjectName = AssetObjectName;
        pAssetObjectType = AssetObjectType;
        pLoadObject = varObj;
    }

    abstract public IEnumerator Load();

    public void Unload(bool varUnloadAll)
    {
        if (pAssetBundle != null)
        {
            pAssetBundle.Unload(varUnloadAll);
        }
    }
}
//Resources加载任务;
public class EditorResourceLoadTask : ResourceLoadTask
{
    public EditorResourceLoadTask(string AssetFileName, string AssetObjectName,
                                   Type AssetObjectType, ResourceLoadCallback callback, object varParam)
        : base(AssetFileName, AssetObjectName, AssetObjectType, callback, varParam)
    {
    }
    /// <summary>
    /// Resources加载任务实现;
    /// </summary>
    /// <returns></returns>
    override public IEnumerator Load()
    {
        pLoadState = ResourceLoadState.Loading;

        if (string.IsNullOrEmpty(pAssetFileName))
        {
            pLoadState = ResourceLoadState.Finished;
            yield break;
        }
        if (pAssetObjectType == typeof(TextAsset))
        {
            pLoadObject = Resources.Load(pAssetFileName+"/"+pAssetObjectName);
        }
        else
        {
            ResourceRequest resquest = Resources.LoadAsync(pAssetFileName + "/" + pAssetObjectName, pAssetObjectType);
            if (resquest != null)
            {
                yield return resquest;
                try
                {
                    pLoadObject = resquest.asset;
                }
                catch (NullReferenceException e)
                {
                   Debug.LogError(e.Message);
                }
            }
        }
        pLoadState = ResourceLoadState.Finished;
    }
}
//AssetBundle打包资源加载任务;
public class AssetBundleLoadTask : ResourceLoadTask
{
    private List<ResourceLoadTask> mDependList;  

    public AssetBundleLoadTask(string AssetFileName, string AssetObjectName,
                              Type AssetObjectType, ResourceLoadCallback callback,
                               object varParam)
        : base(AssetFileName, AssetObjectName, AssetObjectType, callback, varParam)
    {

    }
    //获取依赖文件任务;
    private void GetDependsTask(string varAssetFileName)
    {
        string[] tempDepends = ResourceManager.GetManager().GetAssetBundleDepends(varAssetFileName);
        if (tempDepends != null && tempDepends.Length > 0)
        {
            if (mDependList == null)
            {
                mDependList = new List<ResourceLoadTask>();
            }

            for (int i = 0; i < tempDepends.Length; i++)
            {
                string tempDependRes = tempDepends[i];
                tempDependRes = tempDependRes.ToLower();

                //GetDependsTask(tempDependRes);

                ResourceLoadTask tempTask = ResourceManager.GetManager().GetResourceLoadTask(tempDependRes);
                if (tempTask == null)
                {
                    tempTask = new AssetBundleLoadTask(tempDependRes, null, null, null, null);
                    mDependList.Add(tempTask);
                }
                
            }
        }
    }

   /// <summary>
   /// AssetBundle资源任务的异步加载实现;
   /// </summary>
   /// <returns></returns>
    override public IEnumerator Load()
    {
        pLoadState = ResourceLoadState.Loading;
        ResourceLoadTask tempTask = ResourceManager.GetManager().GetResourceLoadTask(pAssetFileName);
        //如果任务已经加载过,则直接获取资源并完成;
        if (tempTask != null)
        {
            pAssetBundle = tempTask.pAssetBundle;
            pLoadObject = tempTask.pLoadObject;
            if (pLoadObject == null && pAssetBundle != null&&!string.IsNullOrEmpty(pAssetObjectName))
            {
                pLoadObject = pAssetBundle.LoadAsset(pAssetObjectName);
            }
            pLoadState = ResourceLoadState.Finished;
            yield break;
        }
      
       //否则，进行资源的加载;

        //获取资源的依赖文件;
        GetDependsTask(pAssetFileName);
        ResourceLoadTask temTask = null;
        if (mDependList != null )
        {
            //如果有依赖文件则先加载依赖文件;
            while (mDependList.Count > 0)
            {
                if (temTask != null)
                {
                    if (!string.IsNullOrEmpty(temTask.pAssetFileName))
                    {
                        yield return temTask.Load();

                        if (string.IsNullOrEmpty(temTask.pError) == false)
                        {
                            Debug.Log(temTask.pError);
                        }

                        ResourceManager.GetManager().AddLoadedTask(temTask);
                    }
                    ///无论加载成功或失败都要回调
                    temTask.Callback();
                    temTask = null;
                    mDependList.RemoveAt(0);
                }
                if (mDependList.Count > 0)
                {
                    temTask = mDependList[0];
                }

            }
        }
        //通过WWW加载资源;
        string path = Application.dataPath + "/AssetBundles/"+pAssetFileName;
        WWW www = new WWW(path);
        yield return www;
        if (www.isDone)
        {
            pAssetBundle = www.assetBundle;
            if(pAssetBundle!=null)
            {
                if(!string.IsNullOrEmpty(pAssetObjectName)&&pAssetObjectType!=null)
                    pLoadObject = pAssetBundle.LoadAsset(pAssetObjectName, pAssetObjectType);
            }
               
            pLoadState = ResourceLoadState.Finished;
        }
       


        yield return null;
    }
}
/// <summary>
/// 资源加载回调方法的委托类型.
/// </summary>
/// <param name="obj">"加载的资源文件"</param>
/// <param name="callbackParam">“用户传入的参数”</param>
public delegate void ResourceLoadCallback(UnityEngine.Object obj,object callbackParam);
//加载方式;
public enum ResourcesModel
{
    RM_Resources,
    RM_AssetBundle,
}
//资源加载管理；
public class ResourceManager : MonoBehaviour 
{

    ResourcesModel mResourcesModel =ResourcesModel.RM_Resources;

    /// <summary>
    ///资源依赖, 
    /// </summary>
    private Dictionary<string, string[]> mDenpends=new Dictionary<string, string[]>();
    /// <summary>
    ///所有的文件加载任务队列，按先进先出的顺序加载。 
    /// </summary>
    private Queue<ResourceLoadTask> mLoadtasks=new Queue<ResourceLoadTask>();
    /// <summary>
    /// 所有加载对象对应的任务.
    /// </summary>
    private Dictionary<string, ResourceLoadTask> mLoadedBundles=new Dictionary<string, ResourceLoadTask>();
    /// <summary>
    /// 资源加载唯一实例;
    /// </summary>
    private static ResourceManager mManager;
    public static ResourceManager GetManager()
	{
		if (mManager == null) 
		{
			GameObject obj = new GameObject ("ResourceManager");
			mManager = obj.AddComponent<ResourceManager> ();
            mManager.Init();
			DontDestroyOnLoad (obj);
		}
		return mManager;
	}
    /// <summary>
    /// 资源管理器初始化;
    /// </summary>
    public void Init()
    {
        if (mResourcesModel==ResourcesModel.RM_AssetBundle)
        {
            InitDepends();
        }
        StartCoroutine(InitLoad());
    }
    /// <summary>
    /// 每帧更新，所有的load完成后的回调都在这里完成
    /// </summary>
    IEnumerator InitLoad()
    {
        ResourceLoadTask temTask=null;
        while (true)
        {
            if (temTask != null)
            {
                if (string.IsNullOrEmpty(temTask.pAssetFileName))
                {
                    temTask.Callback();
                }
                else
                {
                    yield return StartCoroutine(temTask.Load());

                    if (string.IsNullOrEmpty(temTask.pError) == false)
                    {
                        Debug.Log(temTask.pError);
                    }

                    AddLoadedTask(temTask);//缓存完成的任务;

                    ///无论加载成功或失败都要回调
                    temTask.Callback();
                }
             
                temTask = null;
            }
            if (mLoadtasks.Count > 0)
            {
                temTask = mLoadtasks.Dequeue();
            }
            yield return null;
        }
    }

    /// <summary>
	///初始化所有的文件依赖系 
	/// </summary>
	private void InitDepends ()
	{
		// 读取文件依赖回调;
		ResourceLoadCallback loadInitDependsCallBack = delegate(UnityEngine.Object obj,object varParam) 
        {
			AssetBundleManifest tempABm = obj as AssetBundleManifest;
			if(tempABm==null)
			{
				Debug.LogError("ResourceManager InitDepend Failed.");
				return ;
			}
			string[] tempDenpends = tempABm.GetAllAssetBundles();

			if(tempDenpends == null)
			{
				return;
			}

			for(int i = 0; i < tempDenpends.Length; i++)
			{
				string tempRes = tempDenpends[i];

				string[] tempResDepend = tempABm.GetDirectDependencies(tempRes);
			
				if (mDenpends.ContainsKey (tempRes))
				{
					mDenpends [tempRes] = tempResDepend;
				}
                else 
				{
					mDenpends.Add (tempRes, tempResDepend);
				}
			}
		};

		string tempFilePath = "AssetBundles/AssetBundleManifest";
		LoadAsset (tempFilePath, typeof(AssetBundleManifest), loadInitDependsCallBack,null);
    }

    /// <summary>
    /// 加载资源的方法;
    /// </summary>
    /// <param name="AssetFileName"></param>
    /// <param name="AssetObjectName"></param>
    /// <param name="AssetObjectType"></param>
    /// <param name="callback"></param>
    /// <param name="varParam"></param>
    /// <returns></returns>
    public bool LoadAsset(string AssetFileName, Type AssetObjectType,
                                     ResourceLoadCallback callback=null, object varParam=null)
    {
        int index = AssetFileName.LastIndexOf('/');
        string bundleName = AssetFileName.Substring(0,index);//文件的打包资源包名;
        string fileName = AssetFileName.Substring(index+1);//资源文件名;
        ResourceLoadTask tempTask = null;
        if (mResourcesModel == ResourcesModel.RM_Resources)
        {
            tempTask = new EditorResourceLoadTask(bundleName, fileName, AssetObjectType, callback, varParam);
        }
        else
        {
            AssetFileName = AssetFileName.ToLower();
            tempTask = new AssetBundleLoadTask(bundleName, fileName, AssetObjectType, callback, varParam);
        }
        mLoadtasks.Enqueue(tempTask);
        return true;
    }

    /// <summary>
    /// 获取指定的文件的依赖关系
    /// </summary>
    /// <returns>The asset bundle depends.如果没找到文件，或者没有依赖返回空</returns>
    /// <param name="AssetBundleFileName">Asset bundle file name.</param>
    public string[] GetAssetBundleDepends(string AssetBundleFileName)
    {
        if (mDenpends == null || mDenpends.Count == 0)
        {
            return null;
        }
        string[] tempResult = null;
        if (mDenpends.TryGetValue(AssetBundleFileName, out tempResult) == false)
        {
            return null;
        }
        return tempResult;
    }
    /// <summary>
    /// 获取一个已完成的任务;
    /// </summary>
    /// <param name="AssetFileName"></param>
    /// <returns></returns>
    public ResourceLoadTask GetResourceLoadTask(string AssetFileName)
    {
        if (string.IsNullOrEmpty(AssetFileName))
        {
            return null;
        }
        ResourceLoadTask tempTask = null;
        mLoadedBundles.TryGetValue(AssetFileName, out tempTask);
        return tempTask;
    }
    /// <summary>
    /// 添加一个已完成的任务;
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    public bool AddLoadedTask(ResourceLoadTask task)
    {
        if (task == null)
        {
            return false;
        }
        if (!string.IsNullOrEmpty(task.pAssetFileName))
        {
            if (string.IsNullOrEmpty(task.pError) && mLoadedBundles.ContainsKey(task.pAssetFileName) == false)
            {
                mLoadedBundles.Add(task.pAssetFileName, task);
                return true;
            }
        }
        return false;
    }
    
}
