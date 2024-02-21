using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PlayerInput : MonoBehaviour
{
    public Stat stat;
    public Magic magic;
    public Animator anim;
    public NavMeshAgent agent;
    public AudioSource runSource;
    public AudioSource explosionSource;
    public AudioSource combinSource;
    public AudioSource storeSource;
    public AudioSource portionSource;
    public enum State { Idle = 0, Move, Attack, Die }
    public enum Action { Idle = 0, Move, Atatck, Die }
    public string currntMap;
    public Vector3 mousePos;
    public bool _IsMove = false;
    public bool _IsAttack = false;
    public bool _IsResting = false;
    public bool _IsProduce = false;
    [SerializeField] State Ecurrent = State.Idle;
    [SerializeField] Action Acurrent = Action.Idle;
    public float attackDelay = 1.2f;
    public GameObject Target;
    public float speed;
    public float restTimer = 1f;
    public GameObject shiled;
    public GameObject iceFeild;
    public static PlayerInput instance;
    public Image shiledbuffbase;
    public Image shiledbuff;
    public GameObject minimap;
    public GameObject teleportmap;
    public bool _Wkey;
    public bool _Qkey;
    public bool _Fkey;
    Vector3 vPos;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    public static PlayerInput GetInstance()
    {
        return instance;
    }
    void Start()
    {
        Target = null;
        mousePos = transform.position;
        stat = GetComponent<Stat>();
        agent = GetComponent<NavMeshAgent>();
        runSource = GetComponents<AudioSource>()[0];
        explosionSource = GetComponents<AudioSource>()[1];
        combinSource = GetComponents<AudioSource>()[2];
        storeSource = GetComponents<AudioSource>()[3];
        portionSource = GetComponents<AudioSource>()[4];
        anim = GetComponent<Animator>();
        _IsMove = true;
        _IsAttack = true;
        _Wkey = false;
        Ecurrent = State.Idle;
        runSource.clip = SoundManager.GetInstance().GetSoundClip("걷기");
        explosionSource.clip = SoundManager.GetInstance().GetSoundClip("Explosion");
        combinSource.clip = SoundManager.GetInstance().GetSoundClip("조합");
        storeSource.clip = SoundManager.GetInstance().GetSoundClip("골드사용");
        portionSource.clip = SoundManager.GetInstance().GetSoundClip("회복");
    }
    public void ChangAni(Action action)
    {
        if (Acurrent == action)
            return;

        Acurrent = action;
        switch (action)
        {
            case Action.Idle:
                anim.SetBool("IsMove", false);
                MoveSound();
                break;
            case Action.Move:
                anim.SetBool("IsMove", true);
                runSource.Play();
                break;
            case Action.Atatck:
                anim.SetTrigger("IsAttack");
                break;
            case Action.Die:
                break;
        }
    }
    public void ChangState(State state, Action action)
    {
        if (Ecurrent == state)
            return;
        ChangAni(action);
        Ecurrent = state;
    }
    void IdleState()
    {
        if (Vector3.Distance(transform.position, mousePos) >= 1f)
        {
            ChangState(State.Move, Action.Move);
        }
        else
        {
            ChangState(State.Idle, Action.Idle);
        }
    }
    void UpDateState()
    {
        switch (Ecurrent)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Move:
                break;
            case State.Attack:
                IdleState();
                break;
            case State.Die:
                GameManager.GetInstance().EventGameOver();
                break;
        }
    }

    void MoveSound()
    {
        runSource.Stop();

    }
    public void ExplosionSound()
    {
        explosionSource.Play();
    }
    public void CombinSource()
    {
        combinSource.Play();
    }
    public void PortionSource()
    {
        portionSource.Play();
    }
    void Move()
    {
#if UNITY_STANDALONE_WIN
        if (Input.GetMouseButtonDown(1))
        {
            _IsResting = false;
            _IsProduce = false;
            if (_IsMove)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                int mask = LayerMask.GetMask("Plane");
                if (Physics.Raycast(ray, out hit, mask))
                {
                    NavMeshHit navHit;
                    if (NavMesh.SamplePosition(hit.point, out navHit, 1.0f, NavMesh.AllAreas))
                    {
                        mousePos = navHit.position;
                        agent.SetDestination(mousePos);

                    }
                }
            }
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return;
                }
                _IsResting = false;
                _IsProduce = false;
                if (_IsMove)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    int mask = LayerMask.GetMask("Plane");
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                    {
                        NavMeshHit navHit;
                        if (NavMesh.SamplePosition(hit.point, out navHit, 1.0f, NavMesh.AllAreas))
                        {
                            mousePos = navHit.position;
                            agent.SetDestination(mousePos);
                        }
                    }
                    if (Physics.SphereCast(ray, 0.2f,out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
                    {
                        if (_IsAttack && !_Qkey || !_Fkey)
                        {
                            MoveSound();
                            Targetting();
                        }
                    }
                    if (Physics.SphereCast(ray, 0.2f,out hit, Mathf.Infinity, LayerMask.GetMask("ItemBox")))
                    {
                        float dist = 2f;
                        float distPlayer = Vector3.Distance(transform.position,hit.point);
                        if (dist > distPlayer)
                        {
                            agent.SetDestination(this.transform.position);
                            mousePos = transform.position;
                        }
                    }
                    if (Physics.SphereCast(ray, 0.2f, out hit, Mathf.Infinity, LayerMask.GetMask("Store")))
                    {
                        float dist = Vector3.Distance(this.transform.position,hit.point);
                        if(dist <= 2)
                        {
                            Store.GetInstance().storeUI.SetActive(true);
                            agent.SetDestination(this.transform.position);
                            mousePos = transform.position;
                        }
                    }

                }
            }
        }
