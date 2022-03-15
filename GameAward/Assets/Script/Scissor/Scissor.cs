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
    public float m_fSpeed_trigger = 0.2f; // 進む速度
    public float m_fSpeed_hold    = 0.2f; // 進む速度
    public float m_fDelayTime   　= 1.0f; // スーと切るときのディレイ時間
    public bool m_bMove = false;   // 動いてるかどうか(動いてたらtrue)

    public KeyCode key  = (KeyCode)323;   // 切るボタン(マウス左クリック)
    public KeyCode key1 = 0;
   GameObject poitObj;

    //--- Unityに公開しないパラメータ
    private bool m_bDelay = false;  // trueの時スーと切る
    private bool m_bKeyPress = false;   // キーを押してるか押してないか
    private float m_fDelay = 0.0f;  // スーと切るときのディレイ時間の器
    private Point m_point;  // カットポイント

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
            transform.position += transform.forward * m_fSpeed_trigger;
            m_bMove = true;
        }

        // 切って進む処理(スー)
        if (Input.GetKey(KeyCode.Mouse0))
        {
            m_bKeyPress = true; // キーを押してるかどうか

            // ディレイ開始
            m_fDelay -= 1.0f / 60.0f;
            if(m_fDelay < 0.0f)
            {
                m_fDelay = 0.0f;    // ディレイ時間ストップ
                m_bDelay = true;    // ディレイフラグON
            }

            // ディレイフラグONの時
            if (m_bDelay)
            {
                // 前に進む処理
                transform.position += transform.forward * m_fSpeed_hold;
            }
           
        }
        else
        {
            // 変数リセット
            m_fDelay = m_fDelayTime;    // ディレイ時間
            m_bDelay = false;   // ディレイフラグ 
            m_bMove  = false;   // ムーブフラグ
            m_bKeyPress = false;    // キーフラグ
        }

        // 向き変更
        if(Input.GetKey(KeyCode.A) && !m_bDelay) // 左
        {
            transform.Rotate(new Vector3(0.0f,-0.3f,0.0f));
        }
        if (Input.GetKey(KeyCode.D) && !m_bDelay) // 右
        {
            transform.Rotate(new Vector3(0.0f, 0.3f, 0.0f));
        }

        
    }

    // 地面に当っている時
    void OnTriggerStay(Collider other)
    {
        if (m_bMove)
        {
            //GetComponent<point>().AddPoitn(gameObject.transform.position);
           // Instantiate(poitObj, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            m_point.AddPoint(gameObject.transform.position);
            m_bMove = false;
        }
    }

}
