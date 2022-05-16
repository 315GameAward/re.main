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
using UnityEngine.InputSystem;

public class tondekee : MonoBehaviour
{
    //�Ώۂ̃p�[�e�B�N��
    [SerializeField] private GameObject targetParticle;
    private GameObject targetParticleInstance;

    private Vector3 spawnPos;
    private Vector3 pos;    //���W

    public Life life;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        //�e�X�g�p
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SpawnParticle();
        }
    }

    void SpawnParticle()
    {
        //�p�[�e�B�N���ݒu
        //targetParticleInstance = GameObject.Instantiate(targetParticle, spawnPos, transform.rotation) as GameObject;
        targetParticleInstance = GameObject.Instantiate(targetParticle, new Vector3(0, 0, 0), transform.rotation) as GameObject;
        //z 1.5
        targetParticleInstance.transform.Translate(0, 0, 1.95f);
        StartCoroutine("Tondekee");
        //���C�t����
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
