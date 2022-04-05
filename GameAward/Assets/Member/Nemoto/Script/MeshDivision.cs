//======================================================
//
//        MeshDivision.cs
//        ���b�V���𕪊����鏈��
//
//------------------------------------------------------
//      �쐬��:���{���V��
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDivision : MonoBehaviour
{
    // ���b�V���̕ϐ�
    private MeshFilter attachedMeshFilter;
    private Mesh attachedMesh;
    private bool coliBool = false;
    private double delta = 0.000000001f;
    private float skinWidth = 0.05f;

    public List<Vector3> meshVertices;

    public static List<Vector3> CutPoint1;  // �J�b�g�|�C���g�i�[�p

    public static List<int> CrossNum;

    // Start is called before the first frame update
    void Start()
    {
        // ���b�V���̃A�^�b�`
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;
        CrossNum = new List<int>();
        //CrossNum.Clear();
        CutPoint1 = new List<Vector3>();
        CutPoint1.Clear();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ���b�V���𕪊����鏈��
    public void DivisionMesh(List<Vector3> cutPoint,int cpNum)
    {
        // ���b�V���̃A�^�b�`
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;

        // �ϐ�
        Vector3 p0, p1, p2;    // ���b�V���̃|���S���̒��_
        var uvs1 = new List<Vector2>(); // �e�N�X�`��
        var uvs2 = new List<Vector2>();
        var vertices1 = new List<Vector3>();   // ���_
        var vertices2 = new List<DVector3>();
        var triangles1 = new List<int>();       // �O�p�`�C���f�b�N�X
        var triangles2 = new List<int>();
        var normals1 = new List<Vector3>();     // �@��
        var normals2 = new List<Vector3>();

        var crossVertices = new List<Vector3>();

        // �J�b�g�|�C���g�̊i�[
        CutPoint1.Add(cutPoint[cpNum]);
        //Debug.Log("�J�b�g�|�C���g�̌����ɂ�"+ CutPoint1.Count);

        CrossNum.Add(1);
        Debug.Log("�J�b�g�|�C���g�̌����ɂ�"+CrossNum.Count);


        //�J�b�g�������I�u�W�F�N�g�̃��b�V�����g���C�A���O�����Ƃɏ���
        for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
        {
            //���b�V����3�̒��_���擾
            p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);
            p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);
            p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]);

            // �J�b�g�|�C���g�����̃|���S���̒��ɂ��邩
            double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
            double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[cpNum].x + (p0.x - p2.x) * cutPoint[cpNum].z);
            double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[cpNum].x + (p1.x - p0.x) * cutPoint[cpNum].z);
            if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
            {
                // �|���S���̒��ɃJ�b�g�|�C���g�̎n�_���������Ƃ�

                

                // ��O�̃J�b�g�|�C���g�����̃|���S���̒��ɂ��邩
                if (cpNum > 0)  // �J�b�g�|�C���g��0�Ԗڂ���Ȃ�������
                {
                    double _s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[cpNum - 1].x + (p0.x - p2.x) * cutPoint[cpNum - 1].z);
                    double _t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[cpNum - 1].x + (p1.x - p0.x) * cutPoint[cpNum - 1].z);

                    // ��O�̃J�b�g�|�C���g�����̃|���S���̒��ɓ����Ă�����
                    if ((0 <= _s && _s <= 1) && (0 <= _t && _t <= 1) && (0 <= 1 - _s - _t && 1 - _s - _t <= 1))
                    {  
                        // ���_�̒ǉ�
                        vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i]]);
                        vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);
                        vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + 2]]);
                        vertices1.Add(cutPoint[cpNum] - transform.position);
                        CrossNum.Add(vertices1.Count - 1);

                        // ���_�̃C���f�b�N�X
                        int _0 = vertices1.Count - 4;
                        int _1 = vertices1.Count - 3;
                        int _2 = vertices1.Count - 2;
                        int _3 = vertices1.Count - 1;

                        // �O�p�`�C���f�N�X�̒ǉ�
                        //triangles1
                        if (t < 0.001f)
                        {
                            triangles1.Add(_0);
                            triangles1.Add(_3);
                            triangles1.Add(_2);

                            triangles1.Add(_3);
                            triangles1.Add(_1);
                            triangles1.Add(_2);
                            //Debug.Log("t");
                        }
                        else if (s < 0.001f)
                        {

                            triangles1.Add(_1);
                            triangles1.Add(_3);
                            triangles1.Add(_2);

                            triangles1.Add(_0);
                            triangles1.Add(_3);
                            triangles1.Add(_2);
                            //Debug.Log("s ");
                        }
                        else if (s + t > 0.98f)
                        {
                            triangles1.Add(_0);
                            triangles1.Add(_1);
                            triangles1.Add(_3);

                            triangles1.Add(_0);
                            triangles1.Add(_3);
                            triangles1.Add(_2);
                            //Debug.Log("s + t:");
                        }
                        else
                        {
                            triangles1.Add(_0);
                            triangles1.Add(_1);
                            triangles1.Add(_3);

                            triangles1.Add(_0);
                            triangles1.Add(_3);
                            triangles1.Add(_2);

                            triangles1.Add(_3);
                            triangles1.Add(_1);
                            triangles1.Add(_2);
                        }
                    }                   
                    else
                    {// ��O�̃J�b�g�|�C���g�����̃|���S���̒��ɂȂ�������
                        // �Ƃ肠�������̃J�b�g�|�C���g�Ń|���S���̕���
                        
                        // ���_�̒ǉ�
                        vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i]]);
                        vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);
                        vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + 2]]);
                        vertices1.Add(cutPoint[cpNum] - transform.position);
                        CrossNum.Add(vertices1.Count - 1);

                        // ���_�̃C���f�b�N�X
                        int _0 = vertices1.Count - 4;
                        int _1 = vertices1.Count - 3;
                        int _2 = vertices1.Count - 2;
                        int _3 = vertices1.Count - 1;

                        // �O�p�`�C���f�N�X�̒ǉ�
                        //triangles1
                        if (t < 0.001f)
                        {
                            triangles1.Add(_0);
                            triangles1.Add(_3);
                            triangles1.Add(_2);

                            triangles1.Add(_3);
                            triangles1.Add(_1);
                            triangles1.Add(_2);
                            //Debug.Log("t");
                        }
                        else if (s < 0.001f)
                        {

                            triangles1.Add(_1);
                            triangles1.Add(_3);
                            triangles1.Add(_2);

                            triangles1.Add(_0);
                            triangles1.Add(_3);
                            triangles1.Add(_2);
                            //Debug.Log("s ");
                        }
                        else if (s + t > 0.98f)
                        {
                            triangles1.Add(_0);
                            triangles1.Add(_1);
                            triangles1.Add(_3);

                            triangles1.Add(_0);
                            triangles1.Add(_3);
                            triangles1.Add(_2);
                            //Debug.Log("s + t:");
                        }
                        else
                        {
                            triangles1.Add(_0);
                            triangles1.Add(_1);
                            triangles1.Add(_3);

                            triangles1.Add(_0);
                            triangles1.Add(_3);
                            triangles1.Add(_2);

                            triangles1.Add(_3);
                            triangles1.Add(_1);
                            triangles1.Add(_2);
                        }
                        
                        
                        // ���̃J�b�g�|�C���g�ƈ�O�̃J�b�g�|�C���g�Ƃ̐����ƁA�|���S���̕�(����)�Ƃ̌�_����鏈��
                        for (int j = 0;j < attachedMesh.triangles.Length;j+= 3)
                        {
                            // �|���S���̒��_(�O�p�`�Ȃ̂�3)�̐������܂킷
                            for(int k = 0;k < 3;k++)
                            {
                                // �|���S���p�ϐ�
                                int idx_s = attachedMesh.triangles[j + k];  // �n�_
                                int idx_v = attachedMesh.triangles[j + ((k + 1)%3)];  // �I�_

                                // ���̃|���S���̕ӂ̎n�_�ƏI�_�̃x�N�g��
                                Vector2 vtx_s = new Vector2(attachedMesh.vertices[idx_s].x + transform.position.x, attachedMesh.vertices[idx_s].z + transform.position.z);    // �n�_�̃x�N�g��
                                Vector2 vtx_v = new Vector2(attachedMesh.vertices[idx_v].x + transform.position.x, attachedMesh.vertices[idx_v].z + transform.position.z);    // �I�_�̃x�N�g��

                                // ���̃J�b�g�|�C���g�ƈ�O�̃J�b�g�|�C���g�Ƃ̐����ƁA�|���S���̕�(����)�̎n�_���Ȃ����x�N�g��
                                Vector2 v = new Vector2(vtx_s.x - cutPoint[cpNum - 1].x, vtx_s.y - cutPoint[cpNum - 1].z);

                                // ����
                                Vector2 v1 = new Vector2(cutPoint[cpNum].x - cutPoint[cpNum - 1].x, cutPoint[cpNum].z - cutPoint[cpNum - 1].z); // �J�b�g�|�C���g�̐���
                                Vector2 v2 = new Vector2(vtx_v.x - vtx_s.x, vtx_v.y - vtx_s.y); // �|���S���̐���

                                // �����̎n�_�����_�̃x�N�g��
                                float t1 = (v.x * v2.y - v2.x * v.y) / (v1.x * v2.y - v2.x * v1.y);
                                float t2 = (v.x * v1.y - v1.x * v.y) / (v1.x * v2.y - v2.x * v1.y);

                                // ��_
                                Vector2 p = new Vector2(vtx_s.x, vtx_v.y) + new Vector2(v2.x * t2, v2.y * t2);

                                // �����Ɛ�����������Ă��邩
                                const float eps = 0.00001f;
                                if (t1 + eps < 0 || t1 - eps > 1 || t2 + eps < 0 || t2 - eps > 1)
                                {
                                    // Debug.Log("�������ĂȂ�");
                                }
                                else
                                {
                                    //Debug.Log("��_��ǉ�");
                                    // ���_�Ɍ�_��ǉ�
                                    vertices1.Add(new Vector3(p.x + transform.position.x, attachedMesh.vertices[attachedMesh.triangles[i]].y, p.y + transform.position.z));


                                }
                            }
                        }
                        
                    }
                }               
                else if(cpNum == 0)    // �J�b�g�|�C���g��0�Ԗڂ�������
                {    // �J�b�g�|�C���g��0�Ԗڂ�������

                   
                    // ���_�̒ǉ�
                    vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i]]);
                    vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);
                    vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + 2]]);
                    vertices1.Add(cutPoint[cpNum] - transform.position);
                    CrossNum.Add(vertices1.Count - 1);

                    // ���_�̃C���f�b�N�X
                    int _0 = vertices1.Count - 4;
                    int _1 = vertices1.Count - 3;
                    int _2 = vertices1.Count - 2;
                    int _3 = vertices1.Count - 1;

                    // �O�p�`�C���f�N�X�̒ǉ�
                    //triangles1
                    if (t < 0.001f)
                    {
                        triangles1.Add(_0);
                        triangles1.Add(_3);
                        triangles1.Add(_2);

                        triangles1.Add(_3);
                        triangles1.Add(_1);
                        triangles1.Add(_2);
                        Debug.Log("t");
                    }
                    else if (s < 0.001f)
                    {

                        triangles1.Add(_1);
                        triangles1.Add(_3);
                        triangles1.Add(_2);

                        triangles1.Add(_0);
                        triangles1.Add(_3);
                        triangles1.Add(_2);
                        Debug.Log("s ");
                    }
                    else if (s + t > 0.98f)
                    {
                        triangles1.Add(_0);
                        triangles1.Add(_1);
                        triangles1.Add(_3);

                        triangles1.Add(_0);
                        triangles1.Add(_3);
                        triangles1.Add(_2);
                        Debug.Log("s + t:");
                    }
                    else
                    {
                        triangles1.Add(_0);
                        triangles1.Add(_1);
                        triangles1.Add(_3);

                        triangles1.Add(_0);
                        triangles1.Add(_3);
                        triangles1.Add(_2);

                        triangles1.Add(_3);
                        triangles1.Add(_1);
                        triangles1.Add(_2);
                    }
                }
                
            }
            else
            {
                // �|���S���̒��ɃJ�b�g�|�C���g�̎n�_���Ȃ�������              
                for (int k = 0; k < 3; k++)
                {
                    vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + k]]);
                    //uvs1.Add(attachedMesh.uv[attachedMesh.triangles[i + k]]);
                    //normals1.Add(attachedMesh.normals[attachedMesh.triangles[i + k]]);
                    triangles1.Add(vertices1.Count - 1);
                }
            }


        }


        // ������̃I�u�W�F�N�g�����A���낢��Ƃ����
        GameObject obj = new GameObject("DivisionPlane"+ cpNum, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody), typeof(MeshDivision));
        var mesh = new Mesh();
        mesh.vertices = vertices1.ToArray();
        mesh.triangles = triangles1.ToArray();
        //mesh.uv = uvs1.ToArray();
        //mesh.normals = normals1.ToArray();
        obj.GetComponent<MeshFilter>().mesh = mesh;
        obj.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        obj.GetComponent<MeshCollider>().sharedMesh = mesh;
        obj.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
        obj.GetComponent<MeshCollider>().convex = false;
        obj.GetComponent<MeshCollider>().material = GetComponent<Collider>().material;
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.transform.localScale = transform.localScale;
        //obj.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        //obj.GetComponent<Rigidbody>().angularVelocity = GetComponent<Rigidbody>().angularVelocity;
        //obj.GetComponent<MeshCut>().skinWidth = skinWidth;

       
        obj.GetComponent<Rigidbody>().useGravity = false;   // �d�̖͂�����
        obj.GetComponent<Rigidbody>().isKinematic = true;   // �^���𖳌��� 

        

        //���̃I�u�W�F�N�g���f�X�g���C
        Destroy(gameObject);

    }

    // ���b�V����؂鏈��(��)
    public void CutMesh()
    {
        gameObject.name = "Plane";  // ���O�̕ύX

        // �J�b�g�|�C���g�̍폜
        //CutPoint.Clear();
    }

    // �M�Y���̕\��
    private void OnDrawGizmos()
    {
        if (!attachedMesh) return;
        for (int i = 0; i < attachedMesh.vertices.Length; i++)
        {
            if(CrossNum.Count > 0)
            {
                for (int j = 0; j < CrossNum.Count; j++)
                {
                    if (CrossNum[j] == i)
                    {
                        Gizmos.color = new Color(0, 0, 225, 1);   // �F�̎w��
                        
                    }
                    else
                    {
                        //Gizmos.color = new Color(25, 0, 0, 1);   // �F�̎w��
                    }

                }
            }
            else
            {
                Gizmos.color = new Color(25, 0, 0, 1);   // �F�̎w��

            }
            
            Gizmos.DrawSphere(attachedMesh.vertices[i] + transform.position, 0.05f);  // ���̕\��

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
