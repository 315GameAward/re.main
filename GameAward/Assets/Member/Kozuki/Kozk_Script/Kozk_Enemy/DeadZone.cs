using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{

    void OnTriggerStay(Collider collision)
    {
        // デッドゾーンと当っていた場合
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("消した");

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
