/* using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 自定义 Minotaur_State 的编辑器
[CustomEditor(typeof(Minotaur_State), true)] // true 表示作用于所有子类
public class MinotaurStateEditor : Editor
{
    // 找到 attackCooldown 字段的序列化属性
    private SerializedProperty attackCooldownProp;
    private void OnEnable()
    {
         // 获取父类中的 attackCooldown 字段
        attackCooldownProp = serializedObject.FindProperty("attackCooldown");
    }

    public override void OnInspectorGUI()
    {
        // 获取当前选中的状态实例
        Minotaur_State state = (Minotaur_State)target;

        //绘制所有默认字段,除了 attackCooldown
        DrawPropertiesExcluding(serializedObject, "attackCooldown");

        //当选中的是 Attack 状态时，才绘制 attackCooldown 字段
        if (state is Minotaur_Attack)
        {
            EditorGUILayout.Space(); // 空一行，分隔布局
            EditorGUILayout.PropertyField(attackCooldownProp); // 显示冷却时间字段
        }

        //应用序列化修改
        serializedObject.ApplyModifiedProperties();

    }
}
 */