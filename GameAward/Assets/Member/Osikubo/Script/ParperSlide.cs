using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParperSlide : MonoBehaviour
{
    Animation anim;
    public GameObject gameClear;
     Animator animator;

    GameObject slideObject;     // SlideSheet.csがアタッチされたオブジェクト取得用
    SlideSheet slide;           // SlideSheet.csクラス取得
    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        slideObject = GameObject.Find("Result");
        slide = slideObject.GetComponent<SlideSheet>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameClear.GetComponent<Animator>().GetBool("End"))
        {
            animator.SetBool("Start2", true);
        }
    }
}
