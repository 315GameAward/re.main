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
            p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);
            p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);
            p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]);

            // ���_���X�g�ɒǉ�
            vertices1.Add(p0);
            vertices1.Add(p1);
            vertices1.Add(p2);

            // �J�b�g�|�C���g�̎n�_���|���S���̕ӂ̏�ɂ��邩
            double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
            double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[0].x + (p0.x - p2.x) * cutPoint[0].z);
            double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[0].x + (p1.x - p0.x) * cutPoint[0].z);

            // �܂��͎O�p�`�̒��ɂ��邩
            if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
            {       // �O�p�`�̒��ɂ���
                    // �ӂ̏�ɂ��邩
                if (t < 0.001f) // ��S��
                {
                    edge = p2 - p0; // ��p0p2

                    vertices1.Add(cutPoint[1]); // ���_�̒ǉ�
                    vertices1.Add(cutPoint[1] - edge * 0.001f); // �S�Ԗڂ̒��_�̒ǉ�
                    vertices1.Add(cutPoint[1] + edge * 0.001f); // �S�Ԗڂ̒��_�̒ǉ�
                    
                }
                else if (s < 0.001f)    // ��T��
                {
                    edge = p1 - p0; // ��p0p1

                    vertices1.Add(cutPoint[1]);// ���_�̒ǉ�
                    vertices1.Add(cutPoint[1] - edge * 0.001f); // �S�Ԗڂ̒��_�̒ǉ�
                    vertices1.Add(cutPoint[1] + edge * 0.001f); // �S�Ԗڂ̒��_�̒ǉ�

                }
                else if (s + t > 0.98f) // ��S+T��
                {
                    edge = p2 - p1; // ��p1p2

                    vertices1.Add(cutPoint[1]);// ���_�̒ǉ�
                    vertices1.Add(cutPoint[1] - edge * 0.001f); // �S�Ԗڂ̒��_�̒ǉ�
                    vertices1.Add(cutPoint[1] + edge * 0.001f); // �S�Ԗڂ̒��_�̒ǉ�

                }
            }
            else    // �O�p�`�̒��ɂȂ�
            {
                Debug.Log("�|���S���̒��ɂ���܂���");
            }

            // �C���f�b�N�X�̐U�蕪��


        }

    }
}
