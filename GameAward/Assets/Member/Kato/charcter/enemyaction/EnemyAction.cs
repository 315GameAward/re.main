using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyTurn : FSMnord
{

    public override void Start(FSMcontllole Contloller)
    {

        float rand = Random.Range(-90, 90);
        float retu = Random.Range(-20, 20);
        Vector3 Rote = Contloller.Parent.gameObject.transform.rotation.eulerAngles;
        base.Start(Contloller);
        Contloller.changenode("move");
        Contloller.Parent.gameObject.transform.rotation = Quaternion.Euler(Rote.x, Rote.y + retu, Rote.z + 180);
    }
    public override void Update(FSMcontllole Contloller)
    {
        base.Update(Contloller);
    }
    public override void End(FSMcontllole Contloller)
    {
        base.End(Contloller);
    }
}
public class Enemymove : FSMnord
{

    Enemy enemy;

    public override void Start(FSMcontllole Contloller)
    {
        base.Start(Contloller);
        enemy = Contloller.Parent.gameObject.GetComponent<Enemy>();
    }
    public override void Update(FSMcontllole Contloller)
    {

        Vector3 velocity;
        Vector3 ander;
        ander = new Vector3(0, 90, 0);
        base.Update(Contloller);

        velocity = Contloller.Parent.gameObject.transform.right * Time.deltaTime * 2;//forward * Time.deltaTime * 2 *-1;
        Contloller.Parent.gameObject.transform.position += velocity;

        if (enemy.HitEnemy == enemy)
        {
            Contloller.changenode("turn");
        }
        if (enemy.Hitray == false)
        {
            Contloller.changenode("turn");
        }
        if (enemy.HitGround == true)
        {
            Contloller.changenode("Foll");
        }

        if (enemy.isnullTarget == false)
        {
            if (enemy.Distance < 3.0f)
            {
                Contloller.changenode("assault");
                //Contloller.changenode("Foll");
            }

        }
    }
    public override void End(FSMcontllole Contloller)
    {
        base.End(Contloller);
    }
}

public class EnemyPlayerAssault : FSMnord
{
    Enemy enemy;

    public override void Start(FSMcontllole Contloller)
    {

        enemy = Contloller.Parent.gameObject.GetComponent<Enemy>();
        base.Start(Contloller);

        enemy.transform.LookAt(enemy.tage);
        enemy.transform.rotation = Quaternion.Euler(0, enemy.transform.rotation.eulerAngles.y, 0);

    }
    public override void Update(FSMcontllole Contloller)
    {


        base.Update(Contloller);

        UnityEngine.Quaternion vec = Contloller.Parent.gameObject.transform.rotation;
        Vector3 velocity = Contloller.Parent.gameObject.transform.forward * Time.deltaTime * 12;
        if (vec.z == 0)
        {
            Contloller.Parent.gameObject.transform.position += velocity;
        }
        if (vec.z == 180)
        {
            Contloller.Parent.gameObject.transform.position -= velocity;
        }
        if (enemy.Hitray == false)
        {
            Contloller.changenode("turn");

        }
        //   enemy.
        //velocity = Contloller.Parent.gameObject.transform.forward * Time.deltaTime * 6;
    }
    public override void End(FSMcontllole Contloller)
    {
        base.End(Contloller);
    }
}

public class EnemyFoll : FSMnord
{
    Enemy enemy;


    //private CharacterController controller;

    public override void Start(FSMcontllole Contloller)
    {
        enemy = Contloller.Parent.gameObject.GetComponent<Enemy>();
        base.Start(Contloller);
        //enemy.Cap.enabled = false;
    }
    public override void Update(FSMcontllole Contloller)
    {

        base.Update(Contloller);
    }

    public override void End(FSMcontllole Contloller)
    {
        base.End(Contloller);
    }
}
public class EnemyDelete : FSMnord
{
    Enemy enemy;
    public override void Start(FSMcontllole Contloller)
    {
        enemy = Contloller.Parent.gameObject.GetComponent<Enemy>();
        base.Start(Contloller);
    }
    public override void Update(FSMcontllole Contloller)
    {
        base.Update(Contloller);
    }
    public override void End(FSMcontllole Contloller)
    {
        base.End(Contloller);
    }
}