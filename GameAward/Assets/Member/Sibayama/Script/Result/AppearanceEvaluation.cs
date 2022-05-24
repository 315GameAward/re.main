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
        Too_Bad,
        Good,
        Great,

        Max_Type
    }

    // 評価項目数
    const int Max_Obj = 2;
    // スタンプ押す間隔
    public float stopTime;
    // 評価テクスチャを適用させるオブジェクト
    public GameObject[] item = new GameObject[2];
    // 評価に使うスタンプテクスチャ
    public Sprite[] evaluation = new Sprite[(int)EvaType.Max_Type];
    
    
    // Start is called before the first frame update
    void Start()
    {
        StampEvaluation();
    }

    // Update is called once per frame
    void Update()
    {
        if (!SlideSheet.Landing)
            return;
        StartCoroutine("Defeat");

    }

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

    IEnumerator Defeat()
    {
        for (int i = 0; i < Max_Obj; ++i)
        {
            yield return new WaitForSeconds(stopTime);
            item[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}


