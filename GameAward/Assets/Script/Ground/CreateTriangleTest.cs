using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]


public class CreateTriangleTest : MonoBehaviour
{
    // 三角形を作るための変数
    class Triangle
    {
        public int[] idx = new int[3];
        public Triangle[] edgeLink = new Triangle[3];
    };

    Mesh mesh;
    MeshFilter filter;
    Triangle current;
    List<Vector3> vtx = new List<Vector3>();
    List<int> idx = new List<int>();
    List<Vector3> normal = new List<Vector3>();

    Triangle gizmoTri;

    // Start is called before the first frame update
    void Start()
    {
        CreateTriangle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ギズモの描画
    private void OnDrawGizmos()
    {
        if (!mesh)
        {
            return;
        }

        //Debug.Log("テスト用三角形のギズモ表示中");

        Gizmos.color = new Color(0, 1, 0, 1);
        Gizmos.DrawSphere(transform.position , 0.05f);
        Gizmos.color = new Color(0, 0, 1, 1);
        Gizmos.DrawSphere(transform.position , 0.05f);

        Gizmos.color = new Color(0, 1, 1, 1);
        for (int i = 0; i < 3; ++i)
        {
            //DrawTriangle(gizmoTri.edgeLink[i]);
        }
        Gizmos.color = new Color(1, 1, 0, 1);
        //DrawTriangle(gizmoTri);
    }
    void DrawTriangle(Triangle tri)
    {
        if (tri == null)
        {
            return;
        }
        for (int i = 0; i < 3; ++i)
        {
            Gizmos.DrawLine(
                transform.position + vtx[tri.idx[i]],
                transform.position + vtx[tri.idx[(i + 1) % 3]]);
        }
    }

    // 三角形の生成
    void CreateTriangle()
    {
        // 三角形を作る処理
        filter = GetComponent<MeshFilter>();
        mesh = new Mesh();

        vtx.Add(new Vector3(-10, 0, 1));
        vtx.Add(new Vector3(1, 0, -10));
        vtx.Add(new Vector3(-10, 0, -10));

        mesh.SetVertices(vtx);

        idx.Add(0);
        idx.Add(1);
        idx.Add(2);

        mesh.SetTriangles(idx, 0);

        normal.Add(new Vector3(0, 1, 0));
        normal.Add(new Vector3(0, 1, 0));
        normal.Add(new Vector3(0, 1, 0));

        filter.mesh = mesh;

        gameObject.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;


        current = new Triangle();
        current.idx[0] = 0;
        current.idx[1] = 1;
        current.idx[2] = 2;
        // 追加された三角形をもとに新しくオブジェクトを生成
        //GameObject obj = new GameObject("cut obj", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody));
        mesh.vertices = vtx.ToArray();
        mesh.triangles = idx.ToArray();
        ////mesh.uv = uvs1.ToArray();
        mesh.normals = normal.ToArray();
        gameObject.AddComponent<MeshCollider>();
        gameObject.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
       
        //obj.GetComponent<MeshFilter>().mesh = mesh;
        //obj.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        //obj.GetComponent<MeshCollider>().sharedMesh = mesh;
        //obj.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
        //obj.GetComponent<MeshCollider>().convex = false;
        ////obj.GetComponent<MeshCollider>().material = GetComponent<Collider>().material;
        //obj.transform.position = transform.position;
        //obj.transform.rotation = transform.rotation;
        //obj.transform.localScale = transform.localScale;
        ////obj.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        ////obj.GetComponent<Rigidbody>().angularVelocity = GetComponent<Rigidbody>().angularVelocity;


        //obj.GetComponent<Rigidbody>().useGravity = false;   // 重力の無効化
        //obj.GetComponent<Rigidbody>().isKinematic = true;   // 運動を無効化 

    }
}
