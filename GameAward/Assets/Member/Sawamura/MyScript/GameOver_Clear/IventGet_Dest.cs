using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum DestGame
{
    GameStart,      //ゲームスタート
    GameClear,
    GameOver        //ゲームオーバー
}
public class IventGet_Dest : MonoBehaviour
{
    //クリア用UI
    [SerializeField]
    GameObject clearUI;

    public DestGame status_dest;
    public IventPush_Dest target;

    void OnDisable()
    {
        target.OnDestroyed.RemoveAllListeners();
    }
    void OnEnable()
    {
        target.OnDestroyed.AddListener(() => {
            Debug.Log("targetが削除されました");
            // ここに処理を追加
            status_dest = DestGame.GameClear;
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        status_dest = DestGame.GameStart;
    }

    // Update is called once per frame
    void Update()
    {
        if(status_dest == DestGame.GameClear)
        {
            clearUI.SetActive(true);
        }
    }
}
