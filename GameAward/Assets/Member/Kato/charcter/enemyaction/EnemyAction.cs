
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyTurn : FSMnord
{

    public override void Start(FSMcontllole Contloller)
    {
        float rand = Random.Range(-90, 90);
        float retu = Random.Range(-5, 5);
        Vector3 Rote = Contloller.Parent.gameObject.transform.rotation.eulerAngles;
        base.Start(Contloller);
        Contloller.changenode("move");
        Contloller.Parent.gameObject.transform.rotation = Quaternion.Euler(Rote.x, Rote.y + 180 + retu, Rote.z);
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

        base.Update(Contloller);
        velocity = Contloller.Parent.gameObject.transform.forward * Time.deltaTime * 6;
        Contloller.Parent.gameObject.transform.position += velocity;

        if (enemy.HitEnemy == enemy)
        {
            Contloller.changenode("turn");
        }
        if (enemy.Hitray == false)
        {
            Contloller.changenode("turn");
        }
        if(enemy.HitGround==true)
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
        //if (enemy.HitGround == false)
        //{
        //    Contloller.changenode("Foll");
        //}


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


        Vector3 velocity = Contloller.Parent.gameObject.transform.forward * Time.deltaTime * 12;
        Contloller.Parent.gameObject.transform.position += velocity;
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