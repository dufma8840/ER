using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    public static DragSlot instance;
    public Slot selectSlot;
    public Item item;
    [SerializeField]
    public Image imageItem;
    private void Awake()
    {
    }
    public void Reset()
    {
        instance = this;
        selectSlot = null;
        imageItem = null;
    }
    private void Update()
    {
    }
    public void DragSetImage(Sprite _itemImage)
    {
        imageItem.sprite = _itemImage;
    }
}
