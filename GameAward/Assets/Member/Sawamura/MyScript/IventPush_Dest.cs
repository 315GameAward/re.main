using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class IventPush_Dest : MonoBehaviour
{
    public UnityEvent OnDestroyed = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        //衝突判定
        if (collision.gameObject.tag == "test")
        {
            //スコア処理を追加
            FindObjectOfType<Score>().AddScore(20);
            //相手のタグが○○であるならば、自分を消すに変える
            //削除された時点でスクリプトも消えるので、
            //その時点で参照は出来なくなる
            Destroy(this.gameObject);

        }

    }

    private void OnDestroy()
    {
        Debug.Log("消されたよ！");
        OnDestroyed.Invoke();
    }

}
