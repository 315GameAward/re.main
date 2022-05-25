//================================================
//
//      AppearanceEvaluation.cs
//      ���ʔ��\
//
//------------------------------------------------
//      �쐬��: �ĎR꣑��Y
//      �쐬�J�n���F2022/5/20(��)
//================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class AppearanceEvaluation : MonoBehaviour
{
    // �]��
    public enum EvaType
    {
        Too_Bad,        // ����΂�܂��傤
        Good,           // �悭�ł��܂���
        Great,          // �����ւ�悭�ł��܂���

        Max_Type
    }

    public GameObject SelectButton;
    public const int Max_Obj = 2;  // �]�����ڐ�
    
    public float stopTime;  // �X�^���v�����Ԋu

    static bool[] activeObj = new bool[2];  // �I�u�W�F�N�g���A�N�e�B�u�ɐݒ�
    
    // �]���e�N�X�`����K�p������I�u�W�F�N�g
    public GameObject[] item = new GameObject[Max_Obj];

    // �ǃf�J�X�^���v
    GameObject last_result;
    
    // �]���Ɏg���X�^���v�e�N�X�`��
    public Sprite[] evaluation = new Sprite[(int)EvaType.Max_Type];

    private void Awake()
    {
        SelectButton.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        last_result = GameObject.Find("last_result");
        last_result.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        GameObject.Find("layer").gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        // �]�����ڂ��̓_
        StampEvaluation();
    }

    // Update is called once per frame
    void Update()
    {
        // �~�肫���Ă��Ȃ��Ȃ珈�����Ȃ�
        if (!SlideSheet.Landing)
            return;
        
        StartCoroutine("Defeat");
    }

    // ���̕]������
    void StampEvaluation()
    {
        for (int i = 0; i < Max_Obj; ++i)
        {
            switch (i)
            {
                case 0:
                    item[i].GetComponent<Image>().sprite = evaluation[0];
                    item[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    break;
                case 1:
                    item[i].GetComponent<Image>().sprite = evaluation[1];
                    item[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                    break;
                case 2:
                    item[i].GetComponent<Image>().sprite = evaluation[2];
                    item[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                    break;
                default:
                    break;
            }
        }
    }

    // ���Ԋu�����U���g���ڕ\��
    IEnumerator Defeat()
    {
        for (int i = 0; i < Max_Obj; ++i)
        {
            // �X�^���v����������
            yield return new WaitForSeconds(stopTime);
            item[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        yield return new WaitForSeconds(stopTime);
        last_result.GetComponent<Image>().color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        //activeObj[0] = true;
        yield return new WaitForSeconds(stopTime);
        GameObject.Find("layer").gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        SelectButton.SetActive(true);
        //activeObj[1] = true;
        //SetActive.AcObj.SetActive(true);
    }

}
