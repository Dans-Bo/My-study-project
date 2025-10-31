using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    public Vector2Int gridPosition;
    public Vector2 worldPosition;
    public bool isWalkable = true;
    public int movementCost = 1; // 移动成本，可用于不同地形

        // 在Inspector中显示信息
    private void OnDrawGizmosSelected()
    {
        GUIStyle style = new();
        style.normal.textColor = isWalkable ? Color.green : Color.red;
        style.fontSize = 8;

        Handles.Label(transform.position + Vector3.up * 0.3f,
        $"({gridPosition.x},{gridPosition.y})\n{(isWalkable ? "可行走" : "障碍")}", style);
    }
} 
