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

        GoTo(AIStateType.None);
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
    public void GoTo(AIStateType stateName)
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
        //AICondition cd1 = new DistanceCondition();
        //AICondition cd2 = new HasEnemy();
        //AICondition cd3 = new HPConditon();
        //AICondition[] arr = { cd1, cd2, cd3 };
        ////Dictionary<string, System.Action> dic=new Dictionary<string, System.Action>();
        ////dic.Add("DistanceCondition", Update1);
        ////dic.Add("HasEnemy", Update2);
        ////dic.Add("HPConditon", Update3);
        //Dictionary<string, TestState> dic = new Dictionary<string, TestState>();
        //dic.Add("DistanceCondition", new function1());
        //dic.Add("HasEnemy", new function2());
        //dic.Add("HPConditon", new function3());
        //foreach (var item in arr)
        //{
        //    if (item.OnTrigger())
        //    {
        //        //System.Action action;
        //        //if (dic.TryGetValue(item.ToString(),out action))
        //        //{
        //        //    action();
        //        //}
        //       TestState state;
        //        if (dic.TryGetValue(item.ToString(), out state))
        //        {
        //            state.Update();
        //        }

        //        //if (item is DistanceCondition)
        //        //{
        //        //    Update1();
        //        //}
        //        //if (item is HasEnemy)
        //        //{
        //        //    Update2();
        //        //}
        //        //if (item is HPConditon)
        //        //{
        //        //    Update3();
        //        //}
        //    }
        //}
        //if (cd1.OnTrigger())
        //{
        //    //具体逻辑
        //    //回到出生点
        //    Update1();
        //}
        //else if (cd2.OnTrigger())
        //{
        //    //具体逻辑
        //    //攻击敌人
        //    Update2();
        //}
        //else if (cd3.OnTrigger())
        //{
        //    //具体逻辑
        //    //逃跑
        //    Update3();
        //}
        GoTo(AIStateType.None);//切换到待机状态
    }
    void Update1()
    {
    }
    void Update2()
    {
    }
    void Update3()
    {
    }
    public class TestState
    {
        public virtual void Update() { }
    }
    public class function1 : TestState
    {
        public override void Update()
        {
            Debug.Log("功能一 ");
        }
    }
    public class function2 : TestState
    {
        public override void Update()
        {
            Debug.Log("功能二 ");
        }
    }
    public class function3 : TestState
    {
        public override void Update()
        {
            Debug.Log("功能三 ");
        }
    }
}

