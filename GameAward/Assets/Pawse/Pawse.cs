using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//プログラム説明
//別スクリプトでオブジェクトが削除されたことを監視する

public class Pawse : MonoBehaviour
{
    public RandomMovement target;

    void OnDisable()
    {
        target.OnDestroyed.RemoveAllListeners();
    }

    void OnEnable()
    {
        //中カッコ内にオブジェクトが消えた時の処理を加える
        target.OnDestroyed.AddListener
        (() =>
        {
           
        });
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //
    //}
    //
    //// Update is called once per frame
    //void Update()
    //{
    //   
    //}





}
