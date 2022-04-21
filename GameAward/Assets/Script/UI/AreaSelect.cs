//================================================
//
//      AreaSelect.cs
//      エリアの選択
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

public class AreaSelect : MonoBehaviour
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

        //Pauseイベント登録(Back)
        _gameInputs.Player.Pause.started += OnPause;

        //InputAction有効化
        _gameInputs.Enable();
    }

    private void Start()
    {
        stageSelect = 0;
    }

    private void Update()
    {
        GameObject areas = GameObject.Find("Areas");
        //areas.transform.Rotate(Vector3.up * (90.0f * Time.deltaTime));
    }

    private void OnMoveSelect(InputAction.CallbackContext context)
    {
        //Moveアクションの入力取得
        _moveStickValue = context.ReadValue<Vector2>();
        //現在のシーン名取得
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "AreaSelect")     //ステージセレクトシーン内の場合
        {
            GameObject areas = GameObject.Find("Areas");
            if (_moveStickValue.x > 0.0f)  //左
            {
                if (stageSelect == 0)
                {
                    stageSelect = 1;
                    Debug.Log("1");
                    while (areas.transform.rotation.y < 90.0f)
                    {
                        areas.transform.Rotate(Vector3.up * (90.0f * Time.deltaTime));
                    }
                }
                else if (stageSelect == 1)
                {
                    stageSelect = 2;
                    Debug.Log("2");
                    areas.transform.Rotate(Vector3.up * (90.0f * Time.deltaTime));
                }
                else if (stageSelect == 2)
                {
                    stageSelect = 3;
                    Debug.Log("3");
                    areas.transform.Rotate(Vector3.up * (90.0f * Time.deltaTime));
                }
            }

            if (_moveStickValue.x < -0.0f)  //下
            {
                if (stageSelect == 1)
                {
                    stageSelect = 0;
                    Debug.Log("StageSelect: 0");
                    areas.transform.rotation = Quaternion.Euler(0, -90, 0);
                }
                else if (stageSelect == 2)
                {
                    stageSelect = 1;
                    Debug.Log("StageSelect: 1");
                    areas.transform.rotation = Quaternion.Euler(0, -180, 0);
                }
                else if (stageSelect == 3)
                {
                    stageSelect = 2;
                    Debug.Log("StageSelect: 2");
                    areas.transform.rotation = Quaternion.Euler(0, -270, 0);
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

    private void OnPause(InputAction.CallbackContext context)
    {
        //現在のシーン名取得
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "StageSelect")     //ステージセレクトシーン内の場合
        {
            //
        }
    }


}
