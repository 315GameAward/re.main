using UnityEngine;
using System.Collections;

public class MoveEnemy : MonoBehaviour
{
    private Transform player;

    public enum EnemyState
    {
        Walk,
        Wait,
        Chase,
        Attack,
        Back
    };
    SearchGround ground;
    private CharacterController enemyController;

    //�@�ړI�n
    private Vector3 destination;
    //�@�����X�s�[�h
    [SerializeField]
    private float walkSpeed = 1.0f;
    //�@���x
    private Vector3 velocity;
    //�@�ړ�����
    private Vector3 direction;
    //�@�����t���O
    private bool arrived;
    //�@SetPosition�X�N���v�g
    private SetPosition setPosition;
    //�@�҂�����
    [SerializeField]
    private float waitTime = 5f;
    //�@�o�ߎ���
    private float elapsedTime;
    // �U���Ԋu����
    public float timeOut;
    private float timeTrigger;
    // �G�̏��
    public EnemyState state;

    //�@�v���C���[Transform
    private Transform playerTransform;
    // �������蔻��
    bool hitray;
    bool hitground;

    // Use this for initialization
    void Start()
    {
        enemyController = this.gameObject.GetComponent<CharacterController>();
        setPosition = this.gameObject.GetComponent<SetPosition>();
        setPosition.CreateRandomPosition(true);
        velocity = Vector3.zero;
        arrived = false;
        elapsedTime = 0f;
        SetState(EnemyState.Walk);
    }

    // Update is called once per frame
    void Update()
    {
        // �n�߂Ƀv���C���[�̈ʒu���擾�ł���悤�ɂ���
        player = GameObject.FindWithTag("Player").transform;

        //�@�����܂��̓L�����N�^�[��ǂ���������
        if (state == EnemyState.Walk || state == EnemyState.Chase || state == EnemyState.Attack)
        {
            //�@�L�����N�^�[��ǂ��������Ԃł���΃L�����N�^�[�̖ړI�n���Đݒ�
            if (state == EnemyState.Chase || state == EnemyState.Attack)
            {
                setPosition.SetDestination(new Vector3(playerTransform.position.x, 0.0f, playerTransform.position.z));
            }
            if (enemyController.isGrounded)
            {
                velocity = Vector3.zero;
                //animator.SetFloat("Walk", 1.0f);
                direction = (setPosition.GetDestination() - transform.position).normalized;
                transform.LookAt(new Vector3(setPosition.GetDestination().x, transform.position.y, setPosition.GetDestination().z));
                velocity = direction * walkSpeed;
            }

            //�@�ړI�n�ɓ����������ǂ����̔���
            if (Vector3.Distance(transform.position, setPosition.GetDestination()) < 0.5f)
            {
                SetState(EnemyState.Wait);
                //  animator.SetFloat("Walk", 0.0f);

            }
            //�@�������Ă������莞�ԑ҂�
        }
        else if (state == EnemyState.Wait)
        {
            elapsedTime += Time.deltaTime;

            //�@�҂����Ԃ��z�����玟�̖ړI�n��ݒ�
            if (elapsedTime > waitTime)
            {
                SetState(EnemyState.Walk);
            }
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        enemyController.Move(velocity * Time.deltaTime);
    }

    //�@�G�L�����N�^�[�̏�ԕύX���\�b�h
    public void SetState(EnemyState tempState, Transform targetObj = null)
    {
        if (tempState == EnemyState.Walk)
        {
            Debug.Log("���s");
            arrived = false;
            elapsedTime = 0f;
            state = tempState;
            setPosition.CreateRandomPosition(true);


        }
        else if (tempState == EnemyState.Chase)
        {
            Debug.Log("�ǂ�");
            state = tempState;
            //�@�ҋ@��Ԃ���ǂ�������ꍇ������̂�Off
            arrived = false;
            //�@�ǂ�������Ώۂ��Z�b�g
            playerTransform = targetObj;

        }
        else if (tempState == EnemyState.Wait)
        {
            Debug.Log("�ҋ@");
            elapsedTime = 0f;
            state = tempState;
            arrived = true;
            velocity = Vector3.zero;

            //    animator.SetFloat("Walk", 0f);
        }
        else if (tempState == EnemyState.Attack)
        {
            Debug.Log("�U��");
            elapsedTime = 0f;
            state = tempState;
            arrived = true;

            // ���Ԋu�ōU��
            if (Time.time > timeTrigger)
            {
                // �U��
                Debug.Log("�U�����܂���");
                Life.instance.DelLife();
                timeTrigger = Time.time + timeOut;
            }
        }
        else if (tempState == EnemyState.Back)
        {
            Debug.Log("������");
            setPosition.CreateRandomPosition(false);

        }
    }
    //�@�G�L�����N�^�[�̏�Ԏ擾���\�b�h
    public EnemyState GetState()
    {
        return state;
    }
}