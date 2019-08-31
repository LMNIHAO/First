using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Core;

public class AIController : MonoBehaviour
{
    protected Role pRole { get; set; }
    protected List<AICondition> Conditions = new List<AICondition>();
    protected StateMachine machine = new StateMachine();
    [SerializeField]
    protected string curStateName;
    
    private void Start()
    {
        pRole = GetComponent<Role>();
        
        Init();
    }

    public virtual void Init()
    {
        RegisterEvents();
        InitConditions();
        InitAIState();
    }

    /// <summary>
    /// 初始化注册事件
    /// </summary>
    public virtual void RegisterEvents() { }

    //初始化AI条件
    public virtual void InitConditions() { }
    //初始化AI状态
    public virtual void InitAIState() { }
    public void Update()
    {
        if (machine != null)
        {
            machine.OnUpdate();
        }

        foreach (var item in Conditions)
        {
            if (item.OnTrigger())
            {
                ChangeState(item.pAIStateType);
                break;
            }
        }
    }

    public void ChangeState(EnumManager.AIStateType type)
    {
        curStateName = type.ToString();
        machine.GoTo(curStateName);
    }
}
