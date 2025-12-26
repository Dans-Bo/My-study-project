using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LootBoxPanel : BasePanel
{
    [SerializeField] private Button UICloseButton;
    [SerializeField] private GameObject lootCellPanelPrefab;
    [SerializeField] private RectTransform UILoots;
    List<PackageLocalTableData> boxLoots = new();

    protected override void Awake()
    {
        InitClick();
    }

    void Start() 
    {
        GenerateLoots();
        RefreshBoxView(); 
    }

/// <summary>
/// 战利品生成
/// </summary>
    private void GenerateLoots()
    {
        int num = UnityEngine.Random.Range(6,12);
        for(int i =0 ; i < num; i++)
        {
           var item = LootsManager.Instances.GenerateRandomItem();
           boxLoots.Add(item);
        }
    }
/// <summary>
/// 刷新箱子中的物品
/// </summary>
    private void RefreshBoxView()
    {
        //删除箱子
        if(UILoots.childCount != 0)
        {
            for(int i = UILoots.childCount -1; i>= 0 ; i--)
            {
                Destroy(UILoots.GetChild(i).gameObject);
            }
        }

        foreach(var localItem in boxLoots)
        {
            if(lootCellPanelPrefab == null)
            {
                Debug.LogError("lootCellPanel未赋值");
            }
            
           Transform boxItem = Instantiate(lootCellPanelPrefab.transform,UILoots);
           LootCellPanel cell = boxItem.GetComponent<LootCellPanel>();
           if(cell == null)
            {
                Debug.LogError("未找到预制件");
                continue;
            }

           //更新箱子格子
           cell.Refresh(localItem,this); 
        }


    }

    private void InitClick()
    {
        UICloseButton.onClick.AddListener(OnClose);
    }

    private void OnClose()
    {
        UIManager.Instance.CloseCurrentActivePanel();
    }


}
