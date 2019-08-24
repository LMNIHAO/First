using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//加载进度UI
public class UILoading : MonoBehaviour, ILoadSceneProgress
{
    public Slider slider;
    public event System.Action OnFinish;
    private float curProgress;//进度
   
    private void Update()
    {
        //return;
        //if (slider.value < curProgress)
        //{
        //    slider.value += Time.deltaTime;
        //    if (slider.value >= 1)
        //    {
        //        if (OnFinish != null)
        //        {
        //            OnFinish();
        //        }
        //    }
        //}
    }
    private void Awake()
    {
        slider.value = 0f;
    }
    //更新进度条
    public void OnProgress(float progress)
    {
        curProgress = progress;
        slider.value = curProgress;
        if (slider.value >= 1)
        {
            if (OnFinish != null)
            {
                OnFinish();
            }
        }
    }
}
