using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core;

public class LoginScene : BaseScene
{
    public LoginScene() : base(EnumManager.SceneType.Login)
    {
    }

    public override void OnStart()
    {
        WindowManager.GetManager().OpenWindow("UI/LoginPanel");
        SceneManager.LoadScene("Login");
        base.OnStart();
    }

    public override void OnEnd()
    {
        WindowManager.GetManager().CloseWindow("UI/LoginPanel");
        base.OnEnd();
    }

}

