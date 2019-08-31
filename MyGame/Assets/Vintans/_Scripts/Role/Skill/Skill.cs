using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//技能枚举类型
public enum SkillType
{
    None,
    Skill1,
    Skill2,
    Skill3,
    Skill4,
}
//技能;
public class Skill: MonoBehaviour
{
    public string Name;//技能名称
    public SkillType SkillType;//技能类型
    public RoleMotionType MotionType;//触发该技能的动作类型
    public float CoolTime;//冷却时间
    private float LastUseTime = 0f;//技能最后一次使用的时间
    //技能是否冷却完毕
    public bool IsCooling
    {
        get
        {
            if (LastUseTime==0f)
            {
                return true;//第一次使用时直接返回true;
            }
            return Time.time - LastUseTime >= CoolTime;
        }
    }
    private SkillPlug[] SkillPlugs;//所有的技能插件
    
    public Role pOwner { get; set; }//所属的角色
    public void Init()
    {
        SkillPlugs = GetComponents<SkillPlug>();//获取所有插件
        foreach (var item in SkillPlugs)
        {
            item.Init();
            item.pOwner = pOwner;
        }
    }
   
    //技能开始
    public virtual void OnStart()
    {
        LastUseTime = Time.time;//记录每一次使用技能的时间
        if (SkillPlugs != null)
        {
            //调用每一个插件的OnStart方法
            foreach (var plug in SkillPlugs)
            {
                plug.OnStart();//让每一个技能插件开始运行(让每一个小弟开始做事情)
            }
        }
        pOwner.Notify(RoleEventID.UseSkill, this);//广播开始使用技能
        //Debug.Log("技能开始:" + Name);
    }
    //技能的更新方法
    public virtual void OnUpdate()
    {
        if (SkillPlugs != null)
        {
            //调用每一个插件的OnUpdate方法更新
            foreach (var plug in SkillPlugs)
            {
                plug.OnUpdate();//让每一个插件持续更新
            }
        }
    }
    //技能的结束
    public virtual void OnEnd()
    {
        if (SkillPlugs != null)
        {
            //调用每一个插件的OnEnd方法
            foreach (var plug in SkillPlugs)
            {
                plug.OnEnd();//让每一个技能插件结束运行
            }
        }
        
        //Debug.Log("技能结束:" + Name);
    }
}
