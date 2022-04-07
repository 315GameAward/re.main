//========================
// 
//      Life
// 		�v���C���[�̗�
//
//--------------------------------------------
// �쐬�ҁF�㌎��n
//========================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    // �̗�
    public int nLife;

    [SerializeField]
    GameObject prefab;      // �C���X�y�N�^�[����n�[�g�̃v���n�u�����蓖�Ă�

    List<GameObject> hearts = new List<GameObject>();   // ���������n�[�g������
   
    //========================
    //
    // �n�[�g��1���炷�֐�
    //
    //========================
    public void DelLife()
    {
        nLife--;
        Destroy(hearts[0]);
        hearts.RemoveAt(0);
    }

    //========================
    //
    // �n�[�g��1���₷�֐�
    //
    //========================
    public void Increase()
    {
        // ����2�͂ǂ̃I�u�W�F�N�g�̎q�ɂ��邩�ŁA����3�͎q�ɂ���ۂɈȑO�̈ʒu��ۂ�(LayoutGroup�n�ł�false�ɂ��Ȃ��Ƃ��������Ȃ�)
        GameObject instance = Instantiate(prefab, transform, false);
        hearts.Add(instance);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = nLife; i > 0; i--)
        {
            Increase();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(nLife <= 0)
        {
            // �Q�[���I�[�o�[�Ăяo��
        }
    }
}
