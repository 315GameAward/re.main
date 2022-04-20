using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum DestGame_a
{
    GameStart,      //�Q�[���X�^�[�g
    GameClear,
    GameOver        //�Q�[���I�[�o�[
}
public class IventGet_Dest_a : MonoBehaviour
{
    //�N���A�pUI
    [SerializeField]
    GameObject clearUI;

    public DestGame_a status_dest;
    public IventPush_Dest_a target;

    void OnDisable()
    {
        target.OnDestroyed.RemoveAllListeners();
    }
    void OnEnable()
    {
        target.OnDestroyed.AddListener(() => {
            Debug.Log("target���폜����܂���");
            // �����ɏ�����ǉ�
            status_dest = DestGame_a.GameClear;
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        status_dest = DestGame_a.GameStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (status_dest == DestGame_a.GameClear�@|| Input.GetKeyUp(KeyCode.V))
        {
            clearUI.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            clearUI.SetActive(false);
        }
    }
}
