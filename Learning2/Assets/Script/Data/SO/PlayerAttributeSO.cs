using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[CreateAssetMenu(fileName ="PlayerAttribute" , menuName ="Data/PlayerAttribut")]
public class PlayerAttributeSO : ScriptableObject
{
    public List<AttributePair> _playerAttribute = new List<AttributePair>()
    {
        new AttributePair(){ attributeType = PlayerAttribute.Attack, value = 10},
        new AttributePair(){ attributeType = PlayerAttribute.HP, value = 100 },
        new AttributePair(){ attributeType = PlayerAttribute.MaxHP, value = 100},
        new AttributePair(){ attributeType = PlayerAttribute.Defense, value = 20 },
        new AttributePair(){ attributeType = PlayerAttribute.Exp, value = 0 },
        new AttributePair(){ attributeType = PlayerAttribute.MaxExp, value = 100 },
        new AttributePair(){ attributeType = PlayerAttribute.Level, value = 1 },
        new AttributePair(){ attributeType = PlayerAttribute.MP, value = 100 },
        new AttributePair(){ attributeType = PlayerAttribute.MaxMP, value = 100 },
    };
}
[Serializable]
public struct AttributePair
{
    public PlayerAttribute attributeType;
    public int value;
}

public enum PlayerAttribute
{
    Attack, //攻击力
    HP, //血量
    MaxHP, //最大血量
    MP, //蓝量
    MaxMP, //最大蓝量
    Defense ,//防御力
    Exp, //当前经验值
    MaxExp, //升级所需经验
    Level, //角色等级
}
