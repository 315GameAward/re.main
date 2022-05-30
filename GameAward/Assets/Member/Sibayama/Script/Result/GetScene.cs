using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetScene : MonoBehaviour
{
    static int _currentSceneArea;
    static int _currentSceneStage;
    public static int CurrentSceneArea { get { return _currentSceneArea; } }// ���݂̃V�[���̃C���f�b�N�X�擾
    public static int CurrentSceneStage { get { return _currentSceneStage; } }// ���݂̃V�[���̃C���f�b�N�X�擾
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject == GameObject.Find("UI"))
        {
            _currentSceneArea = SceneManager.GetActiveScene().buildIndex;
        }
        if (gameObject == GameObject.Find("Scissor (1)"))
        {
            _currentSceneStage = SceneManager.GetActiveScene().buildIndex;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
