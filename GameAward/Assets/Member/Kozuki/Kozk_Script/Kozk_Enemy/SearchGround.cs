using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchGround : MonoBehaviour
{
    ParticleManager fallparticle;
    public float speed;
    //OnCollisionExit()

    // もし床から離れたら
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            // エフェクト呼び出し

            Debug.Log("床から落ちました");
        }
    }

    void OnTriggerStay(Collider collision)
    {
        // デッドゾーンと当っていた場合
        if (collision.CompareTag("DeadZone"))
        {
             Debug.Log("当ってる");
           
            this.transform.Translate(Vector3.right * speed);
            GetComponent<GuardBT>().enabled = false;
        }
            else
            {
               // Debug.Log("当ってない");
            }
    }
}
