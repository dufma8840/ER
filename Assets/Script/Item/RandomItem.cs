using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Item;

public class RandomItem : MonoBehaviour
{
    public AudioSource boxSource;
    public List<Item> itemList = new List<Item>();
    public List<boxSlot> box = new List<boxSlot>();
    public GameObject boxUI;
    public bool _UI;
    public PlayerInput player;
    public float dist = 2f;
    public void Reset()
    {
        for (int i = 0; i < box.Count; i++)
        {
            box[i].idx = i;
            box[i].item = null;
        }
    }
    public int FindBox()
    {
        for (int i = 0; i < box.Count; i++)
        {
            if (box[i].item == null)
            {
                return i;
            }
        }
        return -1;
    }
    private void Awake()
    {
        boxSource = GetComponent<AudioSource>();
    }

    void Open()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!IsPointerOverUIObject())
            {
                int mask = LayerMask.GetMask("ItemBox");
                if (Physics.SphereCast(ray, 0.5f, out hit, Mathf.Infinity, mask))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        if (!_UI)
                        {
                            boxSource.Play();
                        }
                        GameManager.OpenBoxUI(boxUI);
                        _UI = true;
                        player.mousePos = player.transform.position;
                        player.agent.SetDestination(player.transform.position);
                    }
                }
            }
        }
    }
    public void boxSound()
    {
    }
    private void Update()
    {
        if (player != null)
        {
            float distPlayer = Vector3.Distance(player.transform.position, this.transform.position);
            if (distPlayer <= dist)
            {
                Open();
                if (Input.GetKeyDown(KeyCode.G))
                {
                    if (_UI)
                    {
                        GameManager.CloseBoxUI(boxUI);
                        _UI = false;
                    }
                }
            }
            else
            {
                if (GameManager.IsActiveBoxUI(boxUI))
                {
                    GameManager.CloseBoxUI(boxUI);
                    _UI = false;
                }
            }
        }
        
    }
    bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        return EventSystem.current.IsPointerOverGameObject();
    }
    public void Init()
    {
        itemList.Add(new Item(Item.ItemType.Weapon, "Book", Resources.Load<Sprite>("book"), 100, 0, 0, 0));
        itemList.Add(new Item(Item.ItemType.Armor, "Armor", Resources.Load<Sprite>("armor"), 0, 200, 200, 0));
        itemList.Add(new Item(Item.ItemType.HP, "HP", Resources.Load<Sprite>("hp"), 0, 0, 0, 1000));
        itemList.Add(new Item(Item.ItemType.MP, "MP", Resources.Load<Sprite>("mp"), 0, 0, 0, 1000));
        itemList.Add(new Item(Item.ItemType.ETC, "Gem", Resources.Load<Sprite>("gem"), 0, 0, 0, 0));
    }
    public void AddRandomItemsToInventory()
    {
        int itemCount = Random.Range(1, itemList.Count + 1);
        for (int i = 0; i < itemCount; i++)
        {
            Item randomItem = GetRandomItem();

            while (box.Find(slot => slot.item == randomItem) != null)
            {
                randomItem = GetRandomItem();
            }

            int emptySlotIndex = FindBox();

            if (emptySlotIndex != -1)
            {
                box[emptySlotIndex].SetItem(randomItem);
            }
            else
            {
                return;
            }
        }
    }
    public void GetItem(boxSlot slot, Inven inven)
    {
        if (slot != null && slot.item != null&& inven != null&& Inven.GetInstance().Findinven() != -1)
        {
            inven.inven[inven.Findinven()].SetItem(slot.item);
            slot.ClearSlot();
        }   
    }
    public void Setitem(Item item)
    {
        int emptySlotIndex = FindBox();
        if (emptySlotIndex != -1)
        {
            box[emptySlotIndex].item = item;
        }
    }
    private Item GetRandomItem()
    {
        int randomIndex = Random.Range(0, itemList.Count);

        return itemList[randomIndex];
    }
}
