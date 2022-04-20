using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }

    // 当たり判定
    private void OnTriggerEnter(Collider other)
    {

        // エネミータグと触れたら
        if (other.gameObject.tag == "Enemy")
        {
            image_gameOver.GetComponent<GameOver>().ShowGameOver();
        }
    }
}
