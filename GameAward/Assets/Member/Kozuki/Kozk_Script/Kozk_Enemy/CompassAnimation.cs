using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassAnimation : MonoBehaviour
{
    // Animator �R���|�[�l���g
    private Animator animator;
    private MoveEnemy enemy;
    private GameObject Cmps;
    private GameObject AttackR;
    private AttackRange Pcol;
    // �ݒ肵���t���O�̖��O
    private const string key_isWalk = "isWalk";
    private const string key_isAttack = "isAttack";

    // ���������\�b�h
    void Start()
    {
        Cmps = GameObject.Find("Compass");
        AttackR = GameObject.Find("AttackRange");
        enemy = Cmps.GetComponent<MoveEnemy>();
        Pcol = AttackR.GetComponent<AttackRange>();

        // �����ɐݒ肳��Ă���Animator�R���|�[�l���g���K������
        this.animator = GetComponent<Animator>();

    }

    // 1�t���[����1��R�[�������
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