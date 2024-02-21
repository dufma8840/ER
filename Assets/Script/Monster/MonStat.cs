using FischlWorks_FogWar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MonStat : MonoBehaviour
{
    public int hp = 100;
    public int mp = 100;
    public int shild = 0;
    public int atk = 10;
    public int hpMax;
    public int mpMax;
    public int Lv;
    public float curexp;
    public int upexp;
    public bool _IsMove, _IsAttack,_IsStun,_IsDead, _IsReturn;
    public int coin;
    public Vector3 sPos;
    public Quaternion sAngel;
    public csFogVisibilityAgent fog;
    public Image hpBar;


    private void Awake()
    {
        _IsMove = true;
        _IsAttack = true;
        _IsStun= false;
        _IsDead = false;
        hpMax = hp;
        mpMax = mp;
        Lv = 1;
        upexp = 30;
        sPos = transform.position;
        sAngel = transform.rotation;   
    }
    public MonStat(int _hp, int _mp, int _shild, int _atk, int _Lv, int _exp, int _coin)
    {
        hp = _hp;
        mp = _mp;
        shild = _shild;
        atk = _atk;
        Lv = _Lv;
        hpMax = hp;
        mpMax = mp;
        curexp = _exp;
        coin = _coin;
    }

    private void Update()
    {
        HPbar();
        curexp += (Time.deltaTime/2);
        hpBar.rectTransform.localScale = new Vector3((float)hp/(float)hpMax,1f,1f);
        if (curexp > upexp)
        {
            LevelUp();
        }
        if (Death())
        {
            List<Item> randomItems = new List<Item>();
            Item item1 = ItemData.instance.GetItem("HP");
            Item item2 = ItemData.instance.GetItem("Gem");
            Item item3 = ItemData.instance.GetItem("MP");
            randomItems.Add(item1);
            randomItems.Add(item2);
            randomItems.Add(item3);
            int itemCount = Random.Range(0, 4);
            List<Item> selectedItems = new List<Item>();
            if(itemCount > 0)
            {
                for (int i = 0; i < itemCount; i++)
                {
                    int randomIndex = Random.Range(0, randomItems.Count);
                    selectedItems.Add(randomItems[randomIndex]);
                    randomItems.RemoveAt(randomIndex);
                }
                GameManager.GetInstance().CopyBox(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), selectedItems);
            }
            _IsDead = true;
        }
    }
    public void GetExp(Stat stat)
    {
        curexp += (stat.upexp / 4);
    }
    public void Attack(Stat target)
    {
        if (target.shild <= 0)
        {
            target.hp = target.hp - atk;
        }
        else
        {
            if (target.shild < atk)
            {
                int leftDamege = atk - target.shild;
                target.shild = 0;
                target.hp -= leftDamege;
            }
            else if (target.shild >= atk)
            {
                target.shild -= atk;
            }
        }
        if (target.Death())
        {
            GetExp(target.GetComponent<Stat>());
        }
    }
    void HPbar()
    {
        if (fog.visibility == true)
        {
            hpBar.gameObject.SetActive(true);
        }
        if (fog.visibility == false)
        {
            hpBar.gameObject.SetActive(false);
        }
    }

    void InitHpBarSize()
    {
        hpBar.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }
    public void LevelUp()
    {
        Lv++;
        curexp -= upexp;
        upexp += 20;
        hpMax += 200;
        hp += 200;
        atk += 50;
        InitHpBarSize();
    }
    public IEnumerator Stun(float time)
    {
        _IsStun= true;
        _IsMove = false;
        _IsAttack = false;
        yield return new WaitForSeconds(time);
        _IsStun= false;
        _IsMove = true;
        _IsAttack = true;
    }
    private void Start()
    {
        StartCoroutine(CoinTimer());
        InitHpBarSize();
    }
    public bool Death()
    {
        if (hp <= 0)
            return true;
        else
            return false;
    }
    private IEnumerator CoinTimer()
    {
        while (true)
        {
            int coinsPerSecond = 1;
            this.coin += coinsPerSecond;

            yield return new WaitForSeconds(2f);
        }
    }
}

