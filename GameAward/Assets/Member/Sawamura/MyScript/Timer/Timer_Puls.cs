using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Timer_Puls : MonoBehaviour
{
    float countTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (countTime <= 10)
        {
            //countTimeに、ゲームが開始してからの秒数を格納
            countTime += Time.deltaTime;

        }

        //少数を2桁にして表示
        GetComponent<Text>().text = countTime.ToString("F2");
    }
}
