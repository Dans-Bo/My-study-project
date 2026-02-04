using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAttributeManager : MonoBehaviour
{
    [Header("初始属性配置文件")]
    [SerializeField] private PlayerAttributeSO initAttributeSO;  //配置好的初始属性

    private Dictionary<PlayerAttribute , int> currentAttribute = new(); //当前属性

    public event Action<PlayerAttribute,int> OnAttributeChange;   //属性改变事件

     void Awake()
    {
        if(initAttributeSO == null)
        {
            initAttributeSO = Resources.Load<PlayerAttributeSO>("Data/PlayerAttribute");
        }

        if(initAttributeSO == null) 
        {
            Debug.LogError($"初始属性配置文件为空");
            return;
        }

        foreach(var attr in initAttributeSO._playerAttribute)
        {
            currentAttribute[attr.attributeType] = attr.value;
        }

        //TODO 存储属性值，读取属性值文档文件
    }
/// <summary>
/// 获取当前属性
/// </summary>
/// <param name="attrType"></param>
/// <returns></returns>
    public int GetAttribute(PlayerAttribute attrType)
    {
        if(currentAttribute.TryGetValue(attrType,out int value))
        {
            return value;
        }

        Debug.LogError($"不存在属性：{attrType}");
        return 0;
    }


/// <summary>
/// 加减修改属性
/// </summary>
/// <param name="attrType"></param>
/// <param name="deltaValue"></param>
    public void ModifyAttribute(PlayerAttribute attrType, int deltaValue)
    {
        if(!currentAttribute.ContainsKey(attrType))
        {
            Debug.LogError($"属性不存在:{attrType}");
            return;
        }

        int oldValue = currentAttribute[attrType];
        int newValue = oldValue + deltaValue;

        SetAttributeValue(attrType,newValue);
        
    }
/// <summary>
/// 外部设置属性的值
/// </summary>
/// <param name="attrType"></param>
/// <param name="value"></param>
    public void SetAttribute(PlayerAttribute attrType, int value)
    {
        if(!currentAttribute.ContainsKey(attrType))
        {
            Debug.LogError($"属性不存在{attrType}");
            return;
        }
        SetAttributeValue(attrType,value);
        
    }
/// <summary>
/// 处理属性值
/// </summary>
/// <param name="attrType"></param>
/// <param name="value"></param>
    private void SetAttributeValue(PlayerAttribute attrType, int value)
    {
        int oldValue = currentAttribute[attrType];
        int newValue = value;

        switch(attrType)
        {
            case PlayerAttribute.MaxHP:
            case PlayerAttribute.MaxMP:
                newValue = Mathf.Max(newValue,1); //至少为1
                break;
            case PlayerAttribute.HP:
                newValue = Mathf.Clamp(newValue,0,currentAttribute[PlayerAttribute.MaxHP]);//最大值不超过最大血量，最小值不为负
                break;
            case PlayerAttribute.MP:
                newValue = Mathf.Clamp(newValue,0,currentAttribute[PlayerAttribute.MaxMP]); //最大值不超过最大蓝量，最小值不为负
                break;
            case PlayerAttribute.Level:
            case PlayerAttribute.Exp:
            case PlayerAttribute.MaxExp:
                newValue = Mathf.Max(newValue,0); //等级经验不为负
                break;
        }

        if(Mathf.Abs(newValue - oldValue) > 0.0001f)  //属性变化，更新属性并触发事件
        {
            currentAttribute[attrType] = newValue;
            OnAttributeChange?.Invoke(attrType,newValue);

            #if UNITY_EDITOR
            Debug.Log($"{attrType} 从 {oldValue} 变为 {newValue}");
            #endif
        }
    }
}
