using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAttributeManager))]
/// <summary>
/// 玩家装备管理
/// </summary>
public class PlayerEquipmentManager : MonoBehaviour
{
    private PlayerAttributeManager attrManager;
    //已装备物品
    public Dictionary<EquipmentType,PackageLocalTableData> equipItems {get ; private set;} = new();

    /// <summary>
    /// 装备事件
    /// </summary>
    public event Action<EquipmentType,PackageLocalTableData> OnEquipmentEquipped;
    /// <summary>
    /// 装备卸载事件
    /// </summary>
    public event Action<EquipmentType, PackageLocalTableData> OnEquipmentUnequipped;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //TODO 存储记录装备，初始读取存档文件
    }

    void Start()
    {
        attrManager = GetComponent<PlayerAttributeManager>();
    }
    /// <summary>
    /// 装备物品
    /// </summary>
    /// <param name="item"></param>
    public bool Equip(PackageLocalTableData item)
    {
        if(item == null) return false;

        PackageTableData tableData = PackageDataManage.Instance.GetPackageItem_ByID(item.itemID);
        if(tableData == null || tableData.itemType != ItemType.equipment)
        {
            Debug.LogWarning("非装备物品无法装备或数据不存在");
            return false;
        }

        if(tableData.equipType == EquipmentType.None)
        {
            Debug.LogWarning($"装备{tableData.itemName} 不是有效装备");
            return false;
        }

        //如果已经有装备，先卸载再装备
        if(equipItems.ContainsKey(tableData.equipType))
        {
            Unequip(tableData.equipType);
        }

        //标记为已装备
        item.isEquip = true;
        //存入装备槽
        equipItems[tableData.equipType] = item; 
        //触发装备事件(ui更新)
        OnEquipmentEquipped?.Invoke(tableData.equipType, item);
        //TODO 更新玩家属性
        attrManager.ModifyAttribute(PlayerAttribute.Attack,item.addAttackPower);
        attrManager.ModifyAttribute(PlayerAttribute.Defense,item.addDefense);
        attrManager.ModifyAttribute(PlayerAttribute.MaxHP,item.addMaxHP);
        attrManager.ModifyAttribute(PlayerAttribute.MaxMP,item.addMaxMP);

        Debug.Log($"装备成功：{tableData.itemName}，槽位：{tableData.equipType}");
        return true;
    }

/// <summary>
/// 卸载指定槽位装备
/// </summary>
/// <param name="type"></param>
/// <exception cref="NotImplementedException"></exception>
    public bool Unequip(EquipmentType type)
    {
        if(!equipItems.TryGetValue(type , out PackageLocalTableData item))
        {
            Debug.LogWarning($"当前槽位{type}无装备");
            return false;
        }

        // 标记物品为未装备
        item.isEquip = false;
        //移除装备
        equipItems.Remove(type);
        OnEquipmentUnequipped?.Invoke(type, item);

        //TODO 移除玩家该装备的属性加成
        attrManager.ModifyAttribute(PlayerAttribute.Attack,-item.addAttackPower);
        attrManager.ModifyAttribute(PlayerAttribute.Defense,-item.addDefense);
        attrManager.ModifyAttribute(PlayerAttribute.MaxHP,-item.addMaxHP);
        attrManager.ModifyAttribute(PlayerAttribute.MaxMP,-item.addMaxMP);

        Debug.Log($"卸载槽位：{type} 装备成功，");
        return true;
    }

    /// <summary>
    /// 获取所有已装备的物品（同步背包物品的状态）
    /// </summary>
    /// <returns></returns>
    public Dictionary<EquipmentType,PackageLocalTableData> GetAllEquippedItem()
    {
        //返回已装备物品的副本
        return new Dictionary<EquipmentType, PackageLocalTableData>(equipItems);
    }
/// <summary>
/// 根据装备类型获取装备
/// </summary>
/// <param name="type"></param>
/// <returns></returns>
    public PackageLocalTableData GetEquipItem(EquipmentType type)
    {
        if(equipItems.ContainsKey(type))
        {
            var item = equipItems[type];
            return item;
        }

        return null;
    }
}
