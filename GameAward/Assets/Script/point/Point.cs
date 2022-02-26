using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    //--- Unityに公開するパラメータ
    GameObject poitObj;

    //--- Unityに公開しないパラメータ

    // Start is called before the first frame update
    void Start()
    {
        poitObj = (GameObject)Resources.Load("point");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ポイントの追加
    public void AddPoint(Vector3 pos)
    {
        Instantiate(poitObj, pos, Quaternion.Euler(0,0,0));
        GameObject obj2 = new GameObject("cut obj", typeof(Point));
        poitObj = new GameObject("cut obj", typeof(Point));
    }

    // 地面から離れたとき
    void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }
}
