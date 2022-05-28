using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParperSlide : MonoBehaviour
{
    Animation anim;
    public GameObject gameClear;
     Animator animator;

    GameObject slideObject;     // SlideSheet.cs���A�^�b�`���ꂽ�I�u�W�F�N�g�擾�p
    SlideSheet slide;           // SlideSheet.cs�N���X�擾
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
