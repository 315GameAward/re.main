//================================================
//
//      AppearanceEvaluation.cs
//      ���ʔ��\
//
//------------------------------------------------
//      �쐬��: �ĎR꣑��Y
//      �쐬�J�n���F2022/5/20(��)
//================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // UI��M��̂ɕK�v


public class AppearanceEvaluation : MonoBehaviour
{
    const int Max_Obj = 2;  // �]�����ڐ�
    
    public float stopTime;  // �X�^���v�����Ԋu
    
    
    GameObject layer;   // �ǃf�J���X�^���v��������̃��U���g�ڂ���

    // �A�N�e�B�u��Ԃ��Ǘ�����I�u�W�F�N�g
    GameObject[] AcObj = new GameObject[Max_Obj];

    GameObject slideObject;     // SlideSheet.cs���A�^�b�`���ꂽ�I�u�W�F�N�g�擾�p
    ParperSlide slide;           // SlideSheet.cs�N���X�擾
    // �]���Ɏg���X�^���v�e�N�X�`��

    private void Awake()
    {
        //--- �q�I�u�W�F�N�g�擾����\��

        layer = GameObject.Find("layer");
        layer.gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        AcObj[0] = GameObject.Find("layer/last_result");
        AcObj[0].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        AcObj[1] = GameObject.Find("layer/SelectButton");

        for (var i = 0; i < Max_Obj; ++i)
        {
            // �I�u�W�F�N�g�̔�A�N�e�B�u��
            AcObj[i].SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        slideObject = GameObject.Find("ParperClear");
        slide = slideObject.GetComponent<ParperSlide>();
    }

    // Update is called once per frame
    void Update()
    {
        // �~�肫���Ă��Ȃ��Ȃ珈�����Ȃ�
        if (!slide.EndAnim)    // ���̏������A�j���[�V�����𓮂����X�N���v�g�Ɏ����Ă����B
            return;                 // ����ɃA�j���[�V�������I���������̃t���O�������ŏ����B
        
        StartCoroutine("Defeat");
    }

    // ���Ԋu�����U���g���ڕ\��
    IEnumerator Defeat()
    {
        yield return new WaitForSeconds(stopTime);

        // �h�f�J�X�^���v
        AcObj[0].SetActive(true);
        AcObj[0].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(stopTime);

        // �Z���N�g�{�^��
        AcObj[1].SetActive(true);

        // �ڂ����p���C���[
        layer.gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);   
    }
}
