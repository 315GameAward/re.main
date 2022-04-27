using UnityEngine;
using System.Collections;

public class SetPosition : MonoBehaviour
{
    private Rigidbody rb;
    private int upForce;
    private float distance;

    //初期位置
    private Vector3 startPosition;
    //目的地
    private Vector3 destination;

    void Start()
    {
        //　初期位置を設定
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

    //　ランダムな位置の作成
    public void CreateRandomPosition()
    {
        //　ランダムなVector2の値を得る
        var randDestination = Random.insideUnitCircle * 8;
        //　現在地にランダムな位置を足して目的地とする
        SetDestination(startPosition + new Vector3(randDestination.x, 0, randDestination.y));
    }

    //　目的地を設定する
    public void SetDestination(Vector3 position)
    {
        destination = position;
    }

    //　目的地を取得する
    public Vector3 GetDestination()
    {
        return destination;
    }
}
