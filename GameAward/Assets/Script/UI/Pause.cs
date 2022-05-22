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
    private ControlBinds _gameInputs;               //インプット
    private Vector2 _moveStickValue;                //スティック移動量
    
    private int pauseSelect;                        //ポーズで何を選択しているか

    private bool g_bPauseOpen;                      //ポーズを開いているか

    

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
                    selectBtn.transform.localPosition= new Vector3(-1550.0f, -100.0f, transform.position.z);
                }
                else if (pauseSelect == 1)
                {
                    pauseSelect = 2;
                    selectBtn.transform.localPosition= new Vector3(-1550.0f, -375.0f, transform.position.z);
                }
            }

            if (_moveStickValue.x <= -0.1f)  //left arrow
            {
                if (pauseSelect == 1)
                {
                    pauseSelect = 0;
                    selectBtn.transform.localPosition= new Vector3(-1550.0f, 155.0f, transform.position.z);
                }
                else if (pauseSelect == 2)
                {
                    pauseSelect = 1;
                    selectBtn.transform.localPosition= new Vector3(-1550.0f, -100.0f, transform.position.z);
                }
            }
        }
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        if (g_bPauseOpen)
        {
            if (pauseSelect == 0)
            {
                g_bPauseOpen = false;       //ポーズ開いてない判定
                Time.timeScale = 1.0f;      //時を動かす
                Destroy(pauseUIInstance);   //ポーズUI破壊
            }
            else if (pauseSelect == 1)
            {
                SceneManager.LoadScene("AreaSelect");
            }
        }
    }

    void Update()
    {
        if (pauseSelect == 1)
        {
            
        }
    }
}