#endif

    }
    void Attack()
    {
#if UNITY_STANDALONE_WIN
        if (Input.GetMouseButtonDown(0))
        {
            _IsResting = false;
            _IsProduce = false;
            if(_IsAttack)
            {
                MoveSound();
                Targetting();
            }
        }
#endif
    }
    void Shild()
    {
        if (stat.shild <= 0)
        {
            shiled.SetActive(false);
        }
    }
    void Update()
    {
        MiniMap();
        Skill();
        StoreNPC();
        Teleport();
        UpDateState();
        Resting();
        Shild();
        Attack();
        Move();
        GameManager.GetInstance().FirePool();
        if (Vector3.Distance(transform.position, mousePos) <= 0.2f)
        {
            IdleState();
        }
        if (Target)
        {
            float dist = Vector3.Distance(transform.position, Target.transform.position);
        }
        stat.curexp += (Time.deltaTime);

    }

    public void ClickRest()
    {
        _IsProduce = false;
        if (!_IsResting)
        {
            _IsResting = true;

            restTimer = 1f;
            mousePos = this.transform.position;
            agent.SetDestination(mousePos);
            ChangAni(Action.Idle);
        }
    }
    public void CloseTeleport()
    {
        teleportmap.SetActive(false);
    }
    public void ClickMap()
    {
        if (minimap.activeSelf)
        {
            minimap.SetActive(false);
        }
        else
        {
            minimap.SetActive(true);
        }
    }
    void MiniMap()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (minimap.activeSelf)
            {
                minimap.SetActive(false);
            }
            else
            {
                minimap.SetActive(true);
            }
        }
    }
    void Skill()
    {
#if UNITY_ANDROID
        if (_Qkey && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return;
                }
                _IsResting = false;
                _IsProduce = false;
                if (_IsMove)
                {
                    if (_IsAttack)
                    {
                        MoveSound();
                        DoubleTargetting();
                    }
                    //   }
                    else
                    {
                        Debug.Log("적을 클릭해주세요");
                    }
                }
            }
        }
        if (_Fkey && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return;
                }
                _IsResting = false;
                _IsProduce = false;
                stat.StartCoroutine(stat.FcoolTime());
                StartCoroutine(FSkill());
            }
        }
