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
    //static Vector3 vBgnP;  // カットポイントの始点
    static List<Vector3> vtx = new List<Vector3>();

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
            p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);//+ Vector3.one * 0.0001f;
            p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);//+ Vector3.one * 0.0001f;
            p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]); //+ Vector3.one * 0.0001f;

            // カットポイントの始点がポリゴンの辺の上にあるか
            double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
            double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[0].x + (p0.x - p2.x) * cutPoint[0].z);
            double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[0].x + (p1.x - p0.x) * cutPoint[0].z);

            // まずは三角形の中にあるか
            if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
            {       // 三角形の中にある

                // 頂点リストに追加
                vertices1.Add(p0 - transform.position);
                vertices1.Add(p1 - transform.position);
                vertices1.Add(p2 - transform.position);

                // 辺の上にあるか
                if (t < 0.001f) // 辺S上
                {
                    Debug.Log("辺S上");
                    edge = p1 - p0; // 辺p0p2

                    vertices1.Add(cutPoint[0] + edge * 0.01f - transform.position); // 3番目の頂点の追加
                    vertices1.Add(cutPoint[0] - edge * 0.01f - transform.position); // 4番目の頂点の追加
                    vertices1.Add(cutPoint[1] - transform.position); // 5番目の頂点
                    vertices1.Add(cutPoint[1] - transform.position); // 6番目の頂点

                    // 頂点のインデックス
                    int _0 = vertices1.Count - 7;
                    int _1 = vertices1.Count - 6;
                    int _2 = vertices1.Count - 5;
                    int _3 = vertices1.Count - 4;
                    int _4 = vertices1.Count - 3;
                    int _5 = vertices1.Count - 2;
                    int _6 = vertices1.Count - 1;   // 使わない

                    // インデックスの振り分け
                    triangles1.Add(_5);
                    triangles1.Add(_2);
                    triangles1.Add(_0);

                    triangles1.Add(_5);
                    triangles1.Add(_0);
                    triangles1.Add(_4);

                    triangles1.Add(_5);
                    triangles1.Add(_3);
                    triangles1.Add(_1);

                    triangles1.Add(_5);
                    triangles1.Add(_1);
                    triangles1.Add(_2);
                }
                else if (s < 0.001f)    // 辺T上
                {
                    Debug.Log("辺T上");
                    edge = p2 - p0; // 辺p0p1

                    vertices1.Add(cutPoint[0] - edge * 0.01f - transform.position); // 3番目の頂点の追加
                    vertices1.Add(cutPoint[0] + edge * 0.01f - transform.position); // 4番目の頂点の追加
                    vertices1.Add(cutPoint[1] - transform.position); // 5番目の頂点
                    vertices1.Add(cutPoint[1] - transform.position); // 6番目の頂点

                    // 頂点のインデックス
                    int _0 = vertices1.Count - 7;
                    int _1 = vertices1.Count - 6;
                    int _2 = vertices1.Count - 5;
                    int _3 = vertices1.Count - 4;
                    int _4 = vertices1.Count - 3;
                    int _5 = vertices1.Count - 2;
                    int _6 = vertices1.Count - 1;   // 使わない

                    // インデックスの振り分け
                    triangles1.Add(_5);
                    triangles1.Add(_1);
                    triangles1.Add(_2);

                    triangles1.Add(_5);
                    triangles1.Add(_2);
                    triangles1.Add(_4);

                    triangles1.Add(_5);
                    triangles1.Add(_3);
                    triangles1.Add(_0);

                    triangles1.Add(_5);
                    triangles1.Add(_0);
                    triangles1.Add(_1);

                }
                else if (s + t > 0.98f) // 辺S+T上
                {
                    Debug.Log("辺S + T上");
                    edge = p2 - p1; // 辺p1p2

                    vertices1.Add(cutPoint[0] + edge * 0.01f - transform.position); // 3番目の頂点の追加
                    vertices1.Add(cutPoint[0] - edge * 0.01f - transform.position); // 4番目の頂点の追加
                    vertices1.Add(cutPoint[1] - transform.position); // 5番目の頂点
                    vertices1.Add(cutPoint[1] - transform.position); // 6番目の頂点

                    // 頂点のインデックス
                    int _0 = vertices1.Count - 7;
                    int _1 = vertices1.Count - 6;
                    int _2 = vertices1.Count - 5;
                    int _3 = vertices1.Count - 4;
                    int _4 = vertices1.Count - 3;
                    int _5 = vertices1.Count - 2;
                    int _6 = vertices1.Count - 1;   // 使わない


                    // インデックスの振り分け
                    triangles1.Add(_5);
                    triangles1.Add(_0);
                    triangles1.Add(_1);

                    triangles1.Add(_5);
                    triangles1.Add(_1);
                    triangles1.Add(_4);

                    triangles1.Add(_5);
                    triangles1.Add(_3);
                    triangles1.Add(_2);

                    triangles1.Add(_5);
                    triangles1.Add(_2);
                    triangles1.Add(_0);
                }

                Debug.Log("s:" + s);
                Debug.Log("t:" + t);
                Debug.Log("s + t:" + s + t);
            }
            else    // 三角形の中にない
            {
                Debug.Log("ポリゴンの中にありません");
                Debug.Log("s:" + s);
                Debug.Log("t:" + t);
                Debug.Log("s + t:" + s + t);
            }


        }

        // 分割後のオブジェクト生成、いろいろといれる
        var normal = new List<Vector3>();
        for (int i = 0; i < vertices1.Count; i++)
        {
            normal.Add(Vector3.up);
        }

        attachedMesh.SetVertices(vertices1.ToArray());
        attachedMesh.SetTriangles(triangles1.ToArray(), 0);
        attachedMesh.SetNormals(normal);
        Debug.Log(attachedMesh.vertices.Length);
        // 分割後のオブジェクト生成、いろいろといれる
        //GameObject obj = new GameObject("Plane1", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody), typeof(MeshDivision2));
        //var mesh = new Mesh();
        //mesh.vertices = vertices1.ToArray();
        //mesh.triangles = triangles1.ToArray();
        ////mesh.uv = uvs1.ToArray();


        ////mesh.normals = normals1.ToArray();
        //Debug.Log("mesh.normals.Length" + mesh.normals.Length);
        //obj.GetComponent<MeshFilter>().mesh = mesh;
        //obj.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        //obj.GetComponent<MeshCollider>().sharedMesh = mesh;
        //obj.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
        //obj.GetComponent<MeshCollider>().convex = false;
        //obj.GetComponent<MeshCollider>().material = GetComponent<Collider>().material;
        //obj.transform.position = transform.position;
        //obj.transform.rotation = transform.rotation;
        //obj.transform.localScale = transform.localScale;
        ////obj.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        ////obj.GetComponent<Rigidbody>().angularVelocity = GetComponent<Rigidbody>().angularVelocity;
        ////obj.GetComponent<MeshCut>().skinWidth = skinWidth;


        //obj.GetComponent<Rigidbody>().useGravity = false;   // 重力の無効化
        //obj.GetComponent<Rigidbody>().isKinematic = true;   // 運動を無効化 



        //このオブジェクトをデストロイ
        //Destroy(gameObject);

    }

    // 途中のカットポイントでの分割
    public bool DiviosionMeshMiddle(List<Vector3> cutPoint)
    {
        if (cutPoint.Count > 3) return false;

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
        Vector3 edge1 = new Vector3();
        Vector3 edge2 = new Vector3();


        var crossVertices = new List<Vector3>();

        // 仮で頂点代入
        for (int i = 0; i < attachedMesh.vertices.Length; i++)
        {
            vertices1.Add(attachedMesh.vertices[i]);
        }
        for (int i = 0; i < attachedMesh.triangles.Length; i++)
        {
            triangles1.Add(attachedMesh.triangles[i]);
        }


        // 同じ座標に頂点があったら広げる
        for (int i = 0; i < vertices1.Count - 1; i++)
        {
            // 同じ座標じゃなかったスルー
            if (vertices1[i] != vertices1[i + 1]) continue;

            // 切る方向に対して垂直に点を移動溜めの処理
            edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
            edge2 = (cutPoint[cutPoint.Count - 1] + Vector3.up) - cutPoint[cutPoint.Count - 1];
            edge = Vector3.Cross(edge1, edge2);            

            vertices1[i] = vertices1[i] - edge * 0.1f;
            vertices1[i + 1] = vertices1[i + 1] + edge * 0.1f;
        }

        //カットしたいオブジェクトのメッシュをトライアングルごとに処理
        for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
        {
            //メッシュの3つの頂点を取得
            p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);//+ Vector3.one * 0.0001f;
            p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);//+ Vector3.one * 0.0001f;
            p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]); //+ Vector3.one * 0.0001f;

            // カットポイントの始点がポリゴンの辺の上にあるか
            double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
            double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[cutPoint.Count - 1].x + (p0.x - p2.x) * cutPoint[cutPoint.Count - 1].z);
            double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[cutPoint.Count - 1].x + (p1.x - p0.x) * cutPoint[cutPoint.Count - 1].z);

            // まずは三角形の中にあるか
            if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
            {
                // インデックスの消去
                triangles1.RemoveRange(i,i+2);

            } 
        }

        // メッシュに代入
        attachedMesh.SetVertices(vertices1.ToArray());
        attachedMesh.SetTriangles(triangles1.ToArray(), 0);


        return true;
    }

    // ギズモの表示
    private void OnDrawGizmos()
    {
        if (!attachedMesh) return;




        //for (int i = 0; i < attachedMesh.vertices.Length; i++)
        //{
        //    if (CrossNum.Count > 0)
        //    {
        //        for (int j = 0; j < CrossNum.Count; j++)
        //        {
        //            if (CrossNum[j] == i)
        //            {
        //                Gizmos.color = new Color(0, 0, 225, 1);   // 色の指定
        //                break;
        //            }
        //            else
        //            {
        //                Gizmos.color = new Color(25, 0, 0, 1);   // 色の指定
        //            }

        //        }


        //    }
        //    else
        //    {
        //        Gizmos.color = new Color(25, 0, 0, 1);   // 色の指定
        //    }

        //    Gizmos.DrawSphere(attachedMesh.vertices[i] + transform.position, 0.05f);  // 球の表示

        //}

        for (int i = 0; i < attachedMesh.vertices.Length; i++)
        {
            Gizmos.color = new Color(25, 0, 0, 1);   // 色の指定
            Gizmos.DrawSphere(attachedMesh.vertices[i] + transform.position, 0.01f);
        }

        for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
        {
            Gizmos.color = new Color(25, 0, 0, 1);   // 色の指定
            for (int j = 0; j < 3; j++)
            {
                Gizmos.DrawLine(attachedMesh.vertices[attachedMesh.triangles[i + j]] + transform.position, attachedMesh.vertices[attachedMesh.triangles[i + (j + 1) % 3]] + transform.position);  // 球の表示

            }

        }
    }
}
