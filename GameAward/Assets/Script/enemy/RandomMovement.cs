using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//�v���O��������
//��莞�ԂŐi�H�������_���ɕύX���Ȃ��瓮��
//"test"�Ƃ����^�O���̂����I�u�W�F�N�g�ƐڐG�����Ƃ��ɍ폜����
//���̃X�N���v�g���A�^�b�`�����I�u�W�F�N�g�����������Ƃ����m����

//��L�̕��͍폜

public class RandomMovement : MonoBehaviour
{
    
    private float chargeTime = 5.0f;
    private float timeCount;
    void Update()
    {
        //timeCount += Time.deltaTime;
        //// �����őO�i����B
        //transform.position += transform.forward * Time.deltaTime;
        //// �w�肵�����Ԃ��o�߂���ƁA
        //if (timeCount > chargeTime)
        //{
        //    // �i�H�������_���ɕύX����B
        //    Vector3 course = new Vector3(0, Random.Range(0, 180), 0);
        //    transform.localRotation = Quaternion.Euler(course);
        //    // �^�C���J�E���g��0�ɖ߂��B
        //    timeCount = 0;
        //}
    }
    //***��肽������***
    //�����łԂ��������I�u�W�F�N�g�������ď������̂ł���΁A
    //�Q�[���X�e�[�^�X���Q�[���I�[�o�[�ɕς���
    //�Q�[���}�l�[�W���[���ŃQ�[���I�[�o�[�̃e�L�X�g��\������
    void OnCollisionEnter(Collision collision)
    {
        //�Փ˔���
        if (collision.gameObject.tag == "test")
        {
            //�X�R�A������ǉ�
            FindObjectOfType<Score>().AddScore(10);
            //����̃^�O�������ł���Ȃ�΁A�����������ɕς���
            //�폜���ꂽ���_�ŃX�N���v�g��������̂ŁA
            //���̎��_�ŎQ�Ƃ͏o���Ȃ��Ȃ�
            Destroy(this.gameObject);
            
        }

    }
}