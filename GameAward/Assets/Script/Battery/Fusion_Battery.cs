//================================================
//
//      Fusion_Battery.cs
//      バッテリー同士の衝突
//
//------------------------------------------------
//      作成者: 柴山凜太郎
//      作成開始日：2022/4/25(月)
//================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fusion_Battery : MonoBehaviour
{
    // 放電オブジェクトの情報格納用(仮) )ほぼいらん
    public static List<GameObject> ball_list = new List<GameObject>();
    // バッテリーインスタンス
    Battery battery = new Battery();
    Vector3 pos;    // 放電生成座標

    // Start is called before the first frame update
    void Start()
    {
        // 放電プレハブ情報のロード
        disCharge.Energy_Ball = (GameObject)Resources.Load("Energy_Ball");
    }

    // Update is called once per frame
    void Update()
    {
        battery.ChargeEnergy();

        // 放電してる時は処理しない
        if (disCharge.EBClone != null)
            return;

        // バッテリー移動(仮)
        battery.fmove = 0.05f;
        transform.position += new Vector3(battery.fmove, 0, 0);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Battery_Minus")
            return;
        if (ChargeEnergy.Energy < 100.0f && Battery.Energy < 100.0f)
            return;

        // 移動量を0に
        battery.fmove = 0.0f;
        ChargeEnergy.move = 0.0f;

        // バッテリーのプラスとマイナスの真ん中に放電を出現させる
        pos = (transform.position + collision.transform.position) / 2;
        pos.y = 0;

        // クローンのインスタンス情報取得
        disCharge.EBClone = Instantiate(disCharge.Energy_Ball, pos, Quaternion.identity);
        ball_list.Add(disCharge.EBClone);
    }

    // ==================
    // バッテリークラス
    // ==================
    public class Battery
    {
        public float fmove;         // 移動速度

        public static float Energy; // エネルギー溜め

        // コンストラクタ
        public Battery()
        {
            fmove = 0.05f;
            Energy = 0;
        }

        // エネルギー溜め
        public void ChargeEnergy()
        {
            if (Energy >= 100)
            {
                Energy = 100;
                return;
            }
            ++Energy;
        }
    }
}
