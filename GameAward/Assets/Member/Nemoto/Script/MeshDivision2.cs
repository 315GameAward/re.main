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
using System.Linq;

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
        public Triangle[] edgeLink = new Triangle[3];
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

    // ���b�V���̕���(�ŏ�)
    public void DivisionMesh(List<Vector3> cutPoint)
    {
        Debug.Log("================= �؂�n�� ==================");
        // ���b�V���̃A�^�b�`
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;

        // �ϐ�
        var uvs1 = new List<Vector2>(); // �e�N�X�`��
        var vertices1 = new List<Vector3>();   // ���_
        var triangles1 = new List<int>();       // �O�p�`�C���f�b�N�X
        var normals1 = new List<Vector3>();     // �@��
       
        // ���b�V���̏�����
        for (int i = 0; i < attachedMesh.vertices.Length; i++)
        {
            vertices1.Add(attachedMesh.vertices[i]);
        }
        for (int i = 0; i < attachedMesh.triangles.Length; i++)
        {
            triangles1.Add(attachedMesh.triangles[i]);
        }

        // �ϐ��錾              
        var straddlePolyIdx = new List<int>();  // �܂������|���S���ԍ����X�g
        var crossPolyIdx = new List<int>();     // �����|���S���ԍ����X�g
        var inerPolyIdx = new List<int>();      // �J�b�g�|�C���g�����ɓ����Ă���|���S���ԍ�
        var intersectPolyList = new List<List<Vector2>>();  // �|���S�����Ƃɂ���������Ă���_�̃��X�g
        var intersectPolyList2 = new List<List<Vector2>>();  // �|���S�����Ƃɂ���������Ă���_�̃��X�g2
        var intersectEdgList = new List<List<Vector2>>();   // �|���S�����Ƃɂ���������Ă���ӂ̃��X�g
        var intersectEdgList2 = new List<List<Vector2>>();   // �|���S�����Ƃɂ���������Ă���ӂ̃��X�g
        var intersectionList = new List<Vector2>();         // ��_�̃��X�g
        var cp_s = new Vector2(cutPoint[cutPoint.Count - 2].x, cutPoint[cutPoint.Count - 2].z);    // �J�b�g�|�C���g�̏I�_��1�O
        var cp_v = new Vector2(cutPoint[cutPoint.Count - 1].x, cutPoint[cutPoint.Count - 1].z);    // �J�b�g�|�C���g�̏I�_
        var cpEdg = cp_v - cp_s;    // �J�b�g�|�C���g�̏I�_�ƃJ�b�g�|�C���g�̏I�_��1�O���Ȃ�����
        var checkCp = cp_s + cpEdg * 0.01f; // �J�b�g�|�C���g�̏I�_��1�O����J�b�g�|�C���g�̏I�_�̕����ɂ�����ƐL�΂����_
        var edgIdx2List = new List<List<int>>();   // �ӂ̃C���f�b�N�X�̃��X�g�̃��X�g   
        var edgIdx2List2 = new List<List<int>>();  // �ӂ̃C���f�b�N�X�̃��X�g�̃��X�g2   

        // �܂����ł�|���S���ƐN�����Ă���|���S���������邩�T��
        for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
        {
            // �ϐ��錾             
            int interPointCnt = 0; // ���������_�̐�
            var intersection = new List<Vector2>(); // ��_�̃��X�g
            var edgList = new List<Vector2>(); //�ӂ̃��X�g
            var edgIdxList = new List<int>();   // �ӂ̃C���f�b�N�X�̃��X�g   

            // �|���S���̕ӂ̐��������[�v
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

                // �J�b�g�|�C���g�̎n�_�̕␳
                cpVtx_s -= cpEdge * 0.02f;

                // �J�b�g�|�C���g�̕ӂ̕␳
                cpEdge = cpVtx_v - cpVtx_s; // ��

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
                    // ������ĂȂ��Ƃ��X���[
                    continue;
                }
                else
                {
                    // ������Ă鎞��_�J�E���g++                               
                    interPointCnt++;    // ��_�J�E���g    
                    intersection.Add(p);    // ��_�̕ۑ�
                    intersectionList.Add(p);// ��_�̕ۑ�
                    edgList.Add(polyEdge);
                    edgIdxList.Add(attachedMesh.triangles[j + k]);
                    edgIdxList.Add(attachedMesh.triangles[j + (k + 1) % 3]);
                  
                }
            }

            // �|���S���ԍ���ۑ�
            if (interPointCnt == 2)// ��_�J�E���g2��(�|���S�����܂����ł鎞)
            {
                Debug.Log("2�����");
                //Debug.Log("�|���S���ԍ���" + attachedMesh.triangles[j] + "," + attachedMesh.triangles[j + 1] + "," + attachedMesh.triangles[j + 2]);

                straddlePolyIdx.Add(attachedMesh.triangles[j]    );
                straddlePolyIdx.Add(attachedMesh.triangles[j + 1]);
                straddlePolyIdx.Add(attachedMesh.triangles[j + 2]);
                crossPolyIdx.Add(j);
                crossPolyIdx.Add(j + 1);
                crossPolyIdx.Add(j + 2);
                intersectPolyList2.Add(intersection);
                intersectEdgList2.Add(edgList);
                edgIdx2List2.Add(edgIdxList);
                //Debug.Log("straddlePolyIdx.Count" + straddlePolyIdx.Count);
            }
            else if (interPointCnt == 1)// ��_�J�E���g1��(�J�b�g�|�C���g�̏I�_���|���S���̒��ɂ���Ƃ�)
            {
                Debug.Log("1�����");
                inerPolyIdx.Add(attachedMesh.triangles[j]    );
                inerPolyIdx.Add(attachedMesh.triangles[j + 1]);
                inerPolyIdx.Add(attachedMesh.triangles[j + 2]);
                crossPolyIdx.Add(j);
                crossPolyIdx.Add(j + 1);
                crossPolyIdx.Add(j + 2);
                edgIdx2List.Add(edgIdxList);
                intersectPolyList.Add(intersection);
                intersectEdgList.Add(edgList);
            }
            else
            {
                // Debug.Log("3�����");
                // Debug.Log("�|���S���ԍ���" + attachedMesh.triangles[j] + "," + attachedMesh.triangles[j + 1] + "," + attachedMesh.triangles[j + 2]);

            }
        }

        //--- �J�b�g�|�C���g�Ƃ̌�_�̐��ŕ��� ---
        // ��_��1�̎�(�|���S���̒���2��_������|���S����1���Ȃ��Ƃ�)
        if(straddlePolyIdx.Count == 0)
        {
            Debug.Log("=============����:��_��1��=============");

            //--- �ϐ��錾 ---
            int firstNum = 0;
            Vector2 cpS = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z);
            Vector2 cpV = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z);
            Vector2 p = cpV;

            var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
            var cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));
            var pEnd = intersectPolyList[firstNum][0];    // ��_�̏I�_
            var pEdge = new Vector2(Mathf.Abs(intersectEdgList[0][0].x), Mathf.Abs(intersectEdgList[0][0].y));
           
            // 4�������鏈��
            {
                Debug.Log("4�������鏈��");
                //Debug.Log("edgIdx2List[0][0]:" + edgIdx2List[0][0]);
                //Debug.Log("edgIdx2List[0][0]:" + edgIdx2List[0][1]);
                //Debug.Log("inerPolyIdx.Count:" + inerPolyIdx.Count);
                //Debug.Log("inerPolyIdx[0]:" + inerPolyIdx[0]);
                //Debug.Log("inerPolyIdx[1]:" + inerPolyIdx[1]);
                //Debug.Log("inerPolyIdx[2]:" + inerPolyIdx[2]);

                // �Ώۂ̃C���f�b�N�X�̍폜
                for (int a = 0; a < triangles1.Count; a += 3)
                {
                    // ��v���Ȃ�������X���[
                    if (!(triangles1[a] == inerPolyIdx[0] && triangles1[a + 1] == inerPolyIdx[1] && triangles1[a + 2] == inerPolyIdx[2])) continue;

                    triangles1.RemoveRange(a, 3);
                    break;
                }

                // ���_�̒ǉ�
                vertices1.Add(new Vector3(cutPoint[0].x - transform.position.x, attachedMesh.vertices[0].y, cutPoint[0].z - transform.position.z) + new Vector3(pEdge.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * -cpNormalAbs.z));
                vertices1.Add(new Vector3(cutPoint[0].x - transform.position.x, attachedMesh.vertices[0].y, cutPoint[0].z - transform.position.z) + new Vector3(pEdge.normalized.x * 0.04f * cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * cpNormalAbs.z));
                
                vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);
                vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);

                // �C���f�b�N�X�̊��蓖��
                int idx0 = inerPolyIdx[0];
                int idx1 = inerPolyIdx[1];
                int idx2 = inerPolyIdx[2];
                int idx3 = vertices1.Count - 4; // 
                int idx4 = vertices1.Count - 3; //  
                int idx5 = vertices1.Count - 2; // 
                int idx6 = vertices1.Count - 1; // 

                // ��_�̎n�_���ǂ̕ӂɂ��邩
                // ��01�̏�
                if ((edgIdx2List[0][0] == inerPolyIdx[0] || edgIdx2List[0][0] == inerPolyIdx[1]) &&
                   (edgIdx2List[0][1] == inerPolyIdx[1] || edgIdx2List[0][1] == inerPolyIdx[0]))
                {
                    Debug.Log("��01�̏�");
                    triangles1.Add(idx5);
                    triangles1.Add(idx2);
                    triangles1.Add(idx0);

                    triangles1.Add(idx5);
                    triangles1.Add(idx0);
                    triangles1.Add(idx4);

                    triangles1.Add(idx5);
                    triangles1.Add(idx3);
                    triangles1.Add(idx1);

                    triangles1.Add(idx5);
                    triangles1.Add(idx1);
                    triangles1.Add(idx2);

                    // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                    idxMemory.Clear();
                    idxMemory.Add(idx5);
                    idxMemory.Add(idx2);
                    idxMemory.Add(idx0);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx0);
                    idxMemory.Add(idx4);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx3);
                    idxMemory.Add(idx1);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx1);
                    idxMemory.Add(idx2);
                }
                // ��12�̏�
                else if ((edgIdx2List[0][0] == inerPolyIdx[1] || edgIdx2List[0][0] == inerPolyIdx[2]) &&
                   (edgIdx2List[0][1] == inerPolyIdx[2] || edgIdx2List[0][1] == inerPolyIdx[1]))
                {
                    Debug.Log("��12�̏�");
                    triangles1.Add(idx5);
                    triangles1.Add(idx0);
                    triangles1.Add(idx1);

                    triangles1.Add(idx5);
                    triangles1.Add(idx1);
                    triangles1.Add(idx4);

                    triangles1.Add(idx5);
                    triangles1.Add(idx3);
                    triangles1.Add(idx2);

                    triangles1.Add(idx5);
                    triangles1.Add(idx2);
                    triangles1.Add(idx0);

                    // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                    idxMemory.Clear();
                    idxMemory.Add(idx5);
                    idxMemory.Add(idx0);
                    idxMemory.Add(idx1);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx1);
                    idxMemory.Add(idx4);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx3);
                    idxMemory.Add(idx2);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx2);
                    idxMemory.Add(idx0);
                }
                // ��20�̏�
                else if ((edgIdx2List[0][0] == inerPolyIdx[2] || edgIdx2List[0][0] == inerPolyIdx[0]) &&
                   (edgIdx2List[0][1] == inerPolyIdx[0] || edgIdx2List[0][1] == inerPolyIdx[2]))
                {
                    Debug.Log("��20�̏�");
                    triangles1.Add(idx5);
                    triangles1.Add(idx1);
                    triangles1.Add(idx2);

                    triangles1.Add(idx5);
                    triangles1.Add(idx2);
                    triangles1.Add(idx4);

                    triangles1.Add(idx5);
                    triangles1.Add(idx3);
                    triangles1.Add(idx0);

                    triangles1.Add(idx5);
                    triangles1.Add(idx0);
                    triangles1.Add(idx1);

                    // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                    idxMemory.Clear();
                    idxMemory.Add(idx5);
                    idxMemory.Add(idx1);
                    idxMemory.Add(idx2);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx2);
                    idxMemory.Add(idx4);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx3);
                    idxMemory.Add(idx0);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx0);
                    idxMemory.Add(idx1);
                }
            }

            //Debug.Log("��_��1��");
            //�J�b�g�������I�u�W�F�N�g�̃��b�V�����g���C�A���O�����Ƃɏ���
            {
                //for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
                //{
                //    //���b�V����3�̒��_���擾
                //    p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);//+ Vector3.one * 0.0001f;
                //    p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);//+ Vector3.one * 0.0001f;
                //    p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]); //+ Vector3.one * 0.0001f;


                //    // �J�b�g�|�C���g�̎n�_���|���S���̒��ɂ��邩
                //    Vector2 cpS = new Vector2(cutPoint[0].x, cutPoint[0].z);
                //    Vector2 cpV = new Vector2(cutPoint[1].x, cutPoint[1].z);
                //    var cp = cpV - cpS;
                //    cpS += cp * 0.02f;
                //    var v2P0 = new Vector2(p0.x, p0.z);
                //    var v2P1 = new Vector2(p1.x, p1.z);
                //    var v2P2 = new Vector2(p2.x, p2.z);


                //    // �J�b�g�|�C���g�̎n�_���|���S���̕ӂ̏�ɂ��邩
                //    double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
                //    double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cpS.x + (p0.x - p2.x) * cpS.y);
                //    double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cpS.x + (p1.x - p0.x) * cpS.y);

                //    //Debug.Log("Area" + Area);

                //    // �܂��͎O�p�`�̒��ɂ��邩
                //    if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
                //    {       // �O�p�`�̒��ɂ���

                //        // ���_���X�g�ɒǉ�
                //        vertices1.Add(p0 - transform.position);
                //        vertices1.Add(p1 - transform.position);
                //        vertices1.Add(p2 - transform.position);

                //        // �ӂ̏�ɂ��邩
                //        if (t < 0.002f) // ��S��
                //        {
                //            Debug.Log("��S��");
                //            edge = p1 - p0; // ��p0p2

                //            vertices1.Add(cutPoint[0] + edge.normalized * 0.1f - transform.position); // 3�Ԗڂ̒��_�̒ǉ�
                //            vertices1.Add(cutPoint[0] - edge.normalized * 0.1f - transform.position); // 4�Ԗڂ̒��_�̒ǉ�
                //            vertices1.Add(cutPoint[1] - transform.position); // 5�Ԗڂ̒��_
                //            vertices1.Add(cutPoint[1] - transform.position); // 6�Ԗڂ̒��_

                //            // ���_�̃C���f�b�N�X
                //            int _0 = attachedMesh.triangles[i];
                //            int _1 = attachedMesh.triangles[i + 1];
                //            int _2 = attachedMesh.triangles[i + 2];
                //            int _3 = vertices1.Count - 4;
                //            int _4 = vertices1.Count - 3;
                //            int _5 = vertices1.Count - 2;
                //            int _6 = vertices1.Count - 1;   // �g��Ȃ�

                //            // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                //            triangles1.RemoveRange(i, 3);

                //            // �C���f�b�N�X�̐U�蕪��
                //            triangles1.Add(_5);
                //            triangles1.Add(_2);
                //            triangles1.Add(_0);

                //            triangles1.Add(_5);
                //            triangles1.Add(_0);
                //            triangles1.Add(_4);

                //            triangles1.Add(_5);
                //            triangles1.Add(_3);
                //            triangles1.Add(_1);

                //            triangles1.Add(_5);
                //            triangles1.Add(_1);
                //            triangles1.Add(_2);

                //            // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                //            idxMemory.Clear();
                //            idxMemory.Add(_5);
                //            idxMemory.Add(_2);
                //            idxMemory.Add(_0);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_0);
                //            idxMemory.Add(_4);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_3);
                //            idxMemory.Add(_1);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_1);
                //            idxMemory.Add(_2);
                //        }
                //        else if (s < 0.002f)    // ��T��
                //        {
                //            Debug.Log("��T��");
                //            edge = p2 - p0; // ��p0p1

                //            vertices1.Add(cutPoint[0] - edge.normalized * 0.1f - transform.position); // 3�Ԗڂ̒��_�̒ǉ�
                //            vertices1.Add(cutPoint[0] + edge.normalized * 0.1f - transform.position); // 4�Ԗڂ̒��_�̒ǉ�
                //            vertices1.Add(cutPoint[1] - transform.position); // 5�Ԗڂ̒��_
                //            vertices1.Add(cutPoint[1] - transform.position); // 6�Ԗڂ̒��_

                //            // ���_�̃C���f�b�N�X
                //            int _0 = attachedMesh.triangles[i];
                //            int _1 = attachedMesh.triangles[i + 1];
                //            int _2 = attachedMesh.triangles[i + 2];
                //            int _3 = vertices1.Count - 4;
                //            int _4 = vertices1.Count - 3;
                //            int _5 = vertices1.Count - 2;
                //            int _6 = vertices1.Count - 1;   // �g��Ȃ�

                //            // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                //            triangles1.RemoveRange(i, 3);

                //            // �C���f�b�N�X�̐U�蕪��
                //            triangles1.Add(_5);
                //            triangles1.Add(_1);
                //            triangles1.Add(_2);

                //            triangles1.Add(_5);
                //            triangles1.Add(_2);
                //            triangles1.Add(_4);

                //            triangles1.Add(_5);
                //            triangles1.Add(_3);
                //            triangles1.Add(_0);

                //            triangles1.Add(_5);
                //            triangles1.Add(_0);
                //            triangles1.Add(_1);

                //            // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                //            idxMemory.Clear();
                //            idxMemory.Add(_5);
                //            idxMemory.Add(_1);
                //            idxMemory.Add(_2);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_2);
                //            idxMemory.Add(_4);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_3);
                //            idxMemory.Add(_0);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_0);
                //            idxMemory.Add(_1);

                //        }
                //        else if (s + t > 0.98f) // ��S+T��
                //        {
                //            Debug.Log("��S + T��");
                //            edge = p2 - p1; // ��p1p2

                //            vertices1.Add(cutPoint[0] + edge.normalized * 0.1f - transform.position); // 3�Ԗڂ̒��_�̒ǉ�
                //            vertices1.Add(cutPoint[0] - edge.normalized * 0.1f - transform.position); // 4�Ԗڂ̒��_�̒ǉ�
                //            vertices1.Add(cutPoint[1] - transform.position); // 5�Ԗڂ̒��_
                //            vertices1.Add(cutPoint[1] - transform.position); // 6�Ԗڂ̒��_

                //            // ���_�̃C���f�b�N�X
                //            int _0 = attachedMesh.triangles[i];
                //            int _1 = attachedMesh.triangles[i + 1];
                //            int _2 = attachedMesh.triangles[i + 2];
                //            int _3 = vertices1.Count - 4;
                //            int _4 = vertices1.Count - 3;
                //            int _5 = vertices1.Count - 2;
                //            int _6 = vertices1.Count - 1;   // �g��Ȃ�

                //            // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                //            triangles1.RemoveRange(i, 3);

                //            // �C���f�b�N�X�̐U�蕪��
                //            triangles1.Add(_5);
                //            triangles1.Add(_0);
                //            triangles1.Add(_1);

                //            triangles1.Add(_5);
                //            triangles1.Add(_1);
                //            triangles1.Add(_4);

                //            triangles1.Add(_5);
                //            triangles1.Add(_3);
                //            triangles1.Add(_2);

                //            triangles1.Add(_5);
                //            triangles1.Add(_2);
                //            triangles1.Add(_0);

                //            // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                //            idxMemory.Clear();
                //            idxMemory.Add(_5);
                //            idxMemory.Add(_0);
                //            idxMemory.Add(_1);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_1);
                //            idxMemory.Add(_4);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_3);
                //            idxMemory.Add(_2);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_2);
                //            idxMemory.Add(_0);
                //        }
                //        else
                //        {
                //            //Debug.Log("�Ȃ�");
                //        }
                //    }
                //    else    // �O�p�`�̒��ɂȂ�
                //    {
                //        // Debug.Log("�O�p�`�̒��ɂȂ�");
                //    }

                //    Debug.Log("s:" + s);
                //    Debug.Log("t:" + t);
                //    Debug.Log("s + t:" + (s + t));
                //}

            }

        }
        // ��_��2�ȏ�̎�
        else
        {

            Debug.Log("=============����:��_��2�ȏ�=============");
            
            //--- �ϐ��錾 ---
            int firstNum  = 0;
            int secondNum = 0;
            Vector2 cpS = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z);
            Vector2 cpV = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z);
            Vector2 p = cpV;
            var idxList = new List<int>();  // 1�O�ɕ��������C���f�b�N�X�̃��X�g
            var rastIdxList = new List<int>();  // 2�ɕ������鏈���������Ō�̃C���f�b�N�X�ۑ��p�̃��X�g

            // �J�b�g�|�C���g�̎n�_���ǂ̃|���S���̕ӂɂ���̂�
            for (int i = 0; i < intersectPolyList2.Count; i++)
            {
                for(int j = 0;j < intersectPolyList2[i].Count;j++)
                {
                    // �J�b�g�|�C���g�̎n�_��1��ƌ�_�Ƃ̋�������ԉ���(�n�_�ɂȂ�)�_��T��
                    if (Vector2.Distance(cpV, p) > Vector2.Distance(cpV, intersectPolyList2[i][j])) continue;

                    p = intersectPolyList2[i][j];   // �n�_
                    firstNum  = i; // �ŏ��̔ԍ�
                    secondNum = j; // ���̔ԍ�
                    //Debug.Log("��_�~�b�P");
                }                
            }

            //Debug.Log("pEdgeCount" + intersectEdgList2[firstNum].Count);
            //Debug.Log("pEdge1" + intersectEdgList2[firstNum][0]);
            //Debug.Log("pEdge2" + intersectEdgList2[firstNum][1]);
            
           // �ŏ���2�������鏈��
           {
                Debug.Log("����:�ŏ��ɕ������鏈��");

                var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
                var cpNormalAbs = new Vector3();
                if (cpNormal.x == 0)
                {
                    cpNormalAbs = new Vector3(0, 0, cpNormal.z / Mathf.Abs(cpNormal.z));

                }
                else if (cpNormal.z == 0)
                {
                    cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, 0);

                }
                else
                {
                    cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));

                }
                var pEnd = intersectPolyList2[firstNum][1];    // ��_�̏I�_
                var pEdge = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][secondNum].x) , Mathf.Abs(intersectEdgList2[firstNum][secondNum].y)) ;                
                var pEdge2 = new Vector2();     // ��_�̏I�_�̕Ӄx�N�g��

                // ���_�̒ǉ�
                if (secondNum == 0)
                {
                    pEnd   = intersectPolyList2[firstNum][1];    // ��_�̏I�_
                    pEdge2 = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][1].x), Mathf.Abs(intersectEdgList2[firstNum][1].y));// ��_�̏I�_�̕Ӄx�N�g��                    
                    idxList.Add(edgIdx2List2[firstNum][2]); // ���̒ǉ�
                    idxList.Add(edgIdx2List2[firstNum][3]); // ���̒ǉ�
                }
                else if(secondNum == 1)
                {
                    pEnd = intersectPolyList2[firstNum][0];    // ��_�̏I�_
                    pEdge2 = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][0].x), Mathf.Abs(intersectEdgList2[firstNum][0].y));// ��_�̏I�_�̕Ӄx�N�g��                                
                    idxList.Add(edgIdx2List2[firstNum][0]); // ���̒ǉ�
                    idxList.Add(edgIdx2List2[firstNum][1]); // ���̒ǉ�
                }

                // �J�b�g�|�C���g�̎n�_   
                vertices1.Add(new Vector3(p.x, attachedMesh.vertices[0].y, p.y) + new Vector3(pEdge.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * -cpNormalAbs.z));
                vertices1.Add(new Vector3(p.x, attachedMesh.vertices[0].y, p.y) + new Vector3(pEdge.normalized.x * 0.04f * cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * cpNormalAbs.z));
                vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f *  -cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f *  -cpNormalAbs.z));
                vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f *  cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f *  cpNormalAbs.z));

                // �C���f�b�N�X�̊��蓖��
                int idx0 = straddlePolyIdx[(firstNum * 3)];
                int idx1 = straddlePolyIdx[(firstNum * 3) + 1];
                int idx2 = straddlePolyIdx[(firstNum * 3) + 2];
                int idx3 = vertices1.Count - 4; // 
                int idx4 = vertices1.Count - 3; //  
                int idx5 = vertices1.Count - 2; // 
                int idx6 = vertices1.Count - 1; // 
                int removeIdx = -1;
                
                // �폜����O�p�`�̌���
                for (int a = 0;a < attachedMesh.triangles.Length;a += 3)
                {
                    if (!(attachedMesh.triangles[a] == idx0 && attachedMesh.triangles[a + 1] == idx1 && attachedMesh.triangles[a + 2] == idx2)) continue;
                    removeIdx = a;
                }

                //Debug.Log("edgIdx2List2[firstNum][0]:" + edgIdx2List2[firstNum][0]);
                //Debug.Log("edgIdx2List2[firstNum][1]:"+edgIdx2List2[firstNum][1]);
                //Debug.Log("edgIdx2List2[firstNum][2]:"+edgIdx2List2[firstNum][2]);
                //Debug.Log("edgIdx2List2[firstNum][3]:"+edgIdx2List2[firstNum][3]);
                //Debug.Log("idx0" + idx0);
                //Debug.Log("idx1"+idx1);
                //Debug.Log("idx2"+idx2);

                // �C���f�b�N�X�̊���U��
                if(secondNum == 0)
                {
                    // edgIdx2List2[firstNum][0]�AedgIdx2List2[firstNum][1]���n�_�̌�_

                    // �C���f�b�N�X�̍폜
                    triangles1.RemoveRange(removeIdx, 3);

                    // 01-12�C���f�b�N�X
                    if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx1)) &&
                        ((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)))
                    {
                        Debug.Log("01-12�C���f�b�N�X");
                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx1);

                        triangles1.Add(idx6);
                        triangles1.Add(idx2);
                        triangles1.Add(idx0);

                        triangles1.Add(idx6);
                        triangles1.Add(idx0);
                        triangles1.Add(idx4);
                    }
                    // 01-02�C���f�b�N�X
                    else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx1)) &&
                             ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)))
                    {
                        Debug.Log("01-02�C���f�b�N�X");
                        triangles1.Add(idx6);
                        triangles1.Add(idx0);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx1);
                        triangles1.Add(idx2);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx1);
                      
                    }
                    // 02-12�C���f�b�N�X
                    else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)) &&
                             ((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)))
                    {
                        Debug.Log("02-12�C���f�b�N�X");
                        triangles1.Add(idx6);
                        triangles1.Add(idx2);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx0);

                        triangles1.Add(idx5);
                        triangles1.Add(idx0);
                        triangles1.Add(idx1);
                    }
                    // 02-01�C���f�b�N�X
                    else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)) &&
                             ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx1 || edgIdx2List2[firstNum][3] == idx0)))
                    {
                        Debug.Log("02-01�C���f�b�N�X");
                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx0);

                        triangles1.Add(idx6);
                        triangles1.Add(idx1);
                        triangles1.Add(idx2);

                        triangles1.Add(idx6);
                        triangles1.Add(idx2);
                        triangles1.Add(idx4);

                       
                    }
                    // 12-01�C���f�b�N�X
                    else if (((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)) &&
                             ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx1 || edgIdx2List2[firstNum][3] == idx0)))
                    {
                        Debug.Log("12-01�C���f�b�N�X");
                        triangles1.Add(idx6);
                        triangles1.Add(idx1);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx2);

                        triangles1.Add(idx5);
                        triangles1.Add(idx2);
                        triangles1.Add(idx0);
                    }
                    // 12-20�C���f�b�N�X
                    else if (((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)) &&
                             ((edgIdx2List2[firstNum][2] == idx2 || edgIdx2List2[firstNum][2] == idx0) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx2)))
                    {
                        Debug.Log(" 12-02�C���f�b�N�X");
                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx2);

                        triangles1.Add(idx6);
                        triangles1.Add(idx1);
                        triangles1.Add(idx4);

                        triangles1.Add(idx6);
                        triangles1.Add(idx0);
                        triangles1.Add(idx1);
                    }
                  

                }
                else if(secondNum == 1)
                {
                   
                    // �C���f�b�N�X�̍폜
                    triangles1.RemoveRange(removeIdx, 3);

                    // 01-12�C���f�b�N�X
                    if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx1)) &&
                             ((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)))
                    {
                        Debug.Log("01-12�C���f�b�N�X");
                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx1);

                        triangles1.Add(idx6);
                        triangles1.Add(idx2);
                        triangles1.Add(idx0);

                        triangles1.Add(idx6);
                        triangles1.Add(idx0);
                        triangles1.Add(idx4);
                    }
                    // 01-02�C���f�b�N�X
                    else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx1)) &&
                             ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)))
                    {
                        Debug.Log("01-02�C���f�b�N�X");
                        triangles1.Add(idx6);
                        triangles1.Add(idx0);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx1);
                        triangles1.Add(idx2);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx1);

                    }
                    // 02-12�C���f�b�N�X
                    else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)) &&
                             ((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)))
                    {
                        Debug.Log("02-12�C���f�b�N�X");
                        triangles1.Add(idx6);
                        triangles1.Add(idx2);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx0);

                        triangles1.Add(idx5);
                        triangles1.Add(idx0);
                        triangles1.Add(idx1);
                    }
                    // 02-01�C���f�b�N�X
                    else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)) &&
                             ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx1 || edgIdx2List2[firstNum][1] == idx0)))
                    {
                        Debug.Log("02-01�C���f�b�N�X");
                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx0);

                        triangles1.Add(idx6);
                        triangles1.Add(idx1);
                        triangles1.Add(idx2);

                        triangles1.Add(idx6);
                        triangles1.Add(idx2);
                        triangles1.Add(idx4);


                    }
                    // 12-01�C���f�b�N�X
                    else if (((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)) &&
                             ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx1 || edgIdx2List2[firstNum][1] == idx0)))
                    {
                        Debug.Log("12-01�C���f�b�N�X");
                        triangles1.Add(idx6);
                        triangles1.Add(idx1);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx2);

                        triangles1.Add(idx5);
                        triangles1.Add(idx2);
                        triangles1.Add(idx0);
                    }
                    // 12-20�C���f�b�N�X
                    else if (((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)) &&
                             ((edgIdx2List2[firstNum][0] == idx2 || edgIdx2List2[firstNum][0] == idx0) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx2)))
                    {
                        Debug.Log(" 12-02�C���f�b�N�X");
                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx2);

                        triangles1.Add(idx6);
                        triangles1.Add(idx1);
                        triangles1.Add(idx4);

                        triangles1.Add(idx6);
                        triangles1.Add(idx0);
                        triangles1.Add(idx1);
                    }
                    
                }

                // ��₩��폜
                intersectPolyList2.RemoveAt(firstNum);
                intersectEdgList2.RemoveAt(firstNum);
                edgIdx2List2.RemoveAt(firstNum);
            }

            // 2�������鏈��(�r��)
            {
                Debug.Log("����:2�������鏈��(�r��)");

                //--- �ϐ��錾 ---
                int count = 0;
                var idxCnt = straddlePolyIdx;
                idxCnt.RemoveRange(firstNum * 3, 3); // ���ۑ��p
                //Debug.Log("idxCnt.Count" + idxCnt.Count);
                //Debug.Log("idxList.Count" + idxList.Count);

                // ��_��2����|���S���̌�₪�Ȃ��Ȃ邩�J�E���g�����ȏ�ɂȂ�܂Ń��[�v
                while (count < 50 && idxCnt.Count > 0)
                {
                    bool end = false;

                    // ���̐��������[�v
                    for(int k = 0;k < idxCnt.Count;k += 3)
                    {
                        // �ӂ̐��������[�v
                        for(int h = 0;h < 3;h++)
                        {
                            // ���ƈ�v���Ȃ�������X���[�A��v�����番���Ώۂ̃C���f�b�N�X��������
                            if (!((idxCnt[k + h] == idxList[0] || idxCnt[k + h] == idxList[1]) && (idxCnt[k + ((h + 1)%3)] == idxList[0] || straddlePolyIdx[k + ((h + 1) % 3)] == idxList[1]))) continue;
                            
                            // �ۑ����ꂽ��⃊�X�g���獡��g�����C���f�b�N�X���폜
                            for(int g = 0; g < straddlePolyIdx.Count;g += 3)
                            {
                                // ���ƈ�v���Ȃ�������X���[
                                if (!(idxCnt[k] == straddlePolyIdx[g] && idxCnt[k + 1] == straddlePolyIdx[g + 1] && idxCnt[k + 2] == straddlePolyIdx[g + 2])) continue;


                                // �|���S���̃C���f�b�N�X�̍ŏ��̔ԍ�
                                firstNum = g / 3;

                                // �ϐ��錾
                                var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
                                var cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));
                                var pEnd = intersectPolyList2[firstNum][1];    // ��_�̏I�_
                                var pEdge = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][secondNum].x), Mathf.Abs(intersectEdgList2[firstNum][secondNum].y));
                                var pEdge2 = new Vector2();     // ��_�̏I�_�̕Ӄx�N�g��

                                Debug.Log("edgIdx2List2[firstNum][0]:" + edgIdx2List2[firstNum][0]);
                                Debug.Log("edgIdx2List2[firstNum][1]:" + edgIdx2List2[firstNum][1]);
                                Debug.Log("edgIdx2List2[firstNum][2]:" + edgIdx2List2[firstNum][2]);
                                Debug.Log("edgIdx2List2[firstNum][3]:" + edgIdx2List2[firstNum][3]);

                                // �ǂ�������_�̎n�_�����ׂ�
                                if ((edgIdx2List2[firstNum][0] == idxList[0] || edgIdx2List2[firstNum][0] == idxList[1]) && (edgIdx2List2[firstNum][1] == idxList[1] || edgIdx2List2[firstNum][1] == idxList[0]) )
                                {
                                    Debug.Log("���_�̒ǉ�");
                                    secondNum = 0;
                                    pEnd = intersectPolyList2[firstNum][1];    // ��_�̏I�_
                                    pEdge2 = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][1].x), Mathf.Abs(intersectEdgList2[firstNum][1].y));// ��_�̏I�_�̕Ӄx�N�g��                                                      
                                    idxList.Clear();    // ���̍폜
                                    idxList.Add(edgIdx2List2[firstNum][2]); // ���̒ǉ�
                                    idxList.Add(edgIdx2List2[firstNum][3]); // ���̒ǉ�
                                    rastIdxList.Clear();    // ���̍폜
                                    rastIdxList.Add(edgIdx2List2[firstNum][2]);// ���̒ǉ�
                                    rastIdxList.Add(edgIdx2List2[firstNum][3]);// ���̒ǉ�
                                }
                                else if ((edgIdx2List2[firstNum][2] == idxList[0] || edgIdx2List2[firstNum][2] == idxList[1]) && (edgIdx2List2[firstNum][3] == idxList[1] || edgIdx2List2[firstNum][3] == idxList[0]))
                                {
                                    Debug.Log("���_�̒ǉ�");
                                    secondNum = 1;
                                    pEnd = intersectPolyList2[firstNum][0];    // ��_�̏I�_
                                    pEdge2 = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][0].x), Mathf.Abs(intersectEdgList2[firstNum][0].y));// ��_�̏I�_�̕Ӄx�N�g��                    
                                    idxList.Clear();    // ���̍폜
                                    idxList.Add(edgIdx2List2[firstNum][0]); // ���̒ǉ�
                                    idxList.Add(edgIdx2List2[firstNum][1]); // ���̒ǉ�
                                    rastIdxList.Clear();    // ���̍폜
                                    rastIdxList.Add(edgIdx2List2[firstNum][0]);// ���̒ǉ�
                                    rastIdxList.Add(edgIdx2List2[firstNum][1]);// ���̒ǉ�
                                }

                                // ���_�̒ǉ�
                                vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f * -cpNormalAbs.z));
                                vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f * cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f * cpNormalAbs.z));

                                // �C���f�b�N�X�̊��蓖��
                                int idx0 = straddlePolyIdx[(firstNum * 3)];
                                int idx1 = straddlePolyIdx[(firstNum * 3) + 1];
                                int idx2 = straddlePolyIdx[(firstNum * 3) + 2];
                                int idx3 = vertices1.Count - 4; // 
                                int idx4 = vertices1.Count - 3; //  
                                int idx5 = vertices1.Count - 2; // 
                                int idx6 = vertices1.Count - 1; // 
                                int removeIdx = -1;

                                // �폜����O�p�`�̌���
                                for (int a = 0; a < attachedMesh.triangles.Length; a += 3)
                                {
                                    if (!(attachedMesh.triangles[a] == idx0 && attachedMesh.triangles[a + 1] == idx1 && attachedMesh.triangles[a + 2] == idx2)) continue;
                                    removeIdx = a;
                                }

                                Debug.Log("secondNum" + secondNum);
                                Debug.Log("idx0" + idx0);
                                Debug.Log("idx1" + idx1);
                                Debug.Log("idx2" + idx2);
                                Debug.Log("edgIdx2List2[firstNum][0]" + edgIdx2List2[firstNum][0]);
                                Debug.Log("edgIdx2List2[firstNum][1]" + edgIdx2List2[firstNum][1]);
                                Debug.Log("edgIdx2List2[firstNum][2]" + edgIdx2List2[firstNum][2]);
                                Debug.Log("edgIdx2List2[firstNum][3]" + edgIdx2List2[firstNum][3]);

                                // �C���f�b�N�X�̊���U��
                                if (secondNum == 0)
                                {
                                    // edgIdx2List2[firstNum][0]�AedgIdx2List2[firstNum][1]���n�_�̌�_

                                    // �C���f�b�N�X�̍폜
                                    triangles1.RemoveRange(removeIdx, 3);

                                    // 01-12�C���f�b�N�X
                                    if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx1)) &&
                                        ((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)))
                                    {
                                        Debug.Log("01-12�C���f�b�N�X");
                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx0);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx0);
                                        triangles1.Add(idx4);
                                    }
                                    // 01-02�C���f�b�N�X
                                    else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx1)) &&
                                             ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)))
                                    {
                                        Debug.Log("01-02�C���f�b�N�X");
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx0);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx1);
                                        triangles1.Add(idx2);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                    }
                                    // 02-12�C���f�b�N�X
                                    else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)) &&
                                             ((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)))
                                    {
                                        Debug.Log("02-12�C���f�b�N�X");
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx0);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx0);
                                        triangles1.Add(idx1);
                                    }
                                    // 02-01�C���f�b�N�X
                                    else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)) &&
                                             ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx1 || edgIdx2List2[firstNum][3] == idx0)))
                                    {
                                        Debug.Log("02-01�C���f�b�N�X");
                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx0);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx1);
                                        triangles1.Add(idx2);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);


                                    }
                                    // 12-01�C���f�b�N�X
                                    else if (((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)) &&
                                             ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx1 || edgIdx2List2[firstNum][3] == idx0)))
                                    {
                                        Debug.Log("12-01�C���f�b�N�X");
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx1);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx2);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx0);
                                    }
                                    // 12-20�C���f�b�N�X
                                    else if (((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)) &&
                                             ((edgIdx2List2[firstNum][2] == idx2 || edgIdx2List2[firstNum][2] == idx0) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx2)))
                                    {
                                        Debug.Log(" 12-02�C���f�b�N�X");
                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx2);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx1);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx0);
                                        triangles1.Add(idx1);
                                    }

                                }
                                else if (secondNum == 1)
                                {
                                    // �C���f�b�N�X�̍폜
                                    triangles1.RemoveRange(removeIdx, 3);

                                    // 01-12�C���f�b�N�X
                                    if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx1)) &&
                                             ((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)))
                                    {
                                        Debug.Log("01-12�C���f�b�N�X");
                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx0);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx0);
                                        triangles1.Add(idx4);
                                    }
                                    // 01-02�C���f�b�N�X
                                    else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx1)) &&
                                             ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)))
                                    {
                                        Debug.Log("01-02�C���f�b�N�X");
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx0);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx1);
                                        triangles1.Add(idx2);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                    }
                                    // 02-12�C���f�b�N�X
                                    else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)) &&
                                             ((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)))
                                    {
                                        Debug.Log("02-12�C���f�b�N�X");
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx0);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx0);
                                        triangles1.Add(idx1);
                                    }
                                    // 02-01�C���f�b�N�X
                                    else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)) &&
                                             ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx1 || edgIdx2List2[firstNum][1] == idx0)))
                                    {
                                        Debug.Log("02-01�C���f�b�N�X");
                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx0);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx1);
                                        triangles1.Add(idx2);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);


                                    }
                                    // 12-01�C���f�b�N�X
                                    else if (((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)) &&
                                             ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx1 || edgIdx2List2[firstNum][1] == idx0)))
                                    {
                                        Debug.Log("12-01�C���f�b�N�X");
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx1);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx2);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx0);
                                    }
                                    // 12-20�C���f�b�N�X
                                    else if (((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)) &&
                                             ((edgIdx2List2[firstNum][0] == idx2 || edgIdx2List2[firstNum][0] == idx0) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx2)))
                                    {
                                        Debug.Log(" 12-02�C���f�b�N�X");
                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx2);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx1);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx0);
                                        triangles1.Add(idx1);
                                    }


                                }

                                // ��₩��폜
                                idxCnt.RemoveRange(k, 3);
                                intersectPolyList2.RemoveAt(firstNum);
                                intersectEdgList2.RemoveAt(firstNum);
                                edgIdx2List2.RemoveAt(firstNum);

                                // �����܂ŗ�����I��
                                end = true;
                                break;
                            }
                            if (end) break;
                        }

                        if (end) break;
                    }

                    // �J�E���g++
                    count++;
                }
            }

            // �Ō��4�������鏈��
            {
                Debug.Log("4�������鏈��");
                //Debug.Log("edgIdx2List[0][0]:"+ edgIdx2List[0][0]);
                //Debug.Log("edgIdx2List[0][0]:"+ edgIdx2List[0][1]);
                //Debug.Log("inerPolyIdx.Count:" + inerPolyIdx.Count);
                //Debug.Log("inerPolyIdx[0]:" + inerPolyIdx[0]);
                //Debug.Log("inerPolyIdx[1]:" + inerPolyIdx[1]);
                //Debug.Log("inerPolyIdx[2]:" + inerPolyIdx[2]);

                // �Ώۂ̃C���f�b�N�X�̍폜
                for(int a = 0;a < triangles1.Count;a+=3)
                {
                    // ��v���Ȃ�������X���[
                    if (!(triangles1[a] == inerPolyIdx[0] && triangles1[a +1] == inerPolyIdx[1]&& triangles1[a +2] == inerPolyIdx[2])) continue;

                    triangles1.RemoveRange(a, 3);
                    break;
                }

                // ���_�̒ǉ�
                vertices1.Add(cutPoint[cutPoint.Count-1] -transform.position);
                vertices1.Add(cutPoint[cutPoint.Count-1] - transform.position);
                
                // �C���f�b�N�X�̊��蓖��
                int idx0 = inerPolyIdx[0];
                int idx1 = inerPolyIdx[1];
                int idx2 = inerPolyIdx[2];
                int idx3 = vertices1.Count - 4; // 
                int idx4 = vertices1.Count - 3; //  
                int idx5 = vertices1.Count - 2; // 
                int idx6 = vertices1.Count - 1; // 
                
                // ��_�̎n�_���ǂ̕ӂɂ��邩
                // ��01�̏�
                if ((edgIdx2List[0][0] == inerPolyIdx[0] || edgIdx2List[0][0] == inerPolyIdx[1])&&
                   (edgIdx2List[0][1] == inerPolyIdx[1] || edgIdx2List[0][1] == inerPolyIdx[0] ))
                {
                    Debug.Log("��01�̏�");
                    triangles1.Add(idx5);
                    triangles1.Add(idx2);
                    triangles1.Add(idx0);

                    triangles1.Add(idx5);
                    triangles1.Add(idx0);
                    triangles1.Add(idx4);

                    triangles1.Add(idx5);
                    triangles1.Add(idx3);
                    triangles1.Add(idx1);

                    triangles1.Add(idx5);
                    triangles1.Add(idx1);
                    triangles1.Add(idx2);

                    // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                    idxMemory.Clear();
                    idxMemory.Add(idx5);
                    idxMemory.Add(idx2);
                    idxMemory.Add(idx0);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx0);
                    idxMemory.Add(idx4);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx3);
                    idxMemory.Add(idx1);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx1);
                    idxMemory.Add(idx2);
                }
                // ��12�̏�
                else if ((edgIdx2List[0][0] == inerPolyIdx[1] || edgIdx2List[0][0] == inerPolyIdx[2]) &&
                   (edgIdx2List[0][1] == inerPolyIdx[2] || edgIdx2List[0][1] == inerPolyIdx[1]))
                {
                    Debug.Log("��12�̏�");
                    triangles1.Add(idx5);
                    triangles1.Add(idx0);
                    triangles1.Add(idx1);

                    triangles1.Add(idx5);
                    triangles1.Add(idx1);
                    triangles1.Add(idx4);

                    triangles1.Add(idx5);
                    triangles1.Add(idx3);
                    triangles1.Add(idx2);

                    triangles1.Add(idx5);
                    triangles1.Add(idx2);
                    triangles1.Add(idx0);

                    // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                    idxMemory.Clear();
                    idxMemory.Add(idx5);
                    idxMemory.Add(idx0);
                    idxMemory.Add(idx1);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx1);
                    idxMemory.Add(idx4);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx3);
                    idxMemory.Add(idx2);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx2);
                    idxMemory.Add(idx0);
                }
                // ��20�̏�
                else if ((edgIdx2List[0][0] == inerPolyIdx[2] || edgIdx2List[0][0] == inerPolyIdx[0]) &&
                   (edgIdx2List[0][1] == inerPolyIdx[0] || edgIdx2List[0][1] == inerPolyIdx[2]))
                {
                    Debug.Log("��20�̏�");
                    triangles1.Add(idx5);
                    triangles1.Add(idx1);
                    triangles1.Add(idx2);

                    triangles1.Add(idx5);
                    triangles1.Add(idx2);
                    triangles1.Add(idx4);

                    triangles1.Add(idx5);
                    triangles1.Add(idx3);
                    triangles1.Add(idx0);

                    triangles1.Add(idx5);
                    triangles1.Add(idx0);
                    triangles1.Add(idx1);

                    // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                    idxMemory.Clear();
                    idxMemory.Add(idx5);
                    idxMemory.Add(idx1);
                    idxMemory.Add(idx2);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx2);
                    idxMemory.Add(idx4);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx3);
                    idxMemory.Add(idx0);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx0);
                    idxMemory.Add(idx1);
                }
            }
        }
          
        // ������̃I�u�W�F�N�g�����A���낢��Ƃ����
        var normal = new List<Vector3>();
        for (int i = 0; i < vertices1.Count; i++)
        {
            normal.Add(Vector3.up);
        }

        // ���b�V���ɑ��
        attachedMesh.SetVertices(vertices1.ToArray());
        attachedMesh.SetTriangles(triangles1.ToArray(), 0);
        attachedMesh.SetNormals(normal);

        var mesh = new Mesh();
        mesh.vertices = vertices1.ToArray();
        mesh.triangles = triangles1.ToArray();
        mesh.normals = normals1.ToArray();

        attachedMesh = mesh;
    }

    // �r���̃J�b�g�|�C���g�ł̕���
    public bool DiviosionMeshMiddle(List<Vector3> cutPoint)
    {
        Debug.Log("============ �r���̏��� ============");
        if (cutPoint.Count < 3) return false;

        // ���b�V���̃A�^�b�`
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;

        // �ϐ�
        Vector3 p0, p1, p2;    // ���b�V���̃|���S���̒��_
        var uvs1 = new List<Vector2>(); // �e�N�X�`��
        var vertices1 = new List<Vector3>();   // ���_
        var triangles1 = new List<int>();       // �O�p�`�C���f�b�N�X
        var normals1 = new List<Vector3>();     // �@��
                                                //var normals2 = new List<Vector3>();
        Vector3 edge = new Vector3();
        Vector3 edge1 = new Vector3();
        Vector3 edge2 = new Vector3();
        Vector3 edge3 = new Vector3();


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

            // �؂�����ɑ΂��ē_���ړ�����߂̏���
            edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 3];
            edge2 = cutPoint[cutPoint.Count - 1] - cutPoint[cutPoint.Count - 2];
            edge3 = edge1 + edge2;

            // �J�b�g�|�C���g���꒼����������
            // �����ɓ_���L����
            if (edge3 == Vector3.zero)
            {
                edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
                edge2 = (cutPoint[cutPoint.Count - 1] + Vector3.up) - cutPoint[cutPoint.Count - 1];
                edge = Vector3.Cross(edge2, edge1);
            }
            else
            {
                edge = Vector3.Cross(edge3, Vector3.up);

            }

            // ���_�Ɋi�[
            vertices1[i] = vertices1[i] + edge.normalized * 0.04f;
            vertices1[i + 1] = vertices1[i + 1] - edge.normalized * 0.04f;
        }

        // �J�b�g���鏈��(�S��)
        {
            // �ϐ��錾              
            var straddlePolyIdx = new List<int>();  // �܂������|���S���ԍ����X�g
            var crossPolyIdx = new List<int>();     // �����|���S���ԍ����X�g
            var inerPolyIdx = new List<int>();      // �J�b�g�|�C���g�����ɓ����Ă���|���S���ԍ�
            var intersectPolyList = new List<List<Vector2>>();  // �|���S�����Ƃɂ���������Ă���_�̃��X�g
            var intersectPolyList2 = new List<List<Vector2>>();  // �|���S�����Ƃɂ���������Ă���_�̃��X�g2
            var intersectEdgList = new List<List<Vector2>>();   // �|���S�����Ƃɂ���������Ă���ӂ̃��X�g
            var intersectEdgList2 = new List<List<Vector2>>();   // �|���S�����Ƃɂ���������Ă���ӂ̃��X�g
            var intersectionList = new List<Vector2>();         // ��_�̃��X�g
            var cp_s = new Vector2(cutPoint[cutPoint.Count - 2].x, cutPoint[cutPoint.Count - 2].z);    // �J�b�g�|�C���g�̏I�_��1�O
            var cp_v = new Vector2(cutPoint[cutPoint.Count - 1].x, cutPoint[cutPoint.Count - 1].z);    // �J�b�g�|�C���g�̏I�_
            var cpEdg = cp_v - cp_s;    // �J�b�g�|�C���g�̏I�_�ƃJ�b�g�|�C���g�̏I�_��1�O���Ȃ�����
            var checkCp = cp_s + cpEdg * 0.01f; // �J�b�g�|�C���g�̏I�_��1�O����J�b�g�|�C���g�̏I�_�̕����ɂ�����ƐL�΂����_
            var edgIdx2List = new List<List<int>>();   // �ӂ̃C���f�b�N�X�̃��X�g�̃��X�g   
            var edgIdx2List2 = new List<List<int>>();  // �ӂ̃C���f�b�N�X�̃��X�g�̃��X�g2   

            // �܂����ł�|���S���ƐN�����Ă���|���S���������邩�T��
            for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
            {
                // �ϐ��錾             
                int interPointCnt = 0; // ���������_�̐�
                var intersection = new List<Vector2>(); // ��_�̃��X�g
                var edgList = new List<Vector2>(); //�ӂ̃��X�g
                var edgIdxList = new List<int>();   // �ӂ̃C���f�b�N�X�̃��X�g   

                // �|���S���̕ӂ̐��������[�v
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

                    // �J�b�g�|�C���g�̎n�_�̕␳
                    cpVtx_s += cpEdge * 0.01f;

                    // �J�b�g�|�C���g�̕ӂ̕␳
                    cpEdge = cpVtx_v - cpVtx_s; // ��

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
                        // ������ĂȂ��Ƃ��X���[
                        continue;
                    }
                    else
                    {
                        // ������Ă鎞��_�J�E���g++                               
                        interPointCnt++;    // ��_�J�E���g    
                        intersection.Add(p);    // ��_�̕ۑ�
                        intersectionList.Add(p);// ��_�̕ۑ�
                        edgList.Add(polyEdge);
                        edgIdxList.Add(attachedMesh.triangles[j + k]);
                        edgIdxList.Add(attachedMesh.triangles[j + (k + 1) % 3]);

                    }
                }

                // �|���S���ԍ���ۑ�
                if (interPointCnt == 2)// ��_�J�E���g2��(�|���S�����܂����ł鎞)
                {
                    Debug.Log("2�����");
                    //Debug.Log("�|���S���ԍ���" + attachedMesh.triangles[j] + "," + attachedMesh.triangles[j + 1] + "," + attachedMesh.triangles[j + 2]);

                    straddlePolyIdx.Add(attachedMesh.triangles[j]);
                    straddlePolyIdx.Add(attachedMesh.triangles[j + 1]);
                    straddlePolyIdx.Add(attachedMesh.triangles[j + 2]);
                    crossPolyIdx.Add(j);
                    crossPolyIdx.Add(j + 1);
                    crossPolyIdx.Add(j + 2);
                    intersectPolyList2.Add(intersection);
                    intersectEdgList2.Add(edgList);
                    edgIdx2List2.Add(edgIdxList);
                    //Debug.Log("straddlePolyIdx.Count" + straddlePolyIdx.Count);
                }
                else if (interPointCnt == 1)// ��_�J�E���g1��(�J�b�g�|�C���g�̏I�_���|���S���̒��ɂ���Ƃ�)
                {
                    Debug.Log("1�����");
                    inerPolyIdx.Add(attachedMesh.triangles[j]);
                    inerPolyIdx.Add(attachedMesh.triangles[j + 1]);
                    inerPolyIdx.Add(attachedMesh.triangles[j + 2]);
                    crossPolyIdx.Add(j);
                    crossPolyIdx.Add(j + 1);
                    crossPolyIdx.Add(j + 2);
                    edgIdx2List.Add(edgIdxList);
                    intersectPolyList.Add(intersection);
                    intersectEdgList.Add(edgList);
                }
                else
                {
                     //Debug.Log("3�����");
                    // Debug.Log("�|���S���ԍ���" + attachedMesh.triangles[j] + "," + attachedMesh.triangles[j + 1] + "," + attachedMesh.triangles[j + 2]);

                }
            }

            //--- �J�b�g�|�C���g�Ƃ̌�_�̐��ŕ��� ---
            // ��_��1�̎�(�|���S���̒���2��_������|���S����1���Ȃ��Ƃ�)
            if (straddlePolyIdx.Count == 0 && inerPolyIdx.Count > 0)
            {
                Debug.Log("=============����:��_��1��=============");

                //--- �ϐ��錾 ---
                int firstNum = 0;
                int secondNum = 0;
                Vector2 cpS = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z);
                Vector2 cpV = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z);
                Vector2 p = cpV;
                var idxList = new List<int>();  // 1�O�ɕ��������C���f�b�N�X�̃��X�g
                var rastIdxList = new List<int>();  // 2�ɕ������鏈���������Ō�̃C���f�b�N�X�ۑ��p�̃��X�g             

                // 2�������鏈��
                {
                    //Debug.Log("2�������鏈��");
                    //Debug.Log("inerPolyIdx.Count:" + inerPolyIdx.Count);
                    //Debug.Log("intersectPolyList.Count:" + intersectPolyList.Count);
                    //Debug.Log("intersectEdgList.Count:" + intersectEdgList.Count);


                    //// �L�����ꂽ�O�p�`�C���f�b�N�X�̐��������[�v
                    //for (int a = 0; a < idxMemory.Count; a += 3)
                    //{                       
                    //    // �����Ώۂ̃|���S���̐��������[�v
                    //    for (int w = 0;w < inerPolyIdx.Count;w += 3)
                    //    {                                                    
                    //        // �L�����ꂽ�C���f�b�N�X�ƈ�v���Ȃ�������X���[
                    //        if (!(inerPolyIdx[w] ==  idxMemory[a] && inerPolyIdx[w + 1] == idxMemory[a + 1] && inerPolyIdx[w + 2] == idxMemory[a + 2])) continue;
                    //        Debug.Log("intersectPolyList[w/3][0]:" + intersectPolyList[w / 3][0]);
                    //        Debug.Log("intersectEdgList[w/3][0]:" + intersectEdgList[w / 3][0]);

                    //        var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
                    //        var cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));
                    //        var pEnd = intersectPolyList[w / 3][0];    // ��_�̏I�_
                    //        var pEdge = new Vector2(Mathf.Abs(intersectEdgList[w/3][0].x), Mathf.Abs(intersectEdgList[w/3][0].y));
                    //        var pEdge2 = new Vector2();     // ��_�̏I�_�̕Ӄx�N�g��

                    //        for (int c = 0; c < triangles1.Count; c += 3)
                    //        {

                    //            // ��v���Ȃ�������X���[
                    //            if (!(triangles1[c] == inerPolyIdx[w] && triangles1[c + 1] == inerPolyIdx[w+1] && triangles1[c + 2] == inerPolyIdx[w+2])) continue;
                    //            Debug.Log("���_�̒ǉ�");
                    //            // ��_�����Ƃɒ��_��ǉ�
                    //            vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f * -cpNormalAbs.z));
                    //            vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f * cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f * cpNormalAbs.z));

                    //            // �C���f�b�N�X�̊��蓖��
                    //            int idx0 = attachedMesh.triangles[c];
                    //            int idx1 = attachedMesh.triangles[c + 1];
                    //            int idx2 = attachedMesh.triangles[c + 2];
                    //            int idx3 = vertices1.Count - 2; // 7
                    //            int idx4 = vertices1.Count - 1; // �g��Ȃ�  
                    //            int idx5 = vertices1.Count - 3; // 6


                    //            if (a == 0)
                    //            {                                    
                    //                Debug.Log("a = 0");                                                                                                          

                    //                // �C���f�b�N�X�̕ύX
                    //                triangles1[c + 3] = idx5;

                    //                // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�       
                    //                triangles1.RemoveRange(c, 3);
                    //                triangles1.Add(idx4);
                    //                triangles1.Add(idx2);
                    //                triangles1.Add(idx5);

                    //                triangles1.Add(idx3);
                    //                triangles1.Add(idx0);
                    //                triangles1.Add(idx1);

                    //                break;
                    //            }
                    //            if (a == 3)
                    //            {
                    //                Debug.Log("a = 3");

                    //                // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                    //                triangles1.RemoveRange(c, 3);
                    //                triangles1.Add(idx4);
                    //                triangles1.Add(idx2);
                    //                triangles1.Add(idx5);

                    //                triangles1.Add(idx3);
                    //                triangles1.Add(idx0);
                    //                triangles1.Add(idx1);

                    //                break;
                    //            }
                    //            if (a == 6)
                    //            {
                    //                Debug.Log("a = 6");

                    //                // �C���f�b�N�X�̕ύX
                    //                triangles1[c - 6] = idx5;
                    //                triangles1[c - 3] = idx5;

                    //                // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                    //                triangles1.RemoveRange(c, 3);
                    //                triangles1.Add(idx4);
                    //                triangles1.Add(idx2);
                    //                triangles1.Add(idx5);

                    //                triangles1.Add(idx3);
                    //                triangles1.Add(idx0);
                    //                triangles1.Add(idx1);

                    //                // �����܂ŗ�����O�p�`��񓙕�����̂͏I��
                    //                break;
                    //            }

                    //        }
                    //        break;
                    //    }

                    //}
                }

                // 2�������鏈��(�ŏ�)
                {
                    Debug.Log("2�������鏈��");

                    //--- �L�����ꂽ�O�p�`�C���f�N�X�����ƂɃC���f�b�N�X������U�� ---
                    // �L�����ꂽ�O�p�`�C���f�b�N�X�̐��������[�v
                    for (int a = 0; a < idxMemory.Count; a += 3)
                    {
                        bool end = false;
                        // �����Ώۂ̃|���S���̐��������[�v
                        for (int w = 0; w < inerPolyIdx.Count; w += 3)
                        {
                            // �L�����ꂽ�C���f�b�N�X�ƈ�v���Ȃ�������X���[
                            if (!(inerPolyIdx[w] == idxMemory[a] && inerPolyIdx[w + 1] == idxMemory[a + 1] && inerPolyIdx[w + 2] == idxMemory[a + 2])) continue;
                            Debug.Log("intersectPolyList[w/3][0]:" + intersectPolyList[w / 3][0]);
                            Debug.Log("intersectEdgList[w/3][0]:" + intersectEdgList[w / 3][0]);

                            //--- �ϐ��錾 ---
                            var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);

                            var cpNormalAbs = new Vector3();
                            if(cpNormal.x == 0)
                            {
                                cpNormalAbs = new Vector3(0, 0, cpNormal.z / Mathf.Abs(cpNormal.z));

                            }
                            else if(cpNormal.z == 0)
                            {
                                cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, 0);

                            }
                            else
                            {
                                cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));

                            }
                            var pEnd = intersectPolyList[w / 3][0];    // ��_�̏I�_
                            var pEdge = new Vector2(Mathf.Abs(intersectEdgList[w / 3][0].x), Mathf.Abs(intersectEdgList[w / 3][0].y));

                            // ���ɒǉ�
                            idxList.Add(edgIdx2List[w / 3][0]);
                            idxList.Add(edgIdx2List[w / 3][1]);

                            // ���ǉ����ꂽ���ƌ�_�������|���S���̕ӂƔ�r
                            for (int g = 0; g < straddlePolyIdx.Count; g += 3)
                            {
                                bool end2 = false;
                                // �ӂ̐��������[�v
                                for (int f = 0; f < 3; f++)
                                {
                                    // ��v���Ȃ�������X���[
                                    if (((straddlePolyIdx[g + f] == idxList[0] || straddlePolyIdx[g + f] == idxList[1]) && (straddlePolyIdx[g + (f + 1) % 3] == idxList[1] || straddlePolyIdx[g + (f + 1) % 3] == idxList[0]))) continue;

                                    Debug.Log("���ɒǉ�");
                                    firstNum = g;
                                    end2 = true;
                                    break;
                                }
                                if (end2) break;
                            }

                            // ���b�V���̃|���S���̐��������[�v
                            for (int c = 0; c < triangles1.Count; c += 3)
                            {
                                // ��v���Ȃ�������X���[
                                if (!(triangles1[c] == inerPolyIdx[w] && triangles1[c + 1] == inerPolyIdx[w + 1] && triangles1[c + 2] == inerPolyIdx[w + 2])) continue;
                                Debug.Log("���_�̒ǉ�");
                                Debug.Log("pEnd"+ pEnd);
                                Debug.Log("pEdge" + pEdge);
                                Debug.Log("" + ( pEdge.normalized.y * 0.079f * -cpNormalAbs.z));
                                Debug.Log("cpNormal" + (cpNormal));
                                
                                // ��_�����Ƃɒ��_��ǉ�
                                vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge.normalized.x * 0.079f * -cpNormalAbs.x, 0, pEdge.normalized.y * 0.079f * -cpNormalAbs.z));
                                vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge.normalized.x * 0.079f * cpNormalAbs.x, 0, pEdge.normalized.y * 0.079f * cpNormalAbs.z));

                                // �C���f�b�N�X�̊��蓖��
                                int idx0 = attachedMesh.triangles[c];
                                int idx1 = attachedMesh.triangles[c + 1];
                                int idx2 = attachedMesh.triangles[c + 2];
                                int idx3 = vertices1.Count - 4; // 7
                                int idx4 = vertices1.Count - 3; // 
                                int idx5 = vertices1.Count - 2; // 6
                                int idx6 = vertices1.Count - 1; // 6
                                int here = 0;   // �L�����ꂽ�C���f�b�N�X�̐擪���i�[����ϐ�

                                // ��v�����L�����ꂽ�擪�̃C���f�b�N�X���S�̂̂ǂ̃C���f�b�N�X�ɂ���̂�
                                for (int z = 0; z < triangles1.Count; z += 3)
                                {
                                    // ��v���Ȃ�������X���[
                                    if (!(idxMemory[a] == triangles1[z] && idxMemory[a + 1] == triangles1[z + 1] && idxMemory[a + 2] == triangles1[z + 2])) continue;

                                    // �L�����ꂽ�C���f�b�N�X�̐擪���i�[
                                    here = z - a;
                                }

                                Debug.Log("idxMemory.Count" + idxMemory.Count);
                                // �L�����ꂽ�C���f�b�N�X�̐��ɂ���ĕ���
                                // �L�����ꂽ�C���f�b�N�X��12��(�|���S����4��)�̎�
                                if (idxMemory.Count == 12)
                                {
                                    // �C���f�b�N�X�̊���U�蕪��
                                    if (a == 0)
                                    {
                                        Debug.Log("a = 0");

                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here + 3] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                    if (a == 3)
                                    {
                                        Debug.Log("a = 3");

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                    if (a == 6)
                                    {
                                        Debug.Log("a = 6");

                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;
                                        triangles1[here + 9] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        // �����܂ŗ�����O�p�`��񓙕�����̂͏I��
                                        end = true;
                                        break;
                                    }
                                    if (a == 9)
                                    {
                                        Debug.Log("a = 9");
                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        // �����܂ŗ�����O�p�`��񓙕�����̂͏I��
                                        end = true;
                                        break;
                                    }
                                }
                                // �L�����ꂽ�C���f�b�N�X��9��(�|���S����3��)�̎�
                                else if (idxMemory.Count == 9)
                                {
                                    // �C���f�b�N�X�̊���U�蕪��
                                    if (a == 0)
                                    {
                                        Debug.Log("a = 0");

                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here + 3] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                    if (a == 3)
                                    {
                                        Debug.Log("a = 3");

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                    if (a == 6)
                                    {
                                        Debug.Log("a = 6");

                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }

                                }


                            }

                            // ��_����̃|���S���̌��̒����獡��Ŏg�����|���S�����폜
                            //inerPolyIdx.RemoveRange(w, 3);
                            //edgIdx2List.RemoveRange(w/3, 1);
                            //intersectPolyList.RemoveRange(w/3, 1);
                            //intersectEdgList.RemoveRange(w/3, 1);

                            break;
                        }

                    }
                }

                // 4�������鏈��
                {
                    Debug.Log("4�������鏈��");
                    Debug.Log("inerPolyIdx.Count:" + inerPolyIdx.Count);

                    //Debug.Log("edgIdx2List[0][0]:" + edgIdx2List[0][0]);
                    //Debug.Log("edgIdx2List[0][0]:" + edgIdx2List[0][1]);
                    //Debug.Log("inerPolyIdx.Count:" + inerPolyIdx.Count);
                    //Debug.Log("inerPolyIdx[0]:" + inerPolyIdx[0]);
                    //Debug.Log("inerPolyIdx[1]:" + inerPolyIdx[1]);
                    //Debug.Log("inerPolyIdx[2]:" + inerPolyIdx[2]);

                    // �Ώۂ̃C���f�b�N�X�̍폜
                    for (int a = 0; a < triangles1.Count; a += 3)
                    {
                        // ��v���Ȃ�������X���[
                        if (!(triangles1[a] == inerPolyIdx[0] && triangles1[a + 1] == inerPolyIdx[1] && triangles1[a + 2] == inerPolyIdx[2])) continue;

                        triangles1.RemoveRange(a, 3);
                        break;
                    }

                    // ���_�̒ǉ�
                    vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);
                    vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);

                    // �C���f�b�N�X�̊��蓖��
                    int idx0 = inerPolyIdx[0];
                    int idx1 = inerPolyIdx[1];
                    int idx2 = inerPolyIdx[2];
                    int idx3 = vertices1.Count - 4; // 
                    int idx4 = vertices1.Count - 3; //  
                    int idx5 = vertices1.Count - 2; // 
                    int idx6 = vertices1.Count - 1; // 

                    // ��_�̎n�_���ǂ̕ӂɂ��邩
                    // ��01�̏�
                    if ((edgIdx2List[0][0] == inerPolyIdx[0] || edgIdx2List[0][0] == inerPolyIdx[1]) &&
                       (edgIdx2List[0][1] == inerPolyIdx[1] || edgIdx2List[0][1] == inerPolyIdx[0]))
                    {
                        Debug.Log("��01�̏�");
                        triangles1.Add(idx5);
                        triangles1.Add(idx2);
                        triangles1.Add(idx0);

                        triangles1.Add(idx5);
                        triangles1.Add(idx0);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx1);

                        triangles1.Add(idx5);
                        triangles1.Add(idx1);
                        triangles1.Add(idx2);

                        // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                        idxMemory.Clear();
                        idxMemory.Add(idx5);
                        idxMemory.Add(idx2);
                        idxMemory.Add(idx0);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx0);
                        idxMemory.Add(idx4);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx3);
                        idxMemory.Add(idx1);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx1);
                        idxMemory.Add(idx2);
                    }
                    // ��12�̏�
                    else if ((edgIdx2List[0][0] == inerPolyIdx[1] || edgIdx2List[0][0] == inerPolyIdx[2]) &&
                       (edgIdx2List[0][1] == inerPolyIdx[2] || edgIdx2List[0][1] == inerPolyIdx[1]))
                    {
                        Debug.Log("��12�̏�");
                        triangles1.Add(idx5);
                        triangles1.Add(idx0);
                        triangles1.Add(idx1);

                        triangles1.Add(idx5);
                        triangles1.Add(idx1);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx2);

                        triangles1.Add(idx5);
                        triangles1.Add(idx2);
                        triangles1.Add(idx0);

                        // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                        idxMemory.Clear();
                        idxMemory.Add(idx5);
                        idxMemory.Add(idx0);
                        idxMemory.Add(idx1);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx1);
                        idxMemory.Add(idx4);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx3);
                        idxMemory.Add(idx2);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx2);
                        idxMemory.Add(idx0);
                    }
                    // ��20�̏�
                    else if ((edgIdx2List[0][0] == inerPolyIdx[2] || edgIdx2List[0][0] == inerPolyIdx[0]) &&
                       (edgIdx2List[0][1] == inerPolyIdx[0] || edgIdx2List[0][1] == inerPolyIdx[2]))
                    {
                        Debug.Log("��20�̏�");
                        triangles1.Add(idx5);
                        triangles1.Add(idx1);
                        triangles1.Add(idx2);

                        triangles1.Add(idx5);
                        triangles1.Add(idx2);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx0);

                        triangles1.Add(idx5);
                        triangles1.Add(idx0);
                        triangles1.Add(idx1);

                        // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                        idxMemory.Clear();
                        idxMemory.Add(idx5);
                        idxMemory.Add(idx1);
                        idxMemory.Add(idx2);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx2);
                        idxMemory.Add(idx4);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx3);
                        idxMemory.Add(idx0);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx0);
                        idxMemory.Add(idx1);
                    }

                }
            }
            // ��_��2�ȏ�̎�
            else if(straddlePolyIdx.Count > 0)
            {
                Debug.Log("=============����:��_��2�ȏ�=============");
                //Debug.Log("straddlePolyIdx.Count"+straddlePolyIdx.Count);
                //--- �ϐ��錾 ---
                int firstNum = 0;
                int secondNum = 0;
                Vector2 cpS = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z);
                Vector2 cpV = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z);
                Vector2 p = cpV;
                var idxList = new List<int>();  // 1�O�ɕ��������C���f�b�N�X�̃��X�g
                var rastIdxList = new List<int>();  // 2�ɕ������鏈���������Ō�̃C���f�b�N�X�ۑ��p�̃��X�g             

                // 2�������鏈��(�ŏ�)
                {
                    Debug.Log("2�������鏈��(�ŏ�)");
                  
                    //--- �L�����ꂽ�O�p�`�C���f�N�X�����ƂɃC���f�b�N�X������U�� ---
                    // �L�����ꂽ�O�p�`�C���f�b�N�X�̐��������[�v
                    for (int a = 0; a < idxMemory.Count; a += 3)
                    {
                        bool end = false;
                        // �����Ώۂ̃|���S���̐��������[�v
                        for (int w = 0; w < inerPolyIdx.Count; w += 3)
                        {
                            // �L�����ꂽ�C���f�b�N�X�ƈ�v���Ȃ�������X���[
                            if (!(inerPolyIdx[w] == idxMemory[a] && inerPolyIdx[w + 1] == idxMemory[a + 1] && inerPolyIdx[w + 2] == idxMemory[a + 2])) continue;
                            //Debug.Log("intersectPolyList[w/3][0]:" + intersectPolyList[w / 3][0]);
                            //Debug.Log("intersectEdgList[w/3][0]:" + intersectEdgList[w / 3][0]);

                            //--- �ϐ��錾 ---
                            var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
                            var cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));
                            var pEnd = intersectPolyList[w / 3][0];    // ��_�̏I�_
                            var pEdge = new Vector2(Mathf.Abs(intersectEdgList[w / 3][0].x), Mathf.Abs(intersectEdgList[w / 3][0].y));
                           
                            // ���ɒǉ�
                            idxList.Add(edgIdx2List[w/3][0]);
                            idxList.Add(edgIdx2List[w/3][1]);

                            // ���ǉ����ꂽ���ƌ�_�������|���S���̕ӂƔ�r
                            for(int g = 0;g < straddlePolyIdx.Count;g += 3)
                            {
                                bool end2 = false;
                                // �ӂ̐��������[�v
                                for(int f = 0;f < 3;f++)
                                {
                                    // ��v���Ȃ�������X���[
                                    if (((straddlePolyIdx[g + f] == idxList[0] || straddlePolyIdx[g + f] == idxList[1]) && (straddlePolyIdx[g + (f + 1)%3] == idxList[1] || straddlePolyIdx[g + (f + 1) % 3] == idxList[0]))) continue;

                                    Debug.Log("���ɒǉ�");
                                    firstNum = g;
                                    end2 = true;
                                    break;
                                }
                                if (end2) break;
                            }
                          
                            // ���b�V���̃|���S���̐��������[�v
                            for (int c = 0; c < triangles1.Count; c += 3)
                            {
                                // ��v���Ȃ�������X���[
                                if (!(triangles1[c] == inerPolyIdx[w] && triangles1[c + 1] == inerPolyIdx[w + 1] && triangles1[c + 2] == inerPolyIdx[w + 2])) continue;
                                Debug.Log("���_�̒ǉ�");
                                // ��_�����Ƃɒ��_��ǉ�
                                vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * -cpNormalAbs.z));
                                vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge.normalized.x * 0.04f *  cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f *  cpNormalAbs.z));

                                // �C���f�b�N�X�̊��蓖��
                                int idx0 = attachedMesh.triangles[c];
                                int idx1 = attachedMesh.triangles[c + 1];
                                int idx2 = attachedMesh.triangles[c + 2];
                                int idx3 = vertices1.Count - 4; // 7
                                int idx4 = vertices1.Count - 3; // 
                                int idx5 = vertices1.Count - 2; // 6
                                int idx6 = vertices1.Count - 1; // 6
                                int here = 0;   // �L�����ꂽ�C���f�b�N�X�̐擪���i�[����ϐ�

                                // ��v�����L�����ꂽ�擪�̃C���f�b�N�X���S�̂̂ǂ̃C���f�b�N�X�ɂ���̂�
                                for(int z = 0;z < triangles1.Count;z+=3)
                                {
                                    // ��v���Ȃ�������X���[
                                    if (!(idxMemory[a] == triangles1[z] && idxMemory[a + 1] == triangles1[z + 1] && idxMemory[a + 2] == triangles1[z + 2])) continue;

                                    // �L�����ꂽ�C���f�b�N�X�̐擪���i�[
                                    here = z - a;
                                }

                                //Debug.Log("idxMemory.Count" + idxMemory.Count);
                                // �L�����ꂽ�C���f�b�N�X�̐��ɂ���ĕ���
                                // �L�����ꂽ�C���f�b�N�X��12��(�|���S����4��)�̎�
                                if (idxMemory.Count == 12)   
                                {                                    
                                    // �C���f�b�N�X�̊���U�蕪��
                                    if (a == 0)
                                    {
                                        Debug.Log("a = 0");

                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here + 3] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                    if (a == 3)
                                    {
                                        Debug.Log("a = 3");

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                    if (a == 6)
                                    {
                                        Debug.Log("a = 6");

                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;
                                        triangles1[here + 9] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        // �����܂ŗ�����O�p�`��񓙕�����̂͏I��
                                        end = true;
                                        break;
                                    }
                                    if (a == 9)
                                    {
                                        Debug.Log("a = 9");
                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;
                                        
                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        // �����܂ŗ�����O�p�`��񓙕�����̂͏I��
                                        end = true;
                                        break;
                                    }
                                }
                                // �L�����ꂽ�C���f�b�N�X��9��(�|���S����3��)�̎�
                                else if (idxMemory.Count == 9)
                                {
                                    // �C���f�b�N�X�̊���U�蕪��
                                    if (a == 0)
                                    {
                                        Debug.Log("a = 0");

                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here + 3] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                    if (a == 3)
                                    {
                                        Debug.Log("a = 3");
                                       
                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                    if (a == 6)
                                    {
                                        Debug.Log("a = 6");

                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                   
                                }

                               
                            }

                            // ��_����̃|���S���̌��̒����獡��Ŏg�����|���S�����폜
                            //inerPolyIdx.RemoveRange(w, 3);
                            //edgIdx2List.RemoveRange(w/3, 1);
                            //intersectPolyList.RemoveRange(w/3, 1);
                            //intersectEdgList.RemoveRange(w/3, 1);
                            if(end)break;
                        }
                        if (end) break;
                    }
                }

                // �L�����ꂽ�C���f�b�N�X�ƈ�v���Ȃ�������
                if(idxList.Count == 0)
                {
                    Debug.Log("2�������鏈��(�ŏ�)���s���Ȃ�������");
                    var point = intersectPolyList2[0][0];
                    int first = 0;
                    int second = 0;
                    for(int h = 0;h < straddlePolyIdx.Count;h+=3)
                    {
                        for(int v = 0;v < 2;v++)
                        {
                            if (Vector2.Distance(new Vector2(cutPoint[cutPoint.Count - 1].x, cutPoint[cutPoint.Count - 1].z), point) > Vector2.Distance(new Vector2(cutPoint[cutPoint.Count - 1].x, cutPoint[cutPoint.Count - 1].z), intersectPolyList2[h/3][v])) continue;
                            point = intersectPolyList2[h/3][v];
                            first = h;
                            second = v;
                        }
                    }

                    // �n�_���ǂ����ɂ��邩�ŕ���
                    if(second == 0)
                    {
                       
                        // ���ɒǉ�
                        idxList.Add(edgIdx2List2[first/3][2]);
                        idxList.Add(edgIdx2List2[first/3][3]);

                    }
                    else if(second == 1)
                    {
                        // ���ɒǉ�
                        idxList.Add(edgIdx2List2[first / 3][0]);
                        idxList.Add(edgIdx2List2[first / 3][1]);

                    }


                }


                // 2�������鏈��(�r��)
                {
                    Debug.Log("2�������鏈��(�r��)");

                    //--- �ϐ��錾 ---
                    int count = 0;
                    var idxCnt = straddlePolyIdx;   // �܂������Ă�|���S���̃��X�g����
                    //idxCnt.RemoveRange(firstNum * 3, 3); // ���ۑ��p
                                                         //Debug.Log("idxCnt.Count" + idxCnt.Count);
                    Debug.Log("���̐�" + idxCnt.Count);

                    //Debug.Log("idxCnt.Count" + idxCnt.Count);
                    
                  
                    // ��_��2����|���S���̌�₪�Ȃ��Ȃ邩�J�E���g�����ȏ�ɂȂ�܂Ń��[�v
                    while (count < 50 && idxCnt.Count > 0)
                    {
                        bool end = false;
                        Debug.Log("=============== ���[�v ==============");
                        for (int z = 0; z < intersectPolyList2.Count; z++)
                        {
                            for (int y = 0; y < intersectPolyList2[z].Count; y++)
                            {
                                Debug.Log("intersectPolyList2[z][y]  " + intersectPolyList2[z][y]);

                            }

                        }
                        // ���̐��������[�v
                        for (int k = 0; k < idxCnt.Count; k += 3)
                        {
                            // �ӂ̐��������[�v
                            for (int h = 0; h < 3; h++)
                            {
                                //Debug.Log("idxCnt[k + h];" + idxCnt[k + h]);
                                //Debug.Log("idxCnt[k + ((h + 1) % 3)];" + idxCnt[k + ((h + 1) % 3)]);
                                //Debug.Log("idxList[0];" + idxList[0]);
                                //Debug.Log("idxList[1];" + idxList[1]);
                                ////// ���ƈ�v���Ȃ�������X���[�A��v�����番���Ώۂ̃C���f�b�N�X��������
                                if (!((idxCnt[k + h] == idxList[0] || idxCnt[k + h] == idxList[1]) && (idxCnt[k + ((h + 1) % 3)] == idxList[0] || straddlePolyIdx[k + ((h + 1) % 3)] == idxList[1]))) continue;

                                // �ۑ����ꂽ��⃊�X�g���獡��g�����C���f�b�N�X���폜
                                for (int g = 0; g < straddlePolyIdx.Count; g += 3)
                                {
                                    // ���ƈ�v���Ȃ�������X���[
                                    if (!(idxCnt[k] == straddlePolyIdx[g] && idxCnt[k + 1] == straddlePolyIdx[g + 1] && idxCnt[k + 2] == straddlePolyIdx[g + 2])) continue;

                                    //Debug.Log("====== ���̃��X�g ======");
                                    //for (int z = 0; z < idxCnt.Count; z += 3)
                                    //{
                                    //    Debug.Log("idxCnt[z]  " + idxCnt[z]);
                                    //    Debug.Log("idxCnt[z+1]" + idxCnt[z + 1]);
                                    //    Debug.Log("idxCnt[z+2]" + idxCnt[z + 2]);
                                    //}
                                    //Debug.Log("====== ���̃��X�g ======");
                                    //Debug.Log("straddlePolyIdx.Count  " + straddlePolyIdx.Count);
                                    //for (int z = 0; z < straddlePolyIdx.Count; z += 3)
                                    //{
                                    //    Debug.Log("straddlePolyIdx[z]  " + straddlePolyIdx[z]);
                                    //    Debug.Log("straddlePolyIdx[z+1]" + straddlePolyIdx[z + 1]);
                                    //    Debug.Log("straddlePolyIdx[z+2]" + straddlePolyIdx[z + 2]);
                                    //}
                                    //Debug.Log("==========================");

                                    // �|���S���̃C���f�b�N�X�̍ŏ��̔ԍ�
                                    firstNum = g / 3;
                                    //Debug.Log("g:" + g);
                                    //Debug.Log("firstNum:" + (g / 3));

                                    // �ϐ��錾
                                    var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
                                    var cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));
                                    var pEnd = new Vector2() ;    // ��_�̏I�_
                                    var pEdge = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][secondNum].x), Mathf.Abs(intersectEdgList2[firstNum][secondNum].y));
                                    var pEdge2 = new Vector2();     // ��_�̏I�_�̕Ӄx�N�g��

                                 
                                    //Debug.Log("====== ��_�̃��X�g ======");
                                    //Debug.Log("edgIdx2List2[firstNum][0]:" + edgIdx2List2[firstNum][0]);
                                    //Debug.Log("edgIdx2List2[firstNum][1]:" + edgIdx2List2[firstNum][1]);
                                    //Debug.Log("edgIdx2List2[firstNum][2]:" + edgIdx2List2[firstNum][2]);
                                    //Debug.Log("edgIdx2List2[firstNum][3]:" + edgIdx2List2[firstNum][3]);
                                    //Debug.Log("intersectPolyList2[firstNum][0]:" + intersectPolyList2[firstNum][0]);
                                    //Debug.Log("intersectPolyList2[firstNum][1]:" + intersectPolyList2[firstNum][1]);
                                    //Debug.Log("intersectPolyList2.Count:" + intersectPolyList2.Count);
                                    //Debug.Log("==========================");
                                    // �ǂ�������_�̎n�_�����ׂ�
                                    if ((edgIdx2List2[firstNum][0] == idxList[0] || edgIdx2List2[firstNum][0] == idxList[1]) && (edgIdx2List2[firstNum][1] == idxList[1] || edgIdx2List2[firstNum][1] == idxList[0]))
                                    {
                                        Debug.Log("���_�̒ǉ�:01���n�_");
                                       
                                        secondNum = 0;
                                        pEnd = intersectPolyList2[firstNum][1];    // ��_�̏I�_
                                        pEdge2 = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][1].x), Mathf.Abs(intersectEdgList2[firstNum][1].y));// ��_�̏I�_�̕Ӄx�N�g��                                                      
                                        idxList.Clear();    // ���̍폜
                                        idxList.Add(edgIdx2List2[firstNum][2]); // ���̒ǉ�
                                        idxList.Add(edgIdx2List2[firstNum][3]); // ���̒ǉ�
                                        rastIdxList.Clear();    // ���̍폜
                                        rastIdxList.Add(edgIdx2List2[firstNum][2]);// ���̒ǉ�
                                        rastIdxList.Add(edgIdx2List2[firstNum][3]);// ���̒ǉ�
                                    }
                                    else if ((edgIdx2List2[firstNum][2] == idxList[0] || edgIdx2List2[firstNum][2] == idxList[1]) && (edgIdx2List2[firstNum][3] == idxList[1] || edgIdx2List2[firstNum][3] == idxList[0]))
                                    {
                                        Debug.Log("���_�̒ǉ�:23���n�_");
                                       
                                        secondNum = 1;
                                        pEnd = intersectPolyList2[firstNum][1];    // ��_�̏I�_
                                        pEdge2 = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][0].x), Mathf.Abs(intersectEdgList2[firstNum][0].y));// ��_�̏I�_�̕Ӄx�N�g��                    
                                        idxList.Clear();    // ���̍폜
                                        idxList.Add(edgIdx2List2[firstNum][0]); // ���̒ǉ�
                                        idxList.Add(edgIdx2List2[firstNum][1]); // ���̒ǉ�
                                        rastIdxList.Clear();    // ���̍폜
                                        rastIdxList.Add(edgIdx2List2[firstNum][0]);// ���̒ǉ�
                                        rastIdxList.Add(edgIdx2List2[firstNum][1]);// ���̒ǉ�
                                    }

                                    Debug.Log("secondNum:" + secondNum);
                                   


                                    // ���_�̒ǉ�
                                    vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f * -cpNormalAbs.z));
                                    vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f * cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f * cpNormalAbs.z));

                                    // �C���f�b�N�X�̊��蓖��
                                    int idx0 = straddlePolyIdx[(firstNum * 3)];
                                    int idx1 = straddlePolyIdx[(firstNum * 3) + 1];
                                    int idx2 = straddlePolyIdx[(firstNum * 3) + 2];
                                    int idx3 = vertices1.Count - 4; // 
                                    int idx4 = vertices1.Count - 3; //  
                                    int idx5 = vertices1.Count - 2; // 
                                    int idx6 = vertices1.Count - 1; // 
                                    int removeIdx = -1;

                                    // �폜����O�p�`�̌���
                                    Debug.Log("�폜����O�p�`�̒T��");
                                    for (int a = 0; a < triangles1.Count; a += 3)
                                    {
                                        if (!(triangles1[a] == idx0 && triangles1[a + 1] == idx1 && triangles1[a + 2] == idx2)) continue;
                                        Debug.Log("�폜���ꂽ�O�p�`"+ idx0 +"," + idx1 + "," + idx2);
                                       
                                        removeIdx = a;
                                    }

                                    // �C���f�b�N�X�̊���U��
                                    if (secondNum == 0)
                                    {
                                        // edgIdx2List2[firstNum][0]�AedgIdx2List2[firstNum][1]���n�_�̌�_

                                        // �C���f�b�N�X�̍폜
                                        triangles1.RemoveRange(removeIdx, 3);

                                        // 01-12�C���f�b�N�X
                                        if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx1)) &&
                                            ((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)))
                                        {
                                            Debug.Log("01-12�C���f�b�N�X");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx1);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx4);
                                        }
                                        // 01-02�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)))
                                        {
                                            Debug.Log("01-02�C���f�b�N�X");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx1);

                                        }
                                        // 02-12�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)))
                                        {
                                            Debug.Log("02-12�C���f�b�N�X");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx1);
                                        }
                                        // 02-01�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx1 || edgIdx2List2[firstNum][3] == idx0)))
                                        {
                                            Debug.Log("02-01�C���f�b�N�X");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx4);


                                        }
                                        // 12-01�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx1 || edgIdx2List2[firstNum][3] == idx0)))
                                        {
                                            Debug.Log("12-01�C���f�b�N�X");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx0);
                                        }
                                        // 12-20�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx2 || edgIdx2List2[firstNum][2] == idx0) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx2)))
                                        {
                                            Debug.Log(" 12-02�C���f�b�N�X");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx1);
                                        }

                                    }
                                    else if (secondNum == 1)
                                    {
                                        // �C���f�b�N�X�̍폜
                                        triangles1.RemoveRange(removeIdx, 3);

                                        //Debug.Log("====== ���̃��X�g ======");
                                        //Debug.Log("straddlePolyIdx.Count  " + straddlePolyIdx.Count);
                                        //for (int z = 0; z < straddlePolyIdx.Count; z += 3)
                                        //{
                                        //    Debug.Log("straddlePolyIdx[z]  " + straddlePolyIdx[z]);
                                        //    Debug.Log("straddlePolyIdx[z+1]" + straddlePolyIdx[z + 1]);
                                        //    Debug.Log("straddlePolyIdx[z+2]" + straddlePolyIdx[z + 2]);
                                        //}
                                        //Debug.Log("==========================");
                                        //Debug.Log("====== ��_�̃��X�g ======");
                                        //Debug.Log("idx0:" + idx0);
                                        //Debug.Log("idx1:" + idx1);
                                        //Debug.Log("idx2:" + idx2);
                                        //Debug.Log("edgIdx2List2[firstNum][0]:" + edgIdx2List2[firstNum][0]);
                                        //Debug.Log("edgIdx2List2[firstNum][1]:" + edgIdx2List2[firstNum][1]);
                                        //Debug.Log("edgIdx2List2[firstNum][2]:" + edgIdx2List2[firstNum][2]);
                                        //Debug.Log("edgIdx2List2[firstNum][3]:" + edgIdx2List2[firstNum][3]);
                                        // Debug.Log("==========================");

                                        // �C���f�b�N�X�̕��ёւ�
                                        //var numChange = new List<int>();
                                        //numChange.Add(edgIdx2List2[firstNum][0]);
                                        //numChange.Add(edgIdx2List2[firstNum][1]);
                                        //edgIdx2List2[firstNum].RemoveRange(0, 2);
                                        //edgIdx2List2[firstNum].Add(numChange[0]);
                                        //edgIdx2List2[firstNum].Add(numChange[1]);

                                        // 01-12�C���f�b�N�X
                                        if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)))
                                        {
                                            Debug.Log("01-12�C���f�b�N�X");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx1);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx4);
                                        }
                                        // 01-02�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)))
                                        {
                                            Debug.Log("01-02�C���f�b�N�X");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx1);

                                        }
                                        // 02-12�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)))
                                        {
                                            Debug.Log("02-12�C���f�b�N�X");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx1);
                                        }
                                        // 02-01�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx1 || edgIdx2List2[firstNum][1] == idx0)))
                                        {
                                            Debug.Log("02-01�C���f�b�N�X");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx4);


                                        }
                                        // 12-01�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx1 || edgIdx2List2[firstNum][1] == idx0)))
                                        {
                                            Debug.Log("12-01�C���f�b�N�X");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx0);
                                        }
                                        // 12-20�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx2 || edgIdx2List2[firstNum][0] == idx0) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx2)))
                                        {
                                            Debug.Log(" 12-02�C���f�b�N�X");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx1);
                                        }

                                        //numChange.RemoveRange(0, 2);
                                        //numChange.Add(edgIdx2List2[firstNum][0]);
                                        //numChange.Add(edgIdx2List2[firstNum][1]);
                                        //edgIdx2List2[firstNum].RemoveRange(0, 2);
                                        //edgIdx2List2[firstNum].Add(numChange[0]);
                                        //edgIdx2List2[firstNum].Add(numChange[1]);

                                    }

                                    // ��₩��폜
                                    Debug.Log("��₩��폜");
                                    idxCnt.RemoveRange(k, 3);
                                    intersectPolyList2.RemoveAt(firstNum);
                                    intersectEdgList2.RemoveAt(firstNum);
                                    edgIdx2List2.RemoveAt(firstNum);
                                    // �����܂ŗ�����I��
                                    end = true;
                                    break;
                                }
                                if (end) break;
                            }

                            if (end) break;
                        }

                        // �J�E���g++
                        count++;
                    }
                }

                // 4�������鏈��
                {
                    Debug.Log("4�������鏈��");
                    //Debug.Log("edgIdx2List[0][0]:" + edgIdx2List[0][0]);
                    //Debug.Log("edgIdx2List[0][0]:" + edgIdx2List[0][1]);
                    //Debug.Log("inerPolyIdx.Count:" + inerPolyIdx.Count);
                    //Debug.Log("inerPolyIdx[0]:" + inerPolyIdx[0]);
                    //Debug.Log("inerPolyIdx[1]:" + inerPolyIdx[1]);
                    //Debug.Log("inerPolyIdx[2]:" + inerPolyIdx[2]);

                    // �Ώۂ̃C���f�b�N�X�̍폜
                    for (int a = 0; a < triangles1.Count; a += 3)
                    {
                        // ��v���Ȃ�������X���[
                        if (!(triangles1[a] == inerPolyIdx[0] && triangles1[a + 1] == inerPolyIdx[1] && triangles1[a + 2] == inerPolyIdx[2])) continue;

                        triangles1.RemoveRange(a, 3);
                        break;
                    }

                    // ���_�̒ǉ�
                    vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);
                    vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);

                    // �C���f�b�N�X�̊��蓖��
                    int idx0 = inerPolyIdx[0];
                    int idx1 = inerPolyIdx[1];
                    int idx2 = inerPolyIdx[2];
                    int idx3 = vertices1.Count - 4; // 
                    int idx4 = vertices1.Count - 3; //  
                    int idx5 = vertices1.Count - 2; // 
                    int idx6 = vertices1.Count - 1; // 

                    // ��_�̎n�_���ǂ̕ӂɂ��邩
                    // ��01�̏�
                    if ((edgIdx2List[0][0] == inerPolyIdx[0] || edgIdx2List[0][0] == inerPolyIdx[1]) &&
                       (edgIdx2List[0][1] == inerPolyIdx[1] || edgIdx2List[0][1] == inerPolyIdx[0]))
                    {
                        Debug.Log("��01�̏�");
                        triangles1.Add(idx5);
                        triangles1.Add(idx2);
                        triangles1.Add(idx0);

                        triangles1.Add(idx5);
                        triangles1.Add(idx0);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx1);

                        triangles1.Add(idx5);
                        triangles1.Add(idx1);
                        triangles1.Add(idx2);

                        // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                        idxMemory.Clear();
                        idxMemory.Add(idx5);
                        idxMemory.Add(idx2);
                        idxMemory.Add(idx0);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx0);
                        idxMemory.Add(idx4);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx3);
                        idxMemory.Add(idx1);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx1);
                        idxMemory.Add(idx2);
                    }
                    // ��12�̏�
                    else if ((edgIdx2List[0][0] == inerPolyIdx[1] || edgIdx2List[0][0] == inerPolyIdx[2]) &&
                       (edgIdx2List[0][1] == inerPolyIdx[2] || edgIdx2List[0][1] == inerPolyIdx[1]))
                    {
                        Debug.Log("��12�̏�");
                        triangles1.Add(idx5);
                        triangles1.Add(idx0);
                        triangles1.Add(idx1);

                        triangles1.Add(idx5);
                        triangles1.Add(idx1);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx2);

                        triangles1.Add(idx5);
                        triangles1.Add(idx2);
                        triangles1.Add(idx0);

                        // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                        idxMemory.Clear();
                        idxMemory.Add(idx5);
                        idxMemory.Add(idx0);
                        idxMemory.Add(idx1);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx1);
                        idxMemory.Add(idx4);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx3);
                        idxMemory.Add(idx2);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx2);
                        idxMemory.Add(idx0);
                    }
                    // ��20�̏�
                    else if ((edgIdx2List[0][0] == inerPolyIdx[2] || edgIdx2List[0][0] == inerPolyIdx[0]) &&
                       (edgIdx2List[0][1] == inerPolyIdx[0] || edgIdx2List[0][1] == inerPolyIdx[2]))
                    {
                        Debug.Log("��20�̏�");
                        triangles1.Add(idx5);
                        triangles1.Add(idx1);
                        triangles1.Add(idx2);

                        triangles1.Add(idx5);
                        triangles1.Add(idx2);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx0);

                        triangles1.Add(idx5);
                        triangles1.Add(idx0);
                        triangles1.Add(idx1);

                        // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                        idxMemory.Clear();
                        idxMemory.Add(idx5);
                        idxMemory.Add(idx1);
                        idxMemory.Add(idx2);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx2);
                        idxMemory.Add(idx4);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx3);
                        idxMemory.Add(idx0);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx0);
                        idxMemory.Add(idx1);
                    }
                }
            }
            // ��_��1�_���Ȃ��Ƃ�
            else if(straddlePolyIdx.Count == 0 && inerPolyIdx.Count == 0)
            {
                Debug.Log("��_���Ȃ���");
                // 3�������鏈��
                {
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
                            // ���_�̒ǉ�
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
                                        //Debug.Log(idxMemory[j] + "" + idxMemory[j + 1] + "" + idxMemory[j + 2]);
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
                                        if (j == 6)
                                        {
                                            Debug.Log("j = 6");
                                            // �C���f�b�N�X�̕ύX
                                            triangles1[i + 3] = _5;
                                            triangles1[i - 3] = _5;
                                            triangles1[i - 6] = _5;

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
                                        if (j == 9)
                                        {
                                            Debug.Log("j = 9");
                                            triangles1[i + j - 9] = _5;
                                            triangles1[i + j - 15] = _5;
                                            triangles1[i + j - 18] = _5;

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

                                    }

                                }
                                else if (idxMemory.Count < 10)  // �L�����ꂽ�O�p�`�C���f�b�N�X�̐���10�������Ȃ��Ƃ�(�O�p�`��3��)
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

                                            triangles1[i + j - 3] = _5;
                                            //triangles1[i + j] = _5;
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
                                        if (j == 6)
                                        {
                                            Debug.Log("j = 6");
                                            Debug.Log("j = " + j);
                                            Debug.Log("j + i = " + (j + i));
                                            Debug.Log("2���");

                                            // �C���f�b�N�X�̕ύX

                                            triangles1[i - 3] = _5;
                                            triangles1[i - 6] = _5;
                                            //triangles1[i + j + 3] = _5;

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
                                    }

                                }



                            }
                        }


                    }
                }
            }
        }

        // �J�b�g���鏈��(�S��)
        {
            ////�J�b�g�������I�u�W�F�N�g�̃��b�V�����g���C�A���O�����Ƃɏ���
            //for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
            //{
            //    //���b�V����3�̒��_���擾
            //    p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);//+ Vector3.one * 0.0001f;
            //    p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);//+ Vector3.one * 0.0001f;
            //    p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]); //+ Vector3.one * 0.0001f;

            //    // �J�b�g�|�C���g�̎n�_���|���S���̒��ɂ��邩
            //    double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
            //    double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[cutPoint.Count - 1].x + (p0.x - p2.x) * cutPoint[cutPoint.Count - 1].z);
            //    double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[cutPoint.Count - 1].x + (p1.x - p0.x) * cutPoint[cutPoint.Count - 1].z);

            //    // �O�p�`�̒��ɂ��邩
            //    if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
            //    {
            //        // ��O�̃J�b�g�|�C���g�����邩
            //        double _s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[cutPoint.Count - 2].x + (p0.x - p2.x) * cutPoint[cutPoint.Count - 2].z);
            //        double _t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[cutPoint.Count - 2].x + (p1.x - p0.x) * cutPoint[cutPoint.Count - 2].z);
            //        if ((0 <= _s && _s <= 1) && (0 <= _t && _t <= 1) && (0 <= 1 - _s - _t && 1 - _s - _t <= 1))
            //        {
            //            // ����Ƃ�
            //            Debug.Log("����");

            //            // �J�b�g�|�C���g�̏ꏊ�ɒ��_�̒ǉ�(���Ƃŕ����邽�ߓ�ǉ�)
            //            vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);
            //            vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);

            //            // �C���f�b�N�X�̊��蓖��
            //            int _0 = attachedMesh.triangles[i];
            //            int _1 = attachedMesh.triangles[i + 1];
            //            int _2 = attachedMesh.triangles[i + 2];
            //            int _3 = vertices1.Count - 2; // 7
            //            int _4 = vertices1.Count - 1; // �g��Ȃ�  
            //            int _5 = vertices1.Count - 3; // 6

            //            // �L�����ꂽ�O�p�`�C���f�b�N�X�̐��������[�v
            //            for (int j = 0; j < idxMemory.Count; j += 3)
            //            {
            //                // �L�����ꂽ�O�p�`�C���f�N�X�ƈ�v������
            //                if (idxMemory.Count > 9)
            //                {
            //                    if (attachedMesh.triangles[i] == idxMemory[j] && attachedMesh.triangles[i + 1] == idxMemory[j + 1] && attachedMesh.triangles[i + 2] == idxMemory[j + 2])
            //                    {
            //                        Debug.Log(idxMemory[j] + "" + idxMemory[j + 1] + "" + idxMemory[j + 2]);
            //                        if (j == 0)
            //                        {
            //                            Debug.Log("j = 0");
            //                            // �C���f�b�N�X�̕ύX
            //                            triangles1[i + j + 3] = _5;

            //                            // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�

            //                            triangles1.RemoveRange(i, 3);
            //                            triangles1.Add(_3);
            //                            triangles1.Add(_1);
            //                            triangles1.Add(_2);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_2);
            //                            triangles1.Add(_5);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_0);
            //                            triangles1.Add(_1);

            //                            // �o�����O�p�`�C���f�b�N�X�̕ۑ�
            //                            idxMemory.Clear();
            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_1);
            //                            idxMemory.Add(_2);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_2);
            //                            idxMemory.Add(_5);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_0);
            //                            idxMemory.Add(_1);
            //                            break;
            //                        }
            //                        if (j == 3)
            //                        {
            //                            Debug.Log("j = 3");
            //                            // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
            //                            triangles1.RemoveRange(i, 3);
            //                            triangles1.Add(_3);
            //                            triangles1.Add(_1);
            //                            triangles1.Add(_2);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_2);
            //                            triangles1.Add(_5);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_0);
            //                            triangles1.Add(_1);

            //                            // �o�����O�p�`�C���f�b�N�X�̕ۑ�
            //                            idxMemory.Clear();
            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_1);
            //                            idxMemory.Add(_2);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_2);
            //                            idxMemory.Add(_5);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_0);
            //                            idxMemory.Add(_1);
            //                            break;
            //                        }
            //                        if (j == 6)
            //                        {
            //                            Debug.Log("j = 6");
            //                            // �C���f�b�N�X�̕ύX
            //                            triangles1[i + 3] = _5;
            //                            triangles1[i - 3] = _5;
            //                            triangles1[i - 6] = _5;

            //                            // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
            //                            triangles1.RemoveRange(i, 3);
            //                            triangles1.Add(_3);
            //                            triangles1.Add(_1);
            //                            triangles1.Add(_2);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_2);
            //                            triangles1.Add(_5);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_0);
            //                            triangles1.Add(_1);


            //                            // �o�����O�p�`�C���f�b�N�X�̕ۑ�
            //                            idxMemory.Clear();
            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_1);
            //                            idxMemory.Add(_2);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_2);
            //                            idxMemory.Add(_5);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_0);
            //                            idxMemory.Add(_1);
            //                            break;
            //                        }
            //                        if (j == 9)
            //                        {
            //                            Debug.Log("j = 9");
            //                            triangles1[i + j - 9] = _5;
            //                            triangles1[i + j - 15] = _5;
            //                            triangles1[i + j - 18] = _5;

            //                            // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
            //                            triangles1.RemoveRange(i, 3);
            //                            triangles1.Add(_3);
            //                            triangles1.Add(_1);
            //                            triangles1.Add(_2);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_2);
            //                            triangles1.Add(_5);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_0);
            //                            triangles1.Add(_1);

            //                            // �o�����O�p�`�C���f�b�N�X�̕ۑ�
            //                            idxMemory.Clear();
            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_1);
            //                            idxMemory.Add(_2);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_2);
            //                            idxMemory.Add(_5);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_0);
            //                            idxMemory.Add(_1);
            //                            break;
            //                        }

            //                    }

            //                }
            //                else if (idxMemory.Count < 10)  // �L�����ꂽ�O�p�`�C���f�b�N�X�̐���10�������Ȃ��Ƃ�(�O�p�`��3��)
            //                {
            //                    if (attachedMesh.triangles[i] == idxMemory[j] && attachedMesh.triangles[i + 1] == idxMemory[j + 1] && attachedMesh.triangles[i + 2] == idxMemory[j + 2])
            //                    {
            //                        if (j == 0)
            //                        {
            //                            Debug.Log("j = 0");
            //                            // �C���f�b�N�X�̕ύX
            //                            triangles1[i + j + 3] = _5;

            //                            // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�

            //                            triangles1.RemoveRange(i, 3);
            //                            triangles1.Add(_3);
            //                            triangles1.Add(_1);
            //                            triangles1.Add(_2);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_2);
            //                            triangles1.Add(_5);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_0);
            //                            triangles1.Add(_1);

            //                            // �o�����O�p�`�C���f�b�N�X�̕ۑ�
            //                            idxMemory.Clear();
            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_1);
            //                            idxMemory.Add(_2);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_2);
            //                            idxMemory.Add(_5);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_0);
            //                            idxMemory.Add(_1);
            //                            break;
            //                        }
            //                        if (j == 3)
            //                        {
            //                            Debug.Log("j = 3");

            //                            triangles1[i + j - 3] = _5;
            //                            //triangles1[i + j] = _5;
            //                            // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
            //                            triangles1.RemoveRange(i, 3);
            //                            triangles1.Add(_3);
            //                            triangles1.Add(_1);
            //                            triangles1.Add(_2);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_2);
            //                            triangles1.Add(_5);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_0);
            //                            triangles1.Add(_1);

            //                            // �o�����O�p�`�C���f�b�N�X�̕ۑ�
            //                            idxMemory.Clear();
            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_1);
            //                            idxMemory.Add(_2);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_2);
            //                            idxMemory.Add(_5);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_0);
            //                            idxMemory.Add(_1);
            //                            break;
            //                        }
            //                        if (j == 6)
            //                        {
            //                            Debug.Log("j = 6");
            //                            Debug.Log("j = " + j);
            //                            Debug.Log("j + i = " + (j + i));
            //                            Debug.Log("2���");

            //                            // �C���f�b�N�X�̕ύX

            //                            triangles1[i - 3] = _5;
            //                            triangles1[i - 6] = _5;
            //                            //triangles1[i + j + 3] = _5;

            //                            // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
            //                            triangles1.RemoveRange(i, 3);
            //                            triangles1.Add(_3);
            //                            triangles1.Add(_1);
            //                            triangles1.Add(_2);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_2);
            //                            triangles1.Add(_5);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_0);
            //                            triangles1.Add(_1);

            //                            // �o�����O�p�`�C���f�b�N�X�̕ۑ�
            //                            idxMemory.Clear();
            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_1);
            //                            idxMemory.Add(_2);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_2);
            //                            idxMemory.Add(_5);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_0);
            //                            idxMemory.Add(_1);
            //                            break;
            //                        }
            //                    }

            //                }



            //            }

            //        }
            //        else
            //        {
            //            // �Ȃ��Ƃ�
            //            Debug.Log("�Ȃ�");

            //            // --- �|���S���̕ӂƃJ�b�g�|�C���g�������������̏��� ---

            //            // �ϐ��錾              
            //            int vtxCount = vertices1.Count; // �����������Y�ꂽww???
            //            var straddlePolyIdx = new List<int>();  // �܂������|���S���ԍ����X�g
            //            var crossPolyIdx = new List<int>();  // �����|���S���ԍ����X�g
            //            var inerPolyIdx = new List<int>();  // �J�b�g�|�C���g�����ɓ����Ă���|���S���ԍ�
            //            var intersectPolyList = new List<List<Vector2>>();  // �|���S�����Ƃɂ���������Ă���_�̃��X�g
            //            var intersectEdgList = new List<List<Vector2>>();  // �|���S�����Ƃɂ���������Ă���ӂ̃��X�g
            //            var intersectionList = new List<Vector2>(); // ��_�̃��X�g
            //            var cp_s = new Vector2(cutPoint[cutPoint.Count - 2].x, cutPoint[cutPoint.Count - 2].z);    // �J�b�g�|�C���g�̏I�_��1�O
            //            var cp_v = new Vector2(cutPoint[cutPoint.Count - 1].x, cutPoint[cutPoint.Count - 1].z);    // �J�b�g�|�C���g�̏I�_
            //            var cpEdg = cp_v - cp_s;    // �J�b�g�|�C���g�̏I�_�ƃJ�b�g�|�C���g�̏I�_��1�O���Ȃ�����
            //            var checkCp = cp_s + cpEdg * 0.01f; // �J�b�g�|�C���g�̏I�_��1�O����J�b�g�|�C���g�̏I�_�̕����ɂ�����ƐL�΂����_
            //            var edgIdx2List = new List<List<int>>();   // �ӂ̃C���f�b�N�X�̃��X�g�̃��X�g   

            //            // �܂����ł�|���S���ƐN�����Ă���|���S���������邩�T��
            //            for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
            //            {
            //                // �ϐ��錾             
            //                int interPointCnt = 0; // ���������_�̐�
            //                var intersection = new List<Vector2>(); // ��_�̃��X�g
            //                var edgList = new List<Vector2>(); //�ӂ̃��X�g
            //                var edgIdxList = new List<int>();   // �ӂ̃C���f�b�N�X�̃��X�g   

            //                // �|���S���̕ӂ̐��������[�v
            //                for (int k = 0; k < 3; k++)
            //                {
            //                    // �|���S����2���_
            //                    Vector2 polyVtx_s = new Vector2(attachedMesh.vertices[attachedMesh.triangles[j + k]].x, attachedMesh.vertices[attachedMesh.triangles[j + k]].z);  // �n�_
            //                    Vector2 polyVtx_v = new Vector2(attachedMesh.vertices[attachedMesh.triangles[j + (k + 1) % 3]].x, attachedMesh.vertices[attachedMesh.triangles[j + (k + 1) % 3]].z);  // �I�_

            //                    // �|���S���̕�
            //                    Vector2 polyEdge = polyVtx_v - polyVtx_s;   // ��

            //                    // �J�b�g�|�C���g��2���_
            //                    Vector2 cpVtx_s = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z); // �n�_
            //                    Vector2 cpVtx_v = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z); // �I�_

            //                    // �J�b�g�|�C���g�̕�
            //                    Vector2 cpEdge = cpVtx_v - cpVtx_s; // ��

            //                    // �J�b�g�|�C���g�̎n�_�̕␳
            //                    cpVtx_s += cpEdge * 0.01f;

            //                    // �J�b�g�|�C���g�̕ӂ̕␳
            //                    cpEdge = cpVtx_v - cpVtx_s; // ��

            //                    // �|���S���̕ӂƃJ�b�g�|�C���g�̕ӂ̎n�_���Ȃ����x�N�g��
            //                    Vector2 v = polyVtx_s - cpVtx_s;

            //                    // �����̎n�_�����_�̃x�N�g���̌W��(����)
            //                    float t1 = (v.x * polyEdge.y - polyEdge.x * v.y) / (cpEdge.x * polyEdge.y - polyEdge.x * cpEdge.y);
            //                    float t2 = (v.x * cpEdge.y - cpEdge.x * v.y) / (cpEdge.x * polyEdge.y - polyEdge.x * cpEdge.y);

            //                    // ��_
            //                    Vector2 p = new Vector2(polyVtx_s.x, polyVtx_s.y) + new Vector2(polyEdge.x * t2, polyEdge.y * t2);

            //                    // �����Ɛ�����������Ă��邩
            //                    const float eps = 0.00001f;
            //                    if (t1 + eps < 0 || t1 - eps > 1 || t2 + eps < 0 || t2 - eps > 1)
            //                    {
            //                        // ������ĂȂ��Ƃ��X���[
            //                        continue;
            //                    }
            //                    else
            //                    {
            //                        //vertices1.Add(new Vector3(p.x, attachedMesh.vertices[0].y , p.y));
            //                        // ������Ă鎞��_�J�E���g++                               
            //                        interPointCnt++;    // ��_�J�E���g    
            //                        intersection.Add(p);    // ��_�̕ۑ�
            //                        intersectionList.Add(p);// ��_�̕ۑ�
            //                        edgList.Add(polyEdge);
            //                        edgIdxList.Add(attachedMesh.triangles[j + k]);
            //                        edgIdxList.Add(attachedMesh.triangles[j + (k + 1) % 3]);
            //                        Debug.Log("j + k" + attachedMesh.triangles[j + k]);
            //                        Debug.Log("j + (k + 1) % 3" + attachedMesh.triangles[j + (k + 1) % 3]);

            //                    }
            //                }

            //                // �|���S���ԍ���ۑ�
            //                if (interPointCnt == 2)// ��_�J�E���g2��(�|���S�����܂����ł鎞)
            //                {
            //                    Debug.Log("2�����");
            //                    Debug.Log("�|���S���ԍ���" + attachedMesh.triangles[j] + "," + attachedMesh.triangles[j + 1] + "," + attachedMesh.triangles[j + 2]);

            //                    straddlePolyIdx.Add(attachedMesh.triangles[j]);
            //                    straddlePolyIdx.Add(attachedMesh.triangles[j + 1]);
            //                    straddlePolyIdx.Add(attachedMesh.triangles[j + 2]);
            //                    crossPolyIdx.Add(j);
            //                    crossPolyIdx.Add(j + 1);
            //                    crossPolyIdx.Add(j + 2);
            //                    intersectPolyList.Add(intersection);
            //                    intersectEdgList.Add(edgList);
            //                    edgIdx2List.Add(edgIdxList);
            //                    Debug.Log("straddlePolyIdx.Count" + straddlePolyIdx.Count);
            //                }
            //                else if (interPointCnt == 1)// ��_�J�E���g1��(�J�b�g�|�C���g�̏I�_���|���S���̒��ɂ���Ƃ�)
            //                {
            //                    Debug.Log("1�����");
            //                    inerPolyIdx.Add(j);
            //                    inerPolyIdx.Add(j + 1);
            //                    inerPolyIdx.Add(j + 2);
            //                    crossPolyIdx.Add(j);
            //                    crossPolyIdx.Add(j + 1);
            //                    crossPolyIdx.Add(j + 2);
            //                    //intersectPolyList.Add(intersection);
            //                    //intersectEdgList.Add(edgList);
            //                }
            //                else
            //                {
            //                    // Debug.Log("3�����");
            //                    // Debug.Log("�|���S���ԍ���" + attachedMesh.triangles[j] + "," + attachedMesh.triangles[j + 1] + "," + attachedMesh.triangles[j + 2]);

            //                }
            //            }

            //            var _p = new Vector2();

            //            // �|���S�����Ƃɏ���
            //            for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
            //            {
            //                // �J�b�g�|�C���g����������I��
            //                if (vtxCount != vertices1.Count) break;

            //                // ���������_�̐�
            //                int interPointCnt = 0;

            //                // �|���S���̕ӂ̐��������[�v
            //                for (int k = 0; k < 3; k++)
            //                {
            //                    // �|���S����2���_
            //                    Vector2 polyVtx_s = new Vector2(attachedMesh.vertices[attachedMesh.triangles[j + k]].x, attachedMesh.vertices[attachedMesh.triangles[j + k]].z);  // �n�_
            //                    Vector2 polyVtx_v = new Vector2(attachedMesh.vertices[attachedMesh.triangles[j + (k + 1) % 3]].x, attachedMesh.vertices[attachedMesh.triangles[j + (k + 1) % 3]].z);  // �I�_

            //                    // �|���S���̕�
            //                    Vector2 polyEdge = polyVtx_v - polyVtx_s;   // ��

            //                    // �J�b�g�|�C���g��2���_
            //                    Vector2 cpVtx_s = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z); // �n�_
            //                    Vector2 cpVtx_v = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z); // �I�_

            //                    // �J�b�g�|�C���g�̕�
            //                    Vector2 cpEdge = cpVtx_v - cpVtx_s; // ��

            //                    // �|���S���̕ӂƃJ�b�g�|�C���g�̕ӂ̎n�_���Ȃ����x�N�g��
            //                    Vector2 v = polyVtx_s - cpVtx_s;

            //                    // �����̎n�_�����_�̃x�N�g���̌W��(����)
            //                    float t1 = (v.x * polyEdge.y - polyEdge.x * v.y) / (cpEdge.x * polyEdge.y - polyEdge.x * cpEdge.y);
            //                    float t2 = (v.x * cpEdge.y - cpEdge.x * v.y) / (cpEdge.x * polyEdge.y - polyEdge.x * cpEdge.y);

            //                    // ��_
            //                    Vector2 p = new Vector2(polyVtx_s.x, polyVtx_s.y) + new Vector2(polyEdge.x * t2, polyEdge.y * t2);



            //                    // �����Ɛ�����������Ă��邩
            //                    const float eps = 0.00001f;
            //                    if (t1 + eps < 0 || t1 - eps > 1 || t2 + eps < 0 || t2 - eps > 1)
            //                    {

            //                        continue;

            //                    }
            //                    else
            //                    {
            //                        Debug.Log("�������Ă�");

            //                        // ��_�J�E���g�𑫂�
            //                        interPointCnt++;

            //                        // �ϐ��錾
            //                        // ��_��������Ƃ��炷
            //                        var interPoint = new Vector2(cutPoint[cutPoint.Count - 2].x + ((p.x + transform.position.x) - cutPoint[cutPoint.Count - 2].x) * 0.8f - transform.position.x, cutPoint[cutPoint.Count - 2].z + ((p.y + transform.position.z) - cutPoint[cutPoint.Count - 2].z) * 0.8f - transform.position.z);
            //                        var idxList = new List<int>();// �C���f�N�X�̃��X�g
            //                        var edgIdx_s = new int[2];  // �܂��������̍ŏ��̃C���f�b�N�X
            //                        var edgIdx_v = new int[2];  // �܂��������̍ŏ��̃C���f�b�N�X

            //                        // --- 2�������鏈�� ---                             
            //                        // ������Ƃ��炵����_���ǂ̃|���S���ɂ��邩���ׂ�
            //                        for (int n = 0; n < attachedMesh.triangles.Length; n += 3)
            //                        {
            //                            //���b�V����3�̒��_���擾
            //                            var _p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[n]]);//+ Vector3.one * 0.0001f;
            //                            var _p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[n + 1]]);//+ Vector3.one * 0.0001f;
            //                            var _p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[n + 2]]); //+ Vector3.one * 0.0001f;

            //                            // ������Ƃ��炵����_���|���S���̒��ɂ��邩
            //                            double _Area = 0.5 * (-_p1.z * _p2.x + _p0.z * (-_p1.x + _p2.x) + _p0.x * (_p1.z - _p2.z) + _p1.x * _p2.z);
            //                            double _s2 = 1 / (2 * _Area) * (_p0.z * _p2.x - _p0.x * _p2.z + (_p2.z - _p0.z) * (interPoint.x + transform.position.x) + (_p0.x - _p2.x) * (interPoint.y + transform.position.z));
            //                            double _t2 = 1 / (2 * _Area) * (_p0.x * _p1.z - _p0.z * _p1.x + (_p0.z - _p1.z) * (interPoint.x + transform.position.x) + (_p1.x - _p0.x) * (interPoint.y + transform.position.z));


            //                            // �O�p�`�̒��ɂ��邩
            //                            if ((0 <= _s2 && _s2 <= 1) && (0 <= _t2 && _t2 <= 1) && (0 <= 1 - _s2 - _t2 && 1 - _s2 - _t2 <= 1))
            //                            {
            //                                //Debug.Log("�������Ă���_���|���S���̒��ɂ���");

            //                                // �L�����ꂽ�O�p�`�C���f�b�N�X�̐��������[�v
            //                                for (int a = 0; a < idxMemory.Count; a += 3)
            //                                {
            //                                    // ������Ƃ��炵����_���ǂ̋L�����ꂽ�O�p�`�C���f�b�N�X�̒��ɂ��邩����
            //                                    if (attachedMesh.triangles[n] == idxMemory[a] && attachedMesh.triangles[n + 1] == idxMemory[a + 1] && attachedMesh.triangles[n + 2] == idxMemory[a + 2])
            //                                    {
            //                                        // ���ɒǉ�
            //                                        Debug.Log("���ɒǉ�");
            //                                        idxList.Add(attachedMesh.triangles[n]);
            //                                        idxList.Add(attachedMesh.triangles[n + 1]);
            //                                        idxList.Add(attachedMesh.triangles[n + 2]);
            //                                        edgIdx_s[0] = attachedMesh.triangles[j + k];  // ��_������ӂ̎n�_
            //                                        edgIdx_s[1] = attachedMesh.triangles[j + (k + 1) % 3];  // ��_������ӂ̏I�_
            //                                        _p = p; // ��_
            //                                                // ����
            //                                        if (a == 0)
            //                                        {
            //                                            Debug.Log("�|���S���ԍ�" + attachedMesh.triangles[n] + "," + attachedMesh.triangles[n + 1] + "," + attachedMesh.triangles[n + 2]);

            //                                            Debug.Log("a = 0");

            //                                            // ��_�����Ƃɒ��_��ǉ�
            //                                            vertices1.Add(new Vector3(p.x, cutPoint[cutPoint.Count - 1].y - transform.position.y, p.y));
            //                                            vertices1.Add(new Vector3(p.x, cutPoint[cutPoint.Count - 1].y - transform.position.y, p.y));

            //                                            // �������W�ɒ��_����������L����
            //                                            for (int l = 0; l < vertices1.Count - 1; l++)
            //                                            {
            //                                                // �������W����Ȃ������X���[
            //                                                if (vertices1[l] != vertices1[l + 1]) continue;

            //                                                // �؂�����ɑ΂��ē_���ړ�����߂̏���
            //                                                edge1 = new Vector3(polyVtx_v.x, 0, polyVtx_v.y) - new Vector3(polyVtx_s.x, 0, polyVtx_s.y);
            //                                                edge2 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
            //                                                edge = edge1;

            //                                                Debug.Log("edge" + edge);

            //                                                // �J�b�g�|�C���g���꒼����������
            //                                                // �����ɓ_���L����
            //                                                if (edge == Vector3.zero)
            //                                                {
            //                                                    edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
            //                                                    edge2 = (cutPoint[cutPoint.Count - 1] + Vector3.up) - cutPoint[cutPoint.Count - 1];
            //                                                    edge = Vector3.Cross(edge1, edge2);
            //                                                }

            //                                                vertices1[l] = vertices1[l] + edge.normalized * 0.04f;
            //                                                vertices1[l + 1] = vertices1[l + 1] - edge.normalized * 0.04f;
            //                                            }

            //                                            // �C���f�b�N�X�̊��蓖��
            //                                            int idx0 = attachedMesh.triangles[n];
            //                                            int idx1 = attachedMesh.triangles[n + 1];
            //                                            int idx2 = attachedMesh.triangles[n + 2];
            //                                            int idx3 = vertices1.Count - 2; // 7
            //                                            int idx4 = vertices1.Count - 1; // �g��Ȃ�  
            //                                            int idx5 = vertices1.Count - 3; // 6


            //                                            // �C���f�b�N�X�̕ύX
            //                                            triangles1[n + 3] = idx5;

            //                                            // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�

            //                                            triangles1.RemoveRange(n, 3);
            //                                            triangles1.Add(idx4);
            //                                            triangles1.Add(idx2);
            //                                            triangles1.Add(idx5);

            //                                            triangles1.Add(idx3);
            //                                            triangles1.Add(idx0);
            //                                            triangles1.Add(idx1);

            //                                            // �o�����O�p�`�C���f�b�N�X�̕ۑ�
            //                                            //idxMemory.Clear();
            //                                            //idxMemory.Add(idx3);
            //                                            //idxMemory.Add(idx2);
            //                                            //idxMemory.Add(idx5);

            //                                            //idxMemory.Add(idx3);
            //                                            //idxMemory.Add(idx0);
            //                                            //idxMemory.Add(idx1);

            //                                            break;
            //                                        }
            //                                        if (a == 3)
            //                                        {
            //                                            Debug.Log("a = 3");
            //                                            // ��_�����Ƃɒ��_��ǉ�
            //                                            vertices1.Add(new Vector3(p.x, cutPoint[cutPoint.Count - 1].y - transform.position.y, p.y));
            //                                            vertices1.Add(new Vector3(p.x, cutPoint[cutPoint.Count - 1].y - transform.position.y, p.y));

            //                                            // �������W�ɒ��_����������L����
            //                                            for (int l = 0; l < vertices1.Count - 1; l++)
            //                                            {
            //                                                // �������W����Ȃ������X���[
            //                                                if (vertices1[l] != vertices1[l + 1]) continue;

            //                                                // �؂�����ɑ΂��ē_���ړ�����߂̏���
            //                                                edge1 = new Vector3(polyVtx_v.x, 0, polyVtx_v.y) - new Vector3(polyVtx_s.x, 0, polyVtx_s.y);
            //                                                edge2 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
            //                                                edge = edge1;

            //                                                Debug.Log("edge" + edge);

            //                                                // �J�b�g�|�C���g���꒼����������
            //                                                // �����ɓ_���L����
            //                                                if (edge == Vector3.zero)
            //                                                {
            //                                                    edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
            //                                                    edge2 = (cutPoint[cutPoint.Count - 1] + Vector3.up) - cutPoint[cutPoint.Count - 1];
            //                                                    edge = Vector3.Cross(edge1, edge2);
            //                                                }

            //                                                vertices1[l] = vertices1[l] + edge.normalized * 0.04f;
            //                                                vertices1[l + 1] = vertices1[l + 1] - edge.normalized * 0.04f;
            //                                            }

            //                                            // �C���f�b�N�X�̊��蓖��
            //                                            int idx0 = attachedMesh.triangles[n];
            //                                            int idx1 = attachedMesh.triangles[n + 1];
            //                                            int idx2 = attachedMesh.triangles[n + 2];
            //                                            int idx3 = vertices1.Count - 2; // 7
            //                                            int idx4 = vertices1.Count - 1; // �g��Ȃ�  
            //                                            int idx5 = vertices1.Count - 3; // 6

            //                                            // �C���f�b�N�X�̕ύX
            //                                            //triangles1[n + 3] = idx5;

            //                                            // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
            //                                            triangles1.RemoveRange(n, 3);
            //                                            triangles1.Add(idx4);
            //                                            triangles1.Add(idx2);
            //                                            triangles1.Add(idx5);

            //                                            triangles1.Add(idx3);
            //                                            triangles1.Add(idx0);
            //                                            triangles1.Add(idx1);

            //                                            break;
            //                                        }
            //                                        if (a == 6)
            //                                        {
            //                                            Debug.Log("a = 6");
            //                                            // ��_�����Ƃɒ��_��ǉ�
            //                                            vertices1.Add(new Vector3(p.x, cutPoint[cutPoint.Count - 1].y - transform.position.y, p.y));
            //                                            vertices1.Add(new Vector3(p.x, cutPoint[cutPoint.Count - 1].y - transform.position.y, p.y));

            //                                            // �������W�ɒ��_����������L����
            //                                            for (int l = 0; l < vertices1.Count - 1; l++)
            //                                            {
            //                                                // �������W����Ȃ������X���[
            //                                                if (vertices1[l] != vertices1[l + 1]) continue;

            //                                                // �؂�����ɑ΂��ē_���ړ�����߂̏���
            //                                                edge1 = new Vector3(polyVtx_v.x, 0, polyVtx_v.y) - new Vector3(polyVtx_s.x, 0, polyVtx_s.y);
            //                                                edge2 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
            //                                                edge = edge1;

            //                                                Debug.Log("edge" + edge);

            //                                                // �J�b�g�|�C���g���꒼����������
            //                                                // �����ɓ_���L����
            //                                                if (edge == Vector3.zero)
            //                                                {
            //                                                    edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
            //                                                    edge2 = (cutPoint[cutPoint.Count - 1] + Vector3.up) - cutPoint[cutPoint.Count - 1];
            //                                                    edge = Vector3.Cross(edge1, edge2);
            //                                                }

            //                                                vertices1[l] = vertices1[l] + edge.normalized * 0.04f;
            //                                                vertices1[l + 1] = vertices1[l + 1] - edge.normalized * 0.04f;
            //                                            }

            //                                            // �C���f�b�N�X�̊��蓖��
            //                                            int idx0 = attachedMesh.triangles[n];
            //                                            int idx1 = attachedMesh.triangles[n + 1];
            //                                            int idx2 = attachedMesh.triangles[n + 2];
            //                                            int idx3 = vertices1.Count - 2; // 7
            //                                            int idx4 = vertices1.Count - 1; // �g��Ȃ�  
            //                                            int idx5 = vertices1.Count - 3; // 6

            //                                            // �C���f�b�N�X�̕ύX
            //                                            triangles1[n - 6] = idx5;
            //                                            triangles1[n - 3] = idx5;

            //                                            // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
            //                                            triangles1.RemoveRange(n, 3);
            //                                            triangles1.Add(idx4);
            //                                            triangles1.Add(idx2);
            //                                            triangles1.Add(idx5);

            //                                            triangles1.Add(idx3);
            //                                            triangles1.Add(idx0);
            //                                            triangles1.Add(idx1);

            //                                            // �����܂ŗ�����O�p�`��񓙕�����̂͏I��
            //                                            break;
            //                                        }

            //                                    }
            //                                }

            //                                break;
            //                            }
            //                        }

            //                        // --- 2�������鏈��2 ---
            //                        Debug.Log("straddlePolyIdx.Count" + straddlePolyIdx.Count);
            //                        Debug.Log("idxList.Count" + idxList.Count);
            //                        int whileCnt = 0;
            //                        while (straddlePolyIdx.Count > 0 && whileCnt < 50)
            //                        {
            //                            if (idxList.Count > 0)
            //                            {
            //                                bool triger = false;
            //                                for (int n = 0; n < straddlePolyIdx.Count; n += 3)
            //                                {
            //                                    if (straddlePolyIdx.Count == 0) break;
            //                                    // �O�p�`��3�ӂ���̂�3���[�v
            //                                    for (int u = 0; u < 3; u++)
            //                                    {
            //                                        if (straddlePolyIdx.Count == 0) break;
            //                                        // 1�ӂɑ΂���3�Ӓ��ׂ�̂�3���[�v
            //                                        for (int m = 0; m < 3; m++)
            //                                        {
            //                                            // �����ӂ����邩
            //                                            if ((straddlePolyIdx[n + u] == idxList[m] && straddlePolyIdx[n + (u + 1) % 3] == idxList[(m + 1) % 3]) ||
            //                                                (straddlePolyIdx[n + u] == idxList[(m + 1) % 3] && straddlePolyIdx[n + (u + 1) % 3] == idxList[m]))
            //                                            {
            //                                                Debug.Log("���Ȃ����ӂ�����");
            //                                                Debug.Log("�|���S���ԍ���" + straddlePolyIdx[n] + "," + straddlePolyIdx[n + 1] + "," + straddlePolyIdx[n + 2]);
            //                                                Debug.Log("idxList�ԍ���" + idxList[0] + "," + idxList[1] + "," + idxList[2]);

            //                                                var cpEdg_v = cutPoint[cutPoint.Count - 1];
            //                                                var cpEdg_s = cutPoint[cutPoint.Count - 2];
            //                                                var cpEdg_b = cutPoint[cutPoint.Count - 3];
            //                                                var cpEdg_sv = cpEdg_v - cpEdg_s;
            //                                                var cpEdg_bs = cpEdg_s - cpEdg_b;
            //                                                var cpEdg_bv = cpEdg_sv + cpEdg_bs;
            //                                                var cpEdgNor = Vector3.Cross(cpEdg_bv, Vector3.up);

            //                                                // ���_�̒ǉ�
            //                                                vertices1.Add(new Vector3(intersectPolyList[n / 3][0].x, cutPoint[cutPoint.Count - 1].y - transform.position.y, intersectPolyList[n / 3][0].y) + new Vector3(intersectEdgList[n / 3][0].normalized.x * 0.04f * (cpEdgNor.normalized.x / Mathf.Abs(cpEdgNor.normalized.x)), 0, intersectEdgList[n / 3][0].normalized.y * 0.04f * (cpEdgNor.normalized.z / Mathf.Abs(cpEdgNor.normalized.z))));
            //                                                vertices1.Add(new Vector3(intersectPolyList[n / 3][0].x, cutPoint[cutPoint.Count - 1].y - transform.position.y, intersectPolyList[n / 3][0].y) + new Vector3(intersectEdgList[n / 3][0].normalized.x * 0.04f * -(cpEdgNor.normalized.x / Mathf.Abs(cpEdgNor.normalized.x)), 0, intersectEdgList[n / 3][0].normalized.y * 0.04f * -(cpEdgNor.normalized.z / Mathf.Abs(cpEdgNor.normalized.z))));

            //                                                // �C���f�b�N�X�̊��蓖��
            //                                                int idx0 = straddlePolyIdx[n];
            //                                                int idx1 = straddlePolyIdx[n + 1];
            //                                                int idx2 = straddlePolyIdx[n + 2];
            //                                                int idx3 = vertices1.Count - 4; // 7
            //                                                int idx4 = vertices1.Count - 3; // �g��Ȃ�  
            //                                                int idx5 = vertices1.Count - 2; // 6
            //                                                int idx6 = vertices1.Count - 1; // 6

            //                                                // �܂����ł�|���S���̕ӂ̌������Ă���I�_������ӂ̃C���f�b�N�X
            //                                                edgIdx_v[0] = edgIdx2List[n / 3][2];
            //                                                edgIdx_v[1] = edgIdx2List[n / 3][3];


            //                                                // �n����6�����2��s��
            //                                                for (int twice = 0; twice < 2; twice++)
            //                                                {
            //                                                    Debug.Log("edgIdx_s[0]:" + edgIdx_s[0]);
            //                                                    Debug.Log("edgIdx_s[1]:" + edgIdx_s[1]);
            //                                                    Debug.Log("edgIdx_v[0]:" + edgIdx_v[0]);
            //                                                    Debug.Log("edgIdx_v[1]:" + edgIdx_v[1]);

            //                                                    // �n����6����
            //                                                    if ((edgIdx_s[0] == straddlePolyIdx[n] || edgIdx_s[0] == straddlePolyIdx[n + 1]) && (edgIdx_s[1] == straddlePolyIdx[n + 1] || edgIdx_s[1] == straddlePolyIdx[n]) && (edgIdx_v[0] == straddlePolyIdx[n + 1] || edgIdx_v[0] == straddlePolyIdx[n + 2]) && (edgIdx_v[1] == straddlePolyIdx[n + 2] || edgIdx_v[1] == straddlePolyIdx[n + 1]))
            //                                                    {
            //                                                        Debug.Log("�n����6����1");
            //                                                        // �|���S���̐��������[�v
            //                                                        for (int b = 0; b < attachedMesh.triangles.Length; b += 3)
            //                                                        {
            //                                                            if (!(straddlePolyIdx[n] == attachedMesh.triangles[b] && straddlePolyIdx[n + 1] == attachedMesh.triangles[b + 1] && straddlePolyIdx[n + 2] == attachedMesh.triangles[b + 2])) continue;
            //                                                            triangles1.RemoveRange(b, 3);
            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx3);
            //                                                            triangles1.Add(idx1);

            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx2);
            //                                                            triangles1.Add(idx0);

            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx0);
            //                                                            triangles1.Add(idx4);

            //                                                            idxMemory.Add(idx5);
            //                                                            idxMemory.Add(idx3);
            //                                                            idxMemory.Add(idx1);

            //                                                            idxMemory.Add(idx6);
            //                                                            idxMemory.Add(idx2);
            //                                                            idxMemory.Add(idx0);

            //                                                            idxMemory.Add(idx6);
            //                                                            idxMemory.Add(idx0);
            //                                                            idxMemory.Add(idx4);
            //                                                            break;
            //                                                        }
            //                                                        break;
            //                                                    }
            //                                                    else if ((edgIdx_s[0] == straddlePolyIdx[n] || edgIdx_s[0] == straddlePolyIdx[n + 1]) && (edgIdx_s[1] == straddlePolyIdx[n + 1] || edgIdx_s[1] == straddlePolyIdx[n]) &&
            //                                                             (edgIdx_v[0] == straddlePolyIdx[n] || edgIdx_v[0] == straddlePolyIdx[n + 2]) && (edgIdx_v[1] == straddlePolyIdx[n + 2] || edgIdx_v[1] == straddlePolyIdx[n]))
            //                                                    {
            //                                                        Debug.Log("�n����6����2");
            //                                                        // �|���S���̐��������[�v
            //                                                        for (int b = 0; b < attachedMesh.triangles.Length; b += 3)
            //                                                        {
            //                                                            if (!(straddlePolyIdx[n] == attachedMesh.triangles[b] && straddlePolyIdx[n + 1] == attachedMesh.triangles[b + 1] && straddlePolyIdx[n + 2] == attachedMesh.triangles[b + 2])) continue;
            //                                                            triangles1.RemoveRange(b, 3);
            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx0);
            //                                                            triangles1.Add(idx4);

            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx1);
            //                                                            triangles1.Add(idx2);

            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx3);
            //                                                            triangles1.Add(idx1);


            //                                                            idxMemory.Add(idx6);
            //                                                            idxMemory.Add(idx0);
            //                                                            idxMemory.Add(idx4);

            //                                                            idxMemory.Add(idx5);
            //                                                            idxMemory.Add(idx1);
            //                                                            idxMemory.Add(idx2);

            //                                                            idxMemory.Add(idx5);
            //                                                            idxMemory.Add(idx3);
            //                                                            idxMemory.Add(idx1);
            //                                                            break;
            //                                                        }
            //                                                        break;
            //                                                    }
            //                                                    else if ((edgIdx_s[0] == straddlePolyIdx[n + 1] || edgIdx_s[0] == straddlePolyIdx[n + 2]) && (edgIdx_s[1] == straddlePolyIdx[n + 2] || edgIdx_s[1] == straddlePolyIdx[n + 1]) &&
            //                                                                (edgIdx_v[0] == straddlePolyIdx[n] || edgIdx_v[0] == straddlePolyIdx[n + 1]) && (edgIdx_v[1] == straddlePolyIdx[n + 1] || edgIdx_v[1] == straddlePolyIdx[n]))
            //                                                    {
            //                                                        Debug.Log("�n����6����3");
            //                                                        // �|���S���̐��������[�v
            //                                                        for (int b = 0; b < attachedMesh.triangles.Length; b += 3)
            //                                                        {
            //                                                            if (!(straddlePolyIdx[n] == attachedMesh.triangles[b] && straddlePolyIdx[n + 1] == attachedMesh.triangles[b + 1] && straddlePolyIdx[n + 2] == attachedMesh.triangles[b + 2])) continue;
            //                                                            triangles1.RemoveRange(b, 3);
            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx1);
            //                                                            triangles1.Add(idx2);

            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx3);
            //                                                            triangles1.Add(idx2);

            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx2);
            //                                                            triangles1.Add(idx0);

            //                                                            idxMemory.Add(idx6);
            //                                                            idxMemory.Add(idx1);
            //                                                            idxMemory.Add(idx2);

            //                                                            idxMemory.Add(idx5);
            //                                                            idxMemory.Add(idx3);
            //                                                            idxMemory.Add(idx2);

            //                                                            idxMemory.Add(idx5);
            //                                                            idxMemory.Add(idx2);
            //                                                            idxMemory.Add(idx0);
            //                                                            break;
            //                                                        }
            //                                                        break;
            //                                                    }
            //                                                    else if ((edgIdx_s[0] == straddlePolyIdx[n + 1] || edgIdx_s[0] == straddlePolyIdx[n + 2]) && (edgIdx_s[1] == straddlePolyIdx[n + 2] || edgIdx_s[1] == straddlePolyIdx[n + 1]) &&
            //                                                                (edgIdx_v[0] == straddlePolyIdx[n] || edgIdx_v[0] == straddlePolyIdx[n + 2]) && (edgIdx_v[1] == straddlePolyIdx[n + 2] || edgIdx_v[1] == straddlePolyIdx[n]))
            //                                                    {
            //                                                        Debug.Log("�n����6����4");
            //                                                        // �|���S���̐��������[�v
            //                                                        for (int b = 0; b < attachedMesh.triangles.Length; b += 3)
            //                                                        {
            //                                                            if (!(straddlePolyIdx[n] == attachedMesh.triangles[b] && straddlePolyIdx[n + 1] == attachedMesh.triangles[b + 1] && straddlePolyIdx[n + 2] == attachedMesh.triangles[b + 2])) continue;
            //                                                            triangles1.RemoveRange(b, 3);
            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx3);
            //                                                            triangles1.Add(idx2);

            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx1);
            //                                                            triangles1.Add(idx4);

            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx0);
            //                                                            triangles1.Add(idx1);

            //                                                            idxMemory.Add(idx5);
            //                                                            idxMemory.Add(idx3);
            //                                                            idxMemory.Add(idx2);

            //                                                            idxMemory.Add(idx6);
            //                                                            idxMemory.Add(idx1);
            //                                                            idxMemory.Add(idx4);

            //                                                            idxMemory.Add(idx6);
            //                                                            idxMemory.Add(idx0);
            //                                                            idxMemory.Add(idx1);
            //                                                            break;
            //                                                        }
            //                                                        break;
            //                                                    }
            //                                                    else if ((edgIdx_s[0] == straddlePolyIdx[n] || edgIdx_s[0] == straddlePolyIdx[n + 2]) && (edgIdx_s[1] == straddlePolyIdx[n + 2] || edgIdx_s[1] == straddlePolyIdx[n]) &&
            //                                                                (edgIdx_v[0] == straddlePolyIdx[n] || edgIdx_v[0] == straddlePolyIdx[n + 1]) && (edgIdx_v[1] == straddlePolyIdx[n + 1] || edgIdx_v[1] == straddlePolyIdx[n]))
            //                                                    {
            //                                                        Debug.Log("�n����6����5");
            //                                                        // �|���S���̐��������[�v
            //                                                        for (int b = 0; b < attachedMesh.triangles.Length; b += 3)
            //                                                        {
            //                                                            if (!(straddlePolyIdx[n] == attachedMesh.triangles[b] && straddlePolyIdx[n + 1] == attachedMesh.triangles[b + 1] && straddlePolyIdx[n + 2] == attachedMesh.triangles[b + 2])) continue;
            //                                                            triangles1.RemoveRange(b, 3);
            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx3);
            //                                                            triangles1.Add(idx0);

            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx1);
            //                                                            triangles1.Add(idx2);

            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx2);
            //                                                            triangles1.Add(idx4);

            //                                                            idxMemory.Add(idx5);
            //                                                            idxMemory.Add(idx3);
            //                                                            idxMemory.Add(idx0);

            //                                                            idxMemory.Add(idx6);
            //                                                            idxMemory.Add(idx1);
            //                                                            idxMemory.Add(idx2);

            //                                                            idxMemory.Add(idx6);
            //                                                            idxMemory.Add(idx2);
            //                                                            idxMemory.Add(idx4);
            //                                                            break;
            //                                                        }
            //                                                        break;
            //                                                    }
            //                                                    else if ((edgIdx_s[0] == straddlePolyIdx[n] || edgIdx_s[0] == straddlePolyIdx[n + 2]) && (edgIdx_s[1] == straddlePolyIdx[n + 2] || edgIdx_s[1] == straddlePolyIdx[n]) &&
            //                                                                (edgIdx_v[0] == straddlePolyIdx[n + 1] || edgIdx_v[0] == straddlePolyIdx[n + 2]) && (edgIdx_v[1] == straddlePolyIdx[n + 2] || edgIdx_v[1] == straddlePolyIdx[n + 2]))
            //                                                    {
            //                                                        Debug.Log("�n����6����6");
            //                                                        // �|���S���̐��������[�v
            //                                                        for (int b = 0; b < attachedMesh.triangles.Length; b += 3)
            //                                                        {
            //                                                            if (!(straddlePolyIdx[n] == attachedMesh.triangles[b] && straddlePolyIdx[n + 1] == attachedMesh.triangles[b + 1] && straddlePolyIdx[n + 2] == attachedMesh.triangles[b + 2])) continue;
            //                                                            triangles1.RemoveRange(b, 3);
            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx2);
            //                                                            triangles1.Add(idx4);

            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx3);
            //                                                            triangles1.Add(idx0);

            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx0);
            //                                                            triangles1.Add(idx1);

            //                                                            break;
            //                                                        }
            //                                                        break;
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        Debug.Log("aaa");
            //                                                        edgIdx_v[0] = edgIdx2List[n / 3][0];
            //                                                        edgIdx_v[1] = edgIdx2List[n / 3][1];

            //                                                    }

            //                                                }

            //                                                // ��₩��폜

            //                                                edgIdx_s[0] = edgIdx_v[0];
            //                                                edgIdx_s[1] = edgIdx_v[1];
            //                                                idxList.Clear();
            //                                                idxList.Add(straddlePolyIdx[n]);
            //                                                idxList.Add(straddlePolyIdx[n + 1]);
            //                                                idxList.Add(straddlePolyIdx[n + 2]);
            //                                                straddlePolyIdx.RemoveRange(n, 3);
            //                                                intersectPolyList.RemoveRange(n / 3, 1);
            //                                                intersectEdgList.RemoveRange(n / 3, 1);
            //                                                triger = true;
            //                                                break;

            //                                            }

            //                                        }

            //                                        if (triger) break;
            //                                    }


            //                                }

            //                            }
            //                            else
            //                            {
            //                                break;
            //                            }
            //                            whileCnt++;
            //                        }





            //                        // �����܂ŗ�����I��
            //                        //break;

            //                    }

            //                    Debug.Log("interPointCnt" + interPointCnt);

            //                    // �|���S���Ɋ܂܂���_��1�̎�
            //                    if (interPointCnt == 1)
            //                    {
            //                        Debug.Log("1��");

            //                    }
            //                    else if (interPointCnt == 2)
            //                    {   // �|���S���Ɋ܂܂���_��2�̎�
            //                        Debug.Log("2��");
            //                    }

            //                    // �����܂ŗ�����I��
            //                    //break;
            //                }
            //                // �����܂ŗ�����I��
            //                //break;
            //            }


            //            // --- 4�������鏈�� ---
            //            // �J�b�g�|�C���g�̏I�_�����ǂ̃|���S���ɂ��邩���ׂ�
            //            for (int n = 0; n < attachedMesh.triangles.Length; n += 3)
            //            {
            //                //���b�V����3�̒��_���擾
            //                var _p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[n]]);//+ Vector3.one * 0.0001f;
            //                var _p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[n + 1]]);//+ Vector3.one * 0.0001f;
            //                var _p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[n + 2]]); //+ Vector3.one * 0.0001f;

            //                // �J�b�g�|�C���g�̏I�_���|���S���̒��ɂ��邩
            //                double _Area = 0.5 * (-_p1.z * _p2.x + _p0.z * (-_p1.x + _p2.x) + _p0.x * (_p1.z - _p2.z) + _p1.x * _p2.z);
            //                double _s2 = 1 / (2 * _Area) * (_p0.z * _p2.x - _p0.x * _p2.z + (_p2.z - _p0.z) * cutPoint[cutPoint.Count - 1].x + (_p0.x - _p2.x) * cutPoint[cutPoint.Count - 1].z);
            //                double _t2 = 1 / (2 * _Area) * (_p0.x * _p1.z - _p0.z * _p1.x + (_p0.z - _p1.z) * cutPoint[cutPoint.Count - 1].x + (_p1.x - _p0.x) * cutPoint[cutPoint.Count - 1].z);

            //                // �O�p�`�̒��ɂ��邩
            //                if ((0 <= _s2 && _s2 <= 1) && (0 <= _t2 && _t2 <= 1) && (0 <= 1 - _s2 - _t2 && 1 - _s2 - _t2 <= 1))
            //                {
            //                    // ����Ƃ�
            //                    Debug.Log("�|���S���ԍ�" + attachedMesh.triangles[n] + "," + attachedMesh.triangles[n + 1] + "," + attachedMesh.triangles[n + 2]);

            //                    // �x�N�g���̌W���̌v�Z
            //                    double _s3 = 1 / (2 * _Area) * (_p0.z * _p2.x - _p0.x * _p2.z + (_p2.z - _p0.z) * (_p.x + transform.position.x) + (_p0.x - _p2.x) * (_p.y + transform.position.z));
            //                    double _t3 = 1 / (2 * _Area) * (_p0.x * _p1.z - _p0.z * _p1.x + (_p0.z - _p1.z) * (_p.x + transform.position.x) + (_p1.x - _p0.x) * (_p.y + transform.position.z));

            //                    // �J�b�g�|�C���g�̏I�_�ɒ��_�̒ǉ�
            //                    vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);
            //                    vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);

            //                    // ��_���ǂ̃|���S���̕ӂɂ���̂�
            //                    if (_t3 < 0.002f) // ��S��
            //                    {
            //                        Debug.Log("��S��");

            //                        // �C���f�b�N�X�̊��蓖��
            //                        int idx0 = attachedMesh.triangles[n];
            //                        int idx1 = attachedMesh.triangles[n + 1];
            //                        int idx2 = attachedMesh.triangles[n + 2];
            //                        int idx3 = vertices1.Count - 4; // 
            //                        int idx4 = vertices1.Count - 3; // 
            //                        int idx5 = vertices1.Count - 2; // 
            //                        int idx6 = vertices1.Count - 1; // 


            //                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
            //                        triangles1.RemoveRange(i, 3);

            //                        // �C���f�b�N�X�̐U�蕪��
            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx2);
            //                        triangles1.Add(idx0);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx0);
            //                        triangles1.Add(idx4);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx3);
            //                        triangles1.Add(idx1);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx1);
            //                        triangles1.Add(idx2);

            //                        // �o�����O�p�`�C���f�b�N�X�̕ۑ�
            //                        idxMemory.Clear();
            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx2);
            //                        idxMemory.Add(idx0);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx0);
            //                        idxMemory.Add(idx4);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx3);
            //                        idxMemory.Add(idx1);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx1);
            //                        idxMemory.Add(idx2);
            //                    }
            //                    else if (_s3 < 0.002f)    // ��T��
            //                    {
            //                        Debug.Log("��T��");

            //                        // �C���f�b�N�X�̊��蓖��
            //                        int idx0 = attachedMesh.triangles[n];
            //                        int idx1 = attachedMesh.triangles[n + 1];
            //                        int idx2 = attachedMesh.triangles[n + 2];
            //                        int idx3 = vertices1.Count - 4; // 
            //                        int idx4 = vertices1.Count - 3; // 
            //                        int idx5 = vertices1.Count - 2; // 
            //                        int idx6 = vertices1.Count - 1; // 

            //                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
            //                        triangles1.RemoveRange(n, 3);

            //                        // �C���f�b�N�X�̐U�蕪��
            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx1);
            //                        triangles1.Add(idx2);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx2);
            //                        triangles1.Add(idx4);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx3);
            //                        triangles1.Add(idx0);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx0);
            //                        triangles1.Add(idx1);

            //                        // �o�����O�p�`�C���f�b�N�X�̕ۑ�
            //                        idxMemory.Clear();
            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx1);
            //                        idxMemory.Add(idx2);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx2);
            //                        idxMemory.Add(idx4);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx3);
            //                        idxMemory.Add(idx0);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx0);
            //                        idxMemory.Add(idx1);

            //                    }
            //                    else if (_s3 + _t3 > 0.98f) // ��S+T��
            //                    {
            //                        Debug.Log("��S + T��");

            //                        // �C���f�b�N�X�̊��蓖��
            //                        int idx0 = attachedMesh.triangles[n];
            //                        int idx1 = attachedMesh.triangles[n + 1];
            //                        int idx2 = attachedMesh.triangles[n + 2];
            //                        int idx3 = vertices1.Count - 4; // 
            //                        int idx4 = vertices1.Count - 3; // 
            //                        int idx5 = vertices1.Count - 2; // 
            //                        int idx6 = vertices1.Count - 1; // 

            //                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
            //                        triangles1.RemoveRange(i, 3);

            //                        // �C���f�b�N�X�̐U�蕪��
            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx0);
            //                        triangles1.Add(idx1);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx1);
            //                        triangles1.Add(idx4);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx3);
            //                        triangles1.Add(idx2);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx2);
            //                        triangles1.Add(idx0);

            //                        // �o�����O�p�`�C���f�b�N�X�̕ۑ�
            //                        idxMemory.Clear();
            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx0);
            //                        idxMemory.Add(idx1);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx1);
            //                        idxMemory.Add(idx4);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx3);
            //                        idxMemory.Add(idx2);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx2);
            //                        idxMemory.Add(idx0);
            //                    }
            //                    else
            //                    {
            //                        Debug.Log("�ǂ̕ӂɂ��Ȃ�");
            //                    }

            //                }


            //            }


            //        }



            //        Debug.Log(idxMemory.Count);


            //    }
            //}

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

        var mesh = new Mesh();
        mesh.vertices = vertices1.ToArray();
        mesh.triangles = triangles1.ToArray();
        mesh.normals = normals1.ToArray();

        attachedMesh = mesh;
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        return true;
    }

    // ���b�V���̕���(�Ō�)
    public void DivisionMeshTwice(List<Vector3> cutPoint)
    {
        Debug.Log("---------�Ō�̏���---------");
        // ���b�V���̃A�^�b�`
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;

        // �ϐ�
        Vector3 p0, p1, p2;    // ���b�V���̃|���S���̒��_
        var uvs1 = new List<Vector2>(); // �e�N�X�`��
        var vertices1 = new List<Vector3>();   // ���_
        var triangles1 = new List<int>();       // �O�p�`�C���f�b�N�X
        var normals1 = new List<Vector3>();     // �@��
        Vector3 edge = new Vector3();
        Vector3 edge1 = new Vector3();
        Vector3 edge2 = new Vector3();
        Vector3 edge3 = new Vector3();

        // ���_�ƃC���f�b�N�X�̑��
        for (int i = 0; i < attachedMesh.vertices.Length; i++)
        {
            vertices1.Add(attachedMesh.vertices[i]);
        }
        for (int i = 0; i < attachedMesh.triangles.Length; i++)
        {
            triangles1.Add(attachedMesh.triangles[i]);
        }

        // �J�b�g�|�C���g�̏ꏊ�ɒ��_�̒ǉ�(���Ƃŕ����邽�ߓ�ǉ�)
        vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);
        vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);

        // �������W�ɒ��_����������L����
        for (int i = 0; i < vertices1.Count - 1; i++)
        {
            // �������W����Ȃ������X���[
            if (vertices1[i] != vertices1[i + 1]) continue;


            // �؂�����ɑ΂��ē_���ړ�����߂̏���
            edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 3];
            edge2 = cutPoint[cutPoint.Count - 1] - cutPoint[cutPoint.Count - 2];
            edge3 = edge1 + edge2;


            // �J�b�g�|�C���g���꒼����������
            // �����ɓ_���L����
            if (edge3 == Vector3.zero)
            {
                edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
                edge2 = (cutPoint[cutPoint.Count - 1] + Vector3.up) - cutPoint[cutPoint.Count - 1];
                edge = Vector3.Cross(edge2, edge1);
            }
            else
            {
                edge = Vector3.Cross(edge3, Vector3.up);

            }

            // ���_�Ɋi�[
            vertices1[i] = vertices1[i] + edge.normalized * 0.04f;
            vertices1[i + 1] = vertices1[i + 1] - edge.normalized * 0.04f;
        }

        // �J�b�g���鏈��
        {
            // �ϐ��錾              
            var straddlePolyIdx = new List<int>();  // �܂������|���S���ԍ����X�g
            var crossPolyIdx = new List<int>();     // �����|���S���ԍ����X�g
            var inerPolyIdx = new List<int>();      // �J�b�g�|�C���g�����ɓ����Ă���|���S���ԍ�
            var intersectPolyList = new List<List<Vector2>>();  // �|���S�����Ƃɂ���������Ă���_�̃��X�g
            var intersectPolyList2 = new List<List<Vector2>>();  // �|���S�����Ƃɂ���������Ă���_�̃��X�g2
            var intersectEdgList = new List<List<Vector2>>();   // �|���S�����Ƃɂ���������Ă���ӂ̃��X�g
            var intersectEdgList2 = new List<List<Vector2>>();   // �|���S�����Ƃɂ���������Ă���ӂ̃��X�g
            var intersectionList = new List<Vector2>();         // ��_�̃��X�g
            var cp_s = new Vector2(cutPoint[cutPoint.Count - 2].x, cutPoint[cutPoint.Count - 2].z);    // �J�b�g�|�C���g�̏I�_��1�O
            var cp_v = new Vector2(cutPoint[cutPoint.Count - 1].x, cutPoint[cutPoint.Count - 1].z);    // �J�b�g�|�C���g�̏I�_
            var cpEdg = cp_v - cp_s;    // �J�b�g�|�C���g�̏I�_�ƃJ�b�g�|�C���g�̏I�_��1�O���Ȃ�����
            var checkCp = cp_s + cpEdg * 0.01f; // �J�b�g�|�C���g�̏I�_��1�O����J�b�g�|�C���g�̏I�_�̕����ɂ�����ƐL�΂����_
            var edgIdx2List = new List<List<int>>();   // �ӂ̃C���f�b�N�X�̃��X�g�̃��X�g   
            var edgIdx2List2 = new List<List<int>>();  // �ӂ̃C���f�b�N�X�̃��X�g�̃��X�g2   

            // �܂����ł�|���S���ƐN�����Ă���|���S���������邩�T��
            for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
            {
                // �ϐ��錾             
                int interPointCnt = 0; // ���������_�̐�
                var intersection = new List<Vector2>(); // ��_�̃��X�g
                var edgList = new List<Vector2>(); //�ӂ̃��X�g
                var edgIdxList = new List<int>();   // �ӂ̃C���f�b�N�X�̃��X�g   

                // �|���S���̕ӂ̐��������[�v
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

                    // �J�b�g�|�C���g�̎n�_�̕␳
                    cpVtx_s += cpEdge * 0.01f;

                    // �J�b�g�|�C���g�̕ӂ̕␳
                    cpEdge = cpVtx_v - cpVtx_s; // ��

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
                        // ������ĂȂ��Ƃ��X���[
                        continue;
                    }
                    else
                    {
                        // ������Ă鎞��_�J�E���g++                               
                        interPointCnt++;    // ��_�J�E���g    
                        intersection.Add(p);    // ��_�̕ۑ�
                        intersectionList.Add(p);// ��_�̕ۑ�
                        edgList.Add(polyEdge);
                        edgIdxList.Add(attachedMesh.triangles[j + k]);
                        edgIdxList.Add(attachedMesh.triangles[j + (k + 1) % 3]);

                    }
                }

                // �|���S���ԍ���ۑ�
                if (interPointCnt == 2)// ��_�J�E���g2��(�|���S�����܂����ł鎞)
                {
                    Debug.Log("2�����");
                    //Debug.Log("�|���S���ԍ���" + attachedMesh.triangles[j] + "," + attachedMesh.triangles[j + 1] + "," + attachedMesh.triangles[j + 2]);

                    straddlePolyIdx.Add(attachedMesh.triangles[j]);
                    straddlePolyIdx.Add(attachedMesh.triangles[j + 1]);
                    straddlePolyIdx.Add(attachedMesh.triangles[j + 2]);
                    crossPolyIdx.Add(j);
                    crossPolyIdx.Add(j + 1);
                    crossPolyIdx.Add(j + 2);
                    intersectPolyList2.Add(intersection);
                    intersectEdgList2.Add(edgList);
                    edgIdx2List2.Add(edgIdxList);
                    //Debug.Log("straddlePolyIdx.Count" + straddlePolyIdx.Count);
                }
                else if (interPointCnt == 1)// ��_�J�E���g1��(�J�b�g�|�C���g�̏I�_���|���S���̒��ɂ���Ƃ�)
                {
                    Debug.Log("1�����");
                    inerPolyIdx.Add(attachedMesh.triangles[j]);
                    inerPolyIdx.Add(attachedMesh.triangles[j + 1]);
                    inerPolyIdx.Add(attachedMesh.triangles[j + 2]);
                    crossPolyIdx.Add(j);
                    crossPolyIdx.Add(j + 1);
                    crossPolyIdx.Add(j + 2);
                    edgIdx2List.Add(edgIdxList);
                    intersectPolyList.Add(intersection);
                    intersectEdgList.Add(edgList);
                }
                else
                {
                    // Debug.Log("3�����");
                    // Debug.Log("�|���S���ԍ���" + attachedMesh.triangles[j] + "," + attachedMesh.triangles[j + 1] + "," + attachedMesh.triangles[j + 2]);

                }
            }

            //--- �J�b�g�|�C���g�Ƃ̌�_�̐��ŕ��� ---
            // ��_��1�̎�(�|���S���̒���2��_������|���S����1���Ȃ��Ƃ�)
            if (straddlePolyIdx.Count == 0 && inerPolyIdx.Count > 0)
            {
                Debug.Log("��_��1�̎�");

                //--- �ϐ��錾 ---
                int firstNum = 0;
                int secondNum = 0;
                Vector2 cpS = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z);
                Vector2 cpV = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z);
                Vector2 p = cpV;
                var idxList2 = new List<int>();  // 1�O�ɕ��������C���f�b�N�X�̃��X�g
                var rastIdxList = new List<int>();  // 2�ɕ������鏈���������Ō�̃C���f�b�N�X�ۑ��p�̃��X�g             
               
                // 2�������鏈��(�ŏ�)
                {
                    Debug.Log("2�������鏈��");

                    //--- �L�����ꂽ�O�p�`�C���f�N�X�����ƂɃC���f�b�N�X������U�� ---
                    // �L�����ꂽ�O�p�`�C���f�b�N�X�̐��������[�v
                    for (int a = 0; a < idxMemory.Count; a += 3)
                    {
                        bool end3 = false;
                        // �����Ώۂ̃|���S���̐��������[�v
                        for (int w = 0; w < inerPolyIdx.Count; w += 3)
                        {
                            // �L�����ꂽ�C���f�b�N�X�ƈ�v���Ȃ�������X���[
                            if (!(inerPolyIdx[w] == idxMemory[a] && inerPolyIdx[w + 1] == idxMemory[a + 1] && inerPolyIdx[w + 2] == idxMemory[a + 2])) continue;
                            Debug.Log("intersectPolyList[w/3][0]:" + intersectPolyList[w / 3][0]);
                            Debug.Log("intersectEdgList[w/3][0]:" + intersectEdgList[w / 3][0]);

                            //--- �ϐ��錾 ---
                            var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
                            var cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));
                            var pEnd = intersectPolyList[w / 3][0];    // ��_�̏I�_
                            var pEdge = new Vector2(Mathf.Abs(intersectEdgList[w / 3][0].x), Mathf.Abs(intersectEdgList[w / 3][0].y));

                            // ���ɒǉ�
                            //idxList2.Clear();
                            //idxList2.Add(edgIdx2List[w / 3][0]);
                            //idxList2.Add(edgIdx2List[w / 3][1]);

                            // ���ǉ����ꂽ���ƌ�_�������|���S���̕ӂƔ�r
                            //for (int g = 0; g < straddlePolyIdx.Count; g += 3)
                            //{
                            //    bool end2 = false;
                            //    // �ӂ̐��������[�v
                            //    for (int f = 0; f < 3; f++)
                            //    {
                            //        // ��v���Ȃ�������X���[
                            //        if (((straddlePolyIdx[g + f] == idxList2[0] || straddlePolyIdx[g + f] == idxList2[1]) && (straddlePolyIdx[g + (f + 1) % 3] == idxList2[1] || straddlePolyIdx[g + (f + 1) % 3] == idxList2[0]))) continue;

                            //        Debug.Log("���ɒǉ�");
                            //        firstNum = g;
                            //        end2 = true;
                            //        break;
                            //    }
                            //    if (end2) break;
                            //}

                            // ���b�V���̃|���S���̐��������[�v
                            for (int c = 0; c < triangles1.Count; c += 3)
                            {
                                // ��v���Ȃ�������X���[
                                if (!(triangles1[c] == inerPolyIdx[w] && triangles1[c + 1] == inerPolyIdx[w + 1] && triangles1[c + 2] == inerPolyIdx[w + 2])) continue;
                                Debug.Log("���_�̒ǉ�");
                                // ��_�����Ƃɒ��_��ǉ�
                                //vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * -cpNormalAbs.z));
                                //vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge.normalized.x * 0.04f * cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * cpNormalAbs.z));

                                // �C���f�b�N�X�̊��蓖��
                                int idx0 = attachedMesh.triangles[c];
                                int idx1 = attachedMesh.triangles[c + 1];
                                int idx2 = attachedMesh.triangles[c + 2];
                                int idx3 = vertices1.Count - 4; // 7
                                int idx4 = vertices1.Count - 3; // 
                                int idx5 = vertices1.Count - 2; // 6
                                int idx6 = vertices1.Count - 1; // 6
                                int here = 0;   // �L�����ꂽ�C���f�b�N�X�̐擪���i�[����ϐ�

                                // ��v�����L�����ꂽ�擪�̃C���f�b�N�X���S�̂̂ǂ̃C���f�b�N�X�ɂ���̂�
                                for (int z = 0; z < triangles1.Count; z += 3)
                                {
                                    // ��v���Ȃ�������X���[
                                    if (!(idxMemory[a] == triangles1[z] && idxMemory[a + 1] == triangles1[z + 1] && idxMemory[a + 2] == triangles1[z + 2])) continue;

                                    // �L�����ꂽ�C���f�b�N�X�̐擪���i�[
                                    here = z - a;
                                }

                                Debug.Log("idxMemory.Count" + idxMemory.Count);
                                // �L�����ꂽ�C���f�b�N�X�̐��ɂ���ĕ���
                                // �L�����ꂽ�C���f�b�N�X��12��(�|���S����4��)�̎�
                                if (idxMemory.Count == 12)
                                {
                                    // �C���f�b�N�X�̊���U�蕪��
                                    if (a == 0)
                                    {
                                        Debug.Log("a = 0");

                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here + 3] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 3)
                                    {
                                        Debug.Log("a = 3");

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 6)
                                    {
                                        Debug.Log("a = 6");

                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;
                                        triangles1[here + 9] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        // �����܂ŗ�����O�p�`��񓙕�����̂͏I��
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 9)
                                    {
                                        Debug.Log("a = 9");
                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        // �����܂ŗ�����O�p�`��񓙕�����̂͏I��
                                        end3 = true;
                                        break;
                                    }
                                }
                                // �L�����ꂽ�C���f�b�N�X��9��(�|���S����3��)�̎�
                                else if (idxMemory.Count == 9)
                                {
                                    // �C���f�b�N�X�̊���U�蕪��
                                    if (a == 0)
                                    {
                                        Debug.Log("a = 0");

                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here + 3] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 3)
                                    {
                                        Debug.Log("a = 3");

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 6)
                                    {
                                        Debug.Log("a = 6");

                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }

                                }


                            }

                            // ��_����̃|���S���̌��̒����獡��Ŏg�����|���S�����폜
                            //inerPolyIdx.RemoveRange(w, 3);
                            //edgIdx2List.RemoveRange(w/3, 1);
                            //intersectPolyList.RemoveRange(w/3, 1);
                            //intersectEdgList.RemoveRange(w/3, 1);
                            if (end3) break;
                        }
                        if (end3) break;
                    }
                }
                {
                    //// ���b�V���̃|���S���̐��������[�v
                    //for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
                    //{
                    //    //���b�V����3�̒��_���擾
                    //    p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);//+ Vector3.one * 0.0001f;
                    //    p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);//+ Vector3.one * 0.0001f;
                    //    p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]); //+ Vector3.one * 0.0001f;

                    //    // �J�b�g�|�C���g�̏I�_���|���S���̒��ɂ��邩
                    //    Vector2 cp = new Vector2(cutPoint[cutPoint.Count - 2].x + (cutPoint[cutPoint.Count - 1].x - cutPoint[cutPoint.Count - 2].x) * 0.40f - transform.position.x, cutPoint[cutPoint.Count - 2].z + (cutPoint[cutPoint.Count - 1].z - cutPoint[cutPoint.Count - 2].z) * 0.40f - transform.position.z);
                    //    var v2P0 = new Vector2(p0.x, p0.z);
                    //    var v2P1 = new Vector2(p1.x, p1.z);
                    //    var v2P2 = new Vector2(p2.x, p2.z);

                    //    double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
                    //    double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * (cp.x + transform.position.x) + (p0.x - p2.x) * (cp.y + transform.position.z));
                    //    double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * (cp.x + transform.position.x) + (p1.x - p0.x) * (cp.y + transform.position.z));
                    //    // �O�p�`�̒��ɂ��邩
                    //    if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
                    //    {
                    //        Debug.Log("�|���S���̒��ɂ���");
                    //        //vertices1.Add(new Vector3(cp.x, attachedMesh.vertices[0].y, cp.y));
                    //        // �C���f�b�N�X�̊��蓖��
                    //        int _0 = attachedMesh.triangles[i];
                    //        int _1 = attachedMesh.triangles[i + 1];
                    //        int _2 = attachedMesh.triangles[i + 2];
                    //        int _3 = vertices1.Count - 2; // 7
                    //        int _4 = vertices1.Count - 1; // �g��Ȃ�  
                    //        int _5 = vertices1.Count - 3; // 6

                    //        // �L�����ꂽ�O�p�`�C���f�b�N�X�̐��������[�v
                    //        for (int j = 0; j < idxMemory.Count; j += 3)
                    //        {
                    //            if (attachedMesh.triangles[i] == idxMemory[j] && attachedMesh.triangles[i + 1] == idxMemory[j + 1] && attachedMesh.triangles[i + 2] == idxMemory[j + 2])
                    //            {
                    //                if (j == 0)
                    //                {
                    //                    Debug.Log("j = 0");
                    //                    // �C���f�b�N�X�̕ύX
                    //                    triangles1[i + j + 3] = _5;

                    //                    // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�

                    //                    triangles1.RemoveRange(i, 3);

                    //                    // �O�p�`�C���f�b�N�X�̐U�蕪��
                    //                    triangles1.Add(_4);
                    //                    triangles1.Add(_2);
                    //                    triangles1.Add(_5);

                    //                    triangles1.Add(_3);
                    //                    triangles1.Add(_0);
                    //                    triangles1.Add(_1);

                    //                    // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                    //                    idxMemory.Clear();

                    //                    idxMemory.Add(_3);
                    //                    idxMemory.Add(_2);
                    //                    idxMemory.Add(_5);

                    //                    idxMemory.Add(_3);
                    //                    idxMemory.Add(_0);
                    //                    idxMemory.Add(_1);
                    //                    break;
                    //                }
                    //                if (j == 3)
                    //                {
                    //                    Debug.Log("j = 3");
                    //                    Debug.Log("���A��������ˁH");

                    //                    triangles1[i + j] = _5;
                    //                    //triangles1[i  - 3] = _5;

                    //                    //triangles1[i + j] = _5;
                    //                    // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                    //                    triangles1.RemoveRange(i, 3);
                    //                    triangles1.Add(_4);
                    //                    triangles1.Add(_2);
                    //                    triangles1.Add(_5);

                    //                    triangles1.Add(_3);
                    //                    triangles1.Add(_0);
                    //                    triangles1.Add(_1);

                    //                    // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                    //                    idxMemory.Clear();
                    //                    idxMemory.Add(_4);
                    //                    idxMemory.Add(_2);
                    //                    idxMemory.Add(_5);

                    //                    idxMemory.Add(_3);
                    //                    idxMemory.Add(_0);
                    //                    idxMemory.Add(_1);
                    //                    break;
                    //                }
                    //                if (j == 6)
                    //                {
                    //                    Debug.Log("j = 6");
                    //                    Debug.Log("j = " + j);
                    //                    Debug.Log("j + i = " + (j + i));
                    //                    Debug.Log("2���");

                    //                    // �C���f�b�N�X�̕ύX

                    //                    triangles1[i - 3] = _5;
                    //                    triangles1[i - 6] = _5;
                    //                    //triangles1[i + j + 3] = _5;

                    //                    // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                    //                    triangles1.RemoveRange(i, 3);
                    //                    triangles1.Add(_3);
                    //                    triangles1.Add(_1);
                    //                    triangles1.Add(_2);

                    //                    triangles1.Add(_3);
                    //                    triangles1.Add(_2);
                    //                    triangles1.Add(_5);

                    //                    triangles1.Add(_3);
                    //                    triangles1.Add(_0);
                    //                    triangles1.Add(_1);

                    //                    // �o�����O�p�`�C���f�b�N�X�̕ۑ�
                    //                    idxMemory.Clear();
                    //                    idxMemory.Add(_3);
                    //                    idxMemory.Add(_1);
                    //                    idxMemory.Add(_2);

                    //                    idxMemory.Add(_3);
                    //                    idxMemory.Add(_2);
                    //                    idxMemory.Add(_5);

                    //                    idxMemory.Add(_3);
                    //                    idxMemory.Add(_0);
                    //                    idxMemory.Add(_1);
                    //                    break;
                    //                }
                    //            }

                    //        }

                    //    }

                    //    //Debug.Log("���_�̒ǉ�");
                    //    //vertices1.Add(new Vector3(cp.x, attachedMesh.vertices[0].y, cp.y));


                    //}

                }

            }
            // ��_��2�̎�(�܂����ł�|���S��������Ƃ�)
            else if (straddlePolyIdx.Count > 0)
            {
                Debug.Log("============= ��_��2�̎� =============");
                Debug.Log("straddlePolyIdx.Count" + straddlePolyIdx.Count);
                //--- �ϐ��錾 ---
                int firstNum = 0;
                int secondNum = 0;
                Vector2 cpS = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z);
                Vector2 cpV = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z);
                Vector2 p = cpV;
                var idxList2 = new List<int>();  // 1�O�ɕ��������C���f�b�N�X�̃��X�g
                var rastIdxList = new List<int>();  // 2�ɕ������鏈���������Ō�̃C���f�b�N�X�ۑ��p�̃��X�g             
                // 2�������鏈��(�ŏ�)
                {
                    Debug.Log("2�������鏈��");

                    //--- �L�����ꂽ�O�p�`�C���f�N�X�����ƂɃC���f�b�N�X������U�� ---
                    // �L�����ꂽ�O�p�`�C���f�b�N�X�̐��������[�v
                    for (int a = 0; a < idxMemory.Count; a += 3)
                    {
                        bool end3 = false;
                        // �����Ώۂ̃|���S���̐��������[�v
                        for (int w = 0; w < inerPolyIdx.Count; w += 3)
                        {
                            // �L�����ꂽ�C���f�b�N�X�ƈ�v���Ȃ�������X���[
                            if (!(inerPolyIdx[w] == idxMemory[a] && inerPolyIdx[w + 1] == idxMemory[a + 1] && inerPolyIdx[w + 2] == idxMemory[a + 2])) continue;
                            Debug.Log("intersectPolyList[w/3][0]:" + intersectPolyList[w / 3][0]);
                            Debug.Log("intersectEdgList[w/3][0]:" + intersectEdgList[w / 3][0]);

                            //--- �ϐ��錾 ---
                            var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
                            var cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));
                            var pEnd = intersectPolyList[w / 3][0];    // ��_�̏I�_
                            var pEdge = new Vector2(Mathf.Abs(intersectEdgList[w / 3][0].x), Mathf.Abs(intersectEdgList[w / 3][0].y));

                            // ���ɒǉ�
                            idxList2.Add(edgIdx2List[w / 3][0]);
                            idxList2.Add(edgIdx2List[w / 3][1]);

                            // ���ǉ����ꂽ���ƌ�_�������|���S���̕ӂƔ�r
                            for (int g = 0; g < straddlePolyIdx.Count; g += 3)
                            {
                                bool end2 = false;
                                // �ӂ̐��������[�v
                                for (int f = 0; f < 3; f++)
                                {
                                    // ��v���Ȃ�������X���[
                                    if (((straddlePolyIdx[g + f] == idxList2[0] || straddlePolyIdx[g + f] == idxList2[1]) && (straddlePolyIdx[g + (f + 1) % 3] == idxList2[1] || straddlePolyIdx[g + (f + 1) % 3] == idxList2[0]))) continue;

                                    Debug.Log("���ɒǉ�");
                                    firstNum = g;
                                    end2 = true;
                                    break;
                                }
                                if (end2) break;
                            }

                            // ���b�V���̃|���S���̐��������[�v
                            for (int c = 0; c < triangles1.Count; c += 3)
                            {
                                // ��v���Ȃ�������X���[
                                if (!(triangles1[c] == inerPolyIdx[w] && triangles1[c + 1] == inerPolyIdx[w + 1] && triangles1[c + 2] == inerPolyIdx[w + 2])) continue;
                                Debug.Log("���_�̒ǉ�");
                                // ��_�����Ƃɒ��_��ǉ�
                                //vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * -cpNormalAbs.z));
                                //vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge.normalized.x * 0.04f * cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * cpNormalAbs.z));

                                // �C���f�b�N�X�̊��蓖��
                                int idx0 = attachedMesh.triangles[c];
                                int idx1 = attachedMesh.triangles[c + 1];
                                int idx2 = attachedMesh.triangles[c + 2];
                                int idx3 = vertices1.Count - 4; // 7
                                int idx4 = vertices1.Count - 3; // 
                                int idx5 = vertices1.Count - 2; // 6
                                int idx6 = vertices1.Count - 1; // 6
                                int here = 0;   // �L�����ꂽ�C���f�b�N�X�̐擪���i�[����ϐ�

                                // ��v�����L�����ꂽ�擪�̃C���f�b�N�X���S�̂̂ǂ̃C���f�b�N�X�ɂ���̂�
                                for (int z = 0; z < triangles1.Count; z += 3)
                                {
                                    // ��v���Ȃ�������X���[
                                    if (!(idxMemory[a] == triangles1[z] && idxMemory[a + 1] == triangles1[z + 1] && idxMemory[a + 2] == triangles1[z + 2])) continue;

                                    // �L�����ꂽ�C���f�b�N�X�̐擪���i�[
                                    here = z - a;
                                }
                              
                                // �L�����ꂽ�C���f�b�N�X�̐��ɂ���ĕ���
                                // �L�����ꂽ�C���f�b�N�X��12��(�|���S����4��)�̎�
                                if (idxMemory.Count == 12)
                                {
                                    // �C���f�b�N�X�̊���U�蕪��
                                    if (a == 0)
                                    {
                                        Debug.Log("a = 0");

                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here + 3] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 3)
                                    {
                                        Debug.Log("a = 3");

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 6)
                                    {
                                        Debug.Log("a = 6");

                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;
                                        triangles1[here + 9] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        // �����܂ŗ�����O�p�`��񓙕�����̂͏I��
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 9)
                                    {
                                        Debug.Log("a = 9");
                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        // �����܂ŗ�����O�p�`��񓙕�����̂͏I��
                                        end3 = true;
                                        break;
                                    }
                                }
                                // �L�����ꂽ�C���f�b�N�X��9��(�|���S����3��)�̎�
                                else if (idxMemory.Count == 9)
                                {
                                    // �C���f�b�N�X�̊���U�蕪��
                                    if (a == 0)
                                    {
                                        Debug.Log("a = 0");

                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here + 3] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 3)
                                    {
                                        Debug.Log("a = 3");

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 6)
                                    {
                                        Debug.Log("a = 6");

                                        // �C���f�b�N�X�̕ύX
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;

                                        // �J�b�g�|�C���g�̂���|���S���̃C���f�b�N�X�̍폜&�ǉ�       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }

                                }
                            }

                            // ��_��2�̃|���S���̌��̒����獡��Ŏg�����|���S�����폜
                            //inerPolyIdx.RemoveRange(w, 3);
                            //edgIdx2List.RemoveRange(w/3, 1);
                            //intersectPolyList.RemoveRange(w/3, 1);
                            //intersectEdgList.RemoveRange(w/3, 1);
                            if (end3) break;
                        }
                        if (end3) break;
                    }
                }

                Debug.Log("idxList2.Count" + idxList2.Count);

                // �L�����ꂽ�C���f�b�N�X�ƈ�v���Ȃ�������
                if (idxList2.Count == 0)
                {
                    Debug.Log("�L�����ꂽ�C���f�b�N�X�ƈ�v���Ȃ�������");
                    var point = intersectPolyList2[0][0];
                    int first = 0;
                    int second = 0;
                    for (int h = 0; h < straddlePolyIdx.Count; h += 3)
                    {
                        for (int v = 0; v < 2; v++)
                        {
                            if (Vector2.Distance(new Vector2(cutPoint[cutPoint.Count - 1].x, cutPoint[cutPoint.Count - 1].z), point) > Vector2.Distance(new Vector2(cutPoint[cutPoint.Count - 1].x, cutPoint[cutPoint.Count - 1].z), intersectPolyList2[h / 3][v])) continue;
                            point = intersectPolyList2[h / 3][v];
                            first = h;
                            second = v;
                        }
                    }

                    Debug.Log("first / 3 :" + first / 3);
                    Debug.Log("edgIdx2List.Count" + edgIdx2List.Count);
                    Debug.Log("edgIdx2List[first / 3][0]" + edgIdx2List[first / 3][0]);

                    // �n�_���ǂ����ɂ��邩�ŕ���
                    if (second == 0)
                    {
                        // ���ɒǉ�
                        idxList2.Add(edgIdx2List[first / 3][0]);
                        idxList2.Add(edgIdx2List[first / 3][1]);

                    }
                    else if (second == 1)
                    {
                        // ���ɒǉ�
                        idxList2.Add(edgIdx2List[first / 3][0]);
                        idxList2.Add(edgIdx2List[first / 3][1]);

                    }


                }

                // 2�������鏈��(�r��)
                {
                    Debug.Log("2�������鏈��(�r��)");
                    //--- �ϐ��錾 ---
                    int count = 0;
                    var idxCnt = straddlePolyIdx;
                    //idxCnt.RemoveRange(firstNum * 3, 3); // ���ۑ��p
                    //Debug.Log("idxCnt.Count" + idxCnt.Count);
                    //Debug.Log("idxList.Count" + idxList2.Count);
                    //Debug.Log("idxCnt.Count" + idxCnt.Count);

                    // ��_��2����|���S���̌�₪�Ȃ��Ȃ邩�J�E���g�����ȏ�ɂȂ�܂Ń��[�v
                    while (count < 100 && idxCnt.Count > 0)
                    {
                        Debug.Log("================= ���[�v ================= ");

                        bool end3 = false;

                        // ���̐��������[�v
                        for (int k = 0; k < idxCnt.Count; k += 3)
                        {
                            // �ӂ̐��������[�v
                            for (int h = 0; h < 3; h++)
                            {
                                Debug.Log("idxCnt[k + h];" + idxCnt[k + h]);
                                Debug.Log("idxCnt[k + ((h + 1) % 3)];" + idxCnt[k + ((h + 1) % 3)]);
                                Debug.Log("idxList[0];" + idxList2[0]);
                                Debug.Log("idxList[1];" + idxList2[1]);

                                // ���ƈ�v���Ȃ�������X���[�A��v�����番���Ώۂ̃C���f�b�N�X��������
                                if (!((idxCnt[k + h] == idxList2[0] || idxCnt[k + h] == idxList2[1]) && (idxCnt[k + ((h + 1) % 3)] == idxList2[0] || straddlePolyIdx[k + ((h + 1) % 3)] == idxList2[1]))) continue;

                                // �ۑ����ꂽ��⃊�X�g���獡��g�����C���f�b�N�X���폜
                                for (int g = 0; g < straddlePolyIdx.Count; g += 3)
                                {
                                    // ���ƈ�v���Ȃ�������X���[
                                    if (!(idxCnt[k] == straddlePolyIdx[g] && idxCnt[k + 1] == straddlePolyIdx[g + 1] && idxCnt[k + 2] == straddlePolyIdx[g + 2])) continue;
                                   
                                    // �|���S���̃C���f�b�N�X�̍ŏ��̔ԍ�
                                    firstNum = g / 3;

                                    // �ϐ��錾
                                    var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
                                    var cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));
                                    var pEnd = intersectPolyList2[firstNum][1];    // ��_�̏I�_
                                    var pEdge = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][secondNum].x), Mathf.Abs(intersectEdgList2[firstNum][secondNum].y));
                                    var pEdge2 = new Vector2();     // ��_�̏I�_�̕Ӄx�N�g��


                                    Debug.Log("edgIdx2List2[firstNum][0]:" + edgIdx2List2[firstNum][0]);
                                    Debug.Log("edgIdx2List2[firstNum][1]:" + edgIdx2List2[firstNum][1]);
                                    Debug.Log("edgIdx2List2[firstNum][2]:" + edgIdx2List2[firstNum][2]);
                                    Debug.Log("edgIdx2List2[firstNum][3]:" + edgIdx2List2[firstNum][3]);

                                    // �ǂ�������_�̎n�_�����ׂ�
                                    if ((edgIdx2List2[firstNum][0] == idxList2[0] || edgIdx2List2[firstNum][0] == idxList2[1]) && (edgIdx2List2[firstNum][1] == idxList2[1] || edgIdx2List2[firstNum][1] == idxList2[0]))
                                    {
                                        Debug.Log("���_�̒ǉ�");
                                        secondNum = 0;
                                        pEnd = intersectPolyList2[firstNum][1];    // ��_�̏I�_
                                        pEdge2 = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][1].x), Mathf.Abs(intersectEdgList2[firstNum][1].y));// ��_�̏I�_�̕Ӄx�N�g��                                                      
                                        idxList2.Clear();    // ���̍폜
                                        idxList2.Add(edgIdx2List2[firstNum][2]); // ���̒ǉ�
                                        idxList2.Add(edgIdx2List2[firstNum][3]); // ���̒ǉ�
                                        rastIdxList.Clear();    // ���̍폜
                                        rastIdxList.Add(edgIdx2List2[firstNum][2]);// ���̒ǉ�
                                        rastIdxList.Add(edgIdx2List2[firstNum][3]);// ���̒ǉ�
                                    }
                                    else if ((edgIdx2List2[firstNum][2] == idxList2[0] || edgIdx2List2[firstNum][2] == idxList2[1]) && (edgIdx2List2[firstNum][3] == idxList2[1] || edgIdx2List2[firstNum][3] == idxList2[0]))
                                    {
                                        Debug.Log("���_�̒ǉ�");
                                        secondNum = 1;
                                        pEnd = intersectPolyList2[firstNum][0];    // ��_�̏I�_
                                        pEdge2 = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][0].x), Mathf.Abs(intersectEdgList2[firstNum][0].y));// ��_�̏I�_�̕Ӄx�N�g��                    
                                        idxList2.Clear();    // ���̍폜
                                        idxList2.Add(edgIdx2List2[firstNum][0]); // ���̒ǉ�
                                        idxList2.Add(edgIdx2List2[firstNum][1]); // ���̒ǉ�
                                        rastIdxList.Clear();    // ���̍폜
                                        rastIdxList.Add(edgIdx2List2[firstNum][0]);// ���̒ǉ�
                                        rastIdxList.Add(edgIdx2List2[firstNum][1]);// ���̒ǉ�
                                    }

                                    // ���_�̒ǉ�
                                    vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f * -cpNormalAbs.z));
                                    vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f * cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f * cpNormalAbs.z));

                                    // �C���f�b�N�X�̊��蓖��
                                    int idx0 = straddlePolyIdx[(firstNum * 3)];
                                    int idx1 = straddlePolyIdx[(firstNum * 3) + 1];
                                    int idx2 = straddlePolyIdx[(firstNum * 3) + 2];
                                    int idx3 = vertices1.Count - 4; // 
                                    int idx4 = vertices1.Count - 3; //  
                                    int idx5 = vertices1.Count - 2; // 
                                    int idx6 = vertices1.Count - 1; // 
                                    int removeIdx = -1;

                                    // �폜����O�p�`�̌���
                                    Debug.Log("�폜����O�p�`�̒T��");
                                    for (int a = 0; a < attachedMesh.triangles.Length; a += 3)
                                    {
                                        if (!(attachedMesh.triangles[a] == idx0 && attachedMesh.triangles[a + 1] == idx1 && attachedMesh.triangles[a + 2] == idx2)) continue;
                                        removeIdx = a;
                                    }

                                    // �C���f�b�N�X�̊���U��
                                    if (secondNum == 0)
                                    {
                                        // edgIdx2List2[firstNum][0]�AedgIdx2List2[firstNum][1]���n�_�̌�_

                                        // �C���f�b�N�X�̍폜
                                        triangles1.RemoveRange(removeIdx, 3);

                                        // 01-12�C���f�b�N�X
                                        if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx1)) &&
                                            ((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)))
                                        {
                                            Debug.Log("01-12�C���f�b�N�X");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx1);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx4);
                                        }
                                        // 01-02�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)))
                                        {
                                            Debug.Log("01-02�C���f�b�N�X");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx1);

                                        }
                                        // 02-12�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)))
                                        {
                                            Debug.Log("02-12�C���f�b�N�X");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx1);
                                        }
                                        // 02-01�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx1 || edgIdx2List2[firstNum][3] == idx0)))
                                        {
                                            Debug.Log("02-01�C���f�b�N�X");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx4);


                                        }
                                        // 12-01�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx1 || edgIdx2List2[firstNum][3] == idx0)))
                                        {
                                            Debug.Log("12-01�C���f�b�N�X");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx0);
                                        }
                                        // 12-20�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx2 || edgIdx2List2[firstNum][2] == idx0) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx2)))
                                        {
                                            Debug.Log(" 12-02�C���f�b�N�X");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx1);
                                        }

                                    }
                                    else if (secondNum == 1)
                                    {
                                        // �C���f�b�N�X�̍폜
                                        triangles1.RemoveRange(removeIdx, 3);

                                        // 01-12�C���f�b�N�X
                                        if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)))
                                        {
                                            Debug.Log("01-12�C���f�b�N�X");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx1);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx4);
                                        }
                                        // 01-02�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)))
                                        {
                                            Debug.Log("01-02�C���f�b�N�X");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx1);

                                        }
                                        // 02-12�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)))
                                        {
                                            Debug.Log("02-12�C���f�b�N�X");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx1);
                                        }
                                        // 02-01�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx1 || edgIdx2List2[firstNum][1] == idx0)))
                                        {
                                            Debug.Log("02-01�C���f�b�N�X");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx4);


                                        }
                                        // 12-01�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx1 || edgIdx2List2[firstNum][1] == idx0)))
                                        {
                                            Debug.Log("12-01�C���f�b�N�X");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx0);
                                        }
                                        // 12-20�C���f�b�N�X
                                        else if (((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx2 || edgIdx2List2[firstNum][0] == idx0) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx2)))
                                        {
                                            Debug.Log(" 12-02�C���f�b�N�X");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx1);
                                        }


                                    }

                                    // ��₩��폜
                                    idxCnt.RemoveRange(k, 3);
                                    intersectPolyList2.RemoveAt(firstNum);
                                    intersectEdgList2.RemoveAt(firstNum);
                                    edgIdx2List2.RemoveAt(firstNum);

                                    // �����܂ŗ�����I��
                                    end3 = true;
                                    break;
                                }
                                if (end3) break;
                            }

                            if (end3) break;
                        }

                        // �J�E���g++
                        count++;
                    }
                }
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

        // --- �ʁX�̃I�u�W�F�N�g�ɕ����鏈�� ---

        // �ϐ��錾
        int idx = 0;    // �T��������C���f�b�N�X
        var idxList = new List<int>();       // �T��������C���f�b�N�X�̃��X�g
        //int cnt = 0;
        bool addFlg = false;    // �ǉ��t���O
        bool end = false;   // �I���t���O
        var vertices2 = new List<Vector3>();   // ���_
        var triangles2 = new List<int>();       // �O�p�`�C���f�b�N�X
        var removeList = new List<int>();       // �����p�̃��X�g
        var normals2 = new List<Vector3>();     // �@��

        var vtxRemove = new List<int>();

        // �܂��i�[
        idxList.Add(0);
        triangles2.Add(triangles1[idxList[0]]);
        triangles2.Add(triangles1[idxList[0] + 1]);
        triangles2.Add(triangles1[idxList[0] + 2]);
        //for (int l = 0; l < 3; l++)
        //{
        //    bool through = false;   // �X���[�t���O

        //    // ���łɏ����Ă��钸�_�̃C���f�b�N�X��������X���[
        //    for (int g = 0; g < vtxRemove.Count; g++)
        //    {
        //        if (vtxRemove[g] != triangles1[0 + l]) continue;

        //        through = true; // �X���[�t���OON
        //        break;
        //    }
        //    // ���̒T����
        //    if (through) continue;

        //    vertices1.Remove(vertices1[triangles1[0 + l]]); // ���_�̍폜
        //    vtxRemove.Add(triangles1[0+ l]);   // �C���f�b�N�X�ԍ��̕ۑ�                                  
        //    Debug.Log("triangles1[i + l]" + triangles1[0 + l]);
           
        //    // �C���f�b�N�X�̕ύX
        //    for (int d = 0; d < triangles1.Count; d++)
        //    {
        //        if (d == triangles1[0 + l]) continue;
        //        if (triangles1[d] < triangles1[0 + l]) continue;

        //        triangles1[d] -= 1;

        //    }
          
        //}

        //Debug.Log("triangle2:" + triangles2[idx] + "" + triangles2[idx + 1] + "" + triangles2[idx + 2]);



        // �����ӂ����邩��T��
        while (!end)
        {
            // �����ӂ����݂��Ă��邩�ϐ�
            bool Existence = false;

            // �ǉ��t���OOFF
            addFlg = false;

            // �|���S���̐��������[�v
            for (int i = 0; i < triangles1.Count; i += 3)
            {
                Existence = false;
                // �C���f�b�N�X��i��������������X���[
                //if (idx == i) continue;

                // �O�p�`��3�ӂ���̂�3���[�v
                for (int k = 0; k < 3; k++)
                {
                    // 1�ӂɑ΂���3�Ӓ��ׂ�̂�3���[�v
                    for (int j = 0; j < 3; j++)
                    {
                        // �����ӂ����邩
                        if ((triangles1[idxList[0] + k] == triangles1[i + j] && triangles1[idxList[0] + (k + 1) % 3] == triangles1[i + (j + 1) % 3]) ||
                            (triangles1[idxList[0] + k] == triangles1[i + (j + 1) % 3] && triangles1[idxList[0] + (k + 1) % 3] == triangles1[i + j]))
                        {
                            //Debug.Log("�����ӂ�����Ƃ�");
                            //Debug.Log("�q�b�g�����|���S���ԍ�:" + triangles1[i] + "" + triangles1[i + 1] + "" + triangles1[i + 2]);
                            //Debug.Log("idx�̃|���S���ԍ�:" + triangles1[idx] + "" + triangles1[idx + 1] + "" + triangles1[idx + 2]);
                            ////Debug.Log("triangle1[idx + k]:" + triangles1[idx + k] + "" + triangles1[(idx + k + 1) % 3]);
                            //Debug.Log("triangles1[i + j]:" + triangles1[i + j] + "" + triangles1[i + (j + 1) % 3]);
                            //Debug.Log("triangles1[i + (j + 1) % 3]:" + triangles1[i + (j + 1) % 3] + "" + triangles1[i + j]);

                            //Debug.Log("triangles2.Count:" + triangles2.Count);


                            //// �����ӂ�����Ƃ�
                            // ���ꂪ���łɊi�[�ς݂��ǂ������ׂ�                         
                            for (int l = 0; l < triangles2.Count; l+=3)
                            {
                                // �ǉ��������C���f�b�N�X�ƒǉ���̃C���f�b�N�X�Ƃ̔�r
                                // �Ȃ�������X���[
                                if (!(triangles2[l] == triangles1[i] && triangles2[l + 1] == triangles1[i + 1] && triangles2[l + 2] == triangles1[i + 2])) continue;

                                // ���݂��Ă���
                                Existence = true;
                                break;// ���[�v�I��
                            }

                            // ���݂����玟�̒T����
                            if (Existence)
                            {
                                break;
                            }
                            else
                            {
                                //Debug.Log("���݂��Ȃ�������i�[");
                                // ���݂��Ȃ�������i�[
                                triangles2.Add(triangles1[i]);
                                triangles2.Add(triangles1[i + 1]);
                                triangles2.Add(triangles1[i + 2]);

                                // ���_�̍폜
                                //Debug.Log("triangles1[i]" + triangles1[i]);
                                //Debug.Log("triangles1[i+1]" + triangles1[i + 1]);
                                //Debug.Log("triangles1[i+2]" + triangles1[i + 2]);
                                //Debug.Log("vertices1.Count" + vertices1.Count);
                                //Debug.Log("vertices1.Remove(vertices1[triangles1[i + l]])" + vertices1.Remove(vertices1[triangles1[i]]));
                                
                                // �C���f�b�N�X�̕ύX
                                //for (int l = 0; l < 3; l++)
                                //{
                                //    bool through = false;   // �X���[�t���O

                                //    // ���łɏ����Ă��钸�_�̃C���f�b�N�X��������X���[
                                //    for (int g = 0;g < vtxRemove.Count;g++)
                                //    {
                                //        if (vtxRemove[g] != triangles1[i + l]) continue;

                                //        through = true; // �X���[�t���OON
                                //        break;
                                //    }
                                //    // ���̒T����
                                //    if (through) continue;

                                //    //Debug.Log("vertices1.Remove(vertices1[triangles1[i + l]]):" + vertices1.Remove(vertices1[triangles1[i + l]]));
                                //    vertices1.Remove(vertices1[triangles1[i + l]]); // ���_�̍폜
                                //    vtxRemove.Add(triangles1[i + l]);   // �C���f�b�N�X�ԍ��̕ۑ�                                  
                                //    Debug.Log("triangles1[i + l]" + triangles1[i + l]);
                                //    //Debug.Log("�ύX�O");
                                //    //Debug.Log("triangles1[i]" + triangles1[i]);
                                //    //Debug.Log("triangles1[i+1]" + triangles1[i + 1]);
                                //    //Debug.Log("triangles1[i+2]" + triangles1[i + 2]);
                                //    // �C���f�b�N�X�̕ύX
                                //    for (int d = 0; d < triangles1.Count; d++)
                                //    {
                                //        if (triangles1[d] <= triangles1[i + l]) continue;
                                      
                                //        triangles1[d] -= 1;
                                      
                                //    }
                                //    //Debug.Log("�ύX��");
                                //    //Debug.Log("triangles1[i]" + triangles1[i]);
                                //    //Debug.Log("triangles1[i+1]" + triangles1[i + 1]);
                                //    //Debug.Log("triangles1[i+2]" + triangles1[i + 2]);
                                //}


                                // ���̒T����
                                idxList.Add(i);
                                //idx = i ;    // ���̒T���̃|���S���̍ŏ��̃C���f�b�N�X
                                addFlg = true;
                                break;
                            }
                        }


                    }

                    // ���݂����玟�̒T����
                    if (Existence)
                    {
                        break;
                    }
                }

                // �ǉ������玟�̒T����
                if (addFlg)
                {
                    continue;
                }

                // ���݂����玟�̒T����
                if (Existence)
                {
                    continue;
                }

            }

            // ��������̃��X�g����͍폜
            removeList.Add(idxList[0]);

            // �T�����������C���f�b�N�X������
            idxList.RemoveAt(0);

            // �����T���������C���f�b�N�X���Ȃ��Ȃ�����I��
            if (idxList.Count == 0)
            {
                //Debug.Log("����������");
                // end�t���Oon
                end = true;
            }

        }

        //Debug.Log("removeList.Count" + removeList.Count);

        // triangles1�ɓ����Ă�triangles2������
        for (int i = 0; i < removeList.Count; i++)
        {
            triangles1.RemoveRange(removeList[i], 3);
            triangles1.Insert(removeList[i], 0);
            triangles1.Insert(removeList[i], 0);
            triangles1.Insert(removeList[i], 0);
        }

        // triangles1�����Ƃ�vertices1�𐶐����A��������Ƃ�triangles1���㏑��
        for (int i = 0; i < triangles1.Count; i++)
        {
            vertices1.Add(attachedMesh.vertices[triangles1[i]]);
            for (int j = 0; j < vertices1.Count; j++)
            {
                // �ǉ��������_���d�����ĂȂ������炻�̂܂܏I��
                if (j == vertices1.Count - 1)
                {
                    // �C���f�b�N�X�̏㏑��
                    triangles1[i] = vertices1.Count - 1;
                    break;
                }
                // �ǉ��������_���d�����Ă���ǉ��������_������
                if (vertices1[j] == attachedMesh.vertices[triangles1[i]])
                {
                    // ���_�폜
                    vertices1.RemoveAt(vertices1.Count - 1);

                    // �C���f�b�N�X�̏㏑��
                    triangles1[i] = j;
                    break;
                }
            }

        }

        // �d������C���f�b�N�X�ƒ��_���폜
        for(int b = 0;b < triangles1.Count;b += 3)
        {
            // �C���f�N�X��3�Ƃ��������X���[
            if (!(triangles1[b] == triangles1[b + 1] && triangles1[b] == triangles1[b + 2])) continue;
            //Debug.Log("triangles1[b]" + triangles1[b]);
            //Debug.Log("triangles1[b+1]" + triangles1[b+1]);
            //Debug.Log("triangles1[b+2]" + triangles1[b+2]);
            triangles1.RemoveRange(b, 3);
            //Debug.Log("vertices1[triangles1[b]]" + vertices1[triangles1[b]]);
            //vertices1.RemoveAt(triangles1[b]);
            b = 0;
        }

        normals1.Clear();
        // �m�[�}���̐ݒ�       
        for (int i = 0; i < vertices1.Count; i++)
        {
            normals1.Add(Vector3.up);
        }

        // triangles2�����Ƃ�vertices2�𐶐����A��������Ƃ�triangles2���㏑��
        for (int i = 0; i < triangles2.Count; i++)
        {
            vertices2.Add(attachedMesh.vertices[triangles2[i]]);
            for (int j = 0; j < vertices2.Count; j++)
            {
                // �ǉ��������_���d�����ĂȂ������炻�̂܂܏I��
                if (j == vertices2.Count - 1)
                {
                    // �C���f�b�N�X�̏㏑��
                    triangles2[i] = vertices2.Count - 1;
                    break;
                }
                // �ǉ��������_���d�����Ă���ǉ��������_������
                if (vertices2[j] == attachedMesh.vertices[triangles2[i]])
                {
                    // ���_�폜
                    vertices2.RemoveAt(vertices2.Count - 1);

                    // �C���f�b�N�X�̏㏑��
                    triangles2[i] = j;
                    break;
                }
            }

        }



        // �m�[�}���̐ݒ�       
        for (int i = 0; i < vertices2.Count; i++)
        {
            normals2.Add(Vector3.up);
        }

        //�J�b�g��̃I�u�W�F�N�g�����A���낢��Ƃ����
        GameObject obj = new GameObject("Plane", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody), typeof(MeshDivision2), typeof(Ground));
        var mesh = new Mesh();
        mesh.vertices = vertices1.ToArray();
        mesh.triangles = triangles1.ToArray();
        //mesh.uv = uvs1.ToArray();
        mesh.normals = normals1.ToArray();
        obj.GetComponent<MeshFilter>().mesh = mesh;
        obj.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        obj.GetComponent<MeshCollider>().sharedMesh = mesh;
        obj.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
        //obj.GetComponent<MeshCollider>().convex = false;
        obj.GetComponent<MeshCollider>().material = GetComponent<Collider>().material;
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.transform.localScale = transform.localScale;
        //obj.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        //obj.GetComponent<Rigidbody>().angularVelocity = GetComponent<Rigidbody>().angularVelocity;
        obj.GetComponent<Rigidbody>().isKinematic = true;   // �^���𖳌��� 

        GameObject obj2 = new GameObject("Plane", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody), typeof(MeshDivision2), typeof(Ground));
        var mesh2 = new Mesh();
        mesh2.vertices = vertices2.ToArray();
        mesh2.triangles = triangles2.ToArray();
        Debug.Log("vertices2.Count" + vertices2.Count);
        mesh2.normals = normals2.ToArray();
        obj2.GetComponent<MeshFilter>().mesh = mesh2;
        obj2.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        obj2.GetComponent<MeshCollider>().sharedMesh = mesh2;
        obj2.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
        //obj2.GetComponent<MeshCollider>().convex = false;
        obj2.GetComponent<MeshCollider>().material = GetComponent<Collider>().material;
        obj2.transform.position = transform.position;
        obj2.transform.rotation = transform.rotation;
        obj2.transform.localScale = transform.localScale;
        //obj2.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        //obj2.GetComponent<Rigidbody>().angularVelocity = GetComponent<Rigidbody>().angularVelocity;
        obj2.GetComponent<Rigidbody>().isKinematic = true;   // �^���𖳌��� 

        Debug.Log("triangles2:" + triangles2.Count);
        Debug.Log("vertices2:" + vertices2.Count);
        Debug.Log("�炽��");

        // �ʐς̌v�Z
        float s1 = 0;   // �ʐ�1
        float s2 = 0;   // �ʐ�1

        // �ʐ�1�̌v�Z
        for (int i = 0; i < triangles1.Count; i += 3)
        {
            s1 += CalculateArea1(new Vector2(vertices1[triangles1[i]].x, vertices1[triangles1[i]].z), new Vector2(vertices1[triangles1[i + 1]].x, vertices1[triangles1[i + 1]].z), new Vector2(vertices1[triangles1[i + 2]].x, vertices1[triangles1[i + 2]].z));
        }

        // �ʐ�2�̌v�Z
        for (int i = 0; i < triangles2.Count; i += 3)
        {
            s2 += CalculateArea1(new Vector2(vertices2[triangles2[i]].x, vertices2[triangles2[i]].z), new Vector2(vertices2[triangles2[i + 1]].x, vertices2[triangles2[i + 1]].z), new Vector2(vertices2[triangles2[i + 2]].x, vertices2[triangles2[i + 2]].z));
        }

        // �̐ς̔�r(�����ő̐ς��y�����𗎂Ƃ��Ă��܂�)
        if (s1 < s2)
        {
            obj2.GetComponent<Rigidbody>().useGravity = false;   // �d�̖͂�����
            obj2.GetComponent<Rigidbody>().isKinematic = true;   // �^���𖳌��� 
            obj.GetComponent<Renderer>().material.color = Color.gray;
            obj.GetComponent<Ground>().StartFadeOut();
            obj.GetComponent<Rigidbody>().mass = 0.5f;
            obj.GetComponent<Rigidbody>().drag = 7.0f;

        }
        else
        {
            obj.GetComponent<Rigidbody>().useGravity = false;   // �d�̖͂�����
            obj.GetComponent<Rigidbody>().isKinematic = true;   // �^���𖳌��� 
            obj2.GetComponent<Renderer>().material.color = Color.gray;
            obj2.GetComponent<Ground>().StartFadeOut();
            obj2.GetComponent<Rigidbody>().mass = 0.5f;
            obj2.GetComponent<Rigidbody>().drag = 7.0f;
        }

        //���̃I�u�W�F�N�g���f�X�g���C
        Destroy(gameObject);

        // ���b�V���ɑ��
        attachedMesh.SetVertices(vertices1.ToArray());
        attachedMesh.SetTriangles(triangles1.ToArray(), 0);
        attachedMesh.SetNormals(normal);

    }

    // �ʐς̌v�Z
    private float CalculateArea1(Vector2 A, Vector2 B, Vector2 C)
    {
        float S, s;
        float a = Vector2.Distance(B, C);
        float b = Vector2.Distance(A, C);
        float c = Vector2.Distance(A, B);

        s = (a + b + c) / 2;
        S = Mathf.Sqrt(s * (s - a) * (s - b) * (s - c));

        return S;
    }

    // �̐ς̌v�Z
    public float VolumeOfMesh(Mesh mesh)
    {
        if (mesh == null) return 0;

        Vector3[] vertics = mesh.vertices;
        int[] triangles = mesh.triangles;

        float volume = 0;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertics[triangles[i + 0]];
            Vector3 p2 = vertics[triangles[i + 1]];
            Vector3 p3 = vertics[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);

        }


        return Mathf.Abs(volume);
    }

    // �̐ς̌v�Z�Ŏg���֐�
    public float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }
    // �M�Y���̕\��
    private void OnDrawGizmos()
    {
        if (!attachedMesh) return;


        for (int i = 0; i < attachedMesh.vertices.Length; i++)
        {
            Gizmos.color = new Color(25, 0, 0, 1);   // �F�̎w��
            Gizmos.DrawSphere(attachedMesh.vertices[i] + transform.position, 0.005f);
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

    float sign(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
    }

    bool PointInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
    {
        bool b1, b2, b3;

        b1 = sign(pt, v1, v2) < 0.0f;
        b2 = sign(pt, v2, v3) < 0.0f;
        b3 = sign(pt, v3, v1) < 0.0f;

        return ((b1 == b2) && (b2 == b3));
    }
}
