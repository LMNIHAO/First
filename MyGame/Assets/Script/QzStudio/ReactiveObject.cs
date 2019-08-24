using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactiveObject :MonoBehaviour {
    //当前是否可交互 死亡 消失的时候 false
    public bool isReactive = true;
    //展示内容
    public Text showContext = null;
    //展示特效
    public ParticleSystem effectShow;

    public virtual void Show(string showContexts)
    {
        showContext.text = showContexts;
    }

    public virtual void Reactive()
    {
        if (isReactive&&effectShow!=null)
        {
            effectShow.Play();
        }
    }

    public virtual void Deactive()
    {
        if (!isReactive)
        {
            effectShow.Stop();
        }

    }

    
}
