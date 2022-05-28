//========================
// 
//      Life
// 		�v���C���[�̗�
//
//--------------------------------------------
// �쐬�ҁF�㌎��n
//========================
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Life : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;  // �C���X�y�N�^�[����n�[�g�̃v���n�u�����蓖�Ă�

    // ���ʉ�
    public AudioClip sound;
    AudioSource audioSource;

    private int MaxLife;        // �ő�l
    public int nLife = 3;       // �̗�(�ϓ�����)
    public static int nlife;    // ���V�[���֎����Ă����̗͐�

    private bool b_Life = true; // �̗͂��c���Ă��邩 true:����

    bool bBreakAnim = false;    // �A�j���[�V�������Đ�����Ă��邩 true:�Đ���
    Animator animator;
    float animTime = 0.0f;  // �A�j���[�V�����̍Đ�����

    public static Life instance;
    public GameObject image_gameOver;

    // �z��
    List<GameObject> Lifes = new List<GameObject>();   // �����������C�t������
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // ���ʉ��擾
        audioSource = GetComponent<AudioSource>();

        // �ő�l���
        MaxLife = nLife;

        // �̗͐ݒ�
        for (int i = nLife; i > 0; i--)
        {
            // �̗͂�ݒ肵�Ă���
            AddLife();
        }
        b_Life = true;

        // �A�j���[�^�[�ɑ��
        animator = Lifes[0].GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(nLife);

        if (nLife <= 0 && b_Life == true)
        {           
            // �Q�[���I�[�o�[�Ăяo��
            image_gameOver.GetComponent<Game_Over>().SetGMOV(true);
            b_Life = false; // ��x�����ʂ�Ȃ�
        }

       
        // �A�j���[�V�������I������Ƃ��̏���
        if(bBreakAnim)
        {
            
            // �A�j���[�V�������Đ�����Ă��Ȃ�������
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.5f)
            {
                // ���C�t�̍폜
                Destroy(Lifes[0]);
                Lifes.RemoveAt(0);                
               
                nLife--;

                // �A�j���[�^�[�ɑ��
                animator = Lifes[0].GetComponent<Animator>();
                Lifes[0].GetComponent<Animator>().SetBool("StartBreak", false);

                // �A�j���[�V�����Đ�OFF
                bBreakAnim = false;
            }
           
        }

#if DEBUG
        // ��������������̗͏���
        if (Input.GetKeyUp(KeyCode.PageUp))
        {
            AddLife();
            nLife++;
        }

        // �E������������̗͑���
        if (Input.GetKeyUp(KeyCode.PageDown))
        {
            DelLife();
        }
#endif


    }

    //========================
    //
    // �n�[�g��1���₷�֐�
    //
    //========================
    public void AddLife()
    {
        // ����2�͂ǂ̃I�u�W�F�N�g�̎q�ɂ��邩�ŁA
        // ����3�͎q�ɂ���ۂɈȑO�̈ʒu��ۂ�(LayoutGroup�n�ł�false�ɂ��Ȃ��Ƃ��������Ȃ�)
        GameObject instance = Instantiate(prefab, transform, false);
        Lifes.Add(instance);
        
    }

    //========================
    //
    // �n�[�g��1���炷�֐�
    //
    //========================
    public void DelLife()
    {
        if (bBreakAnim) return;

        if(nLife <= 0)
        {
            nLife = 0;
            return;
        }

        // �A�j���[�V�����Đ�
        bBreakAnim = true;
        Lifes[0].GetComponent<Animator>().SetBool("StartBreak",true);
       
        //����炷
        audioSource.PlayOneShot(sound);

        // �A�j���[�V�������ԑ��
        animTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    // �̗͐��擾�p�֐�
    public int GetLife()
    {
        nlife = nLife;
        return nlife;
    }

    // �ő吔�擾�p�֐�
    public int GetMaxLife()
    {       
        return MaxLife;
    }
}
