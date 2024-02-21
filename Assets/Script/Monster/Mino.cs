using FischlWorks_FogWar;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Mino : MonoBehaviour
{
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Return,
        Die,
    }
    public enum Action
    {
        Idle,
        Chase,
        Attack,
        Die,
    }
    Animator minoani;
    MonStat stat;
    public NavMeshAgent nav;
    //PlayerInput player;
    float chaseDistance = 7f; 
    float attackDistance = 1.5f; 
    float reChaseDistance = 2;
    public Transform Target;
    float attackDelay = 2f; 
    [SerializeField] State Ecurrent = State.Idle;
    [SerializeField] Action Acurrent = Action.Idle;
    Vector3 sPos;
    Vector3 vPos;
    Quaternion sAngel;
    float dist;
    float returnTime = 3.5f;
    public GameObject stamp;


    public void Save()
    {
        stat._IsMove = true;
        stat._IsAttack = true;
        stat._IsReturn = false;
        stat._IsStun = false;
        Ecurrent = State.Idle;
        Acurrent = Action.Idle;
        returnTime = 3.5f;
        attackDelay = 2f;
    }
    void Start()
    {
        returnTime = 3.5f;
        nav = GetComponent<NavMeshAgent>();
        minoani = GetComponent<Animator>();
        stat = GetComponent<MonStat>();
        sPos = this.transform.position;
        sAngel = this.transform.rotation;
        Target = null;
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
                minoani.SetBool("IsMove", false);
                break;
            case Action.Chase:
                minoani.SetBool("IsMove", true);
                break;
            case Action.Attack:
                break;
            case Action.Die:
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
        if (Target != null || stat.hp < stat.hpMax)
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
                case State.Return:
                    break;
            }
        }

    }
     void IdleState()
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
        if (Target != null)
        {
            //몬스터가 공격 가능 거리 안으로 들어가면 공격 상태
            if (GetDistanceFromPlayer() < attackDistance)
            {
                if (stat._IsAttack)
                {
                    ChangState(State.Attack, Action.Idle);
                    Ecurrent = State.Attack;
                    transform.LookAt(Target.transform);
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
                    nav.SetDestination(Target.position);
                    nav.stoppingDistance = 1.5f;
                }
                if(!stat._IsMove)
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
            if (stat._IsAttack)
            {
                StartCoroutine(Attack());
                transform.LookAt(Target.transform);
              

            }
            else
            {
                minoani.SetBool("IsMove", false);
            }
        }
    }
    float GetDistanceFromPlayer()
    {
        if (Target != null)
        {
            float distance = Vector3.Distance(transform.position, Target.position);
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
        float Range = 6f;
        Collider[] TargetCollider = Physics.OverlapSphere(transform.position, Range, mask);
        for (int i = 0; i < TargetCollider.Length; i++)
        {
            Target = GameObject.Find("Player").transform;
            Target = TargetCollider[0].transform;
            dist = Vector3.Distance(TargetCollider[0].transform.position, this.transform.position);
        }
    }
    void Return()
    {
        vPos = transform.position;
        dist = Vector3.Distance(sPos, vPos);
        if (dist > 12)
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
            Target = null;
            nav.stoppingDistance = 0;
            nav.SetDestination(sPos);
            ChangState(State.Idle, Action.Chase);
            if (Vector3.Distance(vPos, sPos) <= 0.3f)
            {
                stat.hp = stat.hpMax;
                transform.rotation = sAngel;
                ChangAni(Action.Idle);
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
        if (stat._IsStun == false && stat._IsReturn == false)
        {
            float radius = 5f;
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetLayer);
            foreach (Collider collider in colliders)
            {
                if(collider.CompareTag("Player"))
                {
                    minoani.SetTrigger("IsAttack");
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
        if(stat._IsStun == true)
        {
            yield return null;
        }
    }

    // Update is called once per frame
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
        if(stat._IsDead == true)
        {
            GameManager.GetInstance().StartCoroutine(GameManager.GetInstance().ResponMino(this));
            this.gameObject.SetActive(false);
        }

    }
}
