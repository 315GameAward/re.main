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

    private Vector3 spawnPos;   //スポーン座標

    //対象ライフ
    [SerializeField] private GameObject targetLife;
    //対象ライフA
    [SerializeField] private GameObject targetLifeA;

    public Animator targetAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //targetLife.GetComponent<Animator>();
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
        targetParticleInstance.transform.Translate(0, 0, 1f);
        StartCoroutine("Tondekee");
    }

    IEnumerator Tondekee()
    {
        //while (pos.y < 6.35f && pos.x < 2.1f)

        while (targetParticleInstance.transform.position.y < 4.55f)
        {
            targetParticleInstance.transform.Translate(0, 0.05f, 0);
            if (Life.instance.nLife == 3)
                targetParticleInstance.transform.Translate(0.035f, 0, 0);
            else if (Life.instance.nLife == 2)
                targetParticleInstance.transform.Translate(0.029f, 0, 0);
            else if (Life.instance.nLife == 1)
                targetParticleInstance.transform.Translate(0.024f, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }

        //ライフ消去
        Life.instance.DelLife();

        //ライフ設置
        targetLife.SetActive(true);
        Debug.Log(Life.instance.nLife);

        //1 = 335.3
        //2 = 410.4
        //3 = 485.2

        if (Life.instance.nLife == 1)
        {
            targetLifeA.transform.Translate(-75.1f, 0.0f, 0.0f);
        }
        else if (Life.instance.nLife == 0)
        {
            targetLifeA.transform.Translate(-75.1f, 0.0f, 0.0f);
        }

        targetAnimator.Play("Life");
        yield return new WaitForSeconds(.5f);

        //ライフ隠
        targetLife.SetActive(false);

        //破壊
        Destroy(targetParticleInstance);
    }
}
