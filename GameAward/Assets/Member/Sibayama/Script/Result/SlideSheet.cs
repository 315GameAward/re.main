//================================================
//
//      SlideSheet.cs
//      リザルトスライド
//
//------------------------------------------------
//      作成者: 柴山凜太郎
//      作成開始日：2022/5/18(水)
//================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // UIいじる

public class SlideSheet : MonoBehaviour
{
    public RectTransform slide;
    static bool landing;
    public static bool Landing { get { return landing; } }
    public float moveDistance;     // 移動量
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        landing = false;
        slide.transform.position = new Vector3(Screen.width / 2, Screen.height + slide.rect.height, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // 画面の中心に来たら止める
        if (slide.transform.position.y <= Screen.height / 2)
        {
            pos = slide.transform.position;
            pos.y = Screen.height / 2;
            landing = true;
            return;
        }
        // 特定の位置まで下げる
        slide.transform.position -= new Vector3(0.0f, moveDistance, 0.0f);
    }

}
