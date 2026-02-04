using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LootBoxPanel : BasePanel,IPointerClickHandler
{
    [SerializeField] private Button UICloseButton;
    [SerializeField] private GameObject lootCellPanelPrefab;
    [SerializeField] private RectTransform UILoots;
    [SerializeField] private Button UIGetAllLootsButton;
    [SerializeField] private GameObject lootConfirmPanelPrefab;
                     private LootsConfirmPanel lootsConfirm;
    List<PackageLocalTableData> boxLoots = new();
    private string _chooseUid = string.Empty;
    public string ChooseUid
    {
        get{return _chooseUid;}
        set{_chooseUid = value;}
    }
    
    protected override void Awake()
    {

        InitClick();
        lootsConfirm = lootConfirmPanelPrefab.GetComponent<LootsConfirmPanel>();
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
           var item = LootsManager.Instance.GenerateRandomItem();
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
        UIGetAllLootsButton.onClick.AddListener(OnGetAllLoots);
    }

    private void OnGetAllLoots()
    {
        CloseConfirmPanel();
        PackageDataManage.Instance.AddItems(boxLoots);
        boxLoots.Clear();
        RefreshBoxView();
    }

    private void OnClose()
    {
        UIManager.Instance.CloseCurrentActivePanel();
        lootConfirmPanelPrefab.SetActive(false);  
        // CloseConfirmPanel(); 
    }


    public void OpenConfirmPanel()
    {
        lootConfirmPanelPrefab.SetActive(true);

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Transform obj = UIManager.Instance.UIRoot;
        Canvas canvas = obj.GetComponent<Canvas>();
        if(canvas == null) Debug.LogError("获取canvas失败");

        lootsConfirm.SetPanelPos(canvas.transform as RectTransform , mousePos);
    }

    public void CloseConfirmPanel() => lootConfirmPanelPrefab.SetActive(false);

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            CloseConfirmPanel();
        }
    }
/// <summary>
/// 获取战利品到背包
/// </summary>
/// <returns></returns>
    public PackageLocalTableData PackageGetLoot()
    {
        if(string.IsNullOrEmpty(_chooseUid)) return null;
        foreach(var loot in boxLoots)
        {
            if(loot.itemUID == _chooseUid)
            {
                return loot;
            }
        }
        Debug.Log($"未找到该UID的战利品");
        return null;
    }
/// <summary>
/// 删除战利品
/// </summary>
/// <returns></returns>
    public bool RemoveLoot()
    {
        for(int i = boxLoots.Count -1; i>=0 ; i--)
        {
            if(boxLoots[i].itemUID == _chooseUid)
            {
                boxLoots.Remove(boxLoots[i]);
                _chooseUid = string.Empty;
                RefreshBoxView();
                return true;
            }
        }
        return false;
    }
}
