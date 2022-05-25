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
    // 評価
    public enum EvaType
    {
        Too_Bad,        // がんばりましょう
        Good,           // よくできました
        Great,          // たいへんよくできました

        Max_Type
    }

    const int Max_Obj = 2;  // 評価項目数
    
    public float stopTime;  // スタンプ押す間隔
    
    // 評価テクスチャを適用させるオブジェクト
    public GameObject[] item = new GameObject[Max_Obj];
    
    GameObject layer;   // どデカいスタンプ押した後のリザルトぼかし

    // アクティブ状態を管理するオブジェクト
    GameObject[] AcObj = new GameObject[Max_Obj];
    
    // 評価に使うスタンプテクスチャ
    public Sprite[] evaluation = new Sprite[(int)EvaType.Max_Type];

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
        // 評価項目を採点
        StampEvaluation();
    }

    // Update is called once per frame
    void Update()
    {
        // 降りきっていないなら処理しない
        if (!SlideSheet.Landing)    // この処理をアニメーションを動かすスクリプトに持っていく。
            return;                 // 代わりにアニメーションが終了したかのフラグをここで書く。
        StartCoroutine("Defeat");
    }


    // 仮の評価処理
    void StampEvaluation()
    {
        item[0].GetComponent<Image>().sprite = evaluation[2];
        item[0].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        switch (1)
        {
            case 0:
                item[1].GetComponent<Image>().sprite = evaluation[0];
                item[1].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                break;
            case 1:
                item[1].GetComponent<Image>().sprite = evaluation[1];
                item[1].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                break;
            case 2:
                item[1].GetComponent<Image>().sprite = evaluation[2];
                item[1].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                break;
            default:
                break;
        }
    }

    // 一定間隔ずつリザルト項目表示
    IEnumerator Defeat()
    {
        for (int i = 0; i < Max_Obj; ++i)
        {
            // スタンプを押す処理
            yield return new WaitForSeconds(stopTime);
            item[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
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
