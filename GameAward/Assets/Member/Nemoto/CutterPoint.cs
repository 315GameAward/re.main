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


public class CutterPoint : MonoBehaviour
{
    // �ϐ��錾
    public List<Vector3> m_vCotPoint;   // �n�T�~�̋O�՗p���X�g
    public List<Vector3> CutPointTest;   // �n�T�~�̋O�՗p���X�g(�e�X�g)

    public MeshCut ground;

    private bool triggerFlg = false;    // �f�o�b�N�p�g���K�[�t���O

    // �����p�ϐ�
    public Vector2 v;  
    public Vector2 v1;  
    public Vector2 v2;
    public Vector2 p;
    public float t1;
    public float t2;

    private int count = 0;

    // �����\����
    public struct Segment
    {
        public Vector2 s; // �n�_
        public Vector2 v; // �����x�N�g���i�����̒������S���̂Ő��K�����Ȃ��悤�ɁI�j
    };

   
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
        //Debug.Log("a" );
        // ���C�L���X�g�̕\��
        //Debug.DrawRay(ray.origin,ray.direction * 5,Color.red,3,false);

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

                    // �q�b�g���������؂肽�����̂ƈႤ�Ƃ��͈�O�̃|�C���g���폜�������B�Ȃ�Ȃ�S���폜���Ă������̂��H          
                    if(hit.collider.gameObject.name != "Plane")
                    {
                        // �J�b�g�|�C���g�̍폜
                        CutPointTest.RemoveAt(CutPointTest.Count - 2);
                        CutPointTest.Clear();
                    }

                    // �q�b�g�������b�V���̃|���S����
                    Debug.Log("�|���S����" + hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertexCount);

                    //Debug.Log("�|���S����" + hit.triangleIndex);
                }
            }
            else //�e�X�g�p�̃|�C���g���Ȃ��Ƃ�
            {
                CutPointTest.Add(hit.point);    // �q�b�g�������W���i�[
            }

        }

        // �J�b�g�|�C���g�̎n�_�ƏI�_�����|���S���̕ԏ�ɂ�������
        if(CutPointTest.Count >= 2)
        {
            // ��������񂾂��ɂ��鏈��
            if (CutPointTest.Count == count) return;

            // �����������b�V���̕ӂ̐���������
            for(int i = 0;i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertexCount - 1;i++)
            {
                // �����Ɛ����̎n�_���Ȃ����x�N�g��
                v = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].x - CutPointTest[0].x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].z - CutPointTest[0].z);

                // ����
                v1 = new Vector2(CutPointTest[1].x - CutPointTest[0].x, CutPointTest[1].z - CutPointTest[0].z);
                v2 = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 1].x - hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 1].z - hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].z);

                // �����̎n�_�����_�̃x�N�g��
                t1 = (v.x * v2.y - v2.x * v.y) / (v1.x * v2.y - v2.x * v1.y);
                t2 = (v.x * v1.y - v1.x * v.y) / (v1.x * v2.y - v2.x * v1.y);

                // ��_
                p = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].z) + new Vector2(v2.x * t2, v2.y * t2);

                // �����Ɛ�����������Ă��邩
                const float eps = 0.00001f;
                if (t1 + eps < 0 || t1 - eps > 1 || t2 + eps < 0 || t2 - eps > 1)
                {
                    // Debug.Log("�������ĂȂ�");
                }
                else
                {
                    Debug.Log("�������Ă�");
                    Debug.Log("�����������W:" + p);
                    Debug.Log("����������:" + (double)t1 + ":" + (double)t2);
                    CutPointTest[0] = new Vector3(p.x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].y, p.y);
                }

                //// �����̐錾(�J�b�g�|�C���g����)
                //Segment seg1;
                //seg1.s = new Vector2(CutPointTest[0].x, CutPointTest[0].z); // �����̎n�_
                //seg1.v = new Vector2(CutPointTest[1].x, CutPointTest[1].z); // �����̎n�_

                //// �����̐錾(���b�V������)
                //Segment seg2;
                //seg2.s = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].x    , hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].z); // �����̎n�_
                //seg2.v = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 1].x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 1].z); // �����̎n�_


                //if (ColSegments(seg1,seg2,0,0,new Vector2(0,0)))
                //{
                //    Debug.Log("�������Ă�");
                //    CutPointTest[0] = new Vector3(p.x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].y, p.y);

                //}



            }

            // �����������b�V���̃|���S�����Ƃɏ���
            for (int i = 0; i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertexCount ; i += 3)
            {

            }

                //Debug.Log("t2:"+t2);


                count = CutPointTest.Count;
           
        }

        // ���Ɛ��̌�_
        

    }

    private void OnDrawGizmos()
    {
        // �e�X�g�p�̃|�C���g��\��������
        if(CutPointTest.Count > 0)
        {
            // �n�_�̃J�b�g�|�C���g�M�Y��
            Gizmos.color = new Color(1, 1, 0, 1);   // �F�̎w��
            Gizmos.DrawSphere(CutPointTest[0], 0.05f);  // ���̕\��

            // �r���̃J�b�g�|�C���g�M�Y��
            for (int i = 1;i < CutPointTest.Count;i++)
            {          
                Gizmos.color = new Color(0, 1, 0, 1);   // �F�̎w��
                Gizmos.DrawSphere(CutPointTest[i], 0.05f);  // ���̕\��
            }

            // �I�_�̃J�b�g�|�C���g�M�Y��
            Gizmos.color = new Color(1, 1, 0, 1);   // �F�̎w��
            Gizmos.DrawSphere(CutPointTest[CutPointTest.Count - 1], 0.05f);  // ���̕\��
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

    // 2D�x�N�g���̊O��
    float Vec2Cross(Vector2 v1, Vector2 v2)
    {
        return v1.x * v2.y - v1.y * v2.x;
    }


    // �����̏Փ�
    bool ColSegments(
       Segment seg1,          // ����1
       Segment seg2,          // ����2
       float outT1 ,       // ����1�̓�����i�o�́j
       float outT2 ,       // ����2�̓�����i�o��
       Vector2 outPos  // ��_�i�o�́j
    )
    {

        Vector2 v = seg2.s - seg1.s;
        float Crs_v1_v2 = Vec2Cross(seg1.v, seg2.v);
        if (Crs_v1_v2 == 0.0f)
        {
            // ���s���
            return false;
        }

        float Crs_v_v1 = Vec2Cross(v, seg1.v);
        float Crs_v_v2 = Vec2Cross(v, seg2.v);

        float t1 = Crs_v_v2 / Crs_v1_v2;
        float t2 = Crs_v_v1 / Crs_v1_v2;

        if (outT1 == 0)
            outT1 = Crs_v_v2 / Crs_v1_v2;
        if (outT2 == 0)
            outT2 = Crs_v_v1 / Crs_v1_v2;

        const float eps = 0.00001f;
        if (t1 + eps < 0 || t1 - eps > 1 || t2 + eps < 0 || t2 - eps > 1)
        {
            // �������Ă��Ȃ�
            return false;
        }

        if (outPos == new Vector2(0,0))
        {
            outPos = seg1.s + seg1.v * t1;
            p = seg1.s + seg1.v * t1;
        }
        p = seg1.s + seg1.v * t1;
        Debug.Log("�������Ă�");
        return true;
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