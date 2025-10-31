
using UnityEngine;

/// <summary>
/// 装备类型
/// </summary>
public enum ArmorType
{
    Head, Body, Leg, Hand, Ring, Necklace //头部，身体，腿部，手部，戒指，项链
}
/// <summary>
/// 装备
/// </summary>
public class Armor : BagItem
{
    /// <summary>
    /// 力量
    /// </summary>
    public int Strength { get; set; } 
    /// <summary>
    /// 智力
    /// </summary>
    public int Intellect { get; set; } 
    /// <summary>
    /// 敏捷
    /// </summary>
    public int Agility { get; set; }
    /// <summary>
    /// 体力
    /// </summary>
    public int Stamina { get; set; }
    
    public ArmorType ArmorType{ get; set; }
    public Armor(int id, string name, ItemType type, Quality quality, string description, int capacity, int buyPrice,
    int sellPrice, Sprite itemSprite,int strength,int intellect,int agility , int stamina, ArmorType armortype) : base(id, name, type, quality, description, capacity, buyPrice, sellPrice,itemSprite)
    {
        this.Strength = strength;
        this.Intellect = intellect;
        this.Agility = agility;
        this.Stamina = stamina;
        this.ArmorType = armortype;
    }
}
