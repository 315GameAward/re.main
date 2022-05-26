//======================================================
//
//       SkipScene.cs
//        �V�[�����΂�����
//
//------------------------------------------------------
//      �쐬��:��X����
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;//�L�[��������̏����̂���
using UnityEngine.SceneManagement;



public class SkipScene : MonoBehaviour
{

    // �X�L�b�v�p�ϐ�
    private float cnt_time = 0.0f;      // �^�C�}�[����p
    public float change_time = 22.0f;   // �J�ڗ\�莞��(�����l�F22.0f)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ���ԑ���
        cnt_time += Time.deltaTime;

        // �����J��
        if(cnt_time >= change_time)
        {
            cnt_time = 0.0f;
            SceneManager.LoadScene("GameScene");
        }

        // X�L�[����������
        if (Keyboard.current.xKey.isPressed)
        {
            cnt_time = 0.0f;
            SceneManager.LoadScene("GameScene");
        }
        if(Gamepad.current.xButton.wasPressedThisFrame)
        {
            cnt_time = 0.0f;
            SceneManager.LoadScene("GameScene");
        }
    }
}
