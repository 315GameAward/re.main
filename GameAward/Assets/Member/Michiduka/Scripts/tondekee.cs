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
using UnityEngine.InputSystem;

public class tondekee : MonoBehaviour
{
    //対象のパーティクル
    [SerializeField] private GameObject targetParticle;
    private GameObject targetParticleInstance;

    private Vector3 spawnPos;
    private Vector3 pos;    //座標

    public Life life;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        //テスト用
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SpawnParticle();
        }
    }

    void SpawnParticle()
    {
        //パーティクル設置
        //targetParticleInstance = GameObject.Instantiate(targetParticle, spawnPos, transform.rotation) as GameObject;
        targetParticleInstance = GameObject.Instantiate(targetParticle, new Vector3(0, 0, 0), transform.rotation) as GameObject;
        //z 1.5
        targetParticleInstance.transform.Translate(0, 0, 1.95f);
        StartCoroutine("Tondekee");
        //ライフ消去
        life.DelLife();
    }

    IEnumerator Tondekee()
    {
        //while (pos.y < 6.35f && pos.x < 2.1f)
        while (pos.x < 2.1f)
        {
            pos = targetParticleInstance.transform.position;
            targetParticleInstance.transform.Translate(0, 0.05f, 0);
            targetParticleInstance.transform.Translate(0.02f, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
        
    }
}
