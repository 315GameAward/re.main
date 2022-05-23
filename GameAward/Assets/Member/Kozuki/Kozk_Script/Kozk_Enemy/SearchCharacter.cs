using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class SearchCharacter : MonoBehaviour
{

    private MoveEnemy moveEnemy;

    private GameObject AttackR;
    private AttackRange Pcol;

    private float ChaseTime = 7.0f;
    private float WaitTime = 1.5f;
    public float cnttime = 0.0f;
    public bool b_Chase = true;

    void Start()
    {
        moveEnemy = GetComponentInParent<MoveEnemy>();

        AttackR = GameObject.Find("AttackRange");

        Pcol = AttackR.GetComponent<AttackRange>();
    }
    void OnTriggerStay(Collider col)
    {
        //　プレイヤーキャラクターを発見
        if (col.gameObject.tag == "Player")
        {
            if (b_Chase == true)
            {
                //　敵キャラクターの状態を取得
                MoveEnemy.EnemyState state = moveEnemy.GetState();
                bool pcol = Pcol.GetPcol();

                cnttime += Time.deltaTime;
                if (cnttime >= ChaseTime)
                {
                    cnttime = 0.0f;
                    b_Chase = false;
                }

                //　敵キャラクターが追いかける状態でなければ追いかける設定に変更
                if (pcol == false && state != MoveEnemy.EnemyState.Attack && state != MoveEnemy.EnemyState.Chase)
                {
                    //Debug.Log("プレイヤー発見");
                    moveEnemy.SetState(MoveEnemy.EnemyState.Chase, col.transform);
                }
            }

        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("見失う");
            cnttime = 0.0f;
            moveEnemy.SetState(MoveEnemy.EnemyState.Wait);
            b_Chase = true;
        }
    }
}