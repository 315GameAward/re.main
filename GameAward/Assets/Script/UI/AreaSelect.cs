//================================================
//
//      AreaSelect.cs
//      �G���A�̑I��
//
//------------------------------------------------
//      �쐬��: ���˗I��
//================================================

//================================================
// �J������
// 2022/04/04 �쐬�J�n
// �ҏW��: ���˗I��
//================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AreaSelect : MonoBehaviour
{
    private ControlBinds _gameInputs;               //�C���v�b�g
    private Vector2 _moveStickValue;                //�X�e�B�b�N�ړ���
    private int stageSelect;                        //����I�����Ă��邩



    private void Awake()
    {
        //InputAction�C���X�^���X����
        _gameInputs = new ControlBinds();

        //���ڈړ��C�x���g�o�^
        _gameInputs.Player.MoveSelect.performed += OnMoveSelect;

        //Select�C�x���g�o�^
        _gameInputs.Player.Select.started += OnSelect;

        //Pause�C�x���g�o�^(Back)
        _gameInputs.Player.Pause.started += OnPause;

        //InputAction�L����
        _gameInputs.Enable();
    }

    private void Start()
    {
        stageSelect = 0;
    }

    private void Update()
    {
        GameObject areas = GameObject.Find("Areas");
        //areas.transform.Rotate(Vector3.up * (90.0f * Time.deltaTime));
    }

    private void OnMoveSelect(InputAction.CallbackContext context)
    {
        //Move�A�N�V�����̓��͎擾
        _moveStickValue = context.ReadValue<Vector2>();
        //���݂̃V�[�����擾
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "AreaSelect")     //�X�e�[�W�Z���N�g�V�[�����̏ꍇ
        {
            GameObject areas = GameObject.Find("Areas");
            if (_moveStickValue.x > 0.0f)  //��
            {
                if (stageSelect == 0)
                {
                    stageSelect = 1;
                    Debug.Log("1");
                    while (areas.transform.rotation.y < 90.0f)
                    {
                        areas.transform.Rotate(Vector3.up * (90.0f * Time.deltaTime));
                    }
                }
                else if (stageSelect == 1)
                {
                    stageSelect = 2;
                    Debug.Log("2");
                    areas.transform.Rotate(Vector3.up * (90.0f * Time.deltaTime));
                }
                else if (stageSelect == 2)
                {
                    stageSelect = 3;
                    Debug.Log("3");
                    areas.transform.Rotate(Vector3.up * (90.0f * Time.deltaTime));
                }
            }

            if (_moveStickValue.x < -0.0f)  //��
            {
                if (stageSelect == 1)
                {
                    stageSelect = 0;
                    Debug.Log("StageSelect: 0");
                    areas.transform.rotation = Quaternion.Euler(0, -90, 0);
                }
                else if (stageSelect == 2)
                {
                    stageSelect = 1;
                    Debug.Log("StageSelect: 1");
                    areas.transform.rotation = Quaternion.Euler(0, -180, 0);
                }
                else if (stageSelect == 3)
                {
                    stageSelect = 2;
                    Debug.Log("StageSelect: 2");
                    areas.transform.rotation = Quaternion.Euler(0, -270, 0);
                }
            }
        }
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        //���݂̃V�[�����擾
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "StageSelect")     //�X�e�[�W�Z���N�g�V�[�����̏ꍇ
        {
            //�����X�e�[�W
            if (stageSelect == 0)
            {

            }
            //���Ȏ��X�e�[�W
            else if (stageSelect == 1)
            {

            }
            //�̈�كX�e�[�W
            else if (stageSelect == 2)
            {

            }
            //�^�C�g����
            else if (stageSelect == 3)
            {

            }
        }
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        //���݂̃V�[�����擾
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "StageSelect")     //�X�e�[�W�Z���N�g�V�[�����̏ꍇ
        {
            //
        }
    }


}
