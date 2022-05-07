using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer_UI : MonoBehaviour
{
    private Transform Timer_StickTransform;

    // Start is called before the first frame update
    private void Awake()
    {
        Timer_StickTransform = transform.Find("Timer_Stick");
    }

    // Update is called once per frame
    private void Update()
    {
        Timer_StickTransform.eulerAngles = new Vector3(0, 0, -Time.realtimeSinceStartup * 90f);
    }
}
