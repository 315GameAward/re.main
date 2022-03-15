//======================================================
//
//        Cutter.cs
//        ハサミの切る処理
//
//------------------------------------------------------
//      作成者:根本龍之介
//======================================================

//======================================================
// 開発履歴
// 2022/02/16 プロトタイプ作成開始
// 編集者:根本龍之介
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    //--- Unityに公開しないパラメータ
    private bool m_bBiginPoint = false; // 地面に当たったときに切り始めのポイントを作り、それがあったらtrueにする


    // 動く方向で切断する場合
    //  private Vector3 prePos = Vector3.zero;
    //  private Vector3 prePos2 = Vector3.zero;

    //  void FixedUpdate ()
    //  {
    //      prePos = prePos2;
    //      prePos2 = transform.position;
    //  }

    // このコンポーネントを付けたオブジェクトのCollider.IsTriggerをONにする
    void OnTriggerEnter(Collider other)
    {
        m_bBiginPoint = true;
        

        //var meshCut = other.gameObject.GetComponent<MeshCut>();
        //if (meshCut == null) { return; }
        ////一方向のみで切断する方法、方向については適宜変更
        //var cutPlane = new Plane(transform.right, transform.position);
        ////動きで切断する場合
        ////var cutPlane = new Plane (Vector3.Cross(transform.forward.normalized, prePos - transform.position).normalized, transform.position);
        //meshCut.Cut(cutPlane);
    }

    // 地面に当たったとき
    void OnTriggerStay(Collider other)
    {
        
    }

    // 地面から離れたとき
   void OnTriggerExit(Collider other)
   {
        if(m_bBiginPoint)
        {
            var meshCut = other.gameObject.GetComponent<MeshCut>();
            if (meshCut == null) { return; }
            //一方向のみで切断する方法、方向については適宜変更
            var cutPlane = new Plane(transform.right, transform.position);
            //動きで切断する場合
            //var cutPlane = new Plane (Vector3.Cross(transform.forward.normalized, prePos - transform.position).normalized, transform.position);
            meshCut.Cut(cutPlane);  // 地面を切る
            m_bBiginPoint = false;  // ポイント解除
        }
   }
}