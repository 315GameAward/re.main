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
    // 変数宣言
    public List<Vector3> m_vCotPoint;   // ハサミの軌跡用リスト
    public List<Vector3> CutPointTest;   // ハサミの軌跡用リスト(テスト)

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
    RaycastHit hit_p; // 切りたい物体保存用

    private bool test = false;      // テスト用フラグ

    public bool bCut = false;  // 切り始めたか
    private bool bStartP = false;   // 始点が辺の上にあるか


    // 線分構造体
    public struct Segment
    {
        public Vector2 s; // 始点
        public Vector2 v; // 方向ベクトル（線分の長さも担うので正規化しないように！）
    };


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
       
        // 軌跡の数が1個以上あるとき
        if (m_vCotPoint.Count >= 1)
        {
            // レイキャストがあったとき 軌跡の最後にある座標とレイキャストして出た座標が一緒の時は処理をしない
            if (Physics.Raycast(ray, out hit) && hit.point != m_vCotPoint[m_vCotPoint.Count - 1])
            {
                if (hit.collider.gameObject.name == "Parper" || hit.collider.gameObject.name == "cut obj")
                {
                    // 軌跡を追加
                    m_vCotPoint.Add(hit.point);
                    Debug.Log("レイが当たった座標:" + hit.point);


                    // メッシュを分割する処理
                    hit.collider.gameObject.GetComponent<MeshCut>().Devision(m_vCotPoint);


                    Debug.Log("軌跡を追加");
                    Debug.Log("軌跡の数:" + m_vCotPoint.Count);
                    Debug.Log("軌跡の座標:" + m_vCotPoint[m_vCotPoint.Count - 1]);

                }

                // デバック用文字列表示
                if (!triggerFlg)
                {
                    Debug.Log(hit.collider.gameObject.name + "に当たった");
                    triggerFlg = true;  // デバック用トリガーフラグON
                }
                Debug.Log(hit.collider.gameObject.name + "に当たった");
            }
            else    // レイキャストが当たってないとき
            {
                // 頂点を削除
                if (m_vCotPoint.Count == 0) return;
                m_vCotPoint.Clear();
                Debug.Log("軌跡を削除");
                Debug.Log("軌跡の数:" + m_vCotPoint.Count);
                triggerFlg = false; // デバック用トリガーフラグOFF
            }
        }
        else
        {
            // レイキャストがあったとき
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "Parper" || hit.collider.gameObject.name == "cut obj")
                {

                    // 軌跡を追加
                    m_vCotPoint.Add(hit.point);
                    Debug.Log("レイが当たった座標:" + hit.point);

                    // メッシュを分割する処理
                    hit.collider.gameObject.GetComponent<MeshCut>().Devision(m_vCotPoint);


                    Debug.Log("軌跡を追加");
                    Debug.Log("軌跡の数:" + m_vCotPoint.Count);
                    Debug.Log("軌跡の座標:" + m_vCotPoint[m_vCotPoint.Count - 1]);
                }

                // デバック用文字列表示
                if (!triggerFlg)
                {
                    Debug.Log(hit.collider.gameObject.name + "に当たった");
                    triggerFlg = true;  // デバック用トリガーフラグON
                }

            }
            else    // レイキャストが当たってないとき
            {
                if (m_vCotPoint.Count == 0) return;
                // 頂点を削除
                m_vCotPoint.Clear();
                Debug.Log("軌跡を削除");
                Debug.Log("軌跡の数:" + m_vCotPoint.Count);
            }
        }
        //Debug.Log("a" );
        // レイキャストの表示
        //Debug.DrawRay(ray.origin,ray.direction * 5,Color.red,3,false);

        // レイキャストがあったとき 
        if (Physics.Raycast(ray, out hit))
        {
            // テスト用のポイントがあるとき
            if (CutPointTest.Count > 0)
            {
                // ヒットした座標と最後に格納した座標が違うときリストに格納したい
                if (hit.point != CutPointTest[CutPointTest.Count - 1])
                {
                    CutPointTest.Add(hit.point);    // ヒットした座標を格納

                    test = true;

                    // ヒットした物が切りたいものと違うときは一個前のポイントを削除したい。なんなら全部削除してもいいのか？          
                    if (hit.collider.gameObject.name != "Plane")
                    {
                        // カットポイントが1個以下の時
                        if(CutPointTest.Count <= 2)
                        {
                            // カットポイントの削除
                            //CutPointTest.RemoveAt(CutPointTest.Count - 1);
                            CutPointTest.Clear();
                        }
                      
                        test = false;
                    }

                    // ヒットしたものが切りたい物体の時
                    if (hit.collider.gameObject.name == "Plane" )
                    {
                        hit_p = hit;
                        bCut = true;    // 切り始め
                    }

                    // ヒットしたメッシュのポリゴン数
                    Debug.Log("三角形のインデックス数" + hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length);

                   

                    //Debug.Log("ポリゴン数" + hit.triangleIndex);
                }
            }
            else //テスト用のポイントがないとき
            {
                CutPointTest.Add(hit.point);    // ヒットした座標を格納


            }

        }
        else
        {
           
        }


        // カットポイントの始点と終点ををポリゴンの返上におきたい(カットポイントが増えるたびに処理)
        if (CutPointTest.Count >= 2)
        {
            // 処理を一回だけにする処理
            if (CutPointTest.Count == count) return;

            // 当たったメッシュのポリゴンずつ処理
            for (int i = 0; i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length ; i+=3)
            {
                           
                // 始点をポリゴンの辺におく処理
                if(!bStartP)
                for (int j = 0 ;j < 3 ;j++)
                {
                   
                    // 切りたい物体用の変数
                    int hitIdx_s = hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i+j];  // 始点
                    int hitIdx_v = hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i+((j + 1) % 3)];  // 終点
                   
                    Vector2 hitVtx_s = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].x + hit.collider.gameObject.transform.position.x  , hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].z + hit.collider.gameObject.transform.position.z);    // 始点
                    Vector2 hitVtx_v = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].x + hit.collider.gameObject.transform.position.x  , hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].z + hit.collider.gameObject.transform.position.z);    // 終点


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
                        Debug.Log("交差してる");
                        Debug.Log("交差した座標:" + p);
                        Debug.Log("交差した比:" + (double)t1 + ":" + (double)t2);
                        CutPointTest[0] = new Vector3(p.x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].y + hit.collider.gameObject.transform.position.y, p.y);
                        bStartP = true; // 切り始めセット
                    }
                }

                // 終点をポリゴンの辺におく処理
                if(bCut)
                {
                    for (int j = 0; j < 3; j++)
                    {

                        // 切りたい物体用の変数
                        int hitIdx_s = hit_p.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + j];  // 始点
                        int hitIdx_v = hit_p.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + ((j + 1) % 3)];  // 終点

                        Vector2 hitVtx_s = new Vector2(hit_p.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].x + hit_p.collider.gameObject.transform.position.x, hit_p.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_s].z + hit_p.collider.gameObject.transform.position.z);    // 始点
                        Vector2 hitVtx_v = new Vector2(hit_p.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].x + hit_p.collider.gameObject.transform.position.x, hit_p.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[hitIdx_v].z + hit_p.collider.gameObject.transform.position.z);    // 終点

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
                        else
                        {
                            Debug.Log("終点セット");
                            Debug.Log("終点の座標:" + p);
                           
                            CutPointTest[cp_s] = new Vector3(p.x, hit_p.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].y + hit_p.collider.gameObject.transform.position.y, p.y);
                        }
                    }

                    bCut = false;   // カット終了
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

        
        if(test)
        {
            if(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices.Length > 0)
            {
                for(int i = 0;i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices.Length;i++)
                {
                    Gizmos.color = new Color(25, 0, 0, 1);   // 色の指定
                    Gizmos.DrawSphere(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i] + hit.collider.gameObject.transform.position, 0.05f);  // 球の表示
                }

                for(int i = 0;i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length;i+=3)
                {
                    for(int j = 0;j < 3;j++)
                    {
                        int idx1 = hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + j];
                        int idx2 = hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles[i + ((j + 1)%3)];
                        Gizmos.color = new Color(25, 0, 0, 1);   // 色の指定
                        Gizmos.DrawLine(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[idx1] + hit.collider.gameObject.transform.position, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[idx2] + hit.collider.gameObject.transform.position);  // 線の表示
                    }
                }

                //for (int i = 0; i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length; i += 3)
                //{

                //    Gizmos.color = Random.ColorHSV();   // 色の指定
                //    Gizmos.DrawLine(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i] + hit.collider.gameObject.transform.position, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 1] + hit.collider.gameObject.transform.position);  // 球の表示
                //    Gizmos.DrawLine(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i] + hit.collider.gameObject.transform.position, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 2] + hit.collider.gameObject.transform.position);  // 球の表示
                //    Gizmos.DrawLine(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 1] + hit.collider.gameObject.transform.position, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 2] + hit.collider.gameObject.transform.position);  // 球の表示
                //}
                //{
                //    int i = 0;
                //    Gizmos.color = Random.ColorHSV();   // 色の指定
                //    Gizmos.DrawLine(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i] + hit.collider.gameObject.transform.position, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 11] + hit.collider.gameObject.transform.position);  // 球の表示
                //    Gizmos.DrawLine(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i] + hit.collider.gameObject.transform.position, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 12] + hit.collider.gameObject.transform.position);  // 球の表示
                //    Gizmos.DrawLine(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 11] + hit.collider.gameObject.transform.position, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 12] + hit.collider.gameObject.transform.position);  // 球の表示

                //}


            }
        }

    }

    // 2Dベクトルの外積
    float Vec2Cross(Vector2 v1, Vector2 v2)
    {
        return v1.x * v2.y - v1.y * v2.x;
    }


    // 線分の衝突
    bool ColSegments(
       Segment seg1,          // 線分1
       Segment seg2,          // 線分2
       float outT1,       // 線分1の内分比（出力）
       float outT2,       // 線分2の内分比（出力
       Vector2 outPos  // 交点（出力）
    )
    {

        Vector2 v = seg2.s - seg1.s;
        float Crs_v1_v2 = Vec2Cross(seg1.v, seg2.v);
        if (Crs_v1_v2 == 0.0f)
        {
            // 平行状態
            return false;
        }

        float Crs_v_v1 = Vec2Cross(v, seg1.v);
        float Crs_v_v2 = Vec2Cross(v, seg2.v);

        float t1 = Crs_v_v2 / Crs_v1_v2;
        float t2 = Crs_v_v1 / Crs_v1_v2;

        if (outT1 == 0)
            outT1 = Crs_v_v2 / Crs_v1_v2;
        if (outT2 == 0)
            outT2 = Crs_v_v1 / Crs_v1_v2;

        const float eps = 0.00001f;
        if (t1 + eps < 0 || t1 - eps > 1 || t2 + eps < 0 || t2 - eps > 1)
        {
            // 交差していない
            return false;
        }

        if (outPos == new Vector2(0, 0))
        {
            outPos = seg1.s + seg1.v * t1;
            p = seg1.s + seg1.v * t1;
        }
        p = seg1.s + seg1.v * t1;
        Debug.Log("交差してる");
        return true;
    }
}



