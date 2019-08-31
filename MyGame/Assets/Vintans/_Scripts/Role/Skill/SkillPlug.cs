using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageInfo
{
    [Range(0f, 1f)]
    public float TriggerTime;
    public EnumManager.BeatType BeatType;
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

