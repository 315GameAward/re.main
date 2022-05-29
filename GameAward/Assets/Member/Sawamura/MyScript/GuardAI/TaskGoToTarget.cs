using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using BehaviorTree;

public class TaskGoToTarget : Node
{
    private Transform __transform;

    public TaskGoToTarget(Transform transform)
    {
        __transform = transform;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if (Vector3.Distance(__transform.position,target.position) > 0.01f)
        {
            __transform.position = Vector3.MoveTowards(
                __transform.position, target.position, PatrolAI.speed * Time.deltaTime);
            __transform.LookAt(target.position);
        }

        state = NodeState.RUNNING;
        return state;
    }
    

}
