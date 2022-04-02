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
    

    private int count = -1;

    RaycastHit hit; // �����������̏����i�[����ϐ�
    RaycastHit hit_p; // �؂肽�����̕ۑ��p

    private bool test = false;      // �e�X�g�p�t���O

    public bool bCut = false;  // �؂�n�߂���
    private bool bStartP = false;   // �n�_���ӂ̏�ɂ��邩


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
                if (!triggerFlg)
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
        if (Physics.Raycast(ray, out hit))
        {
            // �e�X�g�p�̃|�C���g������Ƃ�
            if (CutPointTest.Count > 0)
            {
                // �q�b�g�������W�ƍŌ�Ɋi�[�������W���Ⴄ�Ƃ����X�g�Ɋi�[������
                if (hit.point != CutPointTest[CutPointTest.Count - 1])
                {
                    CutPointTest.Add(hit.point);    // �q�b�g�������W���i�[

                    test = true;

                    // �q�b�g���������؂肽�����̂ƈႤ�Ƃ��͈�O�̃|�C���g���폜�������B�Ȃ�Ȃ�S���폜���Ă������̂��H          
                    if (hit.collider.gameObject.name != "Plane")
                    {
                        // �J�b�g�|�C���g��1�ȉ��̎�
                        if(CutPointTest.Count <= 2)
                        {
                            // �J�b�g�|�C���g�̍폜
                            //CutPointTest.RemoveAt(CutPointTest.Count - 1);
                            CutPointTest.Clear();
                        }
                      
                        test = false;
                    }

                    // �q�b�g�������̂��؂肽�����̂̎�
                    if (hit.collider.gameObject.name == "Plane" )
                    {
                        hit_p = hit;
                        bCut = true;    // �؂�n��
                    }

                    // �q�b�g�������b�V���̃|���S����
                    Debug.Log("�O�p�`�̃C���f�b�N�X��" + hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length);

                   

                    //Debug.Log("�|���S����" + hit.triangleIndex);
                }
            }
            else //�e�X�g�p�̃|�C���g���Ȃ��Ƃ�
            {
                CutPointTest.Add(hit.point);    // �q�b�g�������W���i�[


            }

        }
        else
        {
           
        }


        // �J�b�g�|�C���g�̎n�_�ƏI�_�����|���S���̕ԏ�ɂ�������(�J�b�g�|�C���g�������邽�тɏ���)
        if (CutPointTest.Count >= 2)
        {
            // ��������񂾂��ɂ��鏈��
            if (CutPointTest.Count == count) return;

            // �����������b�V���̃|���S��������
            for (int i = 0; i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length ; i+=3)
            {
                           
                // �n�_���|���S���̕ӂɂ�������
                if(!bStartP)
                for (int j = 0 ;j < 3 ;j++)
                {
                   
                    // �؂肽�����̗p�̕ϐ�
                    int hitIdx_s = hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i+j];  // �n�_
                    int hitIdx_v = hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i+((j + 1) % 3)];  // �I�_
                   
                    Vector2 hitVtx_s = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].x + hit.collider.gameObject.transform.position.x  , hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].z + hit.collider.gameObject.transform.position.z);    // �n�_
                    Vector2 hitVtx_v = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].x + hit.collider.gameObject.transform.position.x  , hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].z + hit.collider.gameObject.transform.position.z);    // �I�_


                    // �����Ɛ����̎n�_���Ȃ����x�N�g��
                    v = new Vector2(hitVtx_s.x - CutPointTest[0].x, hitVtx_s.y - CutPointTest[0].z);

                    // ����
                    v1 = new Vector2(CutPointTest[1].x - CutPointTest[0].x, CutPointTest[1].z - CutPointTest[0].z);
                    v2 = new Vector2(hitVtx_v.x - hitVtx_s.x, hitVtx_v.y - hitVtx_s.y);

                    // �����̎n�_�����_�̃x�N�g��
                    t1 = (v.x * v2.y - v2.x * v.y) / (v1.x * v2.y - v2.x * v1.y);
                    t2 = (v.x * v1.y - v1.x * v.y) / (v1.x * v2.y - v2.x * v1.y);

                    // ��_
                    p = new Vector2(hitVtx_s.x, hitVtx_s.y) + new Vector2(v2.x * t2, v2.y * t2);

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
                        CutPointTest[0] = new Vector3(p.x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].y + hit.collider.gameObject.transform.position.y, p.y);
                        bStartP = true; // �؂�n�߃Z�b�g
                    }
                }

                // �I�_���|���S���̕ӂɂ�������
                if(bCut)
                {
                    for (int j = 0; j < 3; j++)
                    {

                        // �؂肽�����̗p�̕ϐ�
                        int hitIdx_s = hit_p.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + j];  // �n�_
                        int hitIdx_v = hit_p.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + ((j + 1) % 3)];  // �I�_

                        Vector2 hitVtx_s = new Vector2(hit_p.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].x + hit_p.collider.gameObject.transform.position.x, hit_p.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].z + hit_p.collider.gameObject.transform.position.z);    // �n�_
                        Vector2 hitVtx_v = new Vector2(hit_p.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].x + hit_p.collider.gameObject.transform.position.x, hit_p.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].z + hit_p.collider.gameObject.transform.position.z);    // �I�_

                        // �J�b�g�|�C���g�p�ϐ�
                        int cp_s = CutPointTest.Count - 2;   // �n�_
                        int cp_v = CutPointTest.Count - 1;   // �I�_

                        // �����Ɛ����̎n�_���Ȃ����x�N�g��
                        v = new Vector2(hitVtx_s.x - CutPointTest[cp_s].x, hitVtx_s.y - CutPointTest[cp_s].z);

                        // ����
                        v1 = new Vector2(CutPointTest[cp_v].x - CutPointTest[cp_s].x, CutPointTest[cp_v].z - CutPointTest[cp_s].z);
                        v2 = new Vector2(hitVtx_v.x - hitVtx_s.x, hitVtx_v.y - hitVtx_s.y);

                        // �����̎n�_�����_�̃x�N�g��
                        t1 = (v.x * v2.y - v2.x * v.y) / (v1.x * v2.y - v2.x * v1.y);
                        t2 = (v.x * v1.y - v1.x * v.y) / (v1.x * v2.y - v2.x * v1.y);

                        // ��_
                        p = new Vector2(hitVtx_s.x, hitVtx_s.y) + new Vector2(v2.x * t2, v2.y * t2);

                        // �����Ɛ�����������Ă��邩
                        const float eps = 0.00001f;
                        if (t1 + eps < 0 || t1 - eps > 1 || t2 + eps < 0 || t2 - eps > 1)
                        {
                            // Debug.Log("�������ĂȂ�");
                        }
                        else
                        {
                            Debug.Log("�I�_�Z�b�g");
                            Debug.Log("�I�_�̍��W:" + p);
                           
                            CutPointTest[cp_s] = new Vector3(p.x, hit_p.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].y + hit_p.collider.gameObject.transform.position.y, p.y);
                        }
                    }

                    bCut = false;   // �J�b�g�I��
                }
               

            }

           

            // ��񂾂��ɂ��邽�߂̏���
            count = CutPointTest.Count;

        }

        


    }

    private void OnDrawGizmos()
    {
        // �e�X�g�p�̃|�C���g��\��������
        if (CutPointTest.Count > 0)
        {
            // �n�_�̃J�b�g�|�C���g�M�Y��
            Gizmos.color = new Color(1, 1, 0, 1);   // �F�̎w��
            Gizmos.DrawSphere(CutPointTest[0], 0.05f);  // ���̕\��

            // �r���̃J�b�g�|�C���g�M�Y��
            for (int i = 1; i < CutPointTest.Count; i++)
            {
                Gizmos.color = new Color(0, 1, 0, 1);   // �F�̎w��
                Gizmos.DrawSphere(CutPointTest[i], 0.05f);  // ���̕\��
            }

            // �I�_�̃J�b�g�|�C���g�M�Y��
            Gizmos.color = new Color(1, 1, 0, 1);   // �F�̎w��
            Gizmos.DrawSphere(CutPointTest[CutPointTest.Count - 1], 0.05f);  // ���̕\��
        }

        // �e�X�g�p�̃|�C���g���Ȃ�����\��������
        if (CutPointTest.Count >= 2)
        {
            for (int i = 1; i < CutPointTest.Count; i++)
            {
                Gizmos.color = new Color(0, 0.5f, 0, 1);    // �F�̎w��                
                Gizmos.DrawLine(CutPointTest[i - 1], CutPointTest[i]);  // ���̕\��
            }
        }

        
        if(test)
        {
            if(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices.Length > 0)
            {
                for(int i = 0;i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices.Length;i++)
                {
                    Gizmos.color = new Color(25, 0, 0, 1);   // �F�̎w��
                    Gizmos.DrawSphere(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i] + hit.collider.gameObject.transform.position, 0.05f);  // ���̕\��
                }

                for(int i = 0;i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length;i+=3)
                {
                    for(int j = 0;j < 3;j++)
                    {
                        int idx1 = hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + j];
                        int idx2 = hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + ((j + 1)%3)];
                        Gizmos.color = new Color(25, 0, 0, 1);   // �F�̎w��
                        Gizmos.DrawLine(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[idx1] + hit.collider.gameObject.transform.position, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[idx2] + hit.collider.gameObject.transform.position);  // ���̕\��
                    }
                }

                //for (int i = 0; i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length; i += 3)
                //{

                //    Gizmos.color = Random.ColorHSV();   // �F�̎w��
                //    Gizmos.DrawLine(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i] + hit.collider.gameObject.transform.position, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 1] + hit.collider.gameObject.transform.position);  // ���̕\��
                //    Gizmos.DrawLine(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i] + hit.collider.gameObject.transform.position, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 2] + hit.collider.gameObject.transform.position);  // ���̕\��
                //    Gizmos.DrawLine(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 1] + hit.collider.gameObject.transform.position, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 2] + hit.collider.gameObject.transform.position);  // ���̕\��
                //}
                //{
                //    int i = 0;
                //    Gizmos.color = Random.ColorHSV();   // �F�̎w��
                //    Gizmos.DrawLine(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i] + hit.collider.gameObject.transform.position, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 11] + hit.collider.gameObject.transform.position);  // ���̕\��
                //    Gizmos.DrawLine(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i] + hit.collider.gameObject.transform.position, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 12] + hit.collider.gameObject.transform.position);  // ���̕\��
                //    Gizmos.DrawLine(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 11] + hit.collider.gameObject.transform.position, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 12] + hit.collider.gameObject.transform.position);  // ���̕\��

                //}


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
       float outT1,       // ����1�̓�����i�o�́j
       float outT2,       // ����2�̓�����i�o��
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

        if (outPos == new Vector2(0, 0))
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
