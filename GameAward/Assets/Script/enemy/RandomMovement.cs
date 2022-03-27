using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//プログラム説明
//一定時間で進路をランダムに変更しながら動く
//"test"というタグ名のついたオブジェクトと接触したときに削除する
//このスクリプトをアタッチしたオブジェクトが消えたことを告知する

//上記の物は削除

public class RandomMovement : MonoBehaviour
{
    
    private float chargeTime = 5.0f;
    private float timeCount;
    void Update()
    {
        //timeCount += Time.deltaTime;
        //// 自動で前進する。
        //transform.position += transform.forward * Time.deltaTime;
        //// 指定した時間を経過すると、
        //if (timeCount > chargeTime)
        //{
        //    // 進路をランダムに変更する。
        //    Vector3 course = new Vector3(0, Random.Range(0, 180), 0);
        //    transform.localRotation = Quaternion.Euler(course);
        //    // タイムカウントを0に戻す。
        //    timeCount = 0;
        //}
    }
    //***やりたいこと***
    //ここでぶつかった時オブジェクトが消えて消えたのであれば、
    //ゲームステータスをゲームオーバーに変えて
    //ゲームマネージャー側でゲームオーバーのテキストを表示する
    void OnCollisionEnter(Collision collision)
    {
        //衝突判定
        if (collision.gameObject.tag == "test")
        {
            //スコア処理を追加
            FindObjectOfType<Score>().AddScore(10);
            //相手のタグが○○であるならば、自分を消すに変える
            //削除された時点でスクリプトも消えるので、
            //その時点で参照は出来なくなる
            Destroy(this.gameObject);
            
        }

    }
}