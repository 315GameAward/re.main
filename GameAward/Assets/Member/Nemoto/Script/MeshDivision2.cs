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

    private List<int> idxMemory = new List<int>();    // 三角形インデックスの記憶用変数


    public class Triangle
    {
        public int[] idx = new int[3];

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
        for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
        {
            tri = new Triangle();
            trianglesList = new List<Triangle>();
            tri.idx[0] = attachedMesh.triangles[i];
            tri.idx[1] = attachedMesh.triangles[i + 1];
            tri.idx[2] = attachedMesh.triangles[i + 2];
            trianglesList.Add(tri);
        }

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

        // メッシュの情報を代入
        for (int i = 0; i < attachedMesh.vertices.Length; i++)
        {
            vertices1.Add(attachedMesh.vertices[i]);
        }
        for (int i = 0; i < attachedMesh.triangles.Length; i++)
        {
            triangles1.Add(attachedMesh.triangles[i]);
        }

        //カットしたいオブジェクトのメッシュをトライアングルごとに処理
        for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
        {
            //メッシュの3つの頂点を取得
            p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]    ]);//+ Vector3.one * 0.0001f;
            p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);//+ Vector3.one * 0.0001f;
            p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]); //+ Vector3.one * 0.0001f;


            // カットポイントの終点がポリゴンの中にあるか
            Vector2 cp = new Vector2(cutPoint[0].x -transform.position.x, cutPoint[0].z - transform.position.z);
            var v2P0 = new Vector2(p0.x, p0.z);
            var v2P1 = new Vector2(p1.x, p1.z);
            var v2P2 = new Vector2(p2.x, p2.z);

           
            // カットポイントの始点がポリゴンの辺の上にあるか
            double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z) ;
            double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[0].x + (p0.x - p2.x) * cutPoint[0].z);
            double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[0].x + (p1.x - p0.x) * cutPoint[0].z);

            Debug.Log("Area"+Area);

            // まずは三角形の中にあるか
            if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
            {       // 三角形の中にある

                // 頂点リストに追加
                vertices1.Add(p0 - transform.position);
                vertices1.Add(p1 - transform.position);
                vertices1.Add(p2 - transform.position);

                // 辺の上にあるか
                if (t < 0.002f) // 辺S上
                {
                    Debug.Log("辺S上");
                    edge = p1 - p0; // 辺p0p2

                    vertices1.Add(cutPoint[0] + edge * 0.01f - transform.position); // 3番目の頂点の追加
                    vertices1.Add(cutPoint[0] - edge * 0.01f - transform.position); // 4番目の頂点の追加
                    vertices1.Add(cutPoint[1] - transform.position); // 5番目の頂点
                    vertices1.Add(cutPoint[1] - transform.position); // 6番目の頂点

                    // 頂点のインデックス
                    int _0 = attachedMesh.triangles[i]    ;
                    int _1 = attachedMesh.triangles[i + 1];
                    int _2 = attachedMesh.triangles[i + 2];
                    int _3 = vertices1.Count - 4;
                    int _4 = vertices1.Count - 3;
                    int _5 = vertices1.Count - 2;
                    int _6 = vertices1.Count - 1;   // 使わない
                                                   
                    // カットポイントのあるポリゴンのインデックスの削除&追加
                    triangles1.RemoveRange(i, 3);

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

                    // 出来た三角形インデックスの保存
                    idxMemory.Clear();
                    idxMemory.Add(_5);
                    idxMemory.Add(_2);
                    idxMemory.Add(_0);

                    idxMemory.Add(_5);
                    idxMemory.Add(_0);
                    idxMemory.Add(_4);

                    idxMemory.Add(_5);
                    idxMemory.Add(_3);
                    idxMemory.Add(_1);

                    idxMemory.Add(_5);
                    idxMemory.Add(_1);
                    idxMemory.Add(_2);
                }
                else if (s < 0.002f)    // 辺T上
                {
                    Debug.Log("辺T上");
                    edge = p2 - p0; // 辺p0p1

                    vertices1.Add(cutPoint[0] - edge * 0.01f - transform.position); // 3番目の頂点の追加
                    vertices1.Add(cutPoint[0] + edge * 0.01f - transform.position); // 4番目の頂点の追加
                    vertices1.Add(cutPoint[1] - transform.position); // 5番目の頂点
                    vertices1.Add(cutPoint[1] - transform.position); // 6番目の頂点

                    // 頂点のインデックス
                    int _0 = attachedMesh.triangles[i];
                    int _1 = attachedMesh.triangles[i + 1];
                    int _2 = attachedMesh.triangles[i + 2];
                    int _3 = vertices1.Count - 4;
                    int _4 = vertices1.Count - 3;
                    int _5 = vertices1.Count - 2;
                    int _6 = vertices1.Count - 1;   // 使わない

                    // カットポイントのあるポリゴンのインデックスの削除&追加
                    triangles1.RemoveRange(i, 3);

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

                    // 出来た三角形インデックスの保存
                    idxMemory.Clear();
                    idxMemory.Add(_5);
                    idxMemory.Add(_1);
                    idxMemory.Add(_2);

                    idxMemory.Add(_5);
                    idxMemory.Add(_2);
                    idxMemory.Add(_4);

                    idxMemory.Add(_5);
                    idxMemory.Add(_3);
                    idxMemory.Add(_0);

                    idxMemory.Add(_5);
                    idxMemory.Add(_0);
                    idxMemory.Add(_1);

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
                    int _0 = attachedMesh.triangles[i];
                    int _1 = attachedMesh.triangles[i + 1];
                    int _2 = attachedMesh.triangles[i + 2];
                    int _3 = vertices1.Count - 4;
                    int _4 = vertices1.Count - 3;
                    int _5 = vertices1.Count - 2;
                    int _6 = vertices1.Count - 1;   // 使わない

                    // カットポイントのあるポリゴンのインデックスの削除&追加
                    triangles1.RemoveRange(i, 3);

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

                    // 出来た三角形インデックスの保存
                    idxMemory.Clear();
                    idxMemory.Add(_5);
                    idxMemory.Add(_0);
                    idxMemory.Add(_1);

                    idxMemory.Add(_5);
                    idxMemory.Add(_1);
                    idxMemory.Add(_4);

                    idxMemory.Add(_5);
                    idxMemory.Add(_3);
                    idxMemory.Add(_2);

                    idxMemory.Add(_5);
                    idxMemory.Add(_2);
                    idxMemory.Add(_0);
                }

                Debug.Log("s:" + s);
                Debug.Log("t:" + t);
                Debug.Log("s + t:" + s + t);
            }
            else    // 三角形の中にない
            {
              
            }


        }

        // 分割後のオブジェクト生成、いろいろといれる
        var normal = new List<Vector3>();
        for (int i = 0; i < vertices1.Count; i++)
        {
            normal.Add(Vector3.up);
        }

        // メッシュに代入
        attachedMesh.SetVertices(vertices1.ToArray());
        attachedMesh.SetTriangles(triangles1.ToArray(), 0);
        attachedMesh.SetNormals(normal);
       

    }

    // 途中のカットポイントでの分割
    public bool DiviosionMeshMiddle(List<Vector3> cutPoint)
    {
       
        if (cutPoint.Count < 3) return false;
      
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

            vertices1[i] = vertices1[i] - edge.normalized * 0.08f;
            vertices1[i + 1] = vertices1[i + 1] + edge.normalized * 0.08f;
        }

        //カットしたいオブジェクトのメッシュをトライアングルごとに処理
        for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
        {
            //メッシュの3つの頂点を取得
            p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);//+ Vector3.one * 0.0001f;
            p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);//+ Vector3.one * 0.0001f;
            p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]); //+ Vector3.one * 0.0001f;

            // カットポイントの始点がポリゴンの中にあるか
            double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
            double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[cutPoint.Count - 1].x + (p0.x - p2.x) * cutPoint[cutPoint.Count - 1].z);
            double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[cutPoint.Count - 1].x + (p1.x - p0.x) * cutPoint[cutPoint.Count - 1].z);

            // 三角形の中にあるか
            if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
            {
                // 一個前のカットポイントがあるか
                double _s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[cutPoint.Count - 2].x + (p0.x - p2.x) * cutPoint[cutPoint.Count - 2].z);
                double _t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[cutPoint.Count - 2].x + (p1.x - p0.x) * cutPoint[cutPoint.Count - 2].z);
                if ((0 <= _s && _s <= 1) && (0 <= _t && _t <= 1) && (0 <= 1 - _s - _t && 1 - _s - _t <= 1))
                {
                    // あるとき
                    Debug.Log("ある");
                }
                else
                {
                    // ないとき
                    Debug.Log("ない");

                    // 線分と線分の交点に頂点を追加したい

                    // ポリゴンごとに処理
                    for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            // ポリゴンの2頂点
                            Vector2 polyVtx_s = new Vector2(attachedMesh.vertices[attachedMesh.triangles[j + k]].x, attachedMesh.vertices[attachedMesh.triangles[j + k]].z);  // 始点
                            Vector2 polyVtx_v = new Vector2(attachedMesh.vertices[attachedMesh.triangles[j + (k + 1) % 3]].x, attachedMesh.vertices[attachedMesh.triangles[j + (k + 1) % 3]].z);  // 終点

                            // ポリゴンの辺
                            Vector2 polyEdge = polyVtx_v - polyVtx_s;   // 辺

                            // カットポイントの2頂点
                            Vector2 cpVtx_s = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z); // 始点
                            Vector2 cpVtx_v = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z); // 終点

                            // カットポイントの辺
                            Vector2 cpEdge = cpVtx_v - cpVtx_s; // 辺

                            // ポリゴンの辺とカットポイントの辺の始点をつないだベクトル
                            Vector2 v = polyVtx_s - cpVtx_s;

                            // 線分の始点から交点のベクトルの係数(多分)
                            float t1 = (v.x * polyEdge.y - polyEdge.x * v.y) / (cpEdge.x * polyEdge.y - polyEdge.x * cpEdge.y);
                            float t2 = (v.x * cpEdge.y - cpEdge.x * v.y) / (cpEdge.x * polyEdge.y - polyEdge.x * cpEdge.y);

                            // 交点
                            Vector2 p = new Vector2(polyVtx_s.x, polyVtx_s.y) + new Vector2(polyEdge.x * t2, polyEdge.y * t2);



                            // 線分と線分が交わっているか
                            const float eps = 0.00001f;
                            if (t1 + eps < 0 || t1 - eps > 1 || t2 + eps < 0 || t2 - eps > 1)
                            {
                                // Debug.Log("交差してない");
                            }
                            else
                            {
                                Debug.Log("交差してる");

                                // 交点をもとに頂点を追加
                                vertices1.Add(new Vector3(p.x, cutPoint[cutPoint.Count - 1].y - transform.position.y, p.y));
                                vertices1.Add(new Vector3(p.x, cutPoint[cutPoint.Count - 1].y - transform.position.y, p.y));

                                // 同じ座標に頂点があったら広げる
                                for (int l = 0; l < vertices1.Count - 1; l++)
                                {
                                    // 同じ座標じゃなかったスルー
                                    if (vertices1[l] != vertices1[l + 1]) continue;

                                    // 切る方向に対して垂直に点を移動溜めの処理
                                    edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
                                    edge2 = (cutPoint[cutPoint.Count - 1] + Vector3.up) - cutPoint[cutPoint.Count - 1];
                                    edge = Vector3.Cross(edge1, edge2);

                                    vertices1[l] = vertices1[l] - edge.normalized * 0.08f;
                                    vertices1[l + 1] = vertices1[l + 1] + edge.normalized * 0.08f;
                                }

                                // 交差している点がどのポリゴンにいるか調べる

                            }
                        }

                    }



                }


                // カットポイントの場所に頂点の追加(あとで分けるため二つ追加)
                vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);
                vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);

                // インデックスの割り当て
                int _0 = attachedMesh.triangles[i];
                int _1 = attachedMesh.triangles[i + 1];
                int _2 = attachedMesh.triangles[i + 2];
                int _3 = vertices1.Count - 2; // 7
                int _4 = vertices1.Count - 1; // 使わない  
                int _5 = vertices1.Count - 3; // 6

                // 記憶された三角形インデックスの数だけループ
                for (int j = 0; j < idxMemory.Count; j += 3)
                {
                    // 記憶された三角形インデクスと一致した時
                    if (idxMemory.Count > 9)
                    {
                        if (attachedMesh.triangles[i] == idxMemory[j] && attachedMesh.triangles[i + 1] == idxMemory[j + 1] && attachedMesh.triangles[i + 2] == idxMemory[j + 2])
                        {
                            Debug.Log(idxMemory[j] + "" + idxMemory[j + 1] + "" + idxMemory[j + 2]);
                            if (j == 0)
                            {
                                Debug.Log("j = 0");
                                // インデックスの変更
                                triangles1[i + j + 3] = _5;

                                // カットポイントのあるポリゴンのインデックスの削除&追加

                                triangles1.RemoveRange(i, 3);
                                triangles1.Add(_3);
                                triangles1.Add(_1);
                                triangles1.Add(_2);

                                triangles1.Add(_3);
                                triangles1.Add(_2);
                                triangles1.Add(_5);

                                triangles1.Add(_3);
                                triangles1.Add(_0);
                                triangles1.Add(_1);

                                // 出来た三角形インデックスの保存
                                idxMemory.Clear();
                                idxMemory.Add(_3);
                                idxMemory.Add(_1);
                                idxMemory.Add(_2);

                                idxMemory.Add(_3);
                                idxMemory.Add(_2);
                                idxMemory.Add(_5);

                                idxMemory.Add(_3);
                                idxMemory.Add(_0);
                                idxMemory.Add(_1);
                                break;
                            }
                            if (j == 3)
                            {
                                Debug.Log("j = 3");
                                // カットポイントのあるポリゴンのインデックスの削除&追加
                                triangles1.RemoveRange(i, 3);
                                triangles1.Add(_3);
                                triangles1.Add(_0);
                                triangles1.Add(_1);

                                triangles1.Add(_3);
                                triangles1.Add(_1);
                                triangles1.Add(_2);

                                triangles1.Add(_3);
                                triangles1.Add(_2);
                                triangles1.Add(_5);

                                // 出来た三角形インデックスの保存
                                idxMemory.Clear();
                                idxMemory.Add(_3);
                                idxMemory.Add(_0);
                                idxMemory.Add(_1);

                                idxMemory.Add(_3);
                                idxMemory.Add(_1);
                                idxMemory.Add(_2);

                                idxMemory.Add(_3);
                                idxMemory.Add(_2);
                                idxMemory.Add(_5);
                                break;
                            }
                            if (j == 6)
                            {
                                Debug.Log("j = 6");
                                // インデックスの変更
                                triangles1[i +j - 6] = _5;
                                triangles1[i +j - 3] = _5;
                                triangles1[i + j + 3] = _5;

                                // カットポイントのあるポリゴンのインデックスの削除&追加
                                triangles1.RemoveRange(i, 3);
                                triangles1.Add(_3);
                                triangles1.Add(_0);
                                triangles1.Add(_1);

                                triangles1.Add(_3);
                                triangles1.Add(_1);
                                triangles1.Add(_2);

                                triangles1.Add(_3);
                                triangles1.Add(_2);
                                triangles1.Add(_5);

                                // 出来た三角形インデックスの保存
                                idxMemory.Clear();
                                idxMemory.Add(_3);
                                idxMemory.Add(_0);
                                idxMemory.Add(_1);

                                idxMemory.Add(_3);
                                idxMemory.Add(_1);
                                idxMemory.Add(_2);

                                idxMemory.Add(_3);
                                idxMemory.Add(_2);
                                idxMemory.Add(_5);
                                break;
                            }
                            if (j == 9)
                            {
                                Debug.Log("j = 9");
                                triangles1[i + j - 9] = _5;
                                triangles1[i + j - 15] = _5;
                                triangles1[i + j - 18] = _5;

                                // カットポイントのあるポリゴンのインデックスの削除&追加
                                triangles1.RemoveRange(i, 3);
                                triangles1.Add(_3);
                                triangles1.Add(_1);
                                triangles1.Add(_2);

                                triangles1.Add(_3);
                                triangles1.Add(_2);
                                triangles1.Add(_5);

                                triangles1.Add(_3);
                                triangles1.Add(_0);
                                triangles1.Add(_1);

                                // 出来た三角形インデックスの保存
                                idxMemory.Clear();
                                idxMemory.Add(_3);
                                idxMemory.Add(_1);
                                idxMemory.Add(_2);

                                idxMemory.Add(_3);
                                idxMemory.Add(_2);
                                idxMemory.Add(_5);

                                idxMemory.Add(_3);
                                idxMemory.Add(_0);
                                idxMemory.Add(_1);
                                break;
                            }

                        }

                    }
                    else if (idxMemory.Count < 10)  // 記憶された三角形インデックスの数が10よりも少ないとき(三角形が3個)
                    {
                        if (attachedMesh.triangles[i] == idxMemory[j] && attachedMesh.triangles[i + 1] == idxMemory[j + 1] && attachedMesh.triangles[i + 2] == idxMemory[j + 2])
                        {
                            if (j == 0)
                            {
                                Debug.Log("j = 0");
                                // インデックスの変更
                                triangles1[i + j + 3] = _5;

                                // カットポイントのあるポリゴンのインデックスの削除&追加

                                triangles1.RemoveRange(i, 3);
                                triangles1.Add(_3);
                                triangles1.Add(_1);
                                triangles1.Add(_2);

                                triangles1.Add(_3);
                                triangles1.Add(_2);
                                triangles1.Add(_5);

                                triangles1.Add(_3);
                                triangles1.Add(_0);
                                triangles1.Add(_1);

                                // 出来た三角形インデックスの保存
                                idxMemory.Clear();
                                idxMemory.Add(_3);
                                idxMemory.Add(_1);
                                idxMemory.Add(_2);

                                idxMemory.Add(_3);
                                idxMemory.Add(_2);
                                idxMemory.Add(_5);

                                idxMemory.Add(_3);
                                idxMemory.Add(_0);
                                idxMemory.Add(_1);
                                break;
                            }
                            if (j == 3)
                            {
                                Debug.Log("j = 3");

                                triangles1[i + j - 3] = _5;
                                //triangles1[i + j] = _5;
                                // カットポイントのあるポリゴンのインデックスの削除&追加
                                triangles1.RemoveRange(i, 3);
                                triangles1.Add(_3);
                                triangles1.Add(_1);
                                triangles1.Add(_2);

                                triangles1.Add(_3);
                                triangles1.Add(_2);
                                triangles1.Add(_5);

                                triangles1.Add(_3);
                                triangles1.Add(_0);
                                triangles1.Add(_1);

                                // 出来た三角形インデックスの保存
                                idxMemory.Clear();
                                idxMemory.Add(_3);
                                idxMemory.Add(_1);
                                idxMemory.Add(_2);

                                idxMemory.Add(_3);
                                idxMemory.Add(_2);
                                idxMemory.Add(_5);

                                idxMemory.Add(_3);
                                idxMemory.Add(_0);
                                idxMemory.Add(_1);
                                break;
                            }
                            if (j == 6)
                            {
                                Debug.Log("j = 6");
                                Debug.Log("j = " + j);
                                Debug.Log("j + i = " + (j+i));
                                Debug.Log("2回目");

                                // インデックスの変更
                                
                                triangles1[i - 3 ] = _5;
                                triangles1[i - 6 ] = _5;
                                //triangles1[i + j + 3] = _5;

                                // カットポイントのあるポリゴンのインデックスの削除&追加
                                triangles1.RemoveRange(i, 3);
                                triangles1.Add(_3);
                                triangles1.Add(_1);
                                triangles1.Add(_2);

                                triangles1.Add(_3);
                                triangles1.Add(_2);
                                triangles1.Add(_5);

                                triangles1.Add(_3);
                                triangles1.Add(_0);
                                triangles1.Add(_1);

                                // 出来た三角形インデックスの保存
                                idxMemory.Clear();
                                idxMemory.Add(_3);
                                idxMemory.Add(_1);
                                idxMemory.Add(_2);

                                idxMemory.Add(_3);
                                idxMemory.Add(_2);
                                idxMemory.Add(_5);

                                idxMemory.Add(_3);
                                idxMemory.Add(_0);
                                idxMemory.Add(_1);
                                break;
                            }
                        }

                    }



                }

                Debug.Log(idxMemory.Count);


            }
        }

        // ノーマルの設定
        var normal = new List<Vector3>();
        for (int i = 0; i < vertices1.Count; i++)
        {
            normal.Add(Vector3.up);
        }

        // メッシュに代入
        attachedMesh.SetVertices(vertices1.ToArray());
        attachedMesh.SetTriangles(triangles1.ToArray(), 0);
        attachedMesh.SetNormals(normal);


        return true;
    }

    // メッシュの分割(2分割)
    public void DivisionMeshTwice(List<Vector3> cutPoint)
    {
        Debug.Log("DivisionMeshTwice");
        // メッシュのアタッチ
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;

        // 変数
        Vector3 p0, p1, p2;    // メッシュのポリゴンの頂点
        var uvs1 = new List<Vector2>(); // テクスチャ
        var vertices1 = new List<Vector3>();   // 頂点
        var triangles1 = new List<int>();       // 三角形インデックス
        var normals1 = new List<Vector3>();     // 法線
        Vector3 edge = new Vector3();
        Vector3 edge1 = new Vector3();
        Vector3 edge2 = new Vector3();       

        // 頂点とインデックスの代入
        for (int i = 0; i < attachedMesh.vertices.Length; i++)
        {
            vertices1.Add(attachedMesh.vertices[i]);
        }
        for (int i = 0; i < attachedMesh.triangles.Length; i++)
        {
            triangles1.Add(attachedMesh.triangles[i]);
        }

        // カットポイントの場所に頂点の追加(あとで分けるため二つ追加)
        vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);
        vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);

        // 同じ座標に頂点があったら広げる
        for (int i = 0; i < vertices1.Count - 1; i++)
        {
            // 同じ座標じゃなかったスルー
            if (vertices1[i] != vertices1[i + 1]) continue;
            
            // 切る方向に対して垂直に点を移動するめの処理
            edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
            edge2 = (cutPoint[cutPoint.Count - 1] + Vector3.up) - cutPoint[cutPoint.Count - 1];
            edge = Vector3.Cross(edge1, edge2);

            vertices1[i] = vertices1[i] - edge.normalized * 0.08f;
            vertices1[i + 1] = vertices1[i + 1] + edge.normalized * 0.08f;
        }

        

        // メッシュのポリゴンの数だけループ
        for (int i = 0;i < attachedMesh.triangles.Length;i+=3)
        {
            //メッシュの3つの頂点を取得
            p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);//+ Vector3.one * 0.0001f;
            p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);//+ Vector3.one * 0.0001f;
            p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]); //+ Vector3.one * 0.0001f;

            // カットポイントの終点がポリゴンの中にあるか
            Vector2 cp = new Vector2(cutPoint[cutPoint.Count - 2].x + (cutPoint[cutPoint.Count - 1].x - cutPoint[cutPoint.Count - 2].x) * 0.40f - transform.position.x, cutPoint[cutPoint.Count - 2].z + (cutPoint[cutPoint.Count - 1].z - cutPoint[cutPoint.Count - 2].z) * 0.40f - transform.position.z);
            var v2P0 = new Vector2(p0.x,p0.z);
            var v2P1 = new Vector2(p1.x,p1.z);
            var v2P2 = new Vector2(p2.x,p2.z);
         
            double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
            double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * (cp.x +transform.position.x) + (p0.x - p2.x) * (cp.y + transform.position.z));
            double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * (cp.x + transform.position.x) + (p1.x - p0.x) * (cp.y + transform.position.z));
            // 三角形の中にあるか
            if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
            {
                Debug.Log("ポリゴンの中にある");
                //vertices1.Add(new Vector3(cp.x, attachedMesh.vertices[0].y, cp.y));
                // インデックスの割り当て
                int _0 = attachedMesh.triangles[i];
                int _1 = attachedMesh.triangles[i + 1];
                int _2 = attachedMesh.triangles[i + 2];
                int _3 = vertices1.Count - 2; // 7
                int _4 = vertices1.Count - 1; // 使わない  
                int _5 = vertices1.Count - 3; // 6

                // 記憶された三角形インデックスの数だけループ
                for (int j = 0; j < idxMemory.Count; j += 3)
                {
                    if (attachedMesh.triangles[i] == idxMemory[j] && attachedMesh.triangles[i + 1] == idxMemory[j + 1] && attachedMesh.triangles[i + 2] == idxMemory[j + 2])
                    {
                        if (j == 0)
                        {
                            Debug.Log("j = 0");
                            // インデックスの変更
                            triangles1[i + j + 3] = _5;

                            // カットポイントのあるポリゴンのインデックスの削除&追加

                            triangles1.RemoveRange(i, 3);
                            
                            // 三角形インデックスの振り分け
                            triangles1.Add(_4);
                            triangles1.Add(_2);
                            triangles1.Add(_5);

                            triangles1.Add(_3);
                            triangles1.Add(_0);
                            triangles1.Add(_1);

                            // 出来た三角形インデックスの保存
                            idxMemory.Clear();
                           
                            idxMemory.Add(_3);
                            idxMemory.Add(_2);
                            idxMemory.Add(_5);

                            idxMemory.Add(_3);
                            idxMemory.Add(_0);
                            idxMemory.Add(_1);
                            break;
                        }
                        if (j == 3)
                        {
                            Debug.Log("j = 3");

                            triangles1[i + j - 3] = _5;
                            
                            //triangles1[i + j] = _5;
                            // カットポイントのあるポリゴンのインデックスの削除&追加
                            triangles1.RemoveRange(i, 3);
                            triangles1.Add(_4);
                            triangles1.Add(_2);
                            triangles1.Add(_0);
                          
                            triangles1.Add(_3);
                            triangles1.Add(_0);
                            triangles1.Add(_1);

                            // 出来た三角形インデックスの保存
                            idxMemory.Clear();
                            idxMemory.Add(_4);
                            idxMemory.Add(_2);
                            idxMemory.Add(_0);
                          
                            idxMemory.Add(_3);
                            idxMemory.Add(_0);
                            idxMemory.Add(_1);
                            break;
                        }
                        if (j == 6)
                        {
                            Debug.Log("j = 6");
                            Debug.Log("j = " + j);
                            Debug.Log("j + i = " + (j + i));
                            Debug.Log("2回目");

                            // インデックスの変更

                            triangles1[i - 3] = _5;
                            triangles1[i - 6] = _5;
                            //triangles1[i + j + 3] = _5;

                            // カットポイントのあるポリゴンのインデックスの削除&追加
                            triangles1.RemoveRange(i, 3);
                            triangles1.Add(_3);
                            triangles1.Add(_1);
                            triangles1.Add(_2);

                            triangles1.Add(_3);
                            triangles1.Add(_2);
                            triangles1.Add(_5);

                            triangles1.Add(_3);
                            triangles1.Add(_0);
                            triangles1.Add(_1);

                            // 出来た三角形インデックスの保存
                            idxMemory.Clear();
                            idxMemory.Add(_3);
                            idxMemory.Add(_1);
                            idxMemory.Add(_2);

                            idxMemory.Add(_3);
                            idxMemory.Add(_2);
                            idxMemory.Add(_5);

                            idxMemory.Add(_3);
                            idxMemory.Add(_0);
                            idxMemory.Add(_1);
                            break;
                        }
                    }

                }

            }

            //Debug.Log("頂点の追加");
            //vertices1.Add(new Vector3(cp.x, attachedMesh.vertices[0].y, cp.y));


        }


        // ノーマルの設定
        var normal = new List<Vector3>();
        for (int i = 0; i < vertices1.Count; i++)
        {
            normal.Add(Vector3.up);
        }

        // メッシュに代入
        attachedMesh.SetVertices(vertices1.ToArray());
        attachedMesh.SetTriangles(triangles1.ToArray(), 0);
        attachedMesh.SetNormals(normal);


    }

    // ギズモの表示
    private void OnDrawGizmos()
    {
        if (!attachedMesh) return;


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

    float sign(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
    }

    bool PointInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
    {
        bool b1, b2, b3;

        b1 = sign(pt, v1, v2) < 0.0f;
        b2 = sign(pt, v2, v3) < 0.0f;
        b3 = sign(pt, v3, v1) < 0.0f;

        return ((b1 == b2) && (b2 == b3));
    }
}
