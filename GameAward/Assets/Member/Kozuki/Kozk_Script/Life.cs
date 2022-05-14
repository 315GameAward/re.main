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

    // 効果音
    public AudioClip sound;
    AudioSource audioSource;

    public int nLife;   // 体力
    public static Life instance;
    // 配列
    List<GameObject> Lifes = new List<GameObject>();   // 生成したライフを入れる
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = nLife; i > 0; i--)
        {
            // 効果音取得
            audioSource = GetComponent<AudioSource>();
            
            // 体力を設定していく
            AddLife();
        }
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(nLife);

        if (nLife <= 0)
        {
            // ゲームオーバー呼び出し
            // Debug.Log("ゲームオーバー");
        }

        //// スペースを押したら体力消費
        //if (Input.GetKeyUp(KeyCode.UpArrow))
        //{
        //    AddLife();
        //    nLife++;
        //}

        //// 下矢印を押したら体力増加
        //if (Input.GetKeyUp(KeyCode.DownArrow))
        //{
        //    DelLife();
        //}
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
        Lifes.Add(instance);
        
    }

    //========================
    //
    // ハートを1つ減らす関数
    //
    //========================
    public void DelLife()
    {
        if(nLife <= 0)
        {
            return;
        }

        Destroy(Lifes[0]);
        Lifes.RemoveAt(0);

        //音を鳴らす
        audioSource.PlayOneShot(sound);

        nLife--;
    }

}
