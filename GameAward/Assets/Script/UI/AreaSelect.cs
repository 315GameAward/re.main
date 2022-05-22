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
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AreaSelect : MonoBehaviour
{
    private ControlBinds _gameInputs;               //�C���v�b�g
    private Vector2 _moveStickValue;                //�X�e�B�b�N�ړ���
    private int areaSelect;                         //����I�����Ă��邩
    private bool areaRotating;                      //��]��
    
    [SerializeField] private GameObject areas;
    public float rotateSpeed = 1.5f;                //��]�X�s�[�h
    
    [SerializeField] private Text areaText;         //�I�𒆃G���A�\���p�e�L�X�g
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private AudioClip rotate;
    [SerializeField] private AudioClip select;

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
        areaRotating = false;
        areaText.text = "�����X�e�[�W";
    }

    private void Update()
    {
        Debug.Log(areaSelect);
        //Debug.Log(areas.transform.localEulerAngles.y);
        //areas.transform.Rotate(Vector3.up * (90.0f * Time.deltaTime));

        //if (areaSelect == 0)
        //{
            
        //}
        //else if (areaSelect == 1)
        //{
            
        //}
        //else if (areaSelect == 2)
        //{
            
        //}
        //else if (areaSelect == 3)
        //{
            
        //}
    }

    private void OnMoveSelect(InputAction.CallbackContext context)
    {
        //Move�A�N�V�����̓��͎擾
        _moveStickValue = context.ReadValue<Vector2>();
        //���݂̃V�[�����擾
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "AreaSelect")     //�X�e�[�W�Z���N�g�V�[�����̏ꍇ
        {
            if (_moveStickValue.x >= 0.1f)  //right arrow
            {
                if (!areaRotating)
                {
                    if (areaSelect == 0)
                    {
                        areaRotating = true;
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
            }

            if (_moveStickValue.x <= -0.1f)  //left arrow
            {
                if (!areaRotating)
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
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        //���݂̃V�[�����擾
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "AreaSelect")     //�X�e�[�W�Z���N�g�V�[�����̏ꍇ
        {
            audioSource.PlayOneShot(select);
            //�����X�e�[�W
            if (areaSelect == 0)
            {
                Debug.Log("�����X�e�[�W�I��");
                SceneManager.LoadScene("GameScene");
            }
            //���y���X�e�[�W
            else if (areaSelect == 1)
            {
                Debug.Log("���y���X�e�[�W�I��");
            }
            //�̈�كX�e�[�W
            else if (areaSelect == 2)
            {
                Debug.Log("�}�H���X�e�[�W�I��");
            }
            //���Ȏ���
            else if (areaSelect == 3)
            {
                Debug.Log("���Ȏ��X�e�[�W�I��");
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
        areaRotating = true;
        audioSource.PlayOneShot(rotate);
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
            areaText.text = "���y���X�e�[�W";
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
            areaText.text = "�}�H���X�e�[�W";
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
            areaText.text = "���Ȏ��X�e�[�W";
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
            areaText.text = "�����X�e�[�W";
        }

        areaRotating = false;
    }

    IEnumerator ChangeAreaL()
    {
        areaRotating = true;
        audioSource.PlayOneShot(rotate);
        if (areaSelect == 0)
        {
            areas.transform.eulerAngles = new Vector3(0, -0.1f, 0);
            while (areas.transform.eulerAngles.y > 270.0f)
            {
                areas.transform.Rotate(Vector3.up * -rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            areaSelect = 3;
            areaText.text = "���Ȏ��X�e�[�W";
        }
        else if (areaSelect == 3)
        {
            areas.transform.eulerAngles = new Vector3(0, 270.0f, 0);
            while (areas.transform.eulerAngles.y > 180.0f)
            {
                areas.transform.Rotate(Vector3.up * -rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            areaSelect = 2;
            areaText.text = "�}�H���X�e�[�W";
        }
        else if (areaSelect == 2)
        {
            areas.transform.eulerAngles = new Vector3(0, 180.0f, 0);
            while (areas.transform.eulerAngles.y > 90.0f)
            {
                areas.transform.Rotate(Vector3.up * -rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            areaSelect = 1;
            areaText.text = "���y���X�e�[�W";
        }
        else if (areaSelect == 1)
        {
            areas.transform.eulerAngles = new Vector3(0, 90.0f, 0);
            while (areas.transform.eulerAngles.y < 359.0f)
            {
                areas.transform.Rotate(Vector3.up * -rotateSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            areaSelect = 0;
            areaText.text = "�����X�e�[�W";
        }

        areaRotating = false;
    }
}
