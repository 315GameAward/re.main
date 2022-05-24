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
using System.Linq;

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

    // メッシュの分割(最初)
    public void DivisionMesh(List<Vector3> cutPoint)
    {
        Debug.Log("================= 切り始め ==================");
        // メッシュのアタッチ
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;

        // 変数
        var uvs1 = new List<Vector2>(); // テクスチャ
        var vertices1 = new List<Vector3>();   // 頂点
        var triangles1 = new List<int>();       // 三角形インデックス
        var normals1 = new List<Vector3>();     // 法線
       
        // メッシュの情報を代入
        for (int i = 0; i < attachedMesh.vertices.Length; i++)
        {
            vertices1.Add(attachedMesh.vertices[i]);
        }
        for (int i = 0; i < attachedMesh.triangles.Length; i++)
        {
            triangles1.Add(attachedMesh.triangles[i]);
        }

        // 変数宣言              
        var straddlePolyIdx = new List<int>();  // またいだポリゴン番号リスト
        var crossPolyIdx = new List<int>();     // 交差ポリゴン番号リスト
        var inerPolyIdx = new List<int>();      // カットポイントが中に入っているポリゴン番号
        var intersectPolyList = new List<List<Vector2>>();  // ポリゴンごとにある交差している点のリスト
        var intersectPolyList2 = new List<List<Vector2>>();  // ポリゴンごとにある交差している点のリスト2
        var intersectEdgList = new List<List<Vector2>>();   // ポリゴンごとにある交差している辺のリスト
        var intersectEdgList2 = new List<List<Vector2>>();   // ポリゴンごとにある交差している辺のリスト
        var intersectionList = new List<Vector2>();         // 交点のリスト
        var cp_s = new Vector2(cutPoint[cutPoint.Count - 2].x, cutPoint[cutPoint.Count - 2].z);    // カットポイントの終点の1個前
        var cp_v = new Vector2(cutPoint[cutPoint.Count - 1].x, cutPoint[cutPoint.Count - 1].z);    // カットポイントの終点
        var cpEdg = cp_v - cp_s;    // カットポイントの終点とカットポイントの終点の1個前をつないだ辺
        var checkCp = cp_s + cpEdg * 0.01f; // カットポイントの終点の1個前からカットポイントの終点の方向にちょっと伸ばした点
        var edgIdx2List = new List<List<int>>();   // 辺のインデックスのリストのリスト   
        var edgIdx2List2 = new List<List<int>>();  // 辺のインデックスのリストのリスト2   

        // またいでるポリゴンと侵入しているポリゴンが何個あるか探す
        for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
        {
            // 変数宣言             
            int interPointCnt = 0; // 交差した点の数
            var intersection = new List<Vector2>(); // 交点のリスト
            var edgList = new List<Vector2>(); //辺のリスト
            var edgIdxList = new List<int>();   // 辺のインデックスのリスト   

            // ポリゴンの辺の数だけループ
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

                // カットポイントの始点の補正
                cpVtx_s -= cpEdge * 0.02f;

                // カットポイントの辺の補正
                cpEdge = cpVtx_v - cpVtx_s; // 辺

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
                    // 交わってないときスルー
                    continue;
                }
                else
                {
                    // 交わってる時交点カウント++                               
                    interPointCnt++;    // 交点カウント    
                    intersection.Add(p);    // 交点の保存
                    intersectionList.Add(p);// 交点の保存
                    edgList.Add(polyEdge);
                    edgIdxList.Add(attachedMesh.triangles[j + k]);
                    edgIdxList.Add(attachedMesh.triangles[j + (k + 1) % 3]);
                  
                }
            }

            // ポリゴン番号を保存
            if (interPointCnt == 2)// 交点カウント2個(ポリゴンをまたいでる時)
            {
                Debug.Log("2個あるで");
                //Debug.Log("ポリゴン番号は" + attachedMesh.triangles[j] + "," + attachedMesh.triangles[j + 1] + "," + attachedMesh.triangles[j + 2]);

                straddlePolyIdx.Add(attachedMesh.triangles[j]    );
                straddlePolyIdx.Add(attachedMesh.triangles[j + 1]);
                straddlePolyIdx.Add(attachedMesh.triangles[j + 2]);
                crossPolyIdx.Add(j);
                crossPolyIdx.Add(j + 1);
                crossPolyIdx.Add(j + 2);
                intersectPolyList2.Add(intersection);
                intersectEdgList2.Add(edgList);
                edgIdx2List2.Add(edgIdxList);
                //Debug.Log("straddlePolyIdx.Count" + straddlePolyIdx.Count);
            }
            else if (interPointCnt == 1)// 交点カウント1個(カットポイントの終点がポリゴンの中にあるとき)
            {
                Debug.Log("1個あるで");
                inerPolyIdx.Add(attachedMesh.triangles[j]    );
                inerPolyIdx.Add(attachedMesh.triangles[j + 1]);
                inerPolyIdx.Add(attachedMesh.triangles[j + 2]);
                crossPolyIdx.Add(j);
                crossPolyIdx.Add(j + 1);
                crossPolyIdx.Add(j + 2);
                edgIdx2List.Add(edgIdxList);
                intersectPolyList.Add(intersection);
                intersectEdgList.Add(edgList);
            }
            else
            {
                // Debug.Log("3個あるで");
                // Debug.Log("ポリゴン番号は" + attachedMesh.triangles[j] + "," + attachedMesh.triangles[j + 1] + "," + attachedMesh.triangles[j + 2]);

            }
        }

        //--- カットポイントとの交点の数で分岐 ---
        // 交点が1個の時(ポリゴンの中に2個交点があるポリゴンが1個もないとき)
        if(straddlePolyIdx.Count == 0)
        {
            Debug.Log("=============中間:交点が1個=============");

            //--- 変数宣言 ---
            int firstNum = 0;
            Vector2 cpS = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z);
            Vector2 cpV = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z);
            Vector2 p = cpV;

            var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
            var cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));
            var pEnd = intersectPolyList[firstNum][0];    // 交点の終点
            var pEdge = new Vector2(Mathf.Abs(intersectEdgList[0][0].x), Mathf.Abs(intersectEdgList[0][0].y));
           
            // 4分割する処理
            {
                Debug.Log("4分割する処理");
                //Debug.Log("edgIdx2List[0][0]:" + edgIdx2List[0][0]);
                //Debug.Log("edgIdx2List[0][0]:" + edgIdx2List[0][1]);
                //Debug.Log("inerPolyIdx.Count:" + inerPolyIdx.Count);
                //Debug.Log("inerPolyIdx[0]:" + inerPolyIdx[0]);
                //Debug.Log("inerPolyIdx[1]:" + inerPolyIdx[1]);
                //Debug.Log("inerPolyIdx[2]:" + inerPolyIdx[2]);

                // 対象のインデックスの削除
                for (int a = 0; a < triangles1.Count; a += 3)
                {
                    // 一致しなかったらスルー
                    if (!(triangles1[a] == inerPolyIdx[0] && triangles1[a + 1] == inerPolyIdx[1] && triangles1[a + 2] == inerPolyIdx[2])) continue;

                    triangles1.RemoveRange(a, 3);
                    break;
                }

                // 頂点の追加
                vertices1.Add(new Vector3(cutPoint[0].x - transform.position.x, attachedMesh.vertices[0].y, cutPoint[0].z - transform.position.z) + new Vector3(pEdge.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * -cpNormalAbs.z));
                vertices1.Add(new Vector3(cutPoint[0].x - transform.position.x, attachedMesh.vertices[0].y, cutPoint[0].z - transform.position.z) + new Vector3(pEdge.normalized.x * 0.04f * cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * cpNormalAbs.z));
                
                vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);
                vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);

                // インデックスの割り当て
                int idx0 = inerPolyIdx[0];
                int idx1 = inerPolyIdx[1];
                int idx2 = inerPolyIdx[2];
                int idx3 = vertices1.Count - 4; // 
                int idx4 = vertices1.Count - 3; //  
                int idx5 = vertices1.Count - 2; // 
                int idx6 = vertices1.Count - 1; // 

                // 交点の始点がどの辺にあるか
                // 辺01の上
                if ((edgIdx2List[0][0] == inerPolyIdx[0] || edgIdx2List[0][0] == inerPolyIdx[1]) &&
                   (edgIdx2List[0][1] == inerPolyIdx[1] || edgIdx2List[0][1] == inerPolyIdx[0]))
                {
                    Debug.Log("辺01の上");
                    triangles1.Add(idx5);
                    triangles1.Add(idx2);
                    triangles1.Add(idx0);

                    triangles1.Add(idx5);
                    triangles1.Add(idx0);
                    triangles1.Add(idx4);

                    triangles1.Add(idx5);
                    triangles1.Add(idx3);
                    triangles1.Add(idx1);

                    triangles1.Add(idx5);
                    triangles1.Add(idx1);
                    triangles1.Add(idx2);

                    // 出来た三角形インデックスの保存
                    idxMemory.Clear();
                    idxMemory.Add(idx5);
                    idxMemory.Add(idx2);
                    idxMemory.Add(idx0);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx0);
                    idxMemory.Add(idx4);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx3);
                    idxMemory.Add(idx1);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx1);
                    idxMemory.Add(idx2);
                }
                // 辺12の上
                else if ((edgIdx2List[0][0] == inerPolyIdx[1] || edgIdx2List[0][0] == inerPolyIdx[2]) &&
                   (edgIdx2List[0][1] == inerPolyIdx[2] || edgIdx2List[0][1] == inerPolyIdx[1]))
                {
                    Debug.Log("辺12の上");
                    triangles1.Add(idx5);
                    triangles1.Add(idx0);
                    triangles1.Add(idx1);

                    triangles1.Add(idx5);
                    triangles1.Add(idx1);
                    triangles1.Add(idx4);

                    triangles1.Add(idx5);
                    triangles1.Add(idx3);
                    triangles1.Add(idx2);

                    triangles1.Add(idx5);
                    triangles1.Add(idx2);
                    triangles1.Add(idx0);

                    // 出来た三角形インデックスの保存
                    idxMemory.Clear();
                    idxMemory.Add(idx5);
                    idxMemory.Add(idx0);
                    idxMemory.Add(idx1);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx1);
                    idxMemory.Add(idx4);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx3);
                    idxMemory.Add(idx2);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx2);
                    idxMemory.Add(idx0);
                }
                // 辺20の上
                else if ((edgIdx2List[0][0] == inerPolyIdx[2] || edgIdx2List[0][0] == inerPolyIdx[0]) &&
                   (edgIdx2List[0][1] == inerPolyIdx[0] || edgIdx2List[0][1] == inerPolyIdx[2]))
                {
                    Debug.Log("辺20の上");
                    triangles1.Add(idx5);
                    triangles1.Add(idx1);
                    triangles1.Add(idx2);

                    triangles1.Add(idx5);
                    triangles1.Add(idx2);
                    triangles1.Add(idx4);

                    triangles1.Add(idx5);
                    triangles1.Add(idx3);
                    triangles1.Add(idx0);

                    triangles1.Add(idx5);
                    triangles1.Add(idx0);
                    triangles1.Add(idx1);

                    // 出来た三角形インデックスの保存
                    idxMemory.Clear();
                    idxMemory.Add(idx5);
                    idxMemory.Add(idx1);
                    idxMemory.Add(idx2);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx2);
                    idxMemory.Add(idx4);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx3);
                    idxMemory.Add(idx0);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx0);
                    idxMemory.Add(idx1);
                }
            }

            //Debug.Log("交点が1個");
            //カットしたいオブジェクトのメッシュをトライアングルごとに処理
            {
                //for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
                //{
                //    //メッシュの3つの頂点を取得
                //    p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);//+ Vector3.one * 0.0001f;
                //    p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);//+ Vector3.one * 0.0001f;
                //    p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]); //+ Vector3.one * 0.0001f;


                //    // カットポイントの始点がポリゴンの中にあるか
                //    Vector2 cpS = new Vector2(cutPoint[0].x, cutPoint[0].z);
                //    Vector2 cpV = new Vector2(cutPoint[1].x, cutPoint[1].z);
                //    var cp = cpV - cpS;
                //    cpS += cp * 0.02f;
                //    var v2P0 = new Vector2(p0.x, p0.z);
                //    var v2P1 = new Vector2(p1.x, p1.z);
                //    var v2P2 = new Vector2(p2.x, p2.z);


                //    // カットポイントの始点がポリゴンの辺の上にあるか
                //    double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
                //    double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cpS.x + (p0.x - p2.x) * cpS.y);
                //    double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cpS.x + (p1.x - p0.x) * cpS.y);

                //    //Debug.Log("Area" + Area);

                //    // まずは三角形の中にあるか
                //    if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
                //    {       // 三角形の中にある

                //        // 頂点リストに追加
                //        vertices1.Add(p0 - transform.position);
                //        vertices1.Add(p1 - transform.position);
                //        vertices1.Add(p2 - transform.position);

                //        // 辺の上にあるか
                //        if (t < 0.002f) // 辺S上
                //        {
                //            Debug.Log("辺S上");
                //            edge = p1 - p0; // 辺p0p2

                //            vertices1.Add(cutPoint[0] + edge.normalized * 0.1f - transform.position); // 3番目の頂点の追加
                //            vertices1.Add(cutPoint[0] - edge.normalized * 0.1f - transform.position); // 4番目の頂点の追加
                //            vertices1.Add(cutPoint[1] - transform.position); // 5番目の頂点
                //            vertices1.Add(cutPoint[1] - transform.position); // 6番目の頂点

                //            // 頂点のインデックス
                //            int _0 = attachedMesh.triangles[i];
                //            int _1 = attachedMesh.triangles[i + 1];
                //            int _2 = attachedMesh.triangles[i + 2];
                //            int _3 = vertices1.Count - 4;
                //            int _4 = vertices1.Count - 3;
                //            int _5 = vertices1.Count - 2;
                //            int _6 = vertices1.Count - 1;   // 使わない

                //            // カットポイントのあるポリゴンのインデックスの削除&追加
                //            triangles1.RemoveRange(i, 3);

                //            // インデックスの振り分け
                //            triangles1.Add(_5);
                //            triangles1.Add(_2);
                //            triangles1.Add(_0);

                //            triangles1.Add(_5);
                //            triangles1.Add(_0);
                //            triangles1.Add(_4);

                //            triangles1.Add(_5);
                //            triangles1.Add(_3);
                //            triangles1.Add(_1);

                //            triangles1.Add(_5);
                //            triangles1.Add(_1);
                //            triangles1.Add(_2);

                //            // 出来た三角形インデックスの保存
                //            idxMemory.Clear();
                //            idxMemory.Add(_5);
                //            idxMemory.Add(_2);
                //            idxMemory.Add(_0);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_0);
                //            idxMemory.Add(_4);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_3);
                //            idxMemory.Add(_1);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_1);
                //            idxMemory.Add(_2);
                //        }
                //        else if (s < 0.002f)    // 辺T上
                //        {
                //            Debug.Log("辺T上");
                //            edge = p2 - p0; // 辺p0p1

                //            vertices1.Add(cutPoint[0] - edge.normalized * 0.1f - transform.position); // 3番目の頂点の追加
                //            vertices1.Add(cutPoint[0] + edge.normalized * 0.1f - transform.position); // 4番目の頂点の追加
                //            vertices1.Add(cutPoint[1] - transform.position); // 5番目の頂点
                //            vertices1.Add(cutPoint[1] - transform.position); // 6番目の頂点

                //            // 頂点のインデックス
                //            int _0 = attachedMesh.triangles[i];
                //            int _1 = attachedMesh.triangles[i + 1];
                //            int _2 = attachedMesh.triangles[i + 2];
                //            int _3 = vertices1.Count - 4;
                //            int _4 = vertices1.Count - 3;
                //            int _5 = vertices1.Count - 2;
                //            int _6 = vertices1.Count - 1;   // 使わない

                //            // カットポイントのあるポリゴンのインデックスの削除&追加
                //            triangles1.RemoveRange(i, 3);

                //            // インデックスの振り分け
                //            triangles1.Add(_5);
                //            triangles1.Add(_1);
                //            triangles1.Add(_2);

                //            triangles1.Add(_5);
                //            triangles1.Add(_2);
                //            triangles1.Add(_4);

                //            triangles1.Add(_5);
                //            triangles1.Add(_3);
                //            triangles1.Add(_0);

                //            triangles1.Add(_5);
                //            triangles1.Add(_0);
                //            triangles1.Add(_1);

                //            // 出来た三角形インデックスの保存
                //            idxMemory.Clear();
                //            idxMemory.Add(_5);
                //            idxMemory.Add(_1);
                //            idxMemory.Add(_2);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_2);
                //            idxMemory.Add(_4);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_3);
                //            idxMemory.Add(_0);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_0);
                //            idxMemory.Add(_1);

                //        }
                //        else if (s + t > 0.98f) // 辺S+T上
                //        {
                //            Debug.Log("辺S + T上");
                //            edge = p2 - p1; // 辺p1p2

                //            vertices1.Add(cutPoint[0] + edge.normalized * 0.1f - transform.position); // 3番目の頂点の追加
                //            vertices1.Add(cutPoint[0] - edge.normalized * 0.1f - transform.position); // 4番目の頂点の追加
                //            vertices1.Add(cutPoint[1] - transform.position); // 5番目の頂点
                //            vertices1.Add(cutPoint[1] - transform.position); // 6番目の頂点

                //            // 頂点のインデックス
                //            int _0 = attachedMesh.triangles[i];
                //            int _1 = attachedMesh.triangles[i + 1];
                //            int _2 = attachedMesh.triangles[i + 2];
                //            int _3 = vertices1.Count - 4;
                //            int _4 = vertices1.Count - 3;
                //            int _5 = vertices1.Count - 2;
                //            int _6 = vertices1.Count - 1;   // 使わない

                //            // カットポイントのあるポリゴンのインデックスの削除&追加
                //            triangles1.RemoveRange(i, 3);

                //            // インデックスの振り分け
                //            triangles1.Add(_5);
                //            triangles1.Add(_0);
                //            triangles1.Add(_1);

                //            triangles1.Add(_5);
                //            triangles1.Add(_1);
                //            triangles1.Add(_4);

                //            triangles1.Add(_5);
                //            triangles1.Add(_3);
                //            triangles1.Add(_2);

                //            triangles1.Add(_5);
                //            triangles1.Add(_2);
                //            triangles1.Add(_0);

                //            // 出来た三角形インデックスの保存
                //            idxMemory.Clear();
                //            idxMemory.Add(_5);
                //            idxMemory.Add(_0);
                //            idxMemory.Add(_1);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_1);
                //            idxMemory.Add(_4);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_3);
                //            idxMemory.Add(_2);

                //            idxMemory.Add(_5);
                //            idxMemory.Add(_2);
                //            idxMemory.Add(_0);
                //        }
                //        else
                //        {
                //            //Debug.Log("ない");
                //        }
                //    }
                //    else    // 三角形の中にない
                //    {
                //        // Debug.Log("三角形の中にない");
                //    }

                //    Debug.Log("s:" + s);
                //    Debug.Log("t:" + t);
                //    Debug.Log("s + t:" + (s + t));
                //}

            }

        }
        // 交点が2個以上の時
        else
        {

            Debug.Log("=============中間:交点が2個以上=============");
            
            //--- 変数宣言 ---
            int firstNum  = 0;
            int secondNum = 0;
            Vector2 cpS = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z);
            Vector2 cpV = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z);
            Vector2 p = cpV;
            var idxList = new List<int>();  // 1個前に分割したインデックスのリスト
            var rastIdxList = new List<int>();  // 2個に分割する処理をした最後のインデックス保存用のリスト

            // カットポイントの始点がどのポリゴンの辺にいるのか
            for (int i = 0; i < intersectPolyList2.Count; i++)
            {
                for(int j = 0;j < intersectPolyList2[i].Count;j++)
                {
                    // カットポイントの始点の1個後と交点との距離が一番遠い(始点になる)点を探す
                    if (Vector2.Distance(cpV, p) > Vector2.Distance(cpV, intersectPolyList2[i][j])) continue;

                    p = intersectPolyList2[i][j];   // 始点
                    firstNum  = i; // 最初の番号
                    secondNum = j; // 次の番号
                    //Debug.Log("交点ミッケ");
                }                
            }

            //Debug.Log("pEdgeCount" + intersectEdgList2[firstNum].Count);
            //Debug.Log("pEdge1" + intersectEdgList2[firstNum][0]);
            //Debug.Log("pEdge2" + intersectEdgList2[firstNum][1]);
            
           // 最初に2分割する処理
           {
                Debug.Log("中間:最初に分割する処理");

                var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
                var cpNormalAbs = new Vector3();
                if (cpNormal.x == 0)
                {
                    cpNormalAbs = new Vector3(0, 0, cpNormal.z / Mathf.Abs(cpNormal.z));

                }
                else if (cpNormal.z == 0)
                {
                    cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, 0);

                }
                else
                {
                    cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));

                }
                var pEnd = intersectPolyList2[firstNum][1];    // 交点の終点
                var pEdge = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][secondNum].x) , Mathf.Abs(intersectEdgList2[firstNum][secondNum].y)) ;                
                var pEdge2 = new Vector2();     // 交点の終点の辺ベクトル

                // 頂点の追加
                if (secondNum == 0)
                {
                    pEnd   = intersectPolyList2[firstNum][1];    // 交点の終点
                    pEdge2 = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][1].x), Mathf.Abs(intersectEdgList2[firstNum][1].y));// 交点の終点の辺ベクトル                    
                    idxList.Add(edgIdx2List2[firstNum][2]); // 候補の追加
                    idxList.Add(edgIdx2List2[firstNum][3]); // 候補の追加
                }
                else if(secondNum == 1)
                {
                    pEnd = intersectPolyList2[firstNum][0];    // 交点の終点
                    pEdge2 = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][0].x), Mathf.Abs(intersectEdgList2[firstNum][0].y));// 交点の終点の辺ベクトル                                
                    idxList.Add(edgIdx2List2[firstNum][0]); // 候補の追加
                    idxList.Add(edgIdx2List2[firstNum][1]); // 候補の追加
                }

                // カットポイントの始点   
                vertices1.Add(new Vector3(p.x, attachedMesh.vertices[0].y, p.y) + new Vector3(pEdge.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * -cpNormalAbs.z));
                vertices1.Add(new Vector3(p.x, attachedMesh.vertices[0].y, p.y) + new Vector3(pEdge.normalized.x * 0.04f * cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * cpNormalAbs.z));
                vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f *  -cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f *  -cpNormalAbs.z));
                vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f *  cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f *  cpNormalAbs.z));

                // インデックスの割り当て
                int idx0 = straddlePolyIdx[(firstNum * 3)];
                int idx1 = straddlePolyIdx[(firstNum * 3) + 1];
                int idx2 = straddlePolyIdx[(firstNum * 3) + 2];
                int idx3 = vertices1.Count - 4; // 
                int idx4 = vertices1.Count - 3; //  
                int idx5 = vertices1.Count - 2; // 
                int idx6 = vertices1.Count - 1; // 
                int removeIdx = -1;
                
                // 削除する三角形の検索
                for (int a = 0;a < attachedMesh.triangles.Length;a += 3)
                {
                    if (!(attachedMesh.triangles[a] == idx0 && attachedMesh.triangles[a + 1] == idx1 && attachedMesh.triangles[a + 2] == idx2)) continue;
                    removeIdx = a;
                }

                //Debug.Log("edgIdx2List2[firstNum][0]:" + edgIdx2List2[firstNum][0]);
                //Debug.Log("edgIdx2List2[firstNum][1]:"+edgIdx2List2[firstNum][1]);
                //Debug.Log("edgIdx2List2[firstNum][2]:"+edgIdx2List2[firstNum][2]);
                //Debug.Log("edgIdx2List2[firstNum][3]:"+edgIdx2List2[firstNum][3]);
                //Debug.Log("idx0" + idx0);
                //Debug.Log("idx1"+idx1);
                //Debug.Log("idx2"+idx2);

                // インデックスの割り振り
                if(secondNum == 0)
                {
                    // edgIdx2List2[firstNum][0]、edgIdx2List2[firstNum][1]が始点の交点

                    // インデックスの削除
                    triangles1.RemoveRange(removeIdx, 3);

                    // 01-12インデックス
                    if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx1)) &&
                        ((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)))
                    {
                        Debug.Log("01-12インデックス");
                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx1);

                        triangles1.Add(idx6);
                        triangles1.Add(idx2);
                        triangles1.Add(idx0);

                        triangles1.Add(idx6);
                        triangles1.Add(idx0);
                        triangles1.Add(idx4);
                    }
                    // 01-02インデックス
                    else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx1)) &&
                             ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)))
                    {
                        Debug.Log("01-02インデックス");
                        triangles1.Add(idx6);
                        triangles1.Add(idx0);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx1);
                        triangles1.Add(idx2);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx1);
                      
                    }
                    // 02-12インデックス
                    else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)) &&
                             ((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)))
                    {
                        Debug.Log("02-12インデックス");
                        triangles1.Add(idx6);
                        triangles1.Add(idx2);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx0);

                        triangles1.Add(idx5);
                        triangles1.Add(idx0);
                        triangles1.Add(idx1);
                    }
                    // 02-01インデックス
                    else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)) &&
                             ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx1 || edgIdx2List2[firstNum][3] == idx0)))
                    {
                        Debug.Log("02-01インデックス");
                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx0);

                        triangles1.Add(idx6);
                        triangles1.Add(idx1);
                        triangles1.Add(idx2);

                        triangles1.Add(idx6);
                        triangles1.Add(idx2);
                        triangles1.Add(idx4);

                       
                    }
                    // 12-01インデックス
                    else if (((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)) &&
                             ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx1 || edgIdx2List2[firstNum][3] == idx0)))
                    {
                        Debug.Log("12-01インデックス");
                        triangles1.Add(idx6);
                        triangles1.Add(idx1);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx2);

                        triangles1.Add(idx5);
                        triangles1.Add(idx2);
                        triangles1.Add(idx0);
                    }
                    // 12-20インデックス
                    else if (((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)) &&
                             ((edgIdx2List2[firstNum][2] == idx2 || edgIdx2List2[firstNum][2] == idx0) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx2)))
                    {
                        Debug.Log(" 12-02インデックス");
                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx2);

                        triangles1.Add(idx6);
                        triangles1.Add(idx1);
                        triangles1.Add(idx4);

                        triangles1.Add(idx6);
                        triangles1.Add(idx0);
                        triangles1.Add(idx1);
                    }
                  

                }
                else if(secondNum == 1)
                {
                   
                    // インデックスの削除
                    triangles1.RemoveRange(removeIdx, 3);

                    // 01-12インデックス
                    if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx1)) &&
                             ((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)))
                    {
                        Debug.Log("01-12インデックス");
                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx1);

                        triangles1.Add(idx6);
                        triangles1.Add(idx2);
                        triangles1.Add(idx0);

                        triangles1.Add(idx6);
                        triangles1.Add(idx0);
                        triangles1.Add(idx4);
                    }
                    // 01-02インデックス
                    else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx1)) &&
                             ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)))
                    {
                        Debug.Log("01-02インデックス");
                        triangles1.Add(idx6);
                        triangles1.Add(idx0);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx1);
                        triangles1.Add(idx2);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx1);

                    }
                    // 02-12インデックス
                    else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)) &&
                             ((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)))
                    {
                        Debug.Log("02-12インデックス");
                        triangles1.Add(idx6);
                        triangles1.Add(idx2);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx0);

                        triangles1.Add(idx5);
                        triangles1.Add(idx0);
                        triangles1.Add(idx1);
                    }
                    // 02-01インデックス
                    else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)) &&
                             ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx1 || edgIdx2List2[firstNum][1] == idx0)))
                    {
                        Debug.Log("02-01インデックス");
                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx0);

                        triangles1.Add(idx6);
                        triangles1.Add(idx1);
                        triangles1.Add(idx2);

                        triangles1.Add(idx6);
                        triangles1.Add(idx2);
                        triangles1.Add(idx4);


                    }
                    // 12-01インデックス
                    else if (((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)) &&
                             ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx1 || edgIdx2List2[firstNum][1] == idx0)))
                    {
                        Debug.Log("12-01インデックス");
                        triangles1.Add(idx6);
                        triangles1.Add(idx1);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx2);

                        triangles1.Add(idx5);
                        triangles1.Add(idx2);
                        triangles1.Add(idx0);
                    }
                    // 12-20インデックス
                    else if (((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)) &&
                             ((edgIdx2List2[firstNum][0] == idx2 || edgIdx2List2[firstNum][0] == idx0) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx2)))
                    {
                        Debug.Log(" 12-02インデックス");
                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx2);

                        triangles1.Add(idx6);
                        triangles1.Add(idx1);
                        triangles1.Add(idx4);

                        triangles1.Add(idx6);
                        triangles1.Add(idx0);
                        triangles1.Add(idx1);
                    }
                    
                }

                // 候補から削除
                intersectPolyList2.RemoveAt(firstNum);
                intersectEdgList2.RemoveAt(firstNum);
                edgIdx2List2.RemoveAt(firstNum);
            }

            // 2分割する処理(途中)
            {
                Debug.Log("中間:2分割する処理(途中)");

                //--- 変数宣言 ---
                int count = 0;
                var idxCnt = straddlePolyIdx;
                idxCnt.RemoveRange(firstNum * 3, 3); // 候補保存用
                //Debug.Log("idxCnt.Count" + idxCnt.Count);
                //Debug.Log("idxList.Count" + idxList.Count);

                // 交点が2個あるポリゴンの候補がなくなるかカウントが一定以上になるまでループ
                while (count < 50 && idxCnt.Count > 0)
                {
                    bool end = false;

                    // 候補の数だけループ
                    for(int k = 0;k < idxCnt.Count;k += 3)
                    {
                        // 辺の数だけループ
                        for(int h = 0;h < 3;h++)
                        {
                            // 候補と一致しなかったらスルー、一致したら分割対象のインデックスが分かる
                            if (!((idxCnt[k + h] == idxList[0] || idxCnt[k + h] == idxList[1]) && (idxCnt[k + ((h + 1)%3)] == idxList[0] || straddlePolyIdx[k + ((h + 1) % 3)] == idxList[1]))) continue;
                            
                            // 保存された候補リストから今回使ったインデックスを削除
                            for(int g = 0; g < straddlePolyIdx.Count;g += 3)
                            {
                                // 候補と一致しなかったらスルー
                                if (!(idxCnt[k] == straddlePolyIdx[g] && idxCnt[k + 1] == straddlePolyIdx[g + 1] && idxCnt[k + 2] == straddlePolyIdx[g + 2])) continue;


                                // ポリゴンのインデックスの最初の番号
                                firstNum = g / 3;

                                // 変数宣言
                                var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
                                var cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));
                                var pEnd = intersectPolyList2[firstNum][1];    // 交点の終点
                                var pEdge = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][secondNum].x), Mathf.Abs(intersectEdgList2[firstNum][secondNum].y));
                                var pEdge2 = new Vector2();     // 交点の終点の辺ベクトル

                                Debug.Log("edgIdx2List2[firstNum][0]:" + edgIdx2List2[firstNum][0]);
                                Debug.Log("edgIdx2List2[firstNum][1]:" + edgIdx2List2[firstNum][1]);
                                Debug.Log("edgIdx2List2[firstNum][2]:" + edgIdx2List2[firstNum][2]);
                                Debug.Log("edgIdx2List2[firstNum][3]:" + edgIdx2List2[firstNum][3]);

                                // どっちが交点の始点か調べる
                                if ((edgIdx2List2[firstNum][0] == idxList[0] || edgIdx2List2[firstNum][0] == idxList[1]) && (edgIdx2List2[firstNum][1] == idxList[1] || edgIdx2List2[firstNum][1] == idxList[0]) )
                                {
                                    Debug.Log("頂点の追加");
                                    secondNum = 0;
                                    pEnd = intersectPolyList2[firstNum][1];    // 交点の終点
                                    pEdge2 = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][1].x), Mathf.Abs(intersectEdgList2[firstNum][1].y));// 交点の終点の辺ベクトル                                                      
                                    idxList.Clear();    // 候補の削除
                                    idxList.Add(edgIdx2List2[firstNum][2]); // 候補の追加
                                    idxList.Add(edgIdx2List2[firstNum][3]); // 候補の追加
                                    rastIdxList.Clear();    // 候補の削除
                                    rastIdxList.Add(edgIdx2List2[firstNum][2]);// 候補の追加
                                    rastIdxList.Add(edgIdx2List2[firstNum][3]);// 候補の追加
                                }
                                else if ((edgIdx2List2[firstNum][2] == idxList[0] || edgIdx2List2[firstNum][2] == idxList[1]) && (edgIdx2List2[firstNum][3] == idxList[1] || edgIdx2List2[firstNum][3] == idxList[0]))
                                {
                                    Debug.Log("頂点の追加");
                                    secondNum = 1;
                                    pEnd = intersectPolyList2[firstNum][0];    // 交点の終点
                                    pEdge2 = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][0].x), Mathf.Abs(intersectEdgList2[firstNum][0].y));// 交点の終点の辺ベクトル                    
                                    idxList.Clear();    // 候補の削除
                                    idxList.Add(edgIdx2List2[firstNum][0]); // 候補の追加
                                    idxList.Add(edgIdx2List2[firstNum][1]); // 候補の追加
                                    rastIdxList.Clear();    // 候補の削除
                                    rastIdxList.Add(edgIdx2List2[firstNum][0]);// 候補の追加
                                    rastIdxList.Add(edgIdx2List2[firstNum][1]);// 候補の追加
                                }

                                // 頂点の追加
                                vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f * -cpNormalAbs.z));
                                vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f * cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f * cpNormalAbs.z));

                                // インデックスの割り当て
                                int idx0 = straddlePolyIdx[(firstNum * 3)];
                                int idx1 = straddlePolyIdx[(firstNum * 3) + 1];
                                int idx2 = straddlePolyIdx[(firstNum * 3) + 2];
                                int idx3 = vertices1.Count - 4; // 
                                int idx4 = vertices1.Count - 3; //  
                                int idx5 = vertices1.Count - 2; // 
                                int idx6 = vertices1.Count - 1; // 
                                int removeIdx = -1;

                                // 削除する三角形の検索
                                for (int a = 0; a < attachedMesh.triangles.Length; a += 3)
                                {
                                    if (!(attachedMesh.triangles[a] == idx0 && attachedMesh.triangles[a + 1] == idx1 && attachedMesh.triangles[a + 2] == idx2)) continue;
                                    removeIdx = a;
                                }

                                Debug.Log("secondNum" + secondNum);
                                Debug.Log("idx0" + idx0);
                                Debug.Log("idx1" + idx1);
                                Debug.Log("idx2" + idx2);
                                Debug.Log("edgIdx2List2[firstNum][0]" + edgIdx2List2[firstNum][0]);
                                Debug.Log("edgIdx2List2[firstNum][1]" + edgIdx2List2[firstNum][1]);
                                Debug.Log("edgIdx2List2[firstNum][2]" + edgIdx2List2[firstNum][2]);
                                Debug.Log("edgIdx2List2[firstNum][3]" + edgIdx2List2[firstNum][3]);

                                // インデックスの割り振り
                                if (secondNum == 0)
                                {
                                    // edgIdx2List2[firstNum][0]、edgIdx2List2[firstNum][1]が始点の交点

                                    // インデックスの削除
                                    triangles1.RemoveRange(removeIdx, 3);

                                    // 01-12インデックス
                                    if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx1)) &&
                                        ((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)))
                                    {
                                        Debug.Log("01-12インデックス");
                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx0);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx0);
                                        triangles1.Add(idx4);
                                    }
                                    // 01-02インデックス
                                    else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx1)) &&
                                             ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)))
                                    {
                                        Debug.Log("01-02インデックス");
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx0);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx1);
                                        triangles1.Add(idx2);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                    }
                                    // 02-12インデックス
                                    else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)) &&
                                             ((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)))
                                    {
                                        Debug.Log("02-12インデックス");
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx0);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx0);
                                        triangles1.Add(idx1);
                                    }
                                    // 02-01インデックス
                                    else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)) &&
                                             ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx1 || edgIdx2List2[firstNum][3] == idx0)))
                                    {
                                        Debug.Log("02-01インデックス");
                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx0);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx1);
                                        triangles1.Add(idx2);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);


                                    }
                                    // 12-01インデックス
                                    else if (((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)) &&
                                             ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx1 || edgIdx2List2[firstNum][3] == idx0)))
                                    {
                                        Debug.Log("12-01インデックス");
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx1);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx2);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx0);
                                    }
                                    // 12-20インデックス
                                    else if (((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)) &&
                                             ((edgIdx2List2[firstNum][2] == idx2 || edgIdx2List2[firstNum][2] == idx0) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx2)))
                                    {
                                        Debug.Log(" 12-02インデックス");
                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx2);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx1);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx0);
                                        triangles1.Add(idx1);
                                    }

                                }
                                else if (secondNum == 1)
                                {
                                    // インデックスの削除
                                    triangles1.RemoveRange(removeIdx, 3);

                                    // 01-12インデックス
                                    if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx1)) &&
                                             ((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)))
                                    {
                                        Debug.Log("01-12インデックス");
                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx0);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx0);
                                        triangles1.Add(idx4);
                                    }
                                    // 01-02インデックス
                                    else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx1)) &&
                                             ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)))
                                    {
                                        Debug.Log("01-02インデックス");
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx0);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx1);
                                        triangles1.Add(idx2);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                    }
                                    // 02-12インデックス
                                    else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)) &&
                                             ((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)))
                                    {
                                        Debug.Log("02-12インデックス");
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx0);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx0);
                                        triangles1.Add(idx1);
                                    }
                                    // 02-01インデックス
                                    else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)) &&
                                             ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx1 || edgIdx2List2[firstNum][1] == idx0)))
                                    {
                                        Debug.Log("02-01インデックス");
                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx0);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx1);
                                        triangles1.Add(idx2);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);


                                    }
                                    // 12-01インデックス
                                    else if (((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)) &&
                                             ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx1 || edgIdx2List2[firstNum][1] == idx0)))
                                    {
                                        Debug.Log("12-01インデックス");
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx1);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx2);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx0);
                                    }
                                    // 12-20インデックス
                                    else if (((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)) &&
                                             ((edgIdx2List2[firstNum][0] == idx2 || edgIdx2List2[firstNum][0] == idx0) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx2)))
                                    {
                                        Debug.Log(" 12-02インデックス");
                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx2);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx1);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx6);
                                        triangles1.Add(idx0);
                                        triangles1.Add(idx1);
                                    }


                                }

                                // 候補から削除
                                idxCnt.RemoveRange(k, 3);
                                intersectPolyList2.RemoveAt(firstNum);
                                intersectEdgList2.RemoveAt(firstNum);
                                edgIdx2List2.RemoveAt(firstNum);

                                // ここまで来たら終了
                                end = true;
                                break;
                            }
                            if (end) break;
                        }

                        if (end) break;
                    }

                    // カウント++
                    count++;
                }
            }

            // 最後に4分割する処理
            {
                Debug.Log("4分割する処理");
                //Debug.Log("edgIdx2List[0][0]:"+ edgIdx2List[0][0]);
                //Debug.Log("edgIdx2List[0][0]:"+ edgIdx2List[0][1]);
                //Debug.Log("inerPolyIdx.Count:" + inerPolyIdx.Count);
                //Debug.Log("inerPolyIdx[0]:" + inerPolyIdx[0]);
                //Debug.Log("inerPolyIdx[1]:" + inerPolyIdx[1]);
                //Debug.Log("inerPolyIdx[2]:" + inerPolyIdx[2]);

                // 対象のインデックスの削除
                for(int a = 0;a < triangles1.Count;a+=3)
                {
                    // 一致しなかったらスルー
                    if (!(triangles1[a] == inerPolyIdx[0] && triangles1[a +1] == inerPolyIdx[1]&& triangles1[a +2] == inerPolyIdx[2])) continue;

                    triangles1.RemoveRange(a, 3);
                    break;
                }

                // 頂点の追加
                vertices1.Add(cutPoint[cutPoint.Count-1] -transform.position);
                vertices1.Add(cutPoint[cutPoint.Count-1] - transform.position);
                
                // インデックスの割り当て
                int idx0 = inerPolyIdx[0];
                int idx1 = inerPolyIdx[1];
                int idx2 = inerPolyIdx[2];
                int idx3 = vertices1.Count - 4; // 
                int idx4 = vertices1.Count - 3; //  
                int idx5 = vertices1.Count - 2; // 
                int idx6 = vertices1.Count - 1; // 
                
                // 交点の始点がどの辺にあるか
                // 辺01の上
                if ((edgIdx2List[0][0] == inerPolyIdx[0] || edgIdx2List[0][0] == inerPolyIdx[1])&&
                   (edgIdx2List[0][1] == inerPolyIdx[1] || edgIdx2List[0][1] == inerPolyIdx[0] ))
                {
                    Debug.Log("辺01の上");
                    triangles1.Add(idx5);
                    triangles1.Add(idx2);
                    triangles1.Add(idx0);

                    triangles1.Add(idx5);
                    triangles1.Add(idx0);
                    triangles1.Add(idx4);

                    triangles1.Add(idx5);
                    triangles1.Add(idx3);
                    triangles1.Add(idx1);

                    triangles1.Add(idx5);
                    triangles1.Add(idx1);
                    triangles1.Add(idx2);

                    // 出来た三角形インデックスの保存
                    idxMemory.Clear();
                    idxMemory.Add(idx5);
                    idxMemory.Add(idx2);
                    idxMemory.Add(idx0);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx0);
                    idxMemory.Add(idx4);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx3);
                    idxMemory.Add(idx1);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx1);
                    idxMemory.Add(idx2);
                }
                // 辺12の上
                else if ((edgIdx2List[0][0] == inerPolyIdx[1] || edgIdx2List[0][0] == inerPolyIdx[2]) &&
                   (edgIdx2List[0][1] == inerPolyIdx[2] || edgIdx2List[0][1] == inerPolyIdx[1]))
                {
                    Debug.Log("辺12の上");
                    triangles1.Add(idx5);
                    triangles1.Add(idx0);
                    triangles1.Add(idx1);

                    triangles1.Add(idx5);
                    triangles1.Add(idx1);
                    triangles1.Add(idx4);

                    triangles1.Add(idx5);
                    triangles1.Add(idx3);
                    triangles1.Add(idx2);

                    triangles1.Add(idx5);
                    triangles1.Add(idx2);
                    triangles1.Add(idx0);

                    // 出来た三角形インデックスの保存
                    idxMemory.Clear();
                    idxMemory.Add(idx5);
                    idxMemory.Add(idx0);
                    idxMemory.Add(idx1);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx1);
                    idxMemory.Add(idx4);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx3);
                    idxMemory.Add(idx2);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx2);
                    idxMemory.Add(idx0);
                }
                // 辺20の上
                else if ((edgIdx2List[0][0] == inerPolyIdx[2] || edgIdx2List[0][0] == inerPolyIdx[0]) &&
                   (edgIdx2List[0][1] == inerPolyIdx[0] || edgIdx2List[0][1] == inerPolyIdx[2]))
                {
                    Debug.Log("辺20の上");
                    triangles1.Add(idx5);
                    triangles1.Add(idx1);
                    triangles1.Add(idx2);

                    triangles1.Add(idx5);
                    triangles1.Add(idx2);
                    triangles1.Add(idx4);

                    triangles1.Add(idx5);
                    triangles1.Add(idx3);
                    triangles1.Add(idx0);

                    triangles1.Add(idx5);
                    triangles1.Add(idx0);
                    triangles1.Add(idx1);

                    // 出来た三角形インデックスの保存
                    idxMemory.Clear();
                    idxMemory.Add(idx5);
                    idxMemory.Add(idx1);
                    idxMemory.Add(idx2);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx2);
                    idxMemory.Add(idx4);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx3);
                    idxMemory.Add(idx0);

                    idxMemory.Add(idx5);
                    idxMemory.Add(idx0);
                    idxMemory.Add(idx1);
                }
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

        var mesh = new Mesh();
        mesh.vertices = vertices1.ToArray();
        mesh.triangles = triangles1.ToArray();
        mesh.normals = normals1.ToArray();

        attachedMesh = mesh;
    }

    // 途中のカットポイントでの分割
    public bool DiviosionMeshMiddle(List<Vector3> cutPoint)
    {
        Debug.Log("============ 途中の処理 ============");
        if (cutPoint.Count < 3) return false;

        // メッシュのアタッチ
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;

        // 変数
        Vector3 p0, p1, p2;    // メッシュのポリゴンの頂点
        var uvs1 = new List<Vector2>(); // テクスチャ
        var vertices1 = new List<Vector3>();   // 頂点
        var triangles1 = new List<int>();       // 三角形インデックス
        var normals1 = new List<Vector3>();     // 法線
                                                //var normals2 = new List<Vector3>();
        Vector3 edge = new Vector3();
        Vector3 edge1 = new Vector3();
        Vector3 edge2 = new Vector3();
        Vector3 edge3 = new Vector3();


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

            // 切る方向に対して点を移動するめの処理
            edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 3];
            edge2 = cutPoint[cutPoint.Count - 1] - cutPoint[cutPoint.Count - 2];
            edge3 = edge1 + edge2;

            // カットポイントが一直線だったら
            // 垂直に点を広げる
            if (edge3 == Vector3.zero)
            {
                edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
                edge2 = (cutPoint[cutPoint.Count - 1] + Vector3.up) - cutPoint[cutPoint.Count - 1];
                edge = Vector3.Cross(edge2, edge1);
            }
            else
            {
                edge = Vector3.Cross(edge3, Vector3.up);

            }

            // 頂点に格納
            vertices1[i] = vertices1[i] + edge.normalized * 0.04f;
            vertices1[i + 1] = vertices1[i + 1] - edge.normalized * 0.04f;
        }

        // カットする処理(全体)
        {
            // 変数宣言              
            var straddlePolyIdx = new List<int>();  // またいだポリゴン番号リスト
            var crossPolyIdx = new List<int>();     // 交差ポリゴン番号リスト
            var inerPolyIdx = new List<int>();      // カットポイントが中に入っているポリゴン番号
            var intersectPolyList = new List<List<Vector2>>();  // ポリゴンごとにある交差している点のリスト
            var intersectPolyList2 = new List<List<Vector2>>();  // ポリゴンごとにある交差している点のリスト2
            var intersectEdgList = new List<List<Vector2>>();   // ポリゴンごとにある交差している辺のリスト
            var intersectEdgList2 = new List<List<Vector2>>();   // ポリゴンごとにある交差している辺のリスト
            var intersectionList = new List<Vector2>();         // 交点のリスト
            var cp_s = new Vector2(cutPoint[cutPoint.Count - 2].x, cutPoint[cutPoint.Count - 2].z);    // カットポイントの終点の1個前
            var cp_v = new Vector2(cutPoint[cutPoint.Count - 1].x, cutPoint[cutPoint.Count - 1].z);    // カットポイントの終点
            var cpEdg = cp_v - cp_s;    // カットポイントの終点とカットポイントの終点の1個前をつないだ辺
            var checkCp = cp_s + cpEdg * 0.01f; // カットポイントの終点の1個前からカットポイントの終点の方向にちょっと伸ばした点
            var edgIdx2List = new List<List<int>>();   // 辺のインデックスのリストのリスト   
            var edgIdx2List2 = new List<List<int>>();  // 辺のインデックスのリストのリスト2   

            // またいでるポリゴンと侵入しているポリゴンが何個あるか探す
            for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
            {
                // 変数宣言             
                int interPointCnt = 0; // 交差した点の数
                var intersection = new List<Vector2>(); // 交点のリスト
                var edgList = new List<Vector2>(); //辺のリスト
                var edgIdxList = new List<int>();   // 辺のインデックスのリスト   

                // ポリゴンの辺の数だけループ
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

                    // カットポイントの始点の補正
                    cpVtx_s += cpEdge * 0.01f;

                    // カットポイントの辺の補正
                    cpEdge = cpVtx_v - cpVtx_s; // 辺

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
                        // 交わってないときスルー
                        continue;
                    }
                    else
                    {
                        // 交わってる時交点カウント++                               
                        interPointCnt++;    // 交点カウント    
                        intersection.Add(p);    // 交点の保存
                        intersectionList.Add(p);// 交点の保存
                        edgList.Add(polyEdge);
                        edgIdxList.Add(attachedMesh.triangles[j + k]);
                        edgIdxList.Add(attachedMesh.triangles[j + (k + 1) % 3]);

                    }
                }

                // ポリゴン番号を保存
                if (interPointCnt == 2)// 交点カウント2個(ポリゴンをまたいでる時)
                {
                    Debug.Log("2個あるで");
                    //Debug.Log("ポリゴン番号は" + attachedMesh.triangles[j] + "," + attachedMesh.triangles[j + 1] + "," + attachedMesh.triangles[j + 2]);

                    straddlePolyIdx.Add(attachedMesh.triangles[j]);
                    straddlePolyIdx.Add(attachedMesh.triangles[j + 1]);
                    straddlePolyIdx.Add(attachedMesh.triangles[j + 2]);
                    crossPolyIdx.Add(j);
                    crossPolyIdx.Add(j + 1);
                    crossPolyIdx.Add(j + 2);
                    intersectPolyList2.Add(intersection);
                    intersectEdgList2.Add(edgList);
                    edgIdx2List2.Add(edgIdxList);
                    //Debug.Log("straddlePolyIdx.Count" + straddlePolyIdx.Count);
                }
                else if (interPointCnt == 1)// 交点カウント1個(カットポイントの終点がポリゴンの中にあるとき)
                {
                    Debug.Log("1個あるよ");
                    inerPolyIdx.Add(attachedMesh.triangles[j]);
                    inerPolyIdx.Add(attachedMesh.triangles[j + 1]);
                    inerPolyIdx.Add(attachedMesh.triangles[j + 2]);
                    crossPolyIdx.Add(j);
                    crossPolyIdx.Add(j + 1);
                    crossPolyIdx.Add(j + 2);
                    edgIdx2List.Add(edgIdxList);
                    intersectPolyList.Add(intersection);
                    intersectEdgList.Add(edgList);
                }
                else
                {
                     //Debug.Log("3個あるで");
                    // Debug.Log("ポリゴン番号は" + attachedMesh.triangles[j] + "," + attachedMesh.triangles[j + 1] + "," + attachedMesh.triangles[j + 2]);

                }
            }

            //--- カットポイントとの交点の数で分岐 ---
            // 交点が1個の時(ポリゴンの中に2個交点があるポリゴンが1個もないとき)
            if (straddlePolyIdx.Count == 0 && inerPolyIdx.Count > 0)
            {
                Debug.Log("=============中間:交点が1個=============");

                //--- 変数宣言 ---
                int firstNum = 0;
                int secondNum = 0;
                Vector2 cpS = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z);
                Vector2 cpV = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z);
                Vector2 p = cpV;
                var idxList = new List<int>();  // 1個前に分割したインデックスのリスト
                var rastIdxList = new List<int>();  // 2個に分割する処理をした最後のインデックス保存用のリスト             

                // 2分割する処理
                {
                    //Debug.Log("2分割する処理");
                    //Debug.Log("inerPolyIdx.Count:" + inerPolyIdx.Count);
                    //Debug.Log("intersectPolyList.Count:" + intersectPolyList.Count);
                    //Debug.Log("intersectEdgList.Count:" + intersectEdgList.Count);


                    //// 記憶された三角形インデックスの数だけループ
                    //for (int a = 0; a < idxMemory.Count; a += 3)
                    //{                       
                    //    // 分割対象のポリゴンの数だけループ
                    //    for (int w = 0;w < inerPolyIdx.Count;w += 3)
                    //    {                                                    
                    //        // 記憶されたインデックスと一致しなかったらスルー
                    //        if (!(inerPolyIdx[w] ==  idxMemory[a] && inerPolyIdx[w + 1] == idxMemory[a + 1] && inerPolyIdx[w + 2] == idxMemory[a + 2])) continue;
                    //        Debug.Log("intersectPolyList[w/3][0]:" + intersectPolyList[w / 3][0]);
                    //        Debug.Log("intersectEdgList[w/3][0]:" + intersectEdgList[w / 3][0]);

                    //        var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
                    //        var cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));
                    //        var pEnd = intersectPolyList[w / 3][0];    // 交点の終点
                    //        var pEdge = new Vector2(Mathf.Abs(intersectEdgList[w/3][0].x), Mathf.Abs(intersectEdgList[w/3][0].y));
                    //        var pEdge2 = new Vector2();     // 交点の終点の辺ベクトル

                    //        for (int c = 0; c < triangles1.Count; c += 3)
                    //        {

                    //            // 一致しなかったらスルー
                    //            if (!(triangles1[c] == inerPolyIdx[w] && triangles1[c + 1] == inerPolyIdx[w+1] && triangles1[c + 2] == inerPolyIdx[w+2])) continue;
                    //            Debug.Log("頂点の追加");
                    //            // 交点をもとに頂点を追加
                    //            vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f * -cpNormalAbs.z));
                    //            vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f * cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f * cpNormalAbs.z));

                    //            // インデックスの割り当て
                    //            int idx0 = attachedMesh.triangles[c];
                    //            int idx1 = attachedMesh.triangles[c + 1];
                    //            int idx2 = attachedMesh.triangles[c + 2];
                    //            int idx3 = vertices1.Count - 2; // 7
                    //            int idx4 = vertices1.Count - 1; // 使わない  
                    //            int idx5 = vertices1.Count - 3; // 6


                    //            if (a == 0)
                    //            {                                    
                    //                Debug.Log("a = 0");                                                                                                          

                    //                // インデックスの変更
                    //                triangles1[c + 3] = idx5;

                    //                // カットポイントのあるポリゴンのインデックスの削除&追加       
                    //                triangles1.RemoveRange(c, 3);
                    //                triangles1.Add(idx4);
                    //                triangles1.Add(idx2);
                    //                triangles1.Add(idx5);

                    //                triangles1.Add(idx3);
                    //                triangles1.Add(idx0);
                    //                triangles1.Add(idx1);

                    //                break;
                    //            }
                    //            if (a == 3)
                    //            {
                    //                Debug.Log("a = 3");

                    //                // カットポイントのあるポリゴンのインデックスの削除&追加
                    //                triangles1.RemoveRange(c, 3);
                    //                triangles1.Add(idx4);
                    //                triangles1.Add(idx2);
                    //                triangles1.Add(idx5);

                    //                triangles1.Add(idx3);
                    //                triangles1.Add(idx0);
                    //                triangles1.Add(idx1);

                    //                break;
                    //            }
                    //            if (a == 6)
                    //            {
                    //                Debug.Log("a = 6");

                    //                // インデックスの変更
                    //                triangles1[c - 6] = idx5;
                    //                triangles1[c - 3] = idx5;

                    //                // カットポイントのあるポリゴンのインデックスの削除&追加
                    //                triangles1.RemoveRange(c, 3);
                    //                triangles1.Add(idx4);
                    //                triangles1.Add(idx2);
                    //                triangles1.Add(idx5);

                    //                triangles1.Add(idx3);
                    //                triangles1.Add(idx0);
                    //                triangles1.Add(idx1);

                    //                // ここまで来たら三角形を二等分するのは終了
                    //                break;
                    //            }

                    //        }
                    //        break;
                    //    }

                    //}
                }

                // 2分割する処理(最初)
                {
                    Debug.Log("2分割する処理");

                    //--- 記憶された三角形インデクスをもとにインデックスを割り振る ---
                    // 記憶された三角形インデックスの数だけループ
                    for (int a = 0; a < idxMemory.Count; a += 3)
                    {
                        bool end = false;
                        // 分割対象のポリゴンの数だけループ
                        for (int w = 0; w < inerPolyIdx.Count; w += 3)
                        {
                            // 記憶されたインデックスと一致しなかったらスルー
                            if (!(inerPolyIdx[w] == idxMemory[a] && inerPolyIdx[w + 1] == idxMemory[a + 1] && inerPolyIdx[w + 2] == idxMemory[a + 2])) continue;
                            Debug.Log("intersectPolyList[w/3][0]:" + intersectPolyList[w / 3][0]);
                            Debug.Log("intersectEdgList[w/3][0]:" + intersectEdgList[w / 3][0]);

                            //--- 変数宣言 ---
                            var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);

                            var cpNormalAbs = new Vector3();
                            if(cpNormal.x == 0)
                            {
                                cpNormalAbs = new Vector3(0, 0, cpNormal.z / Mathf.Abs(cpNormal.z));

                            }
                            else if(cpNormal.z == 0)
                            {
                                cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, 0);

                            }
                            else
                            {
                                cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));

                            }
                            var pEnd = intersectPolyList[w / 3][0];    // 交点の終点
                            var pEdge = new Vector2(Mathf.Abs(intersectEdgList[w / 3][0].x), Mathf.Abs(intersectEdgList[w / 3][0].y));

                            // 候補に追加
                            idxList.Add(edgIdx2List[w / 3][0]);
                            idxList.Add(edgIdx2List[w / 3][1]);

                            // 今追加された候補と交点が二個あるポリゴンの辺と比較
                            for (int g = 0; g < straddlePolyIdx.Count; g += 3)
                            {
                                bool end2 = false;
                                // 辺の数だけループ
                                for (int f = 0; f < 3; f++)
                                {
                                    // 一致しなかったらスルー
                                    if (((straddlePolyIdx[g + f] == idxList[0] || straddlePolyIdx[g + f] == idxList[1]) && (straddlePolyIdx[g + (f + 1) % 3] == idxList[1] || straddlePolyIdx[g + (f + 1) % 3] == idxList[0]))) continue;

                                    Debug.Log("候補に追加");
                                    firstNum = g;
                                    end2 = true;
                                    break;
                                }
                                if (end2) break;
                            }

                            // メッシュのポリゴンの数だけループ
                            for (int c = 0; c < triangles1.Count; c += 3)
                            {
                                // 一致しなかったらスルー
                                if (!(triangles1[c] == inerPolyIdx[w] && triangles1[c + 1] == inerPolyIdx[w + 1] && triangles1[c + 2] == inerPolyIdx[w + 2])) continue;
                                Debug.Log("頂点の追加");
                                Debug.Log("pEnd"+ pEnd);
                                Debug.Log("pEdge" + pEdge);
                                Debug.Log("" + ( pEdge.normalized.y * 0.079f * -cpNormalAbs.z));
                                Debug.Log("cpNormal" + (cpNormal));
                                
                                // 交点をもとに頂点を追加
                                vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge.normalized.x * 0.079f * -cpNormalAbs.x, 0, pEdge.normalized.y * 0.079f * -cpNormalAbs.z));
                                vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge.normalized.x * 0.079f * cpNormalAbs.x, 0, pEdge.normalized.y * 0.079f * cpNormalAbs.z));

                                // インデックスの割り当て
                                int idx0 = attachedMesh.triangles[c];
                                int idx1 = attachedMesh.triangles[c + 1];
                                int idx2 = attachedMesh.triangles[c + 2];
                                int idx3 = vertices1.Count - 4; // 7
                                int idx4 = vertices1.Count - 3; // 
                                int idx5 = vertices1.Count - 2; // 6
                                int idx6 = vertices1.Count - 1; // 6
                                int here = 0;   // 記憶されたインデックスの先頭を格納する変数

                                // 一致した記憶された先頭のインデックスが全体のどのインデックスにいるのか
                                for (int z = 0; z < triangles1.Count; z += 3)
                                {
                                    // 一致しなかったらスルー
                                    if (!(idxMemory[a] == triangles1[z] && idxMemory[a + 1] == triangles1[z + 1] && idxMemory[a + 2] == triangles1[z + 2])) continue;

                                    // 記憶されたインデックスの先頭を格納
                                    here = z - a;
                                }

                                Debug.Log("idxMemory.Count" + idxMemory.Count);
                                // 記憶されたインデックスの数によって分岐
                                // 記憶されたインデックスが12個(ポリゴンが4個)の時
                                if (idxMemory.Count == 12)
                                {
                                    // インデックスの割り振り分岐
                                    if (a == 0)
                                    {
                                        Debug.Log("a = 0");

                                        // インデックスの変更
                                        triangles1[here + 3] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                    if (a == 3)
                                    {
                                        Debug.Log("a = 3");

                                        // カットポイントのあるポリゴンのインデックスの削除&追加
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                    if (a == 6)
                                    {
                                        Debug.Log("a = 6");

                                        // インデックスの変更
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;
                                        triangles1[here + 9] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        // ここまで来たら三角形を二等分するのは終了
                                        end = true;
                                        break;
                                    }
                                    if (a == 9)
                                    {
                                        Debug.Log("a = 9");
                                        // インデックスの変更
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        // ここまで来たら三角形を二等分するのは終了
                                        end = true;
                                        break;
                                    }
                                }
                                // 記憶されたインデックスが9個(ポリゴンが3個)の時
                                else if (idxMemory.Count == 9)
                                {
                                    // インデックスの割り振り分岐
                                    if (a == 0)
                                    {
                                        Debug.Log("a = 0");

                                        // インデックスの変更
                                        triangles1[here + 3] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                    if (a == 3)
                                    {
                                        Debug.Log("a = 3");

                                        // カットポイントのあるポリゴンのインデックスの削除&追加       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                    if (a == 6)
                                    {
                                        Debug.Log("a = 6");

                                        // インデックスの変更
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }

                                }


                            }

                            // 交点が一個のポリゴンの候補の中から今上で使ったポリゴンを削除
                            //inerPolyIdx.RemoveRange(w, 3);
                            //edgIdx2List.RemoveRange(w/3, 1);
                            //intersectPolyList.RemoveRange(w/3, 1);
                            //intersectEdgList.RemoveRange(w/3, 1);

                            break;
                        }

                    }
                }

                // 4分割する処理
                {
                    Debug.Log("4分割する処理");
                    Debug.Log("inerPolyIdx.Count:" + inerPolyIdx.Count);

                    //Debug.Log("edgIdx2List[0][0]:" + edgIdx2List[0][0]);
                    //Debug.Log("edgIdx2List[0][0]:" + edgIdx2List[0][1]);
                    //Debug.Log("inerPolyIdx.Count:" + inerPolyIdx.Count);
                    //Debug.Log("inerPolyIdx[0]:" + inerPolyIdx[0]);
                    //Debug.Log("inerPolyIdx[1]:" + inerPolyIdx[1]);
                    //Debug.Log("inerPolyIdx[2]:" + inerPolyIdx[2]);

                    // 対象のインデックスの削除
                    for (int a = 0; a < triangles1.Count; a += 3)
                    {
                        // 一致しなかったらスルー
                        if (!(triangles1[a] == inerPolyIdx[0] && triangles1[a + 1] == inerPolyIdx[1] && triangles1[a + 2] == inerPolyIdx[2])) continue;

                        triangles1.RemoveRange(a, 3);
                        break;
                    }

                    // 頂点の追加
                    vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);
                    vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);

                    // インデックスの割り当て
                    int idx0 = inerPolyIdx[0];
                    int idx1 = inerPolyIdx[1];
                    int idx2 = inerPolyIdx[2];
                    int idx3 = vertices1.Count - 4; // 
                    int idx4 = vertices1.Count - 3; //  
                    int idx5 = vertices1.Count - 2; // 
                    int idx6 = vertices1.Count - 1; // 

                    // 交点の始点がどの辺にあるか
                    // 辺01の上
                    if ((edgIdx2List[0][0] == inerPolyIdx[0] || edgIdx2List[0][0] == inerPolyIdx[1]) &&
                       (edgIdx2List[0][1] == inerPolyIdx[1] || edgIdx2List[0][1] == inerPolyIdx[0]))
                    {
                        Debug.Log("辺01の上");
                        triangles1.Add(idx5);
                        triangles1.Add(idx2);
                        triangles1.Add(idx0);

                        triangles1.Add(idx5);
                        triangles1.Add(idx0);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx1);

                        triangles1.Add(idx5);
                        triangles1.Add(idx1);
                        triangles1.Add(idx2);

                        // 出来た三角形インデックスの保存
                        idxMemory.Clear();
                        idxMemory.Add(idx5);
                        idxMemory.Add(idx2);
                        idxMemory.Add(idx0);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx0);
                        idxMemory.Add(idx4);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx3);
                        idxMemory.Add(idx1);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx1);
                        idxMemory.Add(idx2);
                    }
                    // 辺12の上
                    else if ((edgIdx2List[0][0] == inerPolyIdx[1] || edgIdx2List[0][0] == inerPolyIdx[2]) &&
                       (edgIdx2List[0][1] == inerPolyIdx[2] || edgIdx2List[0][1] == inerPolyIdx[1]))
                    {
                        Debug.Log("辺12の上");
                        triangles1.Add(idx5);
                        triangles1.Add(idx0);
                        triangles1.Add(idx1);

                        triangles1.Add(idx5);
                        triangles1.Add(idx1);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx2);

                        triangles1.Add(idx5);
                        triangles1.Add(idx2);
                        triangles1.Add(idx0);

                        // 出来た三角形インデックスの保存
                        idxMemory.Clear();
                        idxMemory.Add(idx5);
                        idxMemory.Add(idx0);
                        idxMemory.Add(idx1);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx1);
                        idxMemory.Add(idx4);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx3);
                        idxMemory.Add(idx2);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx2);
                        idxMemory.Add(idx0);
                    }
                    // 辺20の上
                    else if ((edgIdx2List[0][0] == inerPolyIdx[2] || edgIdx2List[0][0] == inerPolyIdx[0]) &&
                       (edgIdx2List[0][1] == inerPolyIdx[0] || edgIdx2List[0][1] == inerPolyIdx[2]))
                    {
                        Debug.Log("辺20の上");
                        triangles1.Add(idx5);
                        triangles1.Add(idx1);
                        triangles1.Add(idx2);

                        triangles1.Add(idx5);
                        triangles1.Add(idx2);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx0);

                        triangles1.Add(idx5);
                        triangles1.Add(idx0);
                        triangles1.Add(idx1);

                        // 出来た三角形インデックスの保存
                        idxMemory.Clear();
                        idxMemory.Add(idx5);
                        idxMemory.Add(idx1);
                        idxMemory.Add(idx2);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx2);
                        idxMemory.Add(idx4);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx3);
                        idxMemory.Add(idx0);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx0);
                        idxMemory.Add(idx1);
                    }

                }
            }
            // 交点が2個以上の時
            else if(straddlePolyIdx.Count > 0)
            {
                Debug.Log("=============中間:交点が2個以上=============");
                //Debug.Log("straddlePolyIdx.Count"+straddlePolyIdx.Count);
                //--- 変数宣言 ---
                int firstNum = 0;
                int secondNum = 0;
                Vector2 cpS = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z);
                Vector2 cpV = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z);
                Vector2 p = cpV;
                var idxList = new List<int>();  // 1個前に分割したインデックスのリスト
                var rastIdxList = new List<int>();  // 2個に分割する処理をした最後のインデックス保存用のリスト             

                // 2分割する処理(最初)
                {
                    Debug.Log("2分割する処理(最初)");
                  
                    //--- 記憶された三角形インデクスをもとにインデックスを割り振る ---
                    // 記憶された三角形インデックスの数だけループ
                    for (int a = 0; a < idxMemory.Count; a += 3)
                    {
                        bool end = false;
                        // 分割対象のポリゴンの数だけループ
                        for (int w = 0; w < inerPolyIdx.Count; w += 3)
                        {
                            // 記憶されたインデックスと一致しなかったらスルー
                            if (!(inerPolyIdx[w] == idxMemory[a] && inerPolyIdx[w + 1] == idxMemory[a + 1] && inerPolyIdx[w + 2] == idxMemory[a + 2])) continue;
                            //Debug.Log("intersectPolyList[w/3][0]:" + intersectPolyList[w / 3][0]);
                            //Debug.Log("intersectEdgList[w/3][0]:" + intersectEdgList[w / 3][0]);

                            //--- 変数宣言 ---
                            var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
                            var cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));
                            var pEnd = intersectPolyList[w / 3][0];    // 交点の終点
                            var pEdge = new Vector2(Mathf.Abs(intersectEdgList[w / 3][0].x), Mathf.Abs(intersectEdgList[w / 3][0].y));
                           
                            // 候補に追加
                            idxList.Add(edgIdx2List[w/3][0]);
                            idxList.Add(edgIdx2List[w/3][1]);

                            // 今追加された候補と交点が二個あるポリゴンの辺と比較
                            for(int g = 0;g < straddlePolyIdx.Count;g += 3)
                            {
                                bool end2 = false;
                                // 辺の数だけループ
                                for(int f = 0;f < 3;f++)
                                {
                                    // 一致しなかったらスルー
                                    if (((straddlePolyIdx[g + f] == idxList[0] || straddlePolyIdx[g + f] == idxList[1]) && (straddlePolyIdx[g + (f + 1)%3] == idxList[1] || straddlePolyIdx[g + (f + 1) % 3] == idxList[0]))) continue;

                                    Debug.Log("候補に追加");
                                    firstNum = g;
                                    end2 = true;
                                    break;
                                }
                                if (end2) break;
                            }
                          
                            // メッシュのポリゴンの数だけループ
                            for (int c = 0; c < triangles1.Count; c += 3)
                            {
                                // 一致しなかったらスルー
                                if (!(triangles1[c] == inerPolyIdx[w] && triangles1[c + 1] == inerPolyIdx[w + 1] && triangles1[c + 2] == inerPolyIdx[w + 2])) continue;
                                Debug.Log("頂点の追加");
                                // 交点をもとに頂点を追加
                                vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * -cpNormalAbs.z));
                                vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge.normalized.x * 0.04f *  cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f *  cpNormalAbs.z));

                                // インデックスの割り当て
                                int idx0 = attachedMesh.triangles[c];
                                int idx1 = attachedMesh.triangles[c + 1];
                                int idx2 = attachedMesh.triangles[c + 2];
                                int idx3 = vertices1.Count - 4; // 7
                                int idx4 = vertices1.Count - 3; // 
                                int idx5 = vertices1.Count - 2; // 6
                                int idx6 = vertices1.Count - 1; // 6
                                int here = 0;   // 記憶されたインデックスの先頭を格納する変数

                                // 一致した記憶された先頭のインデックスが全体のどのインデックスにいるのか
                                for(int z = 0;z < triangles1.Count;z+=3)
                                {
                                    // 一致しなかったらスルー
                                    if (!(idxMemory[a] == triangles1[z] && idxMemory[a + 1] == triangles1[z + 1] && idxMemory[a + 2] == triangles1[z + 2])) continue;

                                    // 記憶されたインデックスの先頭を格納
                                    here = z - a;
                                }

                                //Debug.Log("idxMemory.Count" + idxMemory.Count);
                                // 記憶されたインデックスの数によって分岐
                                // 記憶されたインデックスが12個(ポリゴンが4個)の時
                                if (idxMemory.Count == 12)   
                                {                                    
                                    // インデックスの割り振り分岐
                                    if (a == 0)
                                    {
                                        Debug.Log("a = 0");

                                        // インデックスの変更
                                        triangles1[here + 3] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                    if (a == 3)
                                    {
                                        Debug.Log("a = 3");

                                        // カットポイントのあるポリゴンのインデックスの削除&追加
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                    if (a == 6)
                                    {
                                        Debug.Log("a = 6");

                                        // インデックスの変更
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;
                                        triangles1[here + 9] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        // ここまで来たら三角形を二等分するのは終了
                                        end = true;
                                        break;
                                    }
                                    if (a == 9)
                                    {
                                        Debug.Log("a = 9");
                                        // インデックスの変更
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;
                                        
                                        // カットポイントのあるポリゴンのインデックスの削除&追加
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        // ここまで来たら三角形を二等分するのは終了
                                        end = true;
                                        break;
                                    }
                                }
                                // 記憶されたインデックスが9個(ポリゴンが3個)の時
                                else if (idxMemory.Count == 9)
                                {
                                    // インデックスの割り振り分岐
                                    if (a == 0)
                                    {
                                        Debug.Log("a = 0");

                                        // インデックスの変更
                                        triangles1[here + 3] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                    if (a == 3)
                                    {
                                        Debug.Log("a = 3");
                                       
                                        // カットポイントのあるポリゴンのインデックスの削除&追加       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                    if (a == 6)
                                    {
                                        Debug.Log("a = 6");

                                        // インデックスの変更
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end = true;
                                        break;
                                    }
                                   
                                }

                               
                            }

                            // 交点が一個のポリゴンの候補の中から今上で使ったポリゴンを削除
                            //inerPolyIdx.RemoveRange(w, 3);
                            //edgIdx2List.RemoveRange(w/3, 1);
                            //intersectPolyList.RemoveRange(w/3, 1);
                            //intersectEdgList.RemoveRange(w/3, 1);
                            if(end)break;
                        }
                        if (end) break;
                    }
                }

                // 記憶されたインデックスと一致しなかったら
                if(idxList.Count == 0)
                {
                    Debug.Log("2分割する処理(最初)が行われなかったら");
                    var point = intersectPolyList2[0][0];
                    int first = 0;
                    int second = 0;
                    for(int h = 0;h < straddlePolyIdx.Count;h+=3)
                    {
                        for(int v = 0;v < 2;v++)
                        {
                            if (Vector2.Distance(new Vector2(cutPoint[cutPoint.Count - 1].x, cutPoint[cutPoint.Count - 1].z), point) > Vector2.Distance(new Vector2(cutPoint[cutPoint.Count - 1].x, cutPoint[cutPoint.Count - 1].z), intersectPolyList2[h/3][v])) continue;
                            point = intersectPolyList2[h/3][v];
                            first = h;
                            second = v;
                        }
                    }

                    // 始点がどっちにあるかで分岐
                    if(second == 0)
                    {
                       
                        // 候補に追加
                        idxList.Add(edgIdx2List2[first/3][2]);
                        idxList.Add(edgIdx2List2[first/3][3]);

                    }
                    else if(second == 1)
                    {
                        // 候補に追加
                        idxList.Add(edgIdx2List2[first / 3][0]);
                        idxList.Add(edgIdx2List2[first / 3][1]);

                    }


                }


                // 2分割する処理(途中)
                {
                    Debug.Log("2分割する処理(途中)");

                    //--- 変数宣言 ---
                    int count = 0;
                    var idxCnt = straddlePolyIdx;   // またがってるポリゴンのリストを代入
                    //idxCnt.RemoveRange(firstNum * 3, 3); // 候補保存用
                                                         //Debug.Log("idxCnt.Count" + idxCnt.Count);
                    Debug.Log("候補の数" + idxCnt.Count);

                    //Debug.Log("idxCnt.Count" + idxCnt.Count);
                    
                  
                    // 交点が2個あるポリゴンの候補がなくなるかカウントが一定以上になるまでループ
                    while (count < 50 && idxCnt.Count > 0)
                    {
                        bool end = false;
                        Debug.Log("=============== ループ ==============");
                        for (int z = 0; z < intersectPolyList2.Count; z++)
                        {
                            for (int y = 0; y < intersectPolyList2[z].Count; y++)
                            {
                                Debug.Log("intersectPolyList2[z][y]  " + intersectPolyList2[z][y]);

                            }

                        }
                        // 候補の数だけループ
                        for (int k = 0; k < idxCnt.Count; k += 3)
                        {
                            // 辺の数だけループ
                            for (int h = 0; h < 3; h++)
                            {
                                //Debug.Log("idxCnt[k + h];" + idxCnt[k + h]);
                                //Debug.Log("idxCnt[k + ((h + 1) % 3)];" + idxCnt[k + ((h + 1) % 3)]);
                                //Debug.Log("idxList[0];" + idxList[0]);
                                //Debug.Log("idxList[1];" + idxList[1]);
                                ////// 候補と一致しなかったらスルー、一致したら分割対象のインデックスが分かる
                                if (!((idxCnt[k + h] == idxList[0] || idxCnt[k + h] == idxList[1]) && (idxCnt[k + ((h + 1) % 3)] == idxList[0] || straddlePolyIdx[k + ((h + 1) % 3)] == idxList[1]))) continue;

                                // 保存された候補リストから今回使ったインデックスを削除
                                for (int g = 0; g < straddlePolyIdx.Count; g += 3)
                                {
                                    // 候補と一致しなかったらスルー
                                    if (!(idxCnt[k] == straddlePolyIdx[g] && idxCnt[k + 1] == straddlePolyIdx[g + 1] && idxCnt[k + 2] == straddlePolyIdx[g + 2])) continue;

                                    //Debug.Log("====== 候補のリスト ======");
                                    //for (int z = 0; z < idxCnt.Count; z += 3)
                                    //{
                                    //    Debug.Log("idxCnt[z]  " + idxCnt[z]);
                                    //    Debug.Log("idxCnt[z+1]" + idxCnt[z + 1]);
                                    //    Debug.Log("idxCnt[z+2]" + idxCnt[z + 2]);
                                    //}
                                    //Debug.Log("====== 候補のリスト ======");
                                    //Debug.Log("straddlePolyIdx.Count  " + straddlePolyIdx.Count);
                                    //for (int z = 0; z < straddlePolyIdx.Count; z += 3)
                                    //{
                                    //    Debug.Log("straddlePolyIdx[z]  " + straddlePolyIdx[z]);
                                    //    Debug.Log("straddlePolyIdx[z+1]" + straddlePolyIdx[z + 1]);
                                    //    Debug.Log("straddlePolyIdx[z+2]" + straddlePolyIdx[z + 2]);
                                    //}
                                    //Debug.Log("==========================");

                                    // ポリゴンのインデックスの最初の番号
                                    firstNum = g / 3;
                                    //Debug.Log("g:" + g);
                                    //Debug.Log("firstNum:" + (g / 3));

                                    // 変数宣言
                                    var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
                                    var cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));
                                    var pEnd = new Vector2() ;    // 交点の終点
                                    var pEdge = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][secondNum].x), Mathf.Abs(intersectEdgList2[firstNum][secondNum].y));
                                    var pEdge2 = new Vector2();     // 交点の終点の辺ベクトル

                                 
                                    //Debug.Log("====== 交点のリスト ======");
                                    //Debug.Log("edgIdx2List2[firstNum][0]:" + edgIdx2List2[firstNum][0]);
                                    //Debug.Log("edgIdx2List2[firstNum][1]:" + edgIdx2List2[firstNum][1]);
                                    //Debug.Log("edgIdx2List2[firstNum][2]:" + edgIdx2List2[firstNum][2]);
                                    //Debug.Log("edgIdx2List2[firstNum][3]:" + edgIdx2List2[firstNum][3]);
                                    //Debug.Log("intersectPolyList2[firstNum][0]:" + intersectPolyList2[firstNum][0]);
                                    //Debug.Log("intersectPolyList2[firstNum][1]:" + intersectPolyList2[firstNum][1]);
                                    //Debug.Log("intersectPolyList2.Count:" + intersectPolyList2.Count);
                                    //Debug.Log("==========================");
                                    // どっちが交点の始点か調べる
                                    if ((edgIdx2List2[firstNum][0] == idxList[0] || edgIdx2List2[firstNum][0] == idxList[1]) && (edgIdx2List2[firstNum][1] == idxList[1] || edgIdx2List2[firstNum][1] == idxList[0]))
                                    {
                                        Debug.Log("頂点の追加:01が始点");
                                       
                                        secondNum = 0;
                                        pEnd = intersectPolyList2[firstNum][1];    // 交点の終点
                                        pEdge2 = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][1].x), Mathf.Abs(intersectEdgList2[firstNum][1].y));// 交点の終点の辺ベクトル                                                      
                                        idxList.Clear();    // 候補の削除
                                        idxList.Add(edgIdx2List2[firstNum][2]); // 候補の追加
                                        idxList.Add(edgIdx2List2[firstNum][3]); // 候補の追加
                                        rastIdxList.Clear();    // 候補の削除
                                        rastIdxList.Add(edgIdx2List2[firstNum][2]);// 候補の追加
                                        rastIdxList.Add(edgIdx2List2[firstNum][3]);// 候補の追加
                                    }
                                    else if ((edgIdx2List2[firstNum][2] == idxList[0] || edgIdx2List2[firstNum][2] == idxList[1]) && (edgIdx2List2[firstNum][3] == idxList[1] || edgIdx2List2[firstNum][3] == idxList[0]))
                                    {
                                        Debug.Log("頂点の追加:23が始点");
                                       
                                        secondNum = 1;
                                        pEnd = intersectPolyList2[firstNum][1];    // 交点の終点
                                        pEdge2 = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][0].x), Mathf.Abs(intersectEdgList2[firstNum][0].y));// 交点の終点の辺ベクトル                    
                                        idxList.Clear();    // 候補の削除
                                        idxList.Add(edgIdx2List2[firstNum][0]); // 候補の追加
                                        idxList.Add(edgIdx2List2[firstNum][1]); // 候補の追加
                                        rastIdxList.Clear();    // 候補の削除
                                        rastIdxList.Add(edgIdx2List2[firstNum][0]);// 候補の追加
                                        rastIdxList.Add(edgIdx2List2[firstNum][1]);// 候補の追加
                                    }

                                    Debug.Log("secondNum:" + secondNum);
                                   


                                    // 頂点の追加
                                    vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f * -cpNormalAbs.z));
                                    vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f * cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f * cpNormalAbs.z));

                                    // インデックスの割り当て
                                    int idx0 = straddlePolyIdx[(firstNum * 3)];
                                    int idx1 = straddlePolyIdx[(firstNum * 3) + 1];
                                    int idx2 = straddlePolyIdx[(firstNum * 3) + 2];
                                    int idx3 = vertices1.Count - 4; // 
                                    int idx4 = vertices1.Count - 3; //  
                                    int idx5 = vertices1.Count - 2; // 
                                    int idx6 = vertices1.Count - 1; // 
                                    int removeIdx = -1;

                                    // 削除する三角形の検索
                                    Debug.Log("削除する三角形の探索");
                                    for (int a = 0; a < triangles1.Count; a += 3)
                                    {
                                        if (!(triangles1[a] == idx0 && triangles1[a + 1] == idx1 && triangles1[a + 2] == idx2)) continue;
                                        Debug.Log("削除された三角形"+ idx0 +"," + idx1 + "," + idx2);
                                       
                                        removeIdx = a;
                                    }

                                    // インデックスの割り振り
                                    if (secondNum == 0)
                                    {
                                        // edgIdx2List2[firstNum][0]、edgIdx2List2[firstNum][1]が始点の交点

                                        // インデックスの削除
                                        triangles1.RemoveRange(removeIdx, 3);

                                        // 01-12インデックス
                                        if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx1)) &&
                                            ((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)))
                                        {
                                            Debug.Log("01-12インデックス");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx1);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx4);
                                        }
                                        // 01-02インデックス
                                        else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)))
                                        {
                                            Debug.Log("01-02インデックス");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx1);

                                        }
                                        // 02-12インデックス
                                        else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)))
                                        {
                                            Debug.Log("02-12インデックス");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx1);
                                        }
                                        // 02-01インデックス
                                        else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx1 || edgIdx2List2[firstNum][3] == idx0)))
                                        {
                                            Debug.Log("02-01インデックス");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx4);


                                        }
                                        // 12-01インデックス
                                        else if (((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx1 || edgIdx2List2[firstNum][3] == idx0)))
                                        {
                                            Debug.Log("12-01インデックス");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx0);
                                        }
                                        // 12-20インデックス
                                        else if (((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx2 || edgIdx2List2[firstNum][2] == idx0) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx2)))
                                        {
                                            Debug.Log(" 12-02インデックス");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx1);
                                        }

                                    }
                                    else if (secondNum == 1)
                                    {
                                        // インデックスの削除
                                        triangles1.RemoveRange(removeIdx, 3);

                                        //Debug.Log("====== 候補のリスト ======");
                                        //Debug.Log("straddlePolyIdx.Count  " + straddlePolyIdx.Count);
                                        //for (int z = 0; z < straddlePolyIdx.Count; z += 3)
                                        //{
                                        //    Debug.Log("straddlePolyIdx[z]  " + straddlePolyIdx[z]);
                                        //    Debug.Log("straddlePolyIdx[z+1]" + straddlePolyIdx[z + 1]);
                                        //    Debug.Log("straddlePolyIdx[z+2]" + straddlePolyIdx[z + 2]);
                                        //}
                                        //Debug.Log("==========================");
                                        //Debug.Log("====== 交点のリスト ======");
                                        //Debug.Log("idx0:" + idx0);
                                        //Debug.Log("idx1:" + idx1);
                                        //Debug.Log("idx2:" + idx2);
                                        //Debug.Log("edgIdx2List2[firstNum][0]:" + edgIdx2List2[firstNum][0]);
                                        //Debug.Log("edgIdx2List2[firstNum][1]:" + edgIdx2List2[firstNum][1]);
                                        //Debug.Log("edgIdx2List2[firstNum][2]:" + edgIdx2List2[firstNum][2]);
                                        //Debug.Log("edgIdx2List2[firstNum][3]:" + edgIdx2List2[firstNum][3]);
                                        // Debug.Log("==========================");

                                        // インデックスの並び替え
                                        //var numChange = new List<int>();
                                        //numChange.Add(edgIdx2List2[firstNum][0]);
                                        //numChange.Add(edgIdx2List2[firstNum][1]);
                                        //edgIdx2List2[firstNum].RemoveRange(0, 2);
                                        //edgIdx2List2[firstNum].Add(numChange[0]);
                                        //edgIdx2List2[firstNum].Add(numChange[1]);

                                        // 01-12インデックス
                                        if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)))
                                        {
                                            Debug.Log("01-12インデックス");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx1);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx4);
                                        }
                                        // 01-02インデックス
                                        else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)))
                                        {
                                            Debug.Log("01-02インデックス");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx1);

                                        }
                                        // 02-12インデックス
                                        else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)))
                                        {
                                            Debug.Log("02-12インデックス");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx1);
                                        }
                                        // 02-01インデックス
                                        else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx1 || edgIdx2List2[firstNum][1] == idx0)))
                                        {
                                            Debug.Log("02-01インデックス");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx4);


                                        }
                                        // 12-01インデックス
                                        else if (((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx1 || edgIdx2List2[firstNum][1] == idx0)))
                                        {
                                            Debug.Log("12-01インデックス");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx0);
                                        }
                                        // 12-20インデックス
                                        else if (((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx2 || edgIdx2List2[firstNum][0] == idx0) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx2)))
                                        {
                                            Debug.Log(" 12-02インデックス");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx1);
                                        }

                                        //numChange.RemoveRange(0, 2);
                                        //numChange.Add(edgIdx2List2[firstNum][0]);
                                        //numChange.Add(edgIdx2List2[firstNum][1]);
                                        //edgIdx2List2[firstNum].RemoveRange(0, 2);
                                        //edgIdx2List2[firstNum].Add(numChange[0]);
                                        //edgIdx2List2[firstNum].Add(numChange[1]);

                                    }

                                    // 候補から削除
                                    Debug.Log("候補から削除");
                                    idxCnt.RemoveRange(k, 3);
                                    intersectPolyList2.RemoveAt(firstNum);
                                    intersectEdgList2.RemoveAt(firstNum);
                                    edgIdx2List2.RemoveAt(firstNum);
                                    // ここまで来たら終了
                                    end = true;
                                    break;
                                }
                                if (end) break;
                            }

                            if (end) break;
                        }

                        // カウント++
                        count++;
                    }
                }

                // 4分割する処理
                {
                    Debug.Log("4分割する処理");
                    //Debug.Log("edgIdx2List[0][0]:" + edgIdx2List[0][0]);
                    //Debug.Log("edgIdx2List[0][0]:" + edgIdx2List[0][1]);
                    //Debug.Log("inerPolyIdx.Count:" + inerPolyIdx.Count);
                    //Debug.Log("inerPolyIdx[0]:" + inerPolyIdx[0]);
                    //Debug.Log("inerPolyIdx[1]:" + inerPolyIdx[1]);
                    //Debug.Log("inerPolyIdx[2]:" + inerPolyIdx[2]);

                    // 対象のインデックスの削除
                    for (int a = 0; a < triangles1.Count; a += 3)
                    {
                        // 一致しなかったらスルー
                        if (!(triangles1[a] == inerPolyIdx[0] && triangles1[a + 1] == inerPolyIdx[1] && triangles1[a + 2] == inerPolyIdx[2])) continue;

                        triangles1.RemoveRange(a, 3);
                        break;
                    }

                    // 頂点の追加
                    vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);
                    vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);

                    // インデックスの割り当て
                    int idx0 = inerPolyIdx[0];
                    int idx1 = inerPolyIdx[1];
                    int idx2 = inerPolyIdx[2];
                    int idx3 = vertices1.Count - 4; // 
                    int idx4 = vertices1.Count - 3; //  
                    int idx5 = vertices1.Count - 2; // 
                    int idx6 = vertices1.Count - 1; // 

                    // 交点の始点がどの辺にあるか
                    // 辺01の上
                    if ((edgIdx2List[0][0] == inerPolyIdx[0] || edgIdx2List[0][0] == inerPolyIdx[1]) &&
                       (edgIdx2List[0][1] == inerPolyIdx[1] || edgIdx2List[0][1] == inerPolyIdx[0]))
                    {
                        Debug.Log("辺01の上");
                        triangles1.Add(idx5);
                        triangles1.Add(idx2);
                        triangles1.Add(idx0);

                        triangles1.Add(idx5);
                        triangles1.Add(idx0);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx1);

                        triangles1.Add(idx5);
                        triangles1.Add(idx1);
                        triangles1.Add(idx2);

                        // 出来た三角形インデックスの保存
                        idxMemory.Clear();
                        idxMemory.Add(idx5);
                        idxMemory.Add(idx2);
                        idxMemory.Add(idx0);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx0);
                        idxMemory.Add(idx4);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx3);
                        idxMemory.Add(idx1);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx1);
                        idxMemory.Add(idx2);
                    }
                    // 辺12の上
                    else if ((edgIdx2List[0][0] == inerPolyIdx[1] || edgIdx2List[0][0] == inerPolyIdx[2]) &&
                       (edgIdx2List[0][1] == inerPolyIdx[2] || edgIdx2List[0][1] == inerPolyIdx[1]))
                    {
                        Debug.Log("辺12の上");
                        triangles1.Add(idx5);
                        triangles1.Add(idx0);
                        triangles1.Add(idx1);

                        triangles1.Add(idx5);
                        triangles1.Add(idx1);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx2);

                        triangles1.Add(idx5);
                        triangles1.Add(idx2);
                        triangles1.Add(idx0);

                        // 出来た三角形インデックスの保存
                        idxMemory.Clear();
                        idxMemory.Add(idx5);
                        idxMemory.Add(idx0);
                        idxMemory.Add(idx1);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx1);
                        idxMemory.Add(idx4);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx3);
                        idxMemory.Add(idx2);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx2);
                        idxMemory.Add(idx0);
                    }
                    // 辺20の上
                    else if ((edgIdx2List[0][0] == inerPolyIdx[2] || edgIdx2List[0][0] == inerPolyIdx[0]) &&
                       (edgIdx2List[0][1] == inerPolyIdx[0] || edgIdx2List[0][1] == inerPolyIdx[2]))
                    {
                        Debug.Log("辺20の上");
                        triangles1.Add(idx5);
                        triangles1.Add(idx1);
                        triangles1.Add(idx2);

                        triangles1.Add(idx5);
                        triangles1.Add(idx2);
                        triangles1.Add(idx4);

                        triangles1.Add(idx5);
                        triangles1.Add(idx3);
                        triangles1.Add(idx0);

                        triangles1.Add(idx5);
                        triangles1.Add(idx0);
                        triangles1.Add(idx1);

                        // 出来た三角形インデックスの保存
                        idxMemory.Clear();
                        idxMemory.Add(idx5);
                        idxMemory.Add(idx1);
                        idxMemory.Add(idx2);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx2);
                        idxMemory.Add(idx4);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx3);
                        idxMemory.Add(idx0);

                        idxMemory.Add(idx5);
                        idxMemory.Add(idx0);
                        idxMemory.Add(idx1);
                    }
                }
            }
            // 交点が1点もないとき
            else if(straddlePolyIdx.Count == 0 && inerPolyIdx.Count == 0)
            {
                Debug.Log("交点がない時");
                // 3分割する処理
                {
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
                            // 頂点の追加
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
                                        //Debug.Log(idxMemory[j] + "" + idxMemory[j + 1] + "" + idxMemory[j + 2]);
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
                                            // インデックスの変更
                                            triangles1[i + 3] = _5;
                                            triangles1[i - 3] = _5;
                                            triangles1[i - 6] = _5;

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
                        }


                    }
                }
            }
        }

        // カットする処理(全体)
        {
            ////カットしたいオブジェクトのメッシュをトライアングルごとに処理
            //for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
            //{
            //    //メッシュの3つの頂点を取得
            //    p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);//+ Vector3.one * 0.0001f;
            //    p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);//+ Vector3.one * 0.0001f;
            //    p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]); //+ Vector3.one * 0.0001f;

            //    // カットポイントの始点がポリゴンの中にあるか
            //    double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
            //    double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[cutPoint.Count - 1].x + (p0.x - p2.x) * cutPoint[cutPoint.Count - 1].z);
            //    double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[cutPoint.Count - 1].x + (p1.x - p0.x) * cutPoint[cutPoint.Count - 1].z);

            //    // 三角形の中にあるか
            //    if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
            //    {
            //        // 一個前のカットポイントがあるか
            //        double _s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[cutPoint.Count - 2].x + (p0.x - p2.x) * cutPoint[cutPoint.Count - 2].z);
            //        double _t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[cutPoint.Count - 2].x + (p1.x - p0.x) * cutPoint[cutPoint.Count - 2].z);
            //        if ((0 <= _s && _s <= 1) && (0 <= _t && _t <= 1) && (0 <= 1 - _s - _t && 1 - _s - _t <= 1))
            //        {
            //            // あるとき
            //            Debug.Log("ある");

            //            // カットポイントの場所に頂点の追加(あとで分けるため二つ追加)
            //            vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);
            //            vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);

            //            // インデックスの割り当て
            //            int _0 = attachedMesh.triangles[i];
            //            int _1 = attachedMesh.triangles[i + 1];
            //            int _2 = attachedMesh.triangles[i + 2];
            //            int _3 = vertices1.Count - 2; // 7
            //            int _4 = vertices1.Count - 1; // 使わない  
            //            int _5 = vertices1.Count - 3; // 6

            //            // 記憶された三角形インデックスの数だけループ
            //            for (int j = 0; j < idxMemory.Count; j += 3)
            //            {
            //                // 記憶された三角形インデクスと一致した時
            //                if (idxMemory.Count > 9)
            //                {
            //                    if (attachedMesh.triangles[i] == idxMemory[j] && attachedMesh.triangles[i + 1] == idxMemory[j + 1] && attachedMesh.triangles[i + 2] == idxMemory[j + 2])
            //                    {
            //                        Debug.Log(idxMemory[j] + "" + idxMemory[j + 1] + "" + idxMemory[j + 2]);
            //                        if (j == 0)
            //                        {
            //                            Debug.Log("j = 0");
            //                            // インデックスの変更
            //                            triangles1[i + j + 3] = _5;

            //                            // カットポイントのあるポリゴンのインデックスの削除&追加

            //                            triangles1.RemoveRange(i, 3);
            //                            triangles1.Add(_3);
            //                            triangles1.Add(_1);
            //                            triangles1.Add(_2);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_2);
            //                            triangles1.Add(_5);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_0);
            //                            triangles1.Add(_1);

            //                            // 出来た三角形インデックスの保存
            //                            idxMemory.Clear();
            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_1);
            //                            idxMemory.Add(_2);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_2);
            //                            idxMemory.Add(_5);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_0);
            //                            idxMemory.Add(_1);
            //                            break;
            //                        }
            //                        if (j == 3)
            //                        {
            //                            Debug.Log("j = 3");
            //                            // カットポイントのあるポリゴンのインデックスの削除&追加
            //                            triangles1.RemoveRange(i, 3);
            //                            triangles1.Add(_3);
            //                            triangles1.Add(_1);
            //                            triangles1.Add(_2);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_2);
            //                            triangles1.Add(_5);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_0);
            //                            triangles1.Add(_1);

            //                            // 出来た三角形インデックスの保存
            //                            idxMemory.Clear();
            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_1);
            //                            idxMemory.Add(_2);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_2);
            //                            idxMemory.Add(_5);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_0);
            //                            idxMemory.Add(_1);
            //                            break;
            //                        }
            //                        if (j == 6)
            //                        {
            //                            Debug.Log("j = 6");
            //                            // インデックスの変更
            //                            triangles1[i + 3] = _5;
            //                            triangles1[i - 3] = _5;
            //                            triangles1[i - 6] = _5;

            //                            // カットポイントのあるポリゴンのインデックスの削除&追加
            //                            triangles1.RemoveRange(i, 3);
            //                            triangles1.Add(_3);
            //                            triangles1.Add(_1);
            //                            triangles1.Add(_2);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_2);
            //                            triangles1.Add(_5);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_0);
            //                            triangles1.Add(_1);


            //                            // 出来た三角形インデックスの保存
            //                            idxMemory.Clear();
            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_1);
            //                            idxMemory.Add(_2);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_2);
            //                            idxMemory.Add(_5);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_0);
            //                            idxMemory.Add(_1);
            //                            break;
            //                        }
            //                        if (j == 9)
            //                        {
            //                            Debug.Log("j = 9");
            //                            triangles1[i + j - 9] = _5;
            //                            triangles1[i + j - 15] = _5;
            //                            triangles1[i + j - 18] = _5;

            //                            // カットポイントのあるポリゴンのインデックスの削除&追加
            //                            triangles1.RemoveRange(i, 3);
            //                            triangles1.Add(_3);
            //                            triangles1.Add(_1);
            //                            triangles1.Add(_2);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_2);
            //                            triangles1.Add(_5);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_0);
            //                            triangles1.Add(_1);

            //                            // 出来た三角形インデックスの保存
            //                            idxMemory.Clear();
            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_1);
            //                            idxMemory.Add(_2);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_2);
            //                            idxMemory.Add(_5);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_0);
            //                            idxMemory.Add(_1);
            //                            break;
            //                        }

            //                    }

            //                }
            //                else if (idxMemory.Count < 10)  // 記憶された三角形インデックスの数が10よりも少ないとき(三角形が3個)
            //                {
            //                    if (attachedMesh.triangles[i] == idxMemory[j] && attachedMesh.triangles[i + 1] == idxMemory[j + 1] && attachedMesh.triangles[i + 2] == idxMemory[j + 2])
            //                    {
            //                        if (j == 0)
            //                        {
            //                            Debug.Log("j = 0");
            //                            // インデックスの変更
            //                            triangles1[i + j + 3] = _5;

            //                            // カットポイントのあるポリゴンのインデックスの削除&追加

            //                            triangles1.RemoveRange(i, 3);
            //                            triangles1.Add(_3);
            //                            triangles1.Add(_1);
            //                            triangles1.Add(_2);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_2);
            //                            triangles1.Add(_5);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_0);
            //                            triangles1.Add(_1);

            //                            // 出来た三角形インデックスの保存
            //                            idxMemory.Clear();
            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_1);
            //                            idxMemory.Add(_2);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_2);
            //                            idxMemory.Add(_5);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_0);
            //                            idxMemory.Add(_1);
            //                            break;
            //                        }
            //                        if (j == 3)
            //                        {
            //                            Debug.Log("j = 3");

            //                            triangles1[i + j - 3] = _5;
            //                            //triangles1[i + j] = _5;
            //                            // カットポイントのあるポリゴンのインデックスの削除&追加
            //                            triangles1.RemoveRange(i, 3);
            //                            triangles1.Add(_3);
            //                            triangles1.Add(_1);
            //                            triangles1.Add(_2);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_2);
            //                            triangles1.Add(_5);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_0);
            //                            triangles1.Add(_1);

            //                            // 出来た三角形インデックスの保存
            //                            idxMemory.Clear();
            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_1);
            //                            idxMemory.Add(_2);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_2);
            //                            idxMemory.Add(_5);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_0);
            //                            idxMemory.Add(_1);
            //                            break;
            //                        }
            //                        if (j == 6)
            //                        {
            //                            Debug.Log("j = 6");
            //                            Debug.Log("j = " + j);
            //                            Debug.Log("j + i = " + (j + i));
            //                            Debug.Log("2回目");

            //                            // インデックスの変更

            //                            triangles1[i - 3] = _5;
            //                            triangles1[i - 6] = _5;
            //                            //triangles1[i + j + 3] = _5;

            //                            // カットポイントのあるポリゴンのインデックスの削除&追加
            //                            triangles1.RemoveRange(i, 3);
            //                            triangles1.Add(_3);
            //                            triangles1.Add(_1);
            //                            triangles1.Add(_2);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_2);
            //                            triangles1.Add(_5);

            //                            triangles1.Add(_3);
            //                            triangles1.Add(_0);
            //                            triangles1.Add(_1);

            //                            // 出来た三角形インデックスの保存
            //                            idxMemory.Clear();
            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_1);
            //                            idxMemory.Add(_2);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_2);
            //                            idxMemory.Add(_5);

            //                            idxMemory.Add(_3);
            //                            idxMemory.Add(_0);
            //                            idxMemory.Add(_1);
            //                            break;
            //                        }
            //                    }

            //                }



            //            }

            //        }
            //        else
            //        {
            //            // ないとき
            //            Debug.Log("ない");

            //            // --- ポリゴンの辺とカットポイントが交差した時の処理 ---

            //            // 変数宣言              
            //            int vtxCount = vertices1.Count; // 何だったか忘れたww???
            //            var straddlePolyIdx = new List<int>();  // またいだポリゴン番号リスト
            //            var crossPolyIdx = new List<int>();  // 交差ポリゴン番号リスト
            //            var inerPolyIdx = new List<int>();  // カットポイントが中に入っているポリゴン番号
            //            var intersectPolyList = new List<List<Vector2>>();  // ポリゴンごとにある交差している点のリスト
            //            var intersectEdgList = new List<List<Vector2>>();  // ポリゴンごとにある交差している辺のリスト
            //            var intersectionList = new List<Vector2>(); // 交点のリスト
            //            var cp_s = new Vector2(cutPoint[cutPoint.Count - 2].x, cutPoint[cutPoint.Count - 2].z);    // カットポイントの終点の1個前
            //            var cp_v = new Vector2(cutPoint[cutPoint.Count - 1].x, cutPoint[cutPoint.Count - 1].z);    // カットポイントの終点
            //            var cpEdg = cp_v - cp_s;    // カットポイントの終点とカットポイントの終点の1個前をつないだ辺
            //            var checkCp = cp_s + cpEdg * 0.01f; // カットポイントの終点の1個前からカットポイントの終点の方向にちょっと伸ばした点
            //            var edgIdx2List = new List<List<int>>();   // 辺のインデックスのリストのリスト   

            //            // またいでるポリゴンと侵入しているポリゴンが何個あるか探す
            //            for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
            //            {
            //                // 変数宣言             
            //                int interPointCnt = 0; // 交差した点の数
            //                var intersection = new List<Vector2>(); // 交点のリスト
            //                var edgList = new List<Vector2>(); //辺のリスト
            //                var edgIdxList = new List<int>();   // 辺のインデックスのリスト   

            //                // ポリゴンの辺の数だけループ
            //                for (int k = 0; k < 3; k++)
            //                {
            //                    // ポリゴンの2頂点
            //                    Vector2 polyVtx_s = new Vector2(attachedMesh.vertices[attachedMesh.triangles[j + k]].x, attachedMesh.vertices[attachedMesh.triangles[j + k]].z);  // 始点
            //                    Vector2 polyVtx_v = new Vector2(attachedMesh.vertices[attachedMesh.triangles[j + (k + 1) % 3]].x, attachedMesh.vertices[attachedMesh.triangles[j + (k + 1) % 3]].z);  // 終点

            //                    // ポリゴンの辺
            //                    Vector2 polyEdge = polyVtx_v - polyVtx_s;   // 辺

            //                    // カットポイントの2頂点
            //                    Vector2 cpVtx_s = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z); // 始点
            //                    Vector2 cpVtx_v = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z); // 終点

            //                    // カットポイントの辺
            //                    Vector2 cpEdge = cpVtx_v - cpVtx_s; // 辺

            //                    // カットポイントの始点の補正
            //                    cpVtx_s += cpEdge * 0.01f;

            //                    // カットポイントの辺の補正
            //                    cpEdge = cpVtx_v - cpVtx_s; // 辺

            //                    // ポリゴンの辺とカットポイントの辺の始点をつないだベクトル
            //                    Vector2 v = polyVtx_s - cpVtx_s;

            //                    // 線分の始点から交点のベクトルの係数(多分)
            //                    float t1 = (v.x * polyEdge.y - polyEdge.x * v.y) / (cpEdge.x * polyEdge.y - polyEdge.x * cpEdge.y);
            //                    float t2 = (v.x * cpEdge.y - cpEdge.x * v.y) / (cpEdge.x * polyEdge.y - polyEdge.x * cpEdge.y);

            //                    // 交点
            //                    Vector2 p = new Vector2(polyVtx_s.x, polyVtx_s.y) + new Vector2(polyEdge.x * t2, polyEdge.y * t2);

            //                    // 線分と線分が交わっているか
            //                    const float eps = 0.00001f;
            //                    if (t1 + eps < 0 || t1 - eps > 1 || t2 + eps < 0 || t2 - eps > 1)
            //                    {
            //                        // 交わってないときスルー
            //                        continue;
            //                    }
            //                    else
            //                    {
            //                        //vertices1.Add(new Vector3(p.x, attachedMesh.vertices[0].y , p.y));
            //                        // 交わってる時交点カウント++                               
            //                        interPointCnt++;    // 交点カウント    
            //                        intersection.Add(p);    // 交点の保存
            //                        intersectionList.Add(p);// 交点の保存
            //                        edgList.Add(polyEdge);
            //                        edgIdxList.Add(attachedMesh.triangles[j + k]);
            //                        edgIdxList.Add(attachedMesh.triangles[j + (k + 1) % 3]);
            //                        Debug.Log("j + k" + attachedMesh.triangles[j + k]);
            //                        Debug.Log("j + (k + 1) % 3" + attachedMesh.triangles[j + (k + 1) % 3]);

            //                    }
            //                }

            //                // ポリゴン番号を保存
            //                if (interPointCnt == 2)// 交点カウント2個(ポリゴンをまたいでる時)
            //                {
            //                    Debug.Log("2個あるで");
            //                    Debug.Log("ポリゴン番号は" + attachedMesh.triangles[j] + "," + attachedMesh.triangles[j + 1] + "," + attachedMesh.triangles[j + 2]);

            //                    straddlePolyIdx.Add(attachedMesh.triangles[j]);
            //                    straddlePolyIdx.Add(attachedMesh.triangles[j + 1]);
            //                    straddlePolyIdx.Add(attachedMesh.triangles[j + 2]);
            //                    crossPolyIdx.Add(j);
            //                    crossPolyIdx.Add(j + 1);
            //                    crossPolyIdx.Add(j + 2);
            //                    intersectPolyList.Add(intersection);
            //                    intersectEdgList.Add(edgList);
            //                    edgIdx2List.Add(edgIdxList);
            //                    Debug.Log("straddlePolyIdx.Count" + straddlePolyIdx.Count);
            //                }
            //                else if (interPointCnt == 1)// 交点カウント1個(カットポイントの終点がポリゴンの中にあるとき)
            //                {
            //                    Debug.Log("1個あるで");
            //                    inerPolyIdx.Add(j);
            //                    inerPolyIdx.Add(j + 1);
            //                    inerPolyIdx.Add(j + 2);
            //                    crossPolyIdx.Add(j);
            //                    crossPolyIdx.Add(j + 1);
            //                    crossPolyIdx.Add(j + 2);
            //                    //intersectPolyList.Add(intersection);
            //                    //intersectEdgList.Add(edgList);
            //                }
            //                else
            //                {
            //                    // Debug.Log("3個あるで");
            //                    // Debug.Log("ポリゴン番号は" + attachedMesh.triangles[j] + "," + attachedMesh.triangles[j + 1] + "," + attachedMesh.triangles[j + 2]);

            //                }
            //            }

            //            var _p = new Vector2();

            //            // ポリゴンごとに処理
            //            for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
            //            {
            //                // カットポイントが増えたら終了
            //                if (vtxCount != vertices1.Count) break;

            //                // 交差した点の数
            //                int interPointCnt = 0;

            //                // ポリゴンの辺の数だけループ
            //                for (int k = 0; k < 3; k++)
            //                {
            //                    // ポリゴンの2頂点
            //                    Vector2 polyVtx_s = new Vector2(attachedMesh.vertices[attachedMesh.triangles[j + k]].x, attachedMesh.vertices[attachedMesh.triangles[j + k]].z);  // 始点
            //                    Vector2 polyVtx_v = new Vector2(attachedMesh.vertices[attachedMesh.triangles[j + (k + 1) % 3]].x, attachedMesh.vertices[attachedMesh.triangles[j + (k + 1) % 3]].z);  // 終点

            //                    // ポリゴンの辺
            //                    Vector2 polyEdge = polyVtx_v - polyVtx_s;   // 辺

            //                    // カットポイントの2頂点
            //                    Vector2 cpVtx_s = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z); // 始点
            //                    Vector2 cpVtx_v = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z); // 終点

            //                    // カットポイントの辺
            //                    Vector2 cpEdge = cpVtx_v - cpVtx_s; // 辺

            //                    // ポリゴンの辺とカットポイントの辺の始点をつないだベクトル
            //                    Vector2 v = polyVtx_s - cpVtx_s;

            //                    // 線分の始点から交点のベクトルの係数(多分)
            //                    float t1 = (v.x * polyEdge.y - polyEdge.x * v.y) / (cpEdge.x * polyEdge.y - polyEdge.x * cpEdge.y);
            //                    float t2 = (v.x * cpEdge.y - cpEdge.x * v.y) / (cpEdge.x * polyEdge.y - polyEdge.x * cpEdge.y);

            //                    // 交点
            //                    Vector2 p = new Vector2(polyVtx_s.x, polyVtx_s.y) + new Vector2(polyEdge.x * t2, polyEdge.y * t2);



            //                    // 線分と線分が交わっているか
            //                    const float eps = 0.00001f;
            //                    if (t1 + eps < 0 || t1 - eps > 1 || t2 + eps < 0 || t2 - eps > 1)
            //                    {

            //                        continue;

            //                    }
            //                    else
            //                    {
            //                        Debug.Log("交差してる");

            //                        // 交点カウントを足す
            //                        interPointCnt++;

            //                        // 変数宣言
            //                        // 交点をちょっとずらす
            //                        var interPoint = new Vector2(cutPoint[cutPoint.Count - 2].x + ((p.x + transform.position.x) - cutPoint[cutPoint.Count - 2].x) * 0.8f - transform.position.x, cutPoint[cutPoint.Count - 2].z + ((p.y + transform.position.z) - cutPoint[cutPoint.Count - 2].z) * 0.8f - transform.position.z);
            //                        var idxList = new List<int>();// インデクスのリスト
            //                        var edgIdx_s = new int[2];  // またいだ時の最初のインデックス
            //                        var edgIdx_v = new int[2];  // またいだ時の最初のインデックス

            //                        // --- 2分割する処理 ---                             
            //                        // ちょっとずらした交点がどのポリゴンにいるか調べる
            //                        for (int n = 0; n < attachedMesh.triangles.Length; n += 3)
            //                        {
            //                            //メッシュの3つの頂点を取得
            //                            var _p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[n]]);//+ Vector3.one * 0.0001f;
            //                            var _p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[n + 1]]);//+ Vector3.one * 0.0001f;
            //                            var _p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[n + 2]]); //+ Vector3.one * 0.0001f;

            //                            // ちょっとずらした交点がポリゴンの中にあるか
            //                            double _Area = 0.5 * (-_p1.z * _p2.x + _p0.z * (-_p1.x + _p2.x) + _p0.x * (_p1.z - _p2.z) + _p1.x * _p2.z);
            //                            double _s2 = 1 / (2 * _Area) * (_p0.z * _p2.x - _p0.x * _p2.z + (_p2.z - _p0.z) * (interPoint.x + transform.position.x) + (_p0.x - _p2.x) * (interPoint.y + transform.position.z));
            //                            double _t2 = 1 / (2 * _Area) * (_p0.x * _p1.z - _p0.z * _p1.x + (_p0.z - _p1.z) * (interPoint.x + transform.position.x) + (_p1.x - _p0.x) * (interPoint.y + transform.position.z));


            //                            // 三角形の中にあるか
            //                            if ((0 <= _s2 && _s2 <= 1) && (0 <= _t2 && _t2 <= 1) && (0 <= 1 - _s2 - _t2 && 1 - _s2 - _t2 <= 1))
            //                            {
            //                                //Debug.Log("交差している点がポリゴンの中にある");

            //                                // 記憶された三角形インデックスの数だけループ
            //                                for (int a = 0; a < idxMemory.Count; a += 3)
            //                                {
            //                                    // ちょっとずらした交点がどの記憶された三角形インデックスの中にあるか分岐
            //                                    if (attachedMesh.triangles[n] == idxMemory[a] && attachedMesh.triangles[n + 1] == idxMemory[a + 1] && attachedMesh.triangles[n + 2] == idxMemory[a + 2])
            //                                    {
            //                                        // 候補に追加
            //                                        Debug.Log("候補に追加");
            //                                        idxList.Add(attachedMesh.triangles[n]);
            //                                        idxList.Add(attachedMesh.triangles[n + 1]);
            //                                        idxList.Add(attachedMesh.triangles[n + 2]);
            //                                        edgIdx_s[0] = attachedMesh.triangles[j + k];  // 交点がある辺の始点
            //                                        edgIdx_s[1] = attachedMesh.triangles[j + (k + 1) % 3];  // 交点がある辺の終点
            //                                        _p = p; // 交点
            //                                                // 分岐
            //                                        if (a == 0)
            //                                        {
            //                                            Debug.Log("ポリゴン番号" + attachedMesh.triangles[n] + "," + attachedMesh.triangles[n + 1] + "," + attachedMesh.triangles[n + 2]);

            //                                            Debug.Log("a = 0");

            //                                            // 交点をもとに頂点を追加
            //                                            vertices1.Add(new Vector3(p.x, cutPoint[cutPoint.Count - 1].y - transform.position.y, p.y));
            //                                            vertices1.Add(new Vector3(p.x, cutPoint[cutPoint.Count - 1].y - transform.position.y, p.y));

            //                                            // 同じ座標に頂点があったら広げる
            //                                            for (int l = 0; l < vertices1.Count - 1; l++)
            //                                            {
            //                                                // 同じ座標じゃなかったスルー
            //                                                if (vertices1[l] != vertices1[l + 1]) continue;

            //                                                // 切る方向に対して点を移動するめの処理
            //                                                edge1 = new Vector3(polyVtx_v.x, 0, polyVtx_v.y) - new Vector3(polyVtx_s.x, 0, polyVtx_s.y);
            //                                                edge2 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
            //                                                edge = edge1;

            //                                                Debug.Log("edge" + edge);

            //                                                // カットポイントが一直線だったら
            //                                                // 垂直に点を広げる
            //                                                if (edge == Vector3.zero)
            //                                                {
            //                                                    edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
            //                                                    edge2 = (cutPoint[cutPoint.Count - 1] + Vector3.up) - cutPoint[cutPoint.Count - 1];
            //                                                    edge = Vector3.Cross(edge1, edge2);
            //                                                }

            //                                                vertices1[l] = vertices1[l] + edge.normalized * 0.04f;
            //                                                vertices1[l + 1] = vertices1[l + 1] - edge.normalized * 0.04f;
            //                                            }

            //                                            // インデックスの割り当て
            //                                            int idx0 = attachedMesh.triangles[n];
            //                                            int idx1 = attachedMesh.triangles[n + 1];
            //                                            int idx2 = attachedMesh.triangles[n + 2];
            //                                            int idx3 = vertices1.Count - 2; // 7
            //                                            int idx4 = vertices1.Count - 1; // 使わない  
            //                                            int idx5 = vertices1.Count - 3; // 6


            //                                            // インデックスの変更
            //                                            triangles1[n + 3] = idx5;

            //                                            // カットポイントのあるポリゴンのインデックスの削除&追加

            //                                            triangles1.RemoveRange(n, 3);
            //                                            triangles1.Add(idx4);
            //                                            triangles1.Add(idx2);
            //                                            triangles1.Add(idx5);

            //                                            triangles1.Add(idx3);
            //                                            triangles1.Add(idx0);
            //                                            triangles1.Add(idx1);

            //                                            // 出来た三角形インデックスの保存
            //                                            //idxMemory.Clear();
            //                                            //idxMemory.Add(idx3);
            //                                            //idxMemory.Add(idx2);
            //                                            //idxMemory.Add(idx5);

            //                                            //idxMemory.Add(idx3);
            //                                            //idxMemory.Add(idx0);
            //                                            //idxMemory.Add(idx1);

            //                                            break;
            //                                        }
            //                                        if (a == 3)
            //                                        {
            //                                            Debug.Log("a = 3");
            //                                            // 交点をもとに頂点を追加
            //                                            vertices1.Add(new Vector3(p.x, cutPoint[cutPoint.Count - 1].y - transform.position.y, p.y));
            //                                            vertices1.Add(new Vector3(p.x, cutPoint[cutPoint.Count - 1].y - transform.position.y, p.y));

            //                                            // 同じ座標に頂点があったら広げる
            //                                            for (int l = 0; l < vertices1.Count - 1; l++)
            //                                            {
            //                                                // 同じ座標じゃなかったスルー
            //                                                if (vertices1[l] != vertices1[l + 1]) continue;

            //                                                // 切る方向に対して点を移動するめの処理
            //                                                edge1 = new Vector3(polyVtx_v.x, 0, polyVtx_v.y) - new Vector3(polyVtx_s.x, 0, polyVtx_s.y);
            //                                                edge2 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
            //                                                edge = edge1;

            //                                                Debug.Log("edge" + edge);

            //                                                // カットポイントが一直線だったら
            //                                                // 垂直に点を広げる
            //                                                if (edge == Vector3.zero)
            //                                                {
            //                                                    edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
            //                                                    edge2 = (cutPoint[cutPoint.Count - 1] + Vector3.up) - cutPoint[cutPoint.Count - 1];
            //                                                    edge = Vector3.Cross(edge1, edge2);
            //                                                }

            //                                                vertices1[l] = vertices1[l] + edge.normalized * 0.04f;
            //                                                vertices1[l + 1] = vertices1[l + 1] - edge.normalized * 0.04f;
            //                                            }

            //                                            // インデックスの割り当て
            //                                            int idx0 = attachedMesh.triangles[n];
            //                                            int idx1 = attachedMesh.triangles[n + 1];
            //                                            int idx2 = attachedMesh.triangles[n + 2];
            //                                            int idx3 = vertices1.Count - 2; // 7
            //                                            int idx4 = vertices1.Count - 1; // 使わない  
            //                                            int idx5 = vertices1.Count - 3; // 6

            //                                            // インデックスの変更
            //                                            //triangles1[n + 3] = idx5;

            //                                            // カットポイントのあるポリゴンのインデックスの削除&追加
            //                                            triangles1.RemoveRange(n, 3);
            //                                            triangles1.Add(idx4);
            //                                            triangles1.Add(idx2);
            //                                            triangles1.Add(idx5);

            //                                            triangles1.Add(idx3);
            //                                            triangles1.Add(idx0);
            //                                            triangles1.Add(idx1);

            //                                            break;
            //                                        }
            //                                        if (a == 6)
            //                                        {
            //                                            Debug.Log("a = 6");
            //                                            // 交点をもとに頂点を追加
            //                                            vertices1.Add(new Vector3(p.x, cutPoint[cutPoint.Count - 1].y - transform.position.y, p.y));
            //                                            vertices1.Add(new Vector3(p.x, cutPoint[cutPoint.Count - 1].y - transform.position.y, p.y));

            //                                            // 同じ座標に頂点があったら広げる
            //                                            for (int l = 0; l < vertices1.Count - 1; l++)
            //                                            {
            //                                                // 同じ座標じゃなかったスルー
            //                                                if (vertices1[l] != vertices1[l + 1]) continue;

            //                                                // 切る方向に対して点を移動するめの処理
            //                                                edge1 = new Vector3(polyVtx_v.x, 0, polyVtx_v.y) - new Vector3(polyVtx_s.x, 0, polyVtx_s.y);
            //                                                edge2 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
            //                                                edge = edge1;

            //                                                Debug.Log("edge" + edge);

            //                                                // カットポイントが一直線だったら
            //                                                // 垂直に点を広げる
            //                                                if (edge == Vector3.zero)
            //                                                {
            //                                                    edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
            //                                                    edge2 = (cutPoint[cutPoint.Count - 1] + Vector3.up) - cutPoint[cutPoint.Count - 1];
            //                                                    edge = Vector3.Cross(edge1, edge2);
            //                                                }

            //                                                vertices1[l] = vertices1[l] + edge.normalized * 0.04f;
            //                                                vertices1[l + 1] = vertices1[l + 1] - edge.normalized * 0.04f;
            //                                            }

            //                                            // インデックスの割り当て
            //                                            int idx0 = attachedMesh.triangles[n];
            //                                            int idx1 = attachedMesh.triangles[n + 1];
            //                                            int idx2 = attachedMesh.triangles[n + 2];
            //                                            int idx3 = vertices1.Count - 2; // 7
            //                                            int idx4 = vertices1.Count - 1; // 使わない  
            //                                            int idx5 = vertices1.Count - 3; // 6

            //                                            // インデックスの変更
            //                                            triangles1[n - 6] = idx5;
            //                                            triangles1[n - 3] = idx5;

            //                                            // カットポイントのあるポリゴンのインデックスの削除&追加
            //                                            triangles1.RemoveRange(n, 3);
            //                                            triangles1.Add(idx4);
            //                                            triangles1.Add(idx2);
            //                                            triangles1.Add(idx5);

            //                                            triangles1.Add(idx3);
            //                                            triangles1.Add(idx0);
            //                                            triangles1.Add(idx1);

            //                                            // ここまで来たら三角形を二等分するのは終了
            //                                            break;
            //                                        }

            //                                    }
            //                                }

            //                                break;
            //                            }
            //                        }

            //                        // --- 2分割する処理2 ---
            //                        Debug.Log("straddlePolyIdx.Count" + straddlePolyIdx.Count);
            //                        Debug.Log("idxList.Count" + idxList.Count);
            //                        int whileCnt = 0;
            //                        while (straddlePolyIdx.Count > 0 && whileCnt < 50)
            //                        {
            //                            if (idxList.Count > 0)
            //                            {
            //                                bool triger = false;
            //                                for (int n = 0; n < straddlePolyIdx.Count; n += 3)
            //                                {
            //                                    if (straddlePolyIdx.Count == 0) break;
            //                                    // 三角形は3辺あるので3ループ
            //                                    for (int u = 0; u < 3; u++)
            //                                    {
            //                                        if (straddlePolyIdx.Count == 0) break;
            //                                        // 1辺に対して3辺調べるので3ループ
            //                                        for (int m = 0; m < 3; m++)
            //                                        {
            //                                            // 同じ辺があるか
            //                                            if ((straddlePolyIdx[n + u] == idxList[m] && straddlePolyIdx[n + (u + 1) % 3] == idxList[(m + 1) % 3]) ||
            //                                                (straddlePolyIdx[n + u] == idxList[(m + 1) % 3] && straddlePolyIdx[n + (u + 1) % 3] == idxList[m]))
            //                                            {
            //                                                Debug.Log("おなじぇ辺がある");
            //                                                Debug.Log("ポリゴン番号は" + straddlePolyIdx[n] + "," + straddlePolyIdx[n + 1] + "," + straddlePolyIdx[n + 2]);
            //                                                Debug.Log("idxList番号は" + idxList[0] + "," + idxList[1] + "," + idxList[2]);

            //                                                var cpEdg_v = cutPoint[cutPoint.Count - 1];
            //                                                var cpEdg_s = cutPoint[cutPoint.Count - 2];
            //                                                var cpEdg_b = cutPoint[cutPoint.Count - 3];
            //                                                var cpEdg_sv = cpEdg_v - cpEdg_s;
            //                                                var cpEdg_bs = cpEdg_s - cpEdg_b;
            //                                                var cpEdg_bv = cpEdg_sv + cpEdg_bs;
            //                                                var cpEdgNor = Vector3.Cross(cpEdg_bv, Vector3.up);

            //                                                // 頂点の追加
            //                                                vertices1.Add(new Vector3(intersectPolyList[n / 3][0].x, cutPoint[cutPoint.Count - 1].y - transform.position.y, intersectPolyList[n / 3][0].y) + new Vector3(intersectEdgList[n / 3][0].normalized.x * 0.04f * (cpEdgNor.normalized.x / Mathf.Abs(cpEdgNor.normalized.x)), 0, intersectEdgList[n / 3][0].normalized.y * 0.04f * (cpEdgNor.normalized.z / Mathf.Abs(cpEdgNor.normalized.z))));
            //                                                vertices1.Add(new Vector3(intersectPolyList[n / 3][0].x, cutPoint[cutPoint.Count - 1].y - transform.position.y, intersectPolyList[n / 3][0].y) + new Vector3(intersectEdgList[n / 3][0].normalized.x * 0.04f * -(cpEdgNor.normalized.x / Mathf.Abs(cpEdgNor.normalized.x)), 0, intersectEdgList[n / 3][0].normalized.y * 0.04f * -(cpEdgNor.normalized.z / Mathf.Abs(cpEdgNor.normalized.z))));

            //                                                // インデックスの割り当て
            //                                                int idx0 = straddlePolyIdx[n];
            //                                                int idx1 = straddlePolyIdx[n + 1];
            //                                                int idx2 = straddlePolyIdx[n + 2];
            //                                                int idx3 = vertices1.Count - 4; // 7
            //                                                int idx4 = vertices1.Count - 3; // 使わない  
            //                                                int idx5 = vertices1.Count - 2; // 6
            //                                                int idx6 = vertices1.Count - 1; // 6

            //                                                // またいでるポリゴンの辺の交差している終点がある辺のインデックス
            //                                                edgIdx_v[0] = edgIdx2List[n / 3][2];
            //                                                edgIdx_v[1] = edgIdx2List[n / 3][3];


            //                                                // 地獄の6分岐を2回行う
            //                                                for (int twice = 0; twice < 2; twice++)
            //                                                {
            //                                                    Debug.Log("edgIdx_s[0]:" + edgIdx_s[0]);
            //                                                    Debug.Log("edgIdx_s[1]:" + edgIdx_s[1]);
            //                                                    Debug.Log("edgIdx_v[0]:" + edgIdx_v[0]);
            //                                                    Debug.Log("edgIdx_v[1]:" + edgIdx_v[1]);

            //                                                    // 地獄の6分岐
            //                                                    if ((edgIdx_s[0] == straddlePolyIdx[n] || edgIdx_s[0] == straddlePolyIdx[n + 1]) && (edgIdx_s[1] == straddlePolyIdx[n + 1] || edgIdx_s[1] == straddlePolyIdx[n]) && (edgIdx_v[0] == straddlePolyIdx[n + 1] || edgIdx_v[0] == straddlePolyIdx[n + 2]) && (edgIdx_v[1] == straddlePolyIdx[n + 2] || edgIdx_v[1] == straddlePolyIdx[n + 1]))
            //                                                    {
            //                                                        Debug.Log("地獄の6分岐1");
            //                                                        // ポリゴンの数だけループ
            //                                                        for (int b = 0; b < attachedMesh.triangles.Length; b += 3)
            //                                                        {
            //                                                            if (!(straddlePolyIdx[n] == attachedMesh.triangles[b] && straddlePolyIdx[n + 1] == attachedMesh.triangles[b + 1] && straddlePolyIdx[n + 2] == attachedMesh.triangles[b + 2])) continue;
            //                                                            triangles1.RemoveRange(b, 3);
            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx3);
            //                                                            triangles1.Add(idx1);

            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx2);
            //                                                            triangles1.Add(idx0);

            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx0);
            //                                                            triangles1.Add(idx4);

            //                                                            idxMemory.Add(idx5);
            //                                                            idxMemory.Add(idx3);
            //                                                            idxMemory.Add(idx1);

            //                                                            idxMemory.Add(idx6);
            //                                                            idxMemory.Add(idx2);
            //                                                            idxMemory.Add(idx0);

            //                                                            idxMemory.Add(idx6);
            //                                                            idxMemory.Add(idx0);
            //                                                            idxMemory.Add(idx4);
            //                                                            break;
            //                                                        }
            //                                                        break;
            //                                                    }
            //                                                    else if ((edgIdx_s[0] == straddlePolyIdx[n] || edgIdx_s[0] == straddlePolyIdx[n + 1]) && (edgIdx_s[1] == straddlePolyIdx[n + 1] || edgIdx_s[1] == straddlePolyIdx[n]) &&
            //                                                             (edgIdx_v[0] == straddlePolyIdx[n] || edgIdx_v[0] == straddlePolyIdx[n + 2]) && (edgIdx_v[1] == straddlePolyIdx[n + 2] || edgIdx_v[1] == straddlePolyIdx[n]))
            //                                                    {
            //                                                        Debug.Log("地獄の6分岐2");
            //                                                        // ポリゴンの数だけループ
            //                                                        for (int b = 0; b < attachedMesh.triangles.Length; b += 3)
            //                                                        {
            //                                                            if (!(straddlePolyIdx[n] == attachedMesh.triangles[b] && straddlePolyIdx[n + 1] == attachedMesh.triangles[b + 1] && straddlePolyIdx[n + 2] == attachedMesh.triangles[b + 2])) continue;
            //                                                            triangles1.RemoveRange(b, 3);
            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx0);
            //                                                            triangles1.Add(idx4);

            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx1);
            //                                                            triangles1.Add(idx2);

            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx3);
            //                                                            triangles1.Add(idx1);


            //                                                            idxMemory.Add(idx6);
            //                                                            idxMemory.Add(idx0);
            //                                                            idxMemory.Add(idx4);

            //                                                            idxMemory.Add(idx5);
            //                                                            idxMemory.Add(idx1);
            //                                                            idxMemory.Add(idx2);

            //                                                            idxMemory.Add(idx5);
            //                                                            idxMemory.Add(idx3);
            //                                                            idxMemory.Add(idx1);
            //                                                            break;
            //                                                        }
            //                                                        break;
            //                                                    }
            //                                                    else if ((edgIdx_s[0] == straddlePolyIdx[n + 1] || edgIdx_s[0] == straddlePolyIdx[n + 2]) && (edgIdx_s[1] == straddlePolyIdx[n + 2] || edgIdx_s[1] == straddlePolyIdx[n + 1]) &&
            //                                                                (edgIdx_v[0] == straddlePolyIdx[n] || edgIdx_v[0] == straddlePolyIdx[n + 1]) && (edgIdx_v[1] == straddlePolyIdx[n + 1] || edgIdx_v[1] == straddlePolyIdx[n]))
            //                                                    {
            //                                                        Debug.Log("地獄の6分岐3");
            //                                                        // ポリゴンの数だけループ
            //                                                        for (int b = 0; b < attachedMesh.triangles.Length; b += 3)
            //                                                        {
            //                                                            if (!(straddlePolyIdx[n] == attachedMesh.triangles[b] && straddlePolyIdx[n + 1] == attachedMesh.triangles[b + 1] && straddlePolyIdx[n + 2] == attachedMesh.triangles[b + 2])) continue;
            //                                                            triangles1.RemoveRange(b, 3);
            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx1);
            //                                                            triangles1.Add(idx2);

            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx3);
            //                                                            triangles1.Add(idx2);

            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx2);
            //                                                            triangles1.Add(idx0);

            //                                                            idxMemory.Add(idx6);
            //                                                            idxMemory.Add(idx1);
            //                                                            idxMemory.Add(idx2);

            //                                                            idxMemory.Add(idx5);
            //                                                            idxMemory.Add(idx3);
            //                                                            idxMemory.Add(idx2);

            //                                                            idxMemory.Add(idx5);
            //                                                            idxMemory.Add(idx2);
            //                                                            idxMemory.Add(idx0);
            //                                                            break;
            //                                                        }
            //                                                        break;
            //                                                    }
            //                                                    else if ((edgIdx_s[0] == straddlePolyIdx[n + 1] || edgIdx_s[0] == straddlePolyIdx[n + 2]) && (edgIdx_s[1] == straddlePolyIdx[n + 2] || edgIdx_s[1] == straddlePolyIdx[n + 1]) &&
            //                                                                (edgIdx_v[0] == straddlePolyIdx[n] || edgIdx_v[0] == straddlePolyIdx[n + 2]) && (edgIdx_v[1] == straddlePolyIdx[n + 2] || edgIdx_v[1] == straddlePolyIdx[n]))
            //                                                    {
            //                                                        Debug.Log("地獄の6分岐4");
            //                                                        // ポリゴンの数だけループ
            //                                                        for (int b = 0; b < attachedMesh.triangles.Length; b += 3)
            //                                                        {
            //                                                            if (!(straddlePolyIdx[n] == attachedMesh.triangles[b] && straddlePolyIdx[n + 1] == attachedMesh.triangles[b + 1] && straddlePolyIdx[n + 2] == attachedMesh.triangles[b + 2])) continue;
            //                                                            triangles1.RemoveRange(b, 3);
            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx3);
            //                                                            triangles1.Add(idx2);

            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx1);
            //                                                            triangles1.Add(idx4);

            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx0);
            //                                                            triangles1.Add(idx1);

            //                                                            idxMemory.Add(idx5);
            //                                                            idxMemory.Add(idx3);
            //                                                            idxMemory.Add(idx2);

            //                                                            idxMemory.Add(idx6);
            //                                                            idxMemory.Add(idx1);
            //                                                            idxMemory.Add(idx4);

            //                                                            idxMemory.Add(idx6);
            //                                                            idxMemory.Add(idx0);
            //                                                            idxMemory.Add(idx1);
            //                                                            break;
            //                                                        }
            //                                                        break;
            //                                                    }
            //                                                    else if ((edgIdx_s[0] == straddlePolyIdx[n] || edgIdx_s[0] == straddlePolyIdx[n + 2]) && (edgIdx_s[1] == straddlePolyIdx[n + 2] || edgIdx_s[1] == straddlePolyIdx[n]) &&
            //                                                                (edgIdx_v[0] == straddlePolyIdx[n] || edgIdx_v[0] == straddlePolyIdx[n + 1]) && (edgIdx_v[1] == straddlePolyIdx[n + 1] || edgIdx_v[1] == straddlePolyIdx[n]))
            //                                                    {
            //                                                        Debug.Log("地獄の6分岐5");
            //                                                        // ポリゴンの数だけループ
            //                                                        for (int b = 0; b < attachedMesh.triangles.Length; b += 3)
            //                                                        {
            //                                                            if (!(straddlePolyIdx[n] == attachedMesh.triangles[b] && straddlePolyIdx[n + 1] == attachedMesh.triangles[b + 1] && straddlePolyIdx[n + 2] == attachedMesh.triangles[b + 2])) continue;
            //                                                            triangles1.RemoveRange(b, 3);
            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx3);
            //                                                            triangles1.Add(idx0);

            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx1);
            //                                                            triangles1.Add(idx2);

            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx2);
            //                                                            triangles1.Add(idx4);

            //                                                            idxMemory.Add(idx5);
            //                                                            idxMemory.Add(idx3);
            //                                                            idxMemory.Add(idx0);

            //                                                            idxMemory.Add(idx6);
            //                                                            idxMemory.Add(idx1);
            //                                                            idxMemory.Add(idx2);

            //                                                            idxMemory.Add(idx6);
            //                                                            idxMemory.Add(idx2);
            //                                                            idxMemory.Add(idx4);
            //                                                            break;
            //                                                        }
            //                                                        break;
            //                                                    }
            //                                                    else if ((edgIdx_s[0] == straddlePolyIdx[n] || edgIdx_s[0] == straddlePolyIdx[n + 2]) && (edgIdx_s[1] == straddlePolyIdx[n + 2] || edgIdx_s[1] == straddlePolyIdx[n]) &&
            //                                                                (edgIdx_v[0] == straddlePolyIdx[n + 1] || edgIdx_v[0] == straddlePolyIdx[n + 2]) && (edgIdx_v[1] == straddlePolyIdx[n + 2] || edgIdx_v[1] == straddlePolyIdx[n + 2]))
            //                                                    {
            //                                                        Debug.Log("地獄の6分岐6");
            //                                                        // ポリゴンの数だけループ
            //                                                        for (int b = 0; b < attachedMesh.triangles.Length; b += 3)
            //                                                        {
            //                                                            if (!(straddlePolyIdx[n] == attachedMesh.triangles[b] && straddlePolyIdx[n + 1] == attachedMesh.triangles[b + 1] && straddlePolyIdx[n + 2] == attachedMesh.triangles[b + 2])) continue;
            //                                                            triangles1.RemoveRange(b, 3);
            //                                                            triangles1.Add(idx6);
            //                                                            triangles1.Add(idx2);
            //                                                            triangles1.Add(idx4);

            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx3);
            //                                                            triangles1.Add(idx0);

            //                                                            triangles1.Add(idx5);
            //                                                            triangles1.Add(idx0);
            //                                                            triangles1.Add(idx1);

            //                                                            break;
            //                                                        }
            //                                                        break;
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        Debug.Log("aaa");
            //                                                        edgIdx_v[0] = edgIdx2List[n / 3][0];
            //                                                        edgIdx_v[1] = edgIdx2List[n / 3][1];

            //                                                    }

            //                                                }

            //                                                // 候補から削除

            //                                                edgIdx_s[0] = edgIdx_v[0];
            //                                                edgIdx_s[1] = edgIdx_v[1];
            //                                                idxList.Clear();
            //                                                idxList.Add(straddlePolyIdx[n]);
            //                                                idxList.Add(straddlePolyIdx[n + 1]);
            //                                                idxList.Add(straddlePolyIdx[n + 2]);
            //                                                straddlePolyIdx.RemoveRange(n, 3);
            //                                                intersectPolyList.RemoveRange(n / 3, 1);
            //                                                intersectEdgList.RemoveRange(n / 3, 1);
            //                                                triger = true;
            //                                                break;

            //                                            }

            //                                        }

            //                                        if (triger) break;
            //                                    }


            //                                }

            //                            }
            //                            else
            //                            {
            //                                break;
            //                            }
            //                            whileCnt++;
            //                        }





            //                        // ここまで来たら終了
            //                        //break;

            //                    }

            //                    Debug.Log("interPointCnt" + interPointCnt);

            //                    // ポリゴンに含まれる交点が1個の時
            //                    if (interPointCnt == 1)
            //                    {
            //                        Debug.Log("1個");

            //                    }
            //                    else if (interPointCnt == 2)
            //                    {   // ポリゴンに含まれる交点が2個の時
            //                        Debug.Log("2個");
            //                    }

            //                    // ここまで来たら終了
            //                    //break;
            //                }
            //                // ここまで来たら終了
            //                //break;
            //            }


            //            // --- 4分割する処理 ---
            //            // カットポイントの終点ががどのポリゴンにいるか調べる
            //            for (int n = 0; n < attachedMesh.triangles.Length; n += 3)
            //            {
            //                //メッシュの3つの頂点を取得
            //                var _p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[n]]);//+ Vector3.one * 0.0001f;
            //                var _p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[n + 1]]);//+ Vector3.one * 0.0001f;
            //                var _p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[n + 2]]); //+ Vector3.one * 0.0001f;

            //                // カットポイントの終点がポリゴンの中にあるか
            //                double _Area = 0.5 * (-_p1.z * _p2.x + _p0.z * (-_p1.x + _p2.x) + _p0.x * (_p1.z - _p2.z) + _p1.x * _p2.z);
            //                double _s2 = 1 / (2 * _Area) * (_p0.z * _p2.x - _p0.x * _p2.z + (_p2.z - _p0.z) * cutPoint[cutPoint.Count - 1].x + (_p0.x - _p2.x) * cutPoint[cutPoint.Count - 1].z);
            //                double _t2 = 1 / (2 * _Area) * (_p0.x * _p1.z - _p0.z * _p1.x + (_p0.z - _p1.z) * cutPoint[cutPoint.Count - 1].x + (_p1.x - _p0.x) * cutPoint[cutPoint.Count - 1].z);

            //                // 三角形の中にあるか
            //                if ((0 <= _s2 && _s2 <= 1) && (0 <= _t2 && _t2 <= 1) && (0 <= 1 - _s2 - _t2 && 1 - _s2 - _t2 <= 1))
            //                {
            //                    // あるとき
            //                    Debug.Log("ポリゴン番号" + attachedMesh.triangles[n] + "," + attachedMesh.triangles[n + 1] + "," + attachedMesh.triangles[n + 2]);

            //                    // ベクトルの係数の計算
            //                    double _s3 = 1 / (2 * _Area) * (_p0.z * _p2.x - _p0.x * _p2.z + (_p2.z - _p0.z) * (_p.x + transform.position.x) + (_p0.x - _p2.x) * (_p.y + transform.position.z));
            //                    double _t3 = 1 / (2 * _Area) * (_p0.x * _p1.z - _p0.z * _p1.x + (_p0.z - _p1.z) * (_p.x + transform.position.x) + (_p1.x - _p0.x) * (_p.y + transform.position.z));

            //                    // カットポイントの終点に頂点の追加
            //                    vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);
            //                    vertices1.Add(cutPoint[cutPoint.Count - 1] - transform.position);

            //                    // 交点がどのポリゴンの辺にいるのか
            //                    if (_t3 < 0.002f) // 辺S上
            //                    {
            //                        Debug.Log("辺S上");

            //                        // インデックスの割り当て
            //                        int idx0 = attachedMesh.triangles[n];
            //                        int idx1 = attachedMesh.triangles[n + 1];
            //                        int idx2 = attachedMesh.triangles[n + 2];
            //                        int idx3 = vertices1.Count - 4; // 
            //                        int idx4 = vertices1.Count - 3; // 
            //                        int idx5 = vertices1.Count - 2; // 
            //                        int idx6 = vertices1.Count - 1; // 


            //                        // カットポイントのあるポリゴンのインデックスの削除&追加
            //                        triangles1.RemoveRange(i, 3);

            //                        // インデックスの振り分け
            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx2);
            //                        triangles1.Add(idx0);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx0);
            //                        triangles1.Add(idx4);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx3);
            //                        triangles1.Add(idx1);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx1);
            //                        triangles1.Add(idx2);

            //                        // 出来た三角形インデックスの保存
            //                        idxMemory.Clear();
            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx2);
            //                        idxMemory.Add(idx0);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx0);
            //                        idxMemory.Add(idx4);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx3);
            //                        idxMemory.Add(idx1);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx1);
            //                        idxMemory.Add(idx2);
            //                    }
            //                    else if (_s3 < 0.002f)    // 辺T上
            //                    {
            //                        Debug.Log("辺T上");

            //                        // インデックスの割り当て
            //                        int idx0 = attachedMesh.triangles[n];
            //                        int idx1 = attachedMesh.triangles[n + 1];
            //                        int idx2 = attachedMesh.triangles[n + 2];
            //                        int idx3 = vertices1.Count - 4; // 
            //                        int idx4 = vertices1.Count - 3; // 
            //                        int idx5 = vertices1.Count - 2; // 
            //                        int idx6 = vertices1.Count - 1; // 

            //                        // カットポイントのあるポリゴンのインデックスの削除&追加
            //                        triangles1.RemoveRange(n, 3);

            //                        // インデックスの振り分け
            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx1);
            //                        triangles1.Add(idx2);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx2);
            //                        triangles1.Add(idx4);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx3);
            //                        triangles1.Add(idx0);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx0);
            //                        triangles1.Add(idx1);

            //                        // 出来た三角形インデックスの保存
            //                        idxMemory.Clear();
            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx1);
            //                        idxMemory.Add(idx2);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx2);
            //                        idxMemory.Add(idx4);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx3);
            //                        idxMemory.Add(idx0);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx0);
            //                        idxMemory.Add(idx1);

            //                    }
            //                    else if (_s3 + _t3 > 0.98f) // 辺S+T上
            //                    {
            //                        Debug.Log("辺S + T上");

            //                        // インデックスの割り当て
            //                        int idx0 = attachedMesh.triangles[n];
            //                        int idx1 = attachedMesh.triangles[n + 1];
            //                        int idx2 = attachedMesh.triangles[n + 2];
            //                        int idx3 = vertices1.Count - 4; // 
            //                        int idx4 = vertices1.Count - 3; // 
            //                        int idx5 = vertices1.Count - 2; // 
            //                        int idx6 = vertices1.Count - 1; // 

            //                        // カットポイントのあるポリゴンのインデックスの削除&追加
            //                        triangles1.RemoveRange(i, 3);

            //                        // インデックスの振り分け
            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx0);
            //                        triangles1.Add(idx1);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx1);
            //                        triangles1.Add(idx4);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx3);
            //                        triangles1.Add(idx2);

            //                        triangles1.Add(idx5);
            //                        triangles1.Add(idx2);
            //                        triangles1.Add(idx0);

            //                        // 出来た三角形インデックスの保存
            //                        idxMemory.Clear();
            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx0);
            //                        idxMemory.Add(idx1);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx1);
            //                        idxMemory.Add(idx4);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx3);
            //                        idxMemory.Add(idx2);

            //                        idxMemory.Add(idx5);
            //                        idxMemory.Add(idx2);
            //                        idxMemory.Add(idx0);
            //                    }
            //                    else
            //                    {
            //                        Debug.Log("どの辺にもない");
            //                    }

            //                }


            //            }


            //        }



            //        Debug.Log(idxMemory.Count);


            //    }
            //}

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

        var mesh = new Mesh();
        mesh.vertices = vertices1.ToArray();
        mesh.triangles = triangles1.ToArray();
        mesh.normals = normals1.ToArray();

        attachedMesh = mesh;
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        return true;
    }

    // メッシュの分割(最後)
    public void DivisionMeshTwice(List<Vector3> cutPoint)
    {
        Debug.Log("---------最後の処理---------");
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
        Vector3 edge3 = new Vector3();

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


            // 切る方向に対して点を移動するめの処理
            edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 3];
            edge2 = cutPoint[cutPoint.Count - 1] - cutPoint[cutPoint.Count - 2];
            edge3 = edge1 + edge2;


            // カットポイントが一直線だったら
            // 垂直に点を広げる
            if (edge3 == Vector3.zero)
            {
                edge1 = cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1];
                edge2 = (cutPoint[cutPoint.Count - 1] + Vector3.up) - cutPoint[cutPoint.Count - 1];
                edge = Vector3.Cross(edge2, edge1);
            }
            else
            {
                edge = Vector3.Cross(edge3, Vector3.up);

            }

            // 頂点に格納
            vertices1[i] = vertices1[i] + edge.normalized * 0.04f;
            vertices1[i + 1] = vertices1[i + 1] - edge.normalized * 0.04f;
        }

        // カットする処理
        {
            // 変数宣言              
            var straddlePolyIdx = new List<int>();  // またいだポリゴン番号リスト
            var crossPolyIdx = new List<int>();     // 交差ポリゴン番号リスト
            var inerPolyIdx = new List<int>();      // カットポイントが中に入っているポリゴン番号
            var intersectPolyList = new List<List<Vector2>>();  // ポリゴンごとにある交差している点のリスト
            var intersectPolyList2 = new List<List<Vector2>>();  // ポリゴンごとにある交差している点のリスト2
            var intersectEdgList = new List<List<Vector2>>();   // ポリゴンごとにある交差している辺のリスト
            var intersectEdgList2 = new List<List<Vector2>>();   // ポリゴンごとにある交差している辺のリスト
            var intersectionList = new List<Vector2>();         // 交点のリスト
            var cp_s = new Vector2(cutPoint[cutPoint.Count - 2].x, cutPoint[cutPoint.Count - 2].z);    // カットポイントの終点の1個前
            var cp_v = new Vector2(cutPoint[cutPoint.Count - 1].x, cutPoint[cutPoint.Count - 1].z);    // カットポイントの終点
            var cpEdg = cp_v - cp_s;    // カットポイントの終点とカットポイントの終点の1個前をつないだ辺
            var checkCp = cp_s + cpEdg * 0.01f; // カットポイントの終点の1個前からカットポイントの終点の方向にちょっと伸ばした点
            var edgIdx2List = new List<List<int>>();   // 辺のインデックスのリストのリスト   
            var edgIdx2List2 = new List<List<int>>();  // 辺のインデックスのリストのリスト2   

            // またいでるポリゴンと侵入しているポリゴンが何個あるか探す
            for (int j = 0; j < attachedMesh.triangles.Length; j += 3)
            {
                // 変数宣言             
                int interPointCnt = 0; // 交差した点の数
                var intersection = new List<Vector2>(); // 交点のリスト
                var edgList = new List<Vector2>(); //辺のリスト
                var edgIdxList = new List<int>();   // 辺のインデックスのリスト   

                // ポリゴンの辺の数だけループ
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

                    // カットポイントの始点の補正
                    cpVtx_s += cpEdge * 0.01f;

                    // カットポイントの辺の補正
                    cpEdge = cpVtx_v - cpVtx_s; // 辺

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
                        // 交わってないときスルー
                        continue;
                    }
                    else
                    {
                        // 交わってる時交点カウント++                               
                        interPointCnt++;    // 交点カウント    
                        intersection.Add(p);    // 交点の保存
                        intersectionList.Add(p);// 交点の保存
                        edgList.Add(polyEdge);
                        edgIdxList.Add(attachedMesh.triangles[j + k]);
                        edgIdxList.Add(attachedMesh.triangles[j + (k + 1) % 3]);

                    }
                }

                // ポリゴン番号を保存
                if (interPointCnt == 2)// 交点カウント2個(ポリゴンをまたいでる時)
                {
                    Debug.Log("2個あるで");
                    //Debug.Log("ポリゴン番号は" + attachedMesh.triangles[j] + "," + attachedMesh.triangles[j + 1] + "," + attachedMesh.triangles[j + 2]);

                    straddlePolyIdx.Add(attachedMesh.triangles[j]);
                    straddlePolyIdx.Add(attachedMesh.triangles[j + 1]);
                    straddlePolyIdx.Add(attachedMesh.triangles[j + 2]);
                    crossPolyIdx.Add(j);
                    crossPolyIdx.Add(j + 1);
                    crossPolyIdx.Add(j + 2);
                    intersectPolyList2.Add(intersection);
                    intersectEdgList2.Add(edgList);
                    edgIdx2List2.Add(edgIdxList);
                    //Debug.Log("straddlePolyIdx.Count" + straddlePolyIdx.Count);
                }
                else if (interPointCnt == 1)// 交点カウント1個(カットポイントの終点がポリゴンの中にあるとき)
                {
                    Debug.Log("1個あるよ");
                    inerPolyIdx.Add(attachedMesh.triangles[j]);
                    inerPolyIdx.Add(attachedMesh.triangles[j + 1]);
                    inerPolyIdx.Add(attachedMesh.triangles[j + 2]);
                    crossPolyIdx.Add(j);
                    crossPolyIdx.Add(j + 1);
                    crossPolyIdx.Add(j + 2);
                    edgIdx2List.Add(edgIdxList);
                    intersectPolyList.Add(intersection);
                    intersectEdgList.Add(edgList);
                }
                else
                {
                    // Debug.Log("3個あるで");
                    // Debug.Log("ポリゴン番号は" + attachedMesh.triangles[j] + "," + attachedMesh.triangles[j + 1] + "," + attachedMesh.triangles[j + 2]);

                }
            }

            //--- カットポイントとの交点の数で分岐 ---
            // 交点が1個の時(ポリゴンの中に2個交点があるポリゴンが1個もないとき)
            if (straddlePolyIdx.Count == 0 && inerPolyIdx.Count > 0)
            {
                Debug.Log("交点が1個の時");

                //--- 変数宣言 ---
                int firstNum = 0;
                int secondNum = 0;
                Vector2 cpS = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z);
                Vector2 cpV = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z);
                Vector2 p = cpV;
                var idxList2 = new List<int>();  // 1個前に分割したインデックスのリスト
                var rastIdxList = new List<int>();  // 2個に分割する処理をした最後のインデックス保存用のリスト             
               
                // 2分割する処理(最初)
                {
                    Debug.Log("2分割する処理");

                    //--- 記憶された三角形インデクスをもとにインデックスを割り振る ---
                    // 記憶された三角形インデックスの数だけループ
                    for (int a = 0; a < idxMemory.Count; a += 3)
                    {
                        bool end3 = false;
                        // 分割対象のポリゴンの数だけループ
                        for (int w = 0; w < inerPolyIdx.Count; w += 3)
                        {
                            // 記憶されたインデックスと一致しなかったらスルー
                            if (!(inerPolyIdx[w] == idxMemory[a] && inerPolyIdx[w + 1] == idxMemory[a + 1] && inerPolyIdx[w + 2] == idxMemory[a + 2])) continue;
                            Debug.Log("intersectPolyList[w/3][0]:" + intersectPolyList[w / 3][0]);
                            Debug.Log("intersectEdgList[w/3][0]:" + intersectEdgList[w / 3][0]);

                            //--- 変数宣言 ---
                            var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
                            var cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));
                            var pEnd = intersectPolyList[w / 3][0];    // 交点の終点
                            var pEdge = new Vector2(Mathf.Abs(intersectEdgList[w / 3][0].x), Mathf.Abs(intersectEdgList[w / 3][0].y));

                            // 候補に追加
                            //idxList2.Clear();
                            //idxList2.Add(edgIdx2List[w / 3][0]);
                            //idxList2.Add(edgIdx2List[w / 3][1]);

                            // 今追加された候補と交点が二個あるポリゴンの辺と比較
                            //for (int g = 0; g < straddlePolyIdx.Count; g += 3)
                            //{
                            //    bool end2 = false;
                            //    // 辺の数だけループ
                            //    for (int f = 0; f < 3; f++)
                            //    {
                            //        // 一致しなかったらスルー
                            //        if (((straddlePolyIdx[g + f] == idxList2[0] || straddlePolyIdx[g + f] == idxList2[1]) && (straddlePolyIdx[g + (f + 1) % 3] == idxList2[1] || straddlePolyIdx[g + (f + 1) % 3] == idxList2[0]))) continue;

                            //        Debug.Log("候補に追加");
                            //        firstNum = g;
                            //        end2 = true;
                            //        break;
                            //    }
                            //    if (end2) break;
                            //}

                            // メッシュのポリゴンの数だけループ
                            for (int c = 0; c < triangles1.Count; c += 3)
                            {
                                // 一致しなかったらスルー
                                if (!(triangles1[c] == inerPolyIdx[w] && triangles1[c + 1] == inerPolyIdx[w + 1] && triangles1[c + 2] == inerPolyIdx[w + 2])) continue;
                                Debug.Log("頂点の追加");
                                // 交点をもとに頂点を追加
                                //vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * -cpNormalAbs.z));
                                //vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge.normalized.x * 0.04f * cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * cpNormalAbs.z));

                                // インデックスの割り当て
                                int idx0 = attachedMesh.triangles[c];
                                int idx1 = attachedMesh.triangles[c + 1];
                                int idx2 = attachedMesh.triangles[c + 2];
                                int idx3 = vertices1.Count - 4; // 7
                                int idx4 = vertices1.Count - 3; // 
                                int idx5 = vertices1.Count - 2; // 6
                                int idx6 = vertices1.Count - 1; // 6
                                int here = 0;   // 記憶されたインデックスの先頭を格納する変数

                                // 一致した記憶された先頭のインデックスが全体のどのインデックスにいるのか
                                for (int z = 0; z < triangles1.Count; z += 3)
                                {
                                    // 一致しなかったらスルー
                                    if (!(idxMemory[a] == triangles1[z] && idxMemory[a + 1] == triangles1[z + 1] && idxMemory[a + 2] == triangles1[z + 2])) continue;

                                    // 記憶されたインデックスの先頭を格納
                                    here = z - a;
                                }

                                Debug.Log("idxMemory.Count" + idxMemory.Count);
                                // 記憶されたインデックスの数によって分岐
                                // 記憶されたインデックスが12個(ポリゴンが4個)の時
                                if (idxMemory.Count == 12)
                                {
                                    // インデックスの割り振り分岐
                                    if (a == 0)
                                    {
                                        Debug.Log("a = 0");

                                        // インデックスの変更
                                        triangles1[here + 3] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 3)
                                    {
                                        Debug.Log("a = 3");

                                        // カットポイントのあるポリゴンのインデックスの削除&追加
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 6)
                                    {
                                        Debug.Log("a = 6");

                                        // インデックスの変更
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;
                                        triangles1[here + 9] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        // ここまで来たら三角形を二等分するのは終了
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 9)
                                    {
                                        Debug.Log("a = 9");
                                        // インデックスの変更
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        // ここまで来たら三角形を二等分するのは終了
                                        end3 = true;
                                        break;
                                    }
                                }
                                // 記憶されたインデックスが9個(ポリゴンが3個)の時
                                else if (idxMemory.Count == 9)
                                {
                                    // インデックスの割り振り分岐
                                    if (a == 0)
                                    {
                                        Debug.Log("a = 0");

                                        // インデックスの変更
                                        triangles1[here + 3] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 3)
                                    {
                                        Debug.Log("a = 3");

                                        // カットポイントのあるポリゴンのインデックスの削除&追加       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 6)
                                    {
                                        Debug.Log("a = 6");

                                        // インデックスの変更
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }

                                }


                            }

                            // 交点が一個のポリゴンの候補の中から今上で使ったポリゴンを削除
                            //inerPolyIdx.RemoveRange(w, 3);
                            //edgIdx2List.RemoveRange(w/3, 1);
                            //intersectPolyList.RemoveRange(w/3, 1);
                            //intersectEdgList.RemoveRange(w/3, 1);
                            if (end3) break;
                        }
                        if (end3) break;
                    }
                }
                {
                    //// メッシュのポリゴンの数だけループ
                    //for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
                    //{
                    //    //メッシュの3つの頂点を取得
                    //    p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);//+ Vector3.one * 0.0001f;
                    //    p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);//+ Vector3.one * 0.0001f;
                    //    p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]); //+ Vector3.one * 0.0001f;

                    //    // カットポイントの終点がポリゴンの中にあるか
                    //    Vector2 cp = new Vector2(cutPoint[cutPoint.Count - 2].x + (cutPoint[cutPoint.Count - 1].x - cutPoint[cutPoint.Count - 2].x) * 0.40f - transform.position.x, cutPoint[cutPoint.Count - 2].z + (cutPoint[cutPoint.Count - 1].z - cutPoint[cutPoint.Count - 2].z) * 0.40f - transform.position.z);
                    //    var v2P0 = new Vector2(p0.x, p0.z);
                    //    var v2P1 = new Vector2(p1.x, p1.z);
                    //    var v2P2 = new Vector2(p2.x, p2.z);

                    //    double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
                    //    double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * (cp.x + transform.position.x) + (p0.x - p2.x) * (cp.y + transform.position.z));
                    //    double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * (cp.x + transform.position.x) + (p1.x - p0.x) * (cp.y + transform.position.z));
                    //    // 三角形の中にあるか
                    //    if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
                    //    {
                    //        Debug.Log("ポリゴンの中にある");
                    //        //vertices1.Add(new Vector3(cp.x, attachedMesh.vertices[0].y, cp.y));
                    //        // インデックスの割り当て
                    //        int _0 = attachedMesh.triangles[i];
                    //        int _1 = attachedMesh.triangles[i + 1];
                    //        int _2 = attachedMesh.triangles[i + 2];
                    //        int _3 = vertices1.Count - 2; // 7
                    //        int _4 = vertices1.Count - 1; // 使わない  
                    //        int _5 = vertices1.Count - 3; // 6

                    //        // 記憶された三角形インデックスの数だけループ
                    //        for (int j = 0; j < idxMemory.Count; j += 3)
                    //        {
                    //            if (attachedMesh.triangles[i] == idxMemory[j] && attachedMesh.triangles[i + 1] == idxMemory[j + 1] && attachedMesh.triangles[i + 2] == idxMemory[j + 2])
                    //            {
                    //                if (j == 0)
                    //                {
                    //                    Debug.Log("j = 0");
                    //                    // インデックスの変更
                    //                    triangles1[i + j + 3] = _5;

                    //                    // カットポイントのあるポリゴンのインデックスの削除&追加

                    //                    triangles1.RemoveRange(i, 3);

                    //                    // 三角形インデックスの振り分け
                    //                    triangles1.Add(_4);
                    //                    triangles1.Add(_2);
                    //                    triangles1.Add(_5);

                    //                    triangles1.Add(_3);
                    //                    triangles1.Add(_0);
                    //                    triangles1.Add(_1);

                    //                    // 出来た三角形インデックスの保存
                    //                    idxMemory.Clear();

                    //                    idxMemory.Add(_3);
                    //                    idxMemory.Add(_2);
                    //                    idxMemory.Add(_5);

                    //                    idxMemory.Add(_3);
                    //                    idxMemory.Add(_0);
                    //                    idxMemory.Add(_1);
                    //                    break;
                    //                }
                    //                if (j == 3)
                    //                {
                    //                    Debug.Log("j = 3");
                    //                    Debug.Log("え、ここだよね？");

                    //                    triangles1[i + j] = _5;
                    //                    //triangles1[i  - 3] = _5;

                    //                    //triangles1[i + j] = _5;
                    //                    // カットポイントのあるポリゴンのインデックスの削除&追加
                    //                    triangles1.RemoveRange(i, 3);
                    //                    triangles1.Add(_4);
                    //                    triangles1.Add(_2);
                    //                    triangles1.Add(_5);

                    //                    triangles1.Add(_3);
                    //                    triangles1.Add(_0);
                    //                    triangles1.Add(_1);

                    //                    // 出来た三角形インデックスの保存
                    //                    idxMemory.Clear();
                    //                    idxMemory.Add(_4);
                    //                    idxMemory.Add(_2);
                    //                    idxMemory.Add(_5);

                    //                    idxMemory.Add(_3);
                    //                    idxMemory.Add(_0);
                    //                    idxMemory.Add(_1);
                    //                    break;
                    //                }
                    //                if (j == 6)
                    //                {
                    //                    Debug.Log("j = 6");
                    //                    Debug.Log("j = " + j);
                    //                    Debug.Log("j + i = " + (j + i));
                    //                    Debug.Log("2回目");

                    //                    // インデックスの変更

                    //                    triangles1[i - 3] = _5;
                    //                    triangles1[i - 6] = _5;
                    //                    //triangles1[i + j + 3] = _5;

                    //                    // カットポイントのあるポリゴンのインデックスの削除&追加
                    //                    triangles1.RemoveRange(i, 3);
                    //                    triangles1.Add(_3);
                    //                    triangles1.Add(_1);
                    //                    triangles1.Add(_2);

                    //                    triangles1.Add(_3);
                    //                    triangles1.Add(_2);
                    //                    triangles1.Add(_5);

                    //                    triangles1.Add(_3);
                    //                    triangles1.Add(_0);
                    //                    triangles1.Add(_1);

                    //                    // 出来た三角形インデックスの保存
                    //                    idxMemory.Clear();
                    //                    idxMemory.Add(_3);
                    //                    idxMemory.Add(_1);
                    //                    idxMemory.Add(_2);

                    //                    idxMemory.Add(_3);
                    //                    idxMemory.Add(_2);
                    //                    idxMemory.Add(_5);

                    //                    idxMemory.Add(_3);
                    //                    idxMemory.Add(_0);
                    //                    idxMemory.Add(_1);
                    //                    break;
                    //                }
                    //            }

                    //        }

                    //    }

                    //    //Debug.Log("頂点の追加");
                    //    //vertices1.Add(new Vector3(cp.x, attachedMesh.vertices[0].y, cp.y));


                    //}

                }

            }
            // 交点が2個の時(またいでるポリゴンがあるとき)
            else if (straddlePolyIdx.Count > 0)
            {
                Debug.Log("============= 交点が2個の時 =============");
                Debug.Log("straddlePolyIdx.Count" + straddlePolyIdx.Count);
                //--- 変数宣言 ---
                int firstNum = 0;
                int secondNum = 0;
                Vector2 cpS = new Vector2(cutPoint[cutPoint.Count - 2].x - transform.position.x, cutPoint[cutPoint.Count - 2].z - transform.position.z);
                Vector2 cpV = new Vector2(cutPoint[cutPoint.Count - 1].x - transform.position.x, cutPoint[cutPoint.Count - 1].z - transform.position.z);
                Vector2 p = cpV;
                var idxList2 = new List<int>();  // 1個前に分割したインデックスのリスト
                var rastIdxList = new List<int>();  // 2個に分割する処理をした最後のインデックス保存用のリスト             
                // 2分割する処理(最初)
                {
                    Debug.Log("2分割する処理");

                    //--- 記憶された三角形インデクスをもとにインデックスを割り振る ---
                    // 記憶された三角形インデックスの数だけループ
                    for (int a = 0; a < idxMemory.Count; a += 3)
                    {
                        bool end3 = false;
                        // 分割対象のポリゴンの数だけループ
                        for (int w = 0; w < inerPolyIdx.Count; w += 3)
                        {
                            // 記憶されたインデックスと一致しなかったらスルー
                            if (!(inerPolyIdx[w] == idxMemory[a] && inerPolyIdx[w + 1] == idxMemory[a + 1] && inerPolyIdx[w + 2] == idxMemory[a + 2])) continue;
                            Debug.Log("intersectPolyList[w/3][0]:" + intersectPolyList[w / 3][0]);
                            Debug.Log("intersectEdgList[w/3][0]:" + intersectEdgList[w / 3][0]);

                            //--- 変数宣言 ---
                            var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
                            var cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));
                            var pEnd = intersectPolyList[w / 3][0];    // 交点の終点
                            var pEdge = new Vector2(Mathf.Abs(intersectEdgList[w / 3][0].x), Mathf.Abs(intersectEdgList[w / 3][0].y));

                            // 候補に追加
                            idxList2.Add(edgIdx2List[w / 3][0]);
                            idxList2.Add(edgIdx2List[w / 3][1]);

                            // 今追加された候補と交点が二個あるポリゴンの辺と比較
                            for (int g = 0; g < straddlePolyIdx.Count; g += 3)
                            {
                                bool end2 = false;
                                // 辺の数だけループ
                                for (int f = 0; f < 3; f++)
                                {
                                    // 一致しなかったらスルー
                                    if (((straddlePolyIdx[g + f] == idxList2[0] || straddlePolyIdx[g + f] == idxList2[1]) && (straddlePolyIdx[g + (f + 1) % 3] == idxList2[1] || straddlePolyIdx[g + (f + 1) % 3] == idxList2[0]))) continue;

                                    Debug.Log("候補に追加");
                                    firstNum = g;
                                    end2 = true;
                                    break;
                                }
                                if (end2) break;
                            }

                            // メッシュのポリゴンの数だけループ
                            for (int c = 0; c < triangles1.Count; c += 3)
                            {
                                // 一致しなかったらスルー
                                if (!(triangles1[c] == inerPolyIdx[w] && triangles1[c + 1] == inerPolyIdx[w + 1] && triangles1[c + 2] == inerPolyIdx[w + 2])) continue;
                                Debug.Log("頂点の追加");
                                // 交点をもとに頂点を追加
                                //vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * -cpNormalAbs.z));
                                //vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge.normalized.x * 0.04f * cpNormalAbs.x, 0, pEdge.normalized.y * 0.04f * cpNormalAbs.z));

                                // インデックスの割り当て
                                int idx0 = attachedMesh.triangles[c];
                                int idx1 = attachedMesh.triangles[c + 1];
                                int idx2 = attachedMesh.triangles[c + 2];
                                int idx3 = vertices1.Count - 4; // 7
                                int idx4 = vertices1.Count - 3; // 
                                int idx5 = vertices1.Count - 2; // 6
                                int idx6 = vertices1.Count - 1; // 6
                                int here = 0;   // 記憶されたインデックスの先頭を格納する変数

                                // 一致した記憶された先頭のインデックスが全体のどのインデックスにいるのか
                                for (int z = 0; z < triangles1.Count; z += 3)
                                {
                                    // 一致しなかったらスルー
                                    if (!(idxMemory[a] == triangles1[z] && idxMemory[a + 1] == triangles1[z + 1] && idxMemory[a + 2] == triangles1[z + 2])) continue;

                                    // 記憶されたインデックスの先頭を格納
                                    here = z - a;
                                }
                              
                                // 記憶されたインデックスの数によって分岐
                                // 記憶されたインデックスが12個(ポリゴンが4個)の時
                                if (idxMemory.Count == 12)
                                {
                                    // インデックスの割り振り分岐
                                    if (a == 0)
                                    {
                                        Debug.Log("a = 0");

                                        // インデックスの変更
                                        triangles1[here + 3] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 3)
                                    {
                                        Debug.Log("a = 3");

                                        // カットポイントのあるポリゴンのインデックスの削除&追加
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 6)
                                    {
                                        Debug.Log("a = 6");

                                        // インデックスの変更
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;
                                        triangles1[here + 9] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        // ここまで来たら三角形を二等分するのは終了
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 9)
                                    {
                                        Debug.Log("a = 9");
                                        // インデックスの変更
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);

                                        // ここまで来たら三角形を二等分するのは終了
                                        end3 = true;
                                        break;
                                    }
                                }
                                // 記憶されたインデックスが9個(ポリゴンが3個)の時
                                else if (idxMemory.Count == 9)
                                {
                                    // インデックスの割り振り分岐
                                    if (a == 0)
                                    {
                                        Debug.Log("a = 0");

                                        // インデックスの変更
                                        triangles1[here + 3] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 3)
                                    {
                                        Debug.Log("a = 3");

                                        // カットポイントのあるポリゴンのインデックスの削除&追加       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }
                                    if (a == 6)
                                    {
                                        Debug.Log("a = 6");

                                        // インデックスの変更
                                        triangles1[here] = idx4;
                                        triangles1[here + 3] = idx4;

                                        // カットポイントのあるポリゴンのインデックスの削除&追加       
                                        triangles1.RemoveRange(c, 3);
                                        triangles1.Add(idx6);
                                        triangles1.Add(idx2);
                                        triangles1.Add(idx4);

                                        triangles1.Add(idx5);
                                        triangles1.Add(idx3);
                                        triangles1.Add(idx1);
                                        end3 = true;
                                        break;
                                    }

                                }
                            }

                            // 交点が2個のポリゴンの候補の中から今上で使ったポリゴンを削除
                            //inerPolyIdx.RemoveRange(w, 3);
                            //edgIdx2List.RemoveRange(w/3, 1);
                            //intersectPolyList.RemoveRange(w/3, 1);
                            //intersectEdgList.RemoveRange(w/3, 1);
                            if (end3) break;
                        }
                        if (end3) break;
                    }
                }

                Debug.Log("idxList2.Count" + idxList2.Count);

                // 記憶されたインデックスと一致しなかったら
                if (idxList2.Count == 0)
                {
                    Debug.Log("記憶されたインデックスと一致しなかったら");
                    var point = intersectPolyList2[0][0];
                    int first = 0;
                    int second = 0;
                    for (int h = 0; h < straddlePolyIdx.Count; h += 3)
                    {
                        for (int v = 0; v < 2; v++)
                        {
                            if (Vector2.Distance(new Vector2(cutPoint[cutPoint.Count - 1].x, cutPoint[cutPoint.Count - 1].z), point) > Vector2.Distance(new Vector2(cutPoint[cutPoint.Count - 1].x, cutPoint[cutPoint.Count - 1].z), intersectPolyList2[h / 3][v])) continue;
                            point = intersectPolyList2[h / 3][v];
                            first = h;
                            second = v;
                        }
                    }

                    Debug.Log("first / 3 :" + first / 3);
                    Debug.Log("edgIdx2List.Count" + edgIdx2List.Count);
                    Debug.Log("edgIdx2List[first / 3][0]" + edgIdx2List[first / 3][0]);

                    // 始点がどっちにあるかで分岐
                    if (second == 0)
                    {
                        // 候補に追加
                        idxList2.Add(edgIdx2List[first / 3][0]);
                        idxList2.Add(edgIdx2List[first / 3][1]);

                    }
                    else if (second == 1)
                    {
                        // 候補に追加
                        idxList2.Add(edgIdx2List[first / 3][0]);
                        idxList2.Add(edgIdx2List[first / 3][1]);

                    }


                }

                // 2分割する処理(途中)
                {
                    Debug.Log("2分割する処理(途中)");
                    //--- 変数宣言 ---
                    int count = 0;
                    var idxCnt = straddlePolyIdx;
                    //idxCnt.RemoveRange(firstNum * 3, 3); // 候補保存用
                    //Debug.Log("idxCnt.Count" + idxCnt.Count);
                    //Debug.Log("idxList.Count" + idxList2.Count);
                    //Debug.Log("idxCnt.Count" + idxCnt.Count);

                    // 交点が2個あるポリゴンの候補がなくなるかカウントが一定以上になるまでループ
                    while (count < 100 && idxCnt.Count > 0)
                    {
                        Debug.Log("================= ループ ================= ");

                        bool end3 = false;

                        // 候補の数だけループ
                        for (int k = 0; k < idxCnt.Count; k += 3)
                        {
                            // 辺の数だけループ
                            for (int h = 0; h < 3; h++)
                            {
                                Debug.Log("idxCnt[k + h];" + idxCnt[k + h]);
                                Debug.Log("idxCnt[k + ((h + 1) % 3)];" + idxCnt[k + ((h + 1) % 3)]);
                                Debug.Log("idxList[0];" + idxList2[0]);
                                Debug.Log("idxList[1];" + idxList2[1]);

                                // 候補と一致しなかったらスルー、一致したら分割対象のインデックスが分かる
                                if (!((idxCnt[k + h] == idxList2[0] || idxCnt[k + h] == idxList2[1]) && (idxCnt[k + ((h + 1) % 3)] == idxList2[0] || straddlePolyIdx[k + ((h + 1) % 3)] == idxList2[1]))) continue;

                                // 保存された候補リストから今回使ったインデックスを削除
                                for (int g = 0; g < straddlePolyIdx.Count; g += 3)
                                {
                                    // 候補と一致しなかったらスルー
                                    if (!(idxCnt[k] == straddlePolyIdx[g] && idxCnt[k + 1] == straddlePolyIdx[g + 1] && idxCnt[k + 2] == straddlePolyIdx[g + 2])) continue;
                                   
                                    // ポリゴンのインデックスの最初の番号
                                    firstNum = g / 3;

                                    // 変数宣言
                                    var cpNormal = Vector3.Cross((cutPoint[cutPoint.Count - 2] - cutPoint[cutPoint.Count - 1]), Vector3.up);
                                    var cpNormalAbs = new Vector3(cpNormal.x / Mathf.Abs(cpNormal.x), 0, cpNormal.z / Mathf.Abs(cpNormal.z));
                                    var pEnd = intersectPolyList2[firstNum][1];    // 交点の終点
                                    var pEdge = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][secondNum].x), Mathf.Abs(intersectEdgList2[firstNum][secondNum].y));
                                    var pEdge2 = new Vector2();     // 交点の終点の辺ベクトル


                                    Debug.Log("edgIdx2List2[firstNum][0]:" + edgIdx2List2[firstNum][0]);
                                    Debug.Log("edgIdx2List2[firstNum][1]:" + edgIdx2List2[firstNum][1]);
                                    Debug.Log("edgIdx2List2[firstNum][2]:" + edgIdx2List2[firstNum][2]);
                                    Debug.Log("edgIdx2List2[firstNum][3]:" + edgIdx2List2[firstNum][3]);

                                    // どっちが交点の始点か調べる
                                    if ((edgIdx2List2[firstNum][0] == idxList2[0] || edgIdx2List2[firstNum][0] == idxList2[1]) && (edgIdx2List2[firstNum][1] == idxList2[1] || edgIdx2List2[firstNum][1] == idxList2[0]))
                                    {
                                        Debug.Log("頂点の追加");
                                        secondNum = 0;
                                        pEnd = intersectPolyList2[firstNum][1];    // 交点の終点
                                        pEdge2 = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][1].x), Mathf.Abs(intersectEdgList2[firstNum][1].y));// 交点の終点の辺ベクトル                                                      
                                        idxList2.Clear();    // 候補の削除
                                        idxList2.Add(edgIdx2List2[firstNum][2]); // 候補の追加
                                        idxList2.Add(edgIdx2List2[firstNum][3]); // 候補の追加
                                        rastIdxList.Clear();    // 候補の削除
                                        rastIdxList.Add(edgIdx2List2[firstNum][2]);// 候補の追加
                                        rastIdxList.Add(edgIdx2List2[firstNum][3]);// 候補の追加
                                    }
                                    else if ((edgIdx2List2[firstNum][2] == idxList2[0] || edgIdx2List2[firstNum][2] == idxList2[1]) && (edgIdx2List2[firstNum][3] == idxList2[1] || edgIdx2List2[firstNum][3] == idxList2[0]))
                                    {
                                        Debug.Log("頂点の追加");
                                        secondNum = 1;
                                        pEnd = intersectPolyList2[firstNum][0];    // 交点の終点
                                        pEdge2 = new Vector2(Mathf.Abs(intersectEdgList2[firstNum][0].x), Mathf.Abs(intersectEdgList2[firstNum][0].y));// 交点の終点の辺ベクトル                    
                                        idxList2.Clear();    // 候補の削除
                                        idxList2.Add(edgIdx2List2[firstNum][0]); // 候補の追加
                                        idxList2.Add(edgIdx2List2[firstNum][1]); // 候補の追加
                                        rastIdxList.Clear();    // 候補の削除
                                        rastIdxList.Add(edgIdx2List2[firstNum][0]);// 候補の追加
                                        rastIdxList.Add(edgIdx2List2[firstNum][1]);// 候補の追加
                                    }

                                    // 頂点の追加
                                    vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f * -cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f * -cpNormalAbs.z));
                                    vertices1.Add(new Vector3(pEnd.x, attachedMesh.vertices[0].y, pEnd.y) + new Vector3(pEdge2.normalized.x * 0.04f * cpNormalAbs.x, 0, pEdge2.normalized.y * 0.04f * cpNormalAbs.z));

                                    // インデックスの割り当て
                                    int idx0 = straddlePolyIdx[(firstNum * 3)];
                                    int idx1 = straddlePolyIdx[(firstNum * 3) + 1];
                                    int idx2 = straddlePolyIdx[(firstNum * 3) + 2];
                                    int idx3 = vertices1.Count - 4; // 
                                    int idx4 = vertices1.Count - 3; //  
                                    int idx5 = vertices1.Count - 2; // 
                                    int idx6 = vertices1.Count - 1; // 
                                    int removeIdx = -1;

                                    // 削除する三角形の検索
                                    Debug.Log("削除する三角形の探索");
                                    for (int a = 0; a < attachedMesh.triangles.Length; a += 3)
                                    {
                                        if (!(attachedMesh.triangles[a] == idx0 && attachedMesh.triangles[a + 1] == idx1 && attachedMesh.triangles[a + 2] == idx2)) continue;
                                        removeIdx = a;
                                    }

                                    // インデックスの割り振り
                                    if (secondNum == 0)
                                    {
                                        // edgIdx2List2[firstNum][0]、edgIdx2List2[firstNum][1]が始点の交点

                                        // インデックスの削除
                                        triangles1.RemoveRange(removeIdx, 3);

                                        // 01-12インデックス
                                        if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx1)) &&
                                            ((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)))
                                        {
                                            Debug.Log("01-12インデックス");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx1);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx4);
                                        }
                                        // 01-02インデックス
                                        else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)))
                                        {
                                            Debug.Log("01-02インデックス");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx1);

                                        }
                                        // 02-12インデックス
                                        else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)))
                                        {
                                            Debug.Log("02-12インデックス");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx1);
                                        }
                                        // 02-01インデックス
                                        else if (((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx1 || edgIdx2List2[firstNum][3] == idx0)))
                                        {
                                            Debug.Log("02-01インデックス");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx4);


                                        }
                                        // 12-01インデックス
                                        else if (((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx1 || edgIdx2List2[firstNum][3] == idx0)))
                                        {
                                            Debug.Log("12-01インデックス");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx0);
                                        }
                                        // 12-20インデックス
                                        else if (((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][2] == idx2 || edgIdx2List2[firstNum][2] == idx0) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx2)))
                                        {
                                            Debug.Log(" 12-02インデックス");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx1);
                                        }

                                    }
                                    else if (secondNum == 1)
                                    {
                                        // インデックスの削除
                                        triangles1.RemoveRange(removeIdx, 3);

                                        // 01-12インデックス
                                        if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)))
                                        {
                                            Debug.Log("01-12インデックス");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx1);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx4);
                                        }
                                        // 01-02インデックス
                                        else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx1) && (edgIdx2List2[firstNum][3] == idx0 || edgIdx2List2[firstNum][3] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx0)))
                                        {
                                            Debug.Log("01-02インデックス");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx1);

                                        }
                                        // 02-12インデックス
                                        else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx1 || edgIdx2List2[firstNum][0] == idx2) && (edgIdx2List2[firstNum][1] == idx2 || edgIdx2List2[firstNum][1] == idx1)))
                                        {
                                            Debug.Log("02-12インデックス");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx1);
                                        }
                                        // 02-01インデックス
                                        else if (((edgIdx2List2[firstNum][2] == idx0 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx0)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx1 || edgIdx2List2[firstNum][1] == idx0)))
                                        {
                                            Debug.Log("02-01インデックス");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx0);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx4);


                                        }
                                        // 12-01インデックス
                                        else if (((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx0 || edgIdx2List2[firstNum][0] == idx1) && (edgIdx2List2[firstNum][1] == idx1 || edgIdx2List2[firstNum][1] == idx0)))
                                        {
                                            Debug.Log("12-01インデックス");
                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx5);
                                            triangles1.Add(idx2);
                                            triangles1.Add(idx0);
                                        }
                                        // 12-20インデックス
                                        else if (((edgIdx2List2[firstNum][2] == idx1 || edgIdx2List2[firstNum][2] == idx2) && (edgIdx2List2[firstNum][3] == idx2 || edgIdx2List2[firstNum][3] == idx1)) &&
                                                 ((edgIdx2List2[firstNum][0] == idx2 || edgIdx2List2[firstNum][0] == idx0) && (edgIdx2List2[firstNum][1] == idx0 || edgIdx2List2[firstNum][1] == idx2)))
                                        {
                                            Debug.Log(" 12-02インデックス");
                                            triangles1.Add(idx5);
                                            triangles1.Add(idx3);
                                            triangles1.Add(idx2);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx1);
                                            triangles1.Add(idx4);

                                            triangles1.Add(idx6);
                                            triangles1.Add(idx0);
                                            triangles1.Add(idx1);
                                        }


                                    }

                                    // 候補から削除
                                    idxCnt.RemoveRange(k, 3);
                                    intersectPolyList2.RemoveAt(firstNum);
                                    intersectEdgList2.RemoveAt(firstNum);
                                    edgIdx2List2.RemoveAt(firstNum);

                                    // ここまで来たら終了
                                    end3 = true;
                                    break;
                                }
                                if (end3) break;
                            }

                            if (end3) break;
                        }

                        // カウント++
                        count++;
                    }
                }
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

        // --- 別々のオブジェクトに分ける処理 ---

        // 変数宣言
        int idx = 0;    // 探索させるインデックス
        var idxList = new List<int>();       // 探索させるインデックスのリスト
        //int cnt = 0;
        bool addFlg = false;    // 追加フラグ
        bool end = false;   // 終了フラグ
        var vertices2 = new List<Vector3>();   // 頂点
        var triangles2 = new List<int>();       // 三角形インデックス
        var removeList = new List<int>();       // 消す用のリスト
        var normals2 = new List<Vector3>();     // 法線

        var vtxRemove = new List<int>();

        // まず格納
        idxList.Add(0);
        triangles2.Add(triangles1[idxList[0]]);
        triangles2.Add(triangles1[idxList[0] + 1]);
        triangles2.Add(triangles1[idxList[0] + 2]);
        //for (int l = 0; l < 3; l++)
        //{
        //    bool through = false;   // スルーフラグ

        //    // すでに消してある頂点のインデックスだったらスルー
        //    for (int g = 0; g < vtxRemove.Count; g++)
        //    {
        //        if (vtxRemove[g] != triangles1[0 + l]) continue;

        //        through = true; // スルーフラグON
        //        break;
        //    }
        //    // 次の探索へ
        //    if (through) continue;

        //    vertices1.Remove(vertices1[triangles1[0 + l]]); // 頂点の削除
        //    vtxRemove.Add(triangles1[0+ l]);   // インデックス番号の保存                                  
        //    Debug.Log("triangles1[i + l]" + triangles1[0 + l]);
           
        //    // インデックスの変更
        //    for (int d = 0; d < triangles1.Count; d++)
        //    {
        //        if (d == triangles1[0 + l]) continue;
        //        if (triangles1[d] < triangles1[0 + l]) continue;

        //        triangles1[d] -= 1;

        //    }
          
        //}

        //Debug.Log("triangle2:" + triangles2[idx] + "" + triangles2[idx + 1] + "" + triangles2[idx + 2]);



        // 同じ辺があるかを探索
        while (!end)
        {
            // 同じ辺が存在しているか変数
            bool Existence = false;

            // 追加フラグOFF
            addFlg = false;

            // ポリゴンの数だけループ
            for (int i = 0; i < triangles1.Count; i += 3)
            {
                Existence = false;
                // インデックスとiが同じだったらスルー
                //if (idx == i) continue;

                // 三角形は3辺あるので3ループ
                for (int k = 0; k < 3; k++)
                {
                    // 1辺に対して3辺調べるので3ループ
                    for (int j = 0; j < 3; j++)
                    {
                        // 同じ辺があるか
                        if ((triangles1[idxList[0] + k] == triangles1[i + j] && triangles1[idxList[0] + (k + 1) % 3] == triangles1[i + (j + 1) % 3]) ||
                            (triangles1[idxList[0] + k] == triangles1[i + (j + 1) % 3] && triangles1[idxList[0] + (k + 1) % 3] == triangles1[i + j]))
                        {
                            //Debug.Log("同じ辺があるとき");
                            //Debug.Log("ヒットしたポリゴン番号:" + triangles1[i] + "" + triangles1[i + 1] + "" + triangles1[i + 2]);
                            //Debug.Log("idxのポリゴン番号:" + triangles1[idx] + "" + triangles1[idx + 1] + "" + triangles1[idx + 2]);
                            ////Debug.Log("triangle1[idx + k]:" + triangles1[idx + k] + "" + triangles1[(idx + k + 1) % 3]);
                            //Debug.Log("triangles1[i + j]:" + triangles1[i + j] + "" + triangles1[i + (j + 1) % 3]);
                            //Debug.Log("triangles1[i + (j + 1) % 3]:" + triangles1[i + (j + 1) % 3] + "" + triangles1[i + j]);

                            //Debug.Log("triangles2.Count:" + triangles2.Count);


                            //// 同じ辺があるとき
                            // それがすでに格納済みかどうか調べる                         
                            for (int l = 0; l < triangles2.Count; l+=3)
                            {
                                // 追加したいインデックスと追加先のインデックスとの比較
                                // なかったらスルー
                                if (!(triangles2[l] == triangles1[i] && triangles2[l + 1] == triangles1[i + 1] && triangles2[l + 2] == triangles1[i + 2])) continue;

                                // 存在している
                                Existence = true;
                                break;// ループ終了
                            }

                            // 存在したら次の探索へ
                            if (Existence)
                            {
                                break;
                            }
                            else
                            {
                                //Debug.Log("存在しなかったら格納");
                                // 存在しなかったら格納
                                triangles2.Add(triangles1[i]);
                                triangles2.Add(triangles1[i + 1]);
                                triangles2.Add(triangles1[i + 2]);

                                // 頂点の削除
                                //Debug.Log("triangles1[i]" + triangles1[i]);
                                //Debug.Log("triangles1[i+1]" + triangles1[i + 1]);
                                //Debug.Log("triangles1[i+2]" + triangles1[i + 2]);
                                //Debug.Log("vertices1.Count" + vertices1.Count);
                                //Debug.Log("vertices1.Remove(vertices1[triangles1[i + l]])" + vertices1.Remove(vertices1[triangles1[i]]));
                                
                                // インデックスの変更
                                //for (int l = 0; l < 3; l++)
                                //{
                                //    bool through = false;   // スルーフラグ

                                //    // すでに消してある頂点のインデックスだったらスルー
                                //    for (int g = 0;g < vtxRemove.Count;g++)
                                //    {
                                //        if (vtxRemove[g] != triangles1[i + l]) continue;

                                //        through = true; // スルーフラグON
                                //        break;
                                //    }
                                //    // 次の探索へ
                                //    if (through) continue;

                                //    //Debug.Log("vertices1.Remove(vertices1[triangles1[i + l]]):" + vertices1.Remove(vertices1[triangles1[i + l]]));
                                //    vertices1.Remove(vertices1[triangles1[i + l]]); // 頂点の削除
                                //    vtxRemove.Add(triangles1[i + l]);   // インデックス番号の保存                                  
                                //    Debug.Log("triangles1[i + l]" + triangles1[i + l]);
                                //    //Debug.Log("変更前");
                                //    //Debug.Log("triangles1[i]" + triangles1[i]);
                                //    //Debug.Log("triangles1[i+1]" + triangles1[i + 1]);
                                //    //Debug.Log("triangles1[i+2]" + triangles1[i + 2]);
                                //    // インデックスの変更
                                //    for (int d = 0; d < triangles1.Count; d++)
                                //    {
                                //        if (triangles1[d] <= triangles1[i + l]) continue;
                                      
                                //        triangles1[d] -= 1;
                                      
                                //    }
                                //    //Debug.Log("変更後");
                                //    //Debug.Log("triangles1[i]" + triangles1[i]);
                                //    //Debug.Log("triangles1[i+1]" + triangles1[i + 1]);
                                //    //Debug.Log("triangles1[i+2]" + triangles1[i + 2]);
                                //}


                                // 次の探索へ
                                idxList.Add(i);
                                //idx = i ;    // 次の探索のポリゴンの最初のインデックス
                                addFlg = true;
                                break;
                            }
                        }


                    }

                    // 存在したら次の探索へ
                    if (Existence)
                    {
                        break;
                    }
                }

                // 追加したら次の探索へ
                if (addFlg)
                {
                    continue;
                }

                // 存在したら次の探索へ
                if (Existence)
                {
                    continue;
                }

            }

            // もう一方のリストからは削除
            removeList.Add(idxList[0]);

            // 探索しきったインデックスを消す
            idxList.RemoveAt(0);

            // もし探索したいインデックスがなくなったら終了
            if (idxList.Count == 0)
            {
                //Debug.Log("お腹減った");
                // endフラグon
                end = true;
            }

        }

        //Debug.Log("removeList.Count" + removeList.Count);

        // triangles1に入ってたtriangles2を消す
        for (int i = 0; i < removeList.Count; i++)
        {
            triangles1.RemoveRange(removeList[i], 3);
            triangles1.Insert(removeList[i], 0);
            triangles1.Insert(removeList[i], 0);
            triangles1.Insert(removeList[i], 0);
        }

        // triangles1をもとにvertices1を生成し、それをもとにtriangles1を上書き
        for (int i = 0; i < triangles1.Count; i++)
        {
            vertices1.Add(attachedMesh.vertices[triangles1[i]]);
            for (int j = 0; j < vertices1.Count; j++)
            {
                // 追加した頂点が重複してなかったらそのまま終了
                if (j == vertices1.Count - 1)
                {
                    // インデックスの上書き
                    triangles1[i] = vertices1.Count - 1;
                    break;
                }
                // 追加した頂点が重複してたら追加した頂点を消す
                if (vertices1[j] == attachedMesh.vertices[triangles1[i]])
                {
                    // 頂点削除
                    vertices1.RemoveAt(vertices1.Count - 1);

                    // インデックスの上書き
                    triangles1[i] = j;
                    break;
                }
            }

        }

        // 重複するインデックスと頂点を削除
        for(int b = 0;b < triangles1.Count;b += 3)
        {
            // インデクスが3個とも違ったらスルー
            if (!(triangles1[b] == triangles1[b + 1] && triangles1[b] == triangles1[b + 2])) continue;
            //Debug.Log("triangles1[b]" + triangles1[b]);
            //Debug.Log("triangles1[b+1]" + triangles1[b+1]);
            //Debug.Log("triangles1[b+2]" + triangles1[b+2]);
            triangles1.RemoveRange(b, 3);
            //Debug.Log("vertices1[triangles1[b]]" + vertices1[triangles1[b]]);
            //vertices1.RemoveAt(triangles1[b]);
            b = 0;
        }

        normals1.Clear();
        // ノーマルの設定       
        for (int i = 0; i < vertices1.Count; i++)
        {
            normals1.Add(Vector3.up);
        }

        // triangles2をもとにvertices2を生成し、それをもとにtriangles2を上書き
        for (int i = 0; i < triangles2.Count; i++)
        {
            vertices2.Add(attachedMesh.vertices[triangles2[i]]);
            for (int j = 0; j < vertices2.Count; j++)
            {
                // 追加した頂点が重複してなかったらそのまま終了
                if (j == vertices2.Count - 1)
                {
                    // インデックスの上書き
                    triangles2[i] = vertices2.Count - 1;
                    break;
                }
                // 追加した頂点が重複してたら追加した頂点を消す
                if (vertices2[j] == attachedMesh.vertices[triangles2[i]])
                {
                    // 頂点削除
                    vertices2.RemoveAt(vertices2.Count - 1);

                    // インデックスの上書き
                    triangles2[i] = j;
                    break;
                }
            }

        }



        // ノーマルの設定       
        for (int i = 0; i < vertices2.Count; i++)
        {
            normals2.Add(Vector3.up);
        }

        //カット後のオブジェクト生成、いろいろといれる
        GameObject obj = new GameObject("Plane", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody), typeof(MeshDivision2), typeof(Ground));
        var mesh = new Mesh();
        mesh.vertices = vertices1.ToArray();
        mesh.triangles = triangles1.ToArray();
        //mesh.uv = uvs1.ToArray();
        mesh.normals = normals1.ToArray();
        obj.GetComponent<MeshFilter>().mesh = mesh;
        obj.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        obj.GetComponent<MeshCollider>().sharedMesh = mesh;
        obj.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
        //obj.GetComponent<MeshCollider>().convex = false;
        obj.GetComponent<MeshCollider>().material = GetComponent<Collider>().material;
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.transform.localScale = transform.localScale;
        //obj.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        //obj.GetComponent<Rigidbody>().angularVelocity = GetComponent<Rigidbody>().angularVelocity;
        obj.GetComponent<Rigidbody>().isKinematic = true;   // 運動を無効化 

        GameObject obj2 = new GameObject("Plane", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody), typeof(MeshDivision2), typeof(Ground));
        var mesh2 = new Mesh();
        mesh2.vertices = vertices2.ToArray();
        mesh2.triangles = triangles2.ToArray();
        Debug.Log("vertices2.Count" + vertices2.Count);
        mesh2.normals = normals2.ToArray();
        obj2.GetComponent<MeshFilter>().mesh = mesh2;
        obj2.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        obj2.GetComponent<MeshCollider>().sharedMesh = mesh2;
        obj2.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
        //obj2.GetComponent<MeshCollider>().convex = false;
        obj2.GetComponent<MeshCollider>().material = GetComponent<Collider>().material;
        obj2.transform.position = transform.position;
        obj2.transform.rotation = transform.rotation;
        obj2.transform.localScale = transform.localScale;
        //obj2.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        //obj2.GetComponent<Rigidbody>().angularVelocity = GetComponent<Rigidbody>().angularVelocity;
        obj2.GetComponent<Rigidbody>().isKinematic = true;   // 運動を無効化 

        Debug.Log("triangles2:" + triangles2.Count);
        Debug.Log("vertices2:" + vertices2.Count);
        Debug.Log("つらたん");

        // 面積の計算
        float s1 = 0;   // 面積1
        float s2 = 0;   // 面積1

        // 面積1の計算
        for (int i = 0; i < triangles1.Count; i += 3)
        {
            s1 += CalculateArea1(new Vector2(vertices1[triangles1[i]].x, vertices1[triangles1[i]].z), new Vector2(vertices1[triangles1[i + 1]].x, vertices1[triangles1[i + 1]].z), new Vector2(vertices1[triangles1[i + 2]].x, vertices1[triangles1[i + 2]].z));
        }

        // 面積2の計算
        for (int i = 0; i < triangles2.Count; i += 3)
        {
            s2 += CalculateArea1(new Vector2(vertices2[triangles2[i]].x, vertices2[triangles2[i]].z), new Vector2(vertices2[triangles2[i + 1]].x, vertices2[triangles2[i + 1]].z), new Vector2(vertices2[triangles2[i + 2]].x, vertices2[triangles2[i + 2]].z));
        }

        // 体積の比較(ここで体積が軽い方を落としています)
        if (s1 < s2)
        {
            obj2.GetComponent<Rigidbody>().useGravity = false;   // 重力の無効化
            obj2.GetComponent<Rigidbody>().isKinematic = true;   // 運動を無効化 
            obj.GetComponent<Renderer>().material.color = Color.gray;
            obj.GetComponent<Ground>().StartFadeOut();
            obj.GetComponent<Rigidbody>().mass = 0.5f;
            obj.GetComponent<Rigidbody>().drag = 7.0f;

        }
        else
        {
            obj.GetComponent<Rigidbody>().useGravity = false;   // 重力の無効化
            obj.GetComponent<Rigidbody>().isKinematic = true;   // 運動を無効化 
            obj2.GetComponent<Renderer>().material.color = Color.gray;
            obj2.GetComponent<Ground>().StartFadeOut();
            obj2.GetComponent<Rigidbody>().mass = 0.5f;
            obj2.GetComponent<Rigidbody>().drag = 7.0f;
        }

        //このオブジェクトをデストロイ
        Destroy(gameObject);

        // メッシュに代入
        attachedMesh.SetVertices(vertices1.ToArray());
        attachedMesh.SetTriangles(triangles1.ToArray(), 0);
        attachedMesh.SetNormals(normal);

    }

    // 面積の計算
    private float CalculateArea1(Vector2 A, Vector2 B, Vector2 C)
    {
        float S, s;
        float a = Vector2.Distance(B, C);
        float b = Vector2.Distance(A, C);
        float c = Vector2.Distance(A, B);

        s = (a + b + c) / 2;
        S = Mathf.Sqrt(s * (s - a) * (s - b) * (s - c));

        return S;
    }

    // 体積の計算
    public float VolumeOfMesh(Mesh mesh)
    {
        if (mesh == null) return 0;

        Vector3[] vertics = mesh.vertices;
        int[] triangles = mesh.triangles;

        float volume = 0;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertics[triangles[i + 0]];
            Vector3 p2 = vertics[triangles[i + 1]];
            Vector3 p3 = vertics[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);

        }


        return Mathf.Abs(volume);
    }

    // 体積の計算で使う関数
    public float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }
    // ギズモの表示
    private void OnDrawGizmos()
    {
        if (!attachedMesh) return;


        for (int i = 0; i < attachedMesh.vertices.Length; i++)
        {
            Gizmos.color = new Color(25, 0, 0, 1);   // 色の指定
            Gizmos.DrawSphere(attachedMesh.vertices[i] + transform.position, 0.005f);
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
