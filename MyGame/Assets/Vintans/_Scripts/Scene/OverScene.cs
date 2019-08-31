using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverScene : BaseScene
{
    public OverScene() : base(EnumManager.SceneType.Over)
    {
    }
    public override void OnStart()
    {
        WindowManager.GetManager().OpenWindow("UI/ui_Over");
        SceneManager.LoadScene("Over");
        base.OnStart();
    }
    public override void OnEnd()
    {
        WindowManager.GetManager().CloseWindow("UI/ui_Over");
        base.OnEnd();
    }
}

