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
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AreaSelect : MonoBehaviour
{
    private ControlBinds _gameInputs;               //インプット
    private Vector2 _moveStickValue;                //スティック移動量
    private int areaSelect;                         //何を選択しているか
    private bool areaRotating;                      //回転中
    
    [SerializeField] private GameObject areas;
    public float rotateSpeed = 1.5f;                //回転スピード
    
    //Animator
    public Animator enpitsuAnime;
    public Animator pianoAnime;
    public Animator hanmaAnime;
    public Animator arukoruAnime;
    //Animator Area Text
    public Animator areaTextAnime;
    
    [SerializeField] private Text areaText;         //選択中エリア表示用テキスト
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private AudioClip rotate;
    [SerializeField] private AudioClip select;

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
        areaRotating = false;
        areaText.text = "教室ステージ";
    }

    private void Update()
    {
        Debug.Log(areaSelect);
        //Debug.Log(areas.transform.localEulerAngles.y);
        //areas.transform.Rotate(Vector3.up * (90.0f * Time.deltaTime));

        //if (areaSelect == 0)
        //{
            
        //}
        //else if (areaSelect == 1)
        //{
            
        //}
        //else if (areaSelect == 2)
        //{
            
        //}
        //else if (areaSelect == 3)
        //{
            
        //}
    }

    private void OnMoveSelect(InputAction.CallbackContext context)
    {
        //Moveアクションの入力取得
        _moveStickValue = context.ReadValue<Vector2>();
        //現在のシーン名取得
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "AreaSelect")     //ステージセレクトシーン内の場合
        {
            if (_moveStickValue.x >= 0.1f)  //right arrow
            {
                if (!areaRotating)
                {
                    if (areaSelect == 0)
                    {
                        areaRotating = true;
                        enpitsuAnime.SetBool("Bounce", false);
                        StartCoroutine("ChangeAreaR");
                    }
                    else if (areaSelect == 1)
                    {
                        areaRotating = true;
                        pianoAnime.SetBool("Bounce", false);
                        StartCoroutine("ChangeAreaR");
                    }
                    else if (areaSelect == 2)
                    {
                        areaRotating = true;
                        hanmaAnime.SetBool("Bounce", false);
                        StartCoroutine("ChangeAreaR");
                    }
                    else if (areaSelect == 3)
                    {
                        areaRotating = true;
                        arukoruAnime.SetBool("Bounce", false);
                        StartCoroutine("ChangeAreaR");
                    }
                }
            }

            if (_moveStickValue.x <= -0.1f)  //left arrow
            {
                if (!areaRotating)
                {
                    if (areaSelect == 0)
                    {
                        areaRotating = true;
                        enpitsuAnime.SetBool("Bounce", false);
                        StartCoroutine("ChangeAreaL");
                    }
                    else if (areaSelect == 1)
                    {
                        areaRotating = true;
                        pianoAnime.SetBool("Bounce", false);
                        StartCoroutine("ChangeAreaL");
                    }
                    else if (areaSelect == 2)
                    {
                        areaRotating = true;
                        hanmaAnime.SetBool("Bounce", false);
                        StartCoroutine("ChangeAreaL");
                    }
                    else if (areaSelect == 3)
                    {
                        areaRotating = true;
                        arukoruAnime.SetBool("Bounce", false);
                        StartCoroutine("ChangeAreaL");
                    }
                }
            }
        }
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        //現在のシーン名取得
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "AreaSelect")     //ステージセレクトシーン内の場合
        {
            audioSource.PlayOneShot(select);
            //教室ステージ
            if (areaSelect == 0)
            {
                Debug.Log("教室ステージ選択");
                SceneManager.LoadScene("OomoriScene");
            }
            //音楽室ステージ
            else if (areaSelect == 1)
            {
                Debug.Log("音楽室ステージ選択");
            }
            //体育館ステージ
            else if (areaSelect == 2)
            {
                Debug.Log("図工室ステージ選択");
            }
            //理科室へ
            else if (areaSelect == 3)
            {
                Debug.Log("理科室ステージ選択");
            }
        }
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        //現在のシーン名取得
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "AreaSelect")
        {
            SceneManager.LoadScene("TitleScene");
        }
    }

    IEnumerator ChangeAreaR()
    {
        areaRotating = true;
        audioSource.PlayOneShot(rotate);
        if (areaSelect == 0)
        {
            areas.transform.eulerAngles = new Vector3(0, 0.0f, 0);
            areaTextAnime.SetBool("ChangeTextR", true);
            pianoAnime.SetBool("Bounce", true);     //play animation
            while (areas.transform.eulerAngles.y < 90.0f)
            {
                //areas.transform.Rotate(Vector3.up * (90.0f * Time.deltaTime));
                areas.transform.Rotate(Vector3.up * rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }
            areaText.text = "音楽室ステージ";
            areaTextAnime.SetBool("ChangeTextR", false);
            areaSelect = 1;
        }
        else if (areaSelect == 1)
        {
            areas.transform.eulerAngles = new Vector3(0, 90.0f, 0);
            areaTextAnime.SetBool("ChangeTextR", true);
            hanmaAnime.SetBool("Bounce", true);
            while (areas.transform.eulerAngles.y < 180.0f)
            {
                areas.transform.Rotate(Vector3.up * rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }
            areaText.text = "図工室ステージ";
            areaTextAnime.SetBool("ChangeTextR", false);
            areaSelect = 2;
        }
        else if (areaSelect == 2)
        {
            areas.transform.eulerAngles = new Vector3(0, 180.0f, 0);
            areaTextAnime.SetBool("ChangeTextR", true);
            arukoruAnime.SetBool("Bounce", true);
            while (areas.transform.eulerAngles.y < 270.0f)
            {
                areas.transform.Rotate(Vector3.up * rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }
            areaText.text = "理科室ステージ";
            areaTextAnime.SetBool("ChangeTextR", false);
            areaSelect = 3;
        }
        else if (areaSelect == 3)
        {
            areas.transform.eulerAngles = new Vector3(0, 270.0f, 0);
            areaTextAnime.SetBool("ChangeTextR", true);
            enpitsuAnime.SetBool("Bounce", true);
            while (areas.transform.eulerAngles.y > 0.0f)
            {
                areas.transform.Rotate(Vector3.up * rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }
            areaText.text = "教室ステージ";
            areaTextAnime.SetBool("ChangeTextR", false);
            areaSelect = 0;
        }

        areaRotating = false;
    }

    IEnumerator ChangeAreaL()
    {
        areaRotating = true;
        audioSource.PlayOneShot(rotate);
        if (areaSelect == 0)
        {
            areas.transform.eulerAngles = new Vector3(0, -0.1f, 0);
            areaTextAnime.SetBool("ChangeTextL", true);
            arukoruAnime.SetBool("Bounce", true);
            while (areas.transform.eulerAngles.y > 270.0f)
            {
                areas.transform.Rotate(Vector3.up * -rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }
            areaText.text = "理科室ステージ";
            areaTextAnime.SetBool("ChangeTextL", false);
            areaSelect = 3;
        }
        else if (areaSelect == 3)
        {
            areas.transform.eulerAngles = new Vector3(0, 270.0f, 0);
            areaTextAnime.SetBool("ChangeTextL", true);
            hanmaAnime.SetBool("Bounce", true);
            while (areas.transform.eulerAngles.y > 180.0f)
            {
                areas.transform.Rotate(Vector3.up * -rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }
            areaText.text = "図工室ステージ";
            areaTextAnime.SetBool("ChangeTextL", false);
            areaSelect = 2;
        }
        else if (areaSelect == 2)
        {
            areas.transform.eulerAngles = new Vector3(0, 180.0f, 0);
            areaTextAnime.SetBool("ChangeTextL", true);
            pianoAnime.SetBool("Bounce", true);
            while (areas.transform.eulerAngles.y > 90.0f)
            {
                areas.transform.Rotate(Vector3.up * -rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }
            areaText.text = "音楽室ステージ";
            areaTextAnime.SetBool("ChangeTextL", false);
            areaSelect = 1;
        }
        else if (areaSelect == 1)
        {
            areas.transform.eulerAngles = new Vector3(0, 90.0f, 0);
            areaTextAnime.SetBool("ChangeTextL", true);
            enpitsuAnime.SetBool("Bounce", true);
            while (areas.transform.eulerAngles.y < 359.0f)
            {
                areas.transform.Rotate(Vector3.up * -rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }
            areaText.text = "教室ステージ";
            areaTextAnime.SetBool("ChangeTextL", false);
            areaSelect = 0;
        }

        areaRotating = false;
    }
}
