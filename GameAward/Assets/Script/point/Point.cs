using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    //--- Unity�Ɍ��J����p�����[�^
    GameObject poitObj;

    //--- Unity�Ɍ��J���Ȃ��p�����[�^

    // Start is called before the first frame update
    void Start()
    {
        poitObj = (GameObject)Resources.Load("point");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �|�C���g�̒ǉ�
    public void AddPoint(Vector3 pos)
    {
        Instantiate(poitObj, pos, Quaternion.Euler(0,0,0));
        GameObject obj2 = new GameObject("cut obj", typeof(Point));
        poitObj = new GameObject("cut obj", typeof(Point));
    }

    // �n�ʂ��痣�ꂽ�Ƃ�
    void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }
}
