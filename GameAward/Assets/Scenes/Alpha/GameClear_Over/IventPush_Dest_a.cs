using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class IventPush_Dest_a : MonoBehaviour
{
    public UnityEvent OnDestroyed = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ���̃I�u�W�F�N�g���폜
        if (gameObject.transform.position.y < -1)
        {
            //FindObjectOfType<Score>().AddScore(10);
            Destroy(this.gameObject);
        }

    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    //�Փ˔���
    //    if (collision.gameObject.tag == "test")
    //    {
    //        //�X�R�A������ǉ�
    //        FindObjectOfType<Score>().AddScore(20);
    //        //����̃^�O�������ł���Ȃ�΁A�����������ɕς���
    //        //�폜���ꂽ���_�ŃX�N���v�g��������̂ŁA
    //        //���̎��_�ŎQ�Ƃ͏o���Ȃ��Ȃ�
    //        Destroy(this.gameObject);
    //
    //    }
    //
    //}

    private void OnDestroy()
    {
        Debug.Log("�����ꂽ��I");
        OnDestroyed.Invoke();
    }

}
