using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Slot : MonoBehaviour,IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item;
    public Image image;
    public Sprite originimage;
    public Button button;
    public int idx;
    private Inven inven;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
        inven = FindObjectOfType<Inven>();
    }
    public void SetItem(Item item)
    {
        if (item != null)
        {
            if (item.itemtype == Item.ItemType.None)
            {
                this.item = null;
                this.image.sprite = originimage;
            }
            else
            {
                this.item = item;
                this.image.sprite = item.icon;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.selectSlot = this;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = Input.mousePosition;

        bool _IsMouseOut1 = !RectTransformUtility.RectangleContainsScreenPoint(Inven.GetInstance().rectTransform1, mousePosition);
        bool _IsMouseOut2 = !RectTransformUtility.RectangleContainsScreenPoint(Inven.GetInstance().rectTransform2, mousePosition);
        if (_IsMouseOut1 && _IsMouseOut2)
        {
            ClearSlot();
        }

        if (item != null)
        {
            DragSlot.instance.selectSlot = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.selectSlot != null && DragSlot.instance.selectSlot.item != null)
        {
            Item temp = item;
            Slot tempSlot = DragSlot.instance.selectSlot;

            if (temp != null)
            {
                if(!(idx== -1 || idx == -2))
                {
                    SetItem(tempSlot.item);
                    tempSlot.SetItem(temp);
                }
            }

            if (temp == null)
            {
                SetItem(tempSlot.item);
                tempSlot.ClearSlot();
            }

        }
    }

    private void OnClick()
    {
        if (inven.Findinven() != -1 && inven != null && item != null && !(idx == -1 || idx == -2))
        {
            if(inven.Findinven() != -1 || inven.Findinven() != -2)
            {
                Inven.GetInstance().Use(item, idx);
            }
            
        }
        else if (inven.Findinven() != -1&&inven != null &&item != null && idx == -1 || idx == -2)
        {
            Item temp = item;
            ClearSlot();
            Inven.GetInstance().inven[Inven.GetInstance().Findinven()].SetItem(temp);
            if (temp.itemtype == Item.ItemType.Weapon)
            {
                PlayerInput.instance.stat.atk -= temp.damage;
            }
            if (temp.itemtype == Item.ItemType.Armor)
            {
                PlayerInput.instance.stat.hpMax -= temp.maxhp;
                PlayerInput.instance.stat.hp -= temp.maxhp;
                PlayerInput.instance.stat.mpMax -= temp.maxmp;
                PlayerInput.instance.stat.mp -= temp.maxmp;
            }
        }
    }
    public void ClearSlot()
    {
        item = null;
        image.sprite = originimage;
    }
}