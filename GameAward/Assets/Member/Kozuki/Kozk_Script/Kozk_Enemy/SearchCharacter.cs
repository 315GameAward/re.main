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
        //�@�v���C���[�L�����N�^�[�𔭌�
        if (col.gameObject.tag == "Player")
        {
            //�@�G�L�����N�^�[�̏�Ԃ��擾
            MoveEnemy.EnemyState state = moveEnemy.GetState();
            bool pcol = Pcol.GetPcol();

            //�@�G�L�����N�^�[���ǂ��������ԂłȂ���Βǂ�������ݒ�ɕύX
            if (pcol == false && state != MoveEnemy.EnemyState.Attack && state != MoveEnemy.EnemyState.Chase)
            {
                //Debug.Log("�v���C���[����");
                moveEnemy.SetState(MoveEnemy.EnemyState.Chase, col.transform);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("������");
            moveEnemy.SetState(MoveEnemy.EnemyState.Wait);
        }
    }
}