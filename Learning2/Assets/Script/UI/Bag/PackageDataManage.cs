using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PackageDataManage 
{
    private static PackageDataManage _instance;
    public static PackageDataManage Instance
    {
        get
        {
            _instance ??= new PackageDataManage();
            return _instance;
        }
    }

    private PackageDataManage()
    {
        UpdateCachedPackageData();
        GetPackageData();

//静态字典存储
/*         itemIdDataDic = new();
        foreach(var data in PackageTable.packageTableDatas)
        {
            if(!itemIdDataDic.ContainsKey(data.itemID))
            {
                itemIdDataDic.Add(data.itemID, data);
            }
        } */
    }

    public PackageTable PackageTable{ get;private set; } //静态信息
    public List<PackageLocalTableData> CachedPackageData{ get;private set;} //背包物品缓存池

//TODO 物品数量多时，使用字典优化查找效率
   /*  private Dictionary<int,PackageTableData> itemIdDataDic; // ID - 静态数据
    private Dictionary<string , PackageLocalTableData> itemUidDataDic; //UID - 动态数据 */

/// <summary>
/// 更新动态数据缓存
/// </summary>
    public void UpdateCachedPackageData()
    {
        CachedPackageData = PackageLocalTable.Instance.LocalTables;

        /* itemUidDataDic = new();
        foreach(var data in CachedPackageData)
        {
            if(!itemUidDataDic.ContainsKey(data.itemUID))
            {
                itemUidDataDic.Add(data.itemUID,data);
            }
        } */
    }
/// <summary>
/// 获取背包静态数据
/// </summary>
/// <returns></returns>
    public void GetPackageData()
    {
        if(PackageTable == null)
        {
            PackageTable = Resources.Load<PackageTable>("Data/PackageData/PackageData");
        }
        /* if(PackageTable == null)
            Debug.Log("静态数据缓存为空");
         */
    }

/// <summary>
/// 根据ID查找物品
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
    public PackageTableData GetPackageItem_ByID(int id)
    {
       /*  itemIdDataDic.TryGetValue(id, out var data);
        return data; */

        var items = PackageTable.packageTableDatas;
        foreach(var item in items)
        {
            if(item.itemID == id) return item;
        }
        return null; 
    }
/// <summary>
/// 根据UID查找物品
/// </summary>
/// <param name="uid"></param>
/// <returns></returns>
    public PackageLocalTableData GetPackageItem_ByUID(string uid)
    {
        /* itemUidDataDic.TryGetValue(uid,out var data);
        return data; */

        var items = CachedPackageData;
        foreach(var item in items)
        {
            if(item.itemUID == uid) return item;
        } 
        return null ; 
    }
/// <summary>
/// 根据物品类型查找物品
/// </summary>
/// <param name="type"></param>
/// <returns></returns>
    public List<PackageTableData> GetPackageItem_ByType(ItemType type)
    {
        var tableItem = new List<PackageTableData>();
        var items = PackageTable.packageTableDatas;

        foreach(var item in items)
        {
            if(item.itemType == type) tableItem.Add(item);
        }
        return tableItem;
    }
/// <summary>
/// 删除单个物品
/// </summary>
/// <param name="uid"></param>
    public void DelectItem(string uid )
    {
        PackageLocalTableData data = GetPackageItem_ByUID(uid);
        if(data == null) return;
        //减少数量，若为0，则删除，剩余仅保存
        data.itemCount --;
        if(data.itemCount <= 0)
        {
            _ = PackageLocalTable.Instance.RemoveItem(uid, true);
        }else  _= PackageLocalTable.Instance.SavePackageData();

        //PackageLocalTable.Instance.RemoveItem(uid,true);
        UpdateCachedPackageData();
    }
/// <summary>
/// 删除多个物品
/// </summary>
/// <param name="uids"></param>
    public void DelectItems(List<string> uids)
    {
        foreach(string uid in uids)
        {
            PackageLocalTableData data = GetPackageItem_ByUID(uid);
            if(data == null)
            {
                Debug.LogWarning($"删除失败，未找到UID为【{uid}】的物品");
                continue;
            }

            data.itemCount -- ;
            if(data.itemCount <= 0)
            {
                _ = PackageLocalTable.Instance.RemoveItem(uid,false);
            }            
        }
        _ =PackageLocalTable.Instance.SavePackageData();
        UpdateCachedPackageData();
    }
/// <summary>
/// 直接删除物品，用于丢弃功能
/// </summary>
/// <param name="uid"></param>
    public void DeleteItem_Whole(string uid)
    {
        PackageLocalTableData data = GetPackageItem_ByUID(uid);
        if(data == null) return;
        _ = PackageLocalTable.Instance.RemoveItem(uid,true);
        UpdateCachedPackageData();
    }
/// <summary>
/// 添加非装备类物品（可堆叠）
/// </summary>
/// <param name="itemID"></param>
/// <param name="count"></param>
    public void AddItem(int itemID, int count = 1)
    {
        PackageTableData tableData = GetPackageItem_ByID(itemID);
        if(tableData == null) return;

        //查找同ID且可堆叠的现有物品
        PackageLocalTableData localData = null;
        

        foreach(PackageLocalTableData Data in CachedPackageData)
        {
            if(Data.itemID == itemID &&Data.itemCount  < tableData.itemMaxStackSize)
            {
                localData = Data;
                break;
            }
        }

        if(localData != null)
        {
            //可堆叠，直接增加数量（不超过上限）
            int addable = Mathf.Min(count, tableData.itemMaxStackSize - localData.itemCount);
            localData.itemCount += addable;
            count -= addable;
        }
        //剩余数量重新创建物品实例（还有剩余继续堆叠）
        while(count > 0 && tableData.itemMaxStackSize > 0)
        {
            PackageLocalTableData newItem = new PackageLocalTableData
            {
                itemID = itemID,
                itemUID = Guid.NewGuid().ToString(),
                itemCount = Mathf.Min(count,tableData.itemMaxStackSize)
            };
            PackageLocalTable.Instance.AddItem(newItem);
            count -= newItem.itemCount;
        }
        UpdateCachedPackageData();
        _ = PackageLocalTable.Instance.SavePackageData();
    }
/// <summary>
/// 添加物品
/// </summary>
/// <param name="item"></param>
    public void AddItem(PackageLocalTableData item,bool canSave = true)
    {
        if(item == null) return;
        if(item.itemCount <0) return;
        PackageTableData tableData = GetPackageItem_ByID(item.itemID);
        if(tableData == null)
        {
            Debug.LogWarning($"放入背包失败，物品ID{item.itemID}不存在");
            return;
        }

        PackageLocalTableData stackableItem = null;
        foreach(PackageLocalTableData existItem in CachedPackageData)
        {
            //堆叠条件，同ID + 当前数量 < 最大堆叠数
            if(existItem.itemID == item.itemID && existItem.itemCount < tableData.itemMaxStackSize)
            {
                stackableItem = existItem;
                break;
            }
        }

        //可堆叠则叠加数量，不可堆叠则直接添加
        if(stackableItem != null)
        {
            //叠加数量
            int addAbleCount = Mathf.Min(item.itemCount, tableData.itemMaxStackSize - stackableItem.itemCount);
            stackableItem.itemCount += addAbleCount;
            int remainCount = item.itemCount - addAbleCount;

            //如果还有剩余，就创建新的格子
            if(remainCount >0)
            {
                PackageLocalTableData newItem = new PackageLocalTableData()
                {
                    itemID = item.itemID,
                    itemUID = item.itemUID,
                    itemCount = remainCount,
                    itemLevel = item.itemLevel,
                    addAttackPower = item.addAttackPower,
                    addDefense = item.addDefense,
                    addMaxHP = item.addMaxHP,
                    addMaxMP = item.addMaxMP
                };
                PackageLocalTable.Instance.AddItem(newItem);
            }
        }
        else
        {
            PackageLocalTable.Instance.AddItem(item);
        }

        if(canSave)
        {
            UpdateCachedPackageData();
            _=PackageLocalTable.Instance.SavePackageData();
        }
        
    }
/// <summary>
/// 添加全部物品
/// </summary>
/// <param name="itemList"></param>
    public void AddItems(List<PackageLocalTableData> itemList)
    {
        if(itemList == null || itemList.Count == 0)
        {
            Debug.LogWarning($"批量添加物品失败，列表为空");
            return;
        }

        PackageTableData tableData = null;
        foreach(var item in itemList )
        {
            if (item == null || item.itemCount <= 0) continue;
            tableData = GetPackageItem_ByID(item.itemID);
            if(tableData == null)
            {
                Debug.LogWarning($"ID为{item.itemID}的物品不存在，跳过该物品");
                continue;
            }

            AddItem(item,false);
        }

        UpdateCachedPackageData();
        _= PackageLocalTable.Instance.SavePackageData();

        #if UNITY_EDITOR
        Debug.Log($"宝箱批量拿取完成，共拿取{itemList.Count}个物品，已全部存入背包");
        #endif
    }

   
}
