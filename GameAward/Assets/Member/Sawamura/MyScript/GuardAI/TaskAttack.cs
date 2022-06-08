using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using BehaviorTree;

public class TaskAttack : Node
{
    private Animator __animator;

    private Transform __lastTarget;
    private EnemyManager __enemyManager;

    private float __attackTime = 1f;
    private float __attackCounter = 0f;

    public TaskAttack(Transform transform)
    {
        __animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != __lastTarget)
        {
            __enemyManager = target.GetComponent<EnemyManager>();
            __lastTarget = target;
        }

        __attackCounter += Time.deltaTime;
        if(__attackCounter >= __attackTime)
        {
            bool enemyIsDead = __enemyManager.TakeHit();
            if (enemyIsDead)
            {
                ClearData("target");
                //__animator.SetBool("Attacking", false);   //アタックパターン2など
                __animator.SetBool("Attacking", false);
                __animator.SetBool("Walking", true);
            }
            else
            {
                __attackCounter = 0f;

            }

        }
        
        state = NodeState.RUNNING;
        return state;
    }


}
