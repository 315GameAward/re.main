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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // このオブジェクトを削除
        if (gameObject.transform.position.y < -50) Destroy(gameObject);
    }
}
