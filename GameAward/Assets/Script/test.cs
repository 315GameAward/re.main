using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // ���͂��擾
        var _h = Input.GetAxis("Horizontal");
        var _v = Input.GetAxis("Vertical");

        // ���x�x�N�g�����쐬�i3�����p�j
        var _speed = new Vector3(_h, rb.velocity.y, _v);
        // ���x�ɐ��K�������x�N�g������
        rb.velocity = _speed.normalized;
    }
}