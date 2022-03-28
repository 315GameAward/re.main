using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    bool hitray;
    bool hitground;
    Vector3 direction;
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

    public bool HitGround
    {
        get
        {
            return hitground;
        }
    }

    void Start()
    {
        FSM = GetComponent<FSM>();
        FSM.addnode("move", new Enemymove());
        FSM.addnode("turn", new EnemyTurn());
        FSM.addnode("assault", new EnemyPlayerAssault());
        FSM.startnode("move");
      //  FSM.addnode("Foll", new EnemyFoll());
       // FSM.addnode("Derete", new);
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
        RaycastHit rayhit;
        rayhit = new RaycastHit();
        Vector3 hiku;
        hiku = new Vector3(0, 1, 0);
        Vector3 angle = transform.forward - hiku;
        hitray = Physics.SphereCast(transform.position, 1, angle.normalized, out rayhit, 3, mask, QueryTriggerInteraction.Ignore);
        Vector3 foll;
        foll = new Vector3(0, -1, 0);
      //  Vector3 velocity = Contloller.Parent.gameObject.transform.position += foll;
    }

    private Vector3 moveRandomDirection()  // 目的地を生成、xとyのポジションをランダムに値を取得 
    {

        Vector3 randomDire = new Vector3(Random.Range(-100, 100), 1, Random.Range(-100, 100));
        return randomDire.normalized;
    }
}
