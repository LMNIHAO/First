using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//技能管理器
public class SkillManager : MonoBehaviour,IRoleMotionListener
{
    //根据技能的动作类型作为键,技能对象作为值,存储所有技能.
    public Dictionary<RoleMotionType, Skill> AllSkills=new Dictionary<RoleMotionType, Skill>();
    private Skill CurSkill;//当前技能
    public Role pOwner { get; set; }//所属的角色
   
	// Use this for initialization
	void Start ()
    {
        pOwner = GetComponentInParent<Role>();
        //初始化所有的技能
        Skill[] allSkill = GetComponentsInChildren<Skill>();
        if (allSkill!=null)
        {
            foreach (var skill in allSkill)
            {
                skill.pOwner = pOwner;
                skill.Init();//技能初始化
                if (AllSkills.ContainsKey(skill.MotionType) == false)
                {
                    AllSkills.Add(skill.MotionType, skill);
                }
            }
        }
        
    }
	// Update is called once per frame
	void Update ()
    {
        //如果当前技能不为空
        if (CurSkill!=null)
        {
            CurSkill.OnUpdate();//更新当前技能
        }
    }
    //根据动作类型获取对应的技能
    public Skill GetSkill(RoleMotionType motionType)
    {
        Skill skill;
        AllSkills.TryGetValue(motionType, out skill);
        return skill;
    }
    //监听动作的开始
    public void OnMotionStart(RoleMotion motion)
    {
        Skill skill=GetSkill(motion.motionType);
        if (skill != null)
        {
            skill.OnStart();//调用技能的开始方法.
        }
        CurSkill = skill;//记录当前技能
    }

    //监听动作的结束
    public void OnMotionEnd(RoleMotion motion)
    {
        Skill skill= GetSkill(motion.motionType); ;
        if (skill != null)
        {
            skill.OnEnd();//调用技能的开始方法.
        }
    }
}
