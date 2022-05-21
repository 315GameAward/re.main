using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveLimit : MonoBehaviour
{
    Camera targetCamera;
    [SerializeField] Transform targetObj;

    Vector3 lb;
    Vector3 rt;
    // Start is called before the first frame update
    void Start()
    {
        targetCamera = GetComponent<Camera>();

        lb = GetRayhitPos(new Vector3(0, 0, 0)); // 左下
        rt = GetRayhitPos(new Vector3(1, 1, 0)); // 右上
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckInScreen())
            Debug.Log("画面内");
        else
            Debug.Log("画面外");

    }

    Vector3 GetRayhitPos(Vector3 targetPos)
    {
        Ray ray = targetCamera.ViewportPointToRay(targetPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            return hit.point;

        return Vector3.zero; // 何もヒットしなければ(0, 0, 0)を返す
    }

    // 対象のオブジェクトの位置から画面内かどうか判定して返す
    bool CheckInScreen()
    {
        // テスト環境のカメラが変な方向を向いてたので少しおかしな判定文になっています
        // ここの条件式は適宜、修正してください
        if (targetObj.position.x < rt.x || lb.x < targetObj.position.x || targetObj.position.z < lb.z || rt.z < targetObj.position.z)
            return false;

        return true;
    }
}
