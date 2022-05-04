//================================================
//
//      movePlayer.cs
//      プレイヤー移動処理
//
//------------------------------------------------
//      作成者: 柴山凜太郎
//      作成開始日：2022/05/05(水)
//================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlayer : MonoBehaviour
{
    public static float move;

    // Start is called before the first frame update
    void Start()
    {
        move = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        if (disCharge.EBClone == null)
            move = 0.05f;
        transform.position -= new Vector3(0.0f, 0.0f, move);
    }


}
