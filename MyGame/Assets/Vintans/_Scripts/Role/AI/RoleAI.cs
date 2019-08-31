using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

//角色AI
public class RoleAI : MonoBehaviour
{
    private Role mRole;//AI控制的角色
    private StateMachine AIStateMachie;//AI状态机
    private List<AICondition> AllConditions;//AI的所有判定条件
    public string curStateName;//当前的AI状态名称
    // Use this for initialization
    void Start ()
    {
        mRole = GetComponent<Role>();
        //初始化所有状态
        AIStateMachie = new StateMachine();
        //AIStateMachie.AddState(new NoneTarget(mRole));
        //AIStateMachie.AddState(new TrackTarget(mRole));
        //AIStateMachie.AddState(new AttackTarget(mRole));
        //AIStateMachie.AddState(new EscapeAIState(mRole));
        //AIStateMachie.AddState(new LoseTarget(mRole));
        //AIStateMachie.AddState(new BackToStartPoint(mRole));

        GoTo(EnumManager.AIStateType.None);
        InitConditions();//初始化所有条件
    }
    //初始化所有条件
    void InitConditions()
    {
        AllConditions = new List<AICondition>();
        AICondition[] arr = GetComponentsInChildren<AICondition>();
        foreach (var item in arr)
        {
            item.pRole = mRole;
            AllConditions.Add(item);
        }
    }
    //切换AI状态
    public void GoTo(EnumManager.AIStateType stateName)
    {
        curStateName = stateName.ToString();
        if (AIStateMachie!=null)
        {
            AIStateMachie.GoTo(stateName.ToString());
        }
    }
	// Update is called once per frame
	void Update ()
    {
        if (AIStateMachie!=null)
        {
            AIStateMachie.OnUpdate();
        }
        foreach (var item in AllConditions)
        {
            if (item.OnTrigger())
            {
                GoTo(item.pAIStateType);
                return;
            }
        }
        //GoTo(AIStateType.None);//切换到待机状态
    }
}

