using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;//�L�[��������̏����̂���
using UnityEngine.SceneManagement;


public class SkipScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �X�y�[�X�L�[����������
        if (Keyboard.current.oKey.isPressed)
        {
            Debug.Log("�V�[���̈ړ�");
            SceneManager.LoadScene("GameScene");
        }
    }
}
