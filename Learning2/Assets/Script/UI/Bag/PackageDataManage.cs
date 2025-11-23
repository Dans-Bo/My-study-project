using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        UpdataCachedPackageData();
        GetPackageData();
    }

    public PackageTable PackageTable{ get;private set; } //静态信息
    public List<PackageLocalTableData> CachedPackageData{ get;private set;} //背包物品缓存池

/// <summary>
/// 更新动态数据缓存
/// </summary>
    public void UpdataCachedPackageData()
    {
        CachedPackageData = PackageLocalTable.Instance.LocalTables;
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
            PackageLocalTable.Instance.RemoveItem(uid,true);
        }else PackageLocalTable.Instance.SavePackageData();

        //PackageLocalTable.Instance.RemoveItem(uid,true);
        UpdataCachedPackageData();
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
            if(data == null) return;

            data.itemCount -- ;
            if(data.itemCount <= 0)
            {
                PackageLocalTable.Instance.RemoveItem(uid,false);
            }            
        }
        PackageLocalTable.Instance.SavePackageData();
        UpdataCachedPackageData();
    }
/// <summary>
/// 添加物品（可堆叠）
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
        while(count > 0 & tableData.itemMaxStackSize > 0)
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
        UpdataCachedPackageData();
        PackageLocalTable.Instance.SavePackageData();

    }
}
