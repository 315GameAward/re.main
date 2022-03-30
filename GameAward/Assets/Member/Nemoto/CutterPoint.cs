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

                    // ヒットした物が切りたいものと違うときは一個前のポイントを削除したい
                    if(hit.collider.gameObject.name != "Plane")
                    {
                        CutPointTest.RemoveAt(CutPointTest.Count - 2);
                    }

                    // ヒットしたメッシュのポリゴン数
                    //Debug.Log("ポリゴン数" + hit.collider.gameObject.GetComponent<Mesh>().triangles.Length);
                }
            }
            else //テスト用のポイントがないとき
            {
                CutPointTest.Add(hit.point);    // ヒットした座標を格納
            }

        }

        // 2Dの線を定義
        if(CutPointTest.Count >= 3)
        {
            v  = new Vector2(CutPointTest[CutPointTest.Count - 2].x - CutPointTest[0].x, CutPointTest[CutPointTest.Count - 2].z - CutPointTest[0].z);
            v1 = new Vector2(CutPointTest[1].x - CutPointTest[0].x, CutPointTest[1].z - CutPointTest[0].z);
            v2 = new Vector2(CutPointTest[CutPointTest.Count - 1].x - CutPointTest[CutPointTest.Count - 2].x, CutPointTest[CutPointTest.Count - 1].z - CutPointTest[CutPointTest.Count - 2].z);
            t2 = (v.x * v1.y - v1.x * v.y)/(v1.x * v2.y - v2.x * v1.y);
            p = new Vector2(CutPointTest[CutPointTest.Count - 2].x, CutPointTest[CutPointTest.Count - 2].z) + new Vector2(v2.x * t2,v2.y * t2);
            //Debug.Log("t2:"+t2);


            const float eps = 0.00001f;
            if (t1 + eps < 0 || t1 - eps > 1 || t2 + eps < 0 || t2 - eps > 1)
            {
                //Debug.Log("交差してない");
            }
            else
            {
                //Debug.Log("交差してる");
                //Debug.Log("交差した座標:" + p);
            }
        }

        // 線と線の交点
        

    }

    private void OnDrawGizmos()
    {
        // テスト用のポイントを表示したい
        if(CutPointTest.Count > 0)
        {
            for(int i = 0;i < CutPointTest.Count;i++)
            {
                Gizmos.color = new Color(0, 1, 0, 1);   // 色の指定
                Gizmos.DrawSphere(CutPointTest[i], 0.05f);  // 球の表示

            }
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
