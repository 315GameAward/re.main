using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public enum ColGame_a
//{
//    GameStart,      //�Q�[���X�^�[�g
//    GameClear,
//    GameOver        //�Q�[���I�[�o�[
//}

public class IventGet_col_a : MonoBehaviour
{
    //�V���A���C�Y
    //�Q�[���I�[�o�[�pUI
    //[SerializeField]
    //GameObject gameoverUI;

    //public ColGame_a status_col;
    public IventPush_col_a[] target;

    int i = 0;

    void OnDisable()
    {
        target[i].OnCollisioned.RemoveAllListeners();
    }
    void OnEnable()
    {
        target[i].OnCollisioned.AddListener(() => {
            Debug.Log("target���Ԃ���܂���");
            // �����ɏ�����ǉ�
            //status_col = ColGame_a.GameOver;
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        //�X�e�[�^�X��GameStart��
        //status_col = ColGame_a.GameStart;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    //�V���ǉ�
    //    // �R�E�d�L����
    //    // C�L�[�ł��\���ł���悤�ɂ��܂���
    //    if (status_col == ColGame_a.GameOver || Input.GetKeyUp(KeyCode.C))
    //    {
    //        gameoverUI.SetActive(true);
    //    }
    //    if( Input.GetKeyUp(KeyCode.R))
    //    {
    //        gameoverUI.SetActive(false);
    //    }
    //}
}
