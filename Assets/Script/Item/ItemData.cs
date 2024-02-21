using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public List<Item> itemData = new List<Item>();
    public static ItemData instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Init();
    }

    public void Init()
    {

        itemData.Add(new Item(Item.ItemType.Weapon, "Book", Resources.Load<Sprite>("book"), 100, 0, 0, 0));
        itemData.Add(new Item(Item.ItemType.Armor, "Armor", Resources.Load<Sprite>("armor"), 0, 200, 200, 0));
        itemData.Add(new Item(Item.ItemType.HP, "HP", Resources.Load<Sprite>("hp"), 0, 0, 0, 1000));
        itemData.Add(new Item(Item.ItemType.MP, "MP", Resources.Load<Sprite>("mp"), 0, 0, 0, 1000));
        itemData.Add(new Item(Item.ItemType.ETC, "Gem", Resources.Load<Sprite>("gem"), 0, 0, 0, 0));
        itemData.Add(new Item(Item.ItemType.ETC, "Ingots", Resources.Load<Sprite>("ingots"), 0, 0, 0, 0));
        itemData.Add(new Item(Item.ItemType.Ticket, "Scroll", Resources.Load<Sprite>("scroll"), 0, 0, 0, 0));
    }

    public void ReleaseItemData()
    {
        itemData.Clear();
    }
    public Item GetItem(string itemName)
    {
        return itemData.Find(item => item.itemName == itemName);
    }
}
