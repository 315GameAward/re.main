//
//  デッドゾーン
//
//  エネミーが落ちた時の処理を記します

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public GameObject kiraPrefab;
    private Vector3 pos;
    private float Deadtaime = 5.0f;
    private bool bDead = false;

    void OnTriggerStay(Collider collision)
    {
        // デッドゾーンと当っていた場合
        if (collision.CompareTag("Enemy"))
        {
            // 生成位置
            pos = collision.transform.position;
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            Invoke(nameof(InsKira), Deadtaime);

            // エネミー消去
            Destroy(collision.gameObject,Deadtaime);
        }
        else
        {
            // Debug.Log("当ってない");
        }
    }

    public void InsKira()
    {
       if(bDead == true) { return; }

        // プレハブを指定位置に生成
        Instantiate(kiraPrefab, pos, Quaternion.identity);

        bDead = true;
    }
}
