//================================================
//
//     CreateGround.cs
//      �n��(��)����鏈��
//
//------------------------------------------------
//      �쐬��: ���{���V��
//================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGround : MonoBehaviour
{
    // �O�p�`����邽�߂̕ϐ�
    class Triangle
    {
        public int[] idx = new int[3];  // �O�p�`�̃C���f�b�N�X�ۑ��p�ϐ�
        public Triangle[] edgeLink = new Triangle[3];
    };

    // �ϐ��錾
    Mesh mesh;
    MeshFilter filter;
    Triangle current;
    public List<Vector3> vtx = new List<Vector3>(); // ���b�V���̒��_
    public List<int> idx = new List<int>();         // ���b�V���̃C���f�b�N�X
    List<Vector3> normal = new List<Vector3>();     // ���b�V���̖@��
    public float MeshSizeX = 10;    // ���b�V���̃T�C�Y(X)
    public float MeshSizeY = 10;    // ���b�V���̃T�C�Y(Y)
    public float MeshSizeZ = 10;    // ���b�V���̃T�C�Y(Z)
    public Color gizmMeshColor = new Color(25.0f,0.0f,0.0f,0.1f);  // �M�Y���̃��b�V���̃J���[

    // Start is called before the first frame update
    void Start()
    {
        //---- ���b�V��(��)�̓��I���� ----
        filter = GetComponent<MeshFilter>();
        mesh = new Mesh();

        // ���_
        vtx.Add(new Vector3(-MeshSizeX/2, 0, -MeshSizeZ/2));    // ����
        vtx.Add(new Vector3(-MeshSizeX/2, 0,  MeshSizeZ/2));    // �E��
        vtx.Add(new Vector3(MeshSizeX/2,  0,  MeshSizeZ/2));    // ����O
        vtx.Add(new Vector3(MeshSizeX/2,  0, -MeshSizeZ/2));    // �E��O

        mesh.SetVertices(vtx);  // ���b�V���ɃZ�b�g

        // �C���f�b�N�X
        idx.Add(0);
        idx.Add(1);
        idx.Add(2);

        idx.Add(2);
        idx.Add(3);
        idx.Add(0);

        mesh.SetTriangles(idx, 0);   // ���b�V���ɃZ�b�g

        // �m�[�}��
        normal.Add(Vector3.up);
        normal.Add(Vector3.up);
        normal.Add(Vector3.up);
        normal.Add(Vector3.up);

        mesh.SetNormals(normal);    // ���b�V���ɃZ�b�g

        filter.mesh = mesh;

        // �I�u�W�F�N�g�ɃZ�b�g
        gameObject.AddComponent<MeshCollider>();
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        gameObject.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
        gameObject.GetComponent<MeshCollider>().convex = false;
        gameObject.GetComponent<MeshCollider>().material = GetComponent<Collider>().material;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        // �M�Y���̕\��
        Gizmos.color = gizmMeshColor;   // �F�̎w��
        Gizmos.DrawCube(transform.position, new Vector3(MeshSizeX, 0.01f, MeshSizeZ));  // �`��
    }
}
