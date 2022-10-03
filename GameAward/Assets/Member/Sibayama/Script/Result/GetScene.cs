//===============================================================
//
//      GetScene.cs
//      シーンの取得
//
//---------------------------------------------------------------
//      作成者: 柴山凜太郎
//      作成開始日：2022/5/26(木)
//===============================================================

//===============================================================
//      <=開発履歴=>
//---------------------------------------------------------------
//      内容：Start関数にコメント追加
//            GetSceneクラスで宣言している変数にコメントを追加
//      編集者：柴山凜太郎
//      編集日：2022/10/03(月)
//===============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetScene : MonoBehaviour
{
    static int _currentSceneArea;   // 現在のシーン(エリア)のインデックス取得
    static int _currentSceneStage;  // 現在のシーン(ステージ)のインデックス取得
    public static int CurrentSceneArea { get { return _currentSceneArea; } }
    public static int CurrentSceneStage { get { return _currentSceneStage; } }
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject == GameObject.Find("UI"))
        {// セレクトシーン(UIオブジェクトがある)なら現在のエリアシーン取得
            _currentSceneArea = SceneManager.GetActiveScene().buildIndex;
        }
        if (gameObject == GameObject.Find("Scissor (1)"))
        {// ゲームシーン(ハサミがある)なら現在のステージシーン取得
            _currentSceneStage = SceneManager.GetActiveScene().buildIndex;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
