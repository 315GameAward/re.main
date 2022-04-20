using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum DestGame_a
{
    GameStart,      //ゲームスタート
    GameClear,
    GameOver        //ゲームオーバー
}
public class IventGet_Dest_a : MonoBehaviour
{
    //クリア用UI
    [SerializeField]
    GameObject clearUI;

    public DestGame_a status_dest;
    public IventPush_Dest_a target;

    void OnDisable()
    {
        target.OnDestroyed.RemoveAllListeners();
    }
    void OnEnable()
    {
        target.OnDestroyed.AddListener(() => {
            Debug.Log("targetが削除されました");
            // ここに処理を追加
            status_dest = DestGame_a.GameClear;
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        status_dest = DestGame_a.GameStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (status_dest == DestGame_a.GameClear　|| Input.GetKeyUp(KeyCode.V))
        {
            clearUI.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            clearUI.SetActive(false);
        }
    }
}
