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
using UnityEngine.UI;



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

    public GameObject SelectButton;
    public const int Max_Obj = 2;  // 評価項目数
    
    public float stopTime;  // スタンプ押す間隔

    static bool[] activeObj = new bool[2];  // オブジェクトをアクティブに設定
    
    // 評価テクスチャを適用させるオブジェクト
    public GameObject[] item = new GameObject[Max_Obj];

    // どデカスタンプ
    GameObject last_result;
    
    // 評価に使うスタンプテクスチャ
    public Sprite[] evaluation = new Sprite[(int)EvaType.Max_Type];

    private void Awake()
    {
        SelectButton.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        last_result = GameObject.Find("last_result");
        last_result.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        GameObject.Find("layer").gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        // 評価項目を採点
        StampEvaluation();
    }

    // Update is called once per frame
    void Update()
    {
        // 降りきっていないなら処理しない
        if (!SlideSheet.Landing)
            return;
        
        StartCoroutine("Defeat");
    }

    // 仮の評価処理
    void StampEvaluation()
    {
        for (int i = 0; i < Max_Obj; ++i)
        {
            switch (i)
            {
                case 0:
                    item[i].GetComponent<Image>().sprite = evaluation[0];
                    item[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    break;
                case 1:
                    item[i].GetComponent<Image>().sprite = evaluation[1];
                    item[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                    break;
                case 2:
                    item[i].GetComponent<Image>().sprite = evaluation[2];
                    item[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                    break;
                default:
                    break;
            }
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
        last_result.GetComponent<Image>().color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        //activeObj[0] = true;
        yield return new WaitForSeconds(stopTime);
        GameObject.Find("layer").gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        SelectButton.SetActive(true);
        //activeObj[1] = true;
        //SetActive.AcObj.SetActive(true);
    }

}
