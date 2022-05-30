using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParperSlide : MonoBehaviour
{
    GameObject ClearObj;
    Animator gameClearAnim;
    Animator ClearPaper;
    bool endAnim;
    public bool EndAnim { get { return endAnim; } }
    // Start is called before the first frame update
    void Start()
    {
        endAnim = false;
        ClearPaper = this.gameObject.GetComponent<Animator>();
        ClearObj = GameObject.Find("GameClear");
        gameClearAnim = ClearObj.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!(ClearObj.activeSelf || gameObject.activeSelf))
            return;
        if(gameClearAnim.GetBool("End"))
        {
            ClearPaper.SetBool("Start2", true);
            if (ClearPaper.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && ClearPaper.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.5f)
            {
                ClearPaper.SetBool("End", true);
                ClearObj.SetActive(false);
            }
        }
        endAnim = ClearPaper.GetBool("End");

    }
}
