using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel_VT : MonoBehaviour
{
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void InitPos()
    {
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;
    }

    /// <summary>
    /// 设置渲染顺序（永远渲染在最外层）
    /// </summary>
    public void SetAsLastSibling()
    {
        transform.SetAsLastSibling();
    }

    /// <summary>
    /// 设置渲染顺序（永远渲染在最里层）
    /// </summary>
    public void SetAsFirstSibling()
    {
        transform.SetAsFirstSibling();
    }

    /// <summary>
    /// 设置渲染顺序（根据父级的子物体下标，设置顺序）
    /// </summary>
    /// <param name="index"></param>
    public void SetSiblingIndex(int index)
    {
        transform.SetSiblingIndex(index);
    }
}
