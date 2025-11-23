using System;
using System.IO;
using UnityEngine;

public static class LocalDataSetting 
{
    public static void SavelByJson(string fileName , object data)
    {
        var json = JsonUtility.ToJson(data);
        var directoryPath = Application.persistentDataPath;
        var path = Path.Combine(directoryPath, fileName);

        try
        {
            if(! Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            File.WriteAllText(path, json);

            #if UNITY_EDITOR
            Debug.Log($"成功保存数据到{path}");
            #endif
        }
        catch (Exception e)
        {
            #if UNITY_EDITOR
            Debug.LogWarning($"保存数据失败{path}.\n{e.Message}");
            #endif
        }
    }

    public static T LoadFromJson<T>( string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);
            return data;
        }
        catch (Exception e)
        {
            #if UNITY_EDITOR
            Debug.LogWarning($"从路径读取失败{path}.\n{e.Message}");
            #endif
            return default;
        }
    }

    public static void DeleteSaveFile( string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath,fileName);

        try
        {
            File.Delete(path);
            
            #if UNITY_EDITOR
            Debug.Log($"成功删除数据{fileName}");
            #endif
        }
        catch(Exception e)
        {
            #if UNITY_EDITOR
            Debug.LogWarning($"删除数据失败{fileName} \n {e.Message}");
            #endif
        }
    }
}
