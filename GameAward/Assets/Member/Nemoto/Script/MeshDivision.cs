//======================================================
//
//        MeshDivision.cs
//        メッシュを分割する処理
//
//------------------------------------------------------
//      作成者:根本龍之介
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDivision : MonoBehaviour
{
    // メッシュの変数
    private MeshFilter attachedMeshFilter;
    private Mesh attachedMesh;
    private bool coliBool = false;
    private double delta = 0.000000001f;
    private float skinWidth = 0.05f;

    public List<Vector3> meshVertices;

    public static List<Vector3> CutPoint1;  // カットポイント格納用

    public static List<int> CrossNum;

    // Start is called before the first frame update
    void Start()
    {
        // メッシュのアタッチ
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;
        CrossNum = new List<int>();
        //CrossNum.Clear();
        CutPoint1 = new List<Vector3>();
        CutPoint1.Clear();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // メッシュを分割する処理
    public void DivisionMesh(List<Vector3> cutPoint,int cpNum)
    {
        // メッシュのアタッチ
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;

        // 変数
        Vector3 p0, p1, p2;    // メッシュのポリゴンの頂点
        var uvs1 = new List<Vector2>(); // テクスチャ
        var uvs2 = new List<Vector2>();
        var vertices1 = new List<Vector3>();   // 頂点
        var vertices2 = new List<DVector3>();
        var triangles1 = new List<int>();       // 三角形インデックス
        var triangles2 = new List<int>();
        var normals1 = new List<Vector3>();     // 法線
        var normals2 = new List<Vector3>();

        var crossVertices = new List<Vector3>();

        // カットポイントの格納
        CutPoint1.Add(cutPoint[cpNum]);
        //Debug.Log("カットポイントの個数だにゃ"+ CutPoint1.Count);

        CrossNum.Add(1);
        Debug.Log("カットポイントの個数だにゃ"+CrossNum.Count);


        //カットしたいオブジェクトのメッシュをトライアングルごとに処理
        for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
        {
            //メッシュの3つの頂点を取得
            p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);
            p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);
            p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]);

            // カットポイントがこのポリゴンの中にあるか
            double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
            double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[cpNum].x + (p0.x - p2.x) * cutPoint[cpNum].z);
            double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[cpNum].x + (p1.x - p0.x) * cutPoint[cpNum].z);
            if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
            {
                // ポリゴンの中にカットポイントの始点があったとき

                

                // 一個前のカットポイントがこのポリゴンの中にあるか
                if (cpNum > 0)  // カットポイントが0番目じゃなかったら
                {
                    double _s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[cpNum - 1].x + (p0.x - p2.x) * cutPoint[cpNum - 1].z);
                    double _t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[cpNum - 1].x + (p1.x - p0.x) * cutPoint[cpNum - 1].z);

                    // 一個前のカットポイントがこのポリゴンの中に入っていたら
                    if ((0 <= _s && _s <= 1) && (0 <= _t && _t <= 1) && (0 <= 1 - _s - _t && 1 - _s - _t <= 1))
                    {  
                        // 頂点の追加
                        vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i]]);
                        vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);
                        vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + 2]]);
                        vertices1.Add(cutPoint[cpNum] - transform.position);
                        CrossNum.Add(vertices1.Count - 1);

                        // 頂点のインデックス
                        int _0 = vertices1.Count - 4;
                        int _1 = vertices1.Count - 3;
                        int _2 = vertices1.Count - 2;
                        int _3 = vertices1.Count - 1;

                        // 三角形インデクスの追加
                        //triangles1
                        if (t < 0.001f)
                        {
                            triangles1.Add(_0);
                            triangles1.Add(_3);
                            triangles1.Add(_2);

                            triangles1.Add(_3);
                            triangles1.Add(_1);
                            triangles1.Add(_2);
                            //Debug.Log("t");
                        }
                        else if (s < 0.001f)
                        {

                            triangles1.Add(_1);
                            triangles1.Add(_3);
                            triangles1.Add(_2);

                            triangles1.Add(_0);
                            triangles1.Add(_3);
                            triangles1.Add(_2);
                            //Debug.Log("s ");
                        }
                        else if (s + t > 0.98f)
                        {
                            triangles1.Add(_0);
                            triangles1.Add(_1);
                            triangles1.Add(_3);

                            triangles1.Add(_0);
                            triangles1.Add(_3);
                            triangles1.Add(_2);
                            //Debug.Log("s + t:");
                        }
                        else
                        {
                            triangles1.Add(_0);
                            triangles1.Add(_1);
                            triangles1.Add(_3);

                            triangles1.Add(_0);
                            triangles1.Add(_3);
                            triangles1.Add(_2);

                            triangles1.Add(_3);
                            triangles1.Add(_1);
                            triangles1.Add(_2);
                        }
                    }                   
                    else
                    {// 一個前のカットポイントがこのポリゴンの中になかったら
                        // とりあえず今のカットポイントでポリゴンの分割
                        
                        // 頂点の追加
                        vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i]]);
                        vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);
                        vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + 2]]);
                        vertices1.Add(cutPoint[cpNum] - transform.position);
                        CrossNum.Add(vertices1.Count - 1);

                        // 頂点のインデックス
                        int _0 = vertices1.Count - 4;
                        int _1 = vertices1.Count - 3;
                        int _2 = vertices1.Count - 2;
                        int _3 = vertices1.Count - 1;

                        // 三角形インデクスの追加
                        //triangles1
                        if (t < 0.001f)
                        {
                            triangles1.Add(_0);
                            triangles1.Add(_3);
                            triangles1.Add(_2);

                            triangles1.Add(_3);
                            triangles1.Add(_1);
                            triangles1.Add(_2);
                            //Debug.Log("t");
                        }
                        else if (s < 0.001f)
                        {

                            triangles1.Add(_1);
                            triangles1.Add(_3);
                            triangles1.Add(_2);

                            triangles1.Add(_0);
                            triangles1.Add(_3);
                            triangles1.Add(_2);
                            //Debug.Log("s ");
                        }
                        else if (s + t > 0.98f)
                        {
                            triangles1.Add(_0);
                            triangles1.Add(_1);
                            triangles1.Add(_3);

                            triangles1.Add(_0);
                            triangles1.Add(_3);
                            triangles1.Add(_2);
                            //Debug.Log("s + t:");
                        }
                        else
                        {
                            triangles1.Add(_0);
                            triangles1.Add(_1);
                            triangles1.Add(_3);

                            triangles1.Add(_0);
                            triangles1.Add(_3);
                            triangles1.Add(_2);

                            triangles1.Add(_3);
                            triangles1.Add(_1);
                            triangles1.Add(_2);
                        }
                        
                        
                        // 今のカットポイントと一個前のカットポイントとの線分と、ポリゴンの辺(線分)との交点を作る処理
                        for (int j = 0;j < attachedMesh.triangles.Length;j+= 3)
                        {
                            // ポリゴンの頂点(三角形なので3)の数だけまわす
                            for(int k = 0;k < 3;k++)
                            {
                                // ポリゴン用変数
                                int idx_s = attachedMesh.triangles[j + k];  // 始点
                                int idx_v = attachedMesh.triangles[j + ((k + 1)%3)];  // 終点

                                // このポリゴンの辺の始点と終点のベクトル
                                Vector2 vtx_s = new Vector2(attachedMesh.vertices[idx_s].x + transform.position.x, attachedMesh.vertices[idx_s].z + transform.position.z);    // 始点のベクトル
                                Vector2 vtx_v = new Vector2(attachedMesh.vertices[idx_v].x + transform.position.x, attachedMesh.vertices[idx_v].z + transform.position.z);    // 終点のベクトル

                                // 今のカットポイントと一個前のカットポイントとの線分と、ポリゴンの辺(線分)の始点をつないだベクトル
                                Vector2 v = new Vector2(vtx_s.x - cutPoint[cpNum - 1].x, vtx_s.y - cutPoint[cpNum - 1].z);

                                // 線分
                                Vector2 v1 = new Vector2(cutPoint[cpNum].x - cutPoint[cpNum - 1].x, cutPoint[cpNum].z - cutPoint[cpNum - 1].z); // カットポイントの線分
                                Vector2 v2 = new Vector2(vtx_v.x - vtx_s.x, vtx_v.y - vtx_s.y); // ポリゴンの線分

                                // 線分の始点から交点のベクトル
                                float t1 = (v.x * v2.y - v2.x * v.y) / (v1.x * v2.y - v2.x * v1.y);
                                float t2 = (v.x * v1.y - v1.x * v.y) / (v1.x * v2.y - v2.x * v1.y);

                                // 交点
                                Vector2 p = new Vector2(vtx_s.x, vtx_v.y) + new Vector2(v2.x * t2, v2.y * t2);

                                // 線分と線分が交わっているか
                                const float eps = 0.00001f;
                                if (t1 + eps < 0 || t1 - eps > 1 || t2 + eps < 0 || t2 - eps > 1)
                                {
                                    // Debug.Log("交差してない");
                                }
                                else
                                {
                                    //Debug.Log("交点を追加");
                                    // 頂点に交点を追加
                                    vertices1.Add(new Vector3(p.x + transform.position.x, attachedMesh.vertices[attachedMesh.triangles[i]].y, p.y + transform.position.z));


                                }
                            }
                        }
                        
                    }
                }               
                else if(cpNum == 0)    // カットポイントが0番目だったら
                {    // カットポイントが0番目だったら

                   
                    // 頂点の追加
                    vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i]]);
                    vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);
                    vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + 2]]);
                    vertices1.Add(cutPoint[cpNum] - transform.position);
                    CrossNum.Add(vertices1.Count - 1);

                    // 頂点のインデックス
                    int _0 = vertices1.Count - 4;
                    int _1 = vertices1.Count - 3;
                    int _2 = vertices1.Count - 2;
                    int _3 = vertices1.Count - 1;

                    // 三角形インデクスの追加
                    //triangles1
                    if (t < 0.001f)
                    {
                        triangles1.Add(_0);
                        triangles1.Add(_3);
                        triangles1.Add(_2);

                        triangles1.Add(_3);
                        triangles1.Add(_1);
                        triangles1.Add(_2);
                        Debug.Log("t");
                    }
                    else if (s < 0.001f)
                    {

                        triangles1.Add(_1);
                        triangles1.Add(_3);
                        triangles1.Add(_2);

                        triangles1.Add(_0);
                        triangles1.Add(_3);
                        triangles1.Add(_2);
                        Debug.Log("s ");
                    }
                    else if (s + t > 0.98f)
                    {
                        triangles1.Add(_0);
                        triangles1.Add(_1);
                        triangles1.Add(_3);

                        triangles1.Add(_0);
                        triangles1.Add(_3);
                        triangles1.Add(_2);
                        Debug.Log("s + t:");
                    }
                    else
                    {
                        triangles1.Add(_0);
                        triangles1.Add(_1);
                        triangles1.Add(_3);

                        triangles1.Add(_0);
                        triangles1.Add(_3);
                        triangles1.Add(_2);

                        triangles1.Add(_3);
                        triangles1.Add(_1);
                        triangles1.Add(_2);
                    }
                }
                
            }
            else
            {
                // ポリゴンの中にカットポイントの始点がなかった時              
                for (int k = 0; k < 3; k++)
                {
                    vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + k]]);
                    //uvs1.Add(attachedMesh.uv[attachedMesh.triangles[i + k]]);
                    //normals1.Add(attachedMesh.normals[attachedMesh.triangles[i + k]]);
                    triangles1.Add(vertices1.Count - 1);
                }
            }


        }


        // 分割後のオブジェクト生成、いろいろといれる
        GameObject obj = new GameObject("DivisionPlane"+ cpNum, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody), typeof(MeshDivision));
        var mesh = new Mesh();
        mesh.vertices = vertices1.ToArray();
        mesh.triangles = triangles1.ToArray();
        //mesh.uv = uvs1.ToArray();
        //mesh.normals = normals1.ToArray();
        obj.GetComponent<MeshFilter>().mesh = mesh;
        obj.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        obj.GetComponent<MeshCollider>().sharedMesh = mesh;
        obj.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
        obj.GetComponent<MeshCollider>().convex = false;
        obj.GetComponent<MeshCollider>().material = GetComponent<Collider>().material;
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.transform.localScale = transform.localScale;
        //obj.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        //obj.GetComponent<Rigidbody>().angularVelocity = GetComponent<Rigidbody>().angularVelocity;
        //obj.GetComponent<MeshCut>().skinWidth = skinWidth;

       
        obj.GetComponent<Rigidbody>().useGravity = false;   // 重力の無効化
        obj.GetComponent<Rigidbody>().isKinematic = true;   // 運動を無効化 

        

        //このオブジェクトをデストロイ
        Destroy(gameObject);

    }

    // メッシュを切る処理(仮)
    public void CutMesh()
    {
        gameObject.name = "Plane";  // 名前の変更

        // カットポイントの削除
        //CutPoint.Clear();
    }

    // ギズモの表示
    private void OnDrawGizmos()
    {
        if (!attachedMesh) return;
        for (int i = 0; i < attachedMesh.vertices.Length; i++)
        {
            if(CrossNum.Count > 0)
            {
                for (int j = 0; j < CrossNum.Count; j++)
                {
                    if (CrossNum[j] == i)
                    {
                        Gizmos.color = new Color(0, 0, 225, 1);   // 色の指定
                        
                    }
                    else
                    {
                        //Gizmos.color = new Color(25, 0, 0, 1);   // 色の指定
                    }

                }
            }
            else
            {
                Gizmos.color = new Color(25, 0, 0, 1);   // 色の指定

            }
            
            Gizmos.DrawSphere(attachedMesh.vertices[i] + transform.position, 0.05f);  // 球の表示

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
