using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IventPush_col_a : MonoBehaviour
{
    //
    public UnityEvent OnCollisioned = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        //�Փ˔���
        if (collision.gameObject.tag == "test")
        {
            //�X�R�A������ǉ�
            //FindObjectOfType<Score>().AddScore(10);
            //����̃^�O�������ł���Ȃ�΁A�����������ɕς���
            //�폜���ꂽ���_�ŃX�N���v�g��������̂ŁA
            //���̎��_�ŎQ�Ƃ͏o���Ȃ��Ȃ�
            Destroy(this.gameObject);

        }

    }

    //private void OnCollision()
    private void OnDestroy()
    {
        Debug.Log("�Ԃ�������I");
        OnCollisioned.Invoke();
    }
}