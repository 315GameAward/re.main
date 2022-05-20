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
    private List<Vector3> Objpos = new List<Vector3>();
    private float Deadtaime = 2.0f;
    //  private bool bDead = false;
    public string objName;
    private int i = 0;
   

    void OnTriggerEnter(Collider collision)
    {
        // �f�b�h�]�[���Ɠ����Ă����ꍇ
        if (collision.CompareTag("Enemy"))
        {
            // �����ʒu
            Objpos.Add(collision.gameObject.transform.position);

            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            Invoke(nameof(InsKira), Deadtaime);
            collision.gameObject.tag = "Untagged";
            // �G�l�~�[����
            Destroy(collision.gameObject, Deadtaime);
        }
        else
        {
            // Debug.Log("�����ĂȂ�");
        }
    }

    public void InsKira()
    {
        // �v���n�u���w��ʒu�ɐ���
        Instantiate(kiraPrefab, Objpos[i], Quaternion.identity);
        i++;
        Objpos.RemoveAt(i - 1);
        Debug.Log(Objpos.Count);
    }
}