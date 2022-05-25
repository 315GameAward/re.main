using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public enum ColGame_a
//{
//    GameStart,      //ゲームスタート
//    GameClear,
//    GameOver        //ゲームオーバー
//}

public class IventGet_col_a : MonoBehaviour
{
    //シリアライズ
    //ゲームオーバー用UI
    //[SerializeField]
    //GameObject gameoverUI;

    //public ColGame_a status_col;
    public IventPush_col_a[] target;

    int i = 0;

    void OnDisable()
    {
        target[i].OnCollisioned.RemoveAllListeners();
    }
    void OnEnable()
    {
        target[i].OnCollisioned.AddListener(() => {
            Debug.Log("targetがぶつかりました");
            // ここに処理を追加
            //status_col = ColGame_a.GameOver;
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        //ステータスをGameStartに
        //status_col = ColGame_a.GameStart;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    //澤村追加
    //    // コウヅキ改変
    //    // Cキーでも表示できるようにしました
    //    if (status_col == ColGame_a.GameOver || Input.GetKeyUp(KeyCode.C))
    //    {
    //        gameoverUI.SetActive(true);
    //    }
    //    if( Input.GetKeyUp(KeyCode.R))
    //    {
    //        gameoverUI.SetActive(false);
    //    }
    //}
}
