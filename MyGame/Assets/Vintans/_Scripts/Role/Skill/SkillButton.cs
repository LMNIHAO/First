using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public delegate void ExcuteCommand(RoleCommand command);
//技能按钮
public class SkillButton : MonoBehaviour
{
    public EnumManager.RoleCommand Command;//要执行的指令
    public EnumManager.SkillType mType;//技能类型
    //public static event ExcuteCommand onCommand;//执行指令的委托
    public static event Action<EnumManager.RoleCommand> onCommand;//用于执行动作命令的事件
    private Role mRole;//当前的主角
    private float CoolTime=0f;//冷却剩余时间
    private float MaxTime = 0f;//冷却最大时间
   // private UISprite GraySpirte;//冷却透明图片组件
    private void Start()
    {
        //GraySpirte = UIHelper.GetCompnent<UISprite>(transform,"mask");
        RoleManager.Register(EnumManager.RoleEventType.CreatHero, OnHeroCreate);
        if (RoleManager.Hero!=null)
        {
            OnHeroCreate(RoleManager.Hero);
        }
    }
    void OnHeroCreate(object obj)
    {
        mRole = obj as Role;
        //注册角色使用技能的事件
        //mRole.Register(RoleEventID.UseSkill, (go) => {
        //    Skill skill = go as Skill;
        //    if (skill.SkillType== mType)
        //    {
        //        CoolTime = skill.CoolTime;
        //        MaxTime = CoolTime;
        //    }
            
        //});
       
    }
    //更新进度
    private void UpdateProgress(float persent)
    {
        //if (GraySpirte!=null)
        //{
        //    GraySpirte.fillAmount = persent;
        //    GraySpirte.gameObject.SetActive(persent!=0f);
        //}
    }
    private void Update()
    {
        if (CoolTime>0f)
        {
            CoolTime -= Time.deltaTime;
            if (CoolTime<=0)
            {
                CoolTime = 0f;
            }
            UpdateProgress(CoolTime/MaxTime);
        }
        
    }
    //NGUI:UICamera会自动调用被点击物体上面所有脚本里面名字为"OnClick"的方法
    void OnClick()
    {
        if (onCommand!=null)
        {
            onCommand(Command);
        }
    }
}
