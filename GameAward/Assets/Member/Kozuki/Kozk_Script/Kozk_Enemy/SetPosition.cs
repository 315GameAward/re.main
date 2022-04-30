using UnityEngine;
using System.Collections;

public class SetPosition : MonoBehaviour
{
    private Rigidbody rb;
    private float distance;

    public float span = 120f;  // ray�p�Aspan�ő���Ԋu�𑪂�
    private float times;// ray�p�A�o�ߎ��Ԃ𓖂Ă͂߂�
    public bool isGround = true;
    //�����ʒu
    private Vector3 startPosition;
    //�ړI�n
    private Vector3 destination;
    private Vector3 newDest;

    void Start()
    {
        //�@�����ʒu��ݒ�
        startPosition = transform.position;
        SetDestination(transform.position);

        rb = GetComponent<Rigidbody>();
        
        distance = 1.0f;
    }
    private void Update()
    {
        times += Time.deltaTime;    // ����o�ߎ���
        if(times > span) { 
        Vector3 rayPosition = transform.position + new Vector3(0.0f, -0.5f, 0.0f);
        Ray ray = new Ray(rayPosition, Vector3.down);
        isGround = Physics.Raycast(ray, distance);
        Debug.DrawRay(rayPosition, Vector3.down * distance, Color.red);

        Debug.Log(isGround);
        times = 0.0f;
        }
    }

    //�@�����_���Ȉʒu�̍쐬
    public void CreateRandomPosition(bool on)
    {
        if (on)
        { 
        var randDestination = Random.insideUnitCircle * 8;
        //�@���ݒn�Ƀ����_���Ȉʒu�𑫂��ĖړI�n�Ƃ���
        SetDestination(startPosition + new Vector3(randDestination.x, 0, randDestination.y));
        }
        else
        {
            float retu = Random.Range(-5, 5);

            newDest = new Vector3(destination.x, destination.y + 180 + retu, destination.z);
            SetDestination(newDest);
        }
    }

    //�@�ړI�n��ݒ肷��
    public void SetDestination(Vector3 position)
    {
        destination = position;
    }

    //�@�ړI�n���擾����
    public Vector3 GetDestination()
    {
        if (isGround == false)
        {
            Debug.Log("���]���܂�");
            destination.x = -destination.x;
            destination.z = -destination.z;
        }
        return destination;
    }
}
