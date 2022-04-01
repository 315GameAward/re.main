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

    private int count = 0;

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
        RaycastHit hit; // 当たった物の情報を格納する変数

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
                if(!triggerFlg)
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
        if (Physics.Raycast(ray, out hit) )
        {
            // テスト用のポイントがあるとき
            if(CutPointTest.Count > 0)
            { 
                // ヒットした座標と最後に格納した座標が違うときリストに格納したい
                if(hit.point != CutPointTest[CutPointTest.Count -1])
                {
                    CutPointTest.Add(hit.point);    // ヒットした座標を格納

                    // ヒットした物が切りたいものと違うときは一個前のポイントを削除したい。なんなら全部削除してもいいのか？          
                    if(hit.collider.gameObject.name != "Plane")
                    {
                        // カットポイントの削除
                        CutPointTest.RemoveAt(CutPointTest.Count - 2);
                        CutPointTest.Clear();
                    }

                    // ヒットしたメッシュのポリゴン数
                    Debug.Log("ポリゴン数" + hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertexCount);

                    //Debug.Log("ポリゴン数" + hit.triangleIndex);
                }
            }
            else //テスト用のポイントがないとき
            {
                CutPointTest.Add(hit.point);    // ヒットした座標を格納
            }

        }

        // カットポイントの始点と終点ををポリゴンの返上におきたい
        if(CutPointTest.Count >= 2)
        {
            // 処理を一回だけにする処理
            if (CutPointTest.Count == count) return;

            // 当たったメッシュの辺の数だけ処理
            for(int i = 0;i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertexCount - 1;i++)
            {
                // 線分と線分の始点をつないだベクトル
                v = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].x - CutPointTest[0].x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].z - CutPointTest[0].z);

                // 線分
                v1 = new Vector2(CutPointTest[1].x - CutPointTest[0].x, CutPointTest[1].z - CutPointTest[0].z);
                v2 = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 1].x - hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 1].z - hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].z);

                // 線分の始点から交点のベクトル
                t1 = (v.x * v2.y - v2.x * v.y) / (v1.x * v2.y - v2.x * v1.y);
                t2 = (v.x * v1.y - v1.x * v.y) / (v1.x * v2.y - v2.x * v1.y);

                // 交点
                p = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].z) + new Vector2(v2.x * t2, v2.y * t2);

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
                    CutPointTest[0] = new Vector3(p.x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].y, p.y);
                }

                //// 線分の宣言(カットポイントから)
                //Segment seg1;
                //seg1.s = new Vector2(CutPointTest[0].x, CutPointTest[0].z); // 線分の始点
                //seg1.v = new Vector2(CutPointTest[1].x, CutPointTest[1].z); // 線分の始点

                //// 線分の宣言(メッシュから)
                //Segment seg2;
                //seg2.s = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].x    , hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].z); // 線分の始点
                //seg2.v = new Vector2(hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 1].x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i + 1].z); // 線分の始点


                //if (ColSegments(seg1,seg2,0,0,new Vector2(0,0)))
                //{
                //    Debug.Log("交差してる");
                //    CutPointTest[0] = new Vector3(p.x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[i].y, p.y);

                //}



            }

            // 当たったメッシュのポリゴンごとに処理
            for (int i = 0; i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertexCount ; i += 3)
            {

            }

                //Debug.Log("t2:"+t2);


                count = CutPointTest.Count;
           
        }

        // 線と線の交点
        

    }

    private void OnDrawGizmos()
    {
        // テスト用のポイントを表示したい
        if(CutPointTest.Count > 0)
        {
            // 始点のカットポイントギズモ
            Gizmos.color = new Color(1, 1, 0, 1);   // 色の指定
            Gizmos.DrawSphere(CutPointTest[0], 0.05f);  // 球の表示

            // 途中のカットポイントギズモ
            for (int i = 1;i < CutPointTest.Count;i++)
            {          
                Gizmos.color = new Color(0, 1, 0, 1);   // 色の指定
                Gizmos.DrawSphere(CutPointTest[i], 0.05f);  // 球の表示
            }

            // 終点のカットポイントギズモ
            Gizmos.color = new Color(1, 1, 0, 1);   // 色の指定
            Gizmos.DrawSphere(CutPointTest[CutPointTest.Count - 1], 0.05f);  // 球の表示
        }

        // テスト用のポイントをつなぐ線を表示したい
        if(CutPointTest.Count >= 2)
        {
            for (int i = 1; i < CutPointTest.Count; i++)
            {
                Gizmos.color = new Color(0, 0.5f, 0, 1);    // 色の指定                
                Gizmos.DrawLine(CutPointTest[i - 1], CutPointTest[i]);  // 線の表示
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
       float outT1 ,       // 線分1の内分比（出力）
       float outT2 ,       // 線分2の内分比（出力
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

        if (outPos == new Vector2(0,0))
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
