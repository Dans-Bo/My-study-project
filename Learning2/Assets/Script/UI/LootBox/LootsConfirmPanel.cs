using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootsConfirmPanel:MonoBehaviour
{
    [SerializeField] private Button UIConfirm;
    [SerializeField] private Button UIClose;

    private static LootsConfirmPanel instance;
    public static LootsConfirmPanel Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject panelPrefab = Resources.Load<GameObject>("Prefabs/UI/LootBox/BoxConfirmPanel");
                if(panelPrefab == null)
                {
                    Debug.LogError("未找到预制件，检查路径");
                    return null;
                }

                GameObject obj = GameObject.Instantiate(panelPrefab);
                instance = obj.GetComponent<LootsConfirmPanel>();

                Canvas canvas = FindAnyObjectByType<Canvas>(); 
                if(canvas != null) obj.transform.SetParent(canvas.transform,false);//设置父物体为Canvas

                obj.SetActive(false);
            }
            return instance;
        }
    }
    

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        InitClick();
    }

    private void InitClick()
    {
        UIConfirm.onClick.AddListener(OnConfirm);
        UIClose.onClick.AddListener(OnClose);
    }

    private void OnClose() => instance.gameObject.SetActive(false);

    private void OnConfirm()
    {
        Debug.Log("放入背包");
    }
/// <summary>
/// 设置面板位置
/// </summary>
/// <param name="canvasRect"></param>
/// <param name="mouseScreenPos"></param>
    public void SetPanelPos(RectTransform canvasRect, Vector2 mouseScreenPos)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            mouseScreenPos,
            null,
            out Vector2 localPoint
        ))
        {
            (transform as RectTransform).localPosition = localPoint;
        }
    }

}
