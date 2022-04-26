using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchGround : MonoBehaviour
{
    
    public float speed;
    //OnCollisionExit()
    void OnTriggerStay(Collider collision)
    {
        // 机と当っていた場合
        if (collision.CompareTag("Ground"))
        {
           // Debug.Log("当ってる");
            this.transform.Translate(Vector3.right * speed);
            GetComponent<GuardBT>().enabled = false;
        }
            else
            {
               // Debug.Log("当ってない");
            }
    }
}
