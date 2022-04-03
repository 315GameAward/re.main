using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float countTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //countTime‚ÉAƒQ[ƒ€‚ªŠJn‚µ‚Ä‚©‚ç‚Ì•b”‚ğŠi”[
        countTime += Time.deltaTime;

        //­”‚ğ2Œ…‚É‚µ‚Ä•\¦
        GetComponent<Text>().text = countTime.ToString("F2");
    }
}
