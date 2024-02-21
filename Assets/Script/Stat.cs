using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    public float Qbasecool = 5;
    public float Wbasecool = 7;
    public float Ebasecool = 10;
    public float Rbasecool = 30;
    public float Qcool, Wcool, Ecool, Rcool, Dcool = 10f,Fcool = 30f;
    public bool Q = true, W = true, E = true, R = true, D = true, F = true;
    public Image Qimage;
    public Image Wimage;
    public Image Eimage;
    public Image Rimage;
    public Image Dimage;
    public Image Fimage;
    public int hp = 100;
    public int mp = 100;
    public int shild = 0;
    public int atk = 10;
    public int hpMax;
    public int mpMax;
    public int lv ;
    public float curexp;
    public int upexp;
    public int coin;
    public Text text;
    static Stat Instance;
    public GameObject lveffect;


    public static Stat GetInstance()
    {
        return Instance;
    }
    private void Awake()
    {
        hpMax = hp;
        mpMax = mp;
        lv = 1;
        upexp = 10;
        Qcool = Qbasecool;
        Wcool = Wbasecool;
        Ecool = Ebasecool;
        Rcool = Rbasecool;
        Dcool = 10f;
        Fcool = 30f;
        coin = 100;
    }
    public void SetText()
    {
        text.text = coin.ToString() + "G";
    }
    private void Start()
    {
        StartCoroutine(CoinTimer());
    }
    public void ReduceCooldown()
    {
        Qbasecool--;
        if (Qbasecool < Qcool)
        {
            Qcool = Qbasecool;
        }
        Wbasecool--;
        if (Wbasecool < Wcool)
        {
            Wcool = Wbasecool;
        }
        Ebasecool--;
        if (Ebasecool < Ecool)
        {
            Ecool = Ebasecool;
        }
        Rbasecool-=3;
        if (Rbasecool < Rcool)
        {
            Rcool = Rbasecool;
        }
    }
    private void Update()
    {
        if (curexp > upexp)
        {
            LevelUp();
        }
        if (Death())
        {
            Destroy(this.gameObject);
            Debug.Log(gameObject.name);
        }
    }
    public void GetCoin(MonStat stat)
    {
        coin += stat.coin;
    }
    public void GetExp(MonStat stat)
    {
        curexp += (stat.upexp/4);
       
    }
    public void LevelUp()
    {
        lv++;
        curexp -= upexp;
        upexp += 20;
        hpMax += 30;
        mpMax += 30;
        hp += 30;
        mp += 30;
        atk += 20;
        StartCoroutine(LvUP());
        ReduceCooldown();
    }

    public bool Death()
    {
        if (hp <= 0)
            return true;
        else
            return false;
    }
    public void Attack(MonStat target,int damege)
    {
        if (target.shild <= 0)
        {
            target.hp = target.hp - damege;
        }
        else
        {
            if (target.shild < atk)
            {
                int leftDamege = damege - target.shild;
                target.shild = 0;
                target.hp -= leftDamege;
            }
            else if (target.shild >= atk)
            {
                target.shild -= damege;
            }
        }
        if (target.Death())
        {
            GetExp(target.GetComponent<MonStat>());
            GetCoin(target.GetComponent <MonStat>());
        }
    }
    public void Attack(MonStat target)
    {
        if (target.shild <= 0)
        {
            target.hp = target.hp - atk;
        }
        else
        {
            if(target.shild < atk)
            {
                int leftDamege = atk - target.shild ;
                target.shild = 0;
                target.hp -= leftDamege;
            }
            else if(target.shild >= atk)
            {
                target.shild -= atk;
            }
        }
        if(target.Death())
        {
            GetExp(target.GetComponent<MonStat>());
            GetCoin(target.GetComponent<MonStat>());
        }
    }
    public IEnumerator QcoolTime()
    {
        Q = false;
        float currentTime = 0f;
        Qimage.fillAmount = 0;
        while (currentTime < Qcool)
        {
            currentTime += Time.deltaTime;
            Qimage.fillAmount = currentTime / Qcool;
            yield return null;
        }
        Qimage.fillAmount = 1f;
        Qcool = Qbasecool;
        Q = true;
        yield break;
    }
    public IEnumerator WcoolTime()
    {
        W = false;
        float currentTime = 0f;
        Wimage.fillAmount = 0;
        while (currentTime < Wcool)
        {
            currentTime += Time.deltaTime;
            Wimage.fillAmount = currentTime / Wcool;
            yield return null;
        }
        Wimage.fillAmount = 1f;
        Wcool = Wbasecool;
        W = true;
        yield break;
    }
    public IEnumerator EcoolTime()
    {
        E = false;
        float currentTime = 0f;
        Eimage.fillAmount = 0;
        while (currentTime < Ecool)
        {
            currentTime += Time.deltaTime;
            Eimage.fillAmount = currentTime / Ecool;
            yield return null;
        }
        Eimage.fillAmount = 1f;
        Ecool = Ebasecool;
        E =  true;
        yield break;
    }
    public IEnumerator RcoolTime()
    {
        R = false;
        float currentTime = 0f;
        Rimage.fillAmount = 0;
        while (currentTime < Rcool)
        {
            currentTime += Time.deltaTime;
            Rimage.fillAmount = currentTime /Rcool;
            yield return null;
        }
        Rimage.fillAmount = 1f;
        Rcool = Rbasecool;
        R = true;
        yield break;
    }
    public IEnumerator DcoolTime()
    {
        D = false;
        float currentTime = 0f;
        Dimage.fillAmount = 0;
        while (currentTime < Dcool)
        {
            currentTime += Time.deltaTime;
            Dimage.fillAmount = currentTime / Dcool;
            yield return null;
        }
        Dimage.fillAmount = 1f;
        Dcool = 10f;
        D = true;
        yield break;
    }
    public IEnumerator FcoolTime()
    {
        F = false;
        float currentTime = 0f;
        Fimage.fillAmount = 0;
        while (currentTime < Fcool)
        {
            currentTime += Time.deltaTime;
            Fimage.fillAmount = currentTime / Fcool;
            yield return null;
        }
        Fimage.fillAmount = 1f;
        Fcool = 30f;
        F = true;
        yield break;
    }
    private IEnumerator CoinTimer()
    {
        while (true)
        {
            int coinsPerSecond = 1;
            this.coin += coinsPerSecond;

            yield return new WaitForSeconds(2f / Time.timeScale);
        }
    }
    IEnumerator LvUP()
    {
        lveffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        lveffect.SetActive(false);
    }
}

