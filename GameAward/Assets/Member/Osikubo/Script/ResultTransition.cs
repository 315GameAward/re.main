using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultTransition : MonoBehaviour
{
    private GameObject[] ResultTest;
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        ResultTest = GameObject.FindGameObjectsWithTag("kesigomu");
       // Debug.Log("Á‚µƒSƒ€‚Ì”" + ResultTest.Length);
        if (ResultTest.Length == 0)
        {
            Debug.Log("ƒV[ƒ“‚ÌˆÚ“®");
            SceneManager.LoadScene("ResultScene");
        }
    }
}