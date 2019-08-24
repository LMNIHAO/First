using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core;

public class GameScene : BaseScene
{
    public GameScene() : base(SceneType.Game) { }

    public override void OnStart()
    {
        UILoading uiLoading;
        WindowManager.GetManager().OpenDialog("UI/UILoad", (uiObj) =>
        {
            WindowManager.GetManager().ClearAllWindow();

            LoadMainUI();//加载主界面UI
            uiLoading = uiObj.GetComponent<UILoading>();

            uiLoading.OnFinish += () =>
            {
                WindowManager.GetManager().CloseDialog("UI/UILoad");//关闭加载界面.
            };

            //LoadSceneManager.Load("City", uiLoading);
        });
        
        base.OnStart();
    }
    //加载主界面UI
    private void LoadMainUI()
    {
        WindowManager.GetManager().OpenWindow("UI/ui_Main");
    }
    public override void OnEnd()
    {
        WindowManager.GetManager().CloseWindow("UI/ui_Main");

        base.OnEnd();
    }
}
