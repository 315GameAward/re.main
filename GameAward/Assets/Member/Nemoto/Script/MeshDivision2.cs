//======================================================
//
//        MeshDivision2.cs
//        メッシュを分割する処理
//
//------------------------------------------------------
//      作成者:根本龍之介
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDivision2 : MonoBehaviour
{
    // メッシュの変数
    private MeshFilter attachedMeshFilter;
    private Mesh attachedMesh;

    // Start is called before the first frame update
    void Start()
    {
        // メッシュのアタッチ
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // メッシュの分割
    public void DivisionMesh(List<Vector3> cutPoint)
    {
        // メッシュのアタッチ
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;

        // 変数
        Vector3 p0, p1, p2;    // メッシュのポリゴンの頂点
        var uvs1 = new List<Vector2>(); // テクスチャ
        //var uvs2 = new List<Vector2>();
        var vertices1 = new List<Vector3>();   // 頂点
        //var vertices2 = new List<DVector3>();
        var triangles1 = new List<int>();       // 三角形インデックス
        //var triangles2 = new List<int>();
        var normals1 = new List<Vector3>();     // 法線
        //var normals2 = new List<Vector3>();
        Vector3 edge = new Vector3();
        var crossVertices = new List<Vector3>();
        //カットしたいオブジェクトのメッシュをトライアングルごとに処理
        for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
        {
            //メッシュの3つの頂点を取得
            p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);
            p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);
            p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]);

            // 頂点リストに追加
            vertices1.Add(p0);
            vertices1.Add(p1);
            vertices1.Add(p2);

            // カットポイントの始点がポリゴンの辺の上にあるか
            double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
            double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[0].x + (p0.x - p2.x) * cutPoint[0].z);
            double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[0].x + (p1.x - p0.x) * cutPoint[0].z);

            // まずは三角形の中にあるか
            if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
            {       // 三角形の中にある
                    // 辺の上にあるか
                if (t < 0.001f) // 辺S上
                {
                    edge = p2 - p0; // 辺p0p2

                    vertices1.Add(cutPoint[1]); // 中点の追加
                    vertices1.Add(cutPoint[1] - edge * 0.001f); // ４番目の頂点の追加
                    vertices1.Add(cutPoint[1] + edge * 0.001f); // ４番目の頂点の追加
                    
                }
                else if (s < 0.001f)    // 辺T上
                {
                    edge = p1 - p0; // 辺p0p1

                    vertices1.Add(cutPoint[1]);// 中点の追加
                    vertices1.Add(cutPoint[1] - edge * 0.001f); // ４番目の頂点の追加
                    vertices1.Add(cutPoint[1] + edge * 0.001f); // ４番目の頂点の追加

                }
                else if (s + t > 0.98f) // 辺S+T上
                {
                    edge = p2 - p1; // 辺p1p2

                    vertices1.Add(cutPoint[1]);// 中点の追加
                    vertices1.Add(cutPoint[1] - edge * 0.001f); // ４番目の頂点の追加
                    vertices1.Add(cutPoint[1] + edge * 0.001f); // ４番目の頂点の追加

                }
            }
            else    // 三角形の中にない
            {
                Debug.Log("ポリゴンの中にありません");
            }

            // インデックスの振り分け


        }

    }
}
