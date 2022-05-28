using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultTransition : MonoBehaviour
{
    bool Trigger;
    private GameObject[] ResultTest;
    private Timer time;
    private GameObject objTime;

    // Start is called before the first frame update
    void Start()
    {
        objTime = GameObject.Find("Timer_UI");
        time = objTime.GetComponent<Timer>();
        Trigger = false;
    }


    // Update is called once per frame
    void Update()
    {
        ResultTest = GameObject.FindGameObjectsWithTag("Enemy");

        if (ResultTest.Length == 0 && !Trigger)
        {
            time.SetStopTimer(true);
            Debug.Log("ÉVÅ[ÉìÇÃà⁄ìÆ");
            SceneManager.LoadScene("ResultScene", LoadSceneMode.Additive);
            Trigger = true;
        }
    }
}