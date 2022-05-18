using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour
{
    Animation anim;
    Animator animator;
    private GameObject[] ResultTest;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.gameObject.GetComponent<Animation>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ResultTest = GameObject.FindGameObjectsWithTag("kesigomu");
        // Debug.Log("è¡ÇµÉSÉÄÇÃêî" + ResultTest.Length);
        if (ResultTest.Length == 0)
        {
            animator.SetBool("Start", true);
        }
        Debug.Log("è¡ÇµÉSÉÄÇÃêî" + ResultTest.Length);
    }
}
