//================================================
//
//      StageSelect.cs
//      ステージの選択
//
//------------------------------------------------
//      作成者: 道塚悠基
//================================================

//================================================
// 開発履歴
// 2022/04/04 作成開始
// 編集者: 道塚悠基
//================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    private ControlBinds _gameInputs;               //インプット
    private Vector2 _moveStickValue;                //スティック移動量
    private int stageSelect;                        //何を選択しているか

    [SerializeField] private GameObject selecting;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip selChange;
    [SerializeField] private AudioClip select;

    [SerializeField] private Image g_1;
    [SerializeField] private Image g_2;
    [SerializeField] private Image g_3;
    [SerializeField] private Image g_4;
    [SerializeField] private Image g_5;


    [SerializeField] private Sprite  _1;
    [SerializeField] private Sprite c_1;
    [SerializeField] private Sprite  _2;
    [SerializeField] private Sprite  c_2;
    [SerializeField] private Sprite  _3;
    [SerializeField] private Sprite c_3;
    [SerializeField] private Sprite  _4;
    [SerializeField] private Sprite c_4;
    [SerializeField] private Sprite  _5;
    [SerializeField] private Sprite c_5;

    private void Awake()
    {
        //InputActionインスタンス生成
        _gameInputs = new ControlBinds();

        //項目移動イベント登録
        _gameInputs.Player.MoveSelect.performed += OnMoveSelect;

        //Selectイベント登録
        _gameInputs.Player.Select.started += OnSelect;

        //Pauseイベント登録(Back)
        _gameInputs.Player.Pause.started += OnPause;

        //InputAction有効化
        _gameInputs.Enable();
    }

    private void Start()
    {
        stageSelect = 0;
    }

    private void OnMoveSelect(InputAction.CallbackContext context)
    {
        //Moveアクションの入力取得
        _moveStickValue = context.ReadValue<Vector2>();

        //現在のシーン名取得
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "StageSelect" || currentScene.name == "StageSelect2" || currentScene.name == "StageSelect3")
        {
            if (_moveStickValue.x >= 0.1f) //right arrow
            {
                if (stageSelect != 4)
                {
                    audioSource.PlayOneShot(selChange);
                }

                if (stageSelect == 0)
                {
                    g_1.sprite = c_1;
                    stageSelect = 1;
                    selecting.transform.localPosition = new Vector3(-200.0f, -240.0f, transform.position.z);
                }
                else if (stageSelect == 1)
                {
                    g_2.sprite = c_2;
                    stageSelect = 2;
                    selecting.transform.localPosition = new Vector3(-10.0f, -240.0f, transform.position.z);
                }
                else if (stageSelect == 2)
                {
                    g_3.sprite = c_3;
                    stageSelect = 3;
                    selecting.transform.localPosition = new Vector3(190.0f, -240.0f, transform.position.z);
                }
                else if (stageSelect == 3)
                {
                    g_4.sprite = c_4;
                    stageSelect = 4;
                    selecting.transform.localPosition = new Vector3(390.0f, -240.0f, transform.position.z);
                }
            }

            if (_moveStickValue.x <= -0.1f)
            {
                if (stageSelect != 0)
                {
                    audioSource.PlayOneShot(selChange);
                }

                if (stageSelect == 1)
                {
                    stageSelect = 0;
                    selecting.transform.localPosition = new Vector3(-410.0f, -240.0f, transform.position.z);
                }
                else if (stageSelect == 2)
                {
                    stageSelect = 1;
                    selecting.transform.localPosition = new Vector3(-190.0f, -240.0f, transform.position.z);
                }
                else if (stageSelect == 3)
                {
                    stageSelect = 2;
                    selecting.transform.localPosition = new Vector3(-10.0f, -240.0f, transform.position.z);
                }
                else if (stageSelect == 4)
                {
                    stageSelect = 3;
                    selecting.transform.localPosition = new Vector3(190.0f, -240.0f, transform.position.z);
                }
            }
        }
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        //現在のシーン名取得
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "StageSelect")     //ステージセレクトシーン内の場合
        {
            audioSource.PlayOneShot(select);
            //1-1
            if (stageSelect == 0)
            {
                SceneManager.LoadScene("1-1");
            }
            //1-2
            else if (stageSelect == 1)
            {
                SceneManager.LoadScene("1-2");
            }
            //1-3
            else if (stageSelect == 2)
            {
                SceneManager.LoadScene("1-3");
            }
            //1-4
            else if (stageSelect == 3)
            {
                SceneManager.LoadScene("1-4");
            }
            //1-5
            else if (stageSelect == 4)
            {
                SceneManager.LoadScene("1-5");
            }
        }

        if (currentScene.name == "StageSelect2")
        {
            audioSource.PlayOneShot(select);
            //1-1
            if (stageSelect == 0)
            {
                SceneManager.LoadScene("2-1");
            }
            //1-2
            else if (stageSelect == 1)
            {
                SceneManager.LoadScene("2-2");
            }
            //1-3
            else if (stageSelect == 2)
            {
                SceneManager.LoadScene("2-3");
            }
            //1-4
            else if (stageSelect == 3)
            {
                SceneManager.LoadScene("2-4");
            }
            //1-5
            else if (stageSelect == 4)
            {
                SceneManager.LoadScene("2-5");
            }
        }

        if (currentScene.name == "StageSelect3")
        {
            audioSource.PlayOneShot(select);
            //1-1
            if (stageSelect == 0)
            {
                SceneManager.LoadScene("3-1");
            }
            //1-2
            else if (stageSelect == 1)
            {
                SceneManager.LoadScene("3-2");
            }
            //1-3
            else if (stageSelect == 2)
            {
                SceneManager.LoadScene("3-3");
            }
            //1-4
            else if (stageSelect == 3)
            {
                SceneManager.LoadScene("3-4");
            }
            //1-5
            else if (stageSelect == 4)
            {
                SceneManager.LoadScene("3-5");
            }
        }
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        //現在のシーン名取得
        Scene currentScene = SceneManager.GetActiveScene();

        if(currentScene.name == "StageSelect" || currentScene.name == "StageSelect2" || currentScene.name == "StageSelect3")
        {
            SceneManager.LoadScene("AreaSelect");
        }
    }
}