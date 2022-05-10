using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public GameObject kiraPrefab;

    void OnTriggerStay(Collider collision)
    {
        // デッドゾーンと当っていた場合
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("消した");
            // 生成位置
            Vector3 pos = collision.transform.position;
            // プレハブを指定位置に生成
            Instantiate(kiraPrefab, pos, Quaternion.identity);
            // this.transform.Translate(Vector3.right * speed);
            // GetComponent<GuardBT>().enabled = false;
            Destroy(collision.gameObject);
        }
        else
        {
            // Debug.Log("当ってない");
        }
    }
}
