//========================
// 
//      Life
// 		プレイヤー体力
//
//--------------------------------------------
// 作成者：上月大地
//========================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    // 体力
    public int nLife;

    [SerializeField]
    GameObject prefab;      // インスペクターからハートのプレハブを割り当てる

    List<GameObject> hearts = new List<GameObject>();   // 生成したハートを入れる
   
    //========================
    //
    // ハートを1つ減らす関数
    //
    //========================
    public void DelLife()
    {
        nLife--;
        Destroy(hearts[0]);
        hearts.RemoveAt(0);
    }

    //========================
    //
    // ハートを1つ増やす関数
    //
    //========================
    public void Increase()
    {
        // 引数2はどのオブジェクトの子にするかで、引数3は子にする際に以前の位置を保つか(LayoutGroup系ではfalseにしないとおかしくなる)
        GameObject instance = Instantiate(prefab, transform, false);
        hearts.Add(instance);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = nLife; i > 0; i--)
        {
            Increase();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(nLife <= 0)
        {
            // ゲームオーバー呼び出し
        }
    }
}
