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

    private int MaxLife;        // 最大値
    public int nLife = 3;       // 体力(変動する)
    public static int nlife;    // 他シーンへ持っていく体力数

    private bool b_Life = true; // 体力が残っているか true:ある

    public static Life instance;
    public GameObject image_gameOver;

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
        // 効果音取得
        audioSource = GetComponent<AudioSource>();

        // 最大値代入
        MaxLife = nLife;

        // 体力設定
        for (int i = nLife; i > 0; i--)
        {
            // 体力を設定していく
            AddLife();
        }
        b_Life = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(nLife);

        if (nLife <= 0 && b_Life == true)
        {           
            // ゲームオーバー呼び出し
            image_gameOver.GetComponent<Game_Over>().SetGMOV(true);
            b_Life = false; // 一度しか通らない
        }

        // 左矢印を押したら体力消費
        if (Input.GetKeyUp(KeyCode.PageUp))
        {
            AddLife();
            nLife++;
        }

        // 右矢印を押したら体力増加
        if (Input.GetKeyUp(KeyCode.PageDown))
        {
            DelLife();
        }
    }

    //========================
    //
    // ハートを1つ増やす関数
    //
    //========================
    public void AddLife()
    {
        // 引数2はどのオブジェクトの子にするかで、
        // 引数3は子にする際に以前の位置を保つか(LayoutGroup系ではfalseにしないとおかしくなる)
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
            nLife = 0;
            return;
        }

        Destroy(Lifes[0]);
        Lifes.RemoveAt(0);

        //音を鳴らす
        audioSource.PlayOneShot(sound);

        nLife--;
    }

    // 体力数取得用関数
    public int GetLife()
    {
        nlife = nLife;
        return nlife;
    }

    // 最大数取得用関数
    public int GetMaxLife()
    {       
        return MaxLife;
    }
}
