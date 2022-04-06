//======================================================
//
//        Cutpoint.cs
//        ハサミが切った軌跡の処理
//
//------------------------------------------------------
//      作成者:根本龍之介
//======================================================

//======================================================
// 開発履歴
// 2022/03/13 作成開始
// 編集者:根本龍之介
//======================================================
//======================================================
// 開発履歴
// 2022/03/14 コメントの追加
// 編集者:根本龍之介
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CutterPoint : MonoBehaviour
{
    //--- 変数宣言 ---

    // カットポイント用変数
    public class CutPointList
    {
        public List<Vector3> CutPoint;      // 切り取りポイント用リスト

    }

    public List<Vector3> m_vCotPoint;   // ハサミの軌跡用リスト
    public List<Vector3> CutPointTest;  // ハサミの軌跡用リスト(テスト)
    public List<Vector3> CutPoint;      // カットポイント用リスト
    public List<CutPointList> CutPointLst;  // カットポイントのリストを保存するためのリスト

    private int cpCount = 0;   // cpはCutPointの略。カットポイントが何個目かを格納する変数


    public MeshCut ground;

    private bool triggerFlg = false;    // デバック用トリガーフラグ

    // 線文用変数
    public Vector2 v;
    public Vector2 v1;
    public Vector2 v2;
    public Vector2 p;
    public float t1;
    public float t2;


    private int count = -1;

    RaycastHit hit; // 当たった物の情報を格納する変数
     GameObject hitGameObject;// 切りたい物体保存用

    private bool test = false;      // テスト用フラグ

    public bool bCut = false;  // 切り始めたか
    public bool bStartP = false;   // 始点が辺の上にあるか
    public bool bPurposeObj = false;

    [SerializeField] [Tooltip("")] private ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        m_vCotPoint.Clear();    // リストの中身をクリア
        CutPointTest.Clear();    // リストの中身をクリア

    }

    // Update is called once per frame
    void Update()
    {
        // レイキャストして正確な頂点を作成
        Ray ray = new Ray(gameObject.transform.position, -gameObject.transform.up); // ハサミの上の刃のある一点から真下に向けてのレイ

      
        // レイキャストがあったとき 
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.name == "Plane" || hit.collider.gameObject.name == "DivisionPlane") bPurposeObj = true;
            else bPurposeObj = false;

            // テスト用のポイントがあるとき
            if (CutPointTest.Count > 0)
            {
                // ヒットした座標と最後に格納した座標が違うときリストに格納したい
                if (hit.point != CutPointTest[CutPointTest.Count - 1])
                {
                    CutPointTest.Add(hit.point);    // ヒットした座標を格納
                    if(hit.collider.gameObject.name == "Plane" || hit.collider.gameObject.name == "DivisionPlane")
                    {
                        ParticleSystem newParticle = Instantiate(particle);
                        newParticle.transform.position = this.transform.position;
                        newParticle.Play();
                        Destroy(newParticle.gameObject, 2.0f);
                    }
                   
                    test = true;

                    // ヒットした物が切りたいものと違うときは一個前のポイントを削除したい。なんなら全部削除してもいいのか？          
                    if (hit.collider.gameObject.name != "Plane" && hit.collider.gameObject.name != "DivisionPlane")
                    {
                        // カットポイントが1個以下の時
                        if (CutPointTest.Count <= 2)
                        {
                            // カットポイントの削除
                            CutPointTest.Clear();
                        }

                        test = false;
                    }
                    
                    // ヒットしたメッシュのポリゴン数
                    Debug.Log("頂点数" + hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices.Length);
                    //Debug.Log("ポリゴン数" + hit.triangleIndex);
                }
            }
            else //テスト用のポイントがないとき
            {
                CutPointTest.Add(hit.point);    // ヒットした座標を格納
            }
         
        }

        // カットポイントの始点と終点ををポリゴンの返上におきたい(カットポイントが増えるたびに処理)
        if (CutPointTest.Count >= 2)
        {
            // 処理を一回だけにする処理
            if (CutPointTest.Count == count) return;

            // 当たったメッシュのポリゴンずつ処理
            for (int i = 0; i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length; i += 3)
            {

                // 始点をポリゴンの辺におく処理
                if (!bStartP)
                    for (int j = 0; j < 3; j++)
                    {

                        // 切りたい物体用の変数
                        int hitIdx_s = hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + j];  // 始点
                        int hitIdx_v = hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + ((j + 1) % 3)];  // 終点

                        Vector2 hitVtx_s = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].x + hit.collider.gameObject.transform.position.x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].z + hit.collider.gameObject.transform.position.z);    // 始点
                        Vector2 hitVtx_v = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].x + hit.collider.gameObject.transform.position.x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].z + hit.collider.gameObject.transform.position.z);    // 終点


                        // 線分と線分の始点をつないだベクトル
                        v = new Vector2(hitVtx_s.x - CutPointTest[0].x, hitVtx_s.y - CutPointTest[0].z);

                        // 線分
                        v1 = new Vector2(CutPointTest[1].x - CutPointTest[0].x, CutPointTest[1].z - CutPointTest[0].z);
                        v2 = new Vector2(hitVtx_v.x - hitVtx_s.x, hitVtx_v.y - hitVtx_s.y);

                        // 線分の始点から交点のベクトル
                        t1 = (v.x * v2.y - v2.x * v.y) / (v1.x * v2.y - v2.x * v1.y);
                        t2 = (v.x * v1.y - v1.x * v.y) / (v1.x * v2.y - v2.x * v1.y);

                        // 交点
                        p = new Vector2(hitVtx_s.x, hitVtx_s.y) + new Vector2(v2.x * t2, v2.y * t2);

                        // 線分と線分が交わっているか
                        const float eps = 0.00001f;
                        if (t1 + eps < 0 || t1 - eps > 1 || t2 + eps < 0 || t2 - eps > 1)
                        {
                            // Debug.Log("交差してない");
                        }
                        else
                        {
                            //Debug.Log("交差してる");
                            //Debug.Log("交差した座標:" + p);
                            //Debug.Log("交差した比:" + (double)t1 + ":" + (double)t2);
                            CutPointTest[0] = new Vector3(p.x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].y + hit.collider.gameObject.transform.position.y, p.y);


                            // メッシュを分割
                            hitGameObject = hit.collider.gameObject;
                            bStartP = true; // 切り始めセット
                            
                        }
                    }                          
            }

            // 今の三角形ポリゴンから離れたときにポリゴンとカットポイントの交点を作る処理
            /*
            if (hit.collider.gameObject.name == "Plane" || hit.collider.gameObject.name == "DivisionPlane")
            for (int i = 0;i < hitGameObject.GetComponent<MeshFilter>().mesh.triangles.Length; i += 3)
            {
                if (hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length > 0)
                    for (int j = 0; j < 3; j++)
                    {

                        // 切りたい物体用の変数
                        int hitIdx_s = hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + j];  // 始点
                        int hitIdx_v = hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + ((j + 1) % 3)];  // 終点

                        Vector2 hitVtx_s = new Vector2(hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].x + hitGameObject.gameObject.transform.position.x, hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].z + hitGameObject.gameObject.transform.position.z);    // 始点
                        Vector2 hitVtx_v = new Vector2(hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].x + hitGameObject.gameObject.transform.position.x, hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].z + hitGameObject.gameObject.transform.position.z);    // 終点

                        // カットポイント用変数
                        int cp_s = CutPointTest.Count - 2;   // 始点
                        int cp_v = CutPointTest.Count - 1;   // 終点

                        // 線分と線分の始点をつないだベクトル
                        v = new Vector2(hitVtx_s.x - CutPointTest[cp_s].x, hitVtx_s.y - CutPointTest[cp_s].z);

                        // 線分
                        v1 = new Vector2(CutPointTest[cp_v].x - CutPointTest[cp_s].x, CutPointTest[cp_v].z - CutPointTest[cp_s].z);
                        v2 = new Vector2(hitVtx_v.x - hitVtx_s.x, hitVtx_v.y - hitVtx_s.y);

                        // 線分の始点から交点のベクトル
                        t1 = (v.x * v2.y - v2.x * v.y) / (v1.x * v2.y - v2.x * v1.y);
                        t2 = (v.x * v1.y - v1.x * v.y) / (v1.x * v2.y - v2.x * v1.y);

                        // 交点
                        p = new Vector2(hitVtx_s.x, hitVtx_s.y) + new Vector2(v2.x * t2, v2.y * t2);

                        // 線分と線分が交わっているか
                        const float eps = 0.00001f;
                        if (t1 + eps < 0 || t1 - eps > 1 || t2 + eps < 0 || t2 - eps > 1)
                        {
                            // Debug.Log("交差してない");
                        }
                        else // ここで切り終わる
                        {
                            //Debug.Log("終点セット");
                            //Debug.Log("終点の座標:" + p);

                            // 終点のセット                        
                            CutPointTest[cp_s] = new Vector3(p.x, hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].y + hitGameObject.gameObject.transform.position.y, p.y);

                           
                        }
                    }

            }
            */

            // 切りたい物体から離れた時
            if(bStartP)
            if (hit.collider.gameObject.name != "Plane" && hit.collider.gameObject.name != "DivisionPlane")
                for (int i = 0; i < hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length; i += 3)
                {
                    if (hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length > 0)
                        for (int j = 0; j < 3; j++)
                        {

                            // 切りたい物体用の変数
                            int hitIdx_s = hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + j];  // 始点
                            int hitIdx_v = hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + ((j + 1) % 3)];  // 終点

                            Vector2 hitVtx_s = new Vector2(hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].x + hitGameObject.gameObject.transform.position.x, hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].z + hitGameObject.gameObject.transform.position.z);    // 始点
                            Vector2 hitVtx_v = new Vector2(hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].x + hitGameObject.gameObject.transform.position.x, hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].z + hitGameObject.gameObject.transform.position.z);    // 終点

                            // カットポイント用変数
                            int cp_s = CutPointTest.Count - 2;   // 始点
                            int cp_v = CutPointTest.Count - 1;   // 終点

                            // 線分と線分の始点をつないだベクトル
                            v = new Vector2(hitVtx_s.x - CutPointTest[cp_s].x, hitVtx_s.y - CutPointTest[cp_s].z);

                            // 線分
                            v1 = new Vector2(CutPointTest[cp_v].x - CutPointTest[cp_s].x, CutPointTest[cp_v].z - CutPointTest[cp_s].z);
                            v2 = new Vector2(hitVtx_v.x - hitVtx_s.x, hitVtx_v.y - hitVtx_s.y);

                            // 線分の始点から交点のベクトル
                            t1 = (v.x * v2.y - v2.x * v.y) / (v1.x * v2.y - v2.x * v1.y);
                            t2 = (v.x * v1.y - v1.x * v.y) / (v1.x * v2.y - v2.x * v1.y);

                            // 交点
                            p = new Vector2(hitVtx_s.x, hitVtx_s.y) + new Vector2(v2.x * t2, v2.y * t2);

                            // 線分と線分が交わっているか
                            const float eps = 0.00001f;
                            if (t1 + eps < 0 || t1 - eps > 1 || t2 + eps < 0 || t2 - eps > 1)
                            {
                                // Debug.Log("交差してない");
                            }
                            else // ここで切り終わる
                            {
                                //Debug.Log("終点セット");
                                //Debug.Log("終点の座標:" + p);

                                // 終点のセット                        
                                CutPointTest[cp_v] = new Vector3(p.x, hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].y + hitGameObject.gameObject.transform.position.y, p.y);

                                // 二個前のカットポイントを削除
                                if(CutPoint.Count > 0)
                                {
                                    CutPoint.Clear();
                                }

                                // カットポイントの保存                               
                                for (int k = 0; k <CutPointTest.Count;k++)
                                {
                                    CutPoint.Add(CutPointTest[k]);

                                }

                                    // メッシュの分割
                                    for (int l = 0; l < CutPoint.Count; l++)
                                    {
                                        if (GameObject.Find("DivisionPlane" + l)) hitGameObject = GameObject.Find("DivisionPlane" + l);
                                        hitGameObject.gameObject.GetComponent<MeshDivision>().DivisionMesh(CutPoint, l);

                                        hitGameObject = GameObject.Find("DivisionPlane" + l);

                                    }
                                    //hitGameObject.gameObject.GetComponent<MeshDivision>().DivisionMesh(CutPoint, 0);
                                    //hitGameObject = GameObject.Find("DivisionPlane0");
                                    //hitGameObject.gameObject.GetComponent<MeshDivision>().DivisionMesh(CutPoint, 1);
                                    //hitGameObject = GameObject.Find("DivisionPlane1");
                                    //// メッシュのカット
                                    hitGameObject.gameObject.GetComponent<MeshDivision>().CutMesh();

                                // 今のカットポイントの削除
                                    CutPointTest.RemoveRange(0, CutPointTest.Count-1);
                                    
                                bStartP = false;
                                return;
                            }
                        }

                }


            // 一回だけにするための処理
            count = CutPointTest.Count;

        }

    }

    private void OnDrawGizmos()
    {
        // テスト用のポイントを表示したい
        if (CutPointTest.Count > 0)
        {
            // 始点のカットポイントギズモ
            Gizmos.color = new Color(1, 1, 0, 1);   // 色の指定
            Gizmos.DrawSphere(CutPointTest[0], 0.05f);  // 球の表示

            // 途中のカットポイントギズモ
            for (int i = 1; i < CutPointTest.Count; i++)
            {
                Gizmos.color = new Color(0, 1, 0, 1);   // 色の指定
                Gizmos.DrawSphere(CutPointTest[i], 0.05f);  // 球の表示
            }

            // 終点のカットポイントギズモ
            Gizmos.color = new Color(1, 1, 0, 1);   // 色の指定
            Gizmos.DrawSphere(CutPointTest[CutPointTest.Count - 1], 0.05f);  // 球の表示
        }

        // テスト用のポイントをつなぐ線を表示したい
        if (CutPointTest.Count >= 2)
        {
            for (int i = 1; i < CutPointTest.Count; i++)
            {
                Gizmos.color = new Color(0, 0.5f, 0, 1);    // 色の指定                
                Gizmos.DrawLine(CutPointTest[i - 1], CutPointTest[i]);  // 線の表示
            }
        }


        //if (test)
        //{
        //    if (hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices.Length > 0)
        //    {
        //        for (int i = 0; i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices.Length; i++)
        //        {
        //            Gizmos.color = new Color(25, 0, 0, 1);   // 色の指定
        //            Gizmos.DrawSphere(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i] + hit.collider.gameObject.transform.position, 0.05f);  // 球の表示
        //        }

        //        for (int i = 0; i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length; i += 3)
        //        {
        //            for (int j = 0; j < 3; j++)
        //            {
        //                int idx1 = hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + j];
        //                int idx2 = hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + ((j + 1) % 3)];
        //                Gizmos.color = new Color(25, 0, 0, 1);   // 色の指定
        //                Gizmos.DrawLine(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[idx1] + hit.collider.gameObject.transform.position, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[idx2] + hit.collider.gameObject.transform.position);  // 線の表示
        //            }
        //        }



        //    }


        //}

        //if (CutPointTest.Count >= 2)
        //    if (hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices.Length > 0)
        //    {
        //        for (int i = 0; i < hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices.Length; i++)
        //        {
        //            Gizmos.color = new Color(25, 0, 0, 1);   // 色の指定
        //            Gizmos.DrawSphere(hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[i] + hitGameObject.gameObject.transform.position, 0.05f);  // 球の表示
        //        }

        //        for (int i = 0; i < hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length; i += 3)
        //        {
        //            for (int j = 0; j < 3; j++)
        //            {
        //                int idx1 = hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + j];
        //                int idx2 = hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + ((j + 1) % 3)];
        //                Gizmos.color = new Color(25, 0, 0, 1);   // 色の指定
        //                Gizmos.DrawLine(hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[idx1] + hitGameObject.gameObject.transform.position, hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.vertices[idx2] + hitGameObject.gameObject.transform.position);  // 線の表示
        //            }
        //        }
        //    }

        // 一個前のカットポイントの表示
        if (CutPoint.Count > 0)
        {
            // 始点のカットポイントギズモ
            Gizmos.color = new Color(1, 1, 0, 1);   // 色の指定
            Gizmos.DrawSphere(CutPoint[0], 0.05f);  // 球の表示

            // 途中のカットポイントギズモ
            for (int i = 1; i < CutPoint.Count; i++)
            {
                Gizmos.color = new Color(0, 1, 0, 1);   // 色の指定
                Gizmos.DrawSphere(CutPoint[i], 0.05f);  // 球の表示
            }

            // 終点のカットポイントギズモ
            Gizmos.color = new Color(1, 1, 0, 1);   // 色の指定
            Gizmos.DrawSphere(CutPoint[CutPoint.Count - 1], 0.05f);  // 球の表示
        }
        //  一個前のカットポイントをつなぐ線を表示したい
        if (CutPoint.Count >= 2)
        {
            for (int i = 1; i < CutPoint.Count; i++)
            {
                Gizmos.color = new Color(0, 0.5f, 0, 1);    // 色の指定                
                Gizmos.DrawLine(CutPoint[i - 1], CutPoint[i]);  // 線の表示
            }
        }
    }

    // 2Dベクトルの外積
    float Vec2Cross(Vector2 v1, Vector2 v2)
    {
        return v1.x * v2.y - v1.y * v2.x;
    }


    
}



