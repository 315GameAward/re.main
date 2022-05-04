//================================================
//
//      disCharge.cs
//      放電処理
//
//------------------------------------------------
//      作成者: 柴山凜太郎
//      作成開始日：2022/4/27(水)
//================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disCharge : MonoBehaviour
{

    public static GameObject Energy_Ball;   // バッテリーに渡すオブジェクト生成用の情報
    public static GameObject EBClone;       // 複製したオブジェクトの情報

    public float SetRealTime;   // 放電生存時間
    float CountFrame;           // フレームのカウント
    float CountTimer;           // 生存時間のフレーム表記

    public string Tagname;      // タグの名前

    void Awake()
    {
        // 生存時間をリアルタイム設定
        QualitySettings.vSyncCount = 0;
        CountFrame = Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
        CountTimer = SetRealTime * CountFrame;
    }

    // Update is called once per frame
    void Update()
    {
        DestroyTimer();
    }

    // ハサミとの当たり判定
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != Tagname)
            return;

        // ここにハサミの移動量を0にする、
        // またはフラグを立てる処理を記入
        //movePlayer.move = 0.0f;


    }

    // 一定時間で放電が消える
    void DestroyTimer()
    {
        if (CountTimer < 0.0f)
        {
            CountTimer = 0.0f;
            Fusion_Battery.ball_list.Remove(EBClone);
            // エネルギーリセット
            ChargeEnergy.Energy = 0;
            Fusion_Battery.Battery.Energy = 0;
            // 放電オブジェクト削除
            Destroy(gameObject);
            return;
        }
        --CountTimer;
    }
}

