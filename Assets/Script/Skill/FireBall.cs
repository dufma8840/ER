
using UnityEngine;

public class FireBall : MonoBehaviour
{
    PlayerInput player;
    public Stat master;
    public float fDist;
    public float speed;
    public GameObject target;
    Vector3 posTarget;
    // Start is called before the first frame update

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerInput>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 vPos = this.transform.position;
        float Dist = Vector3.Distance(Magic.GetInstance().vStart, vPos);
        MonStat montarget = target.gameObject.GetComponent<MonStat>();
        if (montarget.hp <= 0)
        {
            target = null;
        }
        if (target == null)
        {
            gameObject.SetActive(false);
        }
        if (target != null)
        {
            Vector3 dir = (target.transform.position - vPos).normalized;
            posTarget = dir * speed * Time.deltaTime;
            transform.Translate(posTarget);
        }
        if (fDist <= Dist)
        {
            gameObject.SetActive(false);
        }

    }

    public void Move()
    {
        gameObject.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster")&& other.gameObject== player.Target)
        {
            MonStat target = other.gameObject.GetComponent<MonStat>();
            Stat attaker = master;
            if (target != null)
            {
                GameManager.GetInstance().SomeMethod(target.transform.position + new Vector3(1, 3f, 0), master.atk);
                Stat attacker = master;
                attacker.Attack(target);
                gameObject.SetActive(false);
            }
        }
    }
}
