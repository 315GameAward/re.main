using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public bool bFade = false; // fadeスタート
    float fAlpha = 0.0f;    // アルファ値
    static bool bGameOver = false;  // ゲームオーバーフラグ

    // Start is called before the first frame update
    void Start()
    {
        bFade = false;
        bGameOver = false;
        fAlpha = 0.0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (bFade)
        {
            fAlpha += 0.01f;
            if (fAlpha > 80.0f / 255.0f) fAlpha = 80.0f / 255.0f; // 透明度の制限
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, fAlpha);
        }
    }

    public void ShowGameOver()
    {
        bFade = true;
        bGameOver = true;
    }
}
