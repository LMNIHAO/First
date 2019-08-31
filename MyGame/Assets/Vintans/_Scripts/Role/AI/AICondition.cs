using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1&2:0
//1|2:3
[System.Flags]
public enum CompareType
{
    Equal=1,      //等于      000000001
    Less=1<<1, //小于      000000010
    Greater=1<<2,//大于   000000100
}
//条件
public class AICondition:MonoBehaviour
{
    public AIStateType pAIStateType;//条件成立需要调转的状态枚举名称
    public Role pRole;//条件所属的角色
    //public AICondition(Role role) { pRole = role; }
    public virtual bool OnTrigger() { return false; }//条件是否成立
}