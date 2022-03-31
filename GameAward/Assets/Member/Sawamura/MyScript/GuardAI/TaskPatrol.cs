using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskPatrol : Node
{
    private Transform __transform;
    private Transform[] __waypoints;

    private int __currentWaypointIndex = 0;

    private float __waitTime = 1f;
    private float __waitCounter = 0f;
    private bool __waiting = false;


    public TaskPatrol(Transform transform, Transform[] waypoints)
    {
        __transform = transform;
        __waypoints = waypoints;
    }

    public override NodeState Evaluate()
    {
        if (__waiting)
        {
            __waitCounter += Time.deltaTime;
            if (__waitCounter >= __waitTime)
                __waiting = false;

        }
        else
        {
            Transform wp = __waypoints[__currentWaypointIndex];
            if (Vector3.Distance(__transform.position,wp.position) < 0.01f)
            {
                __transform.position = wp.position;
                __waitCounter = 0f;
                __waiting = true;

                __currentWaypointIndex = (__currentWaypointIndex + 1) % __waypoints.Length;
                
            }
            else
            {
                __transform.position = Vector3.MoveTowards(__transform.position, wp.position, __speed * Time.deltaTime);
                __transform.LookAt(wp.position);
            }
            
        }
        state = NodeState.RUNNING;
        return state;
    }
}
