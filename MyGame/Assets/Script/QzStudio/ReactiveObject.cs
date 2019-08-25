using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏中的可交互的物体 Entity(后面有需要在变更)
/// </summary>
public class ReactiveObject :MonoBehaviour {
    //当前是否可交互 死亡 消失的时候 false
    public bool isReactive = true;
    //展示内容
    public Text showContext = null;
    //展示特效
    public ParticleSystem effectShow;
    //更新显示的内容
    public virtual void Show(string showContexts)
    {
        showContext.text = showContexts;
    }

    /// <summary>
    /// 点击物体时显示在物体下方的 特效(后面用UI代替)
    /// </summary>
    public virtual void Reactive()
    {
        if (isReactive&&effectShow!=null)
        {
            effectShow.Play();
        }
    }

    /// <summary>
    /// 点击物体时显示在物体下方的 关闭特效(后面用UI代替)
    /// </summary>
    public virtual void Deactive()
    {
        if (!isReactive)
        {
            effectShow.Stop();
        }

    }

    
}
