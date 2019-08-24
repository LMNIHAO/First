using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UIHelper
{
    public static void SetButtonEvent(Transform root, string path, UIEventListener.VoidDelegate fun)
    {
        if (root == null)
        {
            Debug.LogError("UIHelper.SetButtonEvent:root is Null");
            return;
        }
        Transform tranBtn = root.Find(path);
        if (tranBtn != null)
        {
            UIEventListener.Get(tranBtn.gameObject).onClick += fun;
        }
    }
    public static void SetLabelText(Transform root,string path,string content)
    {
        if (root == null)
        {
            Debug.LogError(" UIHelper.SetButtonEvent:root is Null");
            return;
        }
        Text lb = GetCompnent<Text>(root, path);
        if (lb!=null)
        {
            lb.text = content;
        }
    }
    public static T GetCompnent<T>(Transform root, string path) where T : Component
    {
        if (root != null)
        {
            Transform tranBtn = root.Find(path);
            if (tranBtn != null)
            {
                return tranBtn.GetComponent<T>();
            }
        }
        return null;
    }
}