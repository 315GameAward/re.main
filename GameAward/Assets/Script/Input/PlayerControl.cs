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

    // プレイヤー用変数
    private Rigidbody _rigidbody;           //Rigidbody
    private ControlBinds _gameInputs;        //インプット
    private Vector2 _moveStickValue;        //スティック移動量
    private Vector3 _moveDir;   // プレイヤーの動く向き
    private bool bSmoothCut  = false;    // スーと切る切ってるか
    private double dDelayTime = 0.0f;   // ディレイ用
    Animator anime; // アニメーター変数

    // システム用変数(パッドとかキーボードとかの)
    public float motorDelay = 0.1f;               //パッド振動のディレイ
    public bool m_bPlayerMove = false;       //移動しているか
    private bool bLeftClick = false;    // 左クリックを押してるかどうか

    public AudioClip se1;       // SEを入れる変数
    public AudioClip se2;       // SEを入れる変数
    AudioSource audioSource;    // AudioSourceの取得用 
    public GameObject Scisser;
    private void Awake()
    {
        //Rigidbody取得
        _rigidbody = GetComponent<Rigidbody>();

        // Compornentを取得
        audioSource = GetComponent<AudioSource>();

        //InputActionインスタンス生成
        _gameInputs = new ControlBinds();

        //アニーメーション機能の取得
        anime = GetComponent<Animator>();


        //Moveイベント登録
        _gameInputs.Player.Move.started += OnMove;
        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnMove;
       




        //Cutイベント登録
        //_gameInputs.Player.Cut.started += OnCut;
        //_gameInputs.Player.Cut.canceled += OnCutOff;

        //SmoothCutイベント登録
        //_gameInputs.Player.SmoothCut.started += OnSmoothCut;
        //_gameInputs.Player.SmoothCut.performed += OnSmoothCut;
        //_gameInputs.Player.SmoothCut.canceled += OnSmoothCutOff;

        // Cut2イベント登録
        _gameInputs.Player.Cut2.started += OnCut;
        _gameInputs.Player.Cut2.performed += MoveDir;
        _gameInputs.Player.Cut2.canceled += RessetMobeDir;

        // Clockwiseイベント登録(プレイヤーが紙に対して時計回りで移動する処理)
        _gameInputs.Player.Clockwise.started += ClockwiseMove;
        _gameInputs.Player.Clockwise.performed += ClockwiseMove;

        //InputAction有効化
        _gameInputs.Enable();
    }




    private void OnMove(InputAction.CallbackContext context)
    {
        // スーと切る時は処理を行わない
        if (bSmoothCut)
        {
            _moveStickValue = Vector2.zero; // 方向の初期化
            return;
        }

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

        // SEの再生
        if (gameObject.GetComponent<CutterPoint>().bPurposeObj)
        {
            audioSource.PlayOneShot(se2);
        }
        else
        {
            audioSource.PlayOneShot(se1);
        }

        Scisser.GetComponent<PlayerAnimation>().anime = true;
        //Debug.Log("CutOn");
        //anime.SetBool("Cut1", true);
        //WaitForSecondsRealtime(3.0f);
    }

    // スーと切る処理の始め(途中)
    private void MoveDir(InputAction.CallbackContext context)
    {
        bLeftClick = true;
        
       
       
    }

    // スーと切る処理の終わり
    private void RessetMobeDir(InputAction.CallbackContext context)
    {
        // 変数たちの初期化
        dDelayTime = 0;
        bSmoothCut = false;
        _moveDir = Vector3.zero;
        bLeftClick = false;
        Scisser.GetComponent<PlayerAnimation>().anime = false;
    }
    private void OnCutOff(InputAction.CallbackContext context)
    {
        m_bPlayerMove = false;
        //Debug.Log("CutOff");
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
        // 移動方向の力を与える
        _rigidbody.AddForce(_moveDir * _moveForce);
        transform.position += _moveDir * 0.1f;

        // ディレイ用
        if(bLeftClick)
        dDelayTime += 0.1f;
        if (dDelayTime >= 3.0)
        {
            bSmoothCut = true;  // スーと切るフラグon
            dDelayTime = 3.0f;  // カウントのストップ
        }

        // スーと切るフラグonの時
        if (bSmoothCut)
        {
            _moveDir = transform.forward;   // 方向の代入
        }

       
    }

    //ディレイ入れるコルーチン!
    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    // ある物体に対して時計回りに動く処理
    private void ClockwiseMove(InputAction.CallbackContext context)
    {
        //transform.position += transform.right * 1.0f * context.ReadValue<Vector2>().x;
    }
}
