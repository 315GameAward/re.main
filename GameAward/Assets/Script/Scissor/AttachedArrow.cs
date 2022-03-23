using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachedArrow : MonoBehaviour
{
    public Vector3 PlayerRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerRotation.y += 0.001f;

        //transform.rotation = PlayerRotation;
    }
}


