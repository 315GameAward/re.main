using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassAnimation : MonoBehaviour
{
    // Animator コンポーネント
    private Animator animator;
    private MoveEnemy enemy;
    private GameObject Cmps;
    public GameObject AttackR;
    private AttackRange Pcol;
    // 設定したフラグの名前
    private const string key_isWalk = "isWalk";
    private const string key_isAttack = "isAttack";

    // 時間
    private float timeOut = 4.5f;
    private float timeElapsed = 0.0f;

    // 初期化メソッド
    void Start()
    {

        Cmps = GameObject.Find("Compass");
        // AttackR = GameObject.Find("AttackRange");
        enemy = Cmps.GetComponent<MoveEnemy>();
        Pcol = AttackR.gameObject.GetComponent<AttackRange>();

        // 自分に設定されているAnimatorコンポーネントを習得する
        this.animator = GetComponent<Animator>();

    }

    // 1フレームに1回コールされる
    void Update()
    {
        if (Pcol.GetPcol() == true)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= timeOut)
            {
                this.animator.SetTrigger("AttackT");
                timeElapsed = 0.0f;
            }
        }
        else
        {
            this.animator.SetTrigger("WalkT");
            timeElapsed = 0.0f;
        }
    }
}