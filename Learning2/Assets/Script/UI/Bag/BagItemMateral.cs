

using UnityEngine;

/// <summary>
/// 材料类
/// </summary>
public class BagItemMateral : BagItem
{

    public BagItemMateral(int id, string name, ItemType type, Quality quality, string description, int capacity, int buyPrice, int sellPrice, Sprite itemSprite) : base(id, name, type, quality, description, capacity, buyPrice, sellPrice,itemSprite)
    {
    }
}
