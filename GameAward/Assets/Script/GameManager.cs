//================================================
//
//      GameManager.cs
//      ゲームのマネージャー
//
//------------------------------------------------
//      作成者: 道塚悠基
//================================================

//================================================
// 開発履歴
// 2022/03/03 作成開始
// 編集者: 道塚悠基
//================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ゲームステート
public enum GameState
{
    Title,          //タイトル
    StageSelect,    //ステージセレクト
    GameStart,      //ゲームスタート
    GameClear,
    GameOver        //ゲームオーバー
    //result??
}
public class GameManager : MonoBehaviour
{
    public static GameState status;

    //いじりたい値をシリアライズして
    //インスペクターで触れるようにする

    //スコア表示用テキスト
    [SerializeField]
    Text scoreText;

    //クリア用UI
    [SerializeField]
    GameObject clearUI;

    //ゲームオーバー用UI
    [SerializeField]
    GameObject gameoverUI;

    //現在のスコア
    public int g_nScore;
    //最大スコア
    public int g_nMaxScore;
    //現在のタイム
    public float g_fTime;
    
    
    //ゲームステート
    private GameState gameState;
    
    //インスタンス化後実行用関数
    void Awake()
    {
        SetGameState(GameState.Title);
        InitGame();
    }

    //初期化関数
    void InitGame()
    {
        g_nScore = 0;
        g_nMaxScore = 999;
        g_fTime = 100.0f;
    }

    //澤村追加
    void Start()
    {
        //ステータスをGameStartに
        status = GameState.GameStart;
    }


    //毎フレーム実行
    void Update()
    {
        //澤村追加
        if (status == GameState.GameClear)
        {
            clearUI.SetActive(true);


            /*
            switch (GameState.GameStart) 
            {
                //タイムのカウントダウン
                g_fTime = g_fTime - Time.time;
            }
            */

        }

        //澤村追加
        if (status == GameState.GameOver)
        {
            gameoverUI.SetActive(true);

        }
    }
    
    //ゲームステート変更
    public void SetGameState(GameState state)
    {
        gameState = state;
    }

    //ゲームステート毎実行用
    void PerGameState(GameState state)
    {
        switch (state) 
        {
        case GameState.Title:
            break;
        case GameState.StageSelect:
            break;
        case GameState.GameStart:
            break;
        case GameState.GameClear:   //澤村追加
            break;
            case GameState.GameOver:
            break;
        }
    }
}
