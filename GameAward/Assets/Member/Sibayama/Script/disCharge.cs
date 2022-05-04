//================================================
//
//      disCharge.cs
//      ���d����
//
//------------------------------------------------
//      �쐬��: �ĎR꣑��Y
//      �쐬�J�n���F2022/4/27(��)
//================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disCharge : MonoBehaviour
{

    public static GameObject Energy_Ball;   // �o�b�e���[�ɓn���I�u�W�F�N�g�����p�̏��
    public static GameObject EBClone;       // ���������I�u�W�F�N�g�̏��

    public float SetRealTime;   // ���d��������
    float CountFrame;           // �t���[���̃J�E���g
    float CountTimer;           // �������Ԃ̃t���[���\�L

    public string Tagname;      // �^�O�̖��O

    void Awake()
    {
        // �������Ԃ����A���^�C���ݒ�
        QualitySettings.vSyncCount = 0;
        CountFrame = Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
        CountTimer = SetRealTime * CountFrame;
    }

    // Update is called once per frame
    void Update()
    {
        DestroyTimer();
    }

    // �n�T�~�Ƃ̓����蔻��
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != Tagname)
            return;

        // �����Ƀn�T�~�̈ړ��ʂ�0�ɂ���A
        // �܂��̓t���O�𗧂Ă鏈�����L��
        //movePlayer.move = 0.0f;


    }

    // ��莞�Ԃŕ��d��������
    void DestroyTimer()
    {
        if (CountTimer < 0.0f)
        {
            CountTimer = 0.0f;
            Fusion_Battery.ball_list.Remove(EBClone);
            // �G�l���M�[���Z�b�g
            ChargeEnergy.Energy = 0;
            Fusion_Battery.Battery.Energy = 0;
            // ���d�I�u�W�F�N�g�폜
            Destroy(gameObject);
            return;
        }
        --CountTimer;
    }
}

