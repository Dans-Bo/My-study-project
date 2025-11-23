

using UnityEngine;

/// <summary>
/// 武器类型
/// </summary>
public enum WeaponType
{
    Primary,Secondary //主手武器，副手武器
}
/// <summary>
/// 武器类
/// </summary>
public class Weapon : BagItem
{
    public int Damage { get; set; }
    public WeaponType _weaponType;

    public Weapon(int id, string name, ItemType type, Quality quality, string description, int capacity, int buyPrice, int sellPrice, Sprite itemSprite,int damage, WeaponType weaponType) : base(id, name, type, quality, description, capacity, buyPrice, sellPrice,itemSprite)
    {
        this.Damage = damage;
        this._weaponType = weaponType;
    }
}
