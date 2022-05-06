using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasamiModel : MonoBehaviour
{
    public GameObject PlayerState;  // プレイヤーの情報格納

    public Material blue;   // ハサミの青色のマテリアル
    public Material red;   // ハサミの赤色のマテリアル

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(PlayerState.GetComponent<PlayerControl>().eCutMode);
        // カットモードによるマテリアルの切り替え
        switch (PlayerState.GetComponent<PlayerControl>().eCutMode)
        {
            // 青色
            case PlayerControl.CutMode.CUT_ONE:
                gameObject.GetComponent<MeshRenderer>().material = blue;
                Debug.Log("blue");
                break;
            // 赤色
            case PlayerControl.CutMode.CUT_SMOOTH:
                gameObject.GetComponent<MeshRenderer>().material = red;
                Debug.Log("red");
                break;
        }

    }

    private void FixedUpdate()
    {
        //Debug.Log(PlayerState.GetComponent<PlayerControl>().eCutMode);
        //// カットモードによるマテリアルの切り替え
        //switch (PlayerState.GetComponent<PlayerControl>().eCutMode)
        //{
        //    // 青色
        //    case PlayerControl.CutMode.CUT_ONE:
        //        gameObject.GetComponent<MeshRenderer>().material = blue;
        //        break;
        //    // 赤色
        //    case PlayerControl.CutMode.CUT_SMOOTH:
        //        gameObject.GetComponent<MeshRenderer>().material = blue;
        //        break;
        //}

       
    }
}
