using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchGround : MonoBehaviour
{
    
    public float speed;
    //OnCollisionExit()
    void OnTriggerStay(Collider collision)
    {
        // ���Ɠ����Ă����ꍇ
        if (collision.CompareTag("Ground"))
        {
            Debug.Log("�����Ă�");
            this.transform.Translate(Vector3.right * speed);
            GetComponent<GuardBT>().enabled = false;
        }
            else
            {
                Debug.Log("�����ĂȂ�");
            }
    }
}
