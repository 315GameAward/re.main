using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// /ƒNƒ‰ƒX–¼‚ðTime‚É‚·‚é‚ÆTime‚Ì’è‹`‚ª‚ ‚â‚Ó‚â‚É‚È‚é
/// </summary>
public class TimeS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, -360 / 60* Time.deltaTime);
        
    }
}
