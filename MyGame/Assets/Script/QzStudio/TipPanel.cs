using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : MonoBehaviour {

    private Animator homeController;

    private Button btn1;

    private Button btn2;

    private Button btn3;

    private Button btn4;

    private void Awake()
    {
        homeController = GetComponent<Animator>();

        btn1 = transform.Find("btn1/Image").GetComponent<Button>();

        btn2 = transform.Find("btn2/Image").GetComponent<Button>();

        btn3 = transform.Find("btn3/Image").GetComponent<Button>();

        btn4 = transform.Find("btn4/Image").GetComponent<Button>();


    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Show()
    {
        homeController.SetBool("home", true);
    }

    public void Hide()
    {
        homeController.SetBool("home", false);
    }
}
