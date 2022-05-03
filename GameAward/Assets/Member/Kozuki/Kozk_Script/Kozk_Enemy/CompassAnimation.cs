using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassAnimation : MonoBehaviour
{
    // Animator コンポーネント
    private Animator animator;
    private MoveEnemy enemy;
    private GameObject Cmps;
    private GameObject AttackR;
    private AttackRange Pcol;
    // 設定したフラグの名前
    private const string key_isWalk = "isWalk";
    private const string key_isAttack = "isAttack";

    // 初期化メソッド
    void Start()
    {
        Cmps = GameObject.Find("Compass");
        AttackR = GameObject.Find("AttackRange");
        enemy = Cmps.GetComponent<MoveEnemy>();
        Pcol = AttackR.GetComponent<AttackRange>();

        // 自分に設定されているAnimatorコンポーネントを習得する
        this.animator = GetComponent<Animator>();

    }

    // 1フレームに1回コールされる
    void Update()
    {
        if (Pcol.GetPcol() == true)
        {
            this.animator.SetTrigger("AttackT");
        }
        else
        {
            this.animator.SetTrigger("WalkT");
        }
    }
}