//===============================================================
//
//      SelectButton.cs
//      ボタン選択
//
//---------------------------------------------------------------
//      作成者: 柴山凜太郎
//      作成開始日：2022/5/26(木)
//===============================================================

//===============================================================
//      <=開発履歴=>
//---------------------------------------------------------------
//      内容：Start関数にコメント追加
//            NextStage関数にコメントを追加
//      編集者：柴山凜太郎
//      編集日：2022/10/03(月)
//===============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectButton : MonoBehaviour
{
    // ボタンのインスタンス
    Button[] button = new Button[2];

    // Start is called before the first frame update
    void Start()
    {
        // ボタンの振り分け
        button[0] = GameObject.Find("ReturnSelect").GetComponent<Button>();
        button[1] = GameObject.Find("NextStage").GetComponent<Button>();

        // セレクトボタンにフォーカス
        button[0].Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnSelect()
    {
        SceneManager.LoadScene(GetScene.CurrentSceneArea);
    }

    // シーンの遷移先を判定
    public void NextStage()
    {
        // 現在のシーン(ステージ)を元に次の遷移先を決める
        switch (GetScene.CurrentSceneStage)
        {
            case 9:// 1-5
                // エリア２へ
                SceneManager.LoadScene(GetScene.CurrentSceneArea + 1);
                break;
            case 14:// 2-5
                // エリア３へ
                SceneManager.LoadScene(GetScene.CurrentSceneArea + 1);
                break;
            default:
                if ((GetScene.CurrentSceneStage + 1) > 19)
                {// 最終エリア、最終ステージをクリアしたら
                    SceneManager.LoadScene("TitleScene");
                }
                SceneManager.LoadScene((GetScene.CurrentSceneStage + 1));
                break;
        }
    }

}
