//======================================================
//
//       SkipScene.cs
//        シーンを飛ばす処理
//
//------------------------------------------------------
//      作成者:大森理句
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;//キー押したらの処理のため
using UnityEngine.SceneManagement;



public class SkipScene : MonoBehaviour
{

    // スキップ用変数
    private float cnt_time = 0.0f;      // タイマー測定用
    public float change_time = 22.0f;   // 遷移予定時間(初期値：22.0f)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 時間測定
        cnt_time += Time.deltaTime;

        // 強制遷移
        if(cnt_time >= change_time)
        {
            cnt_time = 0.0f;
            SceneManager.LoadScene("GameScene");
        }

        // Xキーを押したら
        if (Keyboard.current.xKey.isPressed)
        {
            cnt_time = 0.0f;
            SceneManager.LoadScene("GameScene");
        }
        if(Gamepad.current.xButton.wasPressedThisFrame)
        {
            cnt_time = 0.0f;
            SceneManager.LoadScene("GameScene");
        }
    }
}
