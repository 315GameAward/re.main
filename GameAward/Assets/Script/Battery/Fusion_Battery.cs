//================================================
//
//      Fusion_Battery.cs
//      �o�b�e���[���m�̏Փ�
//
//------------------------------------------------
//      �쐬��: �ĎR꣑��Y
//      �쐬�J�n���F2022/4/25(��)
//================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fusion_Battery : MonoBehaviour
{
    // ���d�I�u�W�F�N�g�̏��i�[�p(��) )�قڂ����
    public static List<GameObject> ball_list = new List<GameObject>();
    // �o�b�e���[�C���X�^���X
    Battery battery = new Battery();
    Vector3 pos;    // ���d�������W

    // Start is called before the first frame update
    void Start()
    {
        // ���d�v���n�u���̃��[�h
        disCharge.Energy_Ball = (GameObject)Resources.Load("Energy_Ball");
    }

    // Update is called once per frame
    void Update()
    {
        battery.ChargeEnergy();

        // ���d���Ă鎞�͏������Ȃ�
        if (disCharge.EBClone != null)
            return;

        // �o�b�e���[�ړ�(��)
        battery.fmove = 0.05f;
        transform.position += new Vector3(battery.fmove, 0, 0);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Battery_Minus")
            return;
        if (ChargeEnergy.Energy < 100.0f && Battery.Energy < 100.0f)
            return;

        // �ړ��ʂ�0��
        battery.fmove = 0.0f;
        ChargeEnergy.move = 0.0f;

        // �o�b�e���[�̃v���X�ƃ}�C�i�X�̐^�񒆂ɕ��d���o��������
        pos = (transform.position + collision.transform.position) / 2;
        pos.y = 0;

        // �N���[���̃C���X�^���X���擾
        disCharge.EBClone = Instantiate(disCharge.Energy_Ball, pos, Quaternion.identity);
        ball_list.Add(disCharge.EBClone);
    }

    // ==================
    // �o�b�e���[�N���X
    // ==================
    public class Battery
    {
        public float fmove;         // �ړ����x

        public static float Energy; // �G�l���M�[����

        // �R���X�g���N�^
        public Battery()
        {
            fmove = 0.05f;
            Energy = 0;
        }

        // �G�l���M�[����
        public void ChargeEnergy()
        {
            if (Energy >= 100)
            {
                Energy = 100;
                return;
            }
            ++Energy;
        }
    }
}
