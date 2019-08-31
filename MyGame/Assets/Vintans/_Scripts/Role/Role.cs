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
//角色事件ID
public enum RoleEventID
{
    Attack,//攻击
    BeAttack,//被攻击
    HpChange,//血量改变
    LeaveScene,//离开场景
    UseSkill,//使用技能
    Die,//死亡
}
//阵营
public enum CampType
{
    Camp1,
    Camp2,
    Camp3,
    Camp4
}
//角色类
public class Role : MonoBehaviour
{
    //public RoleType mType;
    public string Name;
    public int RoleID;
    public int mHp;//血量
    private int mMaxHp;
    public int mAttack;//攻击力
    public CampType camp;
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
    ////角色技能管理器
    //private SkillManager mSkillManager;
    //public SkillManager pSkillManager
    //{
    //    get
    //    {
    //        if (mSkillManager==null)
    //        {
    //            mSkillManager=GetComponentInChildren<SkillManager>();
    //        }
    //        return mSkillManager;
    //    }
    //}
    //注册事件
    public void Register(RoleEventID eventID, EventFun<object> fun)
    {
        mEventManager.RegisterEvent((int)eventID, fun);
    }
    //解注册事件
    public void UnRegister(RoleEventID eventID, EventFun<object> fun)
    {
        mEventManager.UnRegisterEvent((int)eventID, fun);
    }
    //广播事件
    public void Notify(RoleEventID eventID, object obj)
    {
        mEventManager.Notify((int)eventID, obj);
    }
    // Use this for initialization
    void Start ()
    {
        mMaxHp=mHp;
        StartPoint = this.transform.position;
    }
    //New攻击
    public void Attack(AttackInfo attakerInfo)
    {
        Notify(RoleEventID.Attack, attakerInfo);
        attakerInfo.BeAttacker.BeAttack(attakerInfo);
    }

    //New被攻击
    public void BeAttack(AttackInfo attakerInfo)
    {
        pEnemy = attakerInfo.Attacker;
        Notify(RoleEventID.BeAttack, attakerInfo);
        int AttackValue = -attakerInfo.Attacker.mAttack-Random.Range(0,30);
        this.mHp += AttackValue;
        ChangeHpInfo hpData = new ChangeHpInfo(this.mHp,this.mMaxHp,AttackValue);
        Notify(RoleEventID.HpChange, hpData);
        if (this.mHp<=0&&!IsDie)
        {
            IsDie=true;
            Notify(RoleEventID.Die, this);
            pController.ExcuteCommand(RoleCommand.Die);
            return;
        }
        //Debug.Log(string.Format("{0}正在被{1}攻击,掉了{2}生命值", this.name, attakerInfo.Attacker.name, attakerInfo.Attacker.mAttack));
        Vector3 dir = attakerInfo.Attacker.transform.position - attakerInfo.BeAttacker.transform.position;
       
        //if (attakerInfo.BeatType == BeatType.BeAttack)
        //{
        //    //pController.ExcuteCommand(RoleCommand.BeAttack);
        //}
        //if (attakerInfo.BeatType == BeatType.BeatFloat)
        //{
        //    pController.ExcuteCommand(RoleCommand.BeatFloat);
        //}
        //if (attakerInfo.BeatType == BeatType.BeatBack)
        //{
        //    pController.Turn(dir);//玩家被打朝向攻击者
        //    pController.ExcuteCommand(RoleCommand.BeatBack);
        //}
        //if (attakerInfo.BeatType == BeatType.BeatWall)
        //{
        //    //pController.ExcuteCommand(RoleCommand.BeatWall);
        //}
    }
    ////执行动作指令,并判断对应的动作技能是否冷却.
    //public bool ExcuteCommand(RoleCommand command)
    //{
    //    Conditional conditional;//接收指令接入成功的条件信息
    //    //通过角色控制器判断当前是否可以接入某个指令
    //    if (pController.CanExcuteCommand(command,out conditional))
    //    {
    //        //根据[ 动作类型 ]获取对应触发的[ 技能 ].
    //        Skill skill = pSkillManager.GetSkill(conditional.MotionName);
    //        if (skill!=null)
    //        {
    //            //技能冷却好了才能真正执行动作
    //            if (skill.IsCooling)
    //            {
    //                return pController.ExcuteCommand(command);//执行动作指令
    //            }
    //        }
    //        else
    //        {
    //            return pController.ExcuteCommand(command);
    //        }
    //    }
    //    return false;
    //}
}
