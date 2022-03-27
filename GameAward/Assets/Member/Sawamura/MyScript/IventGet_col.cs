using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ColGame
{
    GameStart,      //ゲームスタート
    GameClear,
    GameOver        //ゲームオーバー
}

public class IventGet_col : MonoBehaviour
{
    //シリアライズ
    //ゲームオーバー用UI
    [SerializeField]
    GameObject gameoverUI;

    public ColGame status_col;
    public IventPush_col target;

    void OnDisable()
    {
        target.OnCollisioned.RemoveAllListeners();
    }
    void OnEnable()
    {
        target.OnCollisioned.AddListener(() => {
            Debug.Log("targetがぶつかりました");
            // ここに処理を追加
            status_col = ColGame.GameOver;
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        //ステータスをGameStartに
        status_col = ColGame.GameStart;
    }

    // Update is called once per frame
    void Update()
    {
        //澤村追加
        if (status_col == ColGame.GameOver)
        {
            gameoverUI.SetActive(true);
        }
    }
}
