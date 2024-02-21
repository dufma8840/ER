
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public List<Item> storeList = new List<Item>();
    public List<Button> slot = new List<Button>();
    public PlayerInput player;
    public GameObject storeUI;
    static Store Instance;
    public bool _UI;
    public float dist = 2f;

    public static Store GetInstance()
    {
        return Instance;
    }
    private void Awake()
    {
        _UI = false;
        Instance = this;
    }
    void Start()
    {
        SetStore();
        for (int i = 0; i < 4; i++)
        {
            SetItem(slot[i], storeList[i]);
        }

    }
    public void SetItem(Button slot,Item item)
    {
        slot.onClick.AddListener(() => BuyItem(item));
    }

    public void CloseUI()
    {
        storeUI.SetActive(false);
    }
    void BuyItem(Item item)
    {
        if (player.stat.coin >= item.coin)
        {
            if(Inven.GetInstance().Findinven() == -1)
            {
                Debug.Log("인벤창이 부족합니다");
            }
            else
            {

                player.stat.coin -= item.coin;
                Inven.GetInstance().inven[Inven.GetInstance().Findinven()].SetItem(item);
                PlayerInput.GetInstance().storeSource.Play();
            }
        }
        if(player.stat.coin <= item.coin)
        {
        }

    }
    public void SetStore()
    {
        storeList.Add(new Item(Item.ItemType.HP, "HP", Resources.Load<Sprite>("hp"), 0, 0, 0, 1000,150));
        storeList.Add(new Item(Item.ItemType.MP, "MP", Resources.Load<Sprite>("mp"), 0, 0, 0, 1000,150));
        storeList.Add(new Item(Item.ItemType.ETC, "Ingots", Resources.Load<Sprite>("ingots"), 0, 0, 0, 0,300));
        storeList.Add(new Item(Item.ItemType.Ticket, "Scroll", Resources.Load<Sprite>("scroll"), 0, 0, 0, 0,1000));
    }
    // Update is called once per frame
    void Update()
    {
    }
}
