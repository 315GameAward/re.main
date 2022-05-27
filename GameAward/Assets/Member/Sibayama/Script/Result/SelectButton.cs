//===============================================================
//
//      SelectButton.cs
//      �{�^���I��
//
//---------------------------------------------------------------
//      �쐬��: �ĎR꣑��Y
//      �쐬�J�n���F2022/5/26(��)
//===============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectButton : MonoBehaviour
{
    Button[] button = new Button[2];
    // Start is called before the first frame update
    void Start()
    {
        button[0] = GameObject.Find("ReturnSelect").GetComponent<Button>();
        button[1] = GameObject.Find("NextStage").GetComponent<Button>();
        button[0].Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnSelect(string _name)
    {
        SceneManager.LoadScene(_name);
    }

    public void NextStage(int _index)
    {
        SceneManager.LoadScene(_index);
    }

}
