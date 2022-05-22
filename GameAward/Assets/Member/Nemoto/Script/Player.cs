using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject image_gameOver;
    public bool debug = false;
    static float delayTime = 0.0f;
    static bool damage = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ダメージのディレイ
        if (damage)
        {
            delayTime += 1.0f / 60.0f;
            if (delayTime > 3.0f)
            {
                delayTime = 0.0f;
                damage = false;
            }
        }

        // ダメージラインと当たったか
        for(int i = 0;i < gameObject.GetComponent<CutPoint2>().objList.Count;i++)
        {
            if (!gameObject.GetComponent<CutPoint2>().objList[i].GetComponent<DamageLine>().damage) continue;

            if (!damage) 
            {
                Life.instance.DelLife();
                damage = true;
            }
        }

        

        if (debug) return;
        // リトライ
        if(Life.instance.GetLife() <= 0)
        {
            if(Keyboard.current.rKey.isPressed)
            {
                SceneManager.LoadScene("GameScene");
            }
        }
    }

    // 当たり判定
    private void OnTriggerEnter(Collider other)
    {

        // エネミータグと触れたら
        if (other.gameObject.tag == "Enemy" && !damage)
        {
            Life.instance.DelLife();
            damage = true;
            //image_gameOver.GetComponent<GameOver>().ShowGameOver();
        }
        // Wallタグと触れたら
        if (other.gameObject.tag == "Wall")
        {
            Debug.Log("当たった");
        }
    }

    public void SetPotision(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }
}
