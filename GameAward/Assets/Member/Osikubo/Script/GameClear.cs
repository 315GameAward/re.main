using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour
{
    Animation anim;
    public Animator animator;

    GameObject slideObject;     // SlideSheet.csがアタッチされたオブジェクト取得用
    SlideSheet slide;           // SlideSheet.csクラス取得

    // Start is called before the first frame update
    void Start()
    {
        anim = this.gameObject.GetComponent<Animation>();
        slideObject = GameObject.Find("Result");
        slide = slideObject.GetComponent<SlideSheet>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slide.Landing)
        {
            animator.SetBool("Start", true);
        }
    }
}