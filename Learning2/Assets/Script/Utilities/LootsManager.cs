using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootsManager
{
    public PackageTable PackageTable{get; private set;}
    private static LootsManager _instances;
    public static LootsManager Instances
    {
        get
        {
            _instances ??= new LootsManager();
            return _instances;
        }
    }

    private LootsManager()
    {
        PackageTable = Resources.Load<PackageTable>("Data/PackageData/PackageData");
        if (PackageTable == null)
        {
            Debug.LogError($"加载PackageTable失败！请检查");
        }
    }

     /// <summary>
    /// 物品类型生成权重
    /// </summary>
    private Dictionary<ItemType,int> typeWeights = new()
    {
      {ItemType.equipment,30},
      {ItemType.potion,20},
      {ItemType.food,15},
      {ItemType.material,25}  
    };

    /// <summary>
    /// 随机物品生成
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public PackageLocalTableData GenerateRandomItem()
    {
        if(PackageTable == null|| PackageTable.packageTableDatas == null || PackageTable.packageTableDatas.Count == 0)
        {
            throw new ArgumentException("物品数据库为空，配置PackageTable数据");
        }

        //随机选择物品
        ItemType randomType = WeightedRandom.SelectByWeight(typeWeights);

        //筛选类型下的所有物品
        List<PackageTableData> typeItems = PackageTable.packageTableDatas
            .Where(data => data.itemType == randomType).ToList();

        if(typeItems.Count == 0)
        {
            throw new InvalidOperationException($"类型 {randomType} 下没有配置物品数据");
        }

        //将物品转换为物品-权重键值对
        var weightItems = typeItems.Select(data => new KeyValuePair<PackageTableData,int>(data,data.weight));

        //在该类型下选择一个物品
        var item = WeightedRandom.SelectByWeight(weightItems);

        PackageLocalTableData localItem = new()
        {
            itemID = item.itemID,
            itemUID = Guid.NewGuid().ToString(),
            itemCount = 1
        };  

        return localItem;     
    }

    public PackageTableData GetTableDataByID(int id)
    {
        List<PackageTableData> data = PackageTable?.packageTableDatas;

        if(data != null)
        { 
            foreach(var item in data)
            {
                if(item.itemID == id) return item;
            }
        }

        return null;

        //return PackageTable?.packageTableDatas?.FirstOrDefault(data => data.itemID == id);
    }
}
