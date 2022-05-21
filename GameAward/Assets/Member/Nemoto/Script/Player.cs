using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject image_gameOver;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
        if (other.gameObject.tag == "Enemy")
        {
            Life.instance.DelLife();
            //image_gameOver.GetComponent<GameOver>().ShowGameOver();
        }
    }
}
