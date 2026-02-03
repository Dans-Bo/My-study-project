using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EquipmentPanel : BasePanel
{
    [Header("关闭按钮")]
    [SerializeField] private Button uiCloseButton;
    [Header("装备按钮")]
    [SerializeField] private Button uiWeaponButton; 
    [SerializeField] private Button uiArmorButton; 
    [SerializeField] private Button uiHelmetButton; 
    [SerializeField] private Button uiNecklaceButton;  
    [SerializeField] private Button uiRingButton;
    [SerializeField] private Button uiShoeButton;
    
    [Header("装备图标")]
    [SerializeField] private Image  uiWeaponIcon;
    [SerializeField] private Image  uiArmorIcon;
    [SerializeField] private Image  uiHelmetIcon;
    [SerializeField] private Image  uiNecklaceIcon;
    [SerializeField] private Image  uiRingIcon;
    [SerializeField] private Image  uiShoeIcon;
    [Header("属性显示")]
    [SerializeField] private TextMeshProUGUI uiHpText;
    [SerializeField] private TextMeshProUGUI uiMpText;
    [SerializeField] private TextMeshProUGUI uiAttackPowerText;
    [SerializeField] private TextMeshProUGUI uiDefenceText;
    [SerializeField] private TextMeshProUGUI uiExpText;
    [SerializeField] private TextMeshProUGUI uiLevelText;
    [Header("装备背包按钮")]
    [SerializeField] private Button EquipBagButton;
    

    [SerializeField] private PlayerEquipmentManager equipmentManager;
    [SerializeField] private PlayerAttributeManager attrManager;
    [SerializeField] private GameObject itemDetailsPanelPrefab;
    private ItemDetailsPanal itemDetails;
    private Canvas panelCanvas;


    protected override void  Awake()
    {
        InitClick();
        itemDetailsPanelPrefab.SetActive(false);
        //刷新图标
        if(equipmentManager == null)
        {
            GameObject obj = GameObject.FindWithTag("Player");
            equipmentManager = obj.GetComponent<PlayerEquipmentManager>();
        }

         if(attrManager == null)
        {
            GameObject obj = GameObject.FindWithTag("Player");
            attrManager = obj.GetComponent<PlayerAttributeManager>();
        }
        Refresh();
    }

    void Start()
    {
        itemDetails = itemDetailsPanelPrefab.GetComponent<ItemDetailsPanal>();
        if(!UIManager.Instance.UIRoot.TryGetComponent<Canvas>(out panelCanvas)) Debug.LogError("获取canvas失败"); 
    
    }
    #region  装备&卸载事件
    void OnEnable()
    {
        if(equipmentManager != null)
        {
            equipmentManager.OnEquipmentEquipped += OnEquip;
            equipmentManager.OnEquipmentUnequipped += OnUnequip;
        } 
    }

    void OnDisable()
    {
        if(equipmentManager != null)
        {
            equipmentManager.OnEquipmentEquipped -= OnEquip;
            equipmentManager.OnEquipmentUnequipped -= OnUnequip;
        }

    }

    private void OnUnequip(EquipmentType type, PackageLocalTableData data)
    {
        PackageTableData tableData = PackageDataManage.Instance.GetPackageItem_ByID(data.itemID);
        switch(type)
        {
                case EquipmentType.Weapon:
                    uiWeaponIcon.sprite = tableData.itemIcon;
                    break;
                case EquipmentType.Helmet:
                    uiHelmetIcon.sprite = tableData.itemIcon;
                    break;
                case EquipmentType.Armor:
                    uiArmorIcon.sprite = tableData.itemIcon;
                    break;
                case EquipmentType.Ring:
                    uiRingIcon.sprite = tableData.itemIcon;
                    break;
                case EquipmentType.Necklace:
                    uiNecklaceIcon.sprite = tableData.itemIcon;
                    break;
                case EquipmentType.Shoe:
                    uiShoeIcon.sprite = tableData.itemIcon;
                    break;
            }
        RefreshAttr();
    }

    private void OnEquip(EquipmentType type, PackageLocalTableData data)
    {
        switch(type)
        {
                case EquipmentType.Weapon:
                    uiWeaponIcon.sprite = default;
                    break;
                case EquipmentType.Helmet:
                    uiHelmetIcon.sprite = default;
                    break;
                case EquipmentType.Armor:
                    uiArmorIcon.sprite = default;
                    break;
                case EquipmentType.Ring:
                    uiRingIcon.sprite = default;
                    break;
                case EquipmentType.Necklace:
                    uiNecklaceIcon.sprite = default;
                    break;
                case EquipmentType.Shoe:
                    uiShoeIcon.sprite = default;
                    break;
            }
        RefreshAttr();
    }
#endregion
#region  UI按钮事件
    private void InitClick()
    {
        uiCloseButton.onClick.AddListener(OnClosePanel);
        uiArmorButton.onClick.AddListener(OnClickArmor);
        uiHelmetButton.onClick.AddListener(OnClickHelmet);
        uiWeaponButton.onClick.AddListener(OnClickWeapon);
        uiRingButton.onClick.AddListener(OnClickRing);
        uiShoeButton.onClick.AddListener(OnClickShoe);
        uiNecklaceButton.onClick.AddListener(OnClickNecklace);
    }

    private void OnClickNecklace()
    {
        if(uiNecklaceIcon == null) return;
        PackageLocalTableData item = equipmentManager.GetEquipItem(EquipmentType.Necklace);
        OpenDetailPanel(item);
        Debug.Log($"查看项链装备");

    }

    private void OnClickShoe()
    {
        if(uiNecklaceIcon == null) return;
        PackageLocalTableData item = equipmentManager.GetEquipItem(EquipmentType.Shoe);
        OpenDetailPanel(item);
        Debug.Log($"查看鞋子装备");
    }

    private void OnClickRing()
    {
        if(uiNecklaceIcon == null) return;
        PackageLocalTableData item = equipmentManager.GetEquipItem(EquipmentType.Ring);
        OpenDetailPanel(item);
        Debug.Log($"查看戒指装备");
    }

    private void OnClickWeapon()
    {
        if(uiNecklaceIcon == null) return;
        PackageLocalTableData item = equipmentManager.GetEquipItem(EquipmentType.Weapon);
        OpenDetailPanel(item);
        Debug.Log($"查看武器装备");
    }

    private void OnClickHelmet()
    {
        if(uiNecklaceIcon == null) return;
        PackageLocalTableData item = equipmentManager.GetEquipItem(EquipmentType.Helmet);
        OpenDetailPanel(item);
        Debug.Log($"查看盔甲装备");
    }

    private void OnClickArmor()
    {
        if(uiNecklaceIcon == null) return;
        PackageLocalTableData item = equipmentManager.GetEquipItem(EquipmentType.Armor);
        OpenDetailPanel(item);
        Debug.Log($"查看头盔装备");
    }

    /// <summary>
    /// 关闭面板
    /// </summary>
    private void OnClosePanel()
    {
        UIManager.Instance.CloseCurrentActivePanel();
    }

#endregion

/// <summary>
/// 刷新面板
/// </summary>
    public void Refresh()
    {
        var equipItems = equipmentManager.equipItems;
        foreach(var item in equipItems)
        {
            PackageLocalTableData data = item.Value;
            PackageTableData tableData = PackageDataManage.Instance.GetPackageItem_ByID(data.itemID);
            //更新对应的图标
            switch(item.Key)
            {
                case EquipmentType.Weapon:
                    uiWeaponIcon.sprite = tableData.itemIcon;
                    break;
                case EquipmentType.Helmet:
                    uiHelmetIcon.sprite = tableData.itemIcon;
                    break;
                case EquipmentType.Armor:
                    uiArmorIcon.sprite = tableData.itemIcon;
                    break;
                case EquipmentType.Ring:
                    uiRingIcon.sprite = tableData.itemIcon;
                    break;
                case EquipmentType.Necklace:
                    uiNecklaceIcon.sprite = tableData.itemIcon;
                    break;
                case EquipmentType.Shoe:
                    uiShoeIcon.sprite = tableData.itemIcon;
                    break;
            }
        }
        RefreshAttr();
    }
/// <summary>
/// 刷新属性数值
/// </summary>
    private void RefreshAttr()
    {
        uiHpText.text = $"{attrManager.GetAttribute(PlayerAttribute.HP)}/{attrManager.GetAttribute(PlayerAttribute.MaxHP)}";
        uiMpText.text = $"{attrManager.GetAttribute(PlayerAttribute.MP)}/{attrManager.GetAttribute(PlayerAttribute.MaxMP)}";
        uiExpText.text = $"{attrManager.GetAttribute(PlayerAttribute.Exp)}/{attrManager.GetAttribute(PlayerAttribute.MaxExp)}";
        uiDefenceText.text =  $"{attrManager.GetAttribute(PlayerAttribute.Defense)}";
        uiLevelText.text = $"{attrManager.GetAttribute(PlayerAttribute.Level)}";
        uiAttackPowerText.text = $"{attrManager.GetAttribute(PlayerAttribute.Attack)}";
    }
/// <summary>
/// 打开物品详情界面
/// </summary>
    public void OpenDetailPanel(PackageLocalTableData item)
    {   
        if(item == null) return;
        itemDetailsPanelPrefab.SetActive(true);
        itemDetails.Refresh(item,this);

        Vector2 mousePos = Mouse.current.position.ReadValue();
        itemDetails.SetPanelPos(panelCanvas.transform as RectTransform ,mousePos);
     
    }
}
