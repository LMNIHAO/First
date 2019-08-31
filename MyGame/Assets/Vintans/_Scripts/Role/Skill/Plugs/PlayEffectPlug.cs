using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
//特效插件
public class PlayEffectPlug : SkillPlug
{
    public string EffectPath;//特效路径
    public bool UpdatePosition = false;//是否更新位置
    public bool UpdateRotation = false;//是否更新旋转角度
    public Vector3 Offset = Vector3.zero;//偏移量
    [Range(0f,1f)]
    public float DetlaTime=0f;//动画特效的播放偏移时间
    private GameObject EffectObj;//特效物体

    public override void Init()
    {
        base.Init();
        EffectObj = EffectPool.Get(EffectPath);
        EffectPool.Put(EffectPath, EffectObj);
    }

    public override void OnStart()
    {
        EffectObj = EffectPool.Get(EffectPath);
        if (EffectObj!=null)
        {
            EffectObj.SetActive(true);
            EffectObj.transform.position = pOwner.transform.position+pOwner.transform.TransformDirection(Offset);
            EffectObj.transform.rotation = pOwner.transform.rotation;
            
            Animator animator = EffectObj.GetComponentInChildren<Animator>();
            if (animator)
            {
                animator.Update(DetlaTime);
            }
        }
        
    }
    public override void OnUpdate()
    {
        if (UpdatePosition)
        {
            if (EffectObj != null)
            {
                EffectObj.transform.position = pOwner.transform.position + pOwner.transform.TransformDirection(Offset);
            }
        }
        if (UpdateRotation)
        {
            EffectObj.transform.rotation = pOwner.transform.rotation;
        }
        base.OnUpdate();
    }
    public override void OnEnd()
    {
        if (EffectObj!=null)
        {
            EffectPool.Put(EffectPath, EffectObj);
            EffectObj = null;
        }
    }
}

