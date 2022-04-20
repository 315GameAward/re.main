//================================================
//
//     CreateGround.cs
//      地面(紙)を作る処理
//
//------------------------------------------------
//      作成者: 根本龍之介
//================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGround : MonoBehaviour
{
    // 三角形を作るための変数
    class Triangle
    {
        public int[] idx = new int[3];  // 三角形のインデックス保存用変数
        public Triangle[] edgeLink = new Triangle[3];
    };

    Mesh mesh;
    MeshFilter filter;
    MeshRenderer renderer;
    Triangle current;
    public List<Vector3> vtx = new List<Vector3>();
    public List<int> idx = new List<int>();
    List<Vector3> normal = new List<Vector3>();
    
    // Start is called before the first frame update
    void Start()
    {

        filter = GetComponent<MeshFilter>();
        mesh = new Mesh();

        vtx.Add(new Vector3(-3, 0, -3));
        vtx.Add(new Vector3(-3, 0, 3));
        vtx.Add(new Vector3(3, 0, 3));
        //vtx.Add(new Vector3(3, 0, -3));

        mesh.SetVertices(vtx);

        idx.Add(0);
        idx.Add(1);
        idx.Add(2);

        //idx.Add(2);
        //idx.Add(3);
        //idx.Add(0);

        mesh.SetTriangles(idx, 0);

        normal.Add(Vector3.up);
        normal.Add(Vector3.up);
        normal.Add(Vector3.up);
       // normal.Add(Vector3.up);

        mesh.SetNormals(normal);

        filter.mesh = mesh;

        gameObject.AddComponent<MeshCollider>();
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        //gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
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
        Gizmos.color = new Color(25, 0, 0, 0.1f);   // 色の指定
        //Gizmos.DrawMesh(filter.mesh);
        Gizmos.DrawCube(transform.position, new Vector3(6, 0.1f, 6));
    }
}
