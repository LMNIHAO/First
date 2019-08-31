using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

//场景基类
public class BaseScene : State
{
    public BaseScene(EnumManager.SceneType name) : base(name.ToString())
    {
    }
}
