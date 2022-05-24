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
        Too_Bad,
        Good,
        Great,

        Max_Type
    }

    // �]�����ڐ�
    const int Max_Obj = 2;
    // �X�^���v�����Ԋu
    public float stopTime;
    // �]���e�N�X�`����K�p������I�u�W�F�N�g
    public GameObject[] item = new GameObject[2];
    // �]���Ɏg���X�^���v�e�N�X�`��
    public Sprite[] evaluation = new Sprite[(int)EvaType.Max_Type];
    
    
    // Start is called before the first frame update
    void Start()
    {
        StampEvaluation();
    }

    // Update is called once per frame
    void Update()
    {
        if (!SlideSheet.Landing)
            return;
        StartCoroutine("Defeat");

    }

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

    IEnumerator Defeat()
    {
        for (int i = 0; i < Max_Obj; ++i)
        {
            yield return new WaitForSeconds(stopTime);
            item[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}


