using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{

    void OnTriggerStay(Collider collision)
    {
        // �f�b�h�]�[���Ɠ����Ă����ꍇ
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("������");

            // this.transform.Translate(Vector3.right * speed);
            // GetComponent<GuardBT>().enabled = false;
            Destroy(collision.gameObject);
        }
        else
        {
            // Debug.Log("�����ĂȂ�");
        }
    }
}
