//
//  ƒLƒ‰ƒLƒ‰ˆÚ“®
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiraMove : MonoBehaviour
{
    public Vector3 kiramove;

    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, timer);
    }

    // Update is called once per frame
    void Update()
    {
        // ˆÚ“®
        this.transform.Translate(kiramove, Space.Self);
    }
}
