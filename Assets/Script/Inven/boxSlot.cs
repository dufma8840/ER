using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class boxSlot : MonoBehaviour,IPointerClickHandler
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


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Boxinven boxInventory = GetComponentInParent<Boxinven>();
            RandomItem randomItem = GetComponentInParent<RandomItem>();
            if (boxInventory != null )
            {
                boxInventory.GetItem(this, inven);
            }
            else if (randomItem != null)
            {
                randomItem.GetItem(this, inven);
            }
        }
    }
    public void ClearSlot()
    {
        item = null;
        image.sprite = originimage;
    }
}