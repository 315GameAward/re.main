//========================
// 
//      Life
// 		プレイヤー体力
//
//--------------------------------------------
// 作成者：上月大地
//========================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    // 体力
    public int pLife;

    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        if(pLife <= 0)
        {
            // ゲームオーバー呼び出し
        }
    }

    //========================
    //      
    //      体力減少関数
    //
    //========================
    public void DelLife()
    {
        // 体力を減らす
        pLife--;
    }
}
