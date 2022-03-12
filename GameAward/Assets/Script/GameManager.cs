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
    GameOver        //ゲームオーバー
    //result??
}
public class GameManager : MonoBehaviour
{
    //現在のスコア
    public int g_nScore;
    //最大スコア
    public int g_nMaxScore;
    //現在のタイム
    public float g_fTime;
    
    //スコア表示用テキスト
    [SerializeField] Text scoreText;
    
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

    //毎フレーム実行
    void Update()
    {
        /*
        switch (GameState.GameStart) 
        {
            //タイムのカウントダウン
            g_fTime = g_fTime - Time.time;
        }
        */
        
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
        case GameState.GameOver:
            break;
        }
    }
}