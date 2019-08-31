using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllItemSelect : MonoBehaviour {

    /// <summary>
    /// 存放可框线的角色
    /// </summary>
    public List<GameObject> characters;
    /// <summary>
    /// 单击选择的物体
    /// </summary>
    public GameObject SelectItem;
    /// <summary>
    /// 单选射线
    /// </summary>
    public Ray ray;
    /// <summary>
    /// 存储的信息
    /// </summary>
    public RaycastHit raycast;
    /// <summary>
    /// 画线的颜色
    /// </summary>
    public Color rectColor = Color.green;
    /// <summary>
    /// 开始的位置
    /// </summary>
    private Vector3 start = Vector3.zero;
    /// <summary>
    /// 画线的材质
    /// </summary>
    public Material rectMat = null;
    /// <summary>
    /// 是否开始画线
    /// </summary>
    private bool drawRectangle = false;
    /// <summary>
    /// 玩家头像摄像机
    /// </summary>
    public GameObject PlayerCamera;


    void Start()
    {
        rectMat.hideFlags = HideFlags.HideAndDontSave;
        rectMat.shader.hideFlags = HideFlags.HideAndDontSave;//不显示在hierarchy面板中的组合，不保存到场景并且卸载Resources.UnloadUnusedAssets不卸载的对象。
        PlayerCamera = GameObject.Find("UIRoot").transform.Find("ButtonPanel/PlayerPanel/PlayerCamera").gameObject;
    }


    void Update()
    {
        InputSelectItem();
    }
    private void FixedUpdate()
    {
        CameraFollow();
    }

    /// <summary>
    /// 玩家头像摄像机跟随
    /// </summary>
    public void CameraFollow() {
        //if (SelectItem.tag=="Charactor")
        //{
        //    PlayerCamera.transform.position = new Vector3(SelectItem.transform.position.x, SelectItem.transform.position.y, SelectItem.transform.position.z - 12);
        //}
        // else
        if (!SelectItem)
        {
            return;
        }

        PlayerCamera.transform.position = new Vector3(SelectItem.transform.position.x, SelectItem.transform.position.y, SelectItem.transform.position.z-12);
    }
    /// <summary>
    /// 单选和多选
    /// </summary>
    public void InputSelectItem()
    {
      
      
        if (Input.GetMouseButtonDown(0))
        {
           
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycast))
            {
                if (raycast.collider.gameObject.name == "Cube")
                {
                    SelectItem = raycast.collider.gameObject;
                 //   SelectItem.layer = "SelectItem";
                    Debug.Log(raycast.collider.gameObject.name);
                }

            }

            drawRectangle = true;//如果鼠标左键按下 设置开始画线标志
            start = Input.mousePosition;//记录按下位置

        }
        else if (Input.GetMouseButtonUp(0))
        {
            drawRectangle = false;//如果鼠标左键放开 结束画线
            checkSelection(start, Input.mousePosition);
        }
    }


    void OnPostRender()
    {

        if (drawRectangle)
        {

            Vector3 end = Input.mousePosition;//鼠标当前位置

            GL.PushMatrix();//保存摄像机变换矩阵,把投影视图矩阵和模型视图矩阵压入堆栈保存

            if (!rectMat)

                return;

            rectMat.SetPass(0);//为渲染激活给定的pass。

            GL.LoadPixelMatrix();//设置用屏幕坐标绘图

            GL.Begin(GL.QUADS);//开始绘制矩形

            GL.Color(new Color(rectColor.r, rectColor.g, rectColor.b, 0.1f));//设置颜色和透明度，方框内部透明

            //绘制顶点
            GL.Vertex3(start.x, start.y, 0);

            GL.Vertex3(end.x, start.y, 0);

            GL.Vertex3(end.x, end.y, 0);

            GL.Vertex3(start.x, end.y, 0);

            GL.End();


            GL.Begin(GL.LINES);//开始绘制线

            GL.Color(rectColor);//设置方框的边框颜色 边框不透明

            GL.Vertex3(start.x, start.y, 0);

            GL.Vertex3(end.x, start.y, 0);

            GL.Vertex3(end.x, start.y, 0);

            GL.Vertex3(end.x, end.y, 0);

            GL.Vertex3(end.x, end.y, 0);

            GL.Vertex3(start.x, end.y, 0);

            GL.Vertex3(start.x, end.y, 0);

            GL.Vertex3(start.x, start.y, 0);

            GL.End();

            GL.PopMatrix();//恢复摄像机投影矩阵

        }

    }


    /// <summary>
    /// 检测被选择的物体
    /// </summary>
    /// <param name="start">开始的点</param>
    /// <param name="end">结束的点</param>
    void checkSelection(Vector3 start, Vector3 end)
    {

        Vector3 p1 = Vector3.zero;

        Vector3 p2 = Vector3.zero;

        if (start.x > end.x)
        {//这些判断是用来确保p1的xy坐标小于p2的xy坐标，因为画的框不见得就是左下到右上这个方向的

            p1.x = end.x;

            p2.x = start.x;

        }

        else
        {

            p1.x = start.x;

            p2.x = end.x;

        }

        if (start.y > end.y)
        {

            p1.y = end.y;

            p2.y = start.y;

        }

        else
        {

            p1.y = start.y;

            p2.y = end.y;

        }

        foreach (GameObject obj in characters)
        {//把可选择的对象保存在characters数组里

            Vector3 location = Camera.main.WorldToScreenPoint(obj.transform.position);//把对象的position转换成屏幕坐标

            if (location.x < p1.x || location.x > p2.x || location.y < p1.y || location.y > p2.y

            || location.z < Camera.main.nearClipPlane || location.z > Camera.main.farClipPlane)//z方向就用摄像机的设定值，看不见的也不需要选择了
            {
                obj.SetActive(true);
            }

            else
            {
                obj.SetActive(false);
            }

        }

    }
}
