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
    public int pLife;

    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        if(pLife <= 0)
        {
            // �Q�[���I�[�o�[�Ăяo��
        }
    }

    //========================
    //      
    //      �̗͌����֐�
    //
    //========================
    public void DelLife()
    {
        // �̗͂����炷
        pLife--;
    }
}
