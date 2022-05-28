using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour
{
    Animation anim;
    public Animator animator;

    GameObject slideObject;     // SlideSheet.cs���A�^�b�`���ꂽ�I�u�W�F�N�g�擾�p
    SlideSheet slide;           // SlideSheet.cs�N���X�擾

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