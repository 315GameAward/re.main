//======================================================
//
//        Ground.cs
//        地面の処理
//
//------------------------------------------------------
//      作成者:根本龍之介
//======================================================

//======================================================
// 開発履歴
// 2022/02/16 プロトタイプ作成開始
// 編集者:根本龍之介
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    bool fade = false;
    float alpha = 1.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // このオブジェクトを削除
        if (fade)
        {
            alpha -= 0.003f;
            gameObject.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, alpha);
            if (alpha < 0.0f) Destroy(gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Plane (2)")
        {
            fade = true;
        }
    }
}
