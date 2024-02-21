using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Boxinven : MonoBehaviour
{
    public List<boxSlot> box = new List<boxSlot>();
    public GameObject boxUI;
    public bool _UI;
    public PlayerInput player;
    public float dist = 2f;
    private GameObject activeBoxUI;

    private void Awake()
    {

    }
    void Start()
    {
        StartCoroutine(Destory());
    }

    public void Reset()
    {
        for (int i = 0; i < box.Count; i++)
        {
            box[i].item = null;
            box[i].idx = i;
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
    public void GetItem(boxSlot slot,Inven inven)
    {
        if (slot != null && slot.item != null && inven != null && Inven.GetInstance().Findinven() != -1)
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

    void Update()
    {
        if (player != null)
        {
            float distPlayer = Vector3.Distance(player.transform.position, this.transform.position);
            if (distPlayer < dist)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] hits;
                    if (!IsPointerOverUIObject())
                    {
                        int mask = LayerMask.GetMask("ItemBox");
                        hits = Physics.RaycastAll(ray, Mathf.Infinity, mask);
                        foreach(var hit in hits)
                        {
                            if (hit.collider.gameObject == gameObject)
                            {
                                GameManager.OpenBoxUI(boxUI);
                                _UI = true;
                                player.mousePos = player.transform.position;
                                player.agent.SetDestination(player.transform.position);
                            }
                        }
                    }
                }
                if(Input.GetKeyDown(KeyCode.G))
                {
                    if(_UI)
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
    IEnumerator Destory()
    {
        yield return new WaitForSeconds(13f);
        Destroy(gameObject);
    }
}
