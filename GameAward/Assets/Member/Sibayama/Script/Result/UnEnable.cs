//===============================================================
//
//      UnEnable.cs
//      �I�u�W�F�N�g�̔�A�N�e�B�u��
//
//---------------------------------------------------------------
//      �쐬��: �ĎR꣑��Y
//      �쐬�J�n���F2022/5/26(��)
//===============================================================

//===============================================================
//      <=�J������=>
//---------------------------------------------------------------
//      ���e�FStart�֐��ɃR�����g�ǉ�
//      �ҏW�ҁF�ĎR꣑��Y
//      �ҏW���F2022/10/03(��)
//===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnEnable : MonoBehaviour
{
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {// �I�u�W�F�N�g���A�N�e�B�u�ɂ���
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
