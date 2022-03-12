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
        // 入力を取得
        var _h = Input.GetAxis("Horizontal");
        var _v = Input.GetAxis("Vertical");

        // 速度ベクトルを作成（3次元用）
        var _speed = new Vector3(_h, rb.velocity.y, _v);
        // 速度に正規化したベクトルを代入
        rb.velocity = _speed.normalized;
    }
}