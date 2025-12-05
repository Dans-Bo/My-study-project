using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 箱子掉落配置列表
/// </summary>
[CreateAssetMenu(fileName = "BoxData",menuName = "Data/BoxData")]
public class BoxDataSO: ScriptableObject
{
    public List<LootsConfig> lootsConfigs;
}

/// <summary>
/// 箱子掉落配置
/// </summary>
[Serializable]
public class LootsConfig
{
    public ItemType lootType;
    public List<int> itemIDList; //箱子可掉落的ID物品
    public int minCount; //最小掉落数
    public int maxCount; //最大掉落数
    public float dropProbability = 1f; //掉落概率(0-1) 1必掉
}
