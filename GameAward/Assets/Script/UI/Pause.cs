//================================================
//
//      Pause.cs
//      ポーズ画面の開閉
//
//------------------------------------------------
//      作成者: 道塚悠基
//================================================

//================================================
// 開発履歴
// 2022/03/03 作成開始
// 編集者: 道塚悠基
//================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;    //ポーズ画面オブジェクト
    private GameObject pauseUIInstance;             //ポーズ画面用インスタンス

    public Animator tutorialAnime;                  //操作説明用アニメーション

    private ControlBinds _gameInputs;               //インプット
    private Vector2 _moveStickValue;                //スティック移動量

    private int pauseSelect;                        //ポーズで何を選択しているか

    private bool g_bPauseOpen;                      //ポーズを開いているか

    private float x0, x1, x2, x3;
    private float y0, y1, y2, y3;


    private void Awake()
    {
        //InputActionインスタンス生成
        _gameInputs = new ControlBinds();

        //Pauseイベント登録
        _gameInputs.Player.Pause.started += OnPause;

        //項目移動イベント登録
        _gameInputs.Player.MoveSelect.performed += OnMoveSelect;

        //Selectイベント登録
        _gameInputs.Player.Select.started += OnSelect;

        //InputAction有効化
        _gameInputs.Enable();
    }

    private void Start()
    {
        pauseSelect = 0;

        x0 = -1025.0f;
        x1 = -945.0f;
        x2 = -1590.0f;
        x3 = -705.0f;

        y0 = 250.0f;
        y1 = -125.0f;
        y2 = -500.0f;
        y3 = -875.0f;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (pauseUIInstance == null)    //ポーズUIがない場合
        {
            g_bPauseOpen = true;        //ポーズ開いてる判定
            Time.timeScale = 0.0f;      //時を止める
            pauseUIInstance = GameObject.Instantiate(pauseUI) as GameObject;    //ポーズUI設置
        }
        else
        {
            g_bPauseOpen = false;       //ポーズ開いてない判定
            Time.timeScale = 1.0f;      //時を動かす
            Destroy(pauseUIInstance);   //ポーズUI破壊
        }
    }

    private void OnMoveSelect(InputAction.CallbackContext context)
    {
        //Moveアクションの入力取得
        _moveStickValue = context.ReadValue<Vector2>();

        if (g_bPauseOpen) //ポーズを開いている場合
        {
            GameObject selectBtn = GameObject.Find("SelectBtn");
            if (_moveStickValue.x >= 0.1f)  //right arrow
            {
                if (pauseSelect == 0)
                {
                    pauseSelect = 1;
                    selectBtn.transform.localPosition = new Vector3(x1, y1, transform.position.z);
                }
                else if (pauseSelect == 1)
                {
                    pauseSelect = 2;
                    selectBtn.transform.localPosition = new Vector3(x2, y2, transform.position.z);
                }
                else if (pauseSelect == 2)
                {
                    pauseSelect = 3;
                    selectBtn.transform.localPosition = new Vector3(x3, y3, transform.position.z);
                }
            }

            if (_moveStickValue.x <= -0.1f)  //left arrow
            {
                if (pauseSelect == 3)
                {
                    pauseSelect = 2;
                    selectBtn.transform.localPosition = new Vector3(x2, y2, transform.position.z);
                }
                else if (pauseSelect == 2)
                {
                    pauseSelect = 1;
                    selectBtn.transform.localPosition = new Vector3(x1, y1, transform.position.z);
                }
                else if (pauseSelect == 1)
                {
                    pauseSelect = 0;
                    selectBtn.transform.localPosition = new Vector3(x0, y0, transform.position.z);
                }

            }
        }
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        if (g_bPauseOpen)
        {
            GameObject tutorialImg = GameObject.Find("TutorialImage");
            if (pauseSelect == 0)           //ゲームに戻る
            {
                g_bPauseOpen = false;       //ポーズ開いてない判定
                Time.timeScale = 1.0f;      //時を動かす
                Destroy(pauseUIInstance);   //ポーズUI破壊
            }
            else if (pauseSelect == 1)      //やり直す
            {
                SceneManager.LoadScene("OomoriScene");
            }
            else if (pauseSelect == 2)      //ステージセレクトに戻る
            {
                SceneManager.LoadScene("StageSelect");
            }
            else if (pauseSelect == 3)      //操作説明
            {
                //tutorialImg.SetActive(true);
                Debug.Log("tutorial opened");
            }
        }
    }
}
