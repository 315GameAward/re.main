using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// ==========================================
//
//              �Q�[���I�[�o�[
//
// ==========================================
// �㌎��n

public class Game_Over : MonoBehaviour
{
   private enum STATE_GMOV
    {
        OVER_NONE = 0,  // �I�𖳂�
        OVER_RETRY,     // ���g���C
        OVER_END        // �I��
    };

    public GameObject Image_gameOver;
    public Sprite cursor;
    public Image img_rtry;
    public Image img_end;
    private float fAlpha = 0.0f;

    private STATE_GMOV OVERstate;
    public bool b_gmov = false;    // true�Ȃ�Ăяo��


    // Start is called before the first frame update
    void Start()
    {
        // �����͉����I�����Ȃ�
        OVERstate = STATE_GMOV.OVER_RETRY;
        img_rtry = GameObject.Find("cursor_1").GetComponent<Image>();
        img_end = GameObject.Find("cursor_2").GetComponent<Image>();
        img_end.enabled = false;
        img_rtry.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // �I�[�o�[���Ăяo���ꂽ�����
        if(b_gmov == true)
        {           
            // ���͎擾
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                OVERstate++;
                if(OVERstate > STATE_GMOV.OVER_END)
                {
                    OVERstate = STATE_GMOV.OVER_RETRY;
                }
            }
            else if(Input.GetKeyUp(KeyCode.UpArrow))
            {
                OVERstate--;
                if (OVERstate <= STATE_GMOV.OVER_NONE)
                {
                    OVERstate = STATE_GMOV.OVER_END;
                }
            }
            // �G�X�P�[�v�������ꂽ��
            //if (Input.GetKeyUp(KeyCode.Escape))
            //{
            //    b_gmov = false;
            //}

            // UI�Ăяo��
            fAlpha += 0.01f;
            //if (fAlpha > 80.0f / 255.0f) fAlpha = 80.0f / 255.0f; // �����x�̐���
            if (fAlpha > 210.0f / 255.0f) fAlpha = 210.0f / 255.0f; // �����x�̐���
            gameObject.GetComponent< Image >().color = new Color(1, 1, 1, fAlpha);

            // ��Ԃɂ���ĕω�
            switch (OVERstate)
            {
                case STATE_GMOV.OVER_NONE:  // �����Ȃ�

                    img_rtry.enabled = true;
                    img_end.enabled = false;

                    break;
                case STATE_GMOV.OVER_RETRY: // ���g���C�I��
                    img_rtry.enabled = true;
                    img_end.enabled = false;
                    // ���݂̃V�[�����Đ�
                    if (Input.GetKeyUp(KeyCode.Return))
                    {
                        // �V�[���J��
                    }
                    break;
                case STATE_GMOV.OVER_END:   // �Q�[���𔲂����I��
                    img_end.enabled = true;
                    img_rtry.enabled = false;

                    // �Q�[���I��
                    if(Input.GetKeyUp(KeyCode.Return))
                    {
                        Application.Quit();
                    }                 
                    break;
            }           
        }
        else if (b_gmov == false)   // �������ꂽ��
        {
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            img_end.enabled = false;
            img_rtry.enabled = false;
        }
        // Debug.Log(OVERstate);
    }

    // =====================
    // �Q�[���I�[�o�[�Z�b�g
    // =====================
    public void SetGMOV(bool gmov)
    {
        // bool�擾
        b_gmov = gmov;
    }
}
