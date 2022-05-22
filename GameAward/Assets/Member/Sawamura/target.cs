using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class target : MonoBehaviour
{
    //true falseでおこなう
    public UnityEvent OnCollisioned = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionExit(Collision collision)
    {
        //衝突判定
        if (collision.gameObject.tag == "Ground")
        {
            //スコア処理を追加
            //FindObjectOfType<Score>().AddScore(10);
            //相手のタグが○○であるならば、自分を消すに変える
            //削除された時点でスクリプトも消えるので、
            //その時点で参照は出来なくなる
            Destroy(this.gameObject);

        }

    }

    //private void OnCollision()
    //private void OnDestroy()
    //{
    //    Debug.Log("ぶつかったよ！");
    //    OnCollisioned.Invoke();
    //}
}
