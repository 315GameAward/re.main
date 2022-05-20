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

    private Vector3 spawnPos;   //�X�|�[�����W

    //�Ώۃ��C�t
    [SerializeField] private GameObject targetLife;
    //�Ώۃ��C�tA
    [SerializeField] private GameObject targetLifeA;

    public Animator targetAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //targetLife.GetComponent<Animator>();
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

        //���C�t����
        Life.instance.DelLife();

        //���C�t�ݒu
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

        //���C�t�B
        targetLife.SetActive(false);

        //�j��
        Destroy(targetParticleInstance);
    }
}
