using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAttributeManager))]
public class PlayerLevelManager : MonoBehaviour
{
    private PlayerAttributeManager attrManager;
    [Header("每级属性加成")]
    public int hpAddPerLevel = 10; //每级增加生命值
    public int mpAddPerLevel = 20; //每级增加蓝量
    public int attackAddPerLevel = 5; // 每级增加攻击力
    public int defenseAddPerLevel = 5; //每级增加防御力

    [Header("升级经验倍率")]
    public float expMoutiplier = 1.5f; 


    
    void Awake()
    {
        attrManager = GetComponent<PlayerAttributeManager>();

        if(attrManager == null)
        {
            #if UNITY_EDITOR
            Debug.LogError($"找不到PlayerAttributeManager组件");
            #endif
            enabled = false; //禁用脚本
            return;
        }
    }

    void OnEnable()
    {
        if(attrManager != null)
        {
            attrManager.OnAttributeChange += OnExpChange;
        }
    }

    void OnDisable()
    {
        if(attrManager != null)
        {
            attrManager.OnAttributeChange -= OnExpChange;
        }
    }

    private void OnExpChange(PlayerAttribute attribute, int value)
    {
        if(attribute != PlayerAttribute.Exp) return;

        int maxExp = attrManager.GetAttribute(PlayerAttribute.MaxExp);

        while(value >= maxExp)
        {
            value -= maxExp;
            int newMaxExp = Mathf.CeilToInt(maxExp *0.5f);
            //升级
            attrManager.ModifyAttribute(PlayerAttribute.Level,1);
            attrManager.ModifyAttribute(PlayerAttribute.MaxExp,newMaxExp);
            maxExp = attrManager.GetAttribute(PlayerAttribute.MaxExp); 

            //升级属性加成
            attrManager.ModifyAttribute(PlayerAttribute.Attack,attackAddPerLevel);
            attrManager.ModifyAttribute(PlayerAttribute.MaxHP,hpAddPerLevel);
            attrManager.ModifyAttribute(PlayerAttribute.MaxMP,mpAddPerLevel);
            attrManager.ModifyAttribute(PlayerAttribute.Defense,defenseAddPerLevel);
        

            //血量蓝量恢复满状态
            var maxHP = attrManager.GetAttribute(PlayerAttribute.MaxHP);
            var maxMP = attrManager.GetAttribute(PlayerAttribute.MaxMP);
            attrManager.SetAttribute(PlayerAttribute.HP,maxHP);
            attrManager.SetAttribute(PlayerAttribute.MP,maxMP);
        }

        attrManager.SetAttribute(PlayerAttribute.Exp,value);  //当前经验值为升级后剩下的值
            
        LevelUp();
    }

    private void LevelUp()
    {
       //TODO 升级音效，特效，等
        
        
    }
}
