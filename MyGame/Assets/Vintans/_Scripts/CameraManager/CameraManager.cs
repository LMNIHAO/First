using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

//游戏相机类
public class GameCamera:State
{
    //public string pName;
    //跟随的目标物体;
    public GameObject Target
    {
        get { return CameraManager.GetSingle().TargetObj; }
    }
    public Camera pCamera { get; set; }
    public GameCamera(EnumManager.GameCameraType type) : base(type.ToString())
    {
        GameObject cameraObj = new GameObject(type.ToString());
        cameraObj.transform.SetParent(CameraManager.GetSingle().transform);
        pCamera = cameraObj.AddComponent<Camera>();
        pCamera.cullingMask = ~(1<<LayerMask.NameToLayer("UI"));
        cameraObj.SetActive(false);
    }
    public override void OnStart()
    {
        pCamera.gameObject.SetActive(true);
        base.OnStart();
    }
    public override void OnEnd()
    {
        pCamera.gameObject.SetActive(false);
        base.OnEnd();
    }
}
//跟随相机
public class FollowCamera : GameCamera
{
    public FollowCamera() : base(EnumManager.GameCameraType.Follow)
    {
    }
    public override void OnUpdate()
    {
        if (Target==null||pCamera==null)
        {
            return;
        }
        Vector3 offset = Vector3.back * 6 + Vector3.up * 8;
        Vector3 cameraPos = Target.transform.position + offset;
        Vector3 cameraDir = -offset;
        pCamera.transform.position = cameraPos;
        pCamera.transform.rotation = Quaternion.LookRotation(cameraDir);
        base.OnUpdate();
    }
}
//跟随相机
public class PointCamera : GameCamera
{
    public PointCamera() : base(EnumManager.GameCameraType.Point)
    {
    }
    public override void OnStart()
    {
        base.OnStart();
        Vector3 offset = Vector3.back * 6 + Vector3.up * 10;
        pCamera.transform.position = Target.transform.position + offset;
        pCamera.transform.rotation = Quaternion.LookRotation(-offset);
        
    }
    public override void OnUpdate()
    {
        if (Target == null || pCamera == null)
        {
            return;
        }
        
        float offsety = 0f;
        float offsetx = 0f;
        if (Input.mousePosition.y > Screen.height)
        {
            offsety = 1f;
        }
        else if (Input.mousePosition.y < 0)
        {
            offsety = -1f;
        }
        if (Input.mousePosition.x > Screen.width)
        {
            offsetx = 1f;
        }
        else if (Input.mousePosition.x < 0)
        {
            offsetx = -1f;
        }
        Vector3 offset = Vector3.back * 6 + Vector3.up *10;
       
        if (Input.GetKey(KeyCode.Space))
        {
            pCamera.transform.position = Target.transform.position + offset;
        }
        else
        {
            pCamera.transform.position += new Vector3(offsetx, 0, offsety);
        }
        
        pCamera.transform.rotation = Quaternion.LookRotation(-offset);
        base.OnUpdate();
    }
}

//摄像机管理
public class CameraManager : MonoBehaviour
{
    private static CameraManager mInstance;//保存唯一的实例
    private StateMachine mStateMachine;//相机状态机
    public GameObject TargetObj;//摄像机照射的目标物体
    public static CameraManager GetSingle()
    {
        if (mInstance==null)
        {
            GameObject obj = new GameObject("CameraManager");//创建一个空物体
            GameObject.DontDestroyOnLoad(obj);//标记该物体在场景切换时不被销毁掉
            mInstance = obj.AddComponent<CameraManager>();//给空物体上添加CameraManager组件
            mInstance.Init();
        }
        return mInstance;
    }
    //获取当前状态的摄像机
   public static Camera curCamera
    {
        get
        {
            GameCamera camera = GetCurent();
            if (camera!=null)
            {
                return camera.pCamera;
            }
            return null;
        }
    }
    public static GameCamera GetCurent()
    {
        return CameraManager.GetSingle().GetCurrentCamera();
    }
    public GameCamera GetCurrentCamera()
    {
        return mStateMachine.pCurState as GameCamera;
    }
    public void Init()
    {
        mStateMachine = new StateMachine();
        mStateMachine.AddState(new FollowCamera());
        mStateMachine.AddState(new PointCamera());
        Goto(EnumManager.GameCameraType.Follow);
    }
    //切换相机状态
    public void Goto(EnumManager.GameCameraType cameraType)
    {
        mStateMachine.GoTo(cameraType.ToString());
    }
	// Update is called once per frame
	void Update ()
    {
        if (mStateMachine!=null)
        {
            mStateMachine.OnUpdate();
        }
	}
}