//    private void OnTriggerEnter(Collider other)
//    {
//        // 指定の名前だったら処理する
//        if ((other.gameObject.name == "Parper" || other.gameObject.name == "cut obj"))
//        {
//            // レイキャストして正確な頂点を作成
//            Ray ray = new Ray(gameObject.transform.position, -gameObject.transform.up); // ハサミの上の刃のある一点から真下に向けてのレイ
//            RaycastHit hit; // 当たった物の情報を格納する変数

//            // 軌跡の数が1個以上あるとき
//            if (m_vCotPoint.Count >= 1)
//            {
//                // レイキャストがあったとき 軌跡の最後にある座標とレイキャストして出た座標が一緒の時は処理をしない
//                if (Physics.Raycast(ray, out hit) && hit.point != m_vCotPoint[m_vCotPoint.Count - 1])
//                {
//                    // 軌跡を追加
//                    m_vCotPoint.Add(hit.point);
//                    Debug.Log("レイが当たった座標:" + hit.point);

//                    // メッシュを分割する処理
//                    other.gameObject.GetComponent<MeshCut>().Devision(m_vCotPoint);

//                    Debug.Log("軌跡を追加");
//                    Debug.Log("軌跡の数:" + m_vCotPoint.Count);
//                    Debug.Log("軌跡の座標:" + m_vCotPoint[m_vCotPoint.Count - 1]);

