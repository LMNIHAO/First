using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
//攻击信息
public class AttackInfo
{
    public Role Attacker;//攻击者
    public Role BeAttacker;//被攻击者
    public bool isBaoJi;//是否暴击
}
//血量改变信息
public struct ChangeHpInfo
{
    public int Hp;
    public int MaxHp;
    public int changeValue;
    public ChangeHpInfo(int hp, int max, int change)
    {
        this.Hp = hp;
        this.MaxHp = max;
        this.changeValue = change;
    }
}
//角色类
public class Role : MonoBehaviour
{
    public string Name;
    public int RoleID;
    public int mHp;//血量
    private int mMaxHp;
    public int mAttack;//攻击力
    public EnumManager.CampType camp;
    public Vector3 StartPoint;//出生点
    private RoleController mController;
    public RoleController pController
    {
        get
        {
            if (mController == null)
            {
                mController = this.GetComponent<RoleController>();
            }
            return mController;
        }     
    }
    private EventManager mEventManager=new EventManager();
    private bool IsDie=false;
    public bool pIsDie
    {
        get{return IsDie;}
    }

    //敌人
    public Role pEnemy { get; set; }

    #region 事件

    //注册事件
    public void Register(EnumManager.RoleEventID eventID, EventFun<object> fun)
    {
        mEventManager.RegisterEvent((int)eventID, fun);
    }
    //解注册事件
    public void UnRegister(EnumManager.RoleEventID eventID, EventFun<object> fun)
    {
        mEventManager.UnRegisterEvent((int)eventID, fun);
    }
    //广播事件
    public void Notify(EnumManager.RoleEventID eventID, object obj)
    {
        mEventManager.Notify((int)eventID, obj);
    }

    #endregion
    
    void Start ()
    {
        mMaxHp=mHp;
        StartPoint = this.transform.position;
    }
    //New攻击
    public void Attack(AttackInfo attakerInfo)
    {
        Notify(EnumManager.RoleEventID.Attack, attakerInfo);
        attakerInfo.BeAttacker.BeAttack(attakerInfo);
    }

    //New被攻击
    public void BeAttack(AttackInfo attakerInfo)
    {
        pEnemy = attakerInfo.Attacker;
        Notify(EnumManager.RoleEventID.BeAttack, attakerInfo);
        int AttackValue = -attakerInfo.Attacker.mAttack-Random.Range(0,30);
        this.mHp += AttackValue;
        ChangeHpInfo hpData = new ChangeHpInfo(this.mHp,this.mMaxHp,AttackValue);
        Notify(EnumManager.RoleEventID.HpChange, hpData);
        if (this.mHp<=0&&!IsDie)
        {
            IsDie=true;
            Notify(EnumManager.RoleEventID.Die, this);
            pController.ExcuteCommand(EnumManager.RoleCommand.Die);
            return;
        }
    }
}
