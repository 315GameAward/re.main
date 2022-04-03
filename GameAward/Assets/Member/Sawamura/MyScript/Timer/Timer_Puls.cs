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
            //countTime‚ÉAƒQ[ƒ€‚ªŠJŽn‚µ‚Ä‚©‚ç‚Ì•b”‚ðŠi”[
            countTime += Time.deltaTime;

        }

        //­”‚ð2Œ…‚É‚µ‚Ä•\Ž¦
        GetComponent<Text>().text = countTime.ToString("F2");
    }
}
