using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageLine : MonoBehaviour
{
    // 変数宣言
    static float delayTime = 0.0f;
    public bool damage = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    // このオブジェクトの削除
    public void Destroy()
    {
        Destroy(gameObject);
    }

    // 当たり判定
    private void OnTriggerEnter(Collider other)
    {
        // 敵と当たったら
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("当たっている");
            damage = true;
        }
        else
        {
            damage = false;
        }
    }
}
