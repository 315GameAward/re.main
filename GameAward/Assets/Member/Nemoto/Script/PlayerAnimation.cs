using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public bool anime;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {
        if(anime)
        {
            animator.SetBool("Cut1", true);
        }
        else
        {
            animator.SetBool("Cut1", false);
        }
    }
}
