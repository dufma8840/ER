using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Stat master;
    void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            MonStat target = other.gameObject.GetComponent<MonStat>();
            Stat attaker = master;
            if (target != null)
            {
                float damage = Mathf.Round(master.atk * 1.35f);
                int intdamage = (int)damage;
                Stat attacker = master;
                attacker.Attack(target, intdamage);
                GameManager.GetInstance().SomeMethod(target.transform.position + new Vector3(1, 3f, 0), intdamage);
            }
        }
    }
}
