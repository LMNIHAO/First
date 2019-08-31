using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
//动作指令
public enum RoleCommand
{
    Attack,//普攻
    Idle,//站立
    Run,//跑步
    Skill1,//技能1
    Skill2,//技能2
    Skill3,//技能3
    Skill4,//技能4
    BeAttack,//被攻击
    BeatWall,//击到墙上
    BeatBack,//击退
    BeatFloat,//浮空
    Die,//死亡
    Stun,//眩晕
    Relive,//复活
}
//角色动作监听接口
public interface IRoleMotionListener
{
    void OnMotionStart(RoleMotion motion);
    void OnMotionEnd(RoleMotion motion);
}
//角色控制器
public class RoleController : MonoBehaviour
{
    private Animator mAnimator;
    public Animator pAnimator
    {
        get
        {
            if (mAnimator==null)
            {
                mAnimator = GetComponentInChildren<Animator>();
            }
            return mAnimator; }
    }
    private RoleMotion CurMotion;//当前的动画状态的实例
    public RoleMotion pCurMoiton
    {
        get { return CurMotion; }
    }
    private IRoleMotionListener RoleMotionListener; //角色动作监听接口
    //private Queue<RoleCommand> cachCommand=new Queue<RoleCommand>();
    private void Start()
    {
        RoleMotionListener = GetComponentInChildren<IRoleMotionListener>();//获取SkillManager实例
        mAnimator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        //if(cachCommand.Count>0)
        //{
        //    if (ExcuteCommand(cachCommand.Peek()))
        //    {
        //        cachCommand.Dequeue();
        //    }
        //}
    }
    //判断是否可以运行该动作指令
    public bool CanExcuteCommand(RoleCommand command,out Conditional conditional)
    {
        conditional = null;
        if (CurMotion != null)
        {
            return CurMotion.CanExcuteCommand(command,out conditional); //当前状态执行该动作指令
        }
        return false;
    }
    //运行动作指令
    public bool ExcuteCommand(RoleCommand command)
    {
        if (CurMotion!=null)
        {
            if (command == RoleCommand.Run && CurMotion.motionType == RoleMotionType.Run)
            {
                return false;
            }
            return CurMotion.ExcuteCommand(command); //当前状态执行该动作指令
        }
        //else
        //{
        //    cachCommand.Clear();
        //    cachCommand.Enqueue(command);
        //}
        return false;
    }
    //播放动作
    public void PlayMotion(RoleMotionType motion)
    {
        if (pAnimator!=null)
        {
            pAnimator.Play(motion.ToString());
        }
    }
    public void Move(Vector3 offset)
    {
        UnityEngine.AI.NavMeshAgent nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (nav!=null)
        {
            offset = this.transform.TransformDirection(offset);
            nav.Move(offset);
        }
    }
    //角色转向:Force:强制转向
    public void Turn(Vector3 dir, bool Force = false)
    {
        if (Force || (CurMotion != null && CurMotion.CanTurnDirect()))
        {
            dir.y = 0;
            this.transform.rotation=Quaternion.LookRotation(dir);
            //this.transform.localEulerAngles = new Vector3(0, Vector3.Angle(Vector3.forward,dir), 0);
            //this.transform.rotation = Quaternion.Euler(new Vector3(0, Vector3.Angle(Vector3.forward, dir), 0));
        }
    }
    //角色转向:Force:强制转向
    public void TurnOfAxis(Vector3 axis,float angle, bool Force = false)
    {
        if (Force || (CurMotion != null && CurMotion.CanTurnDirect()))
        {
            this.transform.Rotate(axis, angle);
        }
    }
    //动画状态开始
    public void OnMotionStart(RoleMotion motion)
    {
        CurMotion = motion;//保存当前状态实例
        if (RoleMotionListener!=null)
        {
            RoleMotionListener.OnMotionStart(motion);//通知状态开始//skillmanager.OnMotionStart
        }
    }
    //动画状态结束.
    public void OnMotionEnd(RoleMotion motion)
    {
        if (RoleMotionListener != null)
        {
            RoleMotionListener.OnMotionEnd(motion);//通知状态结束
        }
    }
}
