using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //public CapsuleCollider Cap;
    private Vector3 moveDirection = Vector3.zero;

    bool a;
    bool hitray;
    bool hitenemy;
    bool hitground;

    bool enemyfoll;
    Vector3 direction;
    Vector3 targetchar;
    FSM FSM;
    [SerializeField] Transform Target;
    float distance;
    public Transform tage
    {
        get
        {
            return Target;
        }
    }
    public bool isnullTarget
    {
        get
        {
            return Target == null;
        }
    }

    public float Distance
    {
        get
        {
            return distance;
        }
    }

    public bool Hitray
    {
        get
        {
            return hitray;
        }
    }

    public bool HitEnemy
    {
        get
        {
            return hitenemy;
        }
    }
    public bool HitGround
    {
        get
        {
            return hitground;
        }
    }
    void Start()
    {

        //controller = GetComponent<CharacterController>();
        //rb = GetComponent<Rigidbody>();
        FSM = GetComponent<FSM>();
        FSM.addnode("move", new Enemymove());
        FSM.addnode("turn", new EnemyTurn());
        FSM.addnode("assault", new EnemyPlayerAssault());
        FSM.addnode("Foll", new EnemyFoll());
        FSM.addnode("Derete", new EnemyDelete());
        FSM.startnode("move");


    }

    void Update()
    {
        DoTarget();
        Dosperecast();
    }
    void DoTarget()
    {

        if (Target == null)
        {
            return;
        }
        distance = Vector3.Distance(Target.position, transform.position);

    }
    void Dosperecast()
    {

        int mask = LayerMask.GetMask("Default");
        int dark = LayerMask.GetMask("Enemy");
        int back = LayerMask.GetMask("BackGround");

        RaycastHit rayhit;
        //Raybrockhit 
        rayhit = new RaycastHit();
        Vector3 hiku;
        hiku = new Vector3(0, 1, 0);
        float en = 0.1f;
        float enemyserch = 0.3f;
        Vector3 angle = transform.right - hiku;

        hitray = Physics.SphereCast(transform.position, en, angle.normalized, out rayhit, 1, mask, QueryTriggerInteraction.Ignore);
        hitenemy = Physics.SphereCast(transform.position, enemyserch, angle.normalized, out rayhit, 1, dark, QueryTriggerInteraction.Ignore);
        Vector3 foll;
        foll = new Vector3(0, -1, 0);
        hitground = Physics.Raycast(transform.position, foll, out rayhit, 1, back, QueryTriggerInteraction.Ignore);

        Debug.DrawRay(transform.position, angle.normalized, Color.green);
    }
    void OnDrawGizmosSelected()
    {
        float en = 0.1f;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, en);
    }
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position + transform.forward, 1);
    //}

    private Vector3 moveRandomDirection()  // 目的地を生成、xとyのポジションをランダムに値を取得 
    {

        Vector3 randomDire = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
        return randomDire.normalized;
    }

}
