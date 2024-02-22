using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class Bear : MonoBehaviour
{
    public enum State
    {
        Idle,
        Chase,
        Attack,

    }
    public enum Action      
    {
        Idle,
        Chase,
    }
    public NavMeshAgent nav;
    Animator bearani;
    MonStat stat;
    public AudioSource bearSource;
    float chaseDistance = 7f;
    float attackDistance = 2.5f;
    float reChaseDistance = 3f;
    public Transform target;
    float attackDelay = 2f;
    [SerializeField] State Ecurrent = State.Idle;
    [SerializeField] Action Acurrent = Action.Idle;
    Vector3 sPos;
    Vector3 vPos;
    float dist;
    public float returnTime = 3.5f;


    public void Save()
    {
        target = null;
        stat._IsMove = true;
        stat._IsAttack = true;
        stat._IsReturn = false;
        stat._IsStun = false;
        Ecurrent = State.Idle;
        Acurrent = Action.Idle;
        returnTime = 3.5f;
        attackDelay = 2f;
    }

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        bearani = GetComponent<Animator>();
        stat = GetComponent<MonStat>();
;       sPos = this.transform.position;
        target = null;
        returnTime = 3.5f;
        ChangState(State.Idle, Action.Idle);
    }
    public void ChangAni(Action action)
    {
        if (Acurrent == action)
            return;

        Acurrent = action;
        switch (action)
        {
            case Action.Idle:
                bearani.SetBool("IsMove", false);
                break;
            case Action.Chase:
                bearani.SetBool("IsMove", true);
                break;
        }
    }

    void ChangState(State state, Action action)
    {
        if (Ecurrent == state)
            return;
        ChangAni(action);
        Ecurrent = state;
        
    }
    public void UpdateState()
    {
        if(stat.hp < stat.hpMax)
        {
            switch (Ecurrent)
            {
                case State.Idle:
                    IdleState();
                    break;
                case State.Chase:
                    ChaseState();
                    break;
                case State.Attack:
                    AttackState();
                    break;
            }
        }

    }
    public void IdleState()
    {
        if (GetDistanceFromPlayer() < chaseDistance)
        {
            ChangState(State.Chase, Action.Idle);
            
        }
        else
        {
            ChangState(State.Chase, Action.Chase);
        }
    }

    void ChaseState()
    {
        if(target != null)
        {
            if (GetDistanceFromPlayer() <= attackDistance)
            {
                if (stat._IsAttack)
                {
                    ChangState(State.Attack, Action.Idle);
                    Ecurrent = State.Attack;
                }
                else
                {
                    ChangAni(Action.Idle);
                }
            }
            else
            {
                if (stat._IsMove)
                {
                    ChangAni(Action.Chase);
                    nav.SetDestination(target.position);
                    nav.stoppingDistance = 2.5f;
                }
                if (!stat._IsMove)
                {
                    ChangAni(Action.Idle);
                }
            }
        }
    }

    void AttackState()
    {
        if (GetDistanceFromPlayer() > reChaseDistance)
        {
            ChangState(State.Chase, Action.Chase);
            
        }
        else
        {
            if(stat._IsAttack)
            {
                StartCoroutine(Attack());
            }
            else
            {
                bearani.SetBool("IsMove", false);
            }
        }
    }
   
    float GetDistanceFromPlayer()
    {
        if(target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            return distance;
        }
        else
        {
            return 100f;
        }
    }
    public void GetTarget()
    {
        int mask = LayerMask.GetMask("Player");
        float Range = 8f;
        Collider[] TargetCollider = Physics.OverlapSphere(transform.position, Range, mask);
        for (int i = 0; i < TargetCollider.Length; i++)
        {
            target = GameObject.Find("Player").transform;
            target = TargetCollider[0].transform;
            dist = Vector3.Distance(TargetCollider[0].transform.position, this.transform.position);
        }
    }
    void Return()
    {
        vPos = transform.position;
        dist = Vector3.Distance(sPos, vPos);
        if (dist > 5)
        {
            returnTime -= Time.deltaTime;
            if (returnTime <= 0)
            {
                stat._IsReturn = true;
                returnTime = 3.5f;
            }
        }
        if (stat._IsReturn == true)
        {
            target = null;
            nav.stoppingDistance = 0;
            nav.SetDestination(sPos);
            ChangState(State.Idle, Action.Chase);
            if (Vector3.Distance(vPos, sPos) <= 0.1f)
            {
                stat.hp = stat.hpMax;
                ChangAni(Action.Idle);
                transform.rotation = stat.sAngel;
                stat._IsReturn = false;
            }
        }
    }
    IEnumerator Attack()
    {
        int targetLayer = LayerMask.GetMask("Player");
        stat._IsAttack = false;
        stat._IsMove = false;
        yield return new WaitForSeconds(0.5f);
        if (stat._IsStun == false&&stat._IsReturn == false)
        {
            float radius = 5.0f;
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetLayer);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    bearani.SetTrigger("IsAttack");
                    Stat targetHP = collider.GetComponent<Stat>();
                    if (targetHP != null)
                    {
                        stat.Attack(targetHP);
                    }
                }
            }
            yield return new WaitForSeconds(attackDelay);
            stat._IsMove = true;
            stat._IsAttack = true;
        }
        if (stat._IsStun == true)
        {
            yield return null;
        }
    }
    void Update()
    {
        if (!stat._IsStun)
        {
            UpdateState();
            GetTarget();
            Return();
        }
        else if (stat._IsStun)
        {
            ChangAni(Action.Idle);
        }
        if (stat._IsDead == true)
        {
            GameManager.GetInstance().StartCoroutine(GameManager.GetInstance().ResponBear(this));
            this.gameObject.SetActive(false);
        }
    }
}
