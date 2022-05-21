using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDivision3 : MonoBehaviour
{
    // ���b�V���̕ϐ�
    private MeshFilter attachedMeshFilter;
    private Mesh attachedMesh;
    private List<int> idxMemory = new List<int>();    // �O�p�`�C���f�b�N�X�̋L���p�ϐ�


    // Start is called before the first frame update
    void Start()
    {
        // ���b�V���̃A�^�b�`
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;
        idxMemory.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ���b�V���̕���(�ŏ�)
    void MeshDivisionBign(List<Vector3> cutPoint)
    {
        Debug.Log("�؂�n��");
        // ���b�V���̃A�^�b�`
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;

        // �ϐ�
       var uvs1 = new List<Vector2>();       // �e�N�X�`��
       var vertices1 = new List<Vector3>();  // ���_
       var triangles1 = new List<int>();     // �O�p�`�C���f�b�N�X
       var normals1 = new List<Vector3>();   // �@��
       

        // ���b�V���̏�����
        for (int i = 0; i < attachedMesh.vertices.Length; i++)
        {
            vertices1.Add(attachedMesh.vertices[i]);
        }
        for (int i = 0; i < attachedMesh.triangles.Length; i++)
        {
            triangles1.Add(attachedMesh.triangles[i]);
        }

    }
}
