using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{

    private MoveEnemy moveEnemy;
    private bool Pcol = false;

    void Start()
    {
        moveEnemy = GetComponentInParent<MoveEnemy>();

    }
    void OnTriggerStay(Collider col)
    {
        //　プレイヤーキャラクターを発見
        if (col.gameObject.tag == "Player")
        {
            //　敵キャラクターの状態を取得
            MoveEnemy.EnemyState state = moveEnemy.GetState();
            Pcol = true;
            //　敵キャラクターが追いかける状態でなければ追いかける設定に変更
            if (state != MoveEnemy.EnemyState.Attack)
            {
                // Debug.Log("プレイヤー攻撃");
                moveEnemy.SetState(MoveEnemy.EnemyState.Attack, col.transform);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("否攻撃");
            Pcol = false;
            moveEnemy.SetState(MoveEnemy.EnemyState.Wait);
        }
    }
    public bool GetPcol()
    {
        return Pcol;
    }
}
