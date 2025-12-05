using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PackageTest 
{

    [MenuItem("GMCmd/读取背包静态数据")]
    public static void LoadPackage()
    {
        PackageTable table = PackageDataManage.Instance.PackageTable;

        if(table != null)
        {
            if(table.packageTableDatas != null)
            {
                foreach(PackageTableData data in table.packageTableDatas)
                {
                    Debug.Log(string.Format($"[id]: {data.itemID}\n [name]: {data.itemName}" ));
                }
            }else Debug.Log("空数据");   
        }else Debug.Log("加载失败，检查路径或文件");
    }
    [MenuItem("GMCmd/创建动态数据/依次创建")]
     public static void CreatLocalData()
    {
        //List<PackageLocalTableData> items = new List<PackageLocalTableData>();
        for (int i = 0; i < 17; ++i)
        {
            PackageLocalTableData itme = new PackageLocalTableData()
            {
                itemID = i,
                itemUID = Guid.NewGuid().ToString(),
                itemCount = i

            };
            PackageLocalTable.Instance.AddItem(itme);
        }
        PackageLocalTable.Instance.SavePackageData();
    } 
    [MenuItem("GMCmd/读取态数据")]
    public static void LoadLocalData()
    {
        PackageLocalTable.Instance.LoadPackageData();
       List<PackageLocalTableData> items = PackageLocalTable.Instance.LocalTables;
       for (int i = 0; i< items.Count; i++)
        {
            Debug.Log($"[ID]: {items[i].itemID} \n [uid]:{items[i].itemUID}");
        }
    }

    [MenuItem("GMCmd/打开背包")]
    public static void OpenPackagePanel()
    {
        UIManager.Instance.OpenPanel(ConstUIName.packagePanel);
    }

    [MenuItem("GMCmd/清空背包")]
    public static void ClearnPackage()
    {
        PackageLocalTable.Instance.ClearnPackage();
    }
    [MenuItem("GMCmd/创建动态数据/创建苹果")]
    public static void CreatApple()
    {
        PackageDataManage.Instance.AddItem(18,30);
    }
    [MenuItem("GMCmd/创建动态数据/创建药水")]
    public static void CreatPotion()
    {
        PackageDataManage.Instance.AddItem(15,20);
    }

    [MenuItem("GMCmd/打开音量设置")]
    public static void OpenAudioPanel()
    {
        UIManager.Instance.OpenPanel(ConstUIName.audioPanel);
    }
    [MenuItem("GMCmd/清除打开面板缓存")]
    public static void ClearOpenpanel()
    {
        UIManager.Instance.ClearnOpenPanelDict();
    }
}
