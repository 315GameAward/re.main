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
        //�@�v���C���[�L�����N�^�[�𔭌�
        if (col.gameObject.tag == "Player")
        {
            //�@�G�L�����N�^�[�̏�Ԃ��擾
            MoveEnemy.EnemyState state = moveEnemy.GetState();
            Pcol = true;
            //�@�G�L�����N�^�[���ǂ��������ԂłȂ���Βǂ�������ݒ�ɕύX
            if (state != MoveEnemy.EnemyState.Attack)
            {
                // Debug.Log("�v���C���[�U��");
                moveEnemy.SetState(MoveEnemy.EnemyState.Attack, col.transform);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("�ۍU��");
            Pcol = false;
            moveEnemy.SetState(MoveEnemy.EnemyState.Wait);
        }
    }
    public bool GetPcol()
    {
        return Pcol;
    }
}
