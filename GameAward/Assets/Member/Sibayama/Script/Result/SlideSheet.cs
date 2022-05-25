//===============================================================
//
//      SlideSheet.cs
//      ���U���g�X���C�h
//
//---------------------------------------------------------------
//      �쐬��: �ĎR꣑��Y
//      �쐬�J�n���F2022/5/18(��)
//===============================================================
//      �X�V�ҁF�ĎR꣑��Y
//      �X�V���F2022/05/25(��)
//      �X�V���e�F�������N���ăX�N���v�g���N���b�V����������
//                �o�b�N�A�b�v���Ă������̂����Ƃɍ�蒼���܂����B
//===============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // UI������

public class SlideSheet : MonoBehaviour
{
    
    public RectTransform slide;     // �X���C�h�̃p�����[�^�擾
    public float moveDistance;      // �ړ���


    static bool landing;    // �X���C�h���~�肫������
    // ���̃t���O��n��
    public static bool Landing { get { return landing; } }


    Vector3 pos;    // �X���C�h�̍��W

    // Start is called before the first frame update
    void Start()
    {
        landing = false;
        // ��ʊO�ɏo��悤�ݒu
        slide.transform.position = new Vector3(Screen.width / 2, Screen.height + slide.rect.height, 0.0f);

    }

    // Update is called once per frame
    void Update()
    {
        // ��ʂ̒��S�ɗ�����~�߂�
        if (slide.transform.position.y <= Screen.height / 2)
        {
            pos = slide.transform.position;
            pos.y = Screen.height / 2;
            landing = true;
            return;
        }
        // ����̈ʒu�܂ŉ�����
        slide.transform.position -= new Vector3(0.0f, moveDistance, 0.0f);

    }
}
