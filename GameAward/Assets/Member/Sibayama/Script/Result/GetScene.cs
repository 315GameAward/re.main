//===============================================================
//
//      GetScene.cs
//      �V�[���̎擾
//
//---------------------------------------------------------------
//      �쐬��: �ĎR꣑��Y
//      �쐬�J�n���F2022/5/26(��)
//===============================================================

//===============================================================
//      <=�J������=>
//---------------------------------------------------------------
//      ���e�FStart�֐��ɃR�����g�ǉ�
//            GetScene�N���X�Ő錾���Ă���ϐ��ɃR�����g��ǉ�
//      �ҏW�ҁF�ĎR꣑��Y
//      �ҏW���F2022/10/03(��)
//===============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetScene : MonoBehaviour
{
    static int _currentSceneArea;   // ���݂̃V�[��(�G���A)�̃C���f�b�N�X�擾
    static int _currentSceneStage;  // ���݂̃V�[��(�X�e�[�W)�̃C���f�b�N�X�擾
    public static int CurrentSceneArea { get { return _currentSceneArea; } }
    public static int CurrentSceneStage { get { return _currentSceneStage; } }
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject == GameObject.Find("UI"))
        {// �Z���N�g�V�[��(UI�I�u�W�F�N�g������)�Ȃ猻�݂̃G���A�V�[���擾
            _currentSceneArea = SceneManager.GetActiveScene().buildIndex;
        }
        if (gameObject == GameObject.Find("Scissor (1)"))
        {// �Q�[���V�[��(�n�T�~������)�Ȃ猻�݂̃X�e�[�W�V�[���擾
            _currentSceneStage = SceneManager.GetActiveScene().buildIndex;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
