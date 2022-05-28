using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// ==========================================
//
//              ゲームオーバー
//
// ==========================================
// 上月大地

public class Game_Over : MonoBehaviour
{
    private enum STATE_GMOV
    {
        OVER_NONE = 0,  // 選択無し
        OVER_RETRY,     // リトライ
        OVER_SELECT,    // セレクト画面
        OVER_END        // 終了
    };

    public GameObject Image_gameOver;
    public Sprite cursor;
    public Image img_rtry;
    public Image img_slct;
    public Image img_end;
    AudioSource audioSource;
    public AudioClip sound_Rot; // 移動_効果音
    public AudioClip sound_Chs; // 選択_効果音
    public AudioClip sound_Gin; // ジングル音

    private float fAlpha = 0.0f;

    private STATE_GMOV OVERstate;
    public bool b_gmov = false;    // trueなら呼び出す
    private bool b_gingle = false;

    // Start is called before the first frame update
    void Start()
    {
        // 音初期化
        audioSource = GetComponent<AudioSource>();

        // 初期は何も選択しない
        OVERstate = STATE_GMOV.OVER_RETRY;
        img_rtry = GameObject.Find("cursor_1").GetComponent<Image>();
        img_slct = GameObject.Find("cursor_2").GetComponent<Image>();
        img_end = GameObject.Find("cursor_3").GetComponent<Image>();
        img_slct.enabled = false;
        img_rtry.enabled = false;
        img_end.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // オーバーが呼び出されたら入る
        if (b_gmov == true)
        {
            // ジングル再生
            if(b_gingle == true) { audioSource.PlayOneShot(sound_Gin); b_gingle = false; }

            // 入力取得
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                //音を鳴らす
                audioSource.PlayOneShot(sound_Rot);
                OVERstate++;
                if (OVERstate > STATE_GMOV.OVER_END)
                {
                    OVERstate = STATE_GMOV.OVER_RETRY;
                }

            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                //音を鳴らす
                audioSource.PlayOneShot(sound_Rot);
                OVERstate--;
                if (OVERstate <= STATE_GMOV.OVER_NONE)
                {
                    OVERstate = STATE_GMOV.OVER_END;
                }
            }
            // エスケープを押されたら
            //if (Input.GetKeyUp(KeyCode.Escape))
            //{
            //    b_gmov = false;
            //}

            // UI呼び出し
            fAlpha += 0.01f;
            //if (fAlpha > 80.0f / 255.0f) fAlpha = 80.0f / 255.0f; // 透明度の制限
            if (fAlpha > 210.0f / 255.0f) fAlpha = 210.0f / 255.0f; // 透明度の制限
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, fAlpha);

            // 状態によって変化
            switch (OVERstate)
            {
                case STATE_GMOV.OVER_NONE:  // 何もない

                    img_rtry.enabled = true;
                    img_slct.enabled = false;
                    img_end.enabled = false;

                    break;
                case STATE_GMOV.OVER_RETRY: // リトライ選択中
                    img_rtry.enabled = true;
                    img_slct.enabled = false;
                    img_end.enabled = false;

                    // 現在のシーンを再生
                    if (Input.GetKeyUp(KeyCode.Return))
                    {
                        //音を鳴らす
                        audioSource.PlayOneShot(sound_Chs);

                        // シーン遷移
                        SceneManager.LoadScene("GameScene");    // 仮
                    }
                    break;
                case STATE_GMOV.OVER_SELECT: // リトライ選択中
                    img_slct.enabled = true;
                    img_rtry.enabled = false;
                    img_end.enabled = false;

                    // 現在のシーンを再生
                    if (Input.GetKeyUp(KeyCode.Return))
                    {
                        //音を鳴らす
                        audioSource.PlayOneShot(sound_Chs);

                        // シーン遷移
                        SceneManager.LoadScene("AreaSelect");    // 仮
                    }
                    break;
                //case STATE_GMOV.OVER_END:   // ゲームを抜けるを選択中
                //    img_slct.enabled = false;
                //    img_rtry.enabled = false;
                //    img_end.enabled = true;

                //    // ゲーム終了
                //    if (Input.GetKeyUp(KeyCode.Return))
                //    {
                //        //音を鳴らす
                //        audioSource.PlayOneShot(sound_Chs);

                //        // 終了
                //        Application.Quit();
                //    }
                //    break;
            }
        }
        else if (b_gmov == false)   // 解除されたら
        {
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            img_slct.enabled = false;
            img_rtry.enabled = false;
           // img_end.enabled  = false;
        }
        // Debug.Log(OVERstate);
    }

    // =====================
    // ゲームオーバーセット
    // =====================
    public void SetGMOV(bool gmov)
    {
        // bool取得
        b_gmov = gmov;
        b_gingle = true;
    }
}
