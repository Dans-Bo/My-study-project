
using UnityEngine;

/// <summary>
/// 消耗品类
/// </summary>
public class Consumable : BagItem
{
    public int RecoveryHP { get; set; }
    public Consumable(int id, string name, ItemType type, Quality quality, string description, int capacity, int buyPrice, int sellPrice, Sprite itemSprite,int recoveryHP) : base(id, name, type, quality, description, capacity, buyPrice, sellPrice,itemSprite)
    {
        this.RecoveryHP = recoveryHP;
    }
}
