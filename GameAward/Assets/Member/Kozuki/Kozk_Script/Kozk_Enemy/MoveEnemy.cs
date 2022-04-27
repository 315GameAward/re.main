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
    private Animator animator;
    //　目的地
    private Vector3 destination;
    //　歩くスピード
    [SerializeField]
    private float walkSpeed = 1.0f;
    //　速度
    private Vector3 velocity;
    //　移動方向
    private Vector3 direction;
    //　到着フラグ
    private bool arrived;
    //　SetPositionスクリプト
    private SetPosition setPosition;
    //　待ち時間
    [SerializeField]
    private float waitTime = 5f;
    //　経過時間
    private float elapsedTime;
    // 攻撃間隔時間
    public float timeOut;
    private float timeTrigger;
    // 敵の状態
    private EnemyState state;
    //　プレイヤーTransform
    private Transform playerTransform;
    // 床当たり判定
    bool hitray;
    bool hitground;

    // Use this for initialization
    void Start()
    {
        enemyController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        setPosition = GetComponent<SetPosition>();
        setPosition.CreateRandomPosition(true);
        velocity = Vector3.zero;
        arrived = false;
        elapsedTime = 0f;
        SetState(EnemyState.Walk);
    }

    // Update is called once per frame
    void Update()
    {
        // 始めにプレイヤーの位置を取得できるようにする
        player = GameObject.FindWithTag("Player").transform;

        //　見回りまたはキャラクターを追いかける状態
        if (!(state == EnemyState.Attack) && state == EnemyState.Walk || state == EnemyState.Chase)
        {
            //　キャラクターを追いかける状態であればキャラクターの目的地を再設定
            if (state == EnemyState.Chase)
            {
                setPosition.SetDestination(playerTransform.position);
            }
            if (enemyController.isGrounded)
            {
                velocity = Vector3.zero;
               // animator.SetFloat("Speed", 2.0f);
                direction = (setPosition.GetDestination() - transform.position).normalized;
                transform.LookAt(new Vector3(setPosition.GetDestination().x, transform.position.y, setPosition.GetDestination().z));
                velocity = direction * walkSpeed;
            }

            //　目的地に到着したかどうかの判定
            if (Vector3.Distance(transform.position, setPosition.GetDestination()) < 0.5f)
            {
                SetState(EnemyState.Wait);
               // animator.SetFloat("Speed", 0.0f);
            }
            //　到着していたら一定時間待つ
        }
        else if (state == EnemyState.Wait)
        {
            elapsedTime += Time.deltaTime;

            //　待ち時間を越えたら次の目的地を設定
            if (elapsedTime > waitTime)
            {
                SetState(EnemyState.Walk);
            }
        }
        
        velocity.y += Physics.gravity.y * Time.deltaTime;
        enemyController.Move(velocity * Time.deltaTime);
        Dosperecast();  // 床判定取得
    
    }

    //　敵キャラクターの状態変更メソッド
    public void SetState(EnemyState tempState, Transform targetObj = null)
    {
        if (tempState == EnemyState.Walk)
        {
            arrived = false;
            elapsedTime = 0f;
            state = tempState;
            setPosition.CreateRandomPosition( true );
        }
        else if (tempState == EnemyState.Chase)
        {
            state = tempState;
            //　待機状態から追いかける場合もあるのでOff
            arrived = false;
            //　追いかける対象をセット
            playerTransform = targetObj;
        }
        else if (tempState == EnemyState.Wait)
        {
            elapsedTime = 0f;
            state = tempState;
            arrived = true;
            velocity = Vector3.zero;
           // animator.SetFloat("Speed", 0f);
        }
        else if (tempState == EnemyState.Attack)
        {
            elapsedTime = 0f;
            state = tempState;
            arrived = true;


            // 一定間隔で攻撃
            if (Time.time > timeTrigger)
            {
                // 攻撃
                Debug.Log("攻撃しました");
                timeTrigger = Time.time + timeOut;
            }
        }
        else if (tempState == EnemyState.Back)
        {
            setPosition.CreateRandomPosition(false);

        }
    }
    //　敵キャラクターの状態取得メソッド
    public EnemyState GetState()
    {
        return state;
    }
    void Dosperecast()
    {
        int mask = LayerMask.GetMask("Default");
        RaycastHit rayhit;
        rayhit = new RaycastHit();
        Vector3 hiku;
        hiku = new Vector3(0, 1, 0);
        float en = 0.1f;
        Vector3 angle = transform.forward - hiku;
        hitray = Physics.SphereCast(transform.position, en, angle.normalized, out rayhit, 1, mask, QueryTriggerInteraction.Ignore);
        //hitray = Physics.Raycast(controller.transform.position, angle.normalized, out rayhit, 1, mask, QueryTriggerInteraction.Ignore,);
        Vector3 foll;
        foll = new Vector3(0, -1, 0);
        hitground = Physics.Raycast(transform.position, foll, out rayhit, 1, mask, QueryTriggerInteraction.Ignore);

        //Debug.DrawRay(controller.transform.position, angle.normalized, Color.green);
        // Debug.DrawRay(controller.transform.position, angle.normalized, Color.green);

    }
}
