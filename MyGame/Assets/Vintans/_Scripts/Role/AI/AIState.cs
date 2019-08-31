using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

//AI状态父类
public class AIState : State
{
    public Role pRole { get; set; }//状态所属的角色
    public AIState(EnumManager.AIStateType varName, Role role) : base(varName.ToString()) { pRole = role; }
}
