using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// /�N���X����Time�ɂ����Time�̒�`������ӂ�ɂȂ�
/// </summary>
public class TimeS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 360 / 60* Time.deltaTime);
    }
}
