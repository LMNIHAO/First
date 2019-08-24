using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
//场景枚举类型
public enum SceneType
{
    Login,
    Game,
    Over,
}
//场景基类
public class BaseScene : State
{
    public BaseScene(SceneType name) : base(name.ToString())
    {
    }
}
