using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public GameObject kiraPrefab;

    void OnTriggerStay(Collider collision)
    {
        // �f�b�h�]�[���Ɠ����Ă����ꍇ
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("������");
            // �����ʒu
            Vector3 pos = collision.transform.position;
            // �v���n�u���w��ʒu�ɐ���
            Instantiate(kiraPrefab, pos, Quaternion.identity);
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
