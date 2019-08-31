using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumManager
{
    //动作指令
    public enum RoleCommand
    {
        Attack,//普攻
        Idle,//站立
        Run,//跑步
        Die,//死亡
        Stun,//眩晕
        Relive,//复活
    }

    //英雄角色动作
    public enum SkillCommand
    {
        Skill1,//技能1
        Skill2,//技能2
        Skill3,//技能3
        Skill4,//技能4
        BeAttack,//被攻击
        BeatWall,//击到墙上
        BeatBack,//击退
        BeatFloat,//浮空
    }

    //动作类型
    public enum RoleMotionType
    {
        Idle = 0,//站立
        Run = 50,//跑步
        Attack1 = 100,//攻击1
        Attack2,
        Attack3,
        Attack4,
        Skill1 = 200,//技能1
        Skill2,
        Skill3,
        Skill4,
        BeAttack = 300,//被攻击
        BeatWall,//击到墙上
        BeatBack,//击退
        BeatFloat,//浮空
        DownStand,//起身
        Stun,//眩晕
        Die = 400,//死亡
        Relive,//复活
    }
    
    //AI状态枚举名称
    public enum AIStateType
    {
        None,
        Track,
        Attack,
        Escape,
        BackStartPoint,
        LoseTarget,
    }


    //技能枚举类型
    public enum SkillType
    {
        None,
        Skill1,
        Skill2,
        Skill3,
        Skill4,
    }
    
    //被打类型
    public enum BeatType
    {
        None = -1,//无效果
        BeAttack,//普通被打
        BeatBack,//击退
        BeatWall,//击退撞墙
        BeatFloat,//浮空击飞
    }

    //游戏相机类型
    public enum GameCameraType
    {
        Follow,//跟随相机
        Point,//定点相机
    }

    //资源加载任务状态;
    public enum ResourceLoadState
    {
        Waitting = 0,//等待加载;
        Loading,//加载中;
        Finished,//加载完成;
    }

    //状态事件类型
    public enum StateEventType
    {
        OnAdd,
        OnStart,
        OnEnd,
    }

    //角色事件ID
    public enum RoleEventID
    {
        Attack,//攻击
        BeAttack,//被攻击
        HpChange,//血量改变
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

    //角色事件类型
    public enum RoleEventType
    {
        CreatHero,//创建主角
        CreatRole,//创建角色
        RemoveRole,//移除玩家
    }

    //场景枚举类型
    public enum SceneType
    {
        Login,
        Game,
        Over,
    }

    //1&2:0
    //1|2:3
    [System.Flags]
    public enum CompareType
    {
        Equal = 1,      //等于      000000001
        Less = 1 << 1, //小于      000000010
        Greater = 1 << 2,//大于   000000100
    }
}
