using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//强制位移插件
public class ForceMovePlug : SkillPlug
{
    public float MoveDistance = 0f;//移动距离
    public Vector3 MoveDirect;//移动方向
    public float MoveTime = 1f;//移动时间
    public float DelayTime = 0;//延迟时间
    public override void OnStart()
    {
            Vector3 targetPos = pOwner.transform.position + pOwner.transform.TransformDirection(MoveDirect);
            targetPos.y = 0f;
            //TweenPosition tw = pOwner.GetComponent<TweenPosition>();
            //if (tw!=null)
            //{
            //    Destroy(tw);
            //}
            //tw = pOwner.gameObject.AddComponent<TweenPosition>();
            //tw.from = pOwner.transform.position;
            //tw.to = targetPos;
            //tw.duration = MoveTime;
            //tw.delay = DelayTime;
            //tw.ResetToBeginning();
            //tw.PlayForward();
    }
   
}
