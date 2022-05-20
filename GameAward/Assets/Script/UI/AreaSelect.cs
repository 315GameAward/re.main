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
// 2022/05/19 �쐬�J�n2
// �ҏW��: ���˗I��
//
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
    private int areaSelect;                         //����I�����Ă��邩

    [SerializeField] private GameObject areas;

    public float rotateSpeed = 1.0f;                //��]�X�s�[�h

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
        areaSelect = 0;
    }

    private void Update()
    {
        Debug.Log(areaSelect);
        //Debug.Log(areas.transform.localEulerAngles.y);
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
            if (_moveStickValue.x == 1.0f)  //right arrow
            {
                if (areaSelect == 0)
                {
                    StartCoroutine("ChangeAreaR");
                }
                else if (areaSelect == 1)
                {
                    StartCoroutine("ChangeAreaR");
                }
                else if (areaSelect == 2)
                {
                    StartCoroutine("ChangeAreaR");
                }
                else if (areaSelect == 3)
                {
                    StartCoroutine("ChangeAreaR");
                }
            }

            if (_moveStickValue.x == -1.0f)  //left arrow
            {
                if (areaSelect == 0)
                {
                    StartCoroutine("ChangeAreaL");
                }
                else if (areaSelect == 1)
                {
                    StartCoroutine("ChangeAreaL");
                }
                else if (areaSelect == 2)
                {
                    StartCoroutine("ChangeAreaL");
                }
                else if (areaSelect == 3)
                {
                    StartCoroutine("ChangeAreaL");
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
            if (areaSelect == 0)
            {

            }
            //���Ȏ��X�e�[�W
            else if (areaSelect == 1)
            {

            }
            //�̈�كX�e�[�W
            else if (areaSelect == 2)
            {

            }
            //�^�C�g����
            else if (areaSelect == 3)
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

    IEnumerator ChangeAreaR()
    {
        if (areaSelect == 0)
        {
            areas.transform.eulerAngles = new Vector3(0, 0.0f, 0);
            while (areas.transform.eulerAngles.y < 90.0f)
            {
                //areas.transform.Rotate(Vector3.up * (90.0f * Time.deltaTime));
                areas.transform.Rotate(Vector3.up * rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            areaSelect = 1;
        }
        else if (areaSelect == 1)
        {
            areas.transform.eulerAngles = new Vector3(0, 90.0f, 0);
            while (areas.transform.eulerAngles.y < 180.0f)
            {
                areas.transform.Rotate(Vector3.up * rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            areaSelect = 2;
        }
        else if (areaSelect == 2)
        {
            areas.transform.eulerAngles = new Vector3(0, 180.0f, 0);
            while (areas.transform.eulerAngles.y < 270.0f)
            {
                areas.transform.Rotate(Vector3.up * rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            areaSelect = 3;
        }
        else if (areaSelect == 3)
        {
            areas.transform.eulerAngles = new Vector3(0, 270.0f, 0);
            while (areas.transform.eulerAngles.y > 0.0f)
            {
                areas.transform.Rotate(Vector3.up * rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            areaSelect = 0;
        }
    }

    IEnumerator ChangeAreaL()
    {
        if (areaSelect == 0)
        {
            areas.transform.eulerAngles = new Vector3(0, -0.1f, 0);
            while (areas.transform.eulerAngles.y > 270.0f)
            {
                areas.transform.Rotate(Vector3.up * -rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            areaSelect = 1;
        }
        else if (areaSelect == 1)
        {
            while (areas.transform.eulerAngles.y > 180.0f)
            {
                areas.transform.Rotate(Vector3.up * -rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            areaSelect = 2;
        }
        else if (areaSelect == 2)
        {
            while (areas.transform.eulerAngles.y > 90.0f)
            {
                areas.transform.Rotate(Vector3.up * -rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            areaSelect = 3;
        }
        else if (areaSelect == 3)
        {
            while (areas.transform.eulerAngles.y < 359.0f)
            {
                areas.transform.Rotate(Vector3.up * -rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            areaSelect = 0;
        }
    }
}
