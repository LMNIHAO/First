using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//动作类型
public enum RoleMotionType
{
    Idle=0,//站立
    Run=50,//跑步
    Attack1=100,//攻击1
    Attack2,
    Attack3,
    Attack4,
    Skill1=200,//技能1
    Skill2,
    Skill3,
    Skill4,
    BeAttack=300,//被攻击
    BeatWall,//击到墙上
    BeatBack,//击退
    BeatFloat,//浮空
    DownStand,//起身
    Stun,//眩晕
    Die=400,//死亡
    Relive,//复活
}
//接入动作指令的条件
[System.Serializable]
public class Conditional
{
    public RoleCommand Command;//可接入的动作指令(按下某个键)
    public RoleMotionType MotionName;//需要播放的动画名称(枚举成员名称对应动画名称)
    [Range(0f,1f)]
    public float AnimL=0;//可接入指令的起始时间点
    [Range(0f, 1f)]
    public float AnimR=1;//可接入指令的终止时间点
    [Range(0f,1f)]
    public float TriggerTime=1;//缓存触发时间
    public bool Immediate;//是否立刻切换.
}
//移动信息
[System.Serializable]
public class MoveInfo
{
    public Vector3 MoveDir;//移动的方向向量
    [Range(0f,1f)]
    public float StartTime;//移动的开始时间
    [Range(0f,1f)]
    public float EndTime;//移动的结束时间
}
//转向信息
[System.Serializable]
public class TurnInfo
{
    [Range(0f, 1f)]
    public float StartTime;//可以转向的开始时间
    [Range(0f, 1f)]
    public float EndTime;//可以转向的结束时间
}
//继承StateMachineBehaviour,脚本可以挂载在动画状态上
public class RoleMotion : StateMachineBehaviour
{
    public RoleMotionType motionType;//动作类型
    public Conditional[] Conditions;//配置所有的指令条件
    private Conditional CacheCondition;//下一个指令条件.
    private Animator mAnimator;//动画组件
    private RoleController mRoleController;//角色控制器
    public bool EndToDefault=true;//默认回到原地站立动作.
    public RoleMotionType EndToMotion = RoleMotionType.Idle;//当前动作结束后切换到下一个默认的动作.
    public MoveInfo MoveInfo;//移动信息
    public TurnInfo TurInfo;//转向信息
    private AnimatorStateInfo curStateInfo;
    private bool Exchanging = false;//动画是否在切换中
    //获取动画的归一化时间(0-1之间)(动画播放的进度百分比)
    public float pNormalizedTime
    {
        get
        {
            return (curStateInfo.normalizedTime==0f || curStateInfo.normalizedTime == 1f) ? curStateInfo.normalizedTime : curStateInfo.normalizedTime % 1;
        }
    }
    //判断是否可以接入某个指令
    public bool CanExcuteCommand(RoleCommand command,out Conditional conditional)
    {
        conditional = null;
        //正在切换下一个动作中
        if (CacheCondition!=null||Exchanging)
        {
            return false;
        }
        foreach (var item in Conditions)
        {
            if (item.Command == command && item.AnimL <= pNormalizedTime && pNormalizedTime <= item.AnimR)
            {
                conditional = item;
                return true;
            }
        }
        return false;
    }
    //当前状态执行某个动作指令
    public bool ExcuteCommand(RoleCommand command)
    {
        Conditional conditional;
        if (CanExcuteCommand(command,out conditional))
        {
            //是否立刻切换
            if (conditional.Immediate)
            {
                Play(conditional.MotionName.ToString(), GetTranslateTime());//立刻切换
                CacheCondition = null;
                return true;
            }
            //Debug.Log(this.motionType.ToString() + "缓存动作:" + conditional.MotionName.ToString());
            CacheCondition = conditional;//缓存等待播放
            return true;
        }
     
        return false;
    }
    private float GetTranslateTime()
    {
        if (curStateInfo.length>0f)
        {
            float translateTime = curStateInfo.length * (1f - pNormalizedTime);
            if (translateTime>0.1f)
            {
                return 0.1f;
            }
            return translateTime;
        }
        return 0f;
    }
    //播放动画
    public void Play(string stateName,float durtime=0.1f)
    {
        if (mAnimator!=null)
        {
            //获取当前动画状态信息并且判断是否跟要切换的名称相同
            if (mAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName)) 
            {
                Exchanging = true;
                mAnimator.CrossFadeInFixedTime(stateName, 0);//切换同一个动画时,让动画从头开始播放
                return;
            }
            Exchanging = true;
            //播放动画,durtime是两个动画之间的融合时间
            mAnimator.CrossFade(stateName, durtime);//立刻切换
        }
    }
    //状态的开始
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Exchanging = false;
        //Debug.Log("动作的开始:"+motionType);
        curStateInfo = stateInfo;
        mAnimator = animator;
        mRoleController = animator.GetComponentInParent<RoleController>();
        if (mRoleController != null)
        {
            mRoleController.OnMotionStart(this);
        }
    }
    //状态的更新
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        curStateInfo = stateInfo;//记录当前状态信息
        if (Exchanging)
        {
            return;
        }
        //判断是否有缓存执行的指令,并且是否到达了触发时间
        if (CacheCondition != null)
        {
            if (stateInfo.normalizedTime >= CacheCondition.TriggerTime)
            {
                Play(CacheCondition.MotionName.ToString(), GetTranslateTime());//切换到下一个缓存动作.
                CacheCondition = null;
                return;
            }
        }
        else
        {
            //是否需要回到默认状态
            if (EndToDefault && stateInfo.normalizedTime >= 1f)
            {
                Play(EndToMotion.ToString(), 0f);//切换到默认的下一个动作.
                return;
            }
        }
        
        Move();
    }
    //状态的结束
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (mRoleController != null)
        {
            mRoleController.OnMotionEnd(this);
        }
        Exchanging = false;
        //Debug.Log("动作的结束:" + motionType);
    }
    //动画位移
    private void Move()
    {
        if (MoveInfo.MoveDir!=Vector3.zero)
        {
            if (MoveInfo.StartTime<=pNormalizedTime&& pNormalizedTime <= MoveInfo.EndTime)
            {
                //mRoleController.transform.Translate(MoveInfo.MoveDir * Time.deltaTime);
                mRoleController.Move(MoveInfo.MoveDir * Time.deltaTime);
            }

        }
    }
    //是否可以转向
    public bool CanTurnDirect()
    {
        if (this.TurInfo.StartTime<this.pNormalizedTime&&this.pNormalizedTime <= this.TurInfo.EndTime)
        {
            return true;
        }
        return false;
    }
}
