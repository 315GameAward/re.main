using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;//キー押したらの処理のため
using UnityEngine.SceneManagement;


public class SkipScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // スペースキーを押したら
        if (Keyboard.current.oKey.isPressed)
        {
            Debug.Log("シーンの移動");
            SceneManager.LoadScene("GameScene");
        }
    }
}
