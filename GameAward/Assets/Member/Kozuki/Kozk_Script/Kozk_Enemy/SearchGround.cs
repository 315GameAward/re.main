using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchGround : MonoBehaviour
{
    ParticleManager fallparticle;
    public float speed;
    //OnCollisionExit()

    // ���������痣�ꂽ��
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            // �G�t�F�N�g�Ăяo��

            Debug.Log("�����痎���܂���");
        }
    }

    void OnTriggerStay(Collider collision)
    {
        // �f�b�h�]�[���Ɠ����Ă����ꍇ
        if (collision.CompareTag("DeadZone"))
        {
             Debug.Log("�����Ă�");
           
            this.transform.Translate(Vector3.right * speed);
            GetComponent<GuardBT>().enabled = false;
        }
            else
            {
               // Debug.Log("�����ĂȂ�");
            }
    }
}
