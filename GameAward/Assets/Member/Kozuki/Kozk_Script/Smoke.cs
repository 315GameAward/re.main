using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    private float timer = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, timer);
    }
}