#endif
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (stat.Q)
            {
                MoveSound();
                DoubleTargetting();
                _IsResting = false;
                _IsProduce = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (stat.W && _Wkey)
            {
                _Wkey = false;
            }
            else if (stat.W && _Wkey == false)
            {
                if (stat.mp >= 150)
                {
                    _Wkey = true;
                    _IsResting = false;
                    _IsProduce = false;
                }
                else
                {
                    Debug.Log("마나가 부족합니다");
                }
            }
        }
        if (_Wkey && Input.GetMouseButtonDown(0))
        {
            _Wkey = false;
            MoveSound();
            magic.Explosion(GetComponent<Stat>());
            StartCoroutine(WColl());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (stat.E)
            {
                if (stat.mp >= 150)
                {
                    _IsResting = false;
                    _IsProduce = false;
                    stat.mp -= 150;
                    StartCoroutine(ESkill());
                    stat.StartCoroutine(stat.EcoolTime());
                }
                else
                {
                    Debug.Log("마나가 부족합니다");
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            MoveSound();
            if (_IsAttack)
            {
                if (stat.R)
                {
                    if (stat.mp >= 250)
                    {
                        _IsResting = false;
                        _IsProduce = false;
                        stat.mp -= 250;
                        stat.StartCoroutine(stat.RcoolTime());
                        StartCoroutine(RSkill());
                    }
                    else
                        Debug.Log("마나가 부족합니다");
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (stat.D)
            {
                _IsResting = false;
                _IsProduce = false;
                stat.StartCoroutine(stat.DcoolTime());
                StartCoroutine(DSkill());
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_IsAttack)
            {
                if (stat.F)
                {
                    _IsResting = false;
                    _IsProduce = false;
                    stat.StartCoroutine(stat.FcoolTime());
                    StartCoroutine(FSkill());
                }
            }
        }
    }
    void Resting()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            _IsProduce = false;
            if (!_IsResting)
            {
                _IsResting = true;

                restTimer = 1f;
                mousePos = this.transform.position;
                agent.SetDestination(mousePos);
                ChangAni(Action.Idle);
            }
            else
            {
                _IsResting = false;
            }
        }
        if (_IsResting)
        {
            restTimer -= Time.deltaTime;
            if (restTimer <= 0)
            {
                stat.hp++;
                stat.mp++;
                stat.hp = Mathf.Min(stat.hp, stat.hpMax);
                stat.mp = Mathf.Min(stat.mp, stat.mpMax);
            }
        }
        if (!_IsResting)
        {
            restTimer = 1;
        }
    }

    void Targetting()
    {
        if (!_Wkey)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int mask = LayerMask.GetMask("Enemy");
            float range = 5;
            vPos = transform.position;
            if (Physics.SphereCast(ray, 0.5f, out hit, Mathf.Infinity, mask))
            {
                float dist = Vector3.Distance(hit.transform.position, vPos);
                if (hit.collider.CompareTag("Monster"))
                {
                    if (dist <= range)
                    {
                        Target = hit.collider.gameObject;
                        transform.LookAt(Target.transform.position);
                        mousePos = this.transform.position;
                        if (Target != null)
                        {
                            StartCoroutine(Fireball());
                            agent.SetDestination(this.transform.position);
                            mousePos = this.transform.position;

                        }
                    }
                    if (dist > range)
                    {
                        Vector3 destination = vPos + (hit.point - vPos).normalized * (dist - (range - 0.5f));
                        mousePos = destination;
                        agent.SetDestination(destination);
                        if (transform.position == destination)
                        {
                            Target = hit.collider.gameObject;
                            if (Target != null)
                            {
                                StartCoroutine(Fireball());
                                agent.SetDestination(this.transform.position);
                                mousePos = this.transform.position;
                            }
                        }
                    }
                }
                else
                {
                    Target = null;
                }
            }
        }
    }
    void StoreNPC()
    {
#if UNITY_STANDALONE_WIN
        int mask = LayerMask.GetMask("Store");
        Collider[] storeCollider = Physics.OverlapSphere(transform.position, 3f, mask);
        if (storeCollider.Length >= 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.SphereCast(ray, 0.2f, out hit, Mathf.Infinity, mask))
                {
                    agent.SetDestination(this.transform.position);
                    mousePos = this.transform.position;
                    Store.GetInstance().storeUI.SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                agent.SetDestination(this.transform.position);
                mousePos = this.transform.position;
                Store.GetInstance()._UI = !Store.GetInstance()._UI;
                Store.GetInstance().storeUI.SetActive(Store.GetInstance()._UI);
            }
        }
        if (storeCollider.Length == 0)
        {

            Store.GetInstance().storeUI.SetActive(false);
        }
