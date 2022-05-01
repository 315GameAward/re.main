using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassAnimation : MonoBehaviour
{
    // Animator �R���|�[�l���g
    private Animator animator;
    private MoveEnemy enemy;
    private GameObject Cmps;
    // �ݒ肵���t���O�̖��O
    private const string key_isWalk = "isWalk";
    private const string key_isAttack = "isAttack";
   
    // ���������\�b�h
    void Start()
    {
        Cmps = GameObject.Find("Compass");
        enemy = Cmps.GetComponent<MoveEnemy>();

       // �����ɐݒ肳��Ă���Animator�R���|�[�l���g���K������
       this.animator = GetComponent<Animator>();
        
    }

    // 1�t���[����1��R�[�������
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
        // ��󉺃{�^�����������Ă���
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // Wait����Walk�ɑJ�ڂ���
            this.animator.SetBool(key_isWalk, true);
        }
        else
        {
            // Walk����Wait�ɑJ�ڂ���
            this.animator.SetBool(key_isWalk, false);
        }

        // Wait or Run ����Jump�ɐ؂�ւ��鏈��
        // �X�y�[�X�L�[���������Ă���
        if (Input.GetKey(KeyCode.Space))
        {
            // Wait or Walk����Attack�ɑJ�ڂ���
            this.animator.SetBool(key_isAttack, true);
        }
        else
        {
            // Attack����Wait or Walk�ɑJ�ڂ���
            this.animator.SetBool(key_isAttack, false);
        }
    }
}
