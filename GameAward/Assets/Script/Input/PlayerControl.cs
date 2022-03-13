//================================================
//
//      PlayerControl.cs
//      �n�T�~�̃R���g���[���[�ƃL�[�{�[�h����
//
//------------------------------------------------
//      �쐬��: ���˗I��
//================================================

//================================================
// �J������
// 2022/03/01 �쐬�J�n
// �ҏW��: ���˗I��
//================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{

    [SerializeField] private float _moveForce = 5;  //�����ړ���

    private Rigidbody _rigidbody;           //Rigidbody
    private ControlBinds _gameInputs;        //�C���v�b�g
    private Vector2 _moveStickValue;        //�X�e�B�b�N�ړ���

    public float motorDelay = 0.1f;               //�p�b�h�U���̃f�B���C
    public bool m_bPlayerMove = false;       //�ړ����Ă��邩

    private void Awake()
    {
        //Rigidbody�擾
        _rigidbody = GetComponent<Rigidbody>();



        //InputAction�C���X�^���X����
        _gameInputs = new ControlBinds();

        //Move�C�x���g�o�^
        _gameInputs.Player.Move.started += OnMove;
        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnMove;

        //Cut�C�x���g�o�^
        _gameInputs.Player.Cut.started += OnCut;
        _gameInputs.Player.Cut.canceled += OnCutOff;

        //SmoothCut�C�x���g�o�^
        _gameInputs.Player.SmoothCut.started += OnSmoothCut;
        _gameInputs.Player.SmoothCut.performed += OnSmoothCut;
        _gameInputs.Player.SmoothCut.canceled += OnSmoothCutOff;

        //InputAction�L����
        _gameInputs.Enable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        //Move�A�N�V�����̓��͎擾
        _moveStickValue = context.ReadValue<Vector2>();

        //Debug.Log(_moveStickValue.y);
    }

    private void OnCut(InputAction.CallbackContext context)
    {
        //�؂������̈ړ�
        transform.position += transform.forward * .2f;

        //�p�b�h�̐U���ݒ�
        Gamepad.current.SetMotorSpeeds(1.0f, 0.5f);

        //�f�B���C�̃R���[�`�����s
        StartCoroutine(DelayMethod(motorDelay, () =>
        {
            Gamepad.current.SetMotorSpeeds(0.0f, 0.0f);
        }));
        m_bPlayerMove = true;

        //WaitForSecondsRealtime(3.0f);
    }

    private void OnCutOff(InputAction.CallbackContext context)
    {
        m_bPlayerMove = false;
        Debug.Log("CutOff");
    }

    private void OnSmoothCut(InputAction.CallbackContext context)
    {
        transform.position += transform.forward * 1.0f;
        m_bPlayerMove = true;
    }
    private void OnSmoothCutOff(InputAction.CallbackContext context)
    {
        m_bPlayerMove = false;
    }


    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0.0f, _moveStickValue.x * _moveForce, 0.0f));
        /*
        // �ړ������̗͂�^����
        _rigidbody.AddForce(new Vector3(
            _moveInputValue.x,
            0,
            _moveInputValue.y
        ) * _moveForce);
        */
    }

    //�f�B���C�����R���[�`��!
    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
    
}
