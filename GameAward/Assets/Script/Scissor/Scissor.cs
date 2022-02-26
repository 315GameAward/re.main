//======================================================
//
//        Scissor.cs
//        �n�T�~�̓����̏���
//
//------------------------------------------------------
//      �쐬��:���{���V��
//======================================================

//======================================================
// �J������
// 2022/02/16 �v���g�^�C�v�쐬�J�n
// �ҏW��:���{���V��
//======================================================

//------ �C���N���[�h�� ------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Scissor : MonoBehaviour
{
    //--- Unity�Ɍ��J����p�����[�^
    public float g_fSpeed_trigger = 0.2f; // �i�ޑ��x
    public float g_fSpeed_hold    = 0.2f; // �i�ޑ��x
    public float g_fDelayTime   �@= 1.0f; // �X�[�Ɛ؂�Ƃ��̃f�B���C����
    public bool g_bMove = false;   // �����Ă邩�ǂ���(�����Ă���true)

    public KeyCode key  = (KeyCode)323;   // �؂�{�^��(�}�E�X���N���b�N)
    public KeyCode key1 = 0;
   GameObject poitObj;

    //--- Unity�Ɍ��J���Ȃ��p�����[�^
    private bool g_bDelay = false;  // true�̎��X�[�Ɛ؂�
    private bool g_bKeyPress = false;   // �L�[�������Ă邩�����ĂȂ���
    private float g_fDelay = 0.0f;  // �X�[�Ɛ؂�Ƃ��̃f�B���C���Ԃ̊�
    private Point g_point;  // �J�b�g�|�C���g

    // Start is called before the first frame update
    void Start()
    {
        poitObj = (GameObject)Resources.Load("point");
    }

    // Update is called once per frame
    void Update()
    {
        // �؂��Đi�ޏ���(�`���L�`���L)
        if (Input.GetKeyDown(KeyCode.Mouse0))   
        {
            // �O�ɐi�ޏ���
            transform.position += transform.forward * g_fSpeed_trigger;
            g_bMove = true;
        }

        // �؂��Đi�ޏ���(�X�[)
        if (Input.GetKey(KeyCode.Mouse0))
        {
            g_bKeyPress = true; // �L�[�������Ă邩�ǂ���

            // �f�B���C�J�n
            g_fDelay -= 1.0f / 60.0f;
            if(g_fDelay < 0.0f)
            {
                g_fDelay = 0.0f;    // �f�B���C���ԃX�g�b�v
                g_bDelay = true;    // �f�B���C�t���OON
            }

            // �f�B���C�t���OON�̎�
            if (g_bDelay)
            {
                // �O�ɐi�ޏ���
                transform.position += transform.forward * g_fSpeed_hold;
            }
           
        }
        else
        {
            // �ϐ����Z�b�g
            g_fDelay = g_fDelayTime;    // �f�B���C����
            g_bDelay = false;   // �f�B���C�t���O 
            g_bMove  = false;   // ���[�u�t���O
            g_bKeyPress = false;    // �L�[�t���O
        }

        // �����ύX
        if(Input.GetKey(KeyCode.A) && !g_bDelay) // ��
        {
            transform.Rotate(new Vector3(0.0f,-0.3f,0.0f));
        }
        if (Input.GetKey(KeyCode.D) && !g_bDelay) // �E
        {
            transform.Rotate(new Vector3(0.0f, 0.3f, 0.0f));
        }

        
    }

    // �n�ʂɓ����Ă��鎞
    void OnTriggerStay(Collider other)
    {
        if (g_bMove)
        {
            //GetComponent<point>().AddPoitn(gameObject.transform.position);
           Instantiate(poitObj, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
           // g_point.AddPoint(gameObject.transform.position);
            g_bMove = false;
        }
    }

}
