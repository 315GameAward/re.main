using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageLine : MonoBehaviour
{
    // �ϐ��錾
    static float delayTime = 0.0f;
    public bool damage = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    // ���̃I�u�W�F�N�g�̍폜
    public void Destroy()
    {
        Destroy(gameObject);
    }

    // �����蔻��
    private void OnTriggerEnter(Collider other)
    {
        // �G�Ɠ���������
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("�������Ă���");
            damage = true;
        }
        else
        {
            damage = false;
        }
    }
}
