using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class col_Ivent : MonoBehaviour
{

    [SerializeField]
    GameObject gameoverUI;

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
            gameoverUI.SetActive(true);

        }

    }


}
