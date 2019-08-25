using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : UIPanel {

    private Animator homeController;

    private Button btn1;

    private Button btn2;

    private Button btn3;

    private Button btn4;

    protected override void InitViews()
    {
        homeController = GetComponent<Animator>();

        btn1 = transform.Find("btn1/Image").GetComponent<Button>();

        btn2 = transform.Find("btn2/Image").GetComponent<Button>();

        btn3 = transform.Find("btn3/Image").GetComponent<Button>();

        btn4 = transform.Find("btn4/Image").GetComponent<Button>();
    }

    protected override void InitListener()
    {
        
    }

    // Use this for initialization
    protected override void Start () {

        InitViews();

        InitListener();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    //显示UI
    public void Show()
    {
        homeController.SetBool("home", true);
    }
    //关闭UI

    public void Hide()
    {
        homeController.SetBool("home", false);
    }

}
