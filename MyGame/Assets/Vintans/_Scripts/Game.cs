using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class Game : MonoBehaviour
{
    static StateMachine machine;
    public static bool standLogic;
    
    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(this.gameObject);//让某个物体在跳转场景时不被销毁

        machine = new StateMachine();
        machine.AddState(new LoginScene());
        machine.AddState(new GameScene());
        machine.AddState(new OverScene());

        Goto(EnumManager.SceneType.Login);
    }
   
    public static void Goto(EnumManager.SceneType sceneType)
    {
        if (machine != null)
        {
            machine.GoTo(sceneType.ToString());
        }
    }
	// Update is called once per frame
	void Update ()
    {
        if (machine != null)
        {
            machine.OnUpdate();//状态机更新
        }
    }
}
