using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//条件
public class AICondition:MonoBehaviour
{
    public EnumManager.AIStateType pAIStateType;//条件成立需要调转的状态枚举名称
    public Role pRole;//条件所属的角色
    //public AICondition(Role role) { pRole = role; }
    public virtual bool OnTrigger() { return false; }//条件是否成立
}