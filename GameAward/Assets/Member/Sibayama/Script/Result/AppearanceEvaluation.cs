//================================================
//
//      AppearanceEvaluation.cs
//      結果発表
//
//------------------------------------------------
//      作成者: 柴山凜太郎
//      作成開始日：2022/5/20(金)
//================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // UIを弄るのに必要


public class AppearanceEvaluation : MonoBehaviour
{
    const int Max_Obj = 2;  // 評価項目数
    
    public float stopTime;  // スタンプ押す間隔
    
    
    GameObject layer;   // どデカいスタンプ押した後のリザルトぼかし

    // アクティブ状態を管理するオブジェクト
    GameObject[] AcObj = new GameObject[Max_Obj];

    GameObject slideObject;     // SlideSheet.csがアタッチされたオブジェクト取得用
    ParperSlide slide;           // SlideSheet.csクラス取得
    // 評価に使うスタンプテクスチャ

    private void Awake()
    {
        //--- 子オブジェクト取得＆非表示

        layer = GameObject.Find("layer");
        layer.gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        AcObj[0] = GameObject.Find("layer/last_result");
        AcObj[0].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        AcObj[1] = GameObject.Find("layer/SelectButton");

        for (var i = 0; i < Max_Obj; ++i)
        {
            // オブジェクトの非アクティブ化
            AcObj[i].SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        slideObject = GameObject.Find("ParperClear");
        slide = slideObject.GetComponent<ParperSlide>();
    }

    // Update is called once per frame
    void Update()
    {
        // 降りきっていないなら処理しない
        if (!slide.EndAnim)    // この処理をアニメーションを動かすスクリプトに持っていく。
            return;                 // 代わりにアニメーションが終了したかのフラグをここで書く。
        
        StartCoroutine("Defeat");
    }

    // 一定間隔ずつリザルト項目表示
    IEnumerator Defeat()
    {
        yield return new WaitForSeconds(stopTime);

        // ドデカスタンプ
        AcObj[0].SetActive(true);
        AcObj[0].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(stopTime);

        // セレクトボタン
        AcObj[1].SetActive(true);

        // ぼかし用レイヤー
        layer.gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);   
    }
}
