//================================================
//
//      tondekee.cs
//      パーティクル飛ばします
//
//------------------------------------------------
//      作成者: 道塚悠基
//================================================

//================================================
// 開発履歴
// 2022/05/11 作成開始
// 編集者: 道塚悠基
//================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tondekee : MonoBehaviour
{
    //対象のパーティクル
    [SerializeField] private GameObject targetParticle;
    private GameObject targetParticleInstance;

    private Vector3 spawnPos;
    private Vector3 pos;    //座標

    // Start is called before the first frame update
    void Start()
    {
        //テスト用
        //StartCoroutine("Tondekee");
    }

    void SpawnParticle()
    {
        targetParticleInstance = GameObject.Instantiate(targetParticle, spawnPos, transform.rotation) as GameObject;    //パーティクル設置
    }

    IEnumerator Tondekee()
    {
        while (pos.y < 3.0f)
        {
            pos = targetParticle.transform.position;
            transform.Translate(0, 0.02f, 0);
            yield return new WaitForSeconds(0.01f);
        }

        while (pos.x < 3.0f)
        {
            pos = targetParticle.transform.position;
            transform.Translate(0.02f, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
