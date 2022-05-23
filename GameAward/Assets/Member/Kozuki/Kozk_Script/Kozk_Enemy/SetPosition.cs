using UnityEngine;
using System.Collections;

public class SetPosition : MonoBehaviour
{
    private Rigidbody rb;
    private float distance;

    [SerializeField]
    public float span = 0.0f;  // ray�p�Aspan�ő���Ԋu�𑪂�
    private float times = 0.0f;        // ray�p�A�o�ߎ��Ԃ𓖂Ă͂߂�
    public bool isGround = true;
    private bool Return = false;
    public Vector3 rayPosition = Vector3.zero; // �ύX�\
    private Vector3 RayPosition;
    public Vector3 rayVector = Vector3.down;
    //�����ʒu
    private Vector3 startPosition;
    //�ړI�n
    private Vector3 destination;
    private Vector3 newDest;

    private float groundtime = 1.0f;
    private float cnttime = 0.0f;

    void Start()
    {
        //�@�����ʒu��ݒ�
        startPosition = transform.position;
        SetDestination(transform.position);
        distance = 1.0f;
    }
    private void Update()
    {
        Vector3 trans = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
        times += Time.deltaTime;    // ����o�ߎ���

        Vector3 angle = transform.forward - rayVector;
        //
        if (times > span)
        {
            RayPosition = trans + rayPosition;
            Ray ray = new Ray(RayPosition, angle.normalized);
            isGround = Physics.Raycast(ray, distance);
            Debug.DrawRay(RayPosition, angle.normalized, Color.red);
            //Debug.Log(isGround);
            times = 0.0f;
        }

        if (Return == true)
        {
            cnttime += Time.deltaTime;
            if (cnttime > groundtime)
            {
                Return = false;
                cnttime = 0.0f;
            }
        }
        else { cnttime = 0.0f; }
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

    // �ǂ������悤
    public void SetDestinationPlayer(Vector3 position)
    {
        if (Return == true || isGround == false)
        {
            return;
        }
        destination = position;
    }

    //�@�ړI�n���擾����
    public Vector3 GetDestination()
    {
        if (Return == false && isGround == false)
        {
            Debug.Log("���]���܂�");
            destination.x = -destination.x;
            destination.z = -destination.z;
            Return = true;
        }
        return destination;
    }
}
