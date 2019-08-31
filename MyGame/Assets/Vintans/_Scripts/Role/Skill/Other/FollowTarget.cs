using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//跟随目标物体
public class FollowTarget : MonoBehaviour
{
    public Transform Target;//目标物体
    [Range(1f,10f)]
    public float Height=3f;
    [Range(1f, 10f)]
    public float Distacne=3f;
	// Use this for initialization
	void Start ()
    {
        if (Target!=null)
        {
            Vector3 targetPos = Target.position + Vector3.up * Height + Vector3.back * Distacne;
            this.transform.position = targetPos;
            Quaternion targetRotate = Quaternion.LookRotation(Target.position - this.transform.position + Vector3.up);
            this.transform.rotation = targetRotate;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Target==null)
        {
            return;
        }
        Vector3 targetPos = Target.position + Vector3.up * Height + Vector3.back * Distacne;
        Vector3 curPos = this.transform.position;
        this.transform.position = Vector3.Lerp(curPos, targetPos, 1f);
        //this.transform.position = targetPos;
        Quaternion targetRotate = Quaternion.LookRotation(Target.position - this.transform.position + Vector3.up);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotate,1f);
        //this.transform.rotation=targetRotate;
    }
}
