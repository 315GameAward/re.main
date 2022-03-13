//================================================
//
//      PlayerControl.cs
//      ハサミのコントローラーとキーボード操作
//
//------------------------------------------------
//      作成者: 道塚悠基
//================================================

//================================================
// 開発履歴
// 2022/03/01 作成開始
// 編集者: 道塚悠基
//================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{

    [SerializeField] private float _moveForce = 5;  //強制移動量

    private Rigidbody _rigidbody;           //Rigidbody
    private ControlBinds _gameInputs;        //インプット
    private Vector2 _moveStickValue;        //スティック移動量

    public float motorDelay = 0.1f;               //パッド振動のディレイ
    public bool m_bPlayerMove = false;       //移動しているか

    private void Awake()
    {
        //Rigidbody取得
        _rigidbody = GetComponent<Rigidbody>();



        //InputActionインスタンス生成
        _gameInputs = new ControlBinds();

        //Moveイベント登録
        _gameInputs.Player.Move.started += OnMove;
        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnMove;

        //Cutイベント登録
        _gameInputs.Player.Cut.started += OnCut;
        _gameInputs.Player.Cut.canceled += OnCutOff;

        //SmoothCutイベント登録
        _gameInputs.Player.SmoothCut.started += OnSmoothCut;
        _gameInputs.Player.SmoothCut.performed += OnSmoothCut;
        _gameInputs.Player.SmoothCut.canceled += OnSmoothCutOff;

        //InputAction有効化
        _gameInputs.Enable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        //Moveアクションの入力取得
        _moveStickValue = context.ReadValue<Vector2>();

        //Debug.Log(_moveStickValue.y);
    }

    private void OnCut(InputAction.CallbackContext context)
    {
        //切った時の移動
        transform.position += transform.forward * .2f;

        //パッドの振動設定
        Gamepad.current.SetMotorSpeeds(1.0f, 0.5f);

        //ディレイのコルーチン実行
        StartCoroutine(DelayMethod(motorDelay, () =>
        {
            Gamepad.current.SetMotorSpeeds(0.0f, 0.0f);
        }));
        m_bPlayerMove = true;

        //WaitForSecondsRealtime(3.0f);
    }

    private void OnCutOff(InputAction.CallbackContext context)
    {
        m_bPlayerMove = false;
        Debug.Log("CutOff");
    }

    private void OnSmoothCut(InputAction.CallbackContext context)
    {
        transform.position += transform.forward * 1.0f;
        m_bPlayerMove = true;
    }
    private void OnSmoothCutOff(InputAction.CallbackContext context)
    {
        m_bPlayerMove = false;
    }


    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0.0f, _moveStickValue.x * _moveForce, 0.0f));
        /*
        // 移動方向の力を与える
        _rigidbody.AddForce(new Vector3(
            _moveInputValue.x,
            0,
            _moveInputValue.y
        ) * _moveForce);
        */
    }

    //ディレイ入れるコルーチン!
    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
    
}
