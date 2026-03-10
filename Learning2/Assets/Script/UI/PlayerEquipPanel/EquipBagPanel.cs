using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipBagPanel : MonoBehaviour
{
    [SerializeField] RectTransform uiCenter;
    [SerializeField] Button uiNextButton;
    [SerializeField] Button uiPreviousButton;
    [SerializeField] GameObject equipCellPrefab;
    private EquipmentPanel uiParent; //传给cell,用于cell打开物品详情面板

    [Header("分页设置")]
    [SerializeField] int itemsPerPage = 12; //每页显示的物品数量
    private int currentPage = 1; //当前页码
    private int totalPages ; //总页数
    private List<GameObject> cellPool = new(); //格子对象池

    void Awake()
    {
        uiParent = GetComponentInParent<EquipmentPanel>();
        if(uiParent == null)
        {
            Debug.LogError("获取EquipmentPanel组件失败");
        }
        InitCellPool();
        InitClick();
        Refresh();
      
    }

    private void InitCellPool()
    {
        if(equipCellPrefab == null || uiCenter == null)
        {
            Debug.Log($"装备格子预制件或uiCenter为空");
            return;
        }

        //先禁用组件，避免每次创建格子都触发一次
        
        if(!uiCenter.TryGetComponent<GridLayoutGroup>(out var gridLayout))
        {
            Debug.Log($"UICenter没有挂载GridLayoutGroup组件");
        }

        gridLayout.enabled = false;    

        for(int i = 0; i<itemsPerPage; i++)
        {
            var cell = Instantiate(equipCellPrefab, uiCenter);
            cell.SetActive(false);
            cellPool.Add(cell);
        }

        gridLayout.enabled = true;
    }
    private void Refresh() 
    {
        //先禁用组件，避免每次创建格子都触发一次
        GridLayoutGroup gridLayout = uiCenter.GetComponent<GridLayoutGroup>();
        if(gridLayout == null)
        {
            Debug.Log($"UICenter没有挂载GridLayoutGroup组件");
        }

        gridLayout.enabled = false;    

        foreach(var cell in cellPool)
        {
            if(cell != null) cell.SetActive(false);
            
        }

        List<PackageLocalTableData> equips = PackageDataManage.Instance.EquipItems;
        if(equips.Count == 0) return;

        totalPages = Mathf.CeilToInt((float) equips.Count / itemsPerPage); //向上取整
        currentPage = Mathf.Clamp(currentPage, 1, totalPages); //确保页码在有效范围内

        //计算当前页面显示的物品索引范围
        int startIndex = (currentPage - 1) * itemsPerPage; //起始索引
        int endIndex = Mathf.Min(startIndex + itemsPerPage , equips.Count); //结束索引

        for(int i =0 ;i < cellPool.Count; i++)
        {
            //计算当前格子对应的装备数据索引
            int dataIndex = startIndex + i ;
            GameObject cell = cellPool[i];

            if(dataIndex < endIndex)
            {
                cell.SetActive(true);
                EquipCell equipCell = cell.GetComponent<EquipCell>();
                equipCell.Refresh(equips[dataIndex],uiParent);
            }
            else cell.SetActive(false); //没有数据则禁用格子
        }
        //重新启用布局
        gridLayout.enabled = true;
        
        uiNextButton.gameObject.SetActive(currentPage != totalPages);
        uiPreviousButton.gameObject.SetActive(currentPage != 1);

    }

    private void InitClick()
    {
        uiNextButton.onClick.AddListener(OnNext);
        uiPreviousButton.onClick.AddListener(OnPrevious);
    }

    private void OnNext()
    {
        currentPage = Mathf.Clamp(currentPage + 1, 1, totalPages);
        Refresh();
    }

    private void OnPrevious()
    {
        currentPage = Mathf.Clamp(currentPage - 1, 1 , totalPages);
        Refresh();
    }
}
