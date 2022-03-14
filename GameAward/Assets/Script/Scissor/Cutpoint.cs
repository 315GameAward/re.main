//======================================================
//
//        Cutpoint.cs
//        �n�T�~���؂����O�Ղ̏���
//
//------------------------------------------------------
//      �쐬��:���{���V��
//======================================================

//======================================================
// �J������
// 2022/03/13 �쐬�J�n
// �ҏW��:���{���V��
//======================================================
//======================================================
// �J������
// 2022/03/14 �R�����g�̒ǉ�
// �ҏW��:���{���V��
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutpoint : MonoBehaviour
{
    // �ϐ��錾
    public List<Vector3> m_vCotPoint;   // �n�T�~�̋O�՗p���X�g

    

    // Start is called before the first frame update
    void Start()
    {
        m_vCotPoint.Clear();    // ���X�g�̒��g���N���A
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    // �����ɓ����葱���Ă���Ƃ�
    private void OnTriggerStay(Collider other)
    {
        // �w��̖��O�������珈������
        if ((other.gameObject.name == "Parper" || other.gameObject.name == "cut obj"))
        {
            // ���C�L���X�g���Đ��m�Ȓ��_���쐬
            Ray ray = new Ray(gameObject.transform.position, -gameObject.transform.up); // �n�T�~�̏�̐n�̂����_����^���Ɍ����Ẵ��C
            RaycastHit hit; // �����������̏����i�[����ϐ�

            // �O�Ղ̐���1�ȏ゠��Ƃ�
            if (m_vCotPoint.Count >= 1)
            {
                // ���C�L���X�g���������Ƃ� �O�Ղ̍Ō�ɂ�����W�ƃ��C�L���X�g���ďo�����W���ꏏ�̎��͏��������Ȃ�
                if (Physics.Raycast(ray, out hit) && hit.point != m_vCotPoint[m_vCotPoint.Count - 1])
                {
                    // �O�Ղ�ǉ�
                    m_vCotPoint.Add(hit.point);
                    Debug.Log("�O�Ղ�ǉ�");
                    Debug.Log("�O�Ղ̐�:" + m_vCotPoint.Count);

                }
            }
            else
            {
                // ���C�L���X�g���������Ƃ�
                if (Physics.Raycast(ray, out hit))
                {
                    // �O�Ղ�ǉ�
                    m_vCotPoint.Add(hit.point);
                    Debug.Log("�O�Ղ�ǉ�");
                    Debug.Log("�O�Ղ̐�:" + m_vCotPoint.Count);

                }
            }

            // ���b�V���𕪊����鏈��


        }
    }

    // �������痣���u��
    private void OnTriggerExit(Collider other)
    {
        // �w��̖��O�������珈������
        if ((other.gameObject.name == "Parper" || other.gameObject.name == "cut obj"))
        {
            // �O�Ղ�ǉ�
            m_vCotPoint.Add(gameObject.transform.position);
            Debug.Log("�O�Ղ�ǉ�");
            Debug.Log("�O�Ղ̐�:" + m_vCotPoint.Count);

            // ���b�V���𕪊����鏈��

            // ���b�V����؂蕪���鏈��

            // ���_���폜
            m_vCotPoint.Clear();
            Debug.Log("�O�Ղ��폜");
            Debug.Log("�O�Ղ̐�:" + m_vCotPoint.Count);

        }


    }
}
