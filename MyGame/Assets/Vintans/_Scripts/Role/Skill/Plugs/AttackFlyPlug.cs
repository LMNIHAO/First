using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//击飞插件
public class AttackFlyPlug : SkillPlug
{
    public float FlyTime = 1f;//击飞时间
    public float FlyHeight = 2f;//击飞高度
    public float DelayTime = 0f;//播放击飞的延迟时间
    public override void OnStart()
    {
        AddTween();
        base.OnStart();
    }
    //添加击飞移动组件
    public  void AddTween()
    {
            Vector3 startPos = Vector3.zero;
            Vector3 targetPos = startPos + Vector3.up* FlyHeight;
            //TweenPosition tp = pOwner.GetComponent<TweenPosition>();
            //if (tp != null)
            //{
            //    Destroy(tp);
            //}
            //tp = pOwner.pController.pAnimator.gameObject.AddComponent<TweenPosition>();
            //tp.from = startPos;
            //tp.to = targetPos;
            //tp.duration = FlyTime;
                   
            //EventDelegate del = new EventDelegate(() =>
            //    {
            //    tp.PlayReverse();//1to--->from0
            //});
            //del.oneShot = true;
            //tp.SetOnFinished(del);
            //tp.delay = DelayTime;
            //tp.animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            //tp.ResetToBeginning();
            //tp.PlayForward();//from0--->to1
                
    }
}
