using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutPoint2 : MonoBehaviour
{
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

    // �f�o�b�O�p
    private bool triggerFlg = false;    // �f�o�b�N�p�g���K�[�t���O
    public bool AddPointOnOff = true;   // �|�C���g��ǉ����邩�ǂ��� 

    public GameObject m_CubeBase;
    public List<GameObject> objList;

    // �����p�ϐ�
    public Vector2 v;
    public Vector2 v1;
    public Vector2 v2;
    public Vector2 p;
    public float t1;
    public float t2;


    private int count = -1;

    RaycastHit hit; // �����������̏����i�[����ϐ�
    RaycastHit hit2; // �����������̏����i�[����ϐ�
    GameObject hitGameObject;// �؂肽�����̕ۑ��p

    private bool test = false;      // �e�X�g�p�t���O

    public bool bCut = false;  // �؂�n�߂���
    public bool bStartP = false;   // �n�_���ӂ̏�ɂ��邩
    public bool bPurposeObj = false;

    [SerializeField] [Tooltip("")] private ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        objList = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
      
    }
    public bool AddCPPoint()
    {

        // ���C�L���X�g���Đ��m�Ȓ��_���쐬
        Ray ray = new Ray(gameObject.transform.position, -gameObject.transform.up); // �n�T�~�̏�̐n�̂����_����^���Ɍ����Ẵ��C


        // ���C�L���X�g���������Ƃ� 
        if (Physics.Raycast(ray, out hit,5,5))
        {
            // �q�b�g�������̂��ړI�̕���������
            if (hit.collider.gameObject.name == "Plane" || hit.collider.gameObject.name == "DivisionPlane") bPurposeObj = true;
            else bPurposeObj = false;

            // �e�X�g�p�̃|�C���g������Ƃ�
            if (CutPointTest.Count > 0)
            {
                // �q�b�g�������W�ƍŌ�Ɋi�[�������W���Ⴄ�Ƃ����X�g�Ɋi�[������
                if (hit.point != CutPointTest[CutPointTest.Count - 1])
                {
                    // �J�b�g�|�C���g�̒ǉ�
                    if (hit.collider.gameObject.name == "Plane" || hit.collider.gameObject.name == "MoveArea")
                    {
                        CutPointTest.Add(hit.point);    // �q�b�g�������W���i�[
                        Debug.Log("�J�b�g�|�C���g�̒ǉ�");
                    }
                  

                   // �_���[�W���C���̈ʒu�ݒ�A����
                    Vector3 pos = new Vector3();
                    pos = hit.point;
                    GameObject gameObj;
                    gameObj = m_CubeBase;
                    gameObj.transform.position = pos;
                    gameObj.transform.rotation = this.transform.rotation;
                   
                    // m_CubeBase �����ɂ��ĐV����m_CubeBase���쐬
                    objList.Add(Instantiate(m_CubeBase, pos, this.transform.rotation));

                    // ���_���L���鏈��
                    if (CutPointTest.Count >= 3 && hit.collider.gameObject.name == "Plane")
                    {
                        hit.collider.gameObject.GetComponent<MeshDivision2>().DiviosionMeshMiddle(CutPointTest);
                        hitGameObject = hit.collider.gameObject;
                    }

                    // �G�t�F�N�g�̐���
                    if (hit.collider.gameObject.name == "Plane" || hit.collider.gameObject.name == "DivisionPlane")
                    {
                        ParticleSystem newParticle = Instantiate(particle);
                        newParticle.transform.position = this.transform.position;
                        newParticle.Play();
                        Destroy(newParticle.gameObject, 2.0f);
                    }

                    // �Ȃɂ���H
                    test = true;

                    // �q�b�g���������؂肽�����̂ƈႤ�Ƃ��͈�O�̃|�C���g���폜�������B�Ȃ�Ȃ�S���폜���Ă������̂��H          
                    if (hit.collider.gameObject.name != "Plane" && hit.collider.gameObject.name != "DivisionPlane")
                    {
                        // �J�b�g�|�C���g��2�ȉ��̎�
                        if (CutPointTest.Count <= 2)
                        {
                            // �J�b�g�|�C���g�̍폜
                            CutPointTest.RemoveAt(0);

                            // �_���[�W���C���̍폜
                            for (int g = 0; g < objList.Count; g++)
                            {
                                objList[g].GetComponent<DamageLine>().Destroy();
                            }
                            objList.Clear();
                        }

                        test = false;
                    }

                    // �q�b�g�������b�V���̃|���S����
                    //Debug.Log("���_��" + hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices.Length);
                    //Debug.Log("�|���S����" + hit.triangleIndex);
                }
            }
            else //�e�X�g�p�̃|�C���g���Ȃ��Ƃ�
            {

                CutPointTest.Add(hit.point);    // �q�b�g�������W���i�[
            }

        }

        //if (AddPointOnOff)



        // �J�b�g�|�C���g�̎n�_�ƏI�_�����|���S���̕ԏ�ɂ�������(�J�b�g�|�C���g�������邽�тɏ���)
        if (CutPointTest.Count >= 2)
        {
            // ��������񂾂��ɂ��鏈��
            if (CutPointTest.Count != count)
            {
                // �n�_���|���S���̕ӂɂ�������
                if (!bStartP)
                {
                    // �J�b�g�|�C���g�̕ӂƃ|���S���Ƃ̕ӂ̌�_��ۑ�����ϐ�
                    var intersectionMemory = new List<Vector2>();

                    // �����������b�V���̃|���S��������
                    for (int i = 0; i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length; i += 3)
                    {

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
                                // �������Ă��Ȃ��Ƃ�
                            }
                            else
                            {
                                // �������Ă���Ƃ� 
                                intersectionMemory.Add(p);
                              
                            }
                        }


                    }
                    // �L�����ꂽ��_������Ƃ�
                    if (intersectionMemory.Count > 1)
                    {
                        p = intersectionMemory[0];
                        // �L�����ꂽ��_�̐��������[�v
                        for (int k = 0; k < intersectionMemory.Count; k++)
                        {
                            // ��_�̔�r(�J�b�g�|�C���g�̎n�_�Ƃ̋߂����r�A���߂�p���D��)
                            if (Vector2.Distance(new Vector2(CutPointTest[0].x, CutPointTest[0].z), new Vector2(p.x + transform.position.x, p.y + transform.position.z)) > Vector2.Distance(new Vector2(CutPointTest[0].x, CutPointTest[0].z), new Vector2(intersectionMemory[k].x + transform.position.x, intersectionMemory[k].y + transform.position.z))) continue;

                            // ��_�̑��
                            p = intersectionMemory[k];

                            ////--- ��_�̕␳ ---
                            //var vtx_s = p;
                            //var vtx_v = new Vector2(CutPointTest[1].x, CutPointTest[1].z);
                            //var edg = vtx_v - vtx_s;
                            //p += edg * 0.01f;
                        }

                        // �J�b�g�|�C���g�̎n�_��ύX
                        //CutPointTest[0] = new Vector3(p.x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[0].y + hit.collider.gameObject.transform.position.y, p.y);

                        // ���b�V���𕪊�
                        hit.collider.gameObject.GetComponent<MeshDivision2>().DivisionMesh(CutPointTest);
                        hitGameObject = hit.collider.gameObject;
                        bStartP = true; // �؂�n�߃Z�b�g
                        return true;
                    }
                    else if (intersectionMemory.Count == 1)
                    {
                        //--- ��_�̕␳ ---
                        p = intersectionMemory[0];
                        var vtx_s = p;
                        var vtx_v = new Vector2(CutPointTest[1].x, CutPointTest[1].z);
                        var edg = vtx_v - vtx_s;
                        p += edg * 0.01f;

                        // �J�b�g�|�C���g�̎n�_��ύX
                        CutPointTest[0] = new Vector3(p.x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[0].y + hit.collider.gameObject.transform.position.y, p.y);

                        // ���b�V���𕪊�
                        hit.collider.gameObject.GetComponent<MeshDivision2>().DivisionMesh(CutPointTest);
                        hitGameObject = hit.collider.gameObject;
                        bStartP = true; // �؂�n�߃Z�b�g
                        return true;
                    }
                }



                // �؂肽�����̂��痣�ꂽ��
                if (bStartP)
                    if (hit.collider.gameObject.name != "Plane" && hit.collider.gameObject.name != "DivisionPlane")
                    {
                        // �J�b�g�|�C���g�̕ӂƃ|���S���Ƃ̕ӂ̌�_��ۑ�����ϐ�
                        var intersectionMemory = new List<Vector2>();

                        // �q�b�g�����I�u�W�F�N�g�̃|���S���̐��������[�v
                        for (int i = 0; i < hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length; i += 3)
                        {

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

                                    // ��_�̕ۑ�
                                    intersectionMemory.Add(p);


                                }
                            }

                        }

                        // �L�����ꂽ��_������Ƃ�
                        if (intersectionMemory.Count > 0)
                        {
                            // �L�����ꂽ��_�̐��������[�v
                            for (int k = 0; k < intersectionMemory.Count; k++)
                            {
                                // ��_�̔�r(�J�b�g�|�C���g�̏I�_�Ƃ̋߂����r�A���߂�p���D��)
                                if (Vector2.Distance(new Vector2(CutPointTest[CutPointTest.Count - 1].x, CutPointTest[CutPointTest.Count - 1].z), new Vector2(p.x, p.y)) < Vector2.Distance(new Vector2(CutPointTest[CutPointTest.Count - 1].x, CutPointTest[CutPointTest.Count - 1].z), new Vector2(intersectionMemory[k].x, intersectionMemory[k].y))) continue;

                                // ��_�̑��
                                p = intersectionMemory[k];
                            }

                            // �I�_�̃Z�b�g                        
                            CutPointTest[CutPointTest.Count - 1] = new Vector3(p.x, hitGameObject.transform.position.y, p.y);

                            // �|���S������
                            hitGameObject.gameObject.GetComponent<MeshDivision2>().DivisionMeshTwice(CutPointTest);

                            

                            // ��O�̃J�b�g�|�C���g���폜
                            if (CutPoint.Count > 0)
                            {
                                CutPoint.Clear();
                            }

                            // �J�b�g�|�C���g�̕ۑ�                               
                            for (int k = 0; k < CutPointTest.Count; k++)
                            {
                                CutPoint.Add(CutPointTest[k]);
                            }

                            // ���̃J�b�g�|�C���g�̍폜
                            CutPointTest.RemoveRange(0, CutPointTest.Count);

                            // �_���[�W���C���̍폜
                            for(int g = 0;g < objList.Count;g++)
                            {
                                objList[g].GetComponent<DamageLine>().Destroy();
                            }
                            objList.Clear();

                            // �J�b�g�I���t���OOFF
                            bStartP = false;
                            AddCPPoint();
                            return true;    // �����ŏI��
                        }

                    }

            }



            // ��񂾂��ɂ��邽�߂̏���
            count = CutPointTest.Count;

        }


        // �Ȃ񂩂悭�킩���
        return true;
    }

    // �M�Y���̕\��
    private void OnDrawGizmos()
    {
        // �e�X�g�p�̃|�C���g��\��������
        if (CutPointTest.Count > 0)
        {
            // �n�_�̃J�b�g�|�C���g�M�Y��
            Gizmos.color = new Color(1, 1, 0, 1);   // �F�̎w��
            Gizmos.DrawSphere(CutPointTest[0], 0.01f);  // ���̕\��

            // �r���̃J�b�g�|�C���g�M�Y��
            for (int i = 1; i < CutPointTest.Count; i++)
            {
                Gizmos.color = new Color(0, 1, 0, 1);   // �F�̎w��
                Gizmos.DrawSphere(CutPointTest[i], 0.01f);  // ���̕\��
            }

            // �I�_�̃J�b�g�|�C���g�M�Y��
            Gizmos.color = new Color(1, 1, 0, 1);   // �F�̎w��
            Gizmos.DrawSphere(CutPointTest[CutPointTest.Count - 1], 0.01f);  // ���̕\��
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



        // ��O�̃J�b�g�|�C���g�̕\��
        if (CutPoint.Count > 0)
        {
            // �n�_�̃J�b�g�|�C���g�M�Y��
            Gizmos.color = new Color(1, 1, 0, 1);   // �F�̎w��
            Gizmos.DrawSphere(CutPoint[0], 0.01f);  // ���̕\��

            // �r���̃J�b�g�|�C���g�M�Y��
            for (int i = 1; i < CutPoint.Count; i++)
            {
                Gizmos.color = new Color(0, 1, 0, 1);   // �F�̎w��
                Gizmos.DrawSphere(CutPoint[i], 0.01f);  // ���̕\��
            }

            // �I�_�̃J�b�g�|�C���g�M�Y��
            Gizmos.color = new Color(1, 1, 0, 1);   // �F�̎w��
            Gizmos.DrawSphere(CutPoint[CutPoint.Count - 1], 0.01f);  // ���̕\��
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


}
