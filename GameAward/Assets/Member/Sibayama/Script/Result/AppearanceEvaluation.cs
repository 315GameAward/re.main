using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // 評価に使うスタンプテクスチャ
    public Sprite[] evaluation = new Sprite[(int)EvaType.Max_Type];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //switch ()
        //{
        //    default:
        //        break;
        //}
    }
}
