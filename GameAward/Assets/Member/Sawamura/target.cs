using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class target : MonoBehaviour
{
    //true false�ł����Ȃ�
    //���ݏ����S���ɐU�ꂽ��}�[�J�[�������Ă��܂��̂Œn�ʂƂ̐ڐG�������Ȃ�����
    //�Ƃ������ɕς���
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
