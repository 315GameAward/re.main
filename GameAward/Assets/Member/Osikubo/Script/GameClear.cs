using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour
{
    Animation anim;
    private GameObject[] ResultTest;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        ResultTest = GameObject.FindGameObjectsWithTag("kesigomu");
        // Debug.Log("消しゴムの数" + ResultTest.Length);
        if (ResultTest.Length == 0)
            anim.Play();
        {
            Debug.Log("アニメーション開始");
        }
    }
}
