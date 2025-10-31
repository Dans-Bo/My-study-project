using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using AStar;
public class AStarMapEditor : EditorWindow
{

    private int maxX = 10;
    private int maxY = 10;
    private int blockSize = 1;
    private int blockHeight = 1;

    private Vector2 startPoint = Vector2.zero;
    private Vector2 endPoint = new Vector2(10, 10);
    private bool useCustomRange = false;

    private Dictionary<Vector2,GameObject> gridMap = new Dictionary<Vector2,GameObject>();
    

    [MenuItem("AStar/MapEditor")]

    static void Run()
    {
        GetWindow<AStarMapEditor>();
    }

    public void OnGUI()
    {
        GUIStyle customStyle = new(GUI.skin.label)
        {
            fontSize = 13,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset(10, 10, 5, 5),
            margin = new RectOffset(5, 5, 5, 5),
            fixedWidth = 120,
            fixedHeight = 25,
        };

        GUIStyle textFieldStyle = new(GUI.skin.label)
        {
            fixedWidth = 80,
            fixedHeight = 25,
        };

        #region 基础设置区

        EditorGUILayout.LabelField("基础设置", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("地图X轴块数", customStyle);
        maxX = Convert.ToInt32(GUILayout.TextField(maxX.ToString()));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("地图Y轴块数", customStyle);
        maxY = Convert.ToInt32(GUILayout.TextField(maxY.ToString()));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("地图块宽度", customStyle);
        blockSize = Convert.ToInt32(GUILayout.TextField(blockSize.ToString()));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("地图块高度", customStyle);
        blockHeight = Convert.ToInt32(GUILayout.TextField(blockHeight.ToString()));
        EditorGUILayout.EndHorizontal();
        #endregion
        #region 自定义范围
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("自定义范围", EditorStyles.boldLabel);

        useCustomRange = EditorGUILayout.Toggle("使用自定义范围", useCustomRange);

        if (useCustomRange)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("起始点 X", customStyle);
            startPoint.x = Convert.ToInt32(GUILayout.TextField(startPoint.x.ToString()));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("起始点 Y", customStyle);
            startPoint.y = Convert.ToInt32(GUILayout.TextField(startPoint.y.ToString()));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("结束点 X", customStyle);
            endPoint.x = Convert.ToInt32(GUILayout.TextField(endPoint.x.ToString()));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("结束点 Y", customStyle);
            endPoint.y = Convert.ToInt32(GUILayout.TextField(endPoint.y.ToString()));
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("根据范围计算网格数量"))
            {
                CalculateGridFromRange();
            }
        }
        //选择父节点
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("父节点", EditorStyles.boldLabel);
        if (Selection.activeGameObject != null)
        {
            EditorGUILayout.LabelField($"当前选中: {Selection.activeGameObject.name}", customStyle);
        }
        else
        {
            EditorGUILayout.HelpBox("请先选择一个GameObject作为网格的父节点", MessageType.Warning);
        }

        if (GUILayout.Button("重置地图块", GUILayout.Height(30)))
        {
            if (Selection.activeGameObject != null)
            {
                ResetBlock(Selection.activeGameObject);
            }
        }

        if (GUILayout.Button("清理地图块", GUILayout.Height(30)))
        {
            if (Selection.activeGameObject != null)
            {
                CleareBlock(Selection.activeGameObject);
            }
        }
        
        // 显示网格信息
        EditorGUILayout.Space();
        EditorGUILayout.LabelField($"当前网格数量: {gridMap.Count}", EditorStyles.helpBox);

        #endregion
        
        
        GUILayout.Label("选择地图原点", customStyle);
        if (Selection.activeGameObject != null)
        {
            GUILayout.Label(Selection.activeGameObject.name);
        }
        else
        {
            GUILayout.Label("没有选中节点，无法生成");
        }

        if (GUILayout.Button("生成地图块"))
        {
            if (Selection.activeGameObject != null)
            {
                CreateBlock(Selection.activeGameObject);
            }
        }

