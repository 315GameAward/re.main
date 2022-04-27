using UnityEngine;
using System.Collections;

public class SetPosition : MonoBehaviour
{
    private Rigidbody rb;
    private int upForce;
    private float distance;

    //�����ʒu
    private Vector3 startPosition;
    //�ړI�n
    private Vector3 destination;

    void Start()
    {
        //�@�����ʒu��ݒ�
        startPosition = transform.position;
        SetDestination(transform.position);

        rb = GetComponent<Rigidbody>();
        upForce = 300;
        distance = 1.0f;
    }
    private void Update()
    {
        Vector3 rayPosition = transform.position + new Vector3(0.0f, 0.0f, 0.0f);
        Ray ray = new Ray(rayPosition, Vector3.down);
        bool isGround = Physics.Raycast(ray, distance);
        Debug.DrawRay(rayPosition, Vector3.down * distance, Color.red);

        if (Input.GetMouseButtonDown(0))
        {
            rb.AddForce(new Vector3(0, upForce, 0));
        }

        Debug.Log(isGround);
    }

    //�@�����_���Ȉʒu�̍쐬
    public void CreateRandomPosition()
    {
        //�@�����_����Vector2�̒l�𓾂�
        var randDestination = Random.insideUnitCircle * 8;
        //�@���ݒn�Ƀ����_���Ȉʒu�𑫂��ĖړI�n�Ƃ���
        SetDestination(startPosition + new Vector3(randDestination.x, 0, randDestination.y));
    }

    //�@�ړI�n��ݒ肷��
    public void SetDestination(Vector3 position)
    {
        destination = position;
    }

    //�@�ړI�n���擾����
    public Vector3 GetDestination()
    {
        return destination;
    }
}
