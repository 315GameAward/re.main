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
        SpawnParticle();
    }

    void SpawnParticle()
    {
        //パーティクル設置
        //targetParticleInstance = GameObject.Instantiate(targetParticle, spawnPos, transform.rotation) as GameObject;
        targetParticleInstance = GameObject.Instantiate(targetParticle, new Vector3(0, 0, 0), transform.rotation) as GameObject;
        StartCoroutine("Tondekee");
    }

    IEnumerator Tondekee()
    {
        while (pos.y < 6.35f)
        {
            pos = targetParticleInstance.transform.position;
            targetParticleInstance.transform.Translate(0, 0.02f, 0);
            yield return new WaitForSeconds(0.01f);
        }

        while (pos.x < 3.1f)
        {
            pos = targetParticleInstance.transform.position;
            targetParticleInstance.transform.Translate(0.02f, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
