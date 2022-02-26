//======================================================
//
//        Scissor.cs
//        ハサミの動きの処理
//
//------------------------------------------------------
//      作成者:根本龍之介
//======================================================

//======================================================
// 開発履歴
// 2022/02/16 プロトタイプ作成開始
// 編集者:根本龍之介
//======================================================

//------ インクルード部 ------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Scissor : MonoBehaviour
{
    //--- Unityに公開するパラメータ
    public float g_fSpeed_trigger = 0.2f; // 進む速度
    public float g_fSpeed_hold    = 0.2f; // 進む速度
    public float g_fDelayTime   　= 1.0f; // スーと切るときのディレイ時間
    public bool g_bMove = false;   // 動いてるかどうか(動いてたらtrue)

    public KeyCode key  = (KeyCode)323;   // 切るボタン(マウス左クリック)
    public KeyCode key1 = 0;
   GameObject poitObj;

    //--- Unityに公開しないパラメータ
    private bool g_bDelay = false;  // trueの時スーと切る
    private bool g_bKeyPress = false;   // キーを押してるか押してないか
    private float g_fDelay = 0.0f;  // スーと切るときのディレイ時間の器
    private Point g_point;  // カットポイント

    // Start is called before the first frame update
    void Start()
    {
        poitObj = (GameObject)Resources.Load("point");
    }

    // Update is called once per frame
    void Update()
    {
        // 切って進む処理(チョキチョキ)
        if (Input.GetKeyDown(KeyCode.Mouse0))   
        {
            // 前に進む処理
            transform.position += transform.forward * g_fSpeed_trigger;
            g_bMove = true;
        }

        // 切って進む処理(スー)
        if (Input.GetKey(KeyCode.Mouse0))
        {
            g_bKeyPress = true; // キーを押してるかどうか

            // ディレイ開始
            g_fDelay -= 1.0f / 60.0f;
            if(g_fDelay < 0.0f)
            {
                g_fDelay = 0.0f;    // ディレイ時間ストップ
                g_bDelay = true;    // ディレイフラグON
            }

            // ディレイフラグONの時
            if (g_bDelay)
            {
                // 前に進む処理
                transform.position += transform.forward * g_fSpeed_hold;
            }
           
        }
        else
        {
            // 変数リセット
            g_fDelay = g_fDelayTime;    // ディレイ時間
            g_bDelay = false;   // ディレイフラグ 
            g_bMove  = false;   // ムーブフラグ
            g_bKeyPress = false;    // キーフラグ
        }

        // 向き変更
        if(Input.GetKey(KeyCode.A) && !g_bDelay) // 左
        {
            transform.Rotate(new Vector3(0.0f,-0.3f,0.0f));
        }
        if (Input.GetKey(KeyCode.D) && !g_bDelay) // 右
        {
            transform.Rotate(new Vector3(0.0f, 0.3f, 0.0f));
        }

        
    }

    // 地面に当っている時
    void OnTriggerStay(Collider other)
    {
        if (g_bMove)
        {
            //GetComponent<point>().AddPoitn(gameObject.transform.position);
           Instantiate(poitObj, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
           // g_point.AddPoint(gameObject.transform.position);
            g_bMove = false;
        }
    }

}
