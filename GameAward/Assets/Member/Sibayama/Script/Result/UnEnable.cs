//===============================================================
//
//      UnEnable.cs
//      オブジェクトの非アクティブ化
//
//---------------------------------------------------------------
//      作成者: 柴山凜太郎
//      作成開始日：2022/5/26(木)
//===============================================================

//===============================================================
//      <=開発履歴=>
//---------------------------------------------------------------
//      内容：Start関数にコメント追加
//      編集者：柴山凜太郎
//      編集日：2022/10/03(月)
//===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnEnable : MonoBehaviour
{
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {// オブジェクトを非アクティブにする
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
