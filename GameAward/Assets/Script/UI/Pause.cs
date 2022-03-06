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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;    //ポーズ画面オブジェクト
    private GameObject pauseUIInstance;             //ポーズ画面用インスタンス

    private ControlBinds _gameInputs;               //インプット

    private bool g_bPauseOpen;                      //ポーズを開いているか

    private void Awake()
    {
        //InputActionインスタンス生成
        _gameInputs = new ControlBinds();
        
        //Pauseイベント登録
        _gameInputs.Player.Pause.started += OnPause;

        //InputAction有効化
        _gameInputs.Enable();
    }
    
    private void OnPause(InputAction.CallbackContext context)
    {
        if (pauseUIInstance == null)    //ポーズUIがない場合
        {
            g_bPauseOpen = true;        //ポーズ開いてる判定
            Time.timeScale = 0.0f;      //ザ・ワールド
            pauseUIInstance = GameObject.Instantiate(pauseUI) as GameObject;    //ポーズUI設置
        }
        else
        {
            g_bPauseOpen = false;       //ポーズ開いてない判定
            Time.timeScale = 1.0f;      //時を動かす
            Destroy(pauseUIInstance);   //ポーズUI破壊
        }

    }
}
