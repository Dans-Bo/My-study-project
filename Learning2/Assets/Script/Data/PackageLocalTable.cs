using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

/// <summary>
/// 动态数据列表操作
/// </summary>
[Serializable]
public class PackageLocalTable 
{
    private const string saveFileName = "PackageData.sav";
    private static PackageLocalTable _instance;
    public static PackageLocalTable Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PackageLocalTable();
            }
            return _instance;
        }
    }

    private PackageLocalTable()
    {
        LocalTables = new List<PackageLocalTableData>();
        
        LoadPackageData();
    }

    //private List<PackageLocalTableData> _localDatas ;
    public List<PackageLocalTableData> LocalTables {get; private set;}


    /// <summary>
    /// 保存背包数据
    /// </summary>
    public void SavePackageData()
    {
        var wrapper = new PackageWrapper{ data = LocalTables};
        LocalDataSetting.SavelByJson(saveFileName, wrapper);
    }
    /// <summary>
    /// 读取背包数据
    /// </summary>
    public void LoadPackageData()
    {
        var wrapper = LocalDataSetting.LoadFromJson<PackageWrapper>(saveFileName);
        LocalTables = wrapper.data;
    }
/// <summary>
/// 添加物品
/// </summary>
/// <param name="data"></param>
    public void AddItem(PackageLocalTableData data)
    {
        if(data == null || string.IsNullOrEmpty(data.itemUID)) return;

        LocalTables.Add(data);
    }
/// <summary>
/// 移除物品
/// </summary>
/// <param name="itemUID"></param>
/// <param name="canSave"></param>
/// <returns></returns>
    public bool RemoveItem(string itemUID , bool canSave = true)
    {

        foreach(PackageLocalTableData item in LocalTables )
        {
            if(item.itemUID ==itemUID )
            {
                LocalTables.Remove(item);

                if(canSave) SavePackageData();
                return true;
            }
        }
        return false;
    }

    public bool ClearnPackage()
    {
        LocalTables.Clear();
        
        return true;
    }

    [Serializable]
    private class PackageWrapper
    {
       public List<PackageLocalTableData> data;
    }
}

/// <summary>
/// 动态数据参数
/// </summary>
[Serializable]
public class PackageLocalTableData
{
    public int itemID;
    public string itemUID;
    public int itemCount;
}
