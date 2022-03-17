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
    public float m_fSpeed_trigger = 0.2f; // �i�ޑ��x
    public float m_fSpeed_hold    = 0.2f; // �i�ޑ��x
    public float m_fDelayTime   �@= 1.0f; // �X�[�Ɛ؂�Ƃ��̃f�B���C����
    public bool m_bMove = false;   // �����Ă邩�ǂ���(�����Ă���true)

    public KeyCode key  = (KeyCode)323;   // �؂�{�^��(�}�E�X���N���b�N)
    public KeyCode key1 = 0;
   GameObject poitObj;

    //--- Unity�Ɍ��J���Ȃ��p�����[�^
    private bool m_bDelay = false;  // true�̎��X�[�Ɛ؂�
    private bool m_bKeyPress = false;   // �L�[�������Ă邩�����ĂȂ���
    private float m_fDelay = 0.0f;  // �X�[�Ɛ؂�Ƃ��̃f�B���C���Ԃ̊�
    private Point m_point;  // �J�b�g�|�C���g

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
            transform.position += transform.forward * m_fSpeed_trigger;
            m_bMove = true;
        }

        // �؂��Đi�ޏ���(�X�[)
        if (Input.GetKey(KeyCode.Mouse0))
        {
            m_bKeyPress = true; // �L�[�������Ă邩�ǂ���

            // �f�B���C�J�n
            m_fDelay -= 1.0f / 60.0f;
            if(m_fDelay < 0.0f)
            {
                m_fDelay = 0.0f;    // �f�B���C���ԃX�g�b�v
                m_bDelay = true;    // �f�B���C�t���OON
            }

            // �f�B���C�t���OON�̎�
            if (m_bDelay)
            {
                // �O�ɐi�ޏ���
                transform.position += transform.forward * m_fSpeed_hold;
            }
           
        }
        else
        {
            // �ϐ����Z�b�g
            m_fDelay = m_fDelayTime;    // �f�B���C����
            m_bDelay = false;   // �f�B���C�t���O 
            m_bMove  = false;   // ���[�u�t���O
            m_bKeyPress = false;    // �L�[�t���O
        }

        // �����ύX
        if(Input.GetKey(KeyCode.A) && !m_bDelay) // ��
        {
            transform.Rotate(new Vector3(0.0f,-0.3f,0.0f));
        }
        if (Input.GetKey(KeyCode.D) && !m_bDelay) // �E
        {
            transform.Rotate(new Vector3(0.0f, 0.3f, 0.0f));
        }

        
    }

    // �n�ʂɓ����Ă��鎞
    void OnTriggerStay(Collider other)
    {
        if (m_bMove)
        {
            //GetComponent<point>().AddPoitn(gameObject.transform.position);
           // Instantiate(poitObj, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            m_point.AddPoint(gameObject.transform.position);
            m_bMove = false;
        }
    }

}
