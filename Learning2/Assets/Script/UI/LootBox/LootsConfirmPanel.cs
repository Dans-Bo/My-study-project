using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootsConfirmPanel:MonoBehaviour
{
    [SerializeField] private Button UIConfirm;
    [SerializeField] private Button UIClose;
    private LootBoxPanel UIParent; 

    void Awake()
    {
        InitClick();
        this.gameObject.SetActive(false);
    }
    void Start()
    {
        UIParent = GetComponentInParent<LootBoxPanel>();
    }

    private void InitClick()
    {
        UIConfirm.onClick.AddListener(OnConfirm);
        UIClose.onClick.AddListener(OnClose);
    }

    private void OnClose() => this.gameObject.SetActive(false);

    private void OnConfirm()
    {
        var loot = UIParent.PackageGetLoot();
        PackageDataManage.Instance.AddItem(loot);
        UIParent.RemoveLoot();
        this.gameObject.SetActive(false);

        
        Debug.Log("放入背包");
    }
/// <summary>
/// 设置面板位置
/// </summary>
/// <param name="canvasRect"></param>
/// <param name="mouseScreenPos"></param>
    public void SetPanelPos(RectTransform canvasRect, Vector2 mouseScreenPos)
    {
        Camera canvasCamera = canvasRect.GetComponent<Canvas>().worldCamera;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            mouseScreenPos,
            canvasCamera,
            out Vector2 localPoint
        ))
        {
            (transform as RectTransform).localPosition = localPoint;
        }
    }
}
