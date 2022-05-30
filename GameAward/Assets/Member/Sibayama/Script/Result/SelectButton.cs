//===============================================================
//
//      SelectButton.cs
//      �{�^���I��
//
//---------------------------------------------------------------
//      �쐬��: �ĎR꣑��Y
//      �쐬�J�n���F2022/5/26(��)
//===============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectButton : MonoBehaviour
{
    Button[] button = new Button[2];
    GameObject currentScene;
    GetScene scene;
    


    // Start is called before the first frame update
    void Start()
    {
        button[0] = GameObject.Find("ReturnSelect").GetComponent<Button>();
        button[1] = GameObject.Find("NextStage").GetComponent<Button>();
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

    public void NextStage()
    {
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