#endif
    }

    
    void Teleport()
    {
#if UNITY_STANDALONE_WIN
        int mask = LayerMask.GetMask("Teleport");
        Collider[] storeCollider = Physics.OverlapSphere(transform.position, 3f, mask);
        if (storeCollider.Length >= 1)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.SphereCast(ray, 0.2f, out hit, Mathf.Infinity, mask))
                {
                    agent.SetDestination(this.transform.position);
                    mousePos = this.transform.position;
                    teleportmap.SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (teleportmap.activeInHierarchy)
                {
                    teleportmap.SetActive(false);
                }
                else
                {
                    agent.SetDestination(this.transform.position);
                    mousePos = this.transform.position;
                    teleportmap.SetActive(true);
                }
            }
        }
        if (storeCollider.Length == 0)
        {

            teleportmap.SetActive(false);
        }
#elif UNITY_ANDROID
        int mask = LayerMask.GetMask("Teleport");
        Collider[] storeCollider = Physics.OverlapSphere(transform.position, 3f, mask);
        if (storeCollider.Length >= 1)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                {
                    agent.SetDestination(this.transform.position);
                    mousePos = this.transform.position;
                    teleportmap.SetActive(true);
                }
            }
        }
        if (storeCollider.Length == 0)
        {

            teleportmap.SetActive(false);
        }
