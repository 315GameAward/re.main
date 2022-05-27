using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutPoint2 : MonoBehaviour
{
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

    // デバッグ用
    private bool triggerFlg = false;    // デバック用トリガーフラグ
    public bool AddPointOnOff = true;   // ポイントを追加するかどうか 

    public GameObject m_CubeBase;
    public List<GameObject> objList;

    // 線文用変数
    public Vector2 v;
    public Vector2 v1;
    public Vector2 v2;
    public Vector2 p;
    public float t1;
    public float t2;


    private int count = -1;

    RaycastHit hit; // 当たった物の情報を格納する変数
    RaycastHit hit2; // 当たった物の情報を格納する変数
    GameObject hitGameObject;// 切りたい物体保存用

    private bool test = false;      // テスト用フラグ

    public bool bCut = false;  // 切り始めたか
    public bool bStartP = false;   // 始点が辺の上にあるか
    public bool bPurposeObj = false;

    [SerializeField] [Tooltip("")] private ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        objList = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
      
    }
    public bool AddCPPoint()
    {

        // レイキャストして正確な頂点を作成
        Ray ray = new Ray(gameObject.transform.position, -gameObject.transform.up); // ハサミの上の刃のある一点から真下に向けてのレイ


        // レイキャストがあったとき 
        if (Physics.Raycast(ray, out hit,5,5))
        {
            // ヒットしたものが目的の物だったら
            if (hit.collider.gameObject.name == "Plane" || hit.collider.gameObject.name == "DivisionPlane") bPurposeObj = true;
            else bPurposeObj = false;

            // テスト用のポイントがあるとき
            if (CutPointTest.Count > 0)
            {
                // ヒットした座標と最後に格納した座標が違うときリストに格納したい
                if (hit.point != CutPointTest[CutPointTest.Count - 1])
                {
                    // カットポイントの追加
                    if (hit.collider.gameObject.name == "Plane" || hit.collider.gameObject.name == "MoveArea")
                    {
                        CutPointTest.Add(hit.point);    // ヒットした座標を格納
                        Debug.Log("カットポイントの追加");
                    }
                  

                   // ダメージラインの位置設定、生成
                    Vector3 pos = new Vector3();
                    pos = hit.point;
                    GameObject gameObj;
                    gameObj = m_CubeBase;
                    gameObj.transform.position = pos;
                    gameObj.transform.rotation = this.transform.rotation;
                   
                    // m_CubeBase を元にして新しいm_CubeBaseを作成
                    objList.Add(Instantiate(m_CubeBase, pos, this.transform.rotation));

                    // 頂点を広げる処理
                    if (CutPointTest.Count >= 3 && hit.collider.gameObject.name == "Plane")
                    {
                        hit.collider.gameObject.GetComponent<MeshDivision2>().DiviosionMeshMiddle(CutPointTest);
                        hitGameObject = hit.collider.gameObject;
                    }

                    // エフェクトの生成
                    if (hit.collider.gameObject.name == "Plane" || hit.collider.gameObject.name == "DivisionPlane")
                    {
                        ParticleSystem newParticle = Instantiate(particle);
                        newParticle.transform.position = this.transform.position;
                        newParticle.Play();
                        Destroy(newParticle.gameObject, 2.0f);
                    }

                    // なにこれ？
                    test = true;

                    // ヒットした物が切りたいものと違うときは一個前のポイントを削除したい。なんなら全部削除してもいいのか？          
                    if (hit.collider.gameObject.name != "Plane" && hit.collider.gameObject.name != "DivisionPlane")
                    {
                        // カットポイントが2個以下の時
                        if (CutPointTest.Count <= 2)
                        {
                            // カットポイントの削除
                            CutPointTest.RemoveAt(0);

                            // ダメージラインの削除
                            for (int g = 0; g < objList.Count; g++)
                            {
                                objList[g].GetComponent<DamageLine>().Destroy();
                            }
                            objList.Clear();
                        }

                        test = false;
                    }

                    // ヒットしたメッシュのポリゴン数
                    //Debug.Log("頂点数" + hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices.Length);
                    //Debug.Log("ポリゴン数" + hit.triangleIndex);
                }
            }
            else //テスト用のポイントがないとき
            {

                CutPointTest.Add(hit.point);    // ヒットした座標を格納
            }

        }

        //if (AddPointOnOff)



        // カットポイントの始点と終点ををポリゴンの返上におきたい(カットポイントが増えるたびに処理)
        if (CutPointTest.Count >= 2)
        {
            // 処理を一回だけにする処理
            if (CutPointTest.Count != count)
            {
                // 始点をポリゴンの辺におく処理
                if (!bStartP)
                {
                    // カットポイントの辺とポリゴンとの辺の交点を保存する変数
                    var intersectionMemory = new List<Vector2>();

                    // 当たったメッシュのポリゴンずつ処理
                    for (int i = 0; i < hit.collider.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length; i += 3)
                    {

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
                                // 交差していないとき
                            }
                            else
                            {
                                // 交差しているとき 
                                intersectionMemory.Add(p);
                              
                            }
                        }


                    }
                    // 記憶された交点があるとき
                    if (intersectionMemory.Count > 1)
                    {
                        p = intersectionMemory[0];
                        // 記憶された交点の数だけループ
                        for (int k = 0; k < intersectionMemory.Count; k++)
                        {
                            // 交点の比較(カットポイントの始点との近さを比較、より近いpが優勝)
                            if (Vector2.Distance(new Vector2(CutPointTest[0].x, CutPointTest[0].z), new Vector2(p.x + transform.position.x, p.y + transform.position.z)) > Vector2.Distance(new Vector2(CutPointTest[0].x, CutPointTest[0].z), new Vector2(intersectionMemory[k].x + transform.position.x, intersectionMemory[k].y + transform.position.z))) continue;

                            // 交点の代入
                            p = intersectionMemory[k];

                            ////--- 交点の補正 ---
                            //var vtx_s = p;
                            //var vtx_v = new Vector2(CutPointTest[1].x, CutPointTest[1].z);
                            //var edg = vtx_v - vtx_s;
                            //p += edg * 0.01f;
                        }

                        // カットポイントの始点を変更
                        //CutPointTest[0] = new Vector3(p.x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[0].y + hit.collider.gameObject.transform.position.y, p.y);

                        // メッシュを分割
                        hit.collider.gameObject.GetComponent<MeshDivision2>().DivisionMesh(CutPointTest);
                        hitGameObject = hit.collider.gameObject;
                        bStartP = true; // 切り始めセット
                        return true;
                    }
                    else if (intersectionMemory.Count == 1)
                    {
                        //--- 交点の補正 ---
                        p = intersectionMemory[0];
                        var vtx_s = p;
                        var vtx_v = new Vector2(CutPointTest[1].x, CutPointTest[1].z);
                        var edg = vtx_v - vtx_s;
                        p += edg * 0.01f;

                        // カットポイントの始点を変更
                        CutPointTest[0] = new Vector3(p.x, hit.collider.gameObject.GetComponent<MeshFilter>().mesh.vertices[0].y + hit.collider.gameObject.transform.position.y, p.y);

                        // メッシュを分割
                        hit.collider.gameObject.GetComponent<MeshDivision2>().DivisionMesh(CutPointTest);
                        hitGameObject = hit.collider.gameObject;
                        bStartP = true; // 切り始めセット
                        return true;
                    }
                }



                // 切りたい物体から離れた時
                if (bStartP)
                    if (hit.collider.gameObject.name != "Plane" && hit.collider.gameObject.name != "DivisionPlane")
                    {
                        // カットポイントの辺とポリゴンとの辺の交点を保存する変数
                        var intersectionMemory = new List<Vector2>();

                        // ヒットしたオブジェクトのポリゴンの数だけループ
                        for (int i = 0; i < hitGameObject.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length; i += 3)
                        {

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

                                    // 交点の保存
                                    intersectionMemory.Add(p);


                                }
                            }

                        }

                        // 記憶された交点があるとき
                        if (intersectionMemory.Count > 0)
                        {
                            // 記憶された交点の数だけループ
                            for (int k = 0; k < intersectionMemory.Count; k++)
                            {
                                // 交点の比較(カットポイントの終点との近さを比較、より近いpが優勝)
                                if (Vector2.Distance(new Vector2(CutPointTest[CutPointTest.Count - 1].x, CutPointTest[CutPointTest.Count - 1].z), new Vector2(p.x, p.y)) < Vector2.Distance(new Vector2(CutPointTest[CutPointTest.Count - 1].x, CutPointTest[CutPointTest.Count - 1].z), new Vector2(intersectionMemory[k].x, intersectionMemory[k].y))) continue;

                                // 交点の代入
                                p = intersectionMemory[k];
                            }

                            // 終点のセット                        
                            CutPointTest[CutPointTest.Count - 1] = new Vector3(p.x, hitGameObject.transform.position.y, p.y);

                            // ポリゴン分割
                            hitGameObject.gameObject.GetComponent<MeshDivision2>().DivisionMeshTwice(CutPointTest);

                            

                            // 二個前のカットポイントを削除
                            if (CutPoint.Count > 0)
                            {
                                CutPoint.Clear();
                            }

                            // カットポイントの保存                               
                            for (int k = 0; k < CutPointTest.Count; k++)
                            {
                                CutPoint.Add(CutPointTest[k]);
                            }

                            // 今のカットポイントの削除
                            CutPointTest.RemoveRange(0, CutPointTest.Count);

                            // ダメージラインの削除
                            for(int g = 0;g < objList.Count;g++)
                            {
                                objList[g].GetComponent<DamageLine>().Destroy();
                            }
                            objList.Clear();

                            // カット終了フラグOFF
                            bStartP = false;
                            AddCPPoint();
                            return true;    // ここで終了
                        }

                    }

            }



            // 一回だけにするための処理
            count = CutPointTest.Count;

        }


        // なんかよくわからん
        return true;
    }

    // ギズモの表示
    private void OnDrawGizmos()
    {
        // テスト用のポイントを表示したい
        if (CutPointTest.Count > 0)
        {
            // 始点のカットポイントギズモ
            Gizmos.color = new Color(1, 1, 0, 1);   // 色の指定
            Gizmos.DrawSphere(CutPointTest[0], 0.01f);  // 球の表示

            // 途中のカットポイントギズモ
            for (int i = 1; i < CutPointTest.Count; i++)
            {
                Gizmos.color = new Color(0, 1, 0, 1);   // 色の指定
                Gizmos.DrawSphere(CutPointTest[i], 0.01f);  // 球の表示
            }

            // 終点のカットポイントギズモ
            Gizmos.color = new Color(1, 1, 0, 1);   // 色の指定
            Gizmos.DrawSphere(CutPointTest[CutPointTest.Count - 1], 0.01f);  // 球の表示
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



        // 一個前のカットポイントの表示
        if (CutPoint.Count > 0)
        {
            // 始点のカットポイントギズモ
            Gizmos.color = new Color(1, 1, 0, 1);   // 色の指定
            Gizmos.DrawSphere(CutPoint[0], 0.01f);  // 球の表示

            // 途中のカットポイントギズモ
            for (int i = 1; i < CutPoint.Count; i++)
            {
                Gizmos.color = new Color(0, 1, 0, 1);   // 色の指定
                Gizmos.DrawSphere(CutPoint[i], 0.01f);  // 球の表示
            }

            // 終点のカットポイントギズモ
            Gizmos.color = new Color(1, 1, 0, 1);   // 色の指定
            Gizmos.DrawSphere(CutPoint[CutPoint.Count - 1], 0.01f);  // 球の表示
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


}
