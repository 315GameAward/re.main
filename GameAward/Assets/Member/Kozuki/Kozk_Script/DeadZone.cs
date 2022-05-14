//
//  �f�b�h�]�[��
//
//  �G�l�~�[�����������̏������L���܂�

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
        // �f�b�h�]�[���Ɠ����Ă����ꍇ
        if (collision.CompareTag("Enemy"))
        {
            // �����ʒu
            pos = collision.transform.position;
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            Invoke(nameof(InsKira), Deadtaime);

            // �G�l�~�[����
            Destroy(collision.gameObject,Deadtaime);
        }
        else
        {
            // Debug.Log("�����ĂȂ�");
        }
    }

    public void InsKira()
    {
       if(bDead == true) { return; }

        // �v���n�u���w��ʒu�ɐ���
        Instantiate(kiraPrefab, pos, Quaternion.identity);

        bDead = true;
    }
}
