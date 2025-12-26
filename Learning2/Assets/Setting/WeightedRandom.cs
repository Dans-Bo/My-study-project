using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// 加权随机 （权重累加）
/// </summary>
public static class WeightedRandom 
{
    public static T SelectByWeight<T>(IEnumerable<KeyValuePair<T,int>> weightedItems)
    {
        //空值校验
        if(weightedItems == null)
        {
            throw new ArgumentNullException(nameof(weightedItems), "元素列表不能为空");
        }

        var itemList = weightedItems.ToList();
        if(itemList.Count == 0)
        {
            throw new ArgumentNullException(nameof(weightedItems), "元素列表不能为空");
        }

        //权重合法性校验
        if(itemList.Any(x => x.Value <0))
        {
            throw new ArgumentNullException(nameof(weightedItems), "权重值不能为负");
        }

        //计算权重总和
        int totalWeight = itemList.Sum(x => x.Value);
        if(totalWeight == 0)
        {
            throw new ArgumentNullException(nameof(weightedItems), "权重总和不能为0");
        }

        int randomValue = UnityEngine.Random.Range(0,totalWeight);
        
        //累加权重并匹配随机数
        int currentSum = 0;
        foreach(var item in itemList)
        {
            currentSum += item.Value;
            if(randomValue < currentSum)
            {
                return item.Key;
            }
        }

        return itemList.Last().Key;
    }
}
