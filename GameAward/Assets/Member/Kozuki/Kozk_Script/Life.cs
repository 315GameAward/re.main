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
        if(nLife <= 0)
        {
            nLife = 0;
            return;
        }

        Destroy(Lifes[0]);
        Lifes.RemoveAt(0);

        //����炷
        audioSource.PlayOneShot(sound);

        nLife--;
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
