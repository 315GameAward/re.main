
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{

    private GameObject MainCamera;      //���C���J�����i�[�p

    //�g��Ȃ��J�����̓R�����g�A�E�g���Ă�
    private GameObject Camera2;       //�J����2�i�[�p 
    //private GameObject Camera3;       //�J����3�i�[�p 
    //private GameObject Camera4;       //�J����4�i�[�p 
    //private GameObject Camera5;       //�J����5�i�[�p 


    //�Ăяo�����Ɏ��s�����֐�
    void Start()
    {
        //���C���J�������擾
        MainCamera = GameObject.Find("Main Camera");

        //�g��Ȃ��J�����̓R�����g�A�E�g���Ă�
        Camera2 = GameObject.Find("CameraBeside");
        //Camera3 = GameObject.Find("Camera3");
        //Camera4 = GameObject.Find("Camera4");
        //Camera5 = GameObject.Find("Camera5");


        //�J����2�ȍ~���A�N�e�B�u�ɁB�g��Ȃ��J�����̓R�����g�A�E�g���Ă�
        Camera2.SetActive(false);
        //Camera3.SetActive(false);
        //Camera4.SetActive(false);
        //Camera5.SetActive(false);
    }


    //�P�ʎ��Ԃ��ƂɎ��s�����֐�
    void Update()
    {

        //�L�[�{�[�h��1�L�[�������ꂽ��A���C���J�������A�N�e�B�u��
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //���C���J�������A�N�e�B�u�ɐݒ�
            MainCamera.SetActive(true);

            //���̃J�������A�N�e�B�u�ɁB�g��Ȃ��J�����̓R�����g�A�E�g���Ă�
            Camera2.SetActive(false);
            //Camera3.SetActive(false);
            //Camera4.SetActive(false);
            //Camera5.SetActive(false);
        }


        //�y�J����2���g��Ȃ��ꍇ�R�����g�A�E�g��������z
        //�L�[�{�[�h��2�L�[�������ꂽ��A�J����2���A�N�e�B�u��
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //�J����2���A�N�e�B�u�ɐݒ�
            Camera2.SetActive(true);

            //���̃J�������A�N�e�B�u�ɁB�g��Ȃ��J�����̓R�����g�A�E�g���Ă�
            MainCamera.SetActive(false);
            //Camera3.SetActive(false);
            //Camera4.SetActive(false);
            //Camera5.SetActive(false);
        }
        //�y�J����2���g��Ȃ��ꍇ�R�����g�A�E�g�����܂Łz


        //�y�J����3���g��Ȃ��ꍇ�R�����g�A�E�g��������z
        //�L�[�{�[�h��3�L�[�������ꂽ��A�J����3���A�N�e�B�u��
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    //�J����3���A�N�e�B�u�ɐݒ�
        //    Camera3.SetActive(true);

        //    //���̃J�������A�N�e�B�u�ɁB�g��Ȃ��J�����̓R�����g�A�E�g���Ă�
        //    MainCamera.SetActive(false);
        //    Camera2.SetActive(false);
        //    Camera4.SetActive(false);
        //    Camera5.SetActive(false);
        //}
        ////�y�J����3���g��Ȃ��ꍇ�R�����g�A�E�g�����܂Łz


        ////�y�J����4���g��Ȃ��ꍇ�R�����g�A�E�g��������z
        ////�L�[�{�[�h��4�L�[�������ꂽ��A�J����4���A�N�e�B�u��
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    //�J����4���A�N�e�B�u�ɐݒ�
        //    Camera4.SetActive(true);

        //    //���̃J�������A�N�e�B�u�ɁB�g��Ȃ��J�����̓R�����g�A�E�g���Ă�
        //    MainCamera.SetActive(false);
        //    Camera2.SetActive(false);
        //    Camera3.SetActive(false);
        //    Camera5.SetActive(false);
        //}
        ////�y�J����4���g��Ȃ��ꍇ�R�����g�A�E�g�����܂Łz


        ////�y�J����5���g��Ȃ��ꍇ�R�����g�A�E�g��������z
        ////�L�[�{�[�h��5�L�[�������ꂽ��A�J����5���A�N�e�B�u��
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    //�J����5���A�N�e�B�u�ɐݒ�
        //    Camera5.SetActive(true);

        //    //���̃J�������A�N�e�B�u�ɁB�g��Ȃ��J�����̓R�����g�A�E�g���Ă�
        //    MainCamera.SetActive(false);
        //    Camera2.SetActive(false);
        //    Camera3.SetActive(false);
        //    Camera4.SetActive(false);
        //}
        ////�y�J����5���g��Ȃ��ꍇ�R�����g�A�E�g�����܂Łz

    }
}