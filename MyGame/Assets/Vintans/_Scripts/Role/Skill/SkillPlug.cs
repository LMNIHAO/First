using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//被打类型
public enum BeatType
{
    None=-1,//无效果
    BeAttack,//普通被打
    BeatBack,//击退
    BeatWall,//击退撞墙
    BeatFloat,//浮空击飞
}
[System.Serializable]
public class DamageInfo
{
    [Range(0f, 1f)]
    public float TriggerTime;
    public BeatType BeatType;
}

//技能插件
public class SkillPlug:MonoBehaviour
{
    public Role pOwner { get; set; }//所属的角色
    public virtual void Init(){}
    public virtual void OnStart() { }
    public virtual void OnUpdate() { }
    public virtual void OnEnd() { }
}

