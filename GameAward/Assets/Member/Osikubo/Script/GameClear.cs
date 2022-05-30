using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour
{
    public Animator animator;

    GameObject slideObject;     // SlideSheet.csがアタッチされたオブジェクト取得用
    SlideSheet slide;           // SlideSheet.csクラス取得

    void Awake()
    {
        // 生存時間をリアルタイム設定
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        slideObject = GameObject.Find("Result");
        slide = slideObject.GetComponent<SlideSheet>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("slide.Landing:" + slide.Landing);
        if (slide.Landing)
        {
            animator.SetBool("Start", true);
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.5f)
            {
                animator.SetBool("End", true);
            }
        }
    }
}