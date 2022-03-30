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

public class CutPoint : MonoBehaviour
{
    // �ϐ��錾
    public List<Vector3> m_vCotPoint;   // �n�T�~�̋O�՗p���X�g
    public List<Vector3> CutPointTest;   // �n�T�~�̋O�՗p���X�g(�e�X�g)

    public MeshCut ground;

    private bool triggerFlg = false;    // �f�o�b�N�p�g���K�[�t���O


    // Start is called before the first frame update
    void Start()
    {
        m_vCotPoint.Clear();    // ���X�g�̒��g���N���A
        CutPointTest.Clear();    // ���X�g�̒��g���N���A
    }

    // Update is called once per frame
    void Update()
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
                if (hit.collider.gameObject.name == "Parper" || hit.collider.gameObject.name == "cut obj")
                {
                    // �O�Ղ�ǉ�
                    m_vCotPoint.Add(hit.point);
                    Debug.Log("���C�������������W:" + hit.point);


                    // ���b�V���𕪊����鏈��
                    hit.collider.gameObject.GetComponent<MeshCut>().Devision(m_vCotPoint);


                    Debug.Log("�O�Ղ�ǉ�");
                    Debug.Log("�O�Ղ̐�:" + m_vCotPoint.Count);
                    Debug.Log("�O�Ղ̍��W:" + m_vCotPoint[m_vCotPoint.Count - 1]);

                }

                // �f�o�b�N�p������\��
                if(!triggerFlg)
                {
                    Debug.Log(hit.collider.gameObject.name + "�ɓ�������");
                    triggerFlg = true;  // �f�o�b�N�p�g���K�[�t���OON
                }
                Debug.Log(hit.collider.gameObject.name + "�ɓ�������");
            }
            else    // ���C�L���X�g���������ĂȂ��Ƃ�
            {
                // ���_���폜
                if (m_vCotPoint.Count == 0) return;
                m_vCotPoint.Clear();
                Debug.Log("�O�Ղ��폜");
                Debug.Log("�O�Ղ̐�:" + m_vCotPoint.Count);
                triggerFlg = false; // �f�o�b�N�p�g���K�[�t���OOFF
            }
        }
        else
        {
            // ���C�L���X�g���������Ƃ�
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "Parper" || hit.collider.gameObject.name == "cut obj")
                {

                    // �O�Ղ�ǉ�
                    m_vCotPoint.Add(hit.point);
                    Debug.Log("���C�������������W:" + hit.point);

                    // ���b�V���𕪊����鏈��
                    hit.collider.gameObject.GetComponent<MeshCut>().Devision(m_vCotPoint);


                    Debug.Log("�O�Ղ�ǉ�");
                    Debug.Log("�O�Ղ̐�:" + m_vCotPoint.Count);
                    Debug.Log("�O�Ղ̍��W:" + m_vCotPoint[m_vCotPoint.Count - 1]);
                }

                // �f�o�b�N�p������\��
                if (!triggerFlg)
                {
                    Debug.Log(hit.collider.gameObject.name + "�ɓ�������");
                    triggerFlg = true;  // �f�o�b�N�p�g���K�[�t���OON
                }
                
            }
            else    // ���C�L���X�g���������ĂȂ��Ƃ�
            {
                if (m_vCotPoint.Count == 0) return;
                // ���_���폜
                m_vCotPoint.Clear();
                Debug.Log("�O�Ղ��폜");
                Debug.Log("�O�Ղ̐�:" + m_vCotPoint.Count);
            }
        }
        Debug.Log("a" );
        // ���C�L���X�g�̕\��
        Debug.DrawRay(ray.origin,ray.direction * 10,Color.red,3,false);

        // ���C�L���X�g���������Ƃ� 
        if (Physics.Raycast(ray, out hit) )
        {
            // �e�X�g�p�̃|�C���g������Ƃ�
            if(CutPointTest.Count > 0)
            { 
                // �q�b�g�������W�ƍŌ�Ɋi�[�������W���Ⴄ�Ƃ����X�g�Ɋi�[������
                if(hit.point != CutPointTest[CutPointTest.Count -1])
                {
                    CutPointTest.Add(hit.point);    // �q�b�g�������W���i�[

                    // �q�b�g���������؂肽�����̂ƈႤ�Ƃ��͈�O�̃|�C���g���폜������
                    if(hit.collider.gameObject.name != "Plane")
                    {
                        CutPointTest.RemoveAt(CutPointTest.Count - 2);
                    }
                }
            }
            else //�e�X�g�p�̃|�C���g���Ȃ��Ƃ�
            {
                CutPointTest.Add(hit.point);    // �q�b�g�������W���i�[
            }

        }


    }

    private void OnDrawGizmos()
    {
        // �e�X�g�p�̃|�C���g��\��������
        if(CutPointTest.Count > 0)
        {
            for(int i = 0;i < CutPointTest.Count;i++)
            {
                Gizmos.color = new Color(0, 1, 0, 1);   // �F�̎w��
                Gizmos.DrawSphere(CutPointTest[i], 0.05f);  // ���̕\��

            }
        }

        // �e�X�g�p�̃|�C���g���Ȃ�����\��������
        if(CutPointTest.Count >= 2)
        {
            for (int i = 1; i < CutPointTest.Count; i++)
            {
                Gizmos.color = new Color(0, 0.5f, 0, 1);    // �F�̎w��                
                Gizmos.DrawLine(CutPointTest[i - 1], CutPointTest[i]);  // ���̕\��
            }
        }
    }
}



