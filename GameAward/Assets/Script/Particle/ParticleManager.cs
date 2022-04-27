//================================================
//
//      ParticleManager.cs
//      パーティクルのマネージャ
//
//------------------------------------------------
//      作成者: 道塚悠基
//================================================

//================================================
// 開発履歴
// 2022/03/20 作成開始
// 編集者: 道塚悠基
//================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private GameObject particleTest;    //パーティクル(テスト)オブジェクト
    private GameObject particleTestInstance;

    private void Start()
    {
        StartCoroutine(DelayMethod(1f, () =>
        {
            ParticleTestSpawn(3f, new Vector3(0, 0, 0));
        }));
    }

    //spawnTime = 何秒スポーンさせるか
    //position  = パーティクルのポジション
    public void ParticleTestSpawn(float spawnTime, Vector3 position)
    {
        if (particleTestInstance == null)    //ポーズUIがない場合
        {
            particleTestInstance = GameObject.Instantiate(particleTest, position, transform.rotation) as GameObject;    //パーティクル設置
            StartCoroutine(DelayMethod(spawnTime, () =>
            {
                Destroy(particleTestInstance);   //パーティクル破壊
            }));
            
        }
        else
        {
            Destroy(particleTestInstance);   //パーティクル破壊
        }
    }

    //ディレイ用コルーチン
    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}