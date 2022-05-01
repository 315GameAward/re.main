using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassAnimation : MonoBehaviour
{
    // Animator コンポーネント
    private Animator animator;
    private MoveEnemy enemy;
    private GameObject Cmps;
    // 設定したフラグの名前
    private const string key_isWalk = "isWalk";
    private const string key_isAttack = "isAttack";
   
    // 初期化メソッド
    void Start()
    {
        Cmps = GameObject.Find("Compass");
        enemy = Cmps.GetComponent<MoveEnemy>();

       // 自分に設定されているAnimatorコンポーネントを習得する
       this.animator = GetComponent<Animator>();
        
    }

    // 1フレームに1回コールされる
    void Update()
    {
        
        if (enemy.state == MoveEnemy.EnemyState.Walk)
        {
            Debug.Log(enemy.state);
            this.animator.SetBool(key_isWalk, true);
            this.animator.SetBool(key_isAttack, false);
        }
        else if(enemy.state == MoveEnemy.EnemyState.Attack)
        {
            this.animator.SetBool(key_isWalk, false);
            this.animator.SetBool(key_isAttack, true);

        }
        // 矢印下ボタンを押下している
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // WaitからWalkに遷移する
            this.animator.SetBool(key_isWalk, true);
        }
        else
        {
            // WalkからWaitに遷移する
            this.animator.SetBool(key_isWalk, false);
        }

        // Wait or Run からJumpに切り替える処理
        // スペースキーを押下している
        if (Input.GetKey(KeyCode.Space))
        {
            // Wait or WalkからAttackに遷移する
            this.animator.SetBool(key_isAttack, true);
        }
        else
        {
            // AttackからWait or Walkに遷移する
            this.animator.SetBool(key_isAttack, false);
        }
    }
}
