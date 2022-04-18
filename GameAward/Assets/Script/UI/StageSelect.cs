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
        
        //if (currentScene.name == "StageSelect")     //ステージセレクトシーン内の場合
        if (currentScene.name == "MichidukaScene")       //デバッグ用
        {
            GameObject selectBtn = GameObject.Find("Selecting");
            if (_moveStickValue.x < 0.0f)  //下
            {
                if (stageSelect == 0)
                {
                    stageSelect = 1;
                    // 0 ↓
                    //selectBtn.transform.localPosition= new Vector3(-850.0f, -50.0f, transform.position.z);
                    selectBtn.transform.localPosition= new Vector3(-850.0f, -250.0f, transform.position.z);
                }
                else if (stageSelect == 1)
                {
                    stageSelect = 2;
                    selectBtn.transform.localPosition= new Vector3(-850.0f, -550.0f, transform.position.z);
                }
                else if (stageSelect == 2)
                {
                    stageSelect = 3;
                    selectBtn.transform.localPosition= new Vector3(-850.0f, -850.0f, transform.position.z);
                }
            }

            if (_moveStickValue.x > 0.0f)  //上
            {
                if (stageSelect == 1)
                {
                    stageSelect = 0;
                    Debug.Log("StageSelect: 0");
                    selectBtn.transform.localPosition= new Vector3(-850.0f, 50.0f, transform.position.z);
                }
                else if (stageSelect == 2)
                {
                    stageSelect = 1;
                    selectBtn.transform.localPosition= new Vector3(-850.0f, -250.0f, transform.position.z);
                }
                else if (stageSelect == 3)
                {
                    stageSelect = 2;
                    selectBtn.transform.localPosition= new Vector3(-850.0f, -550.0f, transform.position.z);
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
            //教室ステージ
            if (stageSelect == 0)
            {

            }
            //理科室ステージ
            else if (stageSelect == 1)
            {
                
            }
            //体育館ステージ
            else if (stageSelect == 2)
            {
                
            }
            //タイトルへ
            else if (stageSelect == 3)
            {
                
            }
        }
    }
}
