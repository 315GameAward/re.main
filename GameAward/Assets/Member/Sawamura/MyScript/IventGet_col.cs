using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ColGame
{
    GameStart,      //�Q�[���X�^�[�g
    GameClear,
    GameOver        //�Q�[���I�[�o�[
}

public class IventGet_col : MonoBehaviour
{
    //�V���A���C�Y
    //�Q�[���I�[�o�[�pUI
    [SerializeField]
    GameObject gameoverUI;

    public ColGame status_col;
    public IventPush_col target;

    void OnDisable()
    {
        target.OnCollisioned.RemoveAllListeners();
    }
    void OnEnable()
    {
        target.OnCollisioned.AddListener(() => {
            Debug.Log("target���Ԃ���܂���");
            // �����ɏ�����ǉ�
            status_col = ColGame.GameOver;
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        //�X�e�[�^�X��GameStart��
        status_col = ColGame.GameStart;
    }

    // Update is called once per frame
    void Update()
    {
        //�V���ǉ�
        if (status_col == ColGame.GameOver)
        {
            gameoverUI.SetActive(true);
        }
    }
}
