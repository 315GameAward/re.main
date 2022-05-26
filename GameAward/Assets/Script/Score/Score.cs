using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UIのパラメータを変更する
using UnityEngine.UI;

//このスクリプトで得点を加算するのではなく、
//得点の表示を行う

public class Score : MonoBehaviour
{
    // 外でも呼び出せるようにする
    public static Score instance;

    //-----スコアの表示
    public Text scoreText;

    //-----ハイスコアを表示する
    //public Text highScoreText;

    //-----スコア
    private int score;

    //-----ハイスコア
    //private int highScore;

    //-----保存するためのキー（未完成）

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        //スコアがハイスコアより多ければ
        //if (highScore < score)
        //{
        //    highScore = score;
        //}

        //スコア・ハイスコアを表示する

        // 限界値かどうか
        if (score >= 100)
        {
            score = 100;
        }

        //スコア
        scoreText.text = score.ToString();

        //ハイスコア
        //highscoretext.text = highscore.tostring();


    }

    private void Initialize()
    {
        //スコアを0に戻す
        score = 0;

        //ハイスコア
    }

    //
    public void AddScore(int point)
    {
        score = score + point;
    }

    //ハイスコアの保持
    public void Save()
    {
        //ハイスコアを保持する

        //ゲーム開始前の状態に戻す
        Initialize();
    }

    // スコア取得用
    public int GetScore()
    {
        return score;
    }
}
