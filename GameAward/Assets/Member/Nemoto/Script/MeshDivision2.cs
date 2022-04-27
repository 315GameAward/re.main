//======================================================
//
//        MeshDivision2.cs
//        ���b�V���𕪊����鏈��
//
//------------------------------------------------------
//      �쐬��:���{���V��
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDivision2 : MonoBehaviour
{
    // ���b�V���̕ϐ�
    private MeshFilter attachedMeshFilter;
    private Mesh attachedMesh;
    //static Vector3 vBgnP;  // �J�b�g�|�C���g�̎n�_
    static List<Vector3> vtx = new List<Vector3>();

    private List<int> idxMemory = new List<int>();    // �O�p�`�C���f�b�N�X�̋L���p�ϐ�


    public class Triangle
    {
        public int[] idx = new int[3];

    };

    public Triangle tri;
    public List<Triangle> trianglesList;


    // Start is called before the first frame update
    void Start()
    {
        // ���b�V���̃A�^�b�`
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;
        idxMemory.Clear();
        for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
        {
            tri = new Triangle();
            trianglesList = new List<Triangle>();
            tri.idx[0] = attachedMesh.triangles[i];
            tri.idx[1] = attachedMesh.triangles[i + 1];
            tri.idx[2] = attachedMesh.triangles[i + 2];
            trianglesList.Add(tri);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    // ���b�V���̕���
    public void DivisionMesh(List<Vector3> cutPoint)
    {
        // ���b�V���̃A�^�b�`
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;



        // �ϐ�
        Vector3 p0, p1, p2;    // ���b�V���̃|���S���̒��_
        var uvs1 = new List<Vector2>(); // �e�N�X�`��
        //var uvs2 = new List<Vector2>();
        var vertices1 = new List<Vector3>();   // ���_
        //var vertices2 = new List<DVector3>();
        var triangles1 = new List<int>();       // �O�p�`�C���f�b�N�X
        //var triangles2 = new List<int>();
        var normals1 = new List<Vector3>();     // �@��
        //var normals2 = new List<Vector3>();
        Vector3 edge = new Vector3();
        var crossVertices = new List<Vector3>();

        //�J�b�g�������I�u�W�F�N�g�̃��b�V�����g���C�A���O�����Ƃɏ���
        for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
        {
            //���b�V����3�̒��_���擾
            p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);//+ Vector3.one * 0.0001f;
            p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);//+ Vector3.one * 0.0001f;
            p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]); //+ Vector3.one * 0.0001f;

            // �J�b�g�|�C���g�̎n�_���|���S���̕ӂ̏�ɂ��邩
            double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
            double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[0].x + (p0.x - p2.x) * cutPoint[0].z);
            double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[0].x + (p1.x - p0.x) * cutPoint[0].z);

            // �܂��͎O�p�`�̒��ɂ��邩
            if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
            {       // �O�p�`�̒��ɂ���

                // ���_���X�g�ɒǉ�
                vertices1.Add(p0 - transform.position);
                vertices1.Add(p1 - transform.position);
                vertices1.Add(p2 - transform.position);

                // �ӂ̏�ɂ��邩
                if (t < 0.002f) // ��S��
                {
                    Debug.Log("��S��");
                    edge = p1 - p0; // ��p0p2

                    vertices1.Add(cutPoint[0] + edge * 0.01f - transform.position); // 3�Ԗڂ̒��_�̒ǉ�
                    vertices1.Add(cutPoint[0] - edge * 0.01f - transform.position); // 4�Ԗڂ̒��_�̒ǉ�
                    vertices1.Add(cutPoint[1] - transform.position); // 5�Ԗڂ̒��_
                    vertices1.Add(cutPoint[1] - transform.position); // 6�Ԗڂ̒��_

                    // ���_�̃C���f�b�N�X
                    int _0 = vertices1.Count - 7;
                    int _1 = vertices1.Count - 6;
                    int _2 = vertices1.Count - 5;
                    int _3 = vertices1.Count - 4;
                    int _4 = vertices1.Count - 3;
                    int _5 = vertices1.Count - 2;
                    int _6 = vertices1.Count - 1;   // �g��Ȃ�

                    // �C���f�b�N�X�̐U�蕪��
                    triangles1.Add(_5);
                    triangles1.Add(_2);
                    triangles1.Add(_0);

                    triangles1.Add(_5);
                    triangles1.Add(_0);
                    triangles1.Add(_4);

                    triangles1.Add(_5);
                    triangles1.Add(_3);
                    triangles1.Add(_1);

                    triangles1.Add(_5);
                    triangles1.Add(_1);
                    triangles1.Add(_2);

                    // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                    idxMemory.Add(_5);
                    idxMemory.Add(_2);
                    idxMemory.Add(_0);

                    idxMemory.Add(_5);
                    idxMemory.Add(_0);
                    idxMemory.Add(_4);

                    idxMemory.Add(_5);
                    idxMemory.Add(_3);
                    idxMemory.Add(_1);

                    idxMemory.Add(_5);
                    idxMemory.Add(_1);
                    idxMemory.Add(_2);
                }
                else if (s < 0.002f)    // ��T��
                {
                    Debug.Log("��T��");
                    edge = p2 - p0; // ��p0p1

                    vertices1.Add(cutPoint[0] - edge * 0.01f - transform.position); // 3�Ԗڂ̒��_�̒ǉ�
                    vertices1.Add(cutPoint[0] + edge * 0.01f - transform.position); // 4�Ԗڂ̒��_�̒ǉ�
                    vertices1.Add(cutPoint[1] - transform.position); // 5�Ԗڂ̒��_
                    vertices1.Add(cutPoint[1] - transform.position); // 6�Ԗڂ̒��_

                    // ���_�̃C���f�b�N�X
                    int _0 = vertices1.Count - 7;
                    int _1 = vertices1.Count - 6;
                    int _2 = vertices1.Count - 5;
                    int _3 = vertices1.Count - 4;
                    int _4 = vertices1.Count - 3;
                    int _5 = vertices1.Count - 2;
                    int _6 = vertices1.Count - 1;   // �g��Ȃ�

                    // �C���f�b�N�X�̐U�蕪��
                    triangles1.Add(_5);
                    triangles1.Add(_1);
                    triangles1.Add(_2);

                    triangles1.Add(_5);
                    triangles1.Add(_2);
                    triangles1.Add(_4);

                    triangles1.Add(_5);
                    triangles1.Add(_3);
                    triangles1.Add(_0);

                    triangles1.Add(_5);
                    triangles1.Add(_0);
                    triangles1.Add(_1);

                    // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                    idxMemory.Add(_5);
                    idxMemory.Add(_1);
                    idxMemory.Add(_2);

                    idxMemory.Add(_5);
                    idxMemory.Add(_2);
                    idxMemory.Add(_4);

                    idxMemory.Add(_5);
                    idxMemory.Add(_3);
                    idxMemory.Add(_0);

                    idxMemory.Add(_5);
                    idxMemory.Add(_0);
                    idxMemory.Add(_1);

                }
                else if (s + t > 0.98f) // ��S+T��
                {
                    Debug.Log("��S + T��");
                    edge = p2 - p1; // ��p1p2

                    vertices1.Add(cutPoint[0] + edge * 0.01f - transform.position); // 3�Ԗڂ̒��_�̒ǉ�
                    vertices1.Add(cutPoint[0] - edge * 0.01f - transform.position); // 4�Ԗڂ̒��_�̒ǉ�
                    vertices1.Add(cutPoint[1] - transform.position); // 5�Ԗڂ̒��_
                    vertices1.Add(cutPoint[1] - transform.position); // 6�Ԗڂ̒��_

                    // ���_�̃C���f�b�N�X
                    int _0 = vertices1.Count - 7;
                    int _1 = vertices1.Count - 6;
                    int _2 = vertices1.Count - 5;
                    int _3 = vertices1.Count - 4;
                    int _4 = vertices1.Count - 3;
                    int _5 = vertices1.Count - 2;
                    int _6 = vertices1.Count - 1;   // �g��Ȃ�


                    // �C���f�b�N�X�̐U�蕪��
                    triangles1.Add(_5);
                    triangles1.Add(_0);
                    triangles1.Add(_1);

                    triangles1.Add(_5);
                    triangles1.Add(_1);
                    triangles1.Add(_4);

                    triangles1.Add(_5);
                    triangles1.Add(_3);
                    triangles1.Add(_2);

                    triangles1.Add(_5);
                    triangles1.Add(_2);
                    triangles1.Add(_0);

                    // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                    idxMemory.Add(_5);
                    idxMemory.Add(_0);
                    idxMemory.Add(_1);

                    idxMemory.Add(_5);
                    idxMemory.Add(_1);
                    idxMemory.Add(_4);

                    idxMemory.Add(_5);
                    idxMemory.Add(_3);
                    idxMemory.Add(_2);

                    idxMemory.Add(_5);
                    idxMemory.Add(_2);
                    idxMemory.Add(_0);
                }

                Debug.Log("s:" + s);
                Debug.Log("t:" + t);
                Debug.Log("s + t:" + s + t);
            }
            else    // �O�p�`�̒��ɂȂ�
            {
                Debug.Log("�|���S���̒��ɂ���܂���");
                Debug.Log("s:" + s);
                Debug.Log("t:" + t);
                Debug.Log("s + t:" + s + t);
            }


        }

        // ������̃I�u�W�F�N�g�����A���낢��Ƃ����
        var normal = new List<Vector3>();
        for (int i = 0; i < vertices1.Count; i++)
        {
            normal.Add(Vector3.up);
        }

        attachedMesh.SetVertices(vertices1.ToArray());
        attachedMesh.SetTriangles(triangles1.ToArray(), 0);
        attachedMesh.SetNormals(normal);
        Debug.Log(attachedMesh.vertices.Length);
        // ������̃I�u�W�F�N�g�����A���낢��Ƃ����
        //GameObject obj = new GameObject("Plane1", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody), typeof(MeshDivision2));
        //var mesh = new Mesh();
        //mesh.vertices = vertices1.ToArray();
        //mesh.triangles = triangles1.ToArray();
        ////mesh.uv = uvs1.ToArray();


        ////mesh.normals = normals1.ToArray();
        //Debug.Log("mesh.normals.Length" + mesh.normals.Length);
        //obj.GetComponent<MeshFilter>().mesh = mesh;
        //obj.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        //obj.GetComponent<MeshCollider>().sharedMesh = mesh;
        //obj.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
        //obj.GetComponent<MeshCollider>().convex = false;
        //obj.GetComponent<MeshCollider>().material = GetComponent<Collider>().material;
        //obj.transform.position = transform.position;
        //obj.transform.rotation = transform.rotation;
        //obj.transform.localScale = transform.localScale;
        ////obj.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        ////obj.GetComponent<Rigidbody>().angularVelocity = GetComponent<Rigidbody>().angularVelocity;
        ////obj.GetComponent<MeshCut>().skinWidth = skinWidth;


        //obj.GetComponent<Rigidbody>().useGravity = false;   // �d�̖͂�����
        //obj.GetComponent<Rigidbody>().isKinematic = true;   // �^���𖳌��� 



        //���̃I�u�W�F�N�g���f�X�g���C
        //Destroy(gameObject);

    }

    // �r���̃J�b�g�|�C���g�ł̕���
    public bool DiviosionMeshMiddle(List<Vector3> cutPoint)
    {
        Debug.Log("�O" + cutPoint.Count);

        if (cutPoint.Count < 3) return false;
        Debug.Log("��");
        // ���b�V���̃A�^�b�`
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;



        // �ϐ�
        Vector3 p0, p1, p2;    // ���b�V���̃|���S���̒��_
        var uvs1 = new List<Vector2>(); // �e�N�X�`��
        //var uvs2 = new List<Vector2>();
        var vertices1 = new List<Vector3>();   // ���_
        //var vertices2 = new List<DVector3>();
        var triangles1 = new List<int>();       // �O�p�`�C���f�b�N�X
        //var triangles2 = new List<int>();
        var normals1 = new List<Vector3>();     // �@��
                                                //var normals2 = new List<Vector3>();

        Vector3 edge = new Vector3();
        Vector3 edge1 = new Vector3();
        Vector3 edge2 = new Vector3();


        var crossVertices = new List<Vector3>();

        // ���Œ��_���
        for (int i = 0; i < attachedMesh.vertices.Length; i++)
        {
            vertices1.Add(attachedMesh.vertices[i]);
        }
        for (int i = 0; i < attachedMesh.triangles.Length; i++)
        {
            triangles1.Add(attachedMesh.triangles[i]);
        }


        // �������W�ɒ��_����������L����
        for (int i = 0; i < vertices1.Count - 1; i++)
        {
            // �������W����Ȃ������X���[
            if (vertices1[i] != vertices1[i + 1]) continue;

            // �؂�����ɑ΂��Đ����ɓ_���ړ����߂̏���
            edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
            edge2 = (cutPoint[cutPoint.Count - 1] + Vector3.up) - cutPoint[cutPoint.Count - 1];
            edge = Vector3.Cross(edge1, edge2);

            vertices1[i] = vertices1[i] - edge * 0.3f;
            vertices1[i + 1] = vertices1[i + 1] + edge * 0.3f;
        }

        //�J�b�g�������I�u�W�F�N�g�̃��b�V�����g���C�A���O�����Ƃɏ���
        for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
        {
            //���b�V����3�̒��_���擾
            p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);//+ Vector3.one * 0.0001f;
            p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);//+ Vector3.one * 0.0001f;
            p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]); //+ Vector3.one * 0.0001f;

            // �J�b�g�|�C���g�̎n�_���|���S���̒��ɂ��邩
            double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
            double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[cutPoint.Count - 1].x + (p0.x - p2.x) * cutPoint[cutPoint.Count - 1].z);
            double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[cutPoint.Count - 1].x + (p1.x - p0.x) * cutPoint[cutPoint.Count - 1].z);

            // �O�p�`�̒��ɂ��邩
            if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
            {
                // ��O�̃J�b�g�|�C���g�����邩
                double _s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[cutPoint.Count - 2].x + (p0.x - p2.x) * cutPoint[cutPoint.Count - 2].z);
                double _t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[cutPoint.Count - 2].x + (p1.x - p0.x) * cutPoint[cutPoint.Count - 2].z);
                if ((0 <= _s && _s <= 1) && (0 <= _t && _t <= 1) && (0 <= 1 - _s - _t && 1 - _s - _t <= 1))
                {
                    // ����Ƃ�
                    Debug.Log("����");
                }
                else
                {
                    // �Ȃ��Ƃ�
                    Debug.Log("�Ȃ�");

                    // �����Ɛ����̌�_�ɒ��_��ǉ�������

                    // �|���S�����Ƃɏ���
                    for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            // �|���S����2���_
                            Vector2 polyVtx_s = new Vector2(attachedMesh.vertices[attachedMesh.triangles[j + k]].x, attachedMesh.vertices[attachedMesh.triangles[j + k]].z);  // �n�_
                            Vector2 polyVtx_v = new Vector2(attachedMesh.vertices[attachedMesh.triangles[j + (k + 1) % 3]].x, attachedMesh.vertices[attachedMesh.triangles[j + (k + 1) % 3]].z);  // �I�_

                            // �|���S���̕�
                            Vector2 polyEdge = polyVtx_v - polyVtx_s;   // ��

                            // �J�b�g�|�C���g��2���_
                            Vector2 cpVtx_s = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z); // �n�_
                            Vector2 cpVtx_v = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z); // �I�_

                            // �J�b�g�|�C���g�̕�
                            Vector2 cpEdge = cpVtx_v - cpVtx_s; // ��

                            // �|���S���̕ӂƃJ�b�g�|�C���g�̕ӂ̎n�_���Ȃ����x�N�g��
                            Vector2 v = polyVtx_s - cpVtx_s;

                            // �����̎n�_�����_�̃x�N�g���̌W��(����)
                            float t1 = (v.x * polyEdge.y - polyEdge.x * v.y) / (cpEdge.x * polyEdge.y - polyEdge.x * cpEdge.y);
                            float t2 = (v.x * cpEdge.y - cpEdge.x * v.y) / (cpEdge.x * polyEdge.y - polyEdge.x * cpEdge.y);

                            // ��_
                            Vector2 p = new Vector2(polyVtx_s.x, polyVtx_s.y) + new Vector2(polyEdge.x * t2, polyEdge.y * t2);



                            // �����Ɛ�����������Ă��邩
                            const float eps = 0.00001f;
                            if (t1 + eps < 0 || t1 - eps > 1 || t2 + eps < 0 || t2 - eps > 1)
                            {
                                // Debug.Log("�������ĂȂ�");
                            }
                            else
                            {
                                Debug.Log("�������Ă�");

                                // ��_�����Ƃɒ��_��ǉ�
                                vertices1.Add(new Vector3(p.x, cutPoint[cutPoint.Count - 1].y - transform.position.y, p.y));
                            }
                        }

                    }



                }


                // �J�b�g�|�C���g�̏ꏊ�ɒ��_�̒ǉ�(���Ƃŕ����邽�ߓ�ǉ�)
                vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);
                vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);

                // �C���f�b�N�X�̊��蓖��
                int _0 = attachedMesh.triangles[i];
                int _1 = attachedMesh.triangles[i + 1];
                int _2 = attachedMesh.triangles[i + 2];
                int _3 = vertices1.Count - 2; // 7
                int _4 = vertices1.Count - 1; // �g��Ȃ�  
                int _5 = vertices1.Count - 3; // 6

                // �L�����ꂽ�O�p�`�C���f�b�N�X�̐��������[�v
                for (int j = 0; j < idxMemory.Count; j += 3)
                {
                    // �L�����ꂽ�O�p�`�C���f�N�X�ƈ�v������
                    if (idxMemory.Count > 9)
                    {
                        if (attachedMesh.triangles[i] == idxMemory[j] && attachedMesh.triangles[i + 1] == idxMemory[j + 1] && attachedMesh.triangles[i + 2] == idxMemory[j + 2])
                        {
                            Debug.Log(idxMemory[j] + "" + idxMemory[j + 1] + "" + idxMemory[j + 2]);
                            if (j == 0)
                            {
                                Debug.Log("j = 0");
                                // �C���f�b�N�X�̕ύX
                                triangles1[j + 3] = _5;

                                // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�

                                triangles1.RemoveRange(i, 3);
                                triangles1.Add(_3);
                                triangles1.Add(_1);
                                triangles1.Add(_2);

                                triangles1.Add(_3);
                                triangles1.Add(_2);
                                triangles1.Add(_5);

                                triangles1.Add(_3);
                                triangles1.Add(_0);
                                triangles1.Add(_1);

                                // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                                idxMemory.Clear();
                                idxMemory.Add(_3);
                                idxMemory.Add(_1);
                                idxMemory.Add(_2);

                                idxMemory.Add(_3);
                                idxMemory.Add(_2);
                                idxMemory.Add(_5);

                                idxMemory.Add(_3);
                                idxMemory.Add(_0);
                                idxMemory.Add(_1);
                                break;
                            }
                            if (j == 3)
                            {
                                Debug.Log("j = 3");
                                // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                                triangles1.RemoveRange(i, 3);
                                triangles1.Add(_3);
                                triangles1.Add(_0);
                                triangles1.Add(_1);

                                triangles1.Add(_3);
                                triangles1.Add(_1);
                                triangles1.Add(_2);

                                triangles1.Add(_3);
                                triangles1.Add(_2);
                                triangles1.Add(_5);

                                // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                                idxMemory.Clear();
                                idxMemory.Add(_3);
                                idxMemory.Add(_0);
                                idxMemory.Add(_1);

                                idxMemory.Add(_3);
                                idxMemory.Add(_1);
                                idxMemory.Add(_2);

                                idxMemory.Add(_3);
                                idxMemory.Add(_2);
                                idxMemory.Add(_5);
                                break;
                            }
                            if (j == 6)
                            {
                                Debug.Log("j = 6");
                                // �C���f�b�N�X�̕ύX
                                triangles1[j - 6] = _5;
                                triangles1[j - 3] = _5;
                                triangles1[j + 3] = _5;

                                // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                                triangles1.RemoveRange(i, 3);
                                triangles1.Add(_3);
                                triangles1.Add(_0);
                                triangles1.Add(_1);

                                triangles1.Add(_3);
                                triangles1.Add(_1);
                                triangles1.Add(_2);

                                triangles1.Add(_3);
                                triangles1.Add(_2);
                                triangles1.Add(_5);

                                // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                                idxMemory.Clear();
                                idxMemory.Add(_3);
                                idxMemory.Add(_0);
                                idxMemory.Add(_1);

                                idxMemory.Add(_3);
                                idxMemory.Add(_1);
                                idxMemory.Add(_2);

                                idxMemory.Add(_3);
                                idxMemory.Add(_2);
                                idxMemory.Add(_5);
                                break;
                            }
                            if (j == 9)
                            {
                                Debug.Log("j = 9");
                                triangles1[j - 9] = _5;
                                triangles1[j - 6] = _5;

                                // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                                triangles1.RemoveRange(i, 3);
                                triangles1.Add(_3);
                                triangles1.Add(_0);
                                triangles1.Add(_1);

                                triangles1.Add(_3);
                                triangles1.Add(_1);
                                triangles1.Add(_2);

                                triangles1.Add(_3);
                                triangles1.Add(_2);
                                triangles1.Add(_5);

                                // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                                idxMemory.Clear();
                                idxMemory.Add(_3);
                                idxMemory.Add(_0);
                                idxMemory.Add(_1);

                                idxMemory.Add(_3);
                                idxMemory.Add(_1);
                                idxMemory.Add(_2);

                                idxMemory.Add(_3);
                                idxMemory.Add(_2);
                                idxMemory.Add(_5);
                                break;
                            }

                        }

                    }
                    else if (idxMemory.Count < 10)
                    {
                        if (attachedMesh.triangles[i] == idxMemory[j] && attachedMesh.triangles[i + 1] == idxMemory[j + 1] && attachedMesh.triangles[i + 2] == idxMemory[j + 2])
                        {
                            if (j == 0)
                            {
                                Debug.Log("j = 0");
                                // �C���f�b�N�X�̕ύX
                                triangles1[i + j + 3] = _5;

                                // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�

                                triangles1.RemoveRange(i, 3);
                                triangles1.Add(_3);
                                triangles1.Add(_1);
                                triangles1.Add(_2);

                                triangles1.Add(_3);
                                triangles1.Add(_2);
                                triangles1.Add(_5);

                                triangles1.Add(_3);
                                triangles1.Add(_0);
                                triangles1.Add(_1);

                                // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                                idxMemory.Clear();
                                idxMemory.Add(_3);
                                idxMemory.Add(_1);
                                idxMemory.Add(_2);

                                idxMemory.Add(_3);
                                idxMemory.Add(_2);
                                idxMemory.Add(_5);

                                idxMemory.Add(_3);
                                idxMemory.Add(_0);
                                idxMemory.Add(_1);
                                break;
                            }
                            if (j == 3)
                            {
                                Debug.Log("j = 3");

                                triangles1[i + j] = _5;
                                // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                                triangles1.RemoveRange(i, 3);
                                triangles1.Add(_3);
                                triangles1.Add(_0);
                                triangles1.Add(_1);

                                triangles1.Add(_3);
                                triangles1.Add(_1);
                                triangles1.Add(_2);

                                triangles1.Add(_3);
                                triangles1.Add(_2);
                                triangles1.Add(_5);

                                // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                                idxMemory.Clear();
                                idxMemory.Add(_3);
                                idxMemory.Add(_0);
                                idxMemory.Add(_1);

                                idxMemory.Add(_3);
                                idxMemory.Add(_1);
                                idxMemory.Add(_2);

                                idxMemory.Add(_3);
                                idxMemory.Add(_2);
                                idxMemory.Add(_5);
                                break;
                            }
                            if (j == 6)
                            {
                                Debug.Log("j = 6");
                                // �C���f�b�N�X�̕ύX
                                //triangles1[i + j - 6] = _5;
                                //triangles1[i + j ] = _5;
                                //triangles1[i + j + 3] = _5;

                                // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                                triangles1.RemoveRange(i, 3);
                                triangles1.Add(_3);
                                triangles1.Add(_0);
                                triangles1.Add(_1);

                                triangles1.Add(_3);
                                triangles1.Add(_1);
                                triangles1.Add(_2);

                                triangles1.Add(_3);
                                triangles1.Add(_2);
                                triangles1.Add(_5);

                                // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                                idxMemory.Clear();
                                idxMemory.Add(_3);
                                idxMemory.Add(_0);
                                idxMemory.Add(_1);

                                idxMemory.Add(_3);
                                idxMemory.Add(_1);
                                idxMemory.Add(_2);

                                idxMemory.Add(_3);
                                idxMemory.Add(_2);
                                idxMemory.Add(_5);
                                break;
                            }
                        }
                    }



                }

                Debug.Log(idxMemory.Count);

                //if (attachedMesh.triangles[i] == 5 && attachedMesh.triangles[i + 1] == 1 && attachedMesh.triangles[i + 2] == 2)
                //{
                //    Debug.Log("512");

                //    for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
                //    {
                //        if (!(attachedMesh.triangles[j] == 5 && attachedMesh.triangles[j + 1] == 2 && attachedMesh.triangles[j + 2] == 4)) continue;

                //        triangles1[j] = 6;
                //    }

                //    // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                //    triangles1.RemoveRange(i, 3);
                //    triangles1.Add(_3);
                //    triangles1.Add(_0);
                //    triangles1.Add(_1);

                //    triangles1.Add(_3);
                //    triangles1.Add(_1);
                //    triangles1.Add(_2);

                //    triangles1.Add(_3);
                //    triangles1.Add(_2);
                //    triangles1.Add(_5);

                //}
                //else if (attachedMesh.triangles[i] == 5 && attachedMesh.triangles[i + 1] == 2 && attachedMesh.triangles[i + 2] == 4)
                //{
                //    Debug.Log("524");//012

                //    // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                //    triangles1.RemoveRange(i, 3);
                //    triangles1.Add(_3);
                //    triangles1.Add(_0);
                //    triangles1.Add(_1);

                //    triangles1.Add(_3);
                //    triangles1.Add(_1);
                //    triangles1.Add(_2);

                //    triangles1.Add(_3);
                //    triangles1.Add(_2);
                //    triangles1.Add(_5);
                //}
                //else if (attachedMesh.triangles[i] == 5 && attachedMesh.triangles[i + 1] == 3 && attachedMesh.triangles[i + 2] == 0)
                //{
                //    Debug.Log("530");

                //    for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
                //    {
                //        if (!(attachedMesh.triangles[j] == 5 && attachedMesh.triangles[j + 1] == 1 && attachedMesh.triangles[j + 2] == 2)) continue;

                //        triangles1[j] = 6;
                //    }

                //    for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
                //    {
                //        if (!(attachedMesh.triangles[j] == 5 && attachedMesh.triangles[j + 1] == 2 && attachedMesh.triangles[j + 2] == 4)) continue;

                //        triangles1[j] = 6;
                //    }
                //    for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
                //    {
                //        if (!(attachedMesh.triangles[j] == 5 && attachedMesh.triangles[j + 1] == 0 && attachedMesh.triangles[j + 2] == 1)) continue;

                //        triangles1[j] = 6;
                //    }
                //    // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                //    triangles1.RemoveRange(i, 3);
                //    triangles1.Add(_3);
                //    triangles1.Add(_0);
                //    triangles1.Add(_1);

                //    triangles1.Add(_3);
                //    triangles1.Add(_1);
                //    triangles1.Add(_2);

                //    triangles1.Add(_3);
                //    triangles1.Add(_2);
                //    triangles1.Add(_5);
                //}
                //else if (attachedMesh.triangles[i] == 5 && attachedMesh.triangles[i + 1] == 0 && attachedMesh.triangles[i + 2] == 1)
                //{
                //    Debug.Log("501");

                //    for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
                //    {
                //        if (!(attachedMesh.triangles[j] == 5 && attachedMesh.triangles[j + 1] == 1 && attachedMesh.triangles[j + 2] == 2)) continue;

                //        triangles1[j] = 6;
                //    }
                //    for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
                //    {
                //        if (!(attachedMesh.triangles[j] == 5 && attachedMesh.triangles[j + 1] == 2 && attachedMesh.triangles[j + 2] == 4)) continue;

                //        triangles1[j] = 6;
                //    }

                //    // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                //    triangles1.RemoveRange(i, 3);
                //    triangles1.Add(_3);
                //    triangles1.Add(_0);
                //    triangles1.Add(_1);

                //    triangles1.Add(_3);
                //    triangles1.Add(_1);
                //    triangles1.Add(_2);

                //    triangles1.Add(_3);
                //    triangles1.Add(_2);
                //    triangles1.Add(_5);
                //}
                //else
                //{

                //}



                //    triangles1.Add(_3);
                //triangles1.Add(_0);
                //triangles1.Add(_1);

                //triangles1.Add(_3);
                //triangles1.Add(_1);
                //triangles1.Add(_2);

                //triangles1.Add(_3);
                //triangles1.Add(_2);
                //triangles1.Add(_0);

                //// �C���f�b�N�X�̏���
                //Debug.Log("triangle:" + triangles1[i]);

                //triangles1.RemoveRange(i, 3);
                //triangles1.RemoveAt(i);
                //triangles1.RemoveAt(i);
                //triangles1.RemoveAt(i);

                //Debug.Log("a:" + triangles1[i ]);


            }
        }

        // �m�[�}���̐ݒ�
        var normal = new List<Vector3>();
        for (int i = 0; i < vertices1.Count; i++)
        {
            normal.Add(Vector3.up);
        }

        // ���b�V���ɑ��
        attachedMesh.SetVertices(vertices1.ToArray());
        attachedMesh.SetTriangles(triangles1.ToArray(), 0);
        attachedMesh.SetNormals(normal);


        return true;
    }

    // �M�Y���̕\��
    private void OnDrawGizmos()
    {
        if (!attachedMesh) return;




        //for (int i = 0; i < attachedMesh.vertices.Length; i++)
        //{
        //    if (CrossNum.Count > 0)
        //    {
        //        for (int j = 0; j < CrossNum.Count; j++)
        //        {
        //            if (CrossNum[j] == i)
        //            {
        //                Gizmos.color = new Color(0, 0, 225, 1);   // �F�̎w��
        //                break;
        //            }
        //            else
        //            {
        //                Gizmos.color = new Color(25, 0, 0, 1);   // �F�̎w��
        //            }

        //        }


        //    }
        //    else
        //    {
        //        Gizmos.color = new Color(25, 0, 0, 1);   // �F�̎w��
        //    }

        //    Gizmos.DrawSphere(attachedMesh.vertices[i] + transform.position, 0.05f);  // ���̕\��

        //}

        for (int i = 0; i < attachedMesh.vertices.Length; i++)
        {
            Gizmos.color = new Color(25, 0, 0, 1);   // �F�̎w��
            Gizmos.DrawSphere(attachedMesh.vertices[i] + transform.position, 0.01f);
        }

        for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
        {
            Gizmos.color = new Color(25, 0, 0, 1);   // �F�̎w��
            for (int j = 0; j < 3; j++)
            {
                Gizmos.DrawLine(attachedMesh.vertices[attachedMesh.triangles[i + j]] + transform.position, attachedMesh.vertices[attachedMesh.triangles[i + (j + 1) % 3]] + transform.position);  // ���̕\��

            }

        }
    }
}
