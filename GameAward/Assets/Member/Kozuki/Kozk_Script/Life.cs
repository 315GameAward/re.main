//========================
// 
//      Life
// 		プレイヤー体力
//
//--------------------------------------------
// 作成者：上月大地
//========================
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Life : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;  // インスペクターからハートのプレハブを割り当てる

    public int nLife;   // 体力

    List<GameObject> hearts = new List<GameObject>();   // 生成したライフを入れる
   
    // Start is called before the first frame update
    void Start()
    {
        for (int i = nLife; i > 0; i--)
        {
            // 体力を設定していく
            AddLife();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(nLife);

        if (nLife <= 0)
        {
            // ゲームオーバー呼び出し
            Debug.Log("ゲームオーバー");
        }

        // スペースを押したら体力消費
        if (Input.GetKeyUp(KeyCode.Space))
        {
            DelLife();        
        }

        // 下矢印を押したら体力増加
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            AddLife();
            nLife++;
        }
    }

    //========================
    //
    // ハートを1つ増やす関数
    //
    //========================
    public void AddLife()
    {
        // 引数2はどのオブジェクトの子にするかで、引数3は子にする際に以前の位置を保つか(LayoutGroup系ではfalseにしないとおかしくなる)
        GameObject instance = Instantiate(prefab, transform, false);
        hearts.Add(instance);
        
    }

    //========================
    //
    // ハートを1つ減らす関数
    //
    //========================
    public void DelLife()
    {
        Destroy(hearts[0]);
        hearts.RemoveAt(0);
        nLife--;
    }

}
