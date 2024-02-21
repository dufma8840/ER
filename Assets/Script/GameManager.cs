using FischlWorks_FogWar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GUI_STATE { TITLE, GAMEOVER, Escape,PLAY }
    public GUI_STATE curGUIState;
    public Statbar hpStatbar;
    public Statbar mpStatbar;
    public Statbar expStatbar;
    static GameManager Instance;
    private static GameObject activeBoxUI;
    public ItemData itemManager;
    public List<GameObject> listGUIState;
    public GameObject prefabBox;
    public PlayerInput player;
    public Inven inventory;
    public DragSlot dragSlot;
    public List<RandomItem> itemBoxes = new List<RandomItem>();
    public DamagePooling damageDisplayPool;
    public csFogWar fogWar;



    public static GameManager GetInstance()
    {
        return Instance;
    }
    public void SomeMethod(Vector3 position, int damage)
    {
        GameObject damageObj = damageDisplayPool.GetDamageObject(position, damage);
    }
    public static void OpenBoxUI(GameObject boxUI)
    {
        if (activeBoxUI != null && activeBoxUI != boxUI)
        {
            activeBoxUI.SetActive(false);
        }
        boxUI.SetActive(true);
        activeBoxUI = boxUI;
    }
    public static bool IsActiveBoxUI(GameObject boxUI)
    {
        return activeBoxUI == boxUI && activeBoxUI.activeSelf;
    }
    public static void CloseBoxUI(GameObject boxUI)
    {
        if (activeBoxUI == boxUI)
        {
            activeBoxUI.SetActive(false);
            activeBoxUI = null;
        }
    }

    public IEnumerator ResponBear(Bear bear)
    {
        SoundManager.GetInstance().BearSound();
        MonStat stat = bear.GetComponent<MonStat>();
        yield return new WaitForSeconds(30f);
        stat.hp = stat.hpMax;
        stat._IsAttack = true;
        stat._IsMove = true;
        bear.transform.position = stat.sPos;
        bear.transform.rotation = stat.sAngel;
        stat._IsDead = false;
        bear.Save();
        bear.gameObject.SetActive(true);
  
        
    }
    public IEnumerator ResponMino(Mino mino)
    {
        SoundManager.GetInstance().MinoSound();
        MonStat stat = mino.GetComponent<MonStat>();
        yield return new WaitForSeconds(30f);
        stat.hp = stat.hpMax;
        stat._IsAttack = true;
        stat._IsMove = true;
        mino.transform.position = stat.sPos;
        mino.transform.rotation = stat.sAngel;
        stat._IsDead = false;
        mino.Save();
        mino.gameObject.SetActive(true);
    }
    private void Awake()
    {
        Instance = this;
        itemManager = gameObject.AddComponent<ItemData>();
        itemManager.Init();
        inventory.Reset();
        dragSlot.Reset();
        foreach (RandomItem itemBox in itemBoxes)
        {
            itemBox.Reset();
            itemBox.Init();
            itemBox.AddRandomItemsToInventory();
        }   
    }

    public void FirePool()
    {
        if (Magic.queUsePool.Count > 0)
        {
            FireBall bullet = Magic.queUsePool.Dequeue();
            Magic.queDisablePool.Enqueue(bullet);
        }
    }
    void Start()
    {
        curGUIState = GUI_STATE.PLAY;
        SetGUIState(curGUIState);
        inventory.weaponSlot.SetItem(itemManager.GetItem("Book"));
        inventory.armorSlot.SetItem(itemManager.GetItem("Armor"));
    }
    public void ShowScenec(GUI_STATE state)
    {
        for (int i = 0; i < listGUIState.Count; i++)
        {
            if ((GUI_STATE)i == state)
                listGUIState[i].SetActive(true);
            else
                listGUIState[i].SetActive(false);
        }
    }
    public void CopyBox(Vector3 Spawn,List<Item> items)
    {
        GameObject copybox = Instantiate(prefabBox);
        copybox.transform.position = Spawn;
        Boxinven copyboxinven = copybox.GetComponent<Boxinven>();
        copyboxinven.Reset();
        copyboxinven.player = player;
        foreach(Item item in items)
        {
            copyboxinven.box[copyboxinven.FindBox()].SetItem(item);
        }
    }
    public void SetGUIState(GUI_STATE state)
    {
        switch (state)
        {
            case GUI_STATE.TITLE:
                Time.timeScale = 1;
                break;
            case GUI_STATE.GAMEOVER:
                Time.timeScale = 0;
                break;
            case GUI_STATE.PLAY:
                Time.timeScale = 1;
                break;
            case GUI_STATE.Escape:
                Time.timeScale = 0;
                break;
        }
        ShowScenec(state);
        curGUIState = state;
    }


    public void EventContinue()
    {
        SetGUIState(GUI_STATE.PLAY);
    }

    public void EventGameOver()
    {
        SetGUIState(GUI_STATE.GAMEOVER);
    }

    public void EventEscape()
    {
        SetGUIState(GUI_STATE.Escape);
    }

    public void EventTitle()
    {
        SetGUIState(GUI_STATE.TITLE);
    }
    public void ReStartBT()
    {
        AudioSource BTsound = GetComponent<AudioSource>();
        BTsound.Play();
        EventReStart();
    }
    public void ExitBT()
    {
        AudioSource BTsound = GetComponent<AudioSource>();
        BTsound.Play();
        EventExit();
    }
    public void EventReStart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void EventExit()
    {
#if UNITY_STANDALONE_WIN
        Application.Quit();
     
#elif UNITY_ANDROID
        Application.Quit();
#endif
    }

    public void UpdateState()
    {
        switch (curGUIState)
        {
            case GUI_STATE.TITLE:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    EventContinue();
                }
                break;
            case GUI_STATE.GAMEOVER:
                if (Input.GetKeyDown(KeyCode.R))
                {
                    EventReStart();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    EventTitle();
                }
                break;
            case GUI_STATE.PLAY:
                PlayerDeath();
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    EventTitle();
                }
                break;
            case GUI_STATE.Escape:
                if (Input.GetKeyDown(KeyCode.R))
                {
                    EventReStart();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    EventExit();
                }
                break   ;
        }
    }
    void PlayerDeath()
    {
        PlayerInput player = PlayerInput.instance;
        if(player.stat.hp <=0)
        {
            EventGameOver();
        }
    }

    public void HPbar()
    {
        PlayerInput player = PlayerInput.instance;
        hpStatbar.SetBar(player.stat.hp, player.stat.hpMax);
    }   
    public void MPbar()
    {
        PlayerInput player = PlayerInput.instance;
        mpStatbar.SetBar(player.stat.mp, player.stat.mpMax);
    }   
    public void EXPbar()
    {
        PlayerInput player = PlayerInput.instance;
        expStatbar.SetBar(player.stat.curexp, player.stat.upexp);
    }
    // Update is called once per frame
    void Update()
    {
        HPbar();
        MPbar();
        EXPbar();
        UpdateState();
        PlayerInput.GetInstance().stat.SetText();
    }
}
