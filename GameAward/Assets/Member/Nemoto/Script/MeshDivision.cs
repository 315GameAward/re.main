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
        var crossVertices = new List<DVector3>();

        //カットしたいオブジェクトのメッシュをトライアングルごとに処理
        for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
        {
            //メッシュの3つの頂点を取得
            p0 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]);
            p1 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);
            p2 = transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]);

            // カットポイントの始点がこのポリゴンの中にあるか
            double Area = 0.5 * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
            double s = 1 / (2 * Area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * cutPoint[cpNum].x + (p0.x - p2.x) * cutPoint[cpNum].z);
            double t = 1 / (2 * Area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * cutPoint[cpNum].x + (p1.x - p0.x) * cutPoint[cpNum].z);
            if ((0 <= s && s <= 1) && (0 <= t && t <= 1) && (0 <= 1 - s - t && 1 - s - t <= 1))
            {
                // ポリゴンの中にカットポイントの始点があったとき

                // 頂点の追加
                vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i]]);
                vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + 1]]);
                vertices1.Add(attachedMesh.vertices[attachedMesh.triangles[i + 2]]);
                vertices1.Add(cutPoint[cpNum] - transform.position);

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
    }

    // ギズモの表示
    private void OnDrawGizmos()
    {
        if (!attachedMesh) return;
        for (int i = 0; i < attachedMesh.vertices.Length; i++)
        {
            Gizmos.color = new Color(25, 0, 0, 1);   // 色の指定
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
