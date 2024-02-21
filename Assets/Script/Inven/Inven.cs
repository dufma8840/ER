using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inven : MonoBehaviour
{
    Item item;
    public bool _IsHP = false;
    public bool _IsMP = false;
    public bool _IsExit = false;
    public List<Slot> inven = new List<Slot>();
    public Slot weaponSlot;
    public Slot armorSlot;
    static Inven instance;
    public Text text;
    public float exittimer;
    public RectTransform rectTransform1;
    public RectTransform rectTransform2;
    public Image HPbase;
    public Image MPbase;
    public Image HPimage;
    public Image MPimage;
    

    public static Inven GetInstance() { return instance; } 
    private void Awake()
    {
        exittimer = 6f;
        instance = this;
        weaponSlot.item = null;
        weaponSlot.idx = -1;
        armorSlot.item = null;
        armorSlot.idx = -2;
    }
    public void Reset()
    {
        for (int i = 0; i < inven.Count; i++)
        {
            inven[i].item = null;
            inven[i].idx = i;
        }
    }

    private Item GetItemToCombine() 
    { 
        for (int i = 0; i < inven.Count; i++)
        {
            if (inven[i].item != null)
            {
                if(inven[i].item.itemName == "Book" || inven[i].item.itemName == "Armor" || inven[i].item.itemName == "GemBook" || inven[i].item.itemName == "GemArmor")
                    return inven[i].item;
            }
        }
        return null;
    }
    private Item GetEtcItem()
    {
        for (int i = 0; i < inven.Count; i++)
        {
            if (inven[i].item != null && inven[i].item.itemtype == Item.ItemType.ETC)
            {
                return inven[i].item;
            }
        }
        return null;
    }
    public void ClickCombin()
    {
        PlayerInput.GetInstance()._IsProduce = true;
        Item itemToCombine = GetItemToCombine();
        Item etcItem = GetEtcItem();
        StartCoroutine(Combination(itemToCombine, etcItem)); 
    }

    IEnumerator Combination(Item item, Item etc)
    {
        PlayerInput.GetInstance().mousePos = PlayerInput.GetInstance().transform.position;
        PlayerInput.GetInstance().agent.SetDestination(PlayerInput.GetInstance().mousePos);
        if (item != null && etc != null)
        {
            if (item.itemName == "Book" && etc.itemName == "Gem")
            {
                Debug.Log(item.itemName + "조합.");
                yield return new WaitForSeconds(1f);
                if (PlayerInput.GetInstance()._IsProduce)
                {
                    Item Gemitem = new Item(Item.ItemType.Weapon, "GemBook", Resources.Load<Sprite>("rings"), 200, 0, 0, 0);
                    inven[FindItem(item)].ClearSlot();
                    inven[FindItem(etc)].ClearSlot();
                    inven[Findinven()].SetItem(Gemitem);
                    PlayerInput.GetInstance().CombinSource();
                    PlayerInput.GetInstance().stat.curexp += 20;
                    PlayerInput.GetInstance()._IsProduce = false;
                }
            }
            else if (item.itemName == "Armor" && etc.itemName == "Gem")
            {
                Debug.Log(item.itemName + "조합.");
                yield return new WaitForSeconds(1f);
                if (PlayerInput.GetInstance()._IsProduce)
                {
                    Item Gemitem = new Item(Item.ItemType.Armor, "GemArmor", Resources.Load<Sprite>("cloaks"), 0, 500, 500, 0);
                    inven[FindItem(item)].ClearSlot();
                    inven[FindItem(etc)].ClearSlot();
                    inven[Findinven()].SetItem(Gemitem);
                    PlayerInput.GetInstance().CombinSource();
                    PlayerInput.GetInstance().stat.curexp += 20;
                    PlayerInput.GetInstance()._IsProduce = false;
                }
            }
            else if (item.itemName == "GemBook" && etc.itemName == "Ingots")
            {
                Debug.Log(item.itemName + "조합.");
                yield return new WaitForSeconds(1f);
                if (PlayerInput.GetInstance()._IsProduce)
                {
                    Item Ingotsitem = new Item(Item.ItemType.Weapon, "EndBook", Resources.Load<Sprite>("necklace"), 400, 0, 0, 0);
                    inven[FindItem(item)].ClearSlot();
                    inven[FindItem(etc)].ClearSlot();
                    inven[Findinven()].SetItem(Ingotsitem);
                    PlayerInput.GetInstance().CombinSource();
                    PlayerInput.GetInstance().stat.curexp += 100;
                    PlayerInput.GetInstance()._IsProduce = false;
                }
            }
            else if (item.itemName == "GemArmor" && etc.itemName == "Ingots")
            {
                Debug.Log(item.itemName + "조합.");
                yield return new WaitForSeconds(1f);
                if (PlayerInput.GetInstance()._IsProduce)
                {
                    Item Ingotsitem = new Item(Item.ItemType.Armor, "EndArmor", Resources.Load<Sprite>("helmets"), 0, 700, 700, 0);
                    inven[FindItem(item)].ClearSlot();
                    inven[FindItem(etc)].ClearSlot();
                    inven[Findinven()].SetItem(Ingotsitem);
                    PlayerInput.GetInstance().CombinSource();
                    PlayerInput.GetInstance().stat.curexp += 100;
                    PlayerInput.GetInstance()._IsProduce = false;
                }
            }
            else if (item.itemName == "Book" && etc.itemName == "Ingots")
            {
                Debug.Log(item.itemName + "조합.");
                yield return new WaitForSeconds(1f);
                if (PlayerInput.GetInstance()._IsProduce)
                {
                    Item Ingotsitem = new Item(Item.ItemType.Weapon, "IngotsBook", Resources.Load<Sprite>("sword"), 300, 0, 0, 0);
                    inven[FindItem(item)].ClearSlot();
                    inven[FindItem(etc)].ClearSlot();
                    inven[Findinven()].SetItem(Ingotsitem);
                    PlayerInput.GetInstance().CombinSource();
                    PlayerInput.GetInstance().stat.curexp += 50;
                    PlayerInput.GetInstance()._IsProduce = false;
                }
            }
            else if (item.itemName == "Armor" && etc.itemName == "Ingots")
            {

                Debug.Log(item.itemName + "조합.");
                yield return new WaitForSeconds(1f);
                if (PlayerInput.GetInstance()._IsProduce)
                {
                    Item Ingotsitem = new Item(Item.ItemType.Armor, "IngotsArmor", Resources.Load<Sprite>("shoulders"), 0, 750, 750, 0);
                    inven[FindItem(item)].ClearSlot();
                    inven[FindItem(etc)].ClearSlot();
                    inven[Findinven()].SetItem(Ingotsitem);
                    PlayerInput.GetInstance().CombinSource();
                    PlayerInput.GetInstance().stat.curexp += 50;
                    PlayerInput.GetInstance()._IsProduce = false;
                }
            }
            else
            {
                Debug.Log("조합아이템이 없어서 조합할 수 없습니다.");
            }
        }
    }
    void Start()
    {

    }
    private void Update()
    {
       
            if (weaponSlot.item != null && weaponSlot.item.itemtype != Item.ItemType.Weapon)
        {
            Item item = weaponSlot.item;
            inven[Findinven()].SetItem(item);
            weaponSlot.ClearSlot();
        }
        if (armorSlot.item != null && armorSlot.item.itemtype != Item.ItemType.Armor)
        {
            Item item = armorSlot.item;
            inven[Findinven()].SetItem(item);
            armorSlot.ClearSlot();
        }

        for (int i = 1; i < 8; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                QuickSlot(i);
                break;
            }
        }
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (!PlayerInput.GetInstance()._IsProduce)
            {
                PlayerInput.GetInstance()._IsProduce = true;
                Item itemToCombine = GetItemToCombine();
                Item etcItem = GetEtcItem();
                StartCoroutine(Combination(itemToCombine, etcItem));
            }
        }
        if (_IsExit)
            SetText();
    }

    public void QuickSlot(int idx)
    {
        if(idx >=1 && idx <=inven.Count)
        {
            Use(inven[idx-1].item,idx-1);   
        }
        
    }

    public void WeaponEquip(Item item,int idx)
    {
        if (weaponSlot.item != null)
        {
            Item temp = weaponSlot.item;
            weaponSlot.ClearSlot();
            PlayerInput.instance.stat.atk -= temp.damage;
            weaponSlot.SetItem(item);
            PlayerInput.instance.stat.atk += item.damage;
            inven[idx].SetItem(temp);
        }
        else
        {
            weaponSlot.SetItem(item);
            PlayerInput.instance.stat.atk += item.damage;
            inven[idx].ClearSlot();
        }
    }

    public void ArmorEquip(Item item,int idx)
    {
        if (armorSlot.item != null)
        {
            Item temp = armorSlot.item;
            armorSlot.ClearSlot();
            PlayerInput.instance.stat.hpMax -= temp.maxhp;
            PlayerInput.instance.stat.hp -= temp.maxhp;
            PlayerInput.instance.stat.mpMax -= temp.maxmp;
            PlayerInput.instance.stat.mp -= temp.maxmp;
            armorSlot.SetItem(item);
            PlayerInput.instance.stat.hpMax += item.maxhp;
            PlayerInput.instance.stat.hp += item.maxhp;
            PlayerInput.instance.stat.mpMax += item.maxmp;
            PlayerInput.instance.stat.mp += item.maxmp;
            inven[idx].SetItem(temp);
        }
        else
        {
            PlayerInput.instance.stat.hpMax += item.maxhp;
            PlayerInput.instance.stat.hp += item.maxhp;
            PlayerInput.instance.stat.mpMax += item.maxmp;
            PlayerInput.instance.stat.mp += item.maxmp;
            armorSlot.SetItem(item);
            inven[idx].ClearSlot();
        }
    }
    public void Use(Item item,int idx)
    {
        if(item != null)
        {
            switch (item.itemtype)
            {
                case Item.ItemType.Weapon:
                    WeaponEquip(item, idx);
                    Debug.Log(item.itemName + "장착");
                    break;
                case Item.ItemType.Armor:
                    ArmorEquip(item, idx);
                    Debug.Log(item.itemName + "장착");
                    break;
                case Item.ItemType.HP:
                    Consume(item,idx);
                    break;
                case Item.ItemType.MP:
                    Consume(item,idx);
                    break;
                case Item.ItemType.Ticket:
                    Exit(item,idx);
                    break;

            }
        }
    }

    public void Exit(Item item, int idx)
    {
        if(PlayerInput.instance.Escape())
        {
            inven[idx].ClearSlot();
            _IsExit= true;
        }
        else
        {
            Debug.Log("주변에 없습니다");
        }
    }

    public void Consume(Item item,int idx)
    {
        switch (item.itemtype)
        {
            case Item.ItemType.HP:
                if (!_IsHP)
                {
                    StartCoroutine(HPPotion(this.item,idx));
                }
                break;
            case Item.ItemType.MP:
                if (!_IsMP)
                {
                    StartCoroutine(MPPotion(this.item,idx));
                }
                break;
        }
    }
    IEnumerator HPPotion(Item item,int idx)
    {
        _IsHP = true;
        HPbase.gameObject.SetActive(true);
        inven[idx].ClearSlot();
        PlayerInput.GetInstance().PortionSource();
        float totalTime = 10f;
        float totalRecovery = 1000f; 
        float elapsedTime = 0f;
        HPimage.fillAmount = 0;
        while (elapsedTime <= totalTime)
        {
            elapsedTime += Time.deltaTime;
            HPimage.fillAmount = elapsedTime / totalTime;
            int recovery = (int)((float)totalRecovery / totalTime * Time.deltaTime);
            PlayerInput.GetInstance().stat.hp += recovery;
            if (PlayerInput.GetInstance().stat.hp > PlayerInput.GetInstance().stat.hpMax)
            {
                PlayerInput.GetInstance().stat.hp = PlayerInput.GetInstance().stat.hpMax;
            }
            yield return null;
        }
        HPbase.gameObject.SetActive(false);
        HPimage.fillAmount = 1;
        _IsHP = false;
    }
    IEnumerator MPPotion(Item item,int idx)
    {
        _IsMP = true;
        MPbase.gameObject.SetActive(true);
        inven[idx].ClearSlot();
        PlayerInput.GetInstance().PortionSource();
        float totalTime = 10f;
        float totalRecovery = 1000f;
        float elapsedTime = 0f;
        MPimage.fillAmount = 0;
        while (elapsedTime <= totalTime)
        {
            elapsedTime += Time.deltaTime;
            MPimage.fillAmount = elapsedTime / totalTime;
            int recovery = (int)((float)totalRecovery / totalTime * Time.deltaTime);
            PlayerInput.GetInstance().stat.mp += recovery;
            if (PlayerInput.GetInstance().stat.mp > PlayerInput.GetInstance().stat.mpMax)
            {
                PlayerInput.GetInstance().stat.mp = PlayerInput.GetInstance().stat.mpMax;
            }
            yield return null;
        }
        MPbase.gameObject.SetActive(false);
        MPimage.fillAmount = 1;
        _IsMP = false;
    }
    public int Findinven()
    {
        for (int i = 0; i < inven.Count; i++)
        {
            if (inven[i].item == null)
            {
                return i;
            }
        }
        return -1;
    }
    public int FindItem(Item item)
    {
        for (int i = 0; i < inven.Count; i++)
        {
            if (inven[i].item == item)
            {
                return i;
            }
        }
        return -1;
    }
    public void SetText()
    {
        int seconds = Mathf.FloorToInt(exittimer % 60f);
        exittimer -= Time.deltaTime;
        text.text = seconds.ToString();
        if (PlayerInput.GetInstance().stat.hp > 0 && seconds <= 0)
        {
            text.text = null;
            PlayerInput.GetInstance().transform.position = new Vector3(0, 0, 0);
            GameManager.GetInstance().EventEscape();
        }
    }
}
