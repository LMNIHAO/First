using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    //状态
    public class State
    {
        //状态所属的状态机
        public StateMachine pStateMachine { get; set; }
        //状态的名称(自动属性)
        public string pName { get; set; }
        public State(string name) { this.pName = name; }
        public virtual void OnStart() { }
        public virtual void OnUpdate() { }
        public virtual void OnEnd() { }
    }
    public enum StateEventType
    {
        OnAdd,
        OnStart,
        OnEnd,
    }
    //状态机
    public class StateMachine
    {
        State curState = null;//保存当前状态
        Dictionary<string, State> AllStates;//保存状态机里面所有的状态
        public StateMachine()
        {
            AllStates = new Dictionary<string, State>();
        }
        //当前状态属性
        public State pCurState
        {
            get { return curState; }
        }
        //添加状态
        public void AddState(State state)
        {
            if (state == null)
            {
                return;
            }
            if (AllStates.ContainsKey(state.pName) == false)
            {
                state.pStateMachine = this;//设置状态所属的状态机.
                AllStates.Add(state.pName, state);

            }
        }
        //切换状态(根据状态名称切换状态)
        public virtual void GoTo(string name)//登录,主城,登录
        {
            State tmpState;
            if (AllStates.TryGetValue(name, out tmpState))
            {
                //如果当前状态不为空
                if (curState != null)
                {
                    if (curState==tmpState)
                    {
                        return;
                    }
                    curState.OnEnd();//先退出当前状态
                }
                curState = tmpState;//新取出来的状态作为当前状态
                curState.OnStart();//执行状态的开始
            }
        }
        //状态机的更新方法
        public void OnUpdate()
        {
            if (curState != null)
            {
                curState.OnUpdate();//当前状态的持续更新
            }
        }
    }
}

