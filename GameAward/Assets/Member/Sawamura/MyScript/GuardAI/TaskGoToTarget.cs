using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using BehaviorTree;

public class TaskGoToTarget : Node
{
    private Transform __transform;

    private Vector3 m_Position;

    private Rigidbody m_Rigidbody;

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
        else if (__transform.position.y <= target.position.y)
        {
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
        }

        state = NodeState.RUNNING;
        return state;
    }
    

}
