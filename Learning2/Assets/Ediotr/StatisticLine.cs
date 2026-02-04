using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class StatisticLine : MonoBehaviour
{
 [MenuItem("输出总代码行数/输出")]
    private static void PrintTotalLine()
    {
        string[] fileName = AssetDatabase.FindAssets("t:Script", new string[] { "Assets/Script" });

        int totalLine = 0;
        int fileNum = 0;
        foreach (var temp in fileName)
        {
            ++ fileNum;
            int nowLine = 0;
            string path = AssetDatabase.GUIDToAssetPath(temp);
            StreamReader sr = new StreamReader(path);
            while (sr.ReadLine() != null)
            {
                nowLine++;
            }

            //文件名+文件行数
            //Debug.Log(String.Format("{0}——{1}", path, nowLine));

            totalLine += nowLine;
        }

        Debug.Log(String.Format($"总代码行数：{totalLine} , 共有代码文件：{fileNum} 个" ));
    }

}
