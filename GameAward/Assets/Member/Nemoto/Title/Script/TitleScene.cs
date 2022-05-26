using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    ControlBinds cb;    // コントローラーバインド

    [SerializeField] AudioSource selectSE;  // セレクトSE

    public bool bTrigger = false; // トリガーフラグ


    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        cb = new ControlBinds();
        bTrigger = false;

        // イベントの作成
        cb.TitleScene.MoveScene.started += OnMoveScene;
        cb.TitleScene.MoveScene.performed += OnMoveScene;

        // 使用可能にする
        cb.TitleScene.Enable();

        //selectSE = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bTrigger)
        {
            // SEが鳴り終わったらシーンの移動
            if (!selectSE.isPlaying)
            {
                // シーンの移動
                SceneManager.LoadScene("AreaSelect");
                bTrigger = false;
            }
               
        }
        else
        {

        }
    }

    private void OnMoveScene(InputAction.CallbackContext context)
    {
        Debug.Log("シーンの移動");

        selectSE.Play();    // SEの再生

        bTrigger = true;

       
        cb.TitleScene.Disable();
    }
}
