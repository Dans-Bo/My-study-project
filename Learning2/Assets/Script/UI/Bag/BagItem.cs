
using UnityEngine;

/// <summary>
/// 装备类型
/// </summary>
public enum ItemType
{
    Consumable, Armor, Weapon, Material //消耗品，防具，武器，材料
}
/// <summary>
/// 装备品质质量
/// </summary>
public enum Quality
{
    White, Green, Blue, Gold
}

/// <summary>
/// 背包物品基类
/// </summary>
public class BagItem
{

    public int ItemID { get; set; }
    public string ItemName { get; set; }
    public ItemType ItemType { get; set; }
    public Quality BagQuality { get; set; } 
    public string Description { get; set; } //物品描述
    public int Capacity { get; set; } //物品可存储容量

    public int BuyPrice { get; set; } //购买价格
    public int SellPrice { get; set; }//出售价格
    public Sprite ItemSprite{ get; set; } //图标路径
    
    public BagItem(int id, string name, ItemType type, Quality quality, string description,int capacity,int buyPrice,int sellPrice,Sprite itemSprite)
    {
        this.ItemID = id;
        this.ItemName = name;
        this.ItemType = type;
        this.BagQuality = quality;
        this.Description = description;
        this.Capacity = capacity;
        this.BuyPrice = buyPrice;
        this.SellPrice = sellPrice;
        this.ItemSprite = itemSprite;
    }

}