#endif
    }

    public void TeleportExit()
    {
        teleportmap.SetActive(false);
    }
    public bool Escape()
    {
        int mask = LayerMask.GetMask("Escape");
        Collider[] escapeCollider = Physics.OverlapSphere(transform.position, 3f, mask);
        if (escapeCollider.Length >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void DoubleTargetting()
    {
#if UNITY_STANDALONE_WIN
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int mask = LayerMask.GetMask("Enemy");
        float range = 5;
        vPos = transform.position;
#elif UNITY_ANDROID
        Touch touch = Input.GetTouch(0);
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;
        int mask = LayerMask.GetMask("Enemy");
        float range = 5;
        vPos = transform.position;
#endif

        if (Physics.SphereCast(ray, 0.5f, out hit, Mathf.Infinity, mask))
        {
            float dist = Vector3.Distance(hit.transform.position, vPos);
            if (hit.collider.CompareTag("Monster"))
            {
                Target = hit.collider.gameObject;
                if (dist <= range)
                {
                    transform.LookAt(Target.transform.position);
                    mousePos = this.transform.position;
                    if (Target != null)
                    {
                        StartCoroutine(DoubleAttack());
                        stat.StartCoroutine(stat.QcoolTime());
                        stat.mp -= 15;

                    }
                }
                if (dist > range)
                {
                    Vector3 destination = vPos + (hit.point - vPos).normalized * (dist - (range - 0.5f));
                    agent.SetDestination(destination);
                    StartCoroutine(CheckArrival(destination));
                }
            }
            else
            {
                Target = null;
            }
        }
    }
    IEnumerator CheckArrival(Vector3 destination)
    {
        while (Vector3.Distance(transform.position, destination) > agent.stoppingDistance + 0.5f)
        {
            yield return null;
        }
        StartCoroutine(DoubleAttack());
        stat.StartCoroutine(stat.QcoolTime());
        stat.mp -= 15;
        agent.SetDestination(this.transform.position);
        mousePos = this.transform.position;
    }
    IEnumerator WColl()
    {
        yield return new WaitForSeconds(stat.Wcool);
    }
    IEnumerator RSkill()
    {
        _IsMove = false;
        _IsAttack = false;
        mousePos = transform.position;
        agent.SetDestination(transform.position);
        iceFeild.SetActive(true);
        yield return new WaitForSeconds(1f);
        int mask = LayerMask.GetMask("Enemy");
        float Range = 6f;
        Collider[] TargetCollider = Physics.OverlapSphere(transform.position, Range, mask);
        for (int i = 0; i < TargetCollider.Length; i++)
        {
            MonStat target = TargetCollider[i].GetComponent<MonStat>();
            stat.Attack(target);
            GameManager.GetInstance().SomeMethod(target.transform.position + new Vector3(1, 3f, 0), stat.atk * 2);
            target.StartCoroutine(target.Stun(2f));
        }
        iceFeild.SetActive(false);
        _IsMove = true;
        _IsAttack = true;

        yield return new WaitForSeconds(stat.Rcool - 1f);
    }
    IEnumerator DSkill()
    {
        agent.speed = 4f;
        yield return new WaitForSeconds(5f);
        agent.speed = 3f;
        yield return new WaitForSeconds(stat.Dcool - 5f);
    }
    IEnumerator FSkill()
    {
        int mask = LayerMask.GetMask("Plane");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, mask))
        {
            transform.LookAt(hit.point);
            mousePos = hit.point;
            float dist = Vector3.Distance(mousePos, transform.position);
            if (dist > 5f)
            {
                Vector3 dir = (mousePos - transform.position).normalized;
                Vector3 targetPosition = transform.position + dir * 5f;
                agent.Warp(targetPosition);
                agent.SetDestination(targetPosition);
                _Fkey = false;
            }
            else
            {
                agent.Warp(mousePos);
                agent.SetDestination(transform.position);
                _Fkey = false;
            }
            mousePos = transform.position;
            yield return new WaitForSeconds(stat.Fcool);
        }
    }
    IEnumerator Fireball()
    {
        if (!_Wkey)
        {
            _IsAttack = false;
            _IsMove = false;
            transform.LookAt(Target.transform);
            anim.SetTrigger("IsAttack");
            yield return new WaitForSeconds(0.5f);
            if (Target != null)
            {
                magic.Shot(Target, GetComponent<Stat>());
                stat.curexp++;
                yield return new WaitForSeconds(attackDelay - 0.5f);
                _IsAttack = true;
                _IsMove = true;
            }
            else
            {
                _IsAttack = true;
                _IsMove = true;
            }
        }
    }
    IEnumerator DoubleAttack()
    {
        if (!_Wkey)
        {
            agent.SetDestination(this.transform.position);
            mousePos = transform.position;
            anim.SetTrigger("IsAttack");
            _IsAttack = false;
            _IsMove = false;
            yield return new WaitForSeconds(0.5f);
            if (Target != null)
            {
                transform.LookAt(Target.transform.position);
                magic.Shot(Target, GetComponent<Stat>());
                stat.curexp++;
                yield return new WaitForSeconds(0.2f);
                magic.Shot(Target, GetComponent<Stat>());
                yield return new WaitForSeconds(attackDelay - 0.5f);
                _IsAttack = true;
                _IsMove = true;
                _Qkey = false;
            }
            else
            {
                _IsAttack = true;
                _IsMove = true;
            }
        }
    }

    public void QskillButton()
    {
        if (_IsAttack)
        {
            if (stat.Q)
            {
                _Qkey = true;
            }
        }
    }
    public void WskillButton()
    {
        if (_IsAttack)
        {
            if (stat.W)
            {
                _Wkey = true;
            }
        }
    }
    public void EskillButton()
    {
        if (stat.E)
        {
            if (stat.mp >= 150)
            {
                _IsResting = false;
                _IsProduce = false;
                stat.mp -= 150;
                StartCoroutine(ESkill());
                stat.StartCoroutine(stat.EcoolTime());
            }
        }
    }
    public void RskillButton()
    {
        if (_IsAttack)
        {
            if (stat.R)
            {
                if (stat.mp >= 250)
                {
                    _IsResting = false;
                    _IsProduce = false;
                    stat.mp -= 250;
                    stat.StartCoroutine(stat.RcoolTime());
                    StartCoroutine(RSkill());
                }
            }
        }
    }
    public void DskillButton()
    {
        if (stat.D)
        {
            _IsResting = false;
            _IsProduce = false;
            stat.StartCoroutine(stat.DcoolTime());
            StartCoroutine(DSkill());
        }
    }
    public void FskillButton()
    {
        if (_IsAttack)
        {
            if (stat.F)
            {
                _IsResting = false;
                _IsProduce = false;
                _Fkey = true;
            }
        }
    }
    IEnumerator ESkill()
    {
        float time = 0;
        stat.shild = stat.lv * 150;
        shiled.SetActive(true);
        shiledbuffbase.gameObject.SetActive(true);
        shiledbuff.fillAmount = 0;
        while (time < 5)
        {
            time += Time.deltaTime;
            shiledbuff.fillAmount = time / 5f;
            if (time >= 5)
            {
                shiledbuff.fillAmount = 1;
                stat.shild = 0;
                shiledbuffbase.gameObject.SetActive(false);
                shiled.SetActive(false);
            }
            yield return null;
        }
    }
}
