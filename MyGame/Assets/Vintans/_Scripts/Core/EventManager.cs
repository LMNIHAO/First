using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public delegate void EventFun(object obj);
    public class EventManager : EventManager<object>
    {
    }
   
    public delegate void EventFun<T>(T param);//声明委托类型
    
    //事件管理器
    public class EventManager<T>
    {
        Dictionary<int, List<EventFun<T>>> AllEvents;//事件ID--->事件的所有注册的方法集合
        public EventManager()
        {
            AllEvents = new Dictionary<int, List<EventFun<T>>>();
        }
        //注册事件;
        public void RegisterEvent(int EventID,EventFun<T> Fun)
        {
            //根据事件ID:EventID获取对应的方法的集合:list,并将Fun方法存入list.
            List<EventFun<T>> list;
            if (!AllEvents.TryGetValue(EventID,out list))
            {
                list = new List<EventFun<T>>();
                AllEvents.Add(EventID, list);
            }
            list.Add(Fun);//将fun加入集合中
        }
        //解注册事件;
        public void UnRegisterEvent(int EventID, EventFun<T> Fun)
        {
            List<EventFun<T>> list;
            if (AllEvents.TryGetValue(EventID, out list))
            {
                list.Remove(Fun);
            }
           
        }
        //通知事件.
        public void Notify(int EventID,T param)
        {
            //根据事件ID:EventID获取对应的方法的集合:list
            List<EventFun<T>> list;
            if (AllEvents.TryGetValue(EventID, out list))
            {
                //调用集合中的所有委托方法.
                for (int i = 0; i < list.Count; i++)
                {
                    EventFun<T> fun = list[i];
                    if (fun!=null)
                    {
                        fun(param);
                    }
                }
            }
        }

    }
}
    