//                }
//            }
//            else
//            {
//                // レイキャストがあったとき
//                if (Physics.Raycast(ray, out hit))
//                {
//                    // 軌跡を追加
//                    m_vCotPoint.Add(hit.point);
//                    Debug.Log("レイが当たった座標:" + hit.point);

//                    // メッシュを分割する処理
//                    other.gameObject.GetComponent<MeshCut>().Devision(m_vCotPoint);


//                    Debug.Log("軌跡を追加");
//                    Debug.Log("軌跡の数:" + m_vCotPoint.Count);
//                    Debug.Log("軌跡の座標:" + m_vCotPoint[m_vCotPoint.Count - 1]);
//                }
//            }




//        }
//    }
//    // 何かに当たり続けているとき
//    private void OnTriggerStay(Collider other)
//    {
//        // 指定の名前だったら処理する
//        if ((other.gameObject.name == "Parper" || other.gameObject.name == "cut obj"))
//        {
//            // レイキャストして正確な頂点を作成
//            Ray ray = new Ray(gameObject.transform.position, -gameObject.transform.up); // ハサミの上の刃のある一点から真下に向けてのレイ
//            RaycastHit hit; // 当たった物の情報を格納する変数

//            // 軌跡の数が1個以上あるとき
//            if (m_vCotPoint.Count >= 1)
//            {
//                // レイキャストがあったとき 軌跡の最後にある座標とレイキャストして出た座標が一緒の時は処理をしない
//                if (Physics.Raycast(ray, out hit) && hit.point != m_vCotPoint[m_vCotPoint.Count - 1])
//                {
//                    // 軌跡を追加
//                    m_vCotPoint.Add(hit.point);
//                    Debug.Log("レイが当たった座標:" + hit.point);


