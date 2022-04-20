//========================
// 
//      Life
// 		�v���C���[�̗�
//
//--------------------------------------------
// �쐬�ҁF�㌎��n
//========================
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Life : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;  // �C���X�y�N�^�[����n�[�g�̃v���n�u�����蓖�Ă�

    public int nLife;   // �̗�

    // �z��
    List<GameObject> Lifes = new List<GameObject>();   // �����������C�t������
   
    // Start is called before the first frame update
    void Start()
    {
        for (int i = nLife; i > 0; i--)
        {
            // �̗͂�ݒ肵�Ă���
            AddLife();
        }
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(nLife);

        if (nLife <= 0)
        {
            // �Q�[���I�[�o�[�Ăяo��
            Debug.Log("�Q�[���I�[�o�[");
        }

        // �X�y�[�X����������̗͏���
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            AddLife();
            nLife++;
        }

        // ��������������̗͑���
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            DelLife();
        }
    }

    //========================
    //
    // �n�[�g��1���₷�֐�
    //
    //========================
    public void AddLife()
    {
        // ����2�͂ǂ̃I�u�W�F�N�g�̎q�ɂ��邩�ŁA����3�͎q�ɂ���ۂɈȑO�̈ʒu��ۂ�(LayoutGroup�n�ł�false�ɂ��Ȃ��Ƃ��������Ȃ�)
        GameObject instance = Instantiate(prefab, transform, false);
        Lifes.Add(instance);
        
    }

    //========================
    //
    // �n�[�g��1���炷�֐�
    //
    //========================
    public void DelLife()
    {
        Destroy(Lifes[0]);
        Lifes.RemoveAt(0);
        nLife--;
    }

}
