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
// 2022/05/19 作成開始2
// 編集者: 道塚悠基
//
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
    private int areaSelect;                         //何を選択しているか

    [SerializeField] private GameObject areas;

    public float rotateSpeed = 1.0f;                //回転スピード

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
        areaSelect = 0;
    }

    private void Update()
    {
        Debug.Log(areaSelect);
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
            if (_moveStickValue.x > 0.0f)  //right arrow
            {
                if (areaSelect == 0)
                {
                    StartCoroutine("ChangeAreaR");
                }
                else if (areaSelect == 1)
                {
                    StartCoroutine("ChangeAreaR");
                }
                else if (areaSelect == 2)
                {
                    StartCoroutine("ChangeAreaR");
                }
                else if (areaSelect == 3)
                {
                    StartCoroutine("ChangeAreaR");
                }
            }

            if (_moveStickValue.x < -0.0f)  //下
            {
                if (areaSelect == 1)
                {
                    areaSelect = 0;
                    Debug.Log("areaSelect: 0");
                    //areas.transform.rotation = Quaternion.Euler(0, -90, 0);
                }
                else if (areaSelect == 2)
                {
                    areaSelect = 1;
                    Debug.Log("areaSelect: 1");
                    //areas.transform.rotation = Quaternion.Euler(0, -180, 0);
                }
                else if (areaSelect == 3)
                {
                    areaSelect = 2;
                    Debug.Log("StageSelect: 2");
                    //areas.transform.rotation = Quaternion.Euler(0, -270, 0);
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
            if (areaSelect == 0)
            {

            }
            //理科室ステージ
            else if (areaSelect == 1)
            {

            }
            //体育館ステージ
            else if (areaSelect == 2)
            {

            }
            //タイトルへ
            else if (areaSelect == 3)
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

    IEnumerator ChangeAreaR()
    {
        if (areaSelect == 0)
        {
            while (areas.transform.eulerAngles.y < 90.0f)
            {
                //areas.transform.Rotate(Vector3.up * (90.0f * Time.deltaTime));
                areas.transform.Rotate(Vector3.up * rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            areaSelect = 1;
        }
        else if (areaSelect == 1)
        {
            while (areas.transform.eulerAngles.y < 180.0f)
            {
                //areas.transform.Rotate(Vector3.up * (90.0f * Time.deltaTime));
                areas.transform.Rotate(Vector3.up * rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            areaSelect = 2;
        }
        else if (areaSelect == 2)
        {
            while (areas.transform.eulerAngles.y < 270.0f)
            {
                //areas.transform.Rotate(Vector3.up * (90.0f * Time.deltaTime));
                areas.transform.Rotate(Vector3.up * rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            areaSelect = 3;
        }
        else if (areaSelect == 3)
        {
            while (areas.transform.eulerAngles.y < 359.9f)
            {
                //areas.transform.Rotate(Vector3.up * (90.0f * Time.deltaTime));
                areas.transform.Rotate(Vector3.up * rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            areaSelect = 0;
        }
    }

    IEnumerator ChangeAreaL()
    {
        while (areas.transform.eulerAngles.y < -90.0f)
        {
            //areas.transform.Rotate(Vector3.up * (90.0f * Time.deltaTime));
            areas.transform.Rotate(Vector3.up * -rotateSpeed);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
