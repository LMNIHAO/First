using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

//角色管理器
public class RoleManager 
{
    public static Role Hero;//用于保存主角的实例
    static List<Role> AllRoles;//保存所有的角色
    static EventManager<object> EventManager;//角色事件管理器
    public static List<Role> GetAllRoles()
    {
        return new List<Role>(AllRoles);
    }
    static RoleManager()
    {
        AllRoles = new List<Role>();
        EventManager = new EventManager<object>();
        //临时代码,简单获取场景中所有的角色.
        Role[] roles = GameObject.FindObjectsOfType<Role>();//获取场景中所有的角色
        if (roles!=null)
        {
            //将所有角色加入到集合中
            foreach (var item in roles)
            {
                AddRole(item);
            }
        }
      
    }
    //用于添加角色的方法.
    public static void AddRole(Role role)
    {
        //role.Register(RoleEventID.Die,OnRoleDie);
        AllRoles.Add(role);
    }
    public static void ClearAllRole()
    {
        foreach (var item in AllRoles)
        {
            GameObject.Destroy(item.gameObject);
        }
        AllRoles.Clear();
    }
    private static void OnRoleDie(object obj)
    {
        Role role=obj as Role;
        if (role!=null)
        {
            AllRoles.Remove(role);
            GameObject.Destroy(role.gameObject);
        }
    }
    /// <summary>
    /// 获取某个角色位置一定范围内的所有满足条件的角色.
    /// </summary>
    /// <param name="role">目标角色</param>
    /// <param name="offset">目标角色位置的偏移量</param>
    /// <param name="range">距离目标中心点的范围</param>
    /// <param name="sameCamp">是否找相同阵营的角色</param>
    /// <returns>返回满足条件的所有角色</returns>
    public static List<Role> GetRangeRoles(Role role,Vector3 offset,float range, bool sameCamp = false)
    {
        List<Role> list = null;
        //根据角色的位置和偏移量计算出查找范围的中心点坐标
        Vector3 centerPos = role.transform.position + role.transform.TransformDirection(offset);
        foreach (var item in AllRoles)
        {
            if (item == null || item.pIsDie || item == role)
            {
                continue;//查找到死亡的或自己就跳过
            }
            bool success = sameCamp ? item.camp == role.camp : item.camp != role.camp;
            if (success)
            {
                float distance = Vector3.Distance(centerPos, item.transform.position);
                if (distance <= range)
                {
                    if (list==null)
                    {
                        list = new List<Role>();
                    }
                    list.Add(item);//将在范围中的玩家添加到集合中.
                }
            }
        }
        return list;
    }
    /// <summary>
    /// 获取某个角色位置一定范围内的最近的一个满足条件的角色.
    /// </summary>
    /// <param name="role">目标角色</param>
    /// <param name="offset">目标角色位置的偏移量</param>
    /// <param name="range">距离目标中心点的范围</param>
    /// <param name="sameCamp">是否找相同阵营的角色</param>
    /// <returns>返回最近的一个角色</returns>
    public static Role GetNearestRole(Role role, Vector3 offset, float range,bool sameCamp = false)
    {
        Role nearestEnemy = null;
        
        //根据角色的位置和偏移量计算出查找范围的中心点坐标
        Vector3 centerPos = role.transform.position + role.transform.TransformDirection(offset);
        foreach (var item in AllRoles)
        {
            if (item == null || item.pIsDie || item == role)
            {
                continue;//查找到死亡的或自己就跳过
            }
            bool success = sameCamp ? item.camp == role.camp : item.camp != role.camp;
            if (success)
            {
                float distance = Vector3.Distance(centerPos, item.transform.position);
                if (distance <= range)
                {
                    nearestEnemy = item;
                    range = distance;
                }
            }
        }
        return nearestEnemy;
    }
    //根据角色ID获取一个角色
    public static Role GetRole(int roleid)
    {
        foreach (var item in AllRoles)
        {
            if (item!=null&&item.RoleID==roleid)
            {
                return item;
            }
        }
        return null;
    }
    //移除一个角色
    public static void RemoveRole(int roleid)
    {
         foreach (var item in AllRoles)
        {
            if (item!=null&&item.RoleID==roleid)
            {
                AllRoles.Remove(item);
                GameObject.Destroy(item.gameObject);
                break;
            }
        }
       
    }
    //创建角色
    //public static void CreateRole(RoleType roleType, System.Action<Role> onCreate = null)
    //{
    //    Debug.Log(999);
    //    string path = "";
    //    switch (roleType)
    //    {
    //        case RoleType.ZhaoXin:
    //            path = "RolePerfabs/ZhaoXin"; break;
    //        case RoleType.ZhaoYun :
    //            path = "RolePerfabs/ZhaoYun"; break;
    //        default: path = "RolePerfabs/EZ";break;
    //    }
    //    CreateRole(path, onCreate);
    //}
    //创建角色
    public static void CreateRole(string path,System.Action<Role> onCreate=null)
    {
        ResourceManager.GetManager().LoadAsset(path,typeof(GameObject), (loadObj,param)=>
        {
            Role role = null;
            GameObject obj = loadObj as GameObject;
            if (obj != null)
            {
                obj = GameObject.Instantiate(obj);
                GameObject.DontDestroyOnLoad(obj);
                role = obj.GetComponent<Role>();
                if (role != null)
                {
                    AddRole(role);
                }
                else
                {
                    GameObject.Destroy(obj);
                }
            }
            if (onCreate != null)
            {
                onCreate(role);//通知回调方法
            }
            if (role != null)
            {
                Notify(EnumManager.RoleEventType.CreatRole, role);//通知创建角色事件
            }
        }, null);
        
    }
    
    //注册事件
    public static void Register(EnumManager.RoleEventType eventID, EventFun<object> fun)
    {
        EventManager.RegisterEvent((int)eventID, fun);
    }
    //解注册事件
    public static void UnRegister(EnumManager.RoleEventType eventID, EventFun<object> fun)
    {
        EventManager.UnRegisterEvent((int)eventID, fun);
    }
    //广播事件
    public static void Notify(EnumManager.RoleEventType eventID,object obj)
    {
        EventManager.Notify((int)eventID,obj);
    }
   
}
