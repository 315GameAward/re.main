using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum DestGame
{
    GameStart,      //�Q�[���X�^�[�g
    GameClear,
    GameOver        //�Q�[���I�[�o�[
}
public class IventGet_Dest : MonoBehaviour
{
    //�N���A�pUI
    [SerializeField]
    GameObject clearUI;

    public DestGame status_dest;
    public IventPush_Dest target;

    void OnDisable()
    {
        target.OnDestroyed.RemoveAllListeners();
    }
    void OnEnable()
    {
        target.OnDestroyed.AddListener(() => {
            Debug.Log("target���폜����܂���");
            // �����ɏ�����ǉ�
            status_dest = DestGame.GameClear;
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        status_dest = DestGame.GameStart;
    }

    // Update is called once per frame
    void Update()
    {
        if(status_dest == DestGame.GameClear)
        {
            clearUI.SetActive(true);
        }
    }
}
