//======================================================
//
//        Ground.cs
//        �n�ʂ̏���
//
//------------------------------------------------------
//      �쐬��:���{���V��
//======================================================

//======================================================
// �J������
// 2022/02/16 �v���g�^�C�v�쐬�J�n
// �ҏW��:���{���V��
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ���̃I�u�W�F�N�g���폜
        if (gameObject.transform.position.y < -50) Destroy(gameObject);
    }
}
