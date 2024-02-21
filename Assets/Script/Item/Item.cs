using System;
using UnityEngine;
[Serializable]
public class Item
{
    public enum ItemType
    {
        None = -1,
        HP,
        MP,
        Weapon,
        Armor,
        Ticket,
        ETC
    }
    public ItemType itemtype;
    public string itemName;
    public Sprite icon;
    public int damage;
    public int maxhp;
    public int maxmp;
    public int heal;
    public int coin;

    public Item(ItemType type,string itemName, Sprite icon, int damage, int maxhp, int maxmp, int heal)
    {
        itemtype = type; 
        this.itemName = itemName;
        this.icon = icon;
        this.damage = damage;
        this.maxhp = maxhp;
        this.maxmp = maxmp;
        this.heal = heal;
    }
    public Item(ItemType type, string itemName, Sprite icon, int damage, int maxhp, int maxmp, int heal, int coin)
    {
        itemtype = type;
        this.itemName = itemName;
        this.icon = icon;
        this.damage = damage;
        this.maxhp = maxhp;
        this.maxmp = maxmp;
        this.heal = heal;
        this.coin = coin;
    }
}