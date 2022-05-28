using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParperSlide : MonoBehaviour
{
    public GameObject gameClear;
    public Animator ClearPaper;
    bool endAnim;
    public bool EndAnim { get { return endAnim; } }
    // Start is called before the first frame update
    void Start()
    {
        ClearPaper = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameClear.GetComponent<Animator>().GetBool("End"))
        {
            ClearPaper.SetBool("Start2", true);
            if (ClearPaper.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && ClearPaper.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.5f)
            {
                ClearPaper.SetBool("End", true);
            }
        }
        
        endAnim = ClearPaper.gameObject.GetComponent<Animator>().GetBool("End");

    }
}
