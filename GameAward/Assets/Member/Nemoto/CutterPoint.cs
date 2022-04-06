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
    //--- �ϐ��錾 ---

    // �J�b�g�|�C���g�p�ϐ�
    public class CutPointList
    {
        public List<Vector3> CutPoint;      // �؂���|�C���g�p���X�g

    }

    public List<Vector3> m_vCotPoint;   // �n�T�~�̋O�՗p���X�g
    public List<Vector3> CutPointTest;  // �n�T�~�̋O�՗p���X�g(�e�X�g)
    public List<Vector3> CutPoint;      // �J�b�g�|�C���g�p���X�g
    public List<CutPointList> CutPointLst;  // �J�b�g�|�C���g�̃��X�g��ۑ����邽�߂̃��X�g

    private int cpCount = 0;   // cp��CutPoint�̗��B�J�b�g�|�C���g�����ڂ����i�[����ϐ�


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
     GameObject hitGameObject;// �؂肽�����̕ۑ��p

    private bool test = false;      // �e�X�g�p�t���O

    public bool bCut = false;  // �؂�n�߂���
    public bool bStartP = false;   // �n�_���ӂ̏�ɂ��邩
    public bool bPurposeObj = false;

    [SerializeField] [Tooltip("")] private ParticleSystem particle;

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

      
        // ���C�L���X�g���������Ƃ� 
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.name == "Plane" || hit.collider.gameObject.name == "DivisionPlane") bPurposeObj = true;
            else bPurposeObj = false;

            // �e�X�g�p�̃|�C���g������Ƃ�
            if (CutPointTest.Count > 0)
            {
                // �q�b�g�������W�ƍŌ�Ɋi�[�������W���Ⴄ�Ƃ����X�g�Ɋi�[������
                if (hit.point != CutPointTest[CutPointTest.Count - 1])
                {
                    CutPointTest.Add(hit.point);    // �q�b�g�������W���i�[
                    if(hit.collider.gameObject.name == "Plane" || hit.collider.gameObject.name == "DivisionPlane")
                    {
                        ParticleSystem newParticle = Instantiate(particle);
                        newParticle.transform.position = this.transform.position;
                        newParticle.Play();
                        Destroy(newParticle.gameObject, 2.0f);
                    }
                   
                    test = true;

                    // �q�b�g���������؂肽�����̂ƈႤ�Ƃ��͈�O�̃|�C���g���폜�������B�Ȃ�Ȃ�S���폜���Ă������̂��H          
                    if (hit.collider.gameObject.name != "Plane" && hit.collider.gameObject.name != "DivisionPlane")
                    {
                        // �J�b�g�|�C���g��1�ȉ��̎�
                        if (CutPointTest.Count <= 2)
                        {
                            // �J�b�g�|�C���g�̍폜
                            CutPointTest.Clear();
                        }

                        test = false;
                    }
                    
                    // �q�b�g�������b�V���̃|���S����
                    Debug.Log("���_��" + hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices.Length);
                    //Debug.Log("�|���S����" + hit.triangleIndex);
                }
            }
            else //�e�X�g�p�̃|�C���g���Ȃ��Ƃ�
            {
                CutPointTest.Add(hit.point);    // �q�b�g�������W���i�[
            }
         
        }

        // �J�b�g�|�C���g�̎n�_�ƏI�_�����|���S���̕ԏ�ɂ�������(�J�b�g�|�C���g�������邽�тɏ���)
        if (CutPointTest.Count >= 2)
        {
            // ��������񂾂��ɂ��鏈��
            if (CutPointTest.Count == count) return;

            // �����������b�V���̃|���S��������
            for (int i = 0; i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length; i += 3)
            {

                // �n�_���|���S���̕ӂɂ�������
                if (!bStartP)
                    for (int j = 0; j < 3; j++)
                    {

                        // �؂肽�����̗p�̕ϐ�
                        int hitIdx_s = hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + j];  // �n�_
                        int hitIdx_v = hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + ((j + 1) % 3)];  // �I�_

                        Vector2 hitVtx_s = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].x + hit.collider.gameObject.transform.position.x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].z + hit.collider.gameObject.transform.position.z);    // �n�_
                        Vector2 hitVtx_v = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].x + hit.collider.gameObject.transform.position.x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].z + hit.collider.gameObject.transform.position.z);    // �I�_


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
                            //Debug.Log("�������Ă�");
                            //Debug.Log("�����������W:" + p);
                            //Debug.Log("����������:" + (double)t1 + ":" + (double)t2);
                            CutPointTest[0] = new Vector3(p.x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].y + hit.collider.gameObject.transform.position.y, p.y);


                            // ���b�V���𕪊�
                            hitGameObject = hit.collider.gameObject;
                            bStartP = true; // �؂�n�߃Z�b�g
                            
                        }
                    }                          
            }

            // ���̎O�p�`�|���S�����痣�ꂽ�Ƃ��Ƀ|���S���ƃJ�b�g�|�C���g�̌�_����鏈��
            /*
            if (hit.collider.gameObject.name == "Plane" || hit.collider.gameObject.name == "DivisionPlane")
            for (int i = 0;i < hitGameObject.GetComponent<MeshFilter>().mesh.triangles.Length; i += 3)
            {
                if (hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length > 0)
                    for (int j = 0; j < 3; j++)
                    {

                        // �؂肽�����̗p�̕ϐ�
                        int hitIdx_s = hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + j];  // �n�_
                        int hitIdx_v = hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + ((j + 1) % 3)];  // �I�_

                        Vector2 hitVtx_s = new Vector2(hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].x + hitGameObject.gameObject.transform.position.x, hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].z + hitGameObject.gameObject.transform.position.z);    // �n�_
                        Vector2 hitVtx_v = new Vector2(hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].x + hitGameObject.gameObject.transform.position.x, hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].z + hitGameObject.gameObject.transform.position.z);    // �I�_

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
                        else // �����Ő؂�I���
                        {
                            //Debug.Log("�I�_�Z�b�g");
                            //Debug.Log("�I�_�̍��W:" + p);

                            // �I�_�̃Z�b�g                        
                            CutPointTest[cp_s] = new Vector3(p.x, hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].y + hitGameObject.gameObject.transform.position.y, p.y);

                           
                        }
                    }

            }
            */

            // �؂肽�����̂��痣�ꂽ��
            if(bStartP)
            if (hit.collider.gameObject.name != "Plane" && hit.collider.gameObject.name != "DivisionPlane")
                for (int i = 0; i < hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length; i += 3)
                {
                    if (hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length > 0)
                        for (int j = 0; j < 3; j++)
                        {

                            // �؂肽�����̗p�̕ϐ�
                            int hitIdx_s = hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + j];  // �n�_
                            int hitIdx_v = hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + ((j + 1) % 3)];  // �I�_

                            Vector2 hitVtx_s = new Vector2(hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].x + hitGameObject.gameObject.transform.position.x, hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].z + hitGameObject.gameObject.transform.position.z);    // �n�_
                            Vector2 hitVtx_v = new Vector2(hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].x + hitGameObject.gameObject.transform.position.x, hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].z + hitGameObject.gameObject.transform.position.z);    // �I�_

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
                            else // �����Ő؂�I���
                            {
                                //Debug.Log("�I�_�Z�b�g");
                                //Debug.Log("�I�_�̍��W:" + p);

                                // �I�_�̃Z�b�g                        
                                CutPointTest[cp_v] = new Vector3(p.x, hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].y + hitGameObject.gameObject.transform.position.y, p.y);

                                // ��O�̃J�b�g�|�C���g���폜
                                if(CutPoint.Count > 0)
                                {
                                    CutPoint.Clear();
                                }

                                // �J�b�g�|�C���g�̕ۑ�                               
                                for (int k = 0; k <CutPointTest.Count;k++)
                                {
                                    CutPoint.Add(CutPointTest[k]);

                                }

                                    // ���b�V���̕���
                                    for (int l = 0; l < CutPoint.Count; l++)
                                    {
                                        if (GameObject.Find("DivisionPlane" + l)) hitGameObject = GameObject.Find("DivisionPlane" + l);
                                        hitGameObject.gameObject.GetComponent<MeshDivision>().DivisionMesh(CutPoint, l);

                                        hitGameObject = GameObject.Find("DivisionPlane" + l);

                                    }
                                    //hitGameObject.gameObject.GetComponent<MeshDivision>().DivisionMesh(CutPoint, 0);
                                    //hitGameObject = GameObject.Find("DivisionPlane0");
                                    //hitGameObject.gameObject.GetComponent<MeshDivision>().DivisionMesh(CutPoint, 1);
                                    //hitGameObject = GameObject.Find("DivisionPlane1");
                                    //// ���b�V���̃J�b�g
                                    hitGameObject.gameObject.GetComponent<MeshDivision>().CutMesh();

                                // ���̃J�b�g�|�C���g�̍폜
                                    CutPointTest.RemoveRange(0, CutPointTest.Count-1);
                                    
                                bStartP = false;
                                return;
                            }
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


        //if (test)
        //{
        //    if (hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices.Length > 0)
        //    {
        //        for (int i = 0; i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices.Length; i++)
        //        {
        //            Gizmos.color = new Color(25, 0, 0, 1);   // �F�̎w��
        //            Gizmos.DrawSphere(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i] + hit.collider.gameObject.transform.position, 0.05f);  // ���̕\��
        //        }

        //        for (int i = 0; i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length; i += 3)
        //        {
        //            for (int j = 0; j < 3; j++)
        //            {
        //                int idx1 = hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + j];
        //                int idx2 = hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + ((j + 1) % 3)];
        //                Gizmos.color = new Color(25, 0, 0, 1);   // �F�̎w��
        //                Gizmos.DrawLine(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[idx1] + hit.collider.gameObject.transform.position, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[idx2] + hit.collider.gameObject.transform.position);  // ���̕\��
        //            }
        //        }



        //    }


        //}

        //if (CutPointTest.Count >= 2)
        //    if (hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices.Length > 0)
        //    {
        //        for (int i = 0; i < hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices.Length; i++)
        //        {
        //            Gizmos.color = new Color(25, 0, 0, 1);   // �F�̎w��
        //            Gizmos.DrawSphere(hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[i] + hitGameObject.gameObject.transform.position, 0.05f);  // ���̕\��
        //        }

        //        for (int i = 0; i < hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length; i += 3)
        //        {
        //            for (int j = 0; j < 3; j++)
        //            {
        //                int idx1 = hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + j];
        //                int idx2 = hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + ((j + 1) % 3)];
        //                Gizmos.color = new Color(25, 0, 0, 1);   // �F�̎w��
        //                Gizmos.DrawLine(hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[idx1] + hitGameObject.gameObject.transform.position, hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[idx2] + hitGameObject.gameObject.transform.position);  // ���̕\��
        //            }
        //        }
        //    }

        // ��O�̃J�b�g�|�C���g�̕\��
        if (CutPoint.Count > 0)
        {
            // �n�_�̃J�b�g�|�C���g�M�Y��
            Gizmos.color = new Color(1, 1, 0, 1);   // �F�̎w��
            Gizmos.DrawSphere(CutPoint[0], 0.05f);  // ���̕\��

            // �r���̃J�b�g�|�C���g�M�Y��
            for (int i = 1; i < CutPoint.Count; i++)
            {
                Gizmos.color = new Color(0, 1, 0, 1);   // �F�̎w��
                Gizmos.DrawSphere(CutPoint[i], 0.05f);  // ���̕\��
            }

            // �I�_�̃J�b�g�|�C���g�M�Y��
            Gizmos.color = new Color(1, 1, 0, 1);   // �F�̎w��
            Gizmos.DrawSphere(CutPoint[CutPoint.Count - 1], 0.05f);  // ���̕\��
        }
        //  ��O�̃J�b�g�|�C���g���Ȃ�����\��������
        if (CutPoint.Count >= 2)
        {
            for (int i = 1; i < CutPoint.Count; i++)
            {
                Gizmos.color = new Color(0, 0.5f, 0, 1);    // �F�̎w��                
                Gizmos.DrawLine(CutPoint[i - 1], CutPoint[i]);  // ���̕\��
            }
        }
    }

    // 2D�x�N�g���̊O��
    float Vec2Cross(Vector2 v1, Vector2 v2)
    {
        return v1.x * v2.y - v1.y * v2.x;
    }


    
}



