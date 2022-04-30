using UnityEngine;
using System.Collections;

public class SetPosition : MonoBehaviour
{
    private Rigidbody rb;
    private float distance;

    public float span = 120f;  // ray用、spanで測定間隔を測る
    private float times;// ray用、経過時間を当てはめる
    public bool isGround = true;
    //初期位置
    private Vector3 startPosition;
    //目的地
    private Vector3 destination;
    private Vector3 newDest;

    void Start()
    {
        //　初期位置を設定
        startPosition = transform.position;
        SetDestination(transform.position);

        rb = GetComponent<Rigidbody>();
        
        distance = 1.0f;
    }
    private void Update()
    {
        times += Time.deltaTime;    // 測定経過時間
        if(times > span) { 
        Vector3 rayPosition = transform.position + new Vector3(0.0f, -0.5f, 0.0f);
        Ray ray = new Ray(rayPosition, Vector3.down);
        isGround = Physics.Raycast(ray, distance);
        Debug.DrawRay(rayPosition, Vector3.down * distance, Color.red);

        Debug.Log(isGround);
        times = 0.0f;
        }
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

    //　目的地を取得する
    public Vector3 GetDestination()
    {
        if (isGround == false)
        {
            Debug.Log("反転します");
            destination.x = -destination.x;
            destination.z = -destination.z;
        }
        return destination;
    }
}
