using UnityEngine;
using System.Collections;

public class SetPosition : MonoBehaviour
{
    private Rigidbody rb;
    private float distance;

    [SerializeField]
    public float span = 0.0f;  // ray用、spanで測定間隔を測る
    private float times = 0.0f;        // ray用、経過時間を当てはめる
    public bool isGround = true;
    private bool Return = false;
    public Vector3 rayPosition = Vector3.zero; // 変更可能
    private Vector3 RayPosition;
    public Vector3 rayVector = Vector3.down;
    //初期位置
    private Vector3 startPosition;
    //目的地
    private Vector3 destination;
    private Vector3 newDest;

    private float groundtime = 1.0f;
    private float cnttime = 0.0f;

    void Start()
    {
        //　初期位置を設定
        startPosition = transform.position;
        SetDestination(transform.position);
        distance = 1.0f;
    }
    private void Update()
    {
        Vector3 trans = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
        times += Time.deltaTime;    // 測定経過時間

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

    //　ランダムな位置の作成
    public void CreateRandomPosition(bool on)
    {
        if (on)
        {
            var randDestination = Random.insideUnitCircle * 8;
            //　現在地にランダムな位置を足して目的地とする
            SetDestination(startPosition + new Vector3(randDestination.x, 0, randDestination.y));
        }
        else
        {
            float retu = Random.Range(-5, 5);

            newDest = new Vector3(destination.x, destination.y + 180 + retu, destination.z);
            SetDestination(newDest);
        }
    }

    //　目的地を設定する
    public void SetDestination(Vector3 position)
    {
        destination = position;
    }

    // 追いかけよう
    public void SetDestinationPlayer(Vector3 position)
    {
        if (Return == true || isGround == false)
        {
            return;
        }
        destination = position;
    }

    //　目的地を取得する
    public Vector3 GetDestination()
    {
        if (Return == false && isGround == false)
        {
            Debug.Log("反転します");
            destination.x = -destination.x;
            destination.z = -destination.z;
            Return = true;
        }
        return destination;
    }
}
