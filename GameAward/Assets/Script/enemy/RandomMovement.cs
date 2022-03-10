using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RandomMovement : MonoBehaviour
{
    private float chargeTime = 5.0f;
    private float timeCount;
    void Update()
    {
        timeCount += Time.deltaTime;
        // 自動で前進する。
        transform.position += transform.forward * Time.deltaTime;
        // 指定した時間を経過すると、
        if (timeCount > chargeTime)
        {
            // 進路をランダムに変更する。
            Vector3 course = new Vector3(0, Random.Range(0, 180), 0);
            transform.localRotation = Quaternion.Euler(course);
            // タイムカウントを0に戻す。
            timeCount = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //衝突判定
        if (collision.gameObject.tag == "test")
        {
            //スコア処理を追加
            FindObjectOfType<Score>().AddScore(10);

            //相手のタグがScicssorであるならば、自分を消すに変える
            Destroy(this.gameObject);
        }

    }
    //void GameC_O()
    //{
    //    if (Destroy(this.gameObject))
    //    {
    //        
    //    }
    //}
}