//                    // メッシュを分割する処理
//                    other.gameObject.GetComponent<MeshCut>().Devision(m_vCotPoint);

//                    Debug.Log("軌跡を追加");
//                    Debug.Log("軌跡の数:" + m_vCotPoint.Count);
//                    Debug.Log("軌跡の座標:" + m_vCotPoint[m_vCotPoint.Count - 1]);

//                }
//            }
//            else
//            {
//                // レイキャストがあったとき
//                if (Physics.Raycast(ray, out hit))
//                {
//                    // 軌跡を追加
//                    m_vCotPoint.Add(hit.point);
//                    Debug.Log("レイが当たった座標:" + hit.point);

//                    // メッシュを分割する処理
//                    other.gameObject.GetComponent<MeshCut>().Devision(m_vCotPoint);


//                    Debug.Log("軌跡を追加");
//                    Debug.Log("軌跡の数:" + m_vCotPoint.Count);
//                    Debug.Log("軌跡の座標:" + m_vCotPoint[m_vCotPoint.Count - 1]);
//                }
//            }




//        }
//    }

//    // 何かから離れる瞬間
//    private void OnTriggerExit(Collider other)
//    {
//        // 指定の名前だったら処理する
//        if ((other.gameObject.name == "Parper" || other.gameObject.name == "cut obj"))
//        {
//            // 軌跡を追加
//            m_vCotPoint.Add(gameObject.transform.position);
//            Debug.Log("軌跡を追加");
//            Debug.Log("軌跡の数:" + m_vCotPoint.Count);

//            // メッシュを分割する処理

//            // メッシュを切り分ける処理

//            // 頂点を削除
//            m_vCotPoint.Clear();
//            Debug.Log("軌跡を削除");
//            Debug.Log("軌跡の数:" + m_vCotPoint.Count);

//        }


//    }
//}
