using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckEnemyInAttackRange : Node
{
    //private static int __enemyLayeMask = 1 << 6;

    private Transform __transform;
    private Animator __animator;

    public CheckEnemyInAttackRange(Transform transform)
    {
        __transform = transform;
        __animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;
        if (Vector3.Distance(__transform.position, target.position) <= PatrolAI.attackRange)
        {
            __animator.SetBool("Attacking", true);
            __animator.SetBool("Walking", false);

            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
