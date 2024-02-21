using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Magic : MonoBehaviour
{
    public GameObject prefabFireBall;
    public GameObject prefabExplosion;
    static Magic Instance;
    public AudioSource fireballSource;
    public List<FireBall> listFireball;
    public static Queue<FireBall> queUsePool = new Queue<FireBall>();
    public static Queue<FireBall> queDisablePool = new Queue<FireBall>();
    public Vector3 vStart;
    public Animator anim;

    private void Awake()
    {
        Instance = this;
        fireballSource = GetComponent<AudioSource>();
    }

    public static Magic GetInstance()
    {
        return Instance;
    }
    void Start()
    {
        listFireball = new List<FireBall>(5);
        for (int i = 0; i < 5; i++)
        {
            GameObject objFireball = Instantiate(prefabFireBall);
            objFireball.transform.position = this.transform.position;
            FireBall cFireball = objFireball.GetComponent<FireBall>();
            listFireball.Add(cFireball);
            queDisablePool.Enqueue(cFireball);
        }
    }
    public void Shot(GameObject Target,Stat master)
    {
        if (queDisablePool.Count >= 0)
        {
            vStart = this.transform.position;
            FireBall fireball = queDisablePool.Dequeue();
            queUsePool.Enqueue(fireball);
            fireball.transform.position = this.transform.position;
            fireball.master = master;
            fireball.target = Target;
            fireballSource.Play();
            if (fireball.target != null)
            {
                fireball.Move();
                fireball.transform.Translate(Target.transform.position);
                fireball.transform.position = this.transform.position;
            }
        }
    }
    public void Explosion(Stat stat)
    {
        StartCoroutine(explosion(stat));
    }
    public IEnumerator explosion(Stat master)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int mask = LayerMask.GetMask("Plane");
        if (Physics.Raycast(ray, out hit, mask))
        {
            float dist = Vector3.Distance(hit.point, transform.position);
            if (dist > 5f)
            {
                PlayerInput.GetInstance().agent.SetDestination(hit.point);
                while (true)
                {
                    dist = Vector3.Distance(hit.point, transform.position);
                    if (dist < 5f)
                    {
                        PlayerInput.GetInstance().agent.SetDestination(transform.position);
                        PlayerInput.GetInstance()._IsMove = false;
                        anim.speed = 1.5f;
                        anim.SetTrigger("IsAttack");
                        PlayerInput.GetInstance().transform.LookAt(hit.point);
                        PlayerInput.GetInstance().ChangAni(PlayerInput.Action.Idle);
                        yield return new WaitForSeconds(0.5f);
                        GameObject copyExplosion = Instantiate(prefabExplosion);
                        Explosion explosion = copyExplosion.GetComponent<Explosion>();
                        explosion.transform.position = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);
                        explosion.master = master;
                        PlayerInput.GetInstance().ExplosionSound();
                        PlayerInput.GetInstance().mousePos = PlayerInput.GetInstance().transform.position;
                        PlayerInput.GetInstance().agent.SetDestination(PlayerInput.GetInstance().transform.position);
                        PlayerInput.GetInstance().stat.StartCoroutine(PlayerInput.GetInstance().stat.WcoolTime());
                        PlayerInput.GetInstance().stat.mp -= 150;
                        PlayerInput.GetInstance()._IsMove = true;
                        break;
                    }
                    yield return null;
                }
            }
            else
            {
                PlayerInput.GetInstance().agent.SetDestination(transform.position);
                PlayerInput.GetInstance()._IsMove = false;
                anim.speed = 1.5f;
                anim.SetTrigger("IsAttack");
                PlayerInput.GetInstance().transform.LookAt(hit.point);
                PlayerInput.GetInstance().ChangAni(PlayerInput.Action.Idle);
                yield return new WaitForSeconds(0.5f);
                GameObject copyExplosion = Instantiate(prefabExplosion);
                Explosion explosion = copyExplosion.GetComponent<Explosion>();
                explosion.transform.position = new Vector3(hit.point.x, 12.1f, hit.point.z);
                explosion.master = master;
                PlayerInput.GetInstance().ExplosionSound();
                PlayerInput.GetInstance().stat.StartCoroutine(PlayerInput.GetInstance().stat.WcoolTime());
                PlayerInput.GetInstance().stat.mp -= 150;
                PlayerInput.GetInstance()._IsMove = true;
               
            }
        }
    }
}
