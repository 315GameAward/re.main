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

public class Cutpoint : MonoBehaviour
{
    // 変数宣言
    public List<Vector3> m_vCotPoint;   // ハサミの軌跡用リスト

    

    // Start is called before the first frame update
    void Start()
    {
        m_vCotPoint.Clear();    // リストの中身をクリア
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    // 何かに当たり続けているとき
    private void OnTriggerStay(Collider other)
    {
        // 指定の名前だったら処理する
        if ((other.gameObject.name == "Parper" || other.gameObject.name == "cut obj"))
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
                    // 軌跡を追加
                    m_vCotPoint.Add(hit.point);
                    Debug.Log("軌跡を追加");
                    Debug.Log("軌跡の数:" + m_vCotPoint.Count);

                }
            }
            else
            {
                // レイキャストがあったとき
                if (Physics.Raycast(ray, out hit))
                {
                    // 軌跡を追加
                    m_vCotPoint.Add(hit.point);
                    Debug.Log("軌跡を追加");
                    Debug.Log("軌跡の数:" + m_vCotPoint.Count);

                }
            }

            // メッシュを分割する処理


        }
    }

    // 何かから離れる瞬間
    private void OnTriggerExit(Collider other)
    {
        // 指定の名前だったら処理する
        if ((other.gameObject.name == "Parper" || other.gameObject.name == "cut obj"))
        {
            // 軌跡を追加
            m_vCotPoint.Add(gameObject.transform.position);
            Debug.Log("軌跡を追加");
            Debug.Log("軌跡の数:" + m_vCotPoint.Count);

            // メッシュを分割する処理

            // メッシュを切り分ける処理

            // 頂点を削除
            m_vCotPoint.Clear();
            Debug.Log("軌跡を削除");
            Debug.Log("軌跡の数:" + m_vCotPoint.Count);

        }


    }
}
