//
//  �L���L���ړ�
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiraMove : MonoBehaviour
{
    public Vector3 kiramove;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �ړ�
        this.transform.Translate(kiramove, Space.Self);
    }
}
