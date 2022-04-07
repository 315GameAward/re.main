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

public class StageSelect : MonoBehaviour
{
    private ControlBinds _gameInputs;               //インプット
    private Vector2 _moveStickValue;                //スティック移動量
    private int stageSelect;                        //何を選択しているか



    private void Awake()
    {
        //InputActionインスタンス生成
        _gameInputs = new ControlBinds();

        //項目移動イベント登録
        _gameInputs.Player.MoveSelect.performed += OnMoveSelect;
        
        //Selectイベント登録
        _gameInputs.Player.Select.started += OnSelect;
        
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
        
        if (currentScene.name == "StageSelect")     //ステージセレクトシーン内の場合
        {
            //GameObject selectBtn = GameObject.Find("^_^");
            if (_moveStickValue.x < 0.0f)  //下
            {

            }

            if (_moveStickValue.x > 0.0f)  //上
            {

            }
        }
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        //現在のシーン名取得
        Scene currentScene = SceneManager.GetActiveScene();
        
        if (currentScene.name == "StageSelect")     //ステージセレクトシーン内の場合
        {
            if (stageSelect == 0)
            {

            }
            else if (stageSelect == 1)
            {
                
            }
            else if (stageSelect == 2)
            {
                
            }
        }
    }
}
