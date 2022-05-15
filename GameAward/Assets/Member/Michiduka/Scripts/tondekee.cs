//================================================
//
//      tondekee.cs
//      �p�[�e�B�N����΂��܂�
//
//------------------------------------------------
//      �쐬��: ���˗I��
//================================================

//================================================
// �J������
// 2022/05/11 �쐬�J�n
// �ҏW��: ���˗I��
//================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tondekee : MonoBehaviour
{
    //�Ώۂ̃p�[�e�B�N��
    [SerializeField] private GameObject targetParticle;
    private GameObject targetParticleInstance;

    private Vector3 spawnPos;
    private Vector3 pos;    //���W

    // Start is called before the first frame update
    void Start()
    {
        //�e�X�g�p
        //StartCoroutine("Tondekee");
        SpawnParticle();
    }

    void SpawnParticle()
    {
        //�p�[�e�B�N���ݒu
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
