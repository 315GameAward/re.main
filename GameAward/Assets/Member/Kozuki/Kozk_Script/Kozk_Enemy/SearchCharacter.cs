using UnityEngine;
using System.Collections;

public class SearchCharacter : MonoBehaviour
{

    private MoveEnemy moveEnemy;

    private GameObject AttackR;
    private AttackRange Pcol;
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
            //　敵キャラクターの状態を取得
            MoveEnemy.EnemyState state = moveEnemy.GetState();
            bool pcol = Pcol.GetPcol();

            //　敵キャラクターが追いかける状態でなければ追いかける設定に変更
            if (pcol == false && state != MoveEnemy.EnemyState.Attack && state != MoveEnemy.EnemyState.Chase)
            {
                //Debug.Log("プレイヤー発見");
                moveEnemy.SetState(MoveEnemy.EnemyState.Chase, col.transform);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("見失う");
            moveEnemy.SetState(MoveEnemy.EnemyState.Wait);
        }
    }
}