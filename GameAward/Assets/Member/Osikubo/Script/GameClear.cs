using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour
{
    Animation anim;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.gameObject.GetComponent<Animation>();

    }

    // Update is called once per frame
    void Update()
    {
        if (SlideSheet.Landing)
        {
            animator.SetBool("Start", true);
        }
    }
}