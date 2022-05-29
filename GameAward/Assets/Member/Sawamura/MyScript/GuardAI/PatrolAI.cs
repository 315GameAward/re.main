//一定範囲だけ移動したい場合と
//一定範囲＋攻撃をしたい場合があると思うが、
//その場合はスクリプトを分ける又は、ツリーを増やす必要がある


using System.Collections.Generic;
using BehaviorTree;

public class PatrolAI : Tree
{
    public UnityEngine.Transform[] waypoints; //進むべきマーカーがwaypointsでない

    public static float speed = 2f;
    public static float fovRange = 6f;
    public static float attackRange = 1f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckEnemyInAttackRange(transform),
                new TaskAttack(transform),
            }),
            new Sequence(new List<Node>
            {
                new CheckEnemyInFOVRange(transform),
                new TaskGoToTarget(transform),
            }),
            new TaskPatrol(transform,waypoints),
        });


        return root;
    } 
    
}
