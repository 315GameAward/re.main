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
    private List<Vector3> Objpos = new List<Vector3>();
    private float Deadtaime = 1.0f;
    //  private bool bDead = false;
    public string objName;
    private int i = 0;


    void OnTriggerEnter(Collider collision)
    {
        // デッドゾーンと当っていた場合
        if (collision.CompareTag("Enemy"))
        {
            // 生成位置
            Objpos.Add(collision.gameObject.transform.position);

            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            Invoke(nameof(InsKira), Deadtaime);
            collision.gameObject.tag = "Untagged";

            // スコア加算
            Score.instance.AddScore(5);

            // エネミー消去
            Destroy(collision.gameObject, Deadtaime);
        }
        else if (collision.CompareTag("Don'tFall"))
        {
            // 生成位置
            Objpos.Add(collision.gameObject.transform.position);

            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            // Invoke(nameof(InsKira), Deadtaime);
            collision.gameObject.tag = "Untagged";

            // スコア加算
            Score.instance.AddScore(-5);

            // ライフ減少
            Life.instance.DelLife();

            // エネミー消去
            Destroy(collision.gameObject, Deadtaime);
        }
    }

    public void InsKira()
    {
        // プレハブを指定位置に生成
        Instantiate(kiraPrefab, Objpos[i], Quaternion.identity);
        i++;
        Debug.Log(Objpos.Count);
    }
}