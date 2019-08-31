using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//多段伤害插件
public class MultipleDamagePlug : SkillPlug
{
    public float Range = 3;//距离主角的范围
    public Vector3 Offset;//相等主角位置的偏移量
    public bool IsSingle = false;//是否单体伤害
    public DamageInfo[] TriggerDamage;//产生伤害的所有时间点(0.1,0.3,0.5)
    int index;//当前正在判断第几段伤害
    private List<Role> EnemyRoles=new List<Role>();
    public override void OnStart()
    {
        index = 0;
        CheckEnemy();
    }
    //检查敌人
    private void CheckEnemy()
    {
        EnemyRoles.Clear();
        //单体伤害,只获取一个最近的敌人
        if (IsSingle)
        {
            //获取一个最近的敌人
            Role tmpRole = RoleManager.GetNearestRole(pOwner, Offset, Range);
            if (tmpRole != null)
            {
                EnemyRoles.Add(tmpRole);
            }
        }
        else
        {
            //获取范围内的所有敌人
            List<Role> roles = RoleManager.GetRangeRoles(pOwner, Offset, Range);
            if (roles!=null)
            {
                EnemyRoles.AddRange(roles);
            }
        }
    }
    public override void OnUpdate()
    {
        if (index>=TriggerDamage.Length)
        {
            return;
        }
        DamageInfo curInfo = TriggerDamage[index];//获取当前伤害信息
        //角色动画时间达到伤害触发时间,则对所有角色产生伤害
        if (pOwner.pController.pCurMoiton.pNormalizedTime>= curInfo.TriggerTime)
        {
            foreach (var role in EnemyRoles)
            {
                AttackInfo info = new AttackInfo();
                info.Attacker = pOwner;
                info.BeAttacker = role;
               // info.BeatType = curInfo.BeatType;
                info.isBaoJi = false;
               
                pOwner.Attack(info);//主角攻击敌人
            }
            index++;
        }
    }

}
