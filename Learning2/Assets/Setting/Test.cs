using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Test1();
        //Test2();
        for(int i = 0;i<10;i++)
        {
           var item = GenerateRandomItem();

           Debug.Log($"物品ID{item.itemID} 物品名称：{item.itemName}");
            
        }

    }

    private void Test1()
    {
        var lotteryItems = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("一等奖（1%）", 1),
            new KeyValuePair<string, int>("二等奖（9%）", 9),
            new KeyValuePair<string, int>("三等奖（90%）", 90)
        };

        // 测试10000次验证分布
        Dictionary<string, int> resultCount = new Dictionary<string, int>
        {
            { "一等奖（1%）", 0 },
            { "二等奖（9%）", 0 },
            { "三等奖（90%）", 90 }
        };

        int testTimes = 10000;
        for (int i = 0; i < testTimes; i++)
        {
            string selected = WeightedRandom.SelectByWeight(lotteryItems);
            resultCount[selected]++;
        }

        // 输出到Unity控制台
        Debug.Log($"测试{testTimes}次的权重分布：");
        foreach (var item in resultCount)
        {
            Debug.Log($"{item.Key}：{item.Value}次（占比：{item.Value/(double)testTimes:P2}）");
        }
    }

    private void Test2()
    {
        var lotteryItems = new List<KeyValuePair<ItemType, int>>
        {
            new KeyValuePair<ItemType, int>(ItemType.equipment,20),
            new (ItemType.potion,40),
            new(ItemType.material,40),
        };

        ItemType selected = WeightedRandom.SelectByWeight(lotteryItems);
        Debug.Log(selected);

    }

    [SerializeField] private PackageTable packageTable; //物品数据库

    private Dictionary<ItemType,int> typeWeights = new()
    {
      {ItemType.equipment,30},
      {ItemType.potion,20},
      {ItemType.food,15},
      {ItemType.material,25}  
    };

    public PackageTableData GenerateRandomItem()
    {
        if(packageTable == null|| packageTable.packageTableDatas == null || packageTable.packageTableDatas.Count == 0)
        {
            throw new ArgumentException("物品数据库为空，配置PackageTable数据");
        }

        //随机选择物品
        ItemType randomType = WeightedRandom.SelectByWeight(typeWeights);

        //筛选类型下的所有物品
        List<PackageTableData> typeItems = packageTable.packageTableDatas
            .Where(data => data.itemType == randomType).ToList();

        if(typeItems.Count == 0)
        {
            throw new InvalidOperationException($"类型 {randomType} 下没有配置物品数据");
        }

        //将物品转换为物品-权重键值对
        var weightItems = typeItems.Select(data => new KeyValuePair<PackageTableData,int>(data,data.weight));

        //在该类型下选择一个物品
        return WeightedRandom.SelectByWeight(weightItems);
    }



}
