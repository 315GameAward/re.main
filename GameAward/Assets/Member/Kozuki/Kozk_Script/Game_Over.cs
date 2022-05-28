using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        OVER_SELECT,    // �Z���N�g���
        OVER_END        // �I��
    };

    public GameObject Image_gameOver;
    public Sprite cursor;
    public Image img_rtry;
    public Image img_slct;
    public Image img_end;
    AudioSource audioSource;
    public AudioClip sound_Rot; // �ړ�_���ʉ�
    public AudioClip sound_Chs; // �I��_���ʉ�
    public AudioClip sound_Gin; // �W���O����

    private float fAlpha = 0.0f;

    private STATE_GMOV OVERstate;
    public bool b_gmov = false;    // true�Ȃ�Ăяo��
    private bool b_gingle = false;

    // Start is called before the first frame update
    void Start()
    {
        // ��������
        audioSource = GetComponent<AudioSource>();

        // �����͉����I�����Ȃ�
        OVERstate = STATE_GMOV.OVER_RETRY;
        img_rtry = GameObject.Find("cursor_1").GetComponent<Image>();
        img_slct = GameObject.Find("cursor_2").GetComponent<Image>();
        img_end = GameObject.Find("cursor_3").GetComponent<Image>();
        img_slct.enabled = false;
        img_rtry.enabled = false;
        img_end.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // �I�[�o�[���Ăяo���ꂽ�����
        if (b_gmov == true)
        {
            // �W���O���Đ�
            if(b_gingle == true) { audioSource.PlayOneShot(sound_Gin); b_gingle = false; }

            // ���͎擾
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                //����炷
                audioSource.PlayOneShot(sound_Rot);
                OVERstate++;
                if (OVERstate > STATE_GMOV.OVER_END)
                {
                    OVERstate = STATE_GMOV.OVER_RETRY;
                }

            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                //����炷
                audioSource.PlayOneShot(sound_Rot);
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
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, fAlpha);

            // ��Ԃɂ���ĕω�
            switch (OVERstate)
            {
                case STATE_GMOV.OVER_NONE:  // �����Ȃ�

                    img_rtry.enabled = true;
                    img_slct.enabled = false;
                    img_end.enabled = false;

                    break;
                case STATE_GMOV.OVER_RETRY: // ���g���C�I��
                    img_rtry.enabled = true;
                    img_slct.enabled = false;
                    img_end.enabled = false;

                    // ���݂̃V�[�����Đ�
                    if (Input.GetKeyUp(KeyCode.Return))
                    {
                        //����炷
                        audioSource.PlayOneShot(sound_Chs);

                        // �V�[���J��
                        SceneManager.LoadScene("GameScene");    // ��
                    }
                    break;
                case STATE_GMOV.OVER_SELECT: // ���g���C�I��
                    img_slct.enabled = true;
                    img_rtry.enabled = false;
                    img_end.enabled = false;

                    // ���݂̃V�[�����Đ�
                    if (Input.GetKeyUp(KeyCode.Return))
                    {
                        //����炷
                        audioSource.PlayOneShot(sound_Chs);

                        // �V�[���J��
                        SceneManager.LoadScene("AreaSelect");    // ��
                    }
                    break;
                //case STATE_GMOV.OVER_END:   // �Q�[���𔲂����I��
                //    img_slct.enabled = false;
                //    img_rtry.enabled = false;
                //    img_end.enabled = true;

                //    // �Q�[���I��
                //    if (Input.GetKeyUp(KeyCode.Return))
                //    {
                //        //����炷
                //        audioSource.PlayOneShot(sound_Chs);

                //        // �I��
                //        Application.Quit();
                //    }
                //    break;
            }
        }
        else if (b_gmov == false)   // �������ꂽ��
        {
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            img_slct.enabled = false;
            img_rtry.enabled = false;
           // img_end.enabled  = false;
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
        b_gingle = true;
    }
}
