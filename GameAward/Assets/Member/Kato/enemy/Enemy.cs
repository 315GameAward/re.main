using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    bool a;
    bool hitray;
    bool hitground;
    bool enemyfoll;
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
        //rb = GetComponent<Rigidbody>();
        FSM = GetComponent<FSM>();
        FSM.addnode("move", new Enemymove());
        FSM.addnode("turn", new EnemyTurn());
        FSM.addnode("assault", new EnemyPlayerAssault());
        FSM.startnode("move");
        //FSM.addnode("Foll", new EnemyFoll());
        //FSM.addnode("Derete", new EnemyDelete());
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
        int mask = LayerMask.GetMask("Ground");
        RaycastHit rayhit;
        rayhit = new RaycastHit();
        Vector3 hiku;
        hiku = new Vector3(0, 1, 0);
        Vector3 angle = transform.forward - hiku;
        hitray = Physics.SphereCast(transform.position, 1, angle.normalized, out rayhit, 1, mask, QueryTriggerInteraction.Ignore);
        //hitray = Physics.Raycast(transform.position, angle.normalized, out rayhit, 1, mask, QueryTriggerInteraction.Ignore,);
        Vector3 foll;
        foll = new Vector3(0, -1, 0);
        hitground = Physics.Raycast(transform.position, foll, out rayhit, 1, mask, QueryTriggerInteraction.Ignore);

        Debug.DrawRay(transform.position, angle.normalized, Color.green);

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1);
    }


    private Vector3 moveRandomDirection()  // �ړI�n�𐶐��Ax��y�̃|�W�V�����������_���ɒl���擾 
    {

        Vector3 randomDire = new Vector3(Random.Range(-100, 100), 1, Random.Range(-100, 100));
        return randomDire.normalized;
    }
    //public Rigidbody rb;

    // Let the rigidbody take control and detect collisions.

    //public void EnableRagdoll()
    //{
    //    Gravity();
    //
    //}
    //public void Gravity()
    //{
    //    //var Gra = rb.isKinematic = false;
    //    //var Shock = rb.detectCollisions = true;
    //    rb.isKinematic = true;
    //    rb.detectCollisions = false;
    //    //UnityEngine.Debug.Log("Hello World");
    //}
    //
    //
    //// Let animation control the rigidbody and ignore collisions.
    //public void DisableRagdoll()
    //{
    //    void disableragdoll()
    //    {
    //        rb.isKinematic = true;
    //        rb.detectCollisions = false;
    //    }
    //
    //}
}