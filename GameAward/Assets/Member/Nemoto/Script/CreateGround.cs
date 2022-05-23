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

    // 変数宣言
    Mesh mesh;
    MeshFilter filter;
    Triangle current;
    public List<Vector3> vtx = new List<Vector3>(); // メッシュの頂点
    public List<int> idx = new List<int>();         // メッシュのインデックス
    List<Vector3> normal = new List<Vector3>();     // メッシュの法線
    public float MeshSizeX = 10;    // メッシュのサイズ(X)
    public float MeshSizeY = 10;    // メッシュのサイズ(Y)
    public float MeshSizeZ = 10;    // メッシュのサイズ(Z)
    public Color gizmMeshColor = new Color(25.0f,0.0f,0.0f,0.1f);  // ギズモのメッシュのカラー

    // Start is called before the first frame update
    void Start()
    {
        //---- メッシュ(紙)の動的生成 ----
        filter = GetComponent<MeshFilter>();
        mesh = new Mesh();

        // 頂点
        vtx.Add(new Vector3(-MeshSizeX/2, 0, -MeshSizeZ/2));    // 左奥
        vtx.Add(new Vector3(-MeshSizeX/2, 0,  MeshSizeZ/2));    // 右奥
        vtx.Add(new Vector3(MeshSizeX/2,  0,  MeshSizeZ/2));    // 左手前
        vtx.Add(new Vector3(MeshSizeX/2,  0, -MeshSizeZ/2));    // 右手前

        mesh.SetVertices(vtx);  // メッシュにセット

        // インデックス
        idx.Add(0);
        idx.Add(1);
        idx.Add(2);

        idx.Add(2);
        idx.Add(3);
        idx.Add(0);

        mesh.SetTriangles(idx, 0);   // メッシュにセット

        // ノーマル
        normal.Add(Vector3.up);
        normal.Add(Vector3.up);
        normal.Add(Vector3.up);
        normal.Add(Vector3.up);

        mesh.SetNormals(normal);    // メッシュにセット

        filter.mesh = mesh;

        // オブジェクトにセット
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
        // ギズモの表示
        Gizmos.color = gizmMeshColor;   // 色の指定
        Gizmos.DrawCube(transform.position, new Vector3(MeshSizeX, 0.01f, MeshSizeZ));  // 描画
    }
}
