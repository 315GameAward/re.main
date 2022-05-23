using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class target : MonoBehaviour
{
    //true falseでおこなう
    //現在消しゴムに振れたらマーカーが消えてしまうので地面との接触が無くなったら
    //という風に変える
    private bool stay = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (stay == false)
        {
            Destroy(this.gameObject);

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        stay = true;

    }

    void OnCollisionExit(Collision collision)
    {

        stay = false;
    }


}
