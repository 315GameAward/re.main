using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Scenetest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.spaceKey.isPressed)
        {
            SceneManager.LoadSceneAsync("TitleScene", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);
        }
        if (Keyboard.current.aKey.isPressed)
        {
            SceneManager.UnloadSceneAsync("TitleScene");
            SceneManager.UnloadSceneAsync("GameScene");
        }
    }
}
