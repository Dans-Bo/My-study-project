using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class LootsManager
{
    public PackageTable PackageTable{get; private set;}
    private static LootsManager _instance;
    public static LootsManager Instance
    {
        get
        {
            _instance ??= new LootsManager();
            return _instance;
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
    /// 单次随机物品生成(根据角色等级随机属性)
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public PackageLocalTableData GenerateRandomItem()
    {
       int playerLevel = 1;
       PlayerAttributeManager playerAttr = GameObject.FindObjectOfType<PlayerAttributeManager>();
       if(playerAttr != null)
        {
            playerLevel =Mathf.RoundToInt(playerAttr.GetAttribute(PlayerAttribute.Level));
            playerLevel = Mathf.Max(playerLevel , 1);
        }  

        return GenerateRandomItem(playerLevel); 
    }
/// <summary>
/// 多次随机物品生成(根据角色等级随机属性)
/// </summary>
/// <param name="playerLevel"></param>
/// <returns></returns>
/// <exception cref="ArgumentException"></exception>
/// <exception cref="InvalidOperationException"></exception>
    public PackageLocalTableData GenerateRandomItem(int playerLevel)
    {
        if(PackageTable == null|| PackageTable.packageTableDatas == null || PackageTable.packageTableDatas.Count == 0)
        {
            throw new ArgumentException("物品数据库为空，配置PackageTable数据");  //当方法的参数值无效时
        }

        ItemType randomType = WeightedRandom.SelectByWeight(typeWeights);

        //筛选类型下的所有物品
        List<PackageTableData> typeItems = PackageTable.packageTableDatas
            .Where(data => data.itemType == randomType).ToList();

        if(typeItems.Count == 0)
        {
            throw new InvalidOperationException($"类型 {randomType} 下没有配置物品数据");   //当对象的操作在当前对象状态下无效时
        }

         //将物品转换为物品-权重键值对
        var weightItems = typeItems.Select(data => new KeyValuePair<PackageTableData,int>(data,data.weight));

        //在该类型下选择一个物品
        var item = WeightedRandom.SelectByWeight(weightItems);

        playerLevel = Mathf.Max(playerLevel, 1);
        int itemLevel = 1;
        int addAttack =0;
        int addDefense = 0;
        int addMaxHP = 0;
        int addMaxMP = 0;

        if(randomType == ItemType.equipment)
        {
            itemLevel = UnityEngine.Random.Range(playerLevel -5 , playerLevel + 5);
            addAttack = Mathf.CeilToInt( UnityEngine.Random.Range(1,3) + UnityEngine.Random.Range(0.5f,1.2f)* itemLevel);
            addDefense =Mathf.CeilToInt( UnityEngine.Random.Range(0,2) + UnityEngine.Random.Range(0.3f,0.9f) *itemLevel);
            addMaxHP = Mathf.CeilToInt(UnityEngine.Random.Range(5,10) + UnityEngine.Random.Range(2,4) *itemLevel);
            addMaxMP =Mathf.CeilToInt(UnityEngine.Random.Range(3,8) + UnityEngine.Random.Range(1,3) * itemLevel);
        }

        PackageLocalTableData localItem = new()
        {
            itemID = item.itemID,
            itemUID = Guid.NewGuid().ToString(),
            itemLevel = itemLevel,
            itemCount = 1,
            addAttackPower = addAttack,
            addDefense = addDefense,
            addMaxHP = addMaxHP,
            addMaxMP = addMaxMP,
        };  

        return localItem;  

    }


/// <summary>
/// 查找物品根据ID
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
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
