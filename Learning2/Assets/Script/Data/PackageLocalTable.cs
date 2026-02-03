using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public async Task SavePackageData()
    {
        var wrapper = new PackageWrapper{ data = LocalTables};
        await LocalDataSetting.SavelByJson(saveFileName, wrapper);
    }
    /// <summary>
    /// 读取背包数据
    /// </summary>
    public void LoadPackageData()
    {
        var wrapper = LocalDataSetting.LoadFromJson<PackageWrapper>(saveFileName);
        LocalTables = wrapper != null ? wrapper.data : new List<PackageLocalTableData>();
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
    public async Task<bool> RemoveItem(string itemUID , bool canSave = true)
    {
        for(int i =0; i< LocalTables.Count; i++)
        {
            if(LocalTables[i].itemUID == itemUID)
            {
                LocalTables.RemoveAt(i);

                if(canSave) await SavePackageData();
                return true;
            }
        }
        return false;
    }
/// <summary>
/// 清空背包
/// </summary>
/// <returns></returns>
    public async Task<bool> ClearnPackage()
    {
        LocalTables.Clear();
        await SavePackageData();
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
    public int itemID = 0;
    public string itemUID = string.Empty;
    public bool isEquip = false;
    public int itemLevel = 1;
    public int itemCount =1 ;
    public int addAttackPower = 0;
    public int addDefense = 0;
    public int addMaxHP = 0;
    public int addMaxMP = 0;
}