        if (GUILayout.Button("重置地图块"))
        {
            if (Selection.activeGameObject != null)
            {
                CleareBlock(Selection.activeGameObject);
            }
        }
        
    }
    
                
    private void CalculateGridFromRange() //根据自定义范围计算网格数量
    {
        float width = Mathf.Abs(endPoint.x - startPoint.x);
        float height = Mathf.Abs(endPoint.y - startPoint.y);
        
        maxX = Mathf.RoundToInt(width / blockSize);
        maxY = Mathf.RoundToInt(height / blockSize);
        
        // 确保至少有一个网格
        maxX = Mathf.Max(1, maxX);
        maxY = Mathf.Max(1, maxY);
        
        Debug.Log($"计算完成: 宽度 {width}, 高度 {height}, 网格 {maxX}x{maxY}");
    }

    private void ResetBlock(GameObject activeGameObject)
    {
        // 重置所有网格的颜色和状态
        foreach (var grid in gridMap.Values)
        {
            if (grid != null)
            {
                var renderer = grid.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.color = Color.white; // 重置为默认颜色
                }
                
                // 这里可以添加其他重置逻辑，比如设置网格为可行走状态
                var gridNode = grid.GetComponent<GridNode>();
                if (gridNode != null)
                {
                    gridNode.isWalkable = true;
                }
            }
        }
        
        Debug.Log("地图块已重置");
    }

    private void CleareBlock(GameObject activeGameObject)
    {

                // 清理字典
        gridMap.Clear();
        
        // 清理子物体
        int count = activeGameObject.transform.childCount;
        for (int i = count - 1; i >= 0; i--)
        {
            var child = activeGameObject.transform.GetChild(i).gameObject;
            DestroyImmediate(child);
        }

        Debug.Log("地图块已清理");
    }

    private void CreateBlock(GameObject obj)
    {

        //CleareBlock(obj);

        Vector2 startPos;
        if (useCustomRange)
        {
            // 使用自定义范围
            startPos = startPoint;
        }
        else
        {
            // 使用居中布局
            startPos = new Vector2(
                -((maxX - 1) * blockSize) * 0.5f,
                -((maxY - 1) * blockSize) * 0.5f
            );
        }

        var cubePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/A Star/Res/Square.prefab");

        if (cubePrefab == null)
        {
            // 如果预制体不存在，创建一个临时的
            CreateFallbackGrid(obj, startPos);
            return;
        }

        for (int y = 0; y < maxY; y++)
        {
            for (int x = 0; x < maxX; x++)
            {
                Vector2 pos = new(
                startPos.x + x * blockSize,
                startPos.y + y * blockSize
            );

            var square = PrefabUtility.InstantiatePrefab(cubePrefab) as GameObject;
            square.transform.SetParent(obj.transform, false);
            square.transform.localPosition = pos;
            square.transform.localScale = new Vector3(blockSize, blockHeight, 1);

                // 设置网格名称和标签
            square.name = $"Grid_{x}_{y}";

                // 添加到字典
            gridMap[new Vector2Int(x, y)] = square;

            // 添加网格节点组件
            if (!square.TryGetComponent<GridNode>(out var gridNode))
            {
                gridNode = square.AddComponent<GridNode>();
            }
            gridNode.gridPosition = new Vector2Int(x, y);
            gridNode.worldPosition = pos;
            gridNode.isWalkable = true;
            }
        }

        Debug.Log($"地图块生成完成: {maxX}x{maxY} 共{gridMap.Count}个网格");
    }
    
    private void CreateFallbackGrid(GameObject obj, Vector2 startPos)
    {
        for (int y = 0; y < maxY; y++)
        {
            for (int x = 0; x < maxX; x++)
            {
                Vector2 pos = new Vector2(
                    startPos.x + x * blockSize,
                    startPos.y + y * blockSize
                );

                var square = new GameObject($"Grid_{x}_{y}");
                square.transform.SetParent(obj.transform, false);
                square.transform.localPosition = pos;
                
                // 添加SpriteRenderer用于显示
                var renderer = square.AddComponent<SpriteRenderer>();
                renderer.color = new Color(1, 1, 1, 0.3f); // 半透明白色
                
                // 添加碰撞体用于障碍物检测
                var collider = square.AddComponent<BoxCollider2D>();
                collider.isTrigger = true;
                collider.size = new Vector2(blockSize, blockHeight);
                
                // 添加到字典
                gridMap[new Vector2Int(x, y)] = square;
                
                // 添加网格节点组件
                var gridNode = square.AddComponent<GridNode>();
                gridNode.gridPosition = new Vector2Int(x, y);
                gridNode.worldPosition = pos;
                gridNode.isWalkable = true;
            }
        }
        
        Debug.Log("使用备用方案生成网格");
    }
    private void OnSelectionChange()
    {
        Repaint();
    }
    // 网格节点组件，用于存储网格信息
    
}
    


