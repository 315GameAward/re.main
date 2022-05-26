using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Timer_Minus : MonoBehaviour
{
    // ゲームオーバー画像取得(コウヅキ)
    public GameObject image_gameOver;

    float countTime = 60;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (countTime > 0.0f)
        {
            //countTimeに、ゲームが開始してからの秒数を格納
            countTime -= Time.deltaTime;

        }
        else if (countTime < 0.0f)
        {
            countTime = 0.0f;
            // ゲームオーバー表示
            image_gameOver.GetComponent<Game_Over>().SetGMOV(true);
        }

        //少数を2桁にして表示
        GetComponent<Text>().text = countTime.ToString("F2");
    }
}
