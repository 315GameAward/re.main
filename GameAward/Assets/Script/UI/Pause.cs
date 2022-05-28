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
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;    //ポーズ画面オブジェクト
    private GameObject pauseUIInstance;             //ポーズ画面用インスタンス

    private Animator pauseBgAnime;                  //ポーズ背景アニメーション
    private Animator backtogameAnime;               //操作説明用アニメーション
    private Animator retryAnime;                    //リトライ用アニメーション
    private Animator gameselectAnime;               //ゲームセレクト用アニメーション
    private Animator tutorialAnime;                 //ゲームセレクト用アニメーション
    private Animator tutorialImgAnime;              //操作説明画像用アニメーション

    private ControlBinds _gameInputs;               //インプット
    private Vector2 _moveStickValue;                //スティック移動量

    private int pauseSelect;                        //ポーズで何を選択しているか

    private bool tutImgOpen;                        //操作説明画像を開いているか
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
        tutImgOpen = false;

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
            pauseSelect = 0;            //選択初期化

            //アニメーション再生
            backtogameAnime = GameObject.Find("BackToGame").GetComponent<Animator>();
            backtogameAnime.SetBool("anime", true);

            retryAnime = GameObject.Find("Retry").GetComponent<Animator>();
            gameselectAnime = GameObject.Find("GameSelect").GetComponent<Animator>();
            tutorialAnime = GameObject.Find("Tutorial").GetComponent<Animator>();
            tutorialImgAnime = GameObject.Find("TutorialImage").GetComponent<Animator>();

            pauseBgAnime = GameObject.Find("PauseBackground").GetComponent<Animator>();
            pauseBgAnime.Play("Base Layer.PauseBg", 0, 0.25f);
        }
        else
        {

            Animator pausecanvasAnime = GameObject.Find("PauseCanvas").GetComponent<Animator>(); ;
            pausecanvasAnime.Play("Base Layer.ClosePause", 0, 0.25f);

            Time.timeScale = 1.0f;              //時を動かす

            Invoke("ClosePauseDelay", 0.25f);   //.25f遅延でポーズ破壊

        }
    }

    private void OnMoveSelect(InputAction.CallbackContext context)
    {
        //Moveアクションの入力取得
        _moveStickValue = context.ReadValue<Vector2>();

        if (g_bPauseOpen && !tutImgOpen) //ポーズを開いている場合
        {
            GameObject selectBtn = GameObject.Find("SelectBtn");
            if (_moveStickValue.y <= 0.1f && _moveStickValue.y != 0.0f)  //up arrow
            {
                if (pauseSelect == 0)
                {
                    pauseSelect = 1;
                    selectBtn.transform.localPosition = new Vector3(x1, y1, transform.position.z);
                    backtogameAnime.SetBool("anime", false);
                    retryAnime.SetBool("anime", true);
                }
                else if (pauseSelect == 1)
                {
                    pauseSelect = 2;
                    selectBtn.transform.localPosition = new Vector3(x2, y2, transform.position.z);
                    retryAnime.SetBool("anime", false);
                    gameselectAnime.SetBool("anime", true);
                }
                else if (pauseSelect == 2)
                {
                    pauseSelect = 3;
                    selectBtn.transform.localPosition = new Vector3(x3, y3, transform.position.z);
                    gameselectAnime.SetBool("anime", false);
                    tutorialAnime.SetBool("anime", true);
                }
            }

            if (_moveStickValue.y >= 0.1f && _moveStickValue.y != 0.0f)  //down arrow
            {
                if (pauseSelect == 3)
                {
                    pauseSelect = 2;
                    selectBtn.transform.localPosition = new Vector3(x2, y2, transform.position.z);
                    tutorialAnime.SetBool("anime", false);
                    gameselectAnime.SetBool("anime", true);
                }
                else if (pauseSelect == 2)
                {
                    pauseSelect = 1;
                    selectBtn.transform.localPosition = new Vector3(x1, y1, transform.position.z);
                    gameselectAnime.SetBool("anime", false);
                    retryAnime.SetBool("anime", true);
                }
                else if (pauseSelect == 1)
                {
                    pauseSelect = 0;
                    selectBtn.transform.localPosition = new Vector3(x0, y0, transform.position.z);
                    retryAnime.SetBool("anime", false);
                    backtogameAnime.SetBool("anime", true);
                }
            }
        }
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        if (tutImgOpen)
        {
            tutorialImgAnime.Play("Base Layer.CloseTutImg", 0, 0);
            tutImgOpen = false;
        }
        else if (g_bPauseOpen)
        {
            if (pauseSelect == 0)           //ゲームに戻る
            {
                Animator pausecanvasAnime = GameObject.Find("PauseCanvas").GetComponent<Animator>(); ;
                pausecanvasAnime.Play("Base Layer.ClosePause", 0, 0.25f);

                Time.timeScale = 1.0f;              //時を動かす

                Invoke("ClosePauseDelay", 0.25f);   //.25f遅延でポーズ破壊
            }
            else if (pauseSelect == 1)      //やり直す
            {
                Time.timeScale = 1.0f;      //時を動かす
                g_bPauseOpen = false;       //ポーズ開いてない判定
                Destroy(pauseUIInstance);   //ポーズUI破壊
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (pauseSelect == 2)      //ステージセレクトに戻る
            {
                Time.timeScale = 1.0f;      //時を動かす
                g_bPauseOpen = false;       //ポーズ開いてない判定
                Destroy(pauseUIInstance);   //ポーズUI破壊
                SceneManager.LoadScene("AreaSelect");
            }
            else if (pauseSelect == 3)      //操作説明
            {
                tutImgOpen = true;
                tutorialImgAnime.Play("Base Layer.OpenTutImg", 0, 0);
            }
        }
    }

    private void ClosePauseDelay()
    {
        g_bPauseOpen = false;       //ポーズ開いてない判定
        Destroy(pauseUIInstance);   //ポーズUI破壊
    }
}