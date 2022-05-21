using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassAnimation : MonoBehaviour
{
    // Animator �R���|�[�l���g
    private Animator animator;
    private MoveEnemy enemy;
    private GameObject Cmps;
    public GameObject AttackR;
    private AttackRange Pcol;
    // �ݒ肵���t���O�̖��O
    private const string key_isWalk = "isWalk";
    private const string key_isAttack = "isAttack";

    // ����
    private float timeOut = 4.5f;
    private float timeElapsed = 0.0f;

    // ���������\�b�h
    void Start()
    {

        Cmps = GameObject.Find("Compass");
        // AttackR = GameObject.Find("AttackRange");
        enemy = Cmps.GetComponent<MoveEnemy>();
        Pcol = AttackR.gameObject.GetComponent<AttackRange>();

        // �����ɐݒ肳��Ă���Animator�R���|�[�l���g���K������
        this.animator = GetComponent<Animator>();

    }

    // 1�t���[����1��R�[�������
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