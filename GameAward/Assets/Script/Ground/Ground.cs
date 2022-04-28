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
    bool fade = false;
    float alpha = 1.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ���̃I�u�W�F�N�g���폜
        if (fade)
        {
            alpha -= 0.003f;
            gameObject.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, alpha);
            if (alpha < 0.0f) Destroy(gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Plane (2)")
        {
            fade = true;
        }
    }
}
