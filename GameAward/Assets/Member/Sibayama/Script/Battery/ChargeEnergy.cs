//================================================
//
//      ChargeEnergy.cs
//      �G�l���M�[���߂鏈��
//
//------------------------------------------------
//      �쐬��: �ĎR꣑��Y
//      �쐬�J�n���F2022/05/05(��)
//================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEnergy : MonoBehaviour
{
    public static float Energy = 0; // �G�l���M�[����
    public static float move;
    // Start is called before the first frame update
    void Start()
    {
        move = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        Charge();
        // ���d���Ă鎞�͏������Ȃ�
        if (disCharge.EBClone != null)
            return;
        move = 0.05f;
        transform.position -= new Vector3(move, 0.0f, 0.0f);
    }

    // �G�l���M�[����
    void Charge()
    {
        if (Energy >= 100)
        {
            Energy = 100;
            return;
        }
        ++Energy;
    }
}
