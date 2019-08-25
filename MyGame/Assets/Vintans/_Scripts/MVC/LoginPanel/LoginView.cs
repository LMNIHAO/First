using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginView : View
{
    public override string Name
    {
        get
        {
            return "LoginPanel";
        }
    }

    /// <summary>
    /// 处理事件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="data"></param>
    public override void HandleEvent(string name, object data)
    {
        
    }
}
