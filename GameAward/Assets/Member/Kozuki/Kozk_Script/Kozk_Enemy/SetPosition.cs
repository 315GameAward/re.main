using UnityEngine;
using System.Collections;

public class SetPosition : MonoBehaviour
{
    private Rigidbody rb;
    private float distance;

    public float span = 120f;  // ray用、spanで測定間隔を測る
    private float times;// ray用、経過時間を当てはめる
    public bool isGround = true;
    public Vector3 rayPosition = Vector3.zero; // 変更可能
    private Vector3 RayPosition;
    public Vector3 rayVector = Vector3.down;
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
        distance = 1.0f;
    }
    private void Update()
    {
        Vector3 trans = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
        times += Time.deltaTime;    // 測定経過時間
        if (times > span)
        {
            RayPosition = trans + rayPosition;
            Ray ray = new Ray(RayPosition, rayVector);
            isGround = Physics.Raycast(ray, distance);
            Debug.DrawRay(RayPosition, rayVector * distance, Color.red);

            // Debug.Log(isGround);
            times = 0.0f;
        }
        Debug.DrawRay(RayPosition, rayVector * distance, Color.red);
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
