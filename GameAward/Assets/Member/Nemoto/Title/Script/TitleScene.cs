using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    ControlBinds cb;    // �R���g���[���[�o�C���h

    [SerializeField] AudioSource selectSE;  // �Z���N�gSE

    public bool bTrigger = false; // �g���K�[�t���O


    // Start is called before the first frame update
    void Start()
    {
        // ������
        cb = new ControlBinds();
        bTrigger = false;

        // �C�x���g�̍쐬
        cb.TitleScene.MoveScene.started += OnMoveScene;
        cb.TitleScene.MoveScene.performed += OnMoveScene;

        // �g�p�\�ɂ���
        cb.TitleScene.Enable();

        //selectSE = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bTrigger)
        {
            // SE����I�������V�[���̈ړ�
            if (!selectSE.isPlaying)
            {
                // �V�[���̈ړ�
                SceneManager.LoadScene("AreaSelect");
                bTrigger = false;
            }
               
        }
        else
        {

        }
    }

    private void OnMoveScene(InputAction.CallbackContext context)
    {
        Debug.Log("�V�[���̈ړ�");

        selectSE.Play();    // SE�̍Đ�

        bTrigger = true;

       
        cb.TitleScene.Disable();
    }
}
