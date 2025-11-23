using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包物品静态列表
/// </summary>
[CreateAssetMenu (fileName = "PackageData" , menuName = "Data/PackageData" )]
public class PackageTable : ScriptableObject
{
    public List<PackageTableData> packageTableDatas;
}

/// <summary>
/// 物品静态数据
/// </summary>
[System.Serializable]
public class PackageTableData
{
    public int itemID;
    public string itemName;
    public ItemType itemType;
    public string itemDetails;
    public int itemMaxStackSize; //最大堆叠数
    public Sprite itemIcon;
}

/// <summary>
/// 物品类型
/// </summary>
public enum ItemType
{
    equipment,food,potion,material
}
