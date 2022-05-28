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

    bool bBreakAnim = false;    // アニメーションが再生されているか true:再生中
    Animator animator;
    float animTime = 0.0f;  // アニメーションの再生時間

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

        // アニメーターに代入
        animator = Lifes[0].GetComponent<Animator>();
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

       
        // アニメーションが終わったときの処理
        if(bBreakAnim)
        {
            
            // アニメーションが再生されていなかったら
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.5f)
            {
                // ライフの削除
                Destroy(Lifes[0]);
                Lifes.RemoveAt(0);                
               
                nLife--;

                // アニメーターに代入
                animator = Lifes[0].GetComponent<Animator>();
                Lifes[0].GetComponent<Animator>().SetBool("StartBreak", false);

                // アニメーション再生OFF
                bBreakAnim = false;
            }
           
        }

#if DEBUG
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
#endif


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
        if (bBreakAnim) return;

        if(nLife <= 0)
        {
            nLife = 0;
            return;
        }

        // アニメーション再生
        bBreakAnim = true;
        Lifes[0].GetComponent<Animator>().SetBool("StartBreak",true);
       
        //音を鳴らす
        audioSource.PlayOneShot(sound);

        // アニメーション時間代入
        animTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
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
