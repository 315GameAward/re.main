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
    
    public GameObject[] gameObject = new GameObject[3];
    // 評価に使うスタンプテクスチャ
    public Sprite[] evaluation = new Sprite[(int)EvaType.Max_Type];
    EvaType score;
    Life life = new Life();

    // Start is called before the first frame update
    void Start()
    {
        gameObject[0] = GameObject.Find("Item/Evaluation");
        gameObject[0].GetComponent<Image>().sprite = evaluation[0];
    }

    // Update is called once per frame
    void Update()
    {
        switch (life.nLife)
        {
            case 0 | 1:
                StampEvaluation(EvaType.Too_Bad);
                gameObject[0].GetComponent<Image>().sprite = evaluation[0];
                break;
            case 2:
                StampEvaluation(EvaType.Good);
                gameObject[0].GetComponent<Image>().sprite = evaluation[1];
                break;
            case 3:
                StampEvaluation(EvaType.Great);
                gameObject[0].GetComponent<Image>().sprite = evaluation[2];
                break;
            default:
                break;
        }
    }


    void StampEvaluation(EvaType _score)
    {
        switch (_score)
        {
            case EvaType.Too_Bad:

                break;
            case EvaType.Good:
                break;
            case EvaType.Great:
                break;
            default:
                break;
        }
    }
}
