using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverTipManager : MonoBehaviour
{
    //remember to disable raycasting on the Tip Window and Text
    public TextMeshProUGUI tipText;
    public GameObject tipWindow;
    private RectTransform _tipWindowRectTransform;
    public float offScreenXoffSet;
    [SerializeField] Vector2 positionOffset;

    public static Action<string, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;
    }

    private void OnDisale()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }

    void Start()
    {
        HideTip();
        _tipWindowRectTransform = (RectTransform) tipWindow.transform;
    }

    private void ShowTip(string tip, Vector2 mousePos)
    {
        tipText.text = tip;
        _tipWindowRectTransform.sizeDelta = new Vector2(tipText.preferredWidth > 700 ? 700 : tipText.preferredWidth, tipText.preferredHeight+100);


        tipWindow.gameObject.SetActive(true);

        if (mousePos.x +200 > Screen.width)
        {
            tipWindow.transform.position = new Vector2(mousePos.x + _tipWindowRectTransform.sizeDelta.x - offScreenXoffSet, mousePos.y);
            Debug.Log("on left");
        }
        else { tipWindow.transform.position = new Vector2(mousePos.x + _tipWindowRectTransform.sizeDelta.x /8, mousePos.y); }
        
    }

    private void HideTip()
    {
        tipText.text = default;
        tipWindow.gameObject.SetActive(false);
    }
}