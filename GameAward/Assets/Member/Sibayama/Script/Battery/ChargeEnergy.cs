//================================================
//
//      ChargeEnergy.cs
//      エネルギー溜める処理
//
//------------------------------------------------
//      作成者: 柴山凜太郎
//      作成開始日：2022/05/05(水)
//================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEnergy : MonoBehaviour
{
    public static float Energy = 0; // エネルギー溜め
    public static float move;
    // Start is called before the first frame update
    void Start()
    {
        move = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        Charge();
        // 放電してる時は処理しない
        if (disCharge.EBClone != null)
            return;
        move = 0.05f;
        transform.position -= new Vector3(move, 0.0f, 0.0f);
    }

    // エネルギー溜め
    void Charge()
    {
        if (Energy >= 100)
        {
            Energy = 100;
            return;
        }
        ++Energy;
    }
}
