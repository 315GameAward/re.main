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
using UnityEngine.UI;   // UI��M��̂ɕK�v


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

    const int Max_Obj = 2;  // �]�����ڐ�
    
    public float stopTime;  // �X�^���v�����Ԋu
    
    // �]���e�N�X�`����K�p������I�u�W�F�N�g
    public GameObject[] item = new GameObject[Max_Obj];
    
    GameObject layer;   // �ǃf�J���X�^���v��������̃��U���g�ڂ���

    // �A�N�e�B�u��Ԃ��Ǘ�����I�u�W�F�N�g
    GameObject[] AcObj = new GameObject[Max_Obj];
    
    // �]���Ɏg���X�^���v�e�N�X�`��
    public Sprite[] evaluation = new Sprite[(int)EvaType.Max_Type];

    private void Awake()
    {
        //--- �q�I�u�W�F�N�g�擾����\��

        layer = GameObject.Find("layer");
        layer.gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        AcObj[0] = GameObject.Find("layer/last_result");
        AcObj[0].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        AcObj[1] = GameObject.Find("layer/SelectButton");

        for (var i = 0; i < Max_Obj; ++i)
        {
            // �I�u�W�F�N�g�̔�A�N�e�B�u��
            AcObj[i].SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // �]�����ڂ��̓_
        StampEvaluation();
    }

    // Update is called once per frame
    void Update()
    {
        // �~�肫���Ă��Ȃ��Ȃ珈�����Ȃ�
        if (!SlideSheet.Landing)    // ���̏������A�j���[�V�����𓮂����X�N���v�g�Ɏ����Ă����B
            return;                 // ����ɃA�j���[�V�������I���������̃t���O�������ŏ����B
        StartCoroutine("Defeat");
    }


    // ���̕]������
    void StampEvaluation()
    {
        item[0].GetComponent<Image>().sprite = evaluation[2];
        item[0].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        switch (1)
        {
            case 0:
                item[1].GetComponent<Image>().sprite = evaluation[0];
                item[1].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                break;
            case 1:
                item[1].GetComponent<Image>().sprite = evaluation[1];
                item[1].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                break;
            case 2:
                item[1].GetComponent<Image>().sprite = evaluation[2];
                item[1].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                break;
            default:
                break;
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

        // �h�f�J�X�^���v
        AcObj[0].SetActive(true);
        AcObj[0].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(stopTime);

        // �Z���N�g�{�^��
        AcObj[1].SetActive(true);

        // �ڂ����p���C���[
        layer.gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);   
    }
}