//    private void OnTriggerEnter(Collider other)
//    {
//        // �w��̖��O�������珈������
//        if ((other.gameObject.name == "Parper" || other.gameObject.name == "cut obj"))
//        {
//            // ���C�L���X�g���Đ��m�Ȓ��_���쐬
//            Ray ray = new Ray(gameObject.transform.position, -gameObject.transform.up); // �n�T�~�̏�̐n�̂����_����^���Ɍ����Ẵ��C
//            RaycastHit hit; // �����������̏����i�[����ϐ�

//            // �O�Ղ̐���1�ȏ゠��Ƃ�
//            if (m_vCotPoint.Count >= 1)
//            {
//                // ���C�L���X�g���������Ƃ� �O�Ղ̍Ō�ɂ�����W�ƃ��C�L���X�g���ďo�����W���ꏏ�̎��͏��������Ȃ�
//                if (Physics.Raycast(ray, out hit) && hit.point != m_vCotPoint[m_vCotPoint.Count - 1])
//                {
//                    // �O�Ղ�ǉ�
//                    m_vCotPoint.Add(hit.point);
//                    Debug.Log("���C�������������W:" + hit.point);

//                    // ���b�V���𕪊����鏈��
//                    other.gameObject.GetComponent<MeshCut>().Devision(m_vCotPoint);

//                    Debug.Log("�O�Ղ�ǉ�");
//                    Debug.Log("�O�Ղ̐�:" + m_vCotPoint.Count);
//                    Debug.Log("�O�Ղ̍��W:" + m_vCotPoint[m_vCotPoint.Count - 1]);

//                }
//            }
//            else
//            {
//                // ���C�L���X�g���������Ƃ�
//                if (Physics.Raycast(ray, out hit))
//                {
//                    // �O�Ղ�ǉ�
//                    m_vCotPoint.Add(hit.point);
//                    Debug.Log("���C�������������W:" + hit.point);

//                    // ���b�V���𕪊����鏈��
//                    other.gameObject.GetComponent<MeshCut>().Devision(m_vCotPoint);


//                    Debug.Log("�O�Ղ�ǉ�");
//                    Debug.Log("�O�Ղ̐�:" + m_vCotPoint.Count);
//                    Debug.Log("�O�Ղ̍��W:" + m_vCotPoint[m_vCotPoint.Count - 1]);
//                }
//            }




//        }
//    }
//    // �����ɓ����葱���Ă���Ƃ�
//    private void OnTriggerStay(Collider other)
//    {
//        // �w��̖��O�������珈������
//        if ((other.gameObject.name == "Parper" || other.gameObject.name == "cut obj"))
//        {
//            // ���C�L���X�g���Đ��m�Ȓ��_���쐬
//            Ray ray = new Ray(gameObject.transform.position, -gameObject.transform.up); // �n�T�~�̏�̐n�̂����_����^���Ɍ����Ẵ��C
//            RaycastHit hit; // �����������̏����i�[����ϐ�

//            // �O�Ղ̐���1�ȏ゠��Ƃ�
//            if (m_vCotPoint.Count >= 1)
//            {
//                // ���C�L���X�g���������Ƃ� �O�Ղ̍Ō�ɂ�����W�ƃ��C�L���X�g���ďo�����W���ꏏ�̎��͏��������Ȃ�
//                if (Physics.Raycast(ray, out hit) && hit.point != m_vCotPoint[m_vCotPoint.Count - 1])
//                {
//                    // �O�Ղ�ǉ�
//                    m_vCotPoint.Add(hit.point);
//                    Debug.Log("���C�������������W:" + hit.point);


//                    // ���b�V���𕪊����鏈��
//                    other.gameObject.GetComponent<MeshCut>().Devision(m_vCotPoint);

//                    Debug.Log("�O�Ղ�ǉ�");
//                    Debug.Log("�O�Ղ̐�:" + m_vCotPoint.Count);
//                    Debug.Log("�O�Ղ̍��W:" + m_vCotPoint[m_vCotPoint.Count - 1]);

//                }
//            }
//            else
//            {
//                // ���C�L���X�g���������Ƃ�
//                if (Physics.Raycast(ray, out hit))
//                {
//                    // �O�Ղ�ǉ�
//                    m_vCotPoint.Add(hit.point);
//                    Debug.Log("���C�������������W:" + hit.point);

//                    // ���b�V���𕪊����鏈��
//                    other.gameObject.GetComponent<MeshCut>().Devision(m_vCotPoint);


//                    Debug.Log("�O�Ղ�ǉ�");
//                    Debug.Log("�O�Ղ̐�:" + m_vCotPoint.Count);
//                    Debug.Log("�O�Ղ̍��W:" + m_vCotPoint[m_vCotPoint.Count - 1]);
//                }
//            }




//        }
//    }

//    // �������痣���u��
//    private void OnTriggerExit(Collider other)
//    {
//        // �w��̖��O�������珈������
//        if ((other.gameObject.name == "Parper" || other.gameObject.name == "cut obj"))
//        {
//            // �O�Ղ�ǉ�
//            m_vCotPoint.Add(gameObject.transform.position);
//            Debug.Log("�O�Ղ�ǉ�");
//            Debug.Log("�O�Ղ̐�:" + m_vCotPoint.Count);

//            // ���b�V���𕪊����鏈��

//            // ���b�V����؂蕪���鏈��

//            // ���_���폜
//            m_vCotPoint.Clear();
//            Debug.Log("�O�Ղ��폜");
//            Debug.Log("�O�Ղ̐�:" + m_vCotPoint.Count);

//        }


//    }
//}
