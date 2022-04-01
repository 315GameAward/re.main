using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class col_Ivent : MonoBehaviour
{

    [SerializeField]
    GameObject gameoverUI;

    void OnCollisionEnter(Collision collision)
    {
        //衝突判定
        if (collision.gameObject.tag == "test")
        {
            //スコア処理を追加
            //FindObjectOfType<Score>().AddScore(10);
            //相手のタグが○○であるならば、自分を消すに変える
            //削除された時点でスクリプトも消えるので、
            //その時点で参照は出来なくなる
            gameoverUI.SetActive(true);

        }

    }


}
