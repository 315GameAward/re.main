//===============================================================
//
//      SelectButton.cs
//      �{�^���I��
//
//---------------------------------------------------------------
//      �쐬��: �ĎR꣑��Y
//      �쐬�J�n���F2022/5/26(��)
//===============================================================

//===============================================================
//      <=�J������=>
//---------------------------------------------------------------
//      ���e�FStart�֐��ɃR�����g�ǉ�
//            NextStage�֐��ɃR�����g��ǉ�
//      �ҏW�ҁF�ĎR꣑��Y
//      �ҏW���F2022/10/03(��)
//===============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectButton : MonoBehaviour
{
    // �{�^���̃C���X�^���X
    Button[] button = new Button[2];

    // Start is called before the first frame update
    void Start()
    {
        // �{�^���̐U�蕪��
        button[0] = GameObject.Find("ReturnSelect").GetComponent<Button>();
        button[1] = GameObject.Find("NextStage").GetComponent<Button>();

        // �Z���N�g�{�^���Ƀt�H�[�J�X
        button[0].Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnSelect()
    {
        SceneManager.LoadScene(GetScene.CurrentSceneArea);
    }

    // �V�[���̑J�ڐ�𔻒�
    public void NextStage()
    {
        // ���݂̃V�[��(�X�e�[�W)�����Ɏ��̑J�ڐ�����߂�
        switch (GetScene.CurrentSceneStage)
        {
            case 9:// 1-5
                // �G���A�Q��
                SceneManager.LoadScene(GetScene.CurrentSceneArea + 1);
                break;
            case 14:// 2-5
                // �G���A�R��
                SceneManager.LoadScene(GetScene.CurrentSceneArea + 1);
                break;
            default:
                if ((GetScene.CurrentSceneStage + 1) > 19)
                {// �ŏI�G���A�A�ŏI�X�e�[�W���N���A������
                    SceneManager.LoadScene("TitleScene");
                }
                SceneManager.LoadScene((GetScene.CurrentSceneStage + 1));
                break;
        }
    }

}
