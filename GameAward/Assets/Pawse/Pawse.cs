using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�v���O��������
//�ʃX�N���v�g�ŃI�u�W�F�N�g���폜���ꂽ���Ƃ��Ď�����

public class Pawse : MonoBehaviour
{
    public RandomMovement target;

    void OnDisable()
    {
        target.OnDestroyed.RemoveAllListeners();
    }

    void OnEnable()
    {
        //���J�b�R���ɃI�u�W�F�N�g�����������̏�����������
        target.OnDestroyed.AddListener
        (() =>
        {
           
        });
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //
    //}
    //
    //// Update is called once per frame
    //void Update()
    //{
    //   
    //}





}
