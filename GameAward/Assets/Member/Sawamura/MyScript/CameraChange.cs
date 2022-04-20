
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{

    private GameObject MainCamera;      //メインカメラ格納用

    //使わないカメラはコメントアウトしてね
    private GameObject Camera2;       //カメラ2格納用 
    //private GameObject Camera3;       //カメラ3格納用 
    //private GameObject Camera4;       //カメラ4格納用 
    //private GameObject Camera5;       //カメラ5格納用 


    //呼び出し時に実行される関数
    void Start()
    {
        //メインカメラを取得
        MainCamera = GameObject.Find("Main Camera");

        //使わないカメラはコメントアウトしてね
        Camera2 = GameObject.Find("CameraBeside");
        //Camera3 = GameObject.Find("Camera3");
        //Camera4 = GameObject.Find("Camera4");
        //Camera5 = GameObject.Find("Camera5");


        //カメラ2以降を非アクティブに。使わないカメラはコメントアウトしてね
        Camera2.SetActive(false);
        //Camera3.SetActive(false);
        //Camera4.SetActive(false);
        //Camera5.SetActive(false);
    }


    //単位時間ごとに実行される関数
    void Update()
    {

        //キーボード上1キーが押されたら、メインカメラをアクティブに
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //メインカメラをアクティブに設定
            MainCamera.SetActive(true);

            //他のカメラを非アクティブに。使わないカメラはコメントアウトしてね
            Camera2.SetActive(false);
            //Camera3.SetActive(false);
            //Camera4.SetActive(false);
            //Camera5.SetActive(false);
        }


        //【カメラ2を使わない場合コメントアウトここから】
        //キーボード上2キーが押されたら、カメラ2をアクティブに
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //カメラ2をアクティブに設定
            Camera2.SetActive(true);

            //他のカメラを非アクティブに。使わないカメラはコメントアウトしてね
            MainCamera.SetActive(false);
            //Camera3.SetActive(false);
            //Camera4.SetActive(false);
            //Camera5.SetActive(false);
        }
        //【カメラ2を使わない場合コメントアウトここまで】


        //【カメラ3を使わない場合コメントアウトここから】
        //キーボード上3キーが押されたら、カメラ3をアクティブに
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    //カメラ3をアクティブに設定
        //    Camera3.SetActive(true);

        //    //他のカメラを非アクティブに。使わないカメラはコメントアウトしてね
        //    MainCamera.SetActive(false);
        //    Camera2.SetActive(false);
        //    Camera4.SetActive(false);
        //    Camera5.SetActive(false);
        //}
        ////【カメラ3を使わない場合コメントアウトここまで】


        ////【カメラ4を使わない場合コメントアウトここから】
        ////キーボード上4キーが押されたら、カメラ4をアクティブに
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    //カメラ4をアクティブに設定
        //    Camera4.SetActive(true);

        //    //他のカメラを非アクティブに。使わないカメラはコメントアウトしてね
        //    MainCamera.SetActive(false);
        //    Camera2.SetActive(false);
        //    Camera3.SetActive(false);
        //    Camera5.SetActive(false);
        //}
        ////【カメラ4を使わない場合コメントアウトここまで】


        ////【カメラ5を使わない場合コメントアウトここから】
        ////キーボード上5キーが押されたら、カメラ5をアクティブに
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    //カメラ5をアクティブに設定
        //    Camera5.SetActive(true);

        //    //他のカメラを非アクティブに。使わないカメラはコメントアウトしてね
        //    MainCamera.SetActive(false);
        //    Camera2.SetActive(false);
        //    Camera3.SetActive(false);
        //    Camera4.SetActive(false);
        //}
        ////【カメラ5を使わない場合コメントアウトここまで】

    }
}