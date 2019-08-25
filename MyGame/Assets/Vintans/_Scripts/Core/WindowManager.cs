using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//窗口管理器;
public class WindowManager
{
    public delegate void OpenWindowCallBack(GameObject window);
    private Dictionary<string, GameObject> OpenWindows= new Dictionary<string, GameObject>();//打开的窗口.
    private Dictionary<string, GameObject> OpenDialogs = new Dictionary<string, GameObject>();//打开的窗口.
    private GameObject windowRoot;//所有窗口的根节点
    private GameObject dialogRoot;//所有的最上层的界面
    private List<UIPanel_VT> AllWindowPanels=new List<UIPanel_VT>();//保存所有打开的窗口的Panels
    private List<UIPanel_VT> AllDialogPanels = new List<UIPanel_VT>();
    private static WindowManager Instance;//保存唯一实例
    public float ScreenScale
    {
        get
        {
            return (float)Screen.height/720f;
        }
    }
    public static WindowManager GetManager()
    {
        if (Instance==null)
        {
            Instance = new WindowManager();
        }
        return Instance;
    }
    private WindowManager()
    {
        //创建UIRoot
        GameObject uiRootObj = new GameObject("UIRoot");
        Object.DontDestroyOnLoad(uiRootObj);
        Canvas can = uiRootObj.AddComponent<Canvas>();
        can.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler canScaler = uiRootObj.AddComponent<CanvasScaler>();
        canScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canScaler.referenceResolution = new Vector2(1920f,1080f);
        uiRootObj.AddComponent<GraphicRaycaster>();

        //创建 EventSystem
        GameObject eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<StandaloneInputModule>();
        eventSystem.transform.SetParent(uiRootObj.transform);

        //创建摄像机
        GameObject cameraObj = new GameObject("UICamera");
        Camera camera= cameraObj.AddComponent<Camera>();//添加摄像机组件
        camera.depth = 1;
        camera.clearFlags = CameraClearFlags.Depth;
        camera.orthographic = true;//设置正交摄像机2D
        camera.orthographicSize = 1;
        camera.nearClipPlane = -10f;
        camera.farClipPlane = 10f;
        cameraObj.transform.SetParent(uiRootObj.transform);

        //创建窗口存放的根节点.
        windowRoot = new GameObject("Windows");//创建一个窗口根节点,加载的窗口都放到该物体下.
        SetRoot(windowRoot.transform,uiRootObj.transform);//窗口根节点设置到UIRoot下面

        //创建弹框存放的根节点.
        dialogRoot = new GameObject("Dialogs");//创建一个弹框根节点,加载的窗口都放到该物体下.
        SetRoot(dialogRoot.transform,uiRootObj.transform);
    }
    //打开UI界面
    public void OpenWindow(string path, OpenWindowCallBack fun=null)
    {
        GameObject loadObj;
        //判断该路径的窗口没有打开
        if (OpenWindows.TryGetValue(path, out loadObj))
        {
            if (fun != null)
            {
                fun(loadObj);
            }
            return;
        }
        //加载UI
        LoadUI(path, (uiObj) => 
        {
            if (uiObj!=null)
            {
                SetRoot(uiObj.transform, windowRoot.transform);
                UIPanel_VT[] panels = uiObj.GetComponentsInChildren<UIPanel_VT>();
                
                RemoveWindowPanel(AllWindowPanels, panels);
                AddWindowPanel(AllWindowPanels, panels);
                ResetAllPanelsDepth(AllWindowPanels);
                OpenWindows.Add(path, uiObj);
            }
            if (fun!=null)
            {
                fun(uiObj);
            }
        });
      
    }
    //打开UI界面
    public void OpenDialog(string path, OpenWindowCallBack fun=null)
    {
        GameObject loadObj;
        //判断该路径的窗口没有打开
        if (OpenDialogs.TryGetValue(path, out loadObj))
        {
            if (fun != null)
            {
                fun(loadObj);
            }
            return;
        }
        //加载UI
        LoadUI(path, (uiObj) =>
        {
            if (uiObj!=null)
            {
                SetRoot(uiObj.transform, dialogRoot.transform);
                UIPanel_VT[] panels = uiObj.GetComponentsInChildren<UIPanel_VT>();
                RemoveWindowPanel(AllDialogPanels, panels);
                AddWindowPanel(AllDialogPanels, panels);
                ResetAllPanelsDepth(AllDialogPanels, 100);
                OpenDialogs.Add(path, uiObj);
            }
            
            if (fun != null)
            {
                fun(uiObj);
            }
        });

    }
    //提示弹框
    public void OpenTipsDialog(string content,float time=1f)
    {
        string path = "UI/TipsDialog";
        //加载UI
        LoadUI(path, (uiObj) =>
        {
            if (uiObj != null)
            {
                SetRoot(uiObj.transform, dialogRoot.transform);
                uiObj.SetActive(true);
                UIPanel_VT panel = uiObj.GetComponent<UIPanel_VT>();
                uiObj.transform.SetAsLastSibling();

                UIHelper.SetLabelText(uiObj.transform, "Text", content);
                uiObj.transform.localPosition = Vector3.up * 100f;
            }
        });
    }
    //加载UI界面
    private void LoadUI(string path, OpenWindowCallBack fun)
    {
        
        ResourceLoadCallback callback = delegate (UnityEngine.Object obj, object param)
        {
            GameObject loadObj = obj as GameObject;//根据path路径加载一个物体
            if (loadObj == null)
            {
                Debug.LogError("找不到该路径的窗口预设:" + path);
                if (fun != null)
                {
                    fun(null);
                }
                return;
            }

            string name = loadObj.name;
            loadObj = GameObject.Instantiate(loadObj);
            loadObj.name = name;
           
            //NGUITools.SetLayer(loadObj, LayerMask.NameToLayer("UI"));
            if (fun != null)
            {
                fun(loadObj);
            }
        };
        ResourceManager.GetManager().LoadAsset(path, typeof(GameObject), callback);
    }
    //设置父级
    private void SetRoot(Transform child, Transform root)
    {
        child.SetParent(root);
        UIPanel_VT rectPanel = child.GetComponent<UIPanel_VT>();
        if (rectPanel == null)
        {
            rectPanel = child.gameObject.AddComponent<UIPanel_VT>();
        }
        rectPanel.InitPos();
    }
    //关闭窗口
    public void CloseWindow(string path)
    {
        GameObject windowObj;
        if (OpenWindows.TryGetValue(path, out windowObj))
        {
            UIPanel_VT[] panels = windowObj.GetComponentsInChildren<UIPanel_VT>();
            RemoveWindowPanel(AllWindowPanels, panels);//移除该窗口的所有panel
            ResetAllPanelsDepth(AllWindowPanels);//重置所有Panel的深度
            OpenWindows.Remove(path);//从字典中移除这个窗口
            GameObject.Destroy(windowObj);//销毁掉这个窗口
        }
    }
    //关闭窗口
    public void CloseDialog(string path)
    {
        GameObject windowObj;
        if (OpenDialogs.TryGetValue(path, out windowObj))
        {
            UIPanel_VT[] panels = windowObj.GetComponentsInChildren<UIPanel_VT>();
            RemoveWindowPanel(AllDialogPanels, panels);//移除该窗口的所有panel
            ResetAllPanelsDepth(AllDialogPanels, 0);//重置所有Panel的深度
            OpenDialogs.Remove(path);//从字典中移除这个窗口
            GameObject.Destroy(windowObj);//销毁掉这个窗口
        }
    }
    //移除窗口的panel
    private void RemoveWindowPanel(List<UIPanel_VT> AllPanels, UIPanel_VT[] removePanels)
    {
        if (AllPanels==null||AllPanels.Count==0)
        {
            return;
        }
        //如果是已经打开的窗口,则移除这个窗口的所有panel.
        if (removePanels!=null)
        {
            foreach (var panel in removePanels)
            {
                AllPanels.Remove(panel);
            }
        }
       
    }
    //添加窗口的panel
    private void AddWindowPanel(List<UIPanel_VT> AllPanels, UIPanel_VT[] addPanels)
    {
        if (addPanels!=null)
        {
            foreach (var panel in addPanels)
            {
                AllPanels.Add(panel);
            }
        }
    }
    //重置所有Panel的深度
    private void ResetAllPanelsDepth(List<UIPanel_VT> panels,int startAt=0)
    {
        //设置每一个panel的深度为它在集合中的索引值+1;
        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetSiblingIndex(i + startAt);
        }
    }

    //关闭所有界面
    public void ClearAllWindow()
    {
        foreach (var item in OpenWindows)
        {
            GameObject.Destroy(item.Value);
        }
        OpenWindows.Clear();
    }
    //关闭所有界面
    public void ClearAllDialog()
    {
        foreach (var item in OpenWindows)
        {
            GameObject.Destroy(item.Value);
        }
        OpenWindows.Clear();
    }
}
