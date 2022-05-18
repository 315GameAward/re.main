using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kaiten : MonoBehaviour
{
    // Start is called before the first frame update
    float z;
    void Update()
    {
        z += Time.deltaTime * 100;
        transform.Rotate(0, z, 0);
        
        //transform.rotation = Quaternion.Euler(y, 0, 0);
        //gameObject.transform.localRotation = Quaternion.Euler(-90, 0, z);
    }
}
