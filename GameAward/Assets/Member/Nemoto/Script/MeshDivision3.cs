//======================================================
//
//        MeshDivision3.cs
//        メッシュを分割する処理
//
//------------------------------------------------------
//      作成者:根本龍之介
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDivision3 : MonoBehaviour
{
    // メッシュの変数
    private MeshFilter attachedMeshFilter;
    private Mesh attachedMesh;
    private List<int> idxMemory = new List<int>();    // 三角形インデックスの記憶用変数

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
        // メッシュのアタッチ
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;
        idxMemory.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // メッシュの分割(最初)
    void MeshDivisionBign(List<Vector3> cutPoint)
    {
        Debug.Log("切り始め");

        // メッシュのアタッチ
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;

       // 変数
       var uvs1 = new List<Vector2>();       // テクスチャ
       var vertices1 = new List<Vector3>();  // 頂点
       var triangles1 = new List<int>();     // 三角形インデックス
       var normals1 = new List<Vector3>();   // 法線
       

        // メッシュの情報を代入
        for (int i = 0; i < attachedMesh.vertices.Length; i++)
        {
            vertices1.Add(attachedMesh.vertices[i]);
        }
        for (int i = 0; i < attachedMesh.triangles.Length; i++)
        {
            triangles1.Add(attachedMesh.triangles[i]);
        }

    }

    // メッシュの分割(途中)
    public void MeshDivisionMidlle(List<Vector3> cutPoint)
    {
        // メッシュのアタッチ
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;

    }

    // メッシュの分割(最後)
    public void MeshDivisionEnd(List<Vector3> cutPoint)
    {
        // メッシュのアタッチ
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;

    }
}
