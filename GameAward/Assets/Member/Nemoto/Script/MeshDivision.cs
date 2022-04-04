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

    // Start is called before the first frame update
    void Start()
    {
        // ���b�V���̃A�^�b�`
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;

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
        var crossVertices = new List<DVector3>();

        //�J�b�g�������I�u�W�F�N�g�̃��b�V�����g���C�A���O�����Ƃɏ���
        for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
        {
            //���b�V����3�̒��_���擾
            p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);
            p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);
            p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]);

            // �J�b�g�|�C���g�̎n�_�����̃|���S���̒��ɂ��邩
            double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
            double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[cpNum].x + (p0.x - p2.x) * cutPoint[cpNum].z);
            double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[cpNum].x + (p1.x - p0.x) * cutPoint[cpNum].z);
            if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
            {
                // �|���S���̒��ɃJ�b�g�|�C���g�̎n�_���������Ƃ�

                // ���_�̒ǉ�
                vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i]]);
                vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);
                vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + 2]]);
                vertices1.Add(cutPoint[cpNum] - transform.position);

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
    }

    // �M�Y���̕\��
    private void OnDrawGizmos()
    {
        if (!attachedMesh) return;
        for (int i = 0; i < attachedMesh.vertices.Length; i++)
        {
            Gizmos.color = new Color(25, 0, 0, 1);   // �F�̎w��
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